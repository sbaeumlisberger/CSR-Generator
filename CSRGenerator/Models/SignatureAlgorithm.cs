using System;
using System.Collections.Generic;
using System.Text;

namespace CSRGenerator.Models
{
    public class SignatureAlgorithm
    {
        public static readonly SignatureAlgorithm SHA1WithRSA = new SignatureAlgorithm("SHA1WITHRSA");
        public static readonly SignatureAlgorithm SHA256withRSA = new SignatureAlgorithm("SHA256WITHRSA");
        public static readonly SignatureAlgorithm SHA384withRSA = new SignatureAlgorithm("SHA384WITHRSA");
        public static readonly SignatureAlgorithm SHA512withRSA = new SignatureAlgorithm("SHA512WITHRSA");

        public static readonly SignatureAlgorithm SHA1WithECDSA = new SignatureAlgorithm("SHA1WITHECDSA");
        public static readonly SignatureAlgorithm SHA256withECDSA = new SignatureAlgorithm("SHA256WITHECDSA");
        public static readonly SignatureAlgorithm SHA384withECDSA = new SignatureAlgorithm("SHA384WITHECDSA");
        public static readonly SignatureAlgorithm SHA512withRECDSA = new SignatureAlgorithm("SHA512WITHECDSA");

        public static readonly SignatureAlgorithm SHA1WithDSA = new SignatureAlgorithm("SHA1WITHDSA");

        public string Identifier { get; }

        public SignatureAlgorithm(string value)
        {
            Identifier = value;
        }

        public override string ToString()
        {
            return Identifier;
        }
    }
}
