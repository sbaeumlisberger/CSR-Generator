using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace CSRGenerator.ViewModels
{
    public class DistinguishedNameViewModel : ViewModelBase
    {
        [Reactive] public string CommonName { get; set; } = "";
        [Reactive] public string Organization { get; set; } = "";
        [Reactive] public string OrganizationUnit { get; set; } = "";
        [Reactive] public string Country { get; set; } = "";
        [Reactive] public string State { get; set; } = "";
        [Reactive] public string Locality { get; set; } = "";
        [Reactive] public string EMail { get; set; } = "";
        [Reactive] public string Title { get; set; } = "";
        [Reactive] public string Surname { get; set; } = "";
        [Reactive] public string GivenName { get; set; } = "";
        [Reactive] public string Initials { get; set; } = "";
        [Reactive] public string Pseudonym { get; set; } = "";
        [Reactive] public string GenerationQualifier { get; set; } = "";
        [Reactive] public string DistinguishedNameQualifier { get; set; } = "";

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

        public void Load(X509Name? distinguishedName)
        {
            CommonName = "";
            Organization = "";
            OrganizationUnit = "";
            Country = "";
            State = "";
            Locality = "";
            EMail = "";
            Title = "";
            Surname = "";
            GivenName = "";
            Initials = "";
            Pseudonym = "";
            GenerationQualifier = "";
            DistinguishedNameQualifier = "";

            if (distinguishedName is null) return;

            foreach (DerObjectIdentifier derObjectIdentifier in distinguishedName.GetOidList())
            {
                string oid = derObjectIdentifier.Id;
                var value = string.Join(",", distinguishedName.GetValueList(derObjectIdentifier).Cast<string>().ToList());
                if (oid == X509Name.CN.Id) { CommonName = value; }
                else if (oid == X509Name.O.Id) { Organization = value; }
                else if (oid == X509Name.OU.Id) { OrganizationUnit = value; }
                else if (oid == X509Name.C.Id) { Country = value; }
                else if (oid == X509Name.ST.Id) { State = value; }
                else if (oid == X509Name.L.Id) { Locality = value; }
                else if (oid == X509Name.EmailAddress.Id) { EMail = value; }
                else if (oid == X509Name.T.Id) { Title = value; }
                else if (oid == X509Name.Surname.Id) { Surname = value; }
                else if (oid == X509Name.GivenName.Id) { GivenName = value; }
                else if (oid == X509Name.Initials.Id) { Initials = value; }
                else if (oid == X509Name.Pseudonym.Id) { Pseudonym = value; }
                else if (oid == X509Name.Generation.Id) { GenerationQualifier = value; }
                else if (oid == X509Name.DnQualifier.Id) { DistinguishedNameQualifier = value; }
                else { Debug.WriteLine("Unknown OID: " + oid); }
            }
        }
    }
}
