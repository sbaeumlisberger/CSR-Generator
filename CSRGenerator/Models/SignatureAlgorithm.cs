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

        public static readonly SignatureAlgorithm SHA1WithRSA = new SignatureAlgorithm(KeyAlgorithm.RSA, "SHA1WITHRSA");
        public static readonly SignatureAlgorithm SHA256withRSA = new SignatureAlgorithm(KeyAlgorithm.RSA, "SHA256WITHRSA");
        public static readonly SignatureAlgorithm SHA384withRSA = new SignatureAlgorithm(KeyAlgorithm.RSA, "SHA384WITHRSA");
        public static readonly SignatureAlgorithm SHA512withRSA = new SignatureAlgorithm(KeyAlgorithm.RSA, "SHA512WITHRSA");

        public static readonly SignatureAlgorithm SHA1WithECDSA = new SignatureAlgorithm(KeyAlgorithm.ECC, "SHA1WITHECDSA");
        public static readonly SignatureAlgorithm SHA256withECDSA = new SignatureAlgorithm(KeyAlgorithm.ECC, "SHA256WITHECDSA");
        public static readonly SignatureAlgorithm SHA384withECDSA = new SignatureAlgorithm(KeyAlgorithm.ECC, "SHA384WITHECDSA");
        public static readonly SignatureAlgorithm SHA512withRECDSA = new SignatureAlgorithm(KeyAlgorithm.ECC, "SHA512WITHECDSA");

        public static readonly SignatureAlgorithm SHA1WithDSA = new SignatureAlgorithm(KeyAlgorithm.DSA, "SHA1WITHDSA");
        public static readonly SignatureAlgorithm SHA256WITHDSA = new SignatureAlgorithm(KeyAlgorithm.DSA, "SHA256WITHDSA");
        public static readonly SignatureAlgorithm SHA384WITHDSA = new SignatureAlgorithm(KeyAlgorithm.DSA, "SHA384WITHDSA");
        public static readonly SignatureAlgorithm SHA512WITHDSA = new SignatureAlgorithm(KeyAlgorithm.DSA, "SHA512WITHDSA");

        public KeyAlgorithm KeyAlgorithm { get; }

        public string Identifier { get; }

        public SignatureAlgorithm(KeyAlgorithm keyAlgorithm, string value)
        {
            KeyAlgorithm = keyAlgorithm;
            Identifier = value;
            ((List<SignatureAlgorithm>)Values).Add(this);
        }

        public override string ToString()
        {
            return Identifier;
        }
    }
}
