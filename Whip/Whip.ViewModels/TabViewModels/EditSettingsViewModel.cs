using GalaSoft.MvvmLight.Messaging;
using Whip.Common;
using Whip.Services.Interfaces;
using Whip.ViewModels.TabViewModels.Settings;
using Whip.ViewModels.Utilities;

namespace Whip.ViewModels.TabViewModels
{
    public class EditSettingsViewModel : EditableTabViewModelBase
    {
        public EditSettingsViewModel(IUserSettings userSettings, IMessenger messenger, IFolderDialogService folderDialogService)
            :base(TabType.Settings, IconType.Cog, "Settings", messenger, false) 
        {
            Settings = new SettingsViewModel(folderDialogService, userSettings);

            Reset();
        }

        public override bool Modified
        {
            get { return Settings.Modified; }
            set { Settings.Modified = value; }
        }

        public SettingsViewModel Settings { get; }

        protected override string ErrorMessage => Settings.Error;

        public void Reset()
        {
            Settings?.Reset();
        }

        protected override bool CustomSave()
        {
            Settings.Update();

            return true;
        }

        protected override bool CustomCancel()
        {
            Reset();

            return true;
        }
    }
}
