using Microsoft.Owin;
using Owin;
using Tabemashou_Admin;

[assembly: OwinStartup(typeof(Startup))]
namespace Tabemashou_Admin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
