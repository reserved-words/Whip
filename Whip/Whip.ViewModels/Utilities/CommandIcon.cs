using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using Whip.Common;

namespace Whip.ViewModels.Utilities
{
    public class CommandIcon : ViewModelBase
    {
        private Action<CommandIcon> _action;
        private bool _on;
        private readonly string _offIcon;
        private readonly string _onIcon;
        private readonly string _onToolTip;
        private readonly string _offToolTip;

        public CommandIcon(IconType icon, string toolTip, Action<CommandIcon> action, bool on = true, IconType? offIcon = null, string offToolTip = null)
        {
            _action = action;
            _onIcon = icon.ToString();
            _offIcon = offIcon.HasValue ? offIcon.Value.ToString() : _onIcon;
            _onToolTip = toolTip;
            _offToolTip = !string.IsNullOrEmpty(offToolTip) ? offToolTip : _onToolTip;

            Command = new RelayCommand(OnClick);
            On = on;
        }

        public string Icon => On ? _onIcon : _offIcon;

        public string ToolTip => On ? _onToolTip : _offToolTip;

        public RelayCommand Command { get; private set; }

        public bool On
        {
            get { return _on; }
            set
            {
                Set(ref _on, value);
                RaisePropertyChanged(nameof(Icon));
                RaisePropertyChanged(nameof(ToolTip));
            }
        }

        private void OnClick()
        {
            _action(this);
        }
    }
}
