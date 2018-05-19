using System.Windows;
using System.Windows.Controls;

namespace Whip.Controls
{
    public enum ButtonType { Play, Pause, Previous, Next }

    public class PlayerControlButton : Button
    {
        public PlayerControlButton()
            :base()
        {
            FontFamily = new System.Windows.Media.FontFamily("Webdings");
            FontSize = 14;

            SetContent();
        }

        public ButtonType Type
        {
            get { return (ButtonType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register(nameof(Type), typeof(ButtonType), typeof(PlayerControlButton), new FrameworkPropertyMetadata(ButtonType.Play, new PropertyChangedCallback(OnChangeControlType)));

        public void SetContent()
        {
            switch (Type)
            {
                case ButtonType.Play:
                    Content = "4";
                    break;
                case ButtonType.Pause:
                    Content = ";";
                    break;
                case ButtonType.Previous:
                    Content = "9";
                    break;
                case ButtonType.Next:
                    Content = ":";
                    break;
                default:
                    Content = "";
                    break;
            }
        }

        private static void OnChangeControlType(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var button = (PlayerControlButton)d;
            button.SetContent();
        }

    }
}
