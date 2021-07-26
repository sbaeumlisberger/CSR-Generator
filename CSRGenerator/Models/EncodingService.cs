using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Pkcs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CSRGenerator.Models
{
    public class EncodingService
    {
        public string ToPEM(object obj)
        {
            StringBuilder stringBuilder = new StringBuilder();
            StringWriter stringWriter = new StringWriter(stringBuilder);
            PemWriter pemWriter = new PemWriter(stringWriter);
            pemWriter.WriteObject(obj);
            stringWriter.Flush();
            return stringBuilder.ToString();
        }

        public object ParsePEM(string pem)
        {
            return new PemReader(new StringReader(pem)).ReadObject();
        }
    }
}
