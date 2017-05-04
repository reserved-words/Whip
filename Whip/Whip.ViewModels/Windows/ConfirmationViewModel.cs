using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Whip.Common;

namespace Whip.ViewModels.Windows
{
    public class ConfirmationViewModel : DialogViewModel
    {
        public enum ConfirmationType { YesNo, SaveCancel, OkCancel }

        public ConfirmationViewModel(IMessenger messenger, string title, string text, ConfirmationType type, bool questionIcon = false)
            :base(messenger, title, questionIcon ? IconType.QuestionCircle : IconType.ExclamationTriangle)
        {
            Text = text;

            SetCaptions(type);

            SelectResultCommand = new RelayCommand<bool>(OnSelectResult);
        }

        public bool True => true;
        public bool False => false;
        
        public bool Result { get; private set; }
        public string Text { get; private set; }
        public string TrueCaption { get; private set; }
        public string FalseCaption { get; private set; }

        public RelayCommand<bool> SelectResultCommand { get; private set; }

        private void OnSelectResult(bool result)
        {
            Result = result;
            Close();
        }

        private void SetCaptions(ConfirmationType type)
        {
            switch(type){
                case ConfirmationType.YesNo:
                    TrueCaption = "Yes";
                    FalseCaption = "No";
                    break;
                case ConfirmationType.SaveCancel:
                    TrueCaption = "Save";
                    FalseCaption = "Cancel";
                    break;
                default:
                    TrueCaption = "OK";
                    FalseCaption = "Cancel";
                    break;
            }
        }
    }
}
