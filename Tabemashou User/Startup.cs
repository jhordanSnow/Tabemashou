using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Tabemashou_User.Startup))]
namespace Tabemashou_User
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
