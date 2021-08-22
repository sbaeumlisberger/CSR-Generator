using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;

namespace CSRGenerator.Models
{
    public class SignatureAlgorithm
    {
        public static readonly IReadOnlyList<SignatureAlgorithm> Values = new List<SignatureAlgorithm>();

        public static readonly SignatureAlgorithm SHA1withRSA = new SignatureAlgorithm(KeyAlgorithm.RSA, "SHA1WITHRSA", "1.2.840.113549.1.1.5");
        public static readonly SignatureAlgorithm SHA256withRSA = new SignatureAlgorithm(KeyAlgorithm.RSA, "SHA256WITHRSA", "1.2.840.113549.1.1.11");
        public static readonly SignatureAlgorithm SHA384withRSA = new SignatureAlgorithm(KeyAlgorithm.RSA, "SHA384WITHRSA", "1.2.840.113549.1.1.12");
        public static readonly SignatureAlgorithm SHA512withRSA = new SignatureAlgorithm(KeyAlgorithm.RSA, "SHA512WITHRSA", "1.2.840.113549.1.1.13");

        public static readonly SignatureAlgorithm SHA1withECDSA = new SignatureAlgorithm(KeyAlgorithm.ECC, "SHA1WITHECDSA", "1.2.840.10045.4.1");
        public static readonly SignatureAlgorithm SHA256withECDSA = new SignatureAlgorithm(KeyAlgorithm.ECC, "SHA256WITHECDSA", "1.2.840.10045.4.3.2");
        public static readonly SignatureAlgorithm SHA384withECDSA = new SignatureAlgorithm(KeyAlgorithm.ECC, "SHA384WITHECDSA", "1.2.840.10045.4.3.3");
        public static readonly SignatureAlgorithm SHA512withRECDSA = new SignatureAlgorithm(KeyAlgorithm.ECC, "SHA512WITHECDSA", "1.2.840.10045.4.3.4");

        public static readonly SignatureAlgorithm SHA1withDSA = new SignatureAlgorithm(KeyAlgorithm.DSA, "SHA1WITHDSA", "1.2.840.10040.4.3");
        public static readonly SignatureAlgorithm SHA256withDSA = new SignatureAlgorithm(KeyAlgorithm.DSA, "SHA256WITHDSA", "2.16.840.1.101.3.4.3.2");
        public static readonly SignatureAlgorithm SHA384withDSA = new SignatureAlgorithm(KeyAlgorithm.DSA, "SHA384WITHDSA", "2.16.840.1.101.3.4.3.3");
        public static readonly SignatureAlgorithm SHA512withDSA = new SignatureAlgorithm(KeyAlgorithm.DSA, "SHA512WITHDSA", "2.16.840.1.101.3.4.3.4");

        public KeyAlgorithm KeyAlgorithm { get; }

        public string Identifier { get; }

        public string OID { get; }

        public SignatureAlgorithm(KeyAlgorithm keyAlgorithm, string value, string oid)
        {
            KeyAlgorithm = keyAlgorithm;
            Identifier = value;
            OID = oid;
            ((List<SignatureAlgorithm>)Values).Add(this);
        }

        public static SignatureAlgorithm? ForOID(string oid)
        {
            return Values.Where(sa => sa.OID == oid).FirstOrDefault();
        }

        public override string ToString()
        {
            return Identifier;
        }
    }
}
