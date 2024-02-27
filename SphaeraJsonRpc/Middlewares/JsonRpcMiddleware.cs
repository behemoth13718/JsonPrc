using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SphaeraJsonRpc.Extensions;
using SphaeraJsonRpc.Protocol.ErrorMessage;
using SphaeraJsonRpc.Protocol.ModelMessage;
using SphaeraJsonRpc.Protocol.ModelMessage.ErrorMessage;
using SphaeraJsonRpc.Protocol.ModelMessage.RequestMessage;

namespace SphaeraJsonRpc.Middlewares
{
    public class JsonRpcMiddleware<TService> where TService : class
    {
        private string _urlPath;
        public JsonRpcMiddleware(RequestDelegate next, string urlPath) =>_urlPath = urlPath;
        
        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.HasValue && string.Equals(context.Request.Path.Value, _urlPath, StringComparison.InvariantCultureIgnoreCase))
            {
                using var reader = new StreamReader(context.Request.Body);
                var reciveMessage = await reader.ReadToEndAsync();
                var serviceProvider = context.RequestServices;
                var request = JsonConvert.DeserializeObject<JsonRpcRequestServer>(reciveMessage);
                var requestScopedService = ActivatorUtilities.CreateInstance<TService>(serviceProvider);
                
                var method = requestScopedService.GetMethod(request);

                var inputParams = await method.HandlerParams(request, context);
                if (inputParams != null) 
                {
                    var methodResult = method.Invoke(requestScopedService, inputParams);

                    var response = new JsonRpcResult()
                    {
                        RequestId = request.RequestId,
                        Result = methodResult
                    };

                    context.Response.StatusCode = 200;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
                }

            }
        }
    }
}