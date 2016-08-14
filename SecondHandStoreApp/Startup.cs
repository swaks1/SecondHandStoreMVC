using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SecondHandStoreApp.Startup))]
namespace SecondHandStoreApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
