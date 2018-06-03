using System;
using System.Configuration;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Whip.LastFm;
using Whip.Services.Interfaces;
using Whip.Services.Interfaces.Singletons;
using Whip.Web.Interfaces;
using Whip.Web.Models;

namespace Whip.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILastFmApiClientService _lastFm;

        public HomeController(IPlaylist playlist, IErrorLoggingService error, IPlaylistService playlistsService, 
            ICloudService cloudService, ILastFmApiClientService lastFm)
            :base(cloudService, playlist, error)
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
        public async Task<ActionResult> CheckLastFmAuthorized()
        {
            var lastFmUsername = ConfigurationManager.AppSettings["LastFmUsername"];
            var sessionKey = Request.Cookies["SK"]?.Value;
            if (_lastFm.UserApiClient == null)
            {
                await _lastFm.SetClients(lastFmUsername, sessionKey);
            }

            return _lastFm.UserApiClient.Authorized
                ? new HttpStatusCodeResult(HttpStatusCode.OK)
                : (ActionResult)PartialView("_LastFmAuth", new LastFmAuthViewModel(_lastFm.UserApiClient.AuthUrl));
        }

        [HttpPost]
        public async Task<ActionResult> Authorize()
        {
            await _lastFm.AuthorizeUserClient(1);

            if (!_lastFm.UserApiClient.Authorized)
                return PartialView("_LastFmAuth", new LastFmAuthViewModel(_lastFm.UserApiClient.AuthUrl, true));

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