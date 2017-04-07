using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whip.Services.Interfaces;

namespace Whip.Services
{
    public class UserSettingsService : IUserSettingsService
    {
        public bool EssentialSettingsSet => !string.IsNullOrEmpty(MusicDirectory);

        public string MusicDirectory
        {
            get { return Properties.Settings.Default.MusicDirectory; }
            set { Properties.Settings.Default.MusicDirectory = value; }
        }

        public void Save()
        {
            Properties.Settings.Default.Save();
        }
    }
}
