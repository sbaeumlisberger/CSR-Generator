using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using System;

namespace CSRGenerator.Views
{
    public class InfoButton : UserControl
    {
        public static readonly StyledProperty<string> InfoProperty = AvaloniaProperty.Register<InfoButton, string>(nameof(Background));

        public string Info { get => GetValue(InfoProperty);  set => SetValue(InfoProperty, value); }

        public InfoButton()
        {
            DataContext = this;
            this.InitializeComponent();            
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
