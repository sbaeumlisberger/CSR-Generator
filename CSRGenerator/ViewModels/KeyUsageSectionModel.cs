using DynamicData.Binding;
using Org.BouncyCastle.Asn1.X509;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSRGenerator.ViewModels
{
    public class KeyUsageSectionModel : ViewModelBase
    {
        public bool DigitalSignature { get; set; }
        public bool NonRepudiation { get; set; }
        public bool KeyEncipherment { get; set; }
        public bool DataEncipherment { get; set; }
        public bool KeyAgreement { get => keyAgreement; set => this.RaiseAndSetIfChanged(ref keyAgreement, value); }
        public bool EncipherOnly { get => encipherOnly; set => this.RaiseAndSetIfChanged(ref encipherOnly, value); }
        public bool DecipherOnly { get => decipherOnly; set => this.RaiseAndSetIfChanged(ref decipherOnly, value); }
        public bool CertificateSigning { get; set; }
        public bool CRLSigning { get; set; }

        private bool keyAgreement;
        private bool encipherOnly;
        private bool decipherOnly;

        public KeyUsageSectionModel()
        {
            PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(KeyAgreement) && !KeyAgreement)
                {
                    EncipherOnly = false;
                    DecipherOnly = false;
                }
            };
        }

        public KeyUsage? GetKeyUsage()
        {
            int keyUsage = 0;

            if (DigitalSignature) keyUsage |= KeyUsage.DigitalSignature;
            if (NonRepudiation) keyUsage |= KeyUsage.NonRepudiation;
            if (KeyEncipherment) keyUsage |= KeyUsage.KeyEncipherment;
            if (DataEncipherment) keyUsage |= KeyUsage.DataEncipherment;
            if (KeyAgreement) keyUsage |= KeyUsage.KeyAgreement;
            if (EncipherOnly) keyUsage |= KeyUsage.EncipherOnly;
            if (DecipherOnly) keyUsage |= KeyUsage.DecipherOnly;
            if (CertificateSigning) keyUsage |= KeyUsage.KeyCertSign;
            if (CRLSigning) keyUsage |= KeyUsage.CrlSign;

            return keyUsage != 0 ? new KeyUsage(keyUsage) : null;
        }
    }
}
