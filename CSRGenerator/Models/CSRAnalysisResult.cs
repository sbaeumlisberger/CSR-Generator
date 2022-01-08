using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSRGenerator.Models
{
    public class CSRAnalysisResult
    {
        public CSRData CSRData { get; }
        public bool IsSignatureValid { get; }

        public CSRAnalysisResult(CSRData cSRData, bool isSignatureValid)
        {
            CSRData = cSRData;
            IsSignatureValid = isSignatureValid;
        }
    }
}
