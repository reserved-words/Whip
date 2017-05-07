using GalaSoft.MvvmLight.Messaging;
using Whip.Common;
using Whip.Common.Model.Rss;

namespace Whip.ViewModels.Windows
{
    public class EditFeedViewModel : EditableDialogViewModel
    {
        private readonly Feed _originalFeed;

        private string _feedTitle;
        private string _url;
        private string _feedUrl;
        private string _iconUrl;
        private string _color;

        public EditFeedViewModel(IMessenger messenger, Feed feed = null) 
            : base(messenger, feed == null ? "Add New Feed" : "Edit Feed", IconType.Rss)
        {
            _originalFeed = feed ?? new Feed();

            FeedTitle = _originalFeed.Title;
            Url = _originalFeed.Url;
            FeedUrl = _originalFeed.FeedUrl;
            IconUrl = _originalFeed.IconUrl;
            Color = _originalFeed.Color;
        }

        public Feed Feed { get; private set; }

        public string FeedTitle
        {
            set { SetModified(nameof(Title), ref (_feedTitle), value); }
            get { return _feedTitle; }
        }

        public string Url
        {
            set { SetModified(nameof(Url), ref (_url), value); }
            get { return _url; }
        }

        public string FeedUrl
        {
            set { SetModified(nameof(FeedUrl), ref (_feedUrl), value); }
            get { return _feedUrl; }
        }

        public string IconUrl
        {
            set { SetModified(nameof(IconUrl), ref (_iconUrl), value); }
            get { return _iconUrl; }
        }

        public string Color
        {
            set { SetModified(nameof(Color), ref (_color), value); }
            get { return _color; }
        }

        protected override string ErrorMessage => ""; // TO DO 

        protected override bool CustomCancel()
        {
            Feed = _originalFeed;

            return true;
        }

        protected override bool CustomSave()
        {
            Feed = _originalFeed;

            Feed.Title = FeedTitle;
            Feed.Url = Url;
            Feed.FeedUrl = FeedUrl;
            Feed.IconUrl = IconUrl;
            Feed.Color = Color;

            return true;
        }
    }
}
