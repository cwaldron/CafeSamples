using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CafeLib.Identity.Sqlite.Extensions
{
    public static class ServiceProviderExtensions
    {
        public static string GetIdentityConnectionString(this IServiceProvider serviceProvider)
        {
            return GetOperationValue(serviceProvider, "identityConnectionString");
        }

        private static string GetOperationValue(this IServiceProvider serviceProvider, string key)
        {
            var configuration = serviceProvider.GetService<IConfiguration>();
            return configuration.GetSection(configuration.GetValue<string>("operation")).GetValue<string>(key);
        }

        private static IConfigurationSection GetOperationSection(IServiceProvider serviceProvider, string key)
        {
            var configuration = serviceProvider.GetService<IConfiguration>();
            return configuration.GetSection(configuration.GetValue<string>("operation")).GetSection(key);
        }
    }
}
