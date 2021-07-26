using CSRGenerator.Models;
using Org.BouncyCastle.Pkcs;
using ReactiveUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CSRGenerator.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public CSRGeneratorViewModel CSRGeneratorViewModel { get; } = new CSRGeneratorViewModel();

        public CertificateSignerViewModel CertificateSignerViewModel { get; } = new CertificateSignerViewModel();

    }
}
