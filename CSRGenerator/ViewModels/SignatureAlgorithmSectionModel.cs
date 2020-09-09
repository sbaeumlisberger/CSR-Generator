using CSRGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CSRGenerator.ViewModels
{
    public class SignatureAlgorithmSectionModel
    {
        public IReadOnlyList<SignatureAlgorithm> AvailableSignatureAlgorithm { get; } = typeof(SignatureAlgorithm)
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Select(field => (SignatureAlgorithm)field.GetValue(null)).ToArray();

        public SignatureAlgorithm SelectedSignatureAlgorithm { get; set; } = SignatureAlgorithm.SHA256withRSA;
    }
}
