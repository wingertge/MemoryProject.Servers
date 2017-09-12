using Autofac;
using Autofac.Builder;
using Autofac.Extras.CommonServiceLocator;
using FaunaDB.Client;
using MemoryApi.Services;
using MemoryApi.Services.Impl;
using MemoryApi.Storage;
using MemoryApi.Storage.Impl;
using Microsoft.Practices.ServiceLocation;

namespace MemoryApi
{
    public class ServiceLocator
    {
        public ServiceLocator()
        {
            this.Build();
        }

        public IServiceLocator Instance { get; private set; }

        private void Build()
        {
            var builder = new ContainerBuilder();

            // Register dependencies.
            builder.RegisterType<AuthService>().As<IAuthService>().InstancePerLifetimeScope();
            builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();

            builder.RegisterType<FaunaDbAuthRepository>().As<IAuthRepository>().InstancePerLifetimeScope();
            builder.RegisterType<FaunaDbUserRepository>().As<IUserRepository>().InstancePerLifetimeScope();

            builder.RegisterType<FaunaClient>().SingleInstance().WithParameter("secret", Config.FaunaSecret)
                .WithParameter("domain", Config.FaunaHost).WithParameter("scheme", Config.FaunaScheme).WithParameter("port", Config.FaunaPort);

            var container = builder.Build();

            // Create service locator.
            var csl = new AutofacServiceLocator(container);

            // Set the service locator created.
            Microsoft.Practices.ServiceLocation.ServiceLocator.SetLocatorProvider(() => csl);

            // Use the service locator.
            this.Instance = Microsoft.Practices.ServiceLocation.ServiceLocator.Current;
        }
    }
}