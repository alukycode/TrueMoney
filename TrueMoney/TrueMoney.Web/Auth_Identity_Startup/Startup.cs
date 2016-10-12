using Microsoft.Owin;
using Owin;
using TrueMoney.Web.Auth_Identity_Startup;

[assembly: OwinStartup(typeof(Startup))]
namespace TrueMoney.Web.Auth_Identity_Startup
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
