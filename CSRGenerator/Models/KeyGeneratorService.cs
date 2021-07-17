using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSRGenerator.Models
{
    public class KeyGeneratorService
    {
        public AsymmetricCipherKeyPair GenerateKeyPair(KeyAlgorithm keyAlogrithm, object parameters)
        {
            var random = new SecureRandom(new CryptoApiRandomGenerator());

            IAsymmetricCipherKeyPairGenerator keyPairGenerator;
            switch (keyAlogrithm)
            {
                case KeyAlgorithm.RSA:
                    int keyLength = (int)parameters;
                    keyPairGenerator = new RsaKeyPairGenerator();
                    keyPairGenerator.Init(new KeyGenerationParameters(random, keyLength));
                    break;
                case KeyAlgorithm.DSA:
                    keyLength = (int)parameters;
                    keyPairGenerator = new DsaKeyPairGenerator();
                    var parametersGenerator = new DsaParametersGenerator();
                    parametersGenerator.Init(keyLength, 100, random);
                    keyPairGenerator.Init(new DsaKeyGenerationParameters(random, parametersGenerator.GenerateParameters()));
                    break;
                case KeyAlgorithm.ECC:
                    string curveName = (string)parameters;
                    var curve = ECNamedCurveTable.GetByName(curveName);
                    var domainParams = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H, curve.GetSeed());
                    keyPairGenerator = new ECKeyPairGenerator();
                    keyPairGenerator.Init(new ECKeyGenerationParameters(domainParams, random));
                    break;
                default:
                    throw new Exception("Unsupported key alogrithm");
            }

            return keyPairGenerator.GenerateKeyPair();
        }

    }
}
