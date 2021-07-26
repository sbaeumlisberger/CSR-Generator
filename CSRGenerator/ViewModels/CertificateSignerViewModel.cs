using CSRGenerator.Models;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Pkcs;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSRGenerator.ViewModels
{
    public class CertificateSignerViewModel : ViewModelBase
    {
        public event AsyncEventHandler<FileOpenDialogModel>? OpenDialogRequested;

        public event EventHandler<(string Name, string Certificate)>? SaveCertificateRequested;

        [Reactive] public string CSR { get; set; } = "";

        public IReadOnlyList<SignatureAlgorithm> AvailableSignatureAlgorithms { get; } = SignatureAlgorithm.Values;

        [Reactive] public SignatureAlgorithm SelectedSignatureAlgorithm { get; set; } = SignatureAlgorithm.SHA256withRSA;

        [Reactive] public bool GenerateSignatureKey { get; set; } = true;

        [Reactive] public string SignatureKey { get; set; } = "";

        [Reactive] public bool CanSign { get; set; } = false;

        [Reactive] public bool IsCertificateSigned { get; set; } = false;

        [Reactive] public string SignedCertificate { get; set; } = "";

        private readonly EncodingService encodingService = new EncodingService();

        private readonly KeyGeneratorService keyGeneratorService = new KeyGeneratorService();

        private readonly CertificateSigningService certificateSigningService = new CertificateSigningService();

        public CertificateSignerViewModel()
        {
            this.WhenAnyValue(x => x.CSR, x => x.GenerateSignatureKey, x => x.SignatureKey)
                .Subscribe(x => CanSign = CSR != "" && (GenerateSignatureKey || SignatureKey != ""));

            this.WhenAnyValue(x => x.GenerateSignatureKey).Subscribe(x =>
            {
                if (GenerateSignatureKey is true)
                {
                    SignatureKey = "";
                }
            });
        }

        public async void OpenCSR()
        {
            var fileOpenDialogModel = new FileOpenDialogModel();
            fileOpenDialogModel.FilterLabel = "CSR files";
            fileOpenDialogModel.FileExtensionsFilter = new List<string> { "csr", "pem" };
            await OpenDialogRequested!.Invoke(this, fileOpenDialogModel);
            if (fileOpenDialogModel.SelectedFile is string filePath)
            {
                CSR = File.ReadAllText(filePath);
            }
        }

        public async void OpenSignatureKey()
        {
            var fileOpenDialogModel = new FileOpenDialogModel();
            fileOpenDialogModel.FilterLabel = "Private key files";
            fileOpenDialogModel.FileExtensionsFilter = new List<string> { "key", "pem" };
            await OpenDialogRequested!.Invoke(this, fileOpenDialogModel);
            if (fileOpenDialogModel.SelectedFile is string filePath)
            {
                SignatureKey = File.ReadAllText(filePath);
            }
        }

        public void SignCertificate()
        {
            try
            {
                var pkcs10CertificationRequest = (Pkcs10CertificationRequest)encodingService.ParsePEM(CSR);

                AsymmetricKeyParameter signatureKey;

                if (GenerateSignatureKey)
                {
                    object keyParameters = SelectedSignatureAlgorithm.KeyAlgorithm switch
                    {
                        KeyAlgorithm.RSA => 2048,
                        KeyAlgorithm.DSA => 1024,
                        KeyAlgorithm.ECC => "P-256",
                        _ => throw new Exception("Can not generate signature key.")
                    };
                    signatureKey = keyGeneratorService.GenerateKeyPair(SelectedSignatureAlgorithm.KeyAlgorithm, keyParameters).Private;
                }
                else
                {
                    var parsedKeyPair = (AsymmetricCipherKeyPair)encodingService.ParsePEM(SignatureKey);
                    var keyAlgorithm = GetKeyAlgorithm(parsedKeyPair);
                    if (keyAlgorithm != SelectedSignatureAlgorithm.KeyAlgorithm)
                    {
                        throw new Exception("Signature key does not match the selected signature algorithm.");
                    }
                    signatureKey = parsedKeyPair.Private;
                }

                var certificate = certificateSigningService.SignCertificate(pkcs10CertificationRequest, signatureKey, SelectedSignatureAlgorithm);

                SignedCertificate = encodingService.ToPEM(certificate);
            }
            catch (Exception exception)
            {
                SignedCertificate = exception.ToString();
            }
            IsCertificateSigned = true;
        }

        private static KeyAlgorithm GetKeyAlgorithm(AsymmetricCipherKeyPair keyPair)
        {
            return keyPair.Public switch
            {
                RsaKeyParameters _ => KeyAlgorithm.RSA,
                DsaKeyParameters _ => KeyAlgorithm.DSA,
                ECKeyParameters _ => KeyAlgorithm.ECC,
                _ => throw new Exception("Unsupported key algorithm")
            };
        }

        public void Back()
        {
            IsCertificateSigned = false;
            SignedCertificate = "";
        }

        public void SaveCertificate()
        {
            SaveCertificateRequested?.Invoke(this, ("certificate", SignedCertificate));
        }
    }
}