using System;

namespace Whip.ViewModels.Messages
{
    public class HideDialogMessage
    {
        public HideDialogMessage(Guid guid)
        {
            Guid = guid;
        }

        public Guid Guid { get; set; }
    }
}
