using DynamicData.Binding;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSRGenerator.ViewModels
{
    public class ExtendedKeyUsageSectionModel : ViewModelBase
    {
        [Reactive] public bool ServerAuth { get; set; }
        [Reactive] public bool ClientAuth { get; set; }
        [Reactive] public bool CodeSigning { get; set; }
        [Reactive] public bool EmailProtection { get; set; }
        [Reactive] public bool OcspSigning { get; set; }
        [Reactive] public bool SmartCardLogon { get; set; }
        [Reactive] public bool Timestamping { get; set; }

        public ExtendedKeyUsage? GetExtendedKeyUsage()
        {
            List<KeyPurposeID> extendedKeyUsages = new List<KeyPurposeID>();

            if (ServerAuth) extendedKeyUsages.Add(KeyPurposeID.IdKPServerAuth);
            if (ClientAuth) extendedKeyUsages.Add(KeyPurposeID.IdKPClientAuth);
            if (CodeSigning) extendedKeyUsages.Add(KeyPurposeID.IdKPCodeSigning);
            if (EmailProtection) extendedKeyUsages.Add(KeyPurposeID.IdKPEmailProtection);
            if (OcspSigning) extendedKeyUsages.Add(KeyPurposeID.IdKPOcspSigning);
            if (SmartCardLogon) extendedKeyUsages.Add(KeyPurposeID.IdKPSmartCardLogon);
            if (Timestamping) extendedKeyUsages.Add(KeyPurposeID.IdKPTimeStamping);

            if (extendedKeyUsages.Any())
            {
                return new ExtendedKeyUsage(extendedKeyUsages);
            }
            return null;
        }

        public void Load(ExtendedKeyUsage? extendedKeyUsage)
        {
            var keyPurposeIDs = extendedKeyUsage?.GetAllUsages().Cast<DerObjectIdentifier>().Select(derOid => derOid.Id).ToList() ?? new List<string>();

            ServerAuth = keyPurposeIDs.Contains(KeyPurposeID.IdKPServerAuth.Id);
            ClientAuth = keyPurposeIDs.Contains(KeyPurposeID.IdKPClientAuth.Id);
            CodeSigning = keyPurposeIDs.Contains(KeyPurposeID.IdKPCodeSigning.Id);
            EmailProtection = keyPurposeIDs.Contains(KeyPurposeID.IdKPEmailProtection.Id);
            OcspSigning = keyPurposeIDs.Contains(KeyPurposeID.IdKPOcspSigning.Id);
            SmartCardLogon = keyPurposeIDs.Contains(KeyPurposeID.IdKPSmartCardLogon.Id);
            Timestamping = keyPurposeIDs.Contains(KeyPurposeID.IdKPTimeStamping.Id);
        }

    }
}
