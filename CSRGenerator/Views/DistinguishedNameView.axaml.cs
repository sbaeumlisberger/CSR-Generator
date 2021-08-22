using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CSRGenerator.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CSRGenerator.Views
{
    public class DistinguishedNameView : UserControl
    {

        public DistinguishedNameView()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }

}
