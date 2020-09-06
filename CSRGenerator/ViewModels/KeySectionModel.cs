using CSRGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSRGenerator.ViewModels
{
    public class KeySectionModel
    {
        public IReadOnlyList<KeyAlgorithm> AvailableKeyAlgorithms { get; } = Enum.GetValues(typeof(KeyAlgorithm)).Cast<KeyAlgorithm>().ToArray();

        public KeyAlgorithm SelectedKeyAlgorithm { get; set; } = KeyAlgorithm.RSA;

        public int KeyLength { get; set; } = 2048;
    }
}
