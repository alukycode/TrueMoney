using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using TrueMoney.Data.Repositories;
using TrueMoney.Services;

namespace TrueMoney.Web.DependencyInjection
{
    public class WindsorComponentInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Classes.FromAssembly(typeof(UserRepository).Assembly)
                    .Where(type => type.Name.EndsWith("Repository"))
                    .WithService.DefaultInterfaces()
                    .LifestyleSingleton());

            container.Register(
                Classes.FromAssembly(typeof(UserProfileService).Assembly)
                    .Where(type => type.Name.EndsWith("Service"))
                    .WithService.DefaultInterfaces() // for class UserProfileSevice it will be interface IUserProfileSevice
                    .LifestyleSingleton());
        }
    }
}