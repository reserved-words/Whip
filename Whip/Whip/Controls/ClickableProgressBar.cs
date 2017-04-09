using GalaSoft.MvvmLight.Command;
using System.Windows;
using System.Windows.Controls;

namespace Whip.Controls
{
    public class ClickableProgressBar : ProgressBar
    {
        public ClickableProgressBar()
        {
            MouseLeftButtonUp += ClickableProgressBar_MouseLeftButtonUp;
        }

        private void ClickableProgressBar_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var mousePosition = e.GetPosition(this).X;
            var percentage = 100 * mousePosition / ActualWidth;

            if (Command == null || !Command.CanExecute(percentage))
                return;

            Command.Execute(percentage);
        }

        public RelayCommand<double> Command
        {
            get { return (RelayCommand<double>)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(nameof(Command), typeof(RelayCommand<double>), typeof(ClickableProgressBar), new PropertyMetadata(null));
    }
}
