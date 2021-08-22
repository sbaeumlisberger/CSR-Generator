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
    public class CertificateSignerView : UserControl
    {

        public CertificateSignerView()
        {
            AvaloniaXamlLoader.Load(this);
            DataContextChanged += MainWindow_DataContextChanged;
        }

        private void MainWindow_DataContextChanged(object? sender, EventArgs e)
        {
            if (DataContext is CertificateSignerViewModel viewModel)
            {
                viewModel.SaveCertificateRequested += ViewModel_SaveCertificateRequested;
                viewModel.OpenDialogRequested += ViewModel_OpenDialogRequested;

                this.FindControl<DatePicker>("notBeforeDatePicker").SelectedDate = new DateTimeOffset(viewModel.NotBefore);
                this.FindControl<TimePicker>("notBeforeTimePicker").SelectedTime = viewModel.NotBefore.TimeOfDay;
                this.FindControl<DatePicker>("notAfterDatePicker").SelectedDate = new DateTimeOffset(viewModel.NotAfter);
                this.FindControl<TimePicker>("notAfterTimePicker").SelectedTime = viewModel.NotAfter.TimeOfDay;
            }
        }

        private async void ViewModel_SaveCertificateRequested(object? sender, (string Name, string Certificate) e)
        {
            var saveFileDialog = new SaveFileDialog()
            {
                InitialFileName = e.Name + ".pem",
                DefaultExtension = "pem"
            };
            if (await saveFileDialog.ShowAsync(App.Current.MainWindow) is string filePath)
            {
                File.WriteAllText(filePath, e.Certificate);
            }
        }

        private async Task ViewModel_OpenDialogRequested(object sender, FileOpenDialogModel e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filters.Add(new FileDialogFilter() { Name = e.FilterLabel, Extensions = e.FileExtensionsFilter });
            openFileDialog.Filters.Add(new FileDialogFilter() { Name = "All files", Extensions = new List<string>() { "*" } });

            if (await openFileDialog.ShowAsync(App.Current.MainWindow) is string[] filePaths && filePaths.Any())
            {
                e.SelectedFile = filePaths[0];
            }
        }

        private void NotBeforeDatePicker_SelectedDateChanged(object sender, DatePickerSelectedValueChangedEventArgs args)
        {
            var viewModel = ((CertificateSignerViewModel)DataContext!);
            viewModel.NotBefore = args.NewDate!.Value.Date + viewModel.NotBefore.TimeOfDay;
        }

        private void NotBeforeTimePicker_SelectedTimeChanged(object sender, TimePickerSelectedValueChangedEventArgs args)
        {
            var viewModel = ((CertificateSignerViewModel)DataContext!);
            viewModel.NotBefore = viewModel.NotBefore.Date + args.NewTime!.Value;
        }

        private void NotAfterDatePicker_SelectedDateChanged(object sender, DatePickerSelectedValueChangedEventArgs args)
        {
            var viewModel = ((CertificateSignerViewModel)DataContext!);
            viewModel.NotAfter = args.NewDate!.Value.DateTime + viewModel.NotAfter.TimeOfDay;
        }

        private void NotAfterTimePicker_SelectedTimeChanged(object sender, TimePickerSelectedValueChangedEventArgs args)
        {
            var viewModel = ((CertificateSignerViewModel)DataContext!);
            viewModel.NotAfter = viewModel.NotAfter.Date + args.NewTime!.Value;
        }
    }
}
