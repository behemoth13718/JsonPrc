using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SphaeraJsonRpc.Protocol.ModelMessage.RequestMessage;

namespace SphaeraJsonRpc.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseJsonRpc<T>(this IApplicationBuilder app, string path) where T : class
        {
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapPost("/Service", async context =>
                {
                    using var reader = new StreamReader(context.Request.Body);
                    var reciveMessage = await reader.ReadToEndAsync();

                    var requst = JsonConvert.DeserializeObject<JsonRpcRequestServer>(reciveMessage);
                    //var t = myService.Sum(1,2);
                });
            });

            return app;
        }
    }
}