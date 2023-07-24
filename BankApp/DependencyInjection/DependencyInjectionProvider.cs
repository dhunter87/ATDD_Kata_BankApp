using BankApp.Boundaries;
using BankApp.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BankApp.DependencyInjection
{
    public static class DependencyInjectionProvider
    {
        public static IServiceProvider Setup(Action<IServiceCollection> overrides)
        {
            var serviceCollection = new ServiceCollection();

            ConfigureServices(serviceCollection);
            overrides?.Invoke(serviceCollection);

            return serviceCollection.BuildServiceProvider();
        }

        public static IServiceProvider Setup()
        {
            return Setup(null);
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            // put all your real applications registrations here
            serviceCollection.AddTransient<IClientRepository, ClientRepository>();
        }
    }
}