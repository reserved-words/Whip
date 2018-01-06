using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using Whip.Common;
using Whip.Common.Model;
using Whip.Common.TagModel;
using Whip.Common.Utilities;
using Whip.Services.Interfaces;
using Whip.ViewModels.Messages;
using Whip.ViewModels.Utilities;
using Whip.ViewModels.Windows;

namespace Whip.ViewModels.TabViewModels
{
    public class PlayHistoryViewModel : TabViewModelBase
    {
        public PlayHistoryViewModel()
            :base(TabType.PlayHistory, IconType.ClockOutline, "History")
        {
        }
    }
}
