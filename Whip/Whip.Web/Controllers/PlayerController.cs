using System.Net;
using System.Web.Mvc;
using Whip.Common.Interfaces;
using Whip.Services.Interfaces;
using Whip.Services.Interfaces.Singletons;
using Whip.Web.Interfaces;

namespace Whip.Web.Controllers
{
    public class PlayerController : BaseController
    {
        private readonly IPlayer _player;
        private readonly IUpdatePlayProgress _playProgress;

        public PlayerController(IPlayer player, ICloudService cloudService, IPlaylist playlist, IErrorLoggingService logger, IUpdatePlayProgress playProgress)
            :base(cloudService, playlist, logger)
        {
            _player = player;
            _playProgress = playProgress;
        }

        [HttpPost]
        public HttpStatusCodeResult Play(int secondsPlayed)
        {
            _playProgress.StartNewTrack(secondsPlayed, (int)Playlist.CurrentTrack.Duration.TotalSeconds);
            _player.Play(Playlist.CurrentTrack);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public HttpStatusCodeResult Pause(int secondsPlayed)
        {
            _playProgress.UpdateSecondsPlayed(secondsPlayed);
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
            _playProgress.UpdatePercentagePlayed(percentage);
            _player.SkipToPercentage(percentage);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public HttpStatusCodeResult Stop(int secondsPlayed)
        {
            _playProgress.Stop(secondsPlayed);
            _player.Stop();
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}