using System.Net;
using System.Web.Mvc;
using Whip.Common.Interfaces;
using Whip.Common.Singletons;
using Whip.Services.Interfaces;
using Whip.Services.Interfaces.Singletons;

namespace Whip.Web.Controllers
{
    public class PlayerController : BaseController
    {
        private readonly IPlayer _player;

        public PlayerController(IPlayer player, ICloudService cloudService, ITrackRepository trackRepository,
            IPlaylist playlist, Library library)
            :base(trackRepository, cloudService, playlist, library)
        {
            _player = player;
        }
        
        [HttpPost]
        public HttpStatusCodeResult Play()
        {
            _player.Play(Playlist.CurrentTrack);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public HttpStatusCodeResult Pause()
        {
            _player.Pause();
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public HttpStatusCodeResult Resume()
        {
            _player.Resume();
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public HttpStatusCodeResult SkipToPercentage(double percentage)
        {
            _player.SkipToPercentage(percentage);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public HttpStatusCodeResult Stop()
        {
            _player.Stop();
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}