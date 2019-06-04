using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(twright_bugtracker.Startup))]
namespace twright_bugtracker
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
