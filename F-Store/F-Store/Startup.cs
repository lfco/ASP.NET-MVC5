using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(F_Store.Startup))]
namespace F_Store
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
