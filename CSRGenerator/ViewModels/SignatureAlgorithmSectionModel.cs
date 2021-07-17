using CSRGenerator.Models;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CSRGenerator.ViewModels
{
    public class SignatureAlgorithmSectionModel : ViewModelBase
    {
        [Reactive]
        public IReadOnlyList<SignatureAlgorithm> AvailableSignatureAlgorithm { get; set; } = SignatureAlgorithm.Values;

        [Reactive]
        public SignatureAlgorithm SelectedSignatureAlgorithm { get; set; } = SignatureAlgorithm.SHA256withRSA;

        public void FilterBy(KeyAlgorithm keyAlgorithm)
        {
            AvailableSignatureAlgorithm = SignatureAlgorithm.Values.Where(sa => sa.KeyAlgorithm == keyAlgorithm).ToList();
            SelectedSignatureAlgorithm = AvailableSignatureAlgorithm.SkipWhile(sa => sa.Identifier.StartsWith("SHA1")).First();
        }
    }
}
