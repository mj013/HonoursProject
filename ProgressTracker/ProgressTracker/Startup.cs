using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ProgressTracker.Startup))]
namespace ProgressTracker
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
