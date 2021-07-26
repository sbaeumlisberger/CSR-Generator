using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CSRGenerator.ViewModels;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CSRGenerator.Views
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
