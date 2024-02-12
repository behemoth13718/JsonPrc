using Microsoft.Extensions.DependencyInjection;
using SphaeraJsonRpc.Protocol.Implements;
using SphaeraJsonRpc.Protocol.Interfaces;
using SphaeraJsonRpc.Protocol.ModelMessage.ErrorMessage;

namespace SphaeraJsonRpc.Protocol
{
    public static class Extentions
    {
        public static IServiceCollection AddJsonRpc(this IServiceCollection services)
        {
            services.AddTransient<IJsonRpc, JsonRpc>();
            services.AddHttpClient();
            services.AddTransient<IJsonRpcMessageFactory, JsonRpcMessageCreator>();

            return services;
        }
    }
}