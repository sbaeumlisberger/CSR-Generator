using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using CSRGenerator.ViewModels;
using CSRGenerator.Views;

namespace CSRGenerator
{
    public class App : Application
    {
        public static new App Current => (App)Application.Current;

        public MainWindow MainWindow => (MainWindow)((IClassicDesktopStyleApplicationLifetime)ApplicationLifetime).MainWindow;

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
