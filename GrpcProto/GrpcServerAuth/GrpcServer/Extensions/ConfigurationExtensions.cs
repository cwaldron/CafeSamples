using GrpcServer.Support;
using Microsoft.Extensions.Configuration;

namespace GrpcServer.Extensions
{
    public static class ConfigurationExtensions
    {
        public static AppSettings GetAppSettings(this IConfiguration config)
        {
            return new AppSettings { Secret = config.GetSection("AppSettings")["Secret"] };
        }
    }
}