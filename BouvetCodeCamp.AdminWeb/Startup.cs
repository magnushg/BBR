using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BouvetCodeCamp.AdminWeb.Startup))]
namespace BouvetCodeCamp.AdminWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
