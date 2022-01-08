using DynamicData.Binding;
using Org.BouncyCastle.Asn1.X509;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSRGenerator.ViewModels
{
    public class KeyUsageSectionModel : ViewModelBase
    {
        [Reactive] public bool DigitalSignature { get; set; }
        [Reactive] public bool NonRepudiation { get; set; }
        [Reactive] public bool KeyEncipherment { get; set; }
        [Reactive] public bool DataEncipherment { get; set; }
        [Reactive] public bool KeyAgreement { get; set; }
        [Reactive] public bool EncipherOnly { get; set; }
        [Reactive] public bool DecipherOnly { get; set; }
        [Reactive] public bool CertificateSigning { get; set; }
        [Reactive] public bool CRLSigning { get; set; }

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

        public void Load(KeyUsage? keyUsage)
        {
            int keyUsageValue = keyUsage?.IntValue ?? 0;
            DigitalSignature = (keyUsageValue & KeyUsage.DigitalSignature) > 0;
            NonRepudiation = (keyUsageValue & KeyUsage.NonRepudiation) > 0;
            KeyEncipherment = (keyUsageValue & KeyUsage.KeyEncipherment) > 0;
            DataEncipherment = (keyUsageValue & KeyUsage.DataEncipherment) > 0;
            KeyAgreement = (keyUsageValue & KeyUsage.KeyAgreement) > 0;
            EncipherOnly = (keyUsageValue & KeyUsage.EncipherOnly) > 0;
            DecipherOnly = (keyUsageValue & KeyUsage.DecipherOnly) > 0;
            CertificateSigning = (keyUsageValue & KeyUsage.KeyCertSign) > 0;
            CRLSigning = (keyUsageValue & KeyUsage.CrlSign) > 0;
        }
    }
}
