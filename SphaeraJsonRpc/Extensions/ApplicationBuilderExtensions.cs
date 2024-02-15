using Microsoft.AspNetCore.Builder;

namespace SphaeraJsonRpc.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseJsonRpc<T>(this IApplicationBuilder app, string path) where T : class
        {
            //do something

            return app;
        }
    }
}