using Microsoft.Extensions.DependencyInjection;

namespace SphaeraJsonRpc.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJsonRpcClient<T>(this IServiceCollection services) where T : class
        {
            services.AddTransient<T>();

            return services;
        }
    }
}