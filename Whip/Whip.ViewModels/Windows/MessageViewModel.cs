using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Whip.Common;

namespace Whip.ViewModels.Windows
{
    public class MessageViewModel : DialogViewModel
    {
        public MessageViewModel(IMessenger messenger, string title, IconType iconType, string text)
            :base(messenger, title, iconType)
        {
            Text = text;

            OKCommand = new RelayCommand<bool>(OnOK);
        }
        
        public string Text { get; private set; }

        public RelayCommand<bool> OKCommand { get; private set; }

        private void OnOK(bool result)
        {
            Close();
        }
    }
}
