using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using TrueMoney.Data.Repositories;
using TrueMoney.Services;

namespace TrueMoney.DependencyInjection
{
    using Bank.BankApi;

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
                Classes.FromAssembly(typeof(UserService).Assembly)
                    .Where(type => type.Name.EndsWith("Service"))
                    .WithService.DefaultInterfaces() // for class UserSevice it will be interface IUserSevice
                    .LifestyleSingleton());
            container.Register(
                Classes.FromAssembly(typeof(BankApi).Assembly)
                    .Where(type => type.Name.EndsWith("Api"))
                    .WithService.DefaultInterfaces()
                    .LifestyleSingleton());
        }
    }
}