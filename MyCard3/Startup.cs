using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyCard3.Startup))]
namespace MyCard3
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
