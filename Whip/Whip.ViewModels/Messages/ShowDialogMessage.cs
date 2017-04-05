using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whip.ViewModels.Windows;

namespace Whip.ViewModels.Messages
{
    public class ShowDialogMessage : MessageBase
    {
        public ShowDialogMessage(Guid guid, DialogViewModel viewModel)
        {
            Guid = guid;
            ViewModel = viewModel;
        }

        public Guid Guid { get; set; }
        public DialogViewModel ViewModel { get; set; }
    }
}
