using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SphaeraJsonRpc.Exceptions;
using SphaeraJsonRpc.Extensions;
using SphaeraJsonRpc.Protocol.Enums;
using SphaeraJsonRpc.Protocol.ModelMessage;

namespace SphaeraJsonRpc.Middlewares
{
    public class JsonRpcMiddleware<TService> where TService : class
    {
        private readonly string _urlPath;
        public JsonRpcMiddleware(RequestDelegate next ,string urlPath) =>_urlPath = urlPath;
        
        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.HasValue && string.Equals(context.Request.Path.Value, _urlPath,
                    StringComparison.InvariantCultureIgnoreCase))
            {
                JsonRpcRequest request = null;
                
                context.Response.ContentType = Constants.MediaType.ApplicationJson;
                try
                {
                    using var reader = new StreamReader(context.Request.Body);
                    var reciveMessage = await reader.ReadToEndAsync();
                    var serviceProvider = context.RequestServices;
                    request = JsonConvert.DeserializeObject<JsonRpcRequest>(reciveMessage);
                    var requestScopedService = ActivatorUtilities.CreateInstance<TService>(serviceProvider);

                    context.HandlerRequest(request, requestScopedService);
                }
                catch (JsonSerializationException e)
                {
                    context.ErrorWriteContext(
                        request, EnumJsonRpcErrorCode.ResponseSerializationFailure, e.Message);
                }
                catch (JsonRpcCallMethodExeption e)
                {
                    context.ErrorWriteContext(
                        request, EnumJsonRpcErrorCode.InvocationErrorWithException, $"Error call method {e.MethodName}. Error message: {e.OriginalMessage}");
                }
                catch (Exception e)
                {
                    context.ErrorWriteContext(request, EnumJsonRpcErrorCode.InternalError, e.Message);
                }
            }
        }
    }
}