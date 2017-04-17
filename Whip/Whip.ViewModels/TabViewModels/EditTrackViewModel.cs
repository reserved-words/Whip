using Whip.Common;
using Whip.Common.Model;
using Whip.Common.Singletons;
using Whip.ViewModels.Utilities;

namespace Whip.ViewModels.TabViewModels
{
    public class EditTrackViewModel : EditableTabViewModelBase
    {
        private Track _track;
        
        public EditTrackViewModel(TrackViewModel trackViewModel)
            : base(TabType.EditTrack, IconType.Edit, "Edit Track", false)
        {
            TrackViewModel = trackViewModel;

            TrackViewModel.IsModified += OnTrackViewModelModified;
        }

        public TrackViewModel TrackViewModel { get; private set; }

        public void Edit(Track track)
        {
            _track = track;

            TrackViewModel.Populate(track);
        }

        private void OnTrackViewModelModified()
        {
            Modified = true;
        }

        protected override void CustomSave()
        {
            TrackViewModel.UpdateTrack(_track);

            // Save to file using ID3 Tag Service
        }

        protected override void CustomCancel()
        {
            // Do nothing
        }
    }
}
