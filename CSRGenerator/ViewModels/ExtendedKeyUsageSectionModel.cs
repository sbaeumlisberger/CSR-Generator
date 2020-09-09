using DynamicData.Binding;
using Org.BouncyCastle.Asn1.X509;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSRGenerator.ViewModels
{
    public class ExtendedKeyUsageSectionModel : ViewModelBase
    {
        public bool ServerAuth { get; set; }
        public bool ClientAuth { get; set; }
        public bool CodeSigning { get; set; }
        public bool EmailProtection { get; set; }
        public bool OcspSigning { get; set; }
        public bool SmartCardLogon { get; set; }
        public bool Timestamping { get; set; }

        public ExtendedKeyUsage GetExtendedKeyUsage()
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
    }
}
