using System;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Whip.LastFm;
using Whip.Services.Interfaces;
using Whip.Services.Interfaces.Singletons;
using Whip.Web.Interfaces;

namespace Whip.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILastFmApiClientService _lastFm;

        public HomeController(IPlaylist playlist, IErrorLoggingService error, IPlaylistService playlistsService, 
            ICloudService cloudService, ITrackRepository trackRepository, ILastFmApiClientService lastFm)
            :base(trackRepository, cloudService, playlist, error)
        {
            _lastFm = lastFm;
        }

        public ActionResult Index()
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView("_Index");
            }

            return View("_Index");
        }

        [HttpPost]
        public JsonResult GetAuthUrl()
        {
            return new JsonResult
            {
                Data = new
                {
                    Url = _lastFm.UserApiClient.Authorized
                        ? null
                        : _lastFm.UserApiClient.AuthUrl
                }
            };
        }

        [HttpPost]
        public async Task<HttpStatusCodeResult> Authorize()
        {
            await _lastFm.AuthorizeUserClient();
            var sessionKey = _lastFm.UserApiClient.SessionKey;
            Response.SetCookie(
                new HttpCookie("SK", sessionKey)
                {
                    Expires = DateTime.Now.AddDays(30),
                    HttpOnly = true,
                    Secure = true
                });
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}