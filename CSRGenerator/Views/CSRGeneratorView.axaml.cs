using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CSRGenerator.ViewModels;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CSRGenerator.Views
{
    public class CSRGeneratorView : UserControl
    {
        public CSRGeneratorView()
        {
            AvaloniaXamlLoader.Load(this);
            DataContextChanged += MainWindow_DataContextChanged;
        }

        private void MainWindow_DataContextChanged(object? sender, EventArgs e)
        {
            if (DataContext is CSRGeneratorViewModel viewModel)
            {
                viewModel.SaveCSRRequested += ViewModel_SaveCSRRequested;
                viewModel.SavePrivateKeyRequested += ViewModel_SavePrivateKeyRequested;
            }
        }

        private async void ViewModel_SaveCSRRequested(object? sender, (string Name, string CSR) e)
        {
            var saveFileDialog = new SaveFileDialog()
            {
                InitialFileName = e.Name + ".csr",
                DefaultExtension = "csr"
            };
            if (await saveFileDialog.ShowAsync(App.Current.MainWindow) is string filePath)
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.Write(e.CSR);
                }
            }
        }

        private async void ViewModel_SavePrivateKeyRequested(object? sender, (string Name, string PrivateKey) e)
        {
            var saveFileDialog = new SaveFileDialog()
            {
                InitialFileName = e.Name + ".pem",
                DefaultExtension = "pem"
            };
            if (await saveFileDialog.ShowAsync(App.Current.MainWindow) is string filePath)
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.Write(e.PrivateKey);
                }
            }
        }
    }
}
