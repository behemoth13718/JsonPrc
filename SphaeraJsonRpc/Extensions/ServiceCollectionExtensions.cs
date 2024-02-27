using Microsoft.Extensions.DependencyInjection;
using SphaeraJsonRpc.Protocol;
using SphaeraJsonRpc.Protocol.Implements;
using SphaeraJsonRpc.Protocol.Interfaces;

namespace SphaeraJsonRpc.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJsonRpcClient<T>(this IServiceCollection services) where T : class
        {
            services.AddTransient<IJsonRpc, JsonRpc>();
            services.AddHttpClient();
            services.AddTransient<IJsonRpcMessageFactory, JsonRpcMessageCreator>();

            return services;
        }
    }
}