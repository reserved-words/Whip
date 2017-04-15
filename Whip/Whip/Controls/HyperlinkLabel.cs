using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Whip.ViewModels.Utilities;

namespace Whip.Controls
{
    public class HyperlinkLabel : Label
    {
        private string _url;

        public HyperlinkLabel()
        {
            Cursor = Cursors.Hand;
            SetUrl();

            MouseUp += HyperlinkImage_MouseUp;
        }

        private void HyperlinkImage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Hyperlink.Go(_url);
        }

        public string Url
        {
            get { return (string)GetValue(UrlProperty); }
            set { SetValue(UrlProperty, value); }
        }

        public static readonly DependencyProperty UrlProperty =
            DependencyProperty.Register("Url", typeof(string), typeof(HyperlinkLabel), new PropertyMetadata(null, new PropertyChangedCallback(OnUrlChanged)));

        public string Username
        {
            get { return (string)GetValue(UsernameProperty); }
            set { SetValue(UsernameProperty, value); }
        }

        public static readonly DependencyProperty UsernameProperty =
            DependencyProperty.Register("Username", typeof(string), typeof(HyperlinkLabel), new PropertyMetadata(new PropertyChangedCallback(OnUsernameChanged)));
        
        public string UrlFormat
        {
            get { return (string)GetValue(UrlFormatProperty); }
            set { SetValue(UrlFormatProperty, value); }
        }

        public static readonly DependencyProperty UrlFormatProperty =
            DependencyProperty.Register("UrlFormat", typeof(string), typeof(HyperlinkLabel), new PropertyMetadata());

        private static void OnUrlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var imageObject = (HyperlinkLabel)d;
            imageObject.SetUrl();
        }

        private static void OnUsernameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var imageObject = (HyperlinkLabel)d;
            imageObject.SetUrl();
        }

        private void SetUrl()
        {
            _url = string.IsNullOrEmpty(Username) ? Url : string.Format(UrlFormat, Username);

            ToolTip = new ToolTip
            {
                Content = _url
            };
        }
    }
}
