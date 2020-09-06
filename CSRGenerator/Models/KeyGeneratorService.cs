using Org.BouncyCastle.Crypto;
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
        public AsymmetricCipherKeyPair GenerateKeyPair(KeyAlgorithm keyAlogrithm, int keyLength)
        {
            var randomGenerator = new CryptoApiRandomGenerator();
            var random = new SecureRandom(randomGenerator);
            var keyGenerationParameters = new KeyGenerationParameters(random, keyLength);

            IAsymmetricCipherKeyPairGenerator keyPairGenerator;
            switch (keyAlogrithm)
            {
                case KeyAlgorithm.RSA:
                    keyPairGenerator = new RsaKeyPairGenerator();
                    break;
                case KeyAlgorithm.DSA:
                    keyPairGenerator = new DsaKeyPairGenerator();
                    break;
                case KeyAlgorithm.ECC:
                    keyPairGenerator = new ECKeyPairGenerator();
                    break;
                default:
                    throw new Exception("unsupported key alogrithm");
            }
            keyPairGenerator.Init(keyGenerationParameters);
            return keyPairGenerator.GenerateKeyPair();
        }

    }
}
