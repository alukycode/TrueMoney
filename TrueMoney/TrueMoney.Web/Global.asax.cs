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
using TrueMoney.Services.Mapping;
using TrueMoney.Web.Auth_Identity_Startup;

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
            MapperInitializer.Initialize();
            InitializeWindsorContainer();
        }

        private void InitializeWindsorContainer()
        {
            IWindsorContainer container = new WindsorContainer();

            container.Install(
                FromAssembly.Instance(typeof (WindsorComponentInstaller).Assembly),
                FromAssembly.Instance(typeof (IdentityInstaller).Assembly));

            // controllers
            container.Register(Classes.FromThisAssembly().BasedOn<IController>().LifestyleTransient());
            var controllerFactory = new WindsorControllerFactory(container.Kernel);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);

            // custom resolver
            DependencyResolver.SetResolver(new WindsorDependencyResolver(container));
        }
    }
}
