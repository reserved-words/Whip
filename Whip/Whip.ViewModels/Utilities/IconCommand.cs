using GalaSoft.MvvmLight.Command;
using System;
using Whip.Common;

namespace Whip.ViewModels.Utilities
{
    public class IconCommand
    {
        public IconCommand(IconType icon, string toolTip, Action action)
        {
            Icon = icon.ToString();
            ToolTip = toolTip;
            Command = new RelayCommand(action);
        }

        public string Icon { get; private set; }
        public string ToolTip { get; private set; }
        public RelayCommand Command { get; private set; }
    }
}
