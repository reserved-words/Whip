using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using Whip.Common;
using Whip.ViewModels.Messages;
using Whip.ViewModels.Utilities;

namespace Whip.ViewModels.SidebarViewModels
{
    public class SettingsIconsViewModel
    {
        private readonly IMessenger _messenger;

        public SettingsIconsViewModel(IMessenger messenger)
        {
            _messenger = messenger;

            PopulateIconCommands();
        }

        public List<IconCommand> IconCommands { get; private set; }

        private void PopulateIconCommands()
        {
            IconCommands = new List<IconCommand>
            {
                new IconCommand(IconType.Headphones, "Scrobbling", OnToggleScrobbling),
                new IconCommand(IconType.Random, "Shuffle", OnToggleShuffle),
                new IconCommand(IconType.VolumeOff, "Mute", OnMute),
                new IconCommand(IconType.VolumeDown, "Volume Down", OnVolumeDown),
                new IconCommand(IconType.VolumeUp, "Volume Up", OnVolumeUp),
                new IconCommand(IconType.Cog, "Settings", OnSettings),
                new IconCommand(IconType.PaintBrush, "Switch Colour", OnSwitchColour)
            };
        }

        private void OnSwitchColour()
        {
            // throw new NotImplementedException();
        }

        private void OnSettings()
        {
            _messenger.Send(new EditSettingsMessage());
        }

        private void OnVolumeUp()
        {
            // throw new NotImplementedException();
        }

        private void OnVolumeDown()
        {
            // throw new NotImplementedException();
        }

        private void OnMute()
        {
            // throw new NotImplementedException();
        }

        private void OnToggleShuffle()
        {
            // throw new NotImplementedException();
        }

        private void OnToggleScrobbling()
        {
            // throw new NotImplementedException();
        }
    }
}
