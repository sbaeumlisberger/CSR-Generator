using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSRGenerator.Models
{
    public class CSRData
    {
        public AsymmetricCipherKeyPair KeyPair { get; set; }

        public X509Name Subject { get; set; }

        public GeneralNames SubjectAlternativeNames { get; set; }

        public KeyUsage KeyUsage { get; set; }

        public ExtendedKeyUsage ExtendedKeyUsage { get; set; }

        public BasicConstraints BasicConstraints { get; set; }

        public SignatureAlgorithm SignatureAlgorithm { get; set; }
    }
}
