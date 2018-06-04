using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Whip.Web.Startup))]
namespace Whip.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
