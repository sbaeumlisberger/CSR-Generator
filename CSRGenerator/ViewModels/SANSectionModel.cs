using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net;
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
        public SANType? Type { get; set; }
        public string? Value { get; set; }
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

        public GeneralNames? GetGeneralNames()
        {
            var generalNames = SubjectAlternativeNames
                .Where(san => san.Type != null && san.Value != null)
                .Select(san => new GeneralName(san.Type!.Tag, san.Value))
                .ToArray();

            return generalNames.Any() ? new GeneralNames(generalNames) : null;
        }

        public void Load(GeneralNames? generalNames)
        {
            SubjectAlternativeNames.Clear();

            if (generalNames is null) return;

            foreach (var generalName in generalNames.GetNames())
            {
                var san = new SubjectAlternativeName();
                san.Type = TagToSanType(generalName.TagNo);

                if (generalName.Name is DerIA5String derIA5String)
                {
                    san.Value = derIA5String.GetString();
                }
                else if (generalName.TagNo == GeneralName.IPAddress && generalName.Name is DerOctetString derOctetString)
                {
                    san.Value = new IPAddress(derOctetString.GetOctets()).ToString();
                }

                SubjectAlternativeNames.Add(san);
            }
        }

        private static SANType TagToSanType(int tag)
        {
            switch (tag)
            {
                case GeneralName.DnsName: return new SANType("DNS-Name", GeneralName.DnsName);
                case GeneralName.Rfc822Name: return new SANType("E-Mail", GeneralName.Rfc822Name);
                case GeneralName.IPAddress: return new SANType("IP-Address", GeneralName.IPAddress);
                case GeneralName.UniformResourceIdentifier: return new SANType("URI", GeneralName.UniformResourceIdentifier);
            }
            return new SANType("Unknown", tag);
        }
    }
}
