using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataProtection;
using Owin;
using TrueMoney.Data.Entities;
using TrueMoney.DependencyInjection;
using TrueMoney.Services.Mapping;
using TrueMoney.Web;
using TrueMoney.Web.Auth_Identity_Startup;

[assembly: OwinStartup(typeof(Startup))]
namespace TrueMoney.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            MapperInitializer.Initialize();

            InitializeWindsorContainer(app);

            ConfigureAuth(app);

            ClientDataTypeModelValidatorProvider.ResourceClassKey = "ErrorMessages";
            DefaultModelBinder.ResourceClassKey = "ErrorMessages";
        }

        private void InitializeWindsorContainer(IAppBuilder app)
        {
            IWindsorContainer container = new WindsorContainer();

            container.Register(Component.For<IDataProtectionProvider>().Instance(app.GetDataProtectionProvider()));
            container.Install(FromAssembly.Containing<WindsorComponentInstaller>());

            //container.Register(
            //    Component.For<ILog>()
            //        .Instance(LogManager.GetLogger("TrueMoney.Logger"))
            //        .LifestyleSingleton());

            // controllers
            container.Register(Classes.FromThisAssembly().BasedOn<IController>().LifestyleTransient());
            var controllerFactory = new WindsorControllerFactory(container.Kernel);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);

            // custom resolver
            DependencyResolver.SetResolver(new WindsorDependencyResolver(container));
        }

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(() => DependencyResolver.Current.GetService<ApplicationUserManager>());

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator
                        .OnValidateIdentity<ApplicationUserManager, User, int>(
                            validateInterval: TimeSpan.FromMinutes(30),
                            regenerateIdentityCallback: (manager, user) => user.GenerateUserIdentityAsync(manager),
                            getUserIdCallback: (id) => id.GetUserId<int>())
                }
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //});
        }
    }
}
