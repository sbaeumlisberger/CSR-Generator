using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSRGenerator.ViewModels
{
    public class BasicConstraintsSectionModel
    {
        public bool IsCA { get; set; }

        public BasicConstraints GetBasicConstraints()
        {
            return IsCA ? new BasicConstraints(IsCA) : null;
        }
    }
}
