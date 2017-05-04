using GalaSoft.MvvmLight.Messaging;
using Whip.Common;
using Whip.Common.Utilities;

namespace Whip.ViewModels.Windows
{
    public class ProgressBarViewModel : DialogViewModel
    {
        private string caption;
        private int percentage;

        public ProgressBarViewModel(IMessenger messenger, string title, bool isIndeterminate = false)
            : base(messenger, title, IconType.Hourglass2)
        {
            IsIndeterminate = isIndeterminate;
        }

        public bool IsIndeterminate { get; private set; }

        public string Caption
        {
            get { return caption; }
            set { Set(ref caption, value); }
        }

        public int Percentage
        {
            get { return percentage; }
            set { Set(ref percentage, value); }
        }

        public void Update(ProgressArgs args)
        {
            Percentage = args.Percentage;
            Caption = IsIndeterminate 
                ? string.Format("{0} ({1}%)", args.Message, args.Percentage)
                : args.Message;
        }
    }
}
