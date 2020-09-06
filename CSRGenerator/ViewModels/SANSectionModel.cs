using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace CSRGenerator.ViewModels
{
    public class SANType
    {
        public string Name { get; }
        public int Tag { get; }

        public SANType(string name, int tag)
        {
            Name = name;
            Tag = tag;
        }
        public override string ToString()
        {
            return Name;
        }
    }

    public class SubjectAlternativeName
    {
        public SANType Type { get; set; }
        public string Value { get; set; }
    }

    public class SANSectionModel
    {
        public ObservableCollection<SubjectAlternativeName> SubjectAlternativeNames { get; } = new ObservableCollection<SubjectAlternativeName>();

        public IReadOnlyList<SANType> AvailableTypes { get; } = new List<SANType>()
        {
            new SANType("DNS-Name", GeneralName.DnsName),
            new SANType("E-Mail", GeneralName.Rfc822Name),
            new SANType("IP-Address", GeneralName.IPAddress),
            new SANType("URI", GeneralName.UniformResourceIdentifier)
        };

        public void AddSAN()
        {
            SubjectAlternativeNames.Add(new SubjectAlternativeName());
        }

        public void RemoveSAN(SubjectAlternativeName subjectAlternativeName)
        {
            SubjectAlternativeNames.Remove(subjectAlternativeName);
        }

        public GeneralName[] ToGeneralNames()
        {
            return SubjectAlternativeNames
                .Where(san => san.Type != null)
                .Select(san => new GeneralName(san.Type.Tag, san.Value))
                .ToArray();
        }
    }
}
