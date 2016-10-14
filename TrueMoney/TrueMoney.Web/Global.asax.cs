using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using TrueMoney.DependencyInjection;
using TrueMoney.Mapping;
using TrueMoney.Web.Auth_Identity_Startup;
using TrueMoney.Web.Mapping;

namespace TrueMoney.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            MapperInitializer.Initialize(new WebMappingProfile());
            InitializeWindsorContainer();
        }

        private void InitializeWindsorContainer()
        {
            IWindsorContainer container = new WindsorContainer();

            container.Install(FromAssembly.Instance(typeof(WindsorComponentInstaller).Assembly));

            // asp.net identity
            // http://tech.trailmax.info/2014/09/aspnet-identity-and-ioc-container-registration/
            // not sure about transient lifestyle
            container.Register(
                Component.For<ApplicationDbContext>().LifestyleTransient(),
                Component.For<ApplicationSignInManager>().LifestyleTransient(),
                Component.For<ApplicationUserManager>().LifestyleTransient(),
                //Component.For<EmailService>().LifestyleTransient(),
                Component.For<IAuthenticationManager>()
                    .UsingFactoryMethod(kernel => HttpContext.Current.GetOwinContext().Authentication)
                    .LifestyleTransient(),
                Component
                    .For<IUserStore<ApplicationUser>>()
                    .ImplementedBy<UserStore<ApplicationUser>>()
                    .DependsOn(Dependency.OnComponent<DbContext, ApplicationDbContext>())
                    .LifestyleTransient());

            // controllers
            container.Register(Classes.FromThisAssembly().BasedOn<IController>().LifestyleTransient());
            var controllerFactory = new WindsorControllerFactory(container.Kernel);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);

            // custom resolver
            DependencyResolver.SetResolver(new WindsorDependencyResolver(container));
        }
    }
}
