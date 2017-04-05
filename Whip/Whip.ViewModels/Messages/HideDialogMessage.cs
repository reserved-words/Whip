using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whip.ViewModels.Messages
{
    public class HideDialogMessage : MessageBase
    {
        public HideDialogMessage(Guid guid)
        {
            Guid = guid;
        }

        public Guid Guid { get; set; }
    }
}
