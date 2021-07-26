using Org.BouncyCastle.Asn1.X509;
using ReactiveUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CSRGenerator.ViewModels
{
    public class SubjectSectionModel : ViewModelBase
    {
        public string CommonName { get; set; } = "";
        public string Organization { get; set; } = "";
        public string OrganizationUnit { get; set; } = "";
        public string Country { get; set; } = "";
        public string State { get; set; } = "";
        public string Locality { get; set; } = "";
        public string EMail { get; set; } = "";
        public string Title { get; set; } = "";
        public string Surname { get; set; } = "";
        public string GivenName { get; set; } = "";
        public string Initials { get; set; } = "";
        public string Pseudonym { get; set; } = "";
        public string GenerationQualifier { get; set; } = "";
        public string DistinguishedNameQualifier { get; set; } = "";

        public bool ShowAll { get => showAll; set => this.RaiseAndSetIfChanged(ref showAll, value); }

        private bool showAll = false;

        public void ToggleShowAll()
        {
            ShowAll = !ShowAll;
        }

        public X509Name GetX509Name()
        {
            Hashtable subject = new Hashtable();
            if (CommonName != "") subject.Add(X509Name.CN, CommonName);
            if (Organization != "") subject.Add(X509Name.O, Organization);
            if (OrganizationUnit != "") subject.Add(X509Name.OU, OrganizationUnit);
            if (Country != "") subject.Add(X509Name.C, Country);
            if (State != "") subject.Add(X509Name.ST, State);
            if (Locality != "") subject.Add(X509Name.L, Locality);
            if (EMail != "") subject.Add(X509Name.EmailAddress, EMail);
            if (Title != "") subject.Add(X509Name.T, Title);
            if (Surname != "") subject.Add(X509Name.Surname, Surname);
            if (GivenName != "") subject.Add(X509Name.GivenName, GivenName);
            if (Initials != "") subject.Add(X509Name.Initials, Initials);
            if (Pseudonym != "") subject.Add(X509Name.Pseudonym, Pseudonym);
            if (GenerationQualifier != "") subject.Add(X509Name.Generation, GenerationQualifier);
            if (DistinguishedNameQualifier != "") subject.Add(X509Name.DnQualifier, DistinguishedNameQualifier);
            return new X509Name(new ArrayList(subject.Keys), subject);
        }
    }
}
