using CSRGenerator.Models;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Pkcs;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSRGenerator.ViewModels
{
    public class CSRAnalyzerViewModel : ViewModelBase
    {
        public event AsyncEventHandler<FileOpenDialogModel>? OpenDialogRequested;

        [Reactive] public string CSR { get; set; } = "";

        [Reactive] public bool CanAnalyze { get; private set; } = false;

        [Reactive] public bool IsAnalyzed { get; private set; } = false;

        [Reactive] public KeyAlgorithm KeyAlgorithm { get; set; } = KeyAlgorithm.RSA;
        [Reactive] public string KeyParametersLabel { get; set; } = "Length";
        [Reactive] public object KeyParameters { get; set; } = 2048;

        public DistinguishedNameViewModel SubjectSectionModel { get; } = new DistinguishedNameViewModel();

        public SANSectionModel SANSectionModel { get; } = new SANSectionModel();

        public KeyUsageSectionModel KeyUsageSectionModel { get; } = new KeyUsageSectionModel();

        public ExtendedKeyUsageSectionModel ExtendedKeyUsageSectionModel { get; } = new ExtendedKeyUsageSectionModel();

        public BasicConstraintsSectionModel BasicConstraintsSectionModel { get; } = new BasicConstraintsSectionModel();

        [Reactive] public SignatureAlgorithm SignatureAlgorithm { get; private set; } = default;
        [Reactive] public bool IsSignatureValid { get; private set; } = false;

        private static readonly IReadOnlyDictionary<ECCurve, string> ECCurveNames = ECNamedCurveTable.Names.Cast<string>().ToLookup(name => ECNamedCurveTable.GetByName(name).Curve, name => name).ToDictionary(entry => entry.Key, entry => entry.First());

        private readonly CSRAnalyzerService csrAnalyzerService = new CSRAnalyzerService();

        private readonly EncodingService encodingService = new EncodingService();

        public CSRAnalyzerViewModel()
        {
            this.WhenAnyValue(x => x.CSR).Subscribe(x => CanAnalyze = CSR != "");
        }

        public void BrowseCSR()
        {
            var dialogModel = new FileOpenDialogModel();
            OpenDialogRequested?.Invoke(this, dialogModel);

            if (dialogModel.SelectedFile is string filePath)
            {
                CSR = File.ReadAllText(filePath);
            }
        }

        public void AnalyzeCSR()
        {
            try
            {
                var pkcs10CertificationRequest = (Pkcs10CertificationRequest)encodingService.ParsePEM(CSR);

                var csrAnalysisResult = csrAnalyzerService.ParseCSR(pkcs10CertificationRequest);

                var csrData = csrAnalysisResult.CSRData;

                KeyAlgorithm = GetKeyAlgorithm(csrData.PublicKey!);
                KeyParametersLabel = GetKeyParametersLabel(csrData.PublicKey!);
                KeyParameters = GetKeyParameters(csrData.PublicKey!);

                SubjectSectionModel.Load(csrData.Subject);
                SANSectionModel.Load(csrData.SubjectAlternativeNames);
                KeyUsageSectionModel.Load(csrData.KeyUsage);
                ExtendedKeyUsageSectionModel.Load(csrData.ExtendedKeyUsage);
                BasicConstraintsSectionModel.Load(csrData.BasicConstraints);

                SignatureAlgorithm = csrData.SignatureAlgorithm!;
                IsSignatureValid = csrAnalysisResult.IsSignatureValid;

                IsAnalyzed = true;
            }
            catch (Exception exception)
            {
                Debug.WriteLine("Failed to analyze CSR: " + exception);
            }
        }

        public void Back()
        {
            IsAnalyzed = false;
        }

        private static KeyAlgorithm GetKeyAlgorithm(AsymmetricKeyParameter publicKey)
        {
            return publicKey switch
            {
                RsaKeyParameters _ => KeyAlgorithm.RSA,
                DsaKeyParameters _ => KeyAlgorithm.DSA,
                ECKeyParameters _ => KeyAlgorithm.ECC,
                _ => throw new Exception("Unsupported key algorithm")
            };
        }

        private static string GetKeyParametersLabel(AsymmetricKeyParameter publicKey)
        {
            return publicKey switch
            {
                RsaKeyParameters _ => "Length",
                DsaKeyParameters _ => "Length",
                ECKeyParameters _ => "Curve",
                _ => throw new Exception("Unsupported key algorithm")
            };
        }

        private static object GetKeyParameters(AsymmetricKeyParameter publicKey)
        {
            return publicKey switch
            {
                RsaKeyParameters rsaKeyParameters => rsaKeyParameters.Modulus.BitLength,
                DsaKeyParameters dsaKeyParameters => dsaKeyParameters.Parameters.P.BitLength,
                ECKeyParameters ecKeyParameters => ECCurveNames.GetValueOrDefault(ecKeyParameters.Parameters.Curve) ?? "unknown",
                _ => throw new Exception("Unsupported key algorithm")
            };
        }

    }
}
