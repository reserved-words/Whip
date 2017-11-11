using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Interop;
using Whip.ViewModels.Windows;

namespace Whip.View.Windows
{
    /// <summary>
    /// Interaction logic for MiniPlayerWindow.xaml
    /// </summary>
    public partial class MiniPlayerWindow : Window
    {
        public const int WM_SYSCOMMAND = 0x112;
        public const int SC_MAXIMIZE = 0xF030;

        private bool returningToMainPlayer = false;

        public MiniPlayerWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            HwndSource source = HwndSource.FromHwnd(new WindowInteropHelper(this).Handle);
            source?.AddHook(WndProc);
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            if (!returningToMainPlayer)
            {
                Application.Current.MainWindow.Close();
            }
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_SYSCOMMAND)
            {
                if (wParam.ToInt32() == SC_MAXIMIZE)
                {
                    handled = true;
                    returningToMainPlayer = true;
                    var miniMusicPlayerViewModel = DataContext as MiniPlayerViewModel;
                    miniMusicPlayerViewModel?.OnReturnToMainPlayer();
                    returningToMainPlayer = false;
                }
            }

            return IntPtr.Zero;
        }
    }
}
