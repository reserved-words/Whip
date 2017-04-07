using System;
using Whip.Common.Utilities;

namespace Whip.ViewModels.Windows
{
    public class ProgressBarViewModel : DialogViewModel
    {
        private string caption;
        private int percentage;

        public ProgressBarViewModel(string title)
            : base(title)
        {

        }

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
            Caption = args.Message;
        }
    }
}
