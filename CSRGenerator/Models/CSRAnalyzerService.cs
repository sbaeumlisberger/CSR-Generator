using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Pkcs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSRGenerator.Models
{
    public class CSRAnalyzerService
    {

        public CSRAnalysisResult ParseCSR(Pkcs10CertificationRequest pkcs10CertificationRequest)
        {
            var requestInfo = pkcs10CertificationRequest.GetCertificationRequestInfo();

            CSRData csrData = new CSRData();
            csrData.PublicKey = pkcs10CertificationRequest.GetPublicKey();

            csrData.Subject = requestInfo.Subject;

            foreach (var attribute in requestInfo.Attributes.ToArray())
            {

                if (attribute is Asn1Sequence attributeSequence
                    && attributeSequence[0] is DerObjectIdentifier identifier
                    && identifier.Id == PkcsObjectIdentifiers.Pkcs9AtExtensionRequest.Id
                    && attributeSequence[1] is Asn1Set extensionsSet)
                {
                    var x509Extensions = X509Extensions.GetInstance(extensionsSet[0]);

                    csrData.SubjectAlternativeNames = GeneralNames.FromExtensions(x509Extensions, X509Extensions.SubjectAlternativeName);
                    csrData.KeyUsage = KeyUsage.FromExtensions(x509Extensions);
                    csrData.ExtendedKeyUsage = ExtendedKeyUsage.FromExtensions(x509Extensions);
                    csrData.BasicConstraints = BasicConstraints.FromExtensions(x509Extensions);
                }
            }

            csrData.SignatureAlgorithm = SignatureAlgorithm.ForOID(pkcs10CertificationRequest.SignatureAlgorithm.Algorithm.Id);

            bool isSignatureValid = pkcs10CertificationRequest.Verify();

            return new CSRAnalysisResult(csrData, isSignatureValid);
        }

    }
}
