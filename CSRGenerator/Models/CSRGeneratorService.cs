﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;

namespace CSRGenerator.Models
{
    public class CSRGeneratorService
    {
        public Pkcs10CertificationRequest GenerateCSR(CSRData csrData, AsymmetricKeyParameter privateKey)
        {
            if (csrData.PublicKey is null || csrData.SignatureAlgorithm is null)
            {
                throw new ArgumentException("Invalid CSR data", nameof(csrData));
            }

            X509ExtensionsGenerator extensions = new X509ExtensionsGenerator();

            if (csrData.KeyUsage != null)
            {
                extensions.AddExtension(X509Extensions.KeyUsage, true, csrData.KeyUsage);
            }
            if (csrData.ExtendedKeyUsage != null)
            {
                extensions.AddExtension(X509Extensions.ExtendedKeyUsage, false, csrData.ExtendedKeyUsage);
            }
            if (csrData.SubjectAlternativeNames != null)
            {
                extensions.AddExtension(X509Extensions.SubjectAlternativeName, false, csrData.SubjectAlternativeNames);
            }
            if (csrData.BasicConstraints != null)
            {
                extensions.AddExtension(X509Extensions.BasicConstraints, true, csrData.BasicConstraints);
            }

            AttributeX509 extensionsAttribute = new AttributeX509(PkcsObjectIdentifiers.Pkcs9AtExtensionRequest, new DerSet(extensions.Generate()));

            DerSet attributes = new DerSet(extensionsAttribute);

            return new Pkcs10CertificationRequest(csrData.SignatureAlgorithm.Identifier, csrData.Subject, csrData.PublicKey, attributes, privateKey);
        }

    }
}
