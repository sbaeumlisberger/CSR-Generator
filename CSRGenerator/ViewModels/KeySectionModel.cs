using CSRGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using Org.BouncyCastle.Asn1.X9;
using ReactiveUI.Fody.Helpers;

namespace CSRGenerator.ViewModels
{
    public class KeySectionModel : ViewModelBase
    {
        public IReadOnlyList<KeyAlgorithm> AvailableKeyAlgorithms { get; } = Enum.GetValues(typeof(KeyAlgorithm)).Cast<KeyAlgorithm>().ToArray();

        [Reactive]
        public KeyAlgorithm SelectedKeyAlgorithm { get; set; } = KeyAlgorithm.RSA;

        [Reactive]
        public string KeyParametersLabel { get; set; } = "Length";

        [Reactive]
        public List<object> AvailableKeyParameters { get; set; } = new List<object>() { 1024, 2048, 3072, 4096 };

        [Reactive]
        public object SelectedKeyParameters { get; set; } = 2048;

        public KeySectionModel()
        {
            PropertyChanged += KeySectionModel_PropertyChanged;
        }

        private void KeySectionModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectedKeyAlgorithm))
            {
                switch (SelectedKeyAlgorithm)
                {
                    case KeyAlgorithm.RSA:
                        AvailableKeyParameters = new List<object>() { 1024, 2048, 3072, 4096 };
                        KeyParametersLabel = "Length";
                        SelectedKeyParameters = 2048;
                        break;
                    case KeyAlgorithm.DSA:
                        AvailableKeyParameters = new List<object>() { 1024 };
                        KeyParametersLabel = "Length";
                        SelectedKeyParameters = 1024;
                        break;
                    case KeyAlgorithm.ECC:
                        AvailableKeyParameters = ECNamedCurveTable.Names.Cast<object>().ToList();
                        KeyParametersLabel = "Curve";
                        SelectedKeyParameters = AvailableKeyParameters.First();
                        break;
                }
            }
        }
    }
}
