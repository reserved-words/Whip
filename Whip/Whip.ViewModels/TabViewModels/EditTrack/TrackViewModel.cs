using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Whip.Common.Model;
using Whip.ViewModels.Utilities;
using Whip.ViewModels.Validation;
using Whip.Common.Validation;
using System.Collections.ObjectModel;
using static Whip.Resources.Resources;
using System.Linq;

namespace Whip.ViewModels.TabViewModels.EditTrack
{
    public class TrackViewModel : EditableViewModelBase
    {
        private List<string> _allTags;

        private string _title;
        private string _year;
        private string _lyrics;
        private string _newTag;

        public TrackViewModel(Track track, List<string> tags)
        {
            RemoveTagCommand = new RelayCommand<string>(OnRemoveTag);

            AllTags = tags;

            Populate(track);

            Modified = false;
        }
        
        public RelayCommand<string> RemoveTagCommand { get; private set; }

        public List<string> AllTags
        {
            get { return _allTags; }
            set { Set(ref _allTags, value); }
        }

        [Required]
        [MaxLength(TrackValidation.MaxLengthTrackTitle, ErrorMessageResourceName = nameof(MaxLengthErrorMessage), ErrorMessageResourceType = typeof(Resources.Resources))]
        public string Title
        {
            get { return _title; }
            set { SetModified(nameof(Title), ref _title, value); }
        }

        [Required]
        [Year]
        [Display(Name = "Track Year")]
        public string Year
        {
            get { return _year; }
            set { SetModified(nameof(Year), ref _year, value); }
        }

        [MaxLength(TrackValidation.MaxLengthLyrics, ErrorMessageResourceName = nameof(MaxLengthErrorMessage), ErrorMessageResourceType = typeof(Resources.Resources))]
        public string Lyrics
        {
            get { return _lyrics; }
            set { SetModified(nameof(Lyrics), ref _lyrics, value); }
        }

        public ObservableCollection<string> Tags { get; set; }

        [TrackTag]
        public string NewTag
        {
            get { return _newTag; }
            set
            {
                SetModified(nameof(NewTag), ref _newTag, value);
                if (!string.IsNullOrEmpty(value) && TrackTagAttribute.Validate(value, this))
                {
                    Tags.Add(value);
                    SetModified(nameof(NewTag), ref _newTag, string.Empty);
                }
            }
        }

        private void OnRemoveTag(string tag)
        {
            Tags.Remove(tag);
        }

        private void Populate(Track track)
        {
            Title = track.Title;
            Year = track.Year;
            Lyrics = track.Lyrics;

            Tags = new ObservableCollection<string>(track.Tags);
        }

        public void UpdateTrack(Track track)
        {
            track.Title = Title;
            track.Year = Year;
            track.Tags = Tags.ToList();
            track.Lyrics = Lyrics;
        }
    }
}
