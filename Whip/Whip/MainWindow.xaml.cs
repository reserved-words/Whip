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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // There's probably a better MVVM way to do this
            ((MainWindowViewModel)DataContext).PopulateLibraryCommand.Execute(null);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // There's probably a better MVVM way to do this
            ((MainWindowViewModel)DataContext).SaveLibraryCommand.Execute(null);
        }
    }
}
