using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TrueMoney.Web.Startup))]
namespace TrueMoney.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
