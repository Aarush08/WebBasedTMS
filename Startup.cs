using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebBasedTMS.Startup))]
namespace WebBasedTMS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
