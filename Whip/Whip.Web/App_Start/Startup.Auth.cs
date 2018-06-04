using System.Configuration;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;

namespace Whip.Web
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            Configure(app);
        }

        private static void Configure(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType("GoogleCookie");

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "GoogleCookie"
            });

            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions
            {
                ClientId = ConfigurationManager.AppSettings["GoogleOAuthClientID"],
                ClientSecret = ConfigurationManager.AppSettings["GoogleOAuthClientSecret"]
            });
        }
    }
}