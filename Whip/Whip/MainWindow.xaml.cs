using System.Windows;
using Whip.ViewModels;

namespace Whip
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private MainWindowViewModel Context => DataContext as MainWindowViewModel;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Context?.OnLoad();
        }

        public void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Context?.OnExit();
        }
    }
}
