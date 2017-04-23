using GalaSoft.MvvmLight.Command;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Whip.ViewModels.TabViewModels;

namespace Whip.Controls
{
    public class ReportingTabControl : TabControl
    {
        public ReportingTabControl()
        {
            Items.CurrentChanging += Items_CurrentChanging;
            SelectionChanged += ReportingTabControl_SelectionChanged;

            IsSynchronizedWithCurrentItem = true;
        }

        public static readonly DependencyProperty SelectedTabProperty =
            DependencyProperty.Register(nameof(SelectedTab), typeof(TabViewModelBase), typeof(ReportingTabControl), new PropertyMetadata(new PropertyChangedCallback(OnSelectedTabChanged)));

        public static readonly DependencyProperty TabChangeCommandProperty =
            DependencyProperty.Register(nameof(TabChangeCommand), typeof(RelayCommand<TabViewModelBase>), typeof(ReportingTabControl), new PropertyMetadata(null));

        public TabViewModelBase SelectedTab
        {
            get { return (TabViewModelBase)GetValue(SelectedTabProperty); }
            set { SetValue(SelectedTabProperty, value); }
        }

        public RelayCommand<TabViewModelBase> TabChangeCommand
        {
            get { return (RelayCommand<TabViewModelBase>)GetValue(TabChangeCommandProperty); }
            set { SetValue(TabChangeCommandProperty, value); }
        }

        private static void OnSelectedTabChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var tabControl = (ReportingTabControl)d;
            tabControl.SelectedItem = e.NewValue;
        }

        private void Items_CurrentChanging(object sender, CurrentChangingEventArgs e)
        {
            if (e.IsCancelable && TabChangeCommand != null && !TabChangeCommand.CanExecute(SelectedItem))
            {
                var item = ((ICollectionView)sender).CurrentItem;
                e.Cancel = true;
                SelectedItem = item;
            }
        }

        private void ReportingTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedTab = (TabViewModelBase)SelectedItem;
        }
    }
}
