using Whip.Common;
using Whip.Common.Model;
using Whip.ViewModels.Utilities;

namespace Whip.ViewModels.TabViewModels
{
    public class EditTrackViewModel : EditableTabViewModelBase
    {
        private Track _track;

        public EditTrackViewModel()
            : base(TabType.EditTrack, IconType.Edit, "Edit Track", false)
        {

        }

        public Track Track
        {
            get { return _track; }
            set { Set(ref _track, value); }
        }

        public void Edit(Track track)
        {
            Track = track;
            IsVisible = true;
        }

        public override void OnSave()
        {
            IsVisible = false;
        }

        public override void OnCancel()
        {
            IsVisible = false;
        }
    }
}
