using Org.BouncyCastle.Asn1.X509;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSRGenerator.ViewModels
{
    public class BasicConstraintsSectionModel : ViewModelBase
    {
        [Reactive] public bool IsCA { get; set; }

        public BasicConstraints? GetBasicConstraints()
        {
            return IsCA ? new BasicConstraints(IsCA) : null;
        }

        public void Load(BasicConstraints? basicConstraints)
        {
            IsCA = basicConstraints?.IsCA() ?? false;
        }
    }
}
