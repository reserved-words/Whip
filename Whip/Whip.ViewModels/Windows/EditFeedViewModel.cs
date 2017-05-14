using GalaSoft.MvvmLight.Messaging;
using System.ComponentModel.DataAnnotations;
using Whip.Common;
using Whip.Common.Model;
using Whip.ViewModels.Validation;

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

        [Required]
        [MaxLength(100)]
        [Display(Name = "Title")]
        public string FeedTitle
        {
            set { SetModified(nameof(Title), ref (_feedTitle), value); }
            get { return _feedTitle; }
        }

        [Required]
        [Url]
        [Display(Name = "URL")]
        public string Url
        {
            set { SetModified(nameof(Url), ref (_url), value); }
            get { return _url; }
        }

        [Required]
        [Url]
        [Display(Name = "Feed URL")]
        public string FeedUrl
        {
            set { SetModified(nameof(FeedUrl), ref (_feedUrl), value); }
            get { return _feedUrl; }
        }

        [ImageUrl(ImageType.Jpeg,ImageType.Png,ImageType.Ico)]
        [Display(Name = "Icon URL")]
        public string IconUrl
        {
            set { SetModified(nameof(IconUrl), ref (_iconUrl), value); }
            get { return _iconUrl; }
        }

        [Required]
        [Display(Name = "Color")]
        public string Color
        {
            set { SetModified(nameof(Color), ref (_color), value); }
            get { return _color; }
        }
        
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
