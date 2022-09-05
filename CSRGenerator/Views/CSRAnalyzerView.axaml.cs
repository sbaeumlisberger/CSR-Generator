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
    public class CSRAnalyzerView : UserControl
    {
        private CSRAnalyzerViewModel? viewModel;

        public CSRAnalyzerView()
        {
            AvaloniaXamlLoader.Load(this);

            DataContextChanged += CSRAnalyzerView_DataContextChanged;
        }

        private void CSRAnalyzerView_DataContextChanged(object? sender, EventArgs e)
        {
            if (this.viewModel is null && DataContext is CSRAnalyzerViewModel viewModel)
            {
                this.viewModel = viewModel;
                viewModel.OpenDialogRequested += ViewModel_OpenDialogRequested;
            }
        }
        private async Task ViewModel_OpenDialogRequested(object sender, FileOpenDialogModel e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog!.Filters.Add(new FileDialogFilter() { Name = e.FilterLabel, Extensions = e.FileExtensionsFilter });
            openFileDialog.Filters.Add(new FileDialogFilter() { Name = "All files", Extensions = new List<string>() { "*" } });

            if (await openFileDialog.ShowAsync(App.Current.MainWindow) is string[] filePaths && filePaths.Any())
            {
                e.SelectedFile = filePaths[0];
            }
        }
    }
}
