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

        private CertificateSignerViewModel viewModel;

        public CertificateSignerView()
        {
            AvaloniaXamlLoader.Load(this);
            DataContextChanged += CertificateSignerView_DataContextChanged;
        }

        private void CertificateSignerView_DataContextChanged(object? sender, EventArgs e)
        {
            if (this.viewModel is null && DataContext is CertificateSignerViewModel viewModel)
            {
                this.viewModel = viewModel;

                viewModel.SaveCertificateRequested += ViewModel_SaveCertificateRequested;
                viewModel.OpenDialogRequested += ViewModel_OpenDialogRequested;

                this.FindControl<CalendarDatePicker>("notBeforeDatePicker").SelectedDate = viewModel.NotBefore;
                this.FindControl<TimePicker>("notBeforeTimePicker").SelectedTime = viewModel.NotBefore.TimeOfDay;
                this.FindControl<CalendarDatePicker>("notAfterDatePicker").SelectedDate = viewModel.NotAfter;
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

        private void NotBeforeDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs args)
        {
            var selectedDate = this.FindControl<CalendarDatePicker>("notBeforeDatePicker").SelectedDate;
            viewModel.NotBefore = selectedDate!.Value.Date + viewModel.NotBefore.TimeOfDay;
        }

        private void NotBeforeTimePicker_SelectedTimeChanged(object sender, TimePickerSelectedValueChangedEventArgs args)
        {
            viewModel.NotBefore = viewModel.NotBefore.Date + args.NewTime!.Value;
        }

        private void NotAfterDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs args)
        {
            var selectedDate = this.FindControl<CalendarDatePicker>("notAfterDatePicker").SelectedDate;
            viewModel.NotAfter = selectedDate!.Value.Date + viewModel.NotAfter.TimeOfDay;
        }

        private void NotAfterTimePicker_SelectedTimeChanged(object sender, TimePickerSelectedValueChangedEventArgs args)
        {
            viewModel.NotAfter = viewModel.NotAfter.Date + args.NewTime!.Value;
        }
    }
}
