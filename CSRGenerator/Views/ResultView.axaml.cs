using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CSRGenerator.Views
{
    public class ResultView : UserControl
    {
        public ResultView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
