using CSRGenerator.Models;
using Org.BouncyCastle.Pkcs;
using ReactiveUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CSRGenerator.ViewModels
{
    public class CSRGeneratorViewModel : ViewModelBase
    {
        public event EventHandler<(string Name, string CSR)>? SaveCSRRequested;

        public event EventHandler<(string Name, string PrivateKey)>? SavePrivateKeyRequested;

        public KeySectionModel KeySectionModel { get; } = new KeySectionModel();

        public DistinguishedNameViewModel SubjectSectionModel { get; } = new DistinguishedNameViewModel();

        public SANSectionModel SANSectionModel { get; } = new SANSectionModel();

        public KeyUsageSectionModel KeyUsageSectionModel { get; } = new KeyUsageSectionModel();

        public ExtendedKeyUsageSectionModel ExtendedKeyUsageSectionModel { get; } = new ExtendedKeyUsageSectionModel();

        public BasicConstraintsSectionModel BasicConstraintsSectionModel { get; } = new BasicConstraintsSectionModel();

        public SignatureAlgorithmSectionModel SignatureAlgorithmSectionModel { get; } = new SignatureAlgorithmSectionModel();

        public bool IsCSRGenerated { get => isCSRGenerated; set => this.RaiseAndSetIfChanged(ref isCSRGenerated, value); }

        public string GeneratedCSR { get => generatedCSR; set => this.RaiseAndSetIfChanged(ref generatedCSR, value); }

        public string PrivateKey { get => privateKey; set => this.RaiseAndSetIfChanged(ref privateKey, value); }

        private readonly CSRGeneratorService csrGenerator = new CSRGeneratorService();

        private readonly KeyGeneratorService keyGenerator = new KeyGeneratorService();

        private readonly EncodingService encodingService = new EncodingService();

        private bool isCSRGenerated = false;
        private string generatedCSR = string.Empty;
        private string privateKey = string.Empty;

        public CSRGeneratorViewModel()
        {
            KeySectionModel.PropertyChanged += KeySectionModel_PropertyChanged;
            SignatureAlgorithmSectionModel.FilterBy(KeySectionModel.SelectedKeyAlgorithm);
        }

        private void KeySectionModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(KeySectionModel.SelectedKeyAlgorithm))
            {
                SignatureAlgorithmSectionModel.FilterBy(KeySectionModel.SelectedKeyAlgorithm);
            }
        }

        public void GenerateCSR()
        {
            try
            {
                var keyPair = keyGenerator.GenerateKeyPair(KeySectionModel.SelectedKeyAlgorithm, KeySectionModel.SelectedKeyParameters);

                PrivateKey = encodingService.ToPEM(keyPair.Private);

                var csrData = new CSRData
                {
                    PublicKey = keyPair.Public,
                    Subject = SubjectSectionModel.GetX509Name(),
                    SubjectAlternativeNames = SANSectionModel.GetGeneralNames(),
                    KeyUsage = KeyUsageSectionModel.GetKeyUsage(),
                    ExtendedKeyUsage = ExtendedKeyUsageSectionModel.GetExtendedKeyUsage(),
                    BasicConstraints = BasicConstraintsSectionModel.GetBasicConstraints(),
                    SignatureAlgorithm = SignatureAlgorithmSectionModel.SelectedSignatureAlgorithm
                };

                var pkcs10CSR = csrGenerator.GenerateCSR(csrData, keyPair.Private);

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
