using CSRGenerator.Models;
using Org.BouncyCastle.Pkcs;
using ReactiveUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CSRGenerator.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public event EventHandler<(string Name, string CSR)> SaveCSRRequested;

        public event EventHandler<(string Name, string PrivateKey)> SavePrivateKeyRequested;

        public KeySectionModel KeySectionModel { get; } = new KeySectionModel();

        public SubjectSectionModel SubjectSectionModel { get; } = new SubjectSectionModel();

        public SANSectionModel SANSectionModel { get; } = new SANSectionModel();

        public KeyUsageSectionModel KeyUsageSectionModel { get; } = new KeyUsageSectionModel();

        public ExtendedKeyUsageSectionModel ExtendedKeyUsageSectionModel { get; } = new ExtendedKeyUsageSectionModel();

        public IReadOnlyList<SignatureAlgorithm> AvailableSignatureAlgorithm { get; } = typeof(SignatureAlgorithm)
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Select(field => (SignatureAlgorithm)field.GetValue(null)).ToArray();

        public SignatureAlgorithm SelectedSignatureAlgorithm { get; set; } = SignatureAlgorithm.SHA256withRSA;

        public bool IsCSRGenerated { get => isCSRGenerated; set => this.RaiseAndSetIfChanged(ref isCSRGenerated, value); }

        public string GeneratedCSR { get => generatedCSR; set => this.RaiseAndSetIfChanged(ref generatedCSR, value); }

        public string PrivateKey { get => privateKey; set => this.RaiseAndSetIfChanged(ref privateKey, value); }

        private readonly CSRGeneratorService csrGenerator = new CSRGeneratorService();

        private readonly KeyGeneratorService keyGenerator = new KeyGeneratorService();

        private readonly EncodingService encodingService = new EncodingService();

        private bool isCSRGenerated = false;
        private string generatedCSR = string.Empty;
        private string privateKey = string.Empty;

        public void GenerateCSR()
        {
            try
            {
                var keyPair = keyGenerator.GenerateKeyPair(KeySectionModel.SelectedKeyAlgorithm, KeySectionModel.KeyLength);

                PrivateKey = encodingService.ToPEM(keyPair.Private);

                var csrData = new CSRData
                {
                    KeyPair = keyPair,
                    Subject = SubjectSectionModel.ToX509Name(),
                    SubjectAlternativeNames = SANSectionModel.ToGeneralNames(),
                    KeyUsage = KeyUsageSectionModel.ToKeyUsage(),
                    ExtendedKeyUsage = ExtendedKeyUsageSectionModel.ToExtendedKeyUsage(),
                    SignatureAlgorithm = SelectedSignatureAlgorithm
                };

                var pkcs10CSR = csrGenerator.GenerateCSR(csrData);

                GeneratedCSR = encodingService.ToPEM(pkcs10CSR);
            }
            catch (Exception exception)
            {
                GeneratedCSR = exception.ToString();
            }

            IsCSRGenerated = true;
        }

        public void Back()
        {
            IsCSRGenerated = false;
        }

        public void SaveCSR()
        {
            string cn = SubjectSectionModel.CommonName;
            string name = string.IsNullOrWhiteSpace(cn) ? "csr" : cn;
            SaveCSRRequested?.Invoke(this, (name, GeneratedCSR));
        }

        public void SavePrivateKey()
        {
            string cn = SubjectSectionModel.CommonName;
            string name = string.IsNullOrWhiteSpace(cn) ? "key" : cn + "-key";
            SavePrivateKeyRequested?.Invoke(this, (name, PrivateKey));
        }
    }
}
