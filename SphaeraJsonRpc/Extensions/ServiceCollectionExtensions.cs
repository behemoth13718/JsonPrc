using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SphaeraJsonRpc.Options;
using SphaeraJsonRpc.Protocol;
using SphaeraJsonRpc.Protocol.Implements;
using SphaeraJsonRpc.Protocol.Interfaces;

namespace SphaeraJsonRpc.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJsonRpcClient(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddOptions<JsonRpcClientOptions>()
                .Bind(configuration.GetSection(nameof(JsonRpcClientOptions)));
            services.AddHttpClient();
            services.AddTransient<IJsonRpcMessageFactory, JsonRpcMessageCreator>();
            services.AddTransient<IJsonRpc, JsonRpc>();

            return services;
        }
    }
}