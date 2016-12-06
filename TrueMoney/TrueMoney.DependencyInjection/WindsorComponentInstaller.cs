using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using TrueMoney.Data;
using TrueMoney.Services.Services;

namespace TrueMoney.DependencyInjection
{
    using Bank.BankApi;

    public class WindsorComponentInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<ITrueMoneyContext>().ImplementedBy<TrueMoneyContext>().LifestylePerWebRequest());

            // todo: is it possible to use singleton services with dependency from per-web-request context? do we need it?
            container.Register(
                Classes.FromAssembly(typeof(UserService).Assembly)
                    .Where(type => type.Name.EndsWith("Service"))
                    .WithService.DefaultInterfaces() // for class UserSevice it will be interface IUserSevice
                    .LifestylePerWebRequest());

            container.Register(
                Classes.FromAssembly(typeof(SimpleBankApi).Assembly)
                    .Where(type => type.Name.EndsWith("SimpleBankApi"))
                    .WithService.DefaultInterfaces()
                    .LifestylePerWebRequest());
        }
    }
}