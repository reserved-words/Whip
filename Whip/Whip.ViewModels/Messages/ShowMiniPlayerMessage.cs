using Whip.ViewModels.Windows;

namespace Whip.ViewModels.Messages
{
    public class ShowMiniPlayerMessage
    {
        public ShowMiniPlayerMessage(MiniPlayerViewModel miniPlayerViewModel)
        {
            MiniPlayerViewModel = miniPlayerViewModel;
        }

        public MiniPlayerViewModel MiniPlayerViewModel { get; }
    }
}
