using Avalonia.Data.Converters;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Asn1;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSRGenerator.Converters
{
    internal class SubjectNameIdentifierToDisplayNameConverter : IValueConverter
    {
        public Dictionary<DerObjectIdentifier, string> DisplayNames { get; } = new()
        {
            { X509Name.CN, "Common Name (CN)" },
            { X509Name.O, "Organization (O)" },
            { X509Name.OU, "Organization Unit (OU)" },
            { X509Name.C, "Country (C)"},
            { X509Name.ST, "State (ST)" },
            { X509Name.L, "Locality (L)" },
            { X509Name.EmailAddress, "CE-Mail (E)" },
            { X509Name.T, "Title (T)" },
            { X509Name.Surname, "Surname (SN)" },
            { X509Name.GivenName, "Given Name (GN)" },
            { X509Name.Initials, "Initials" },
            { X509Name.Pseudonym, "Pseudonym" },
            { X509Name.Generation, "Generation" },
            { X509Name.DnQualifier, "DN Qualifier" }
        };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var identifier = (DerObjectIdentifier)value;
            return DisplayNames.TryGetValue(identifier, out var displayName) ? displayName : identifier.Id;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
