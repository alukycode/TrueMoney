using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using TrueMoney.Data;

namespace TrueMoney.Web.Auth_Identity_Startup
{
    public class IdentityInstaller: IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            // asp.net identity
            // http://tech.trailmax.info/2014/09/aspnet-identity-and-ioc-container-registration/
            // not sure about transient lifestyle
            container.Register(
                Component.For<ApplicationSignInManager>().LifestyleTransient(),
                Component.For<ApplicationUserManager>().LifestyleTransient(),
                //Component.For<EmailService>().LifestyleTransient(),
                Component.For<IAuthenticationManager>()
                    .UsingFactoryMethod(kernel => HttpContext.Current.GetOwinContext().Authentication)
                    .LifestyleTransient(),
                Component
                    .For<IUserStore<ApplicationIdentityUser>>()
                    .ImplementedBy<UserStore<ApplicationIdentityUser>>()
                    .DependsOn(Dependency.OnComponent<DbContext, TrueMoneyContext>())
                    .LifestyleTransient());
        }
    }
}