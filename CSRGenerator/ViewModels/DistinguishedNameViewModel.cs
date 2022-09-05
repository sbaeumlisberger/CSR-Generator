using DynamicData;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;

namespace CSRGenerator.ViewModels
{
    public class SubjectName
    {
        public DerObjectIdentifier? Identifier { get; set; }

        public string Value { get; set; } = string.Empty;
    }

    public class DistinguishedNameViewModel : ViewModelBase
    {
        public ObservableCollection<SubjectName> SubjectNames { get; } = new ObservableCollection<SubjectName>();

        public IReadOnlyList<DerObjectIdentifier> AvailableIdentifiers { get; } = new List<DerObjectIdentifier>()
        {
            X509Name.CN,
            X509Name.O,
            X509Name.OU,
            X509Name.C,
            X509Name.ST,
            X509Name.L,
            X509Name.EmailAddress,
            X509Name.T,
            X509Name.Surname,
            X509Name.GivenName,
            X509Name.Initials,
            X509Name.Pseudonym,
            X509Name.Generation,
            X509Name.DnQualifier
        };

        public void AddSubjectName()
        {
            SubjectNames.Add(new SubjectName());
        }

        public void RemoveSubjectName(SubjectName subjectName)
        {
            SubjectNames.Remove(subjectName);
        }

        public X509Name GetX509Name()
        {
            var oids = new List<DerObjectIdentifier>();
            var values = new List<string>();
            foreach (var subjectName in SubjectNames)
            {
                if (subjectName.Identifier != null && !string.IsNullOrEmpty(subjectName.Value))
                {
                    oids.Add(subjectName.Identifier);
                    values.Add(subjectName.Value);
                }
            }
            return new X509Name(oids, values);
        }

        public void Load(X509Name? distinguishedName)
        {
            SubjectNames.Clear();

            if (distinguishedName is null) return;

            var oidList = distinguishedName.GetOidList().Cast<DerObjectIdentifier>().ToList();
            var valuesList = distinguishedName.GetValueList().Cast<string>().ToList();

            for (int i = 0; i < oidList.Count; i++)
            {
                SubjectNames.Add(new SubjectName()
                {
                    Identifier = oidList[i],
                    Value = valuesList[i]
                });
            }
        }
    }
}
