using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Operators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.X509;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSRGenerator.Models
{
    public class CertificateSigningService
    {
        public X509Certificate SignCertificate(Pkcs10CertificationRequest pkcs10CertificationRequest, AsymmetricKeyParameter signatureKey, SignatureAlgorithm signatureAlgorithm)
        {
            var requestInfo = pkcs10CertificationRequest.GetCertificationRequestInfo();

            var certGenerator = new X509V3CertificateGenerator();

            certGenerator.SetSubjectDN(requestInfo.Subject);
            certGenerator.SetPublicKey(pkcs10CertificationRequest.GetPublicKey());
            certGenerator.SetIssuerDN(requestInfo.Subject); // TODO
            certGenerator.SetNotBefore(DateTime.Now); // TODO
            certGenerator.SetNotAfter(DateTime.Now.AddYears(1)); // TODO
            certGenerator.SetSerialNumber(BigInteger.Arbitrary(32));

            foreach (var attribute in requestInfo.Attributes.ToArray())
            {
                if(attribute is Asn1Sequence attributeSequence 
                    && attributeSequence[0] is DerObjectIdentifier identifier 
                    && identifier.Id == PkcsObjectIdentifiers.Pkcs9AtExtensionRequest.Id
                    && attributeSequence[1] is Asn1Set extensionsSet) 
                {
                    var x509Extensions = X509Extensions.GetInstance(extensionsSet[0]);
                    foreach (DerObjectIdentifier oid in x509Extensions.GetExtensionOids()) 
                    {
                        X509Extension x509Extension = x509Extensions.GetExtension(oid);                        
                        certGenerator.AddExtension(oid, x509Extension.IsCritical, x509Extension.GetParsedValue());
                    }
                    break;
                }
            }

            var signatureFactory = new Asn1SignatureFactory(signatureAlgorithm.Identifier, signatureKey);

            var certificate = certGenerator.Generate(signatureFactory);

            return certificate;
        }

    }
}
