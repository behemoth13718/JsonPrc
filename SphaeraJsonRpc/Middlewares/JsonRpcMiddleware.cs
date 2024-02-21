using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SphaeraJsonRpc.Protocol.ErrorMessage;
using SphaeraJsonRpc.Protocol.ModelMessage;
using SphaeraJsonRpc.Protocol.ModelMessage.ErrorMessage;
using SphaeraJsonRpc.Protocol.ModelMessage.RequestMessage;

namespace SphaeraJsonRpc.Middlewares
{
    public class JsonRpcMiddleware<TService> where TService : class
    {
        //private readonly RequestDelegate _next;
        private string _urlPath;
        
        public JsonRpcMiddleware(RequestDelegate next, string urlPath)
        {
            //_next = next;
            _urlPath = urlPath;
        }
        
        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.HasValue && string.Equals(context.Request.Path.Value, _urlPath, StringComparison.InvariantCultureIgnoreCase))
            {
                using var reader = new StreamReader(context.Request.Body);
                var reciveMessage = await reader.ReadToEndAsync();
                var serviceProvider = context.RequestServices;
                var requst = JsonConvert.DeserializeObject<JsonRpcRequestServer>(reciveMessage);
                var requestScopedService = ActivatorUtilities.CreateInstance<TService>(serviceProvider);

                var method = requestScopedService
                    .GetType()
                    .GetMethods()
                    .FirstOrDefault(x => string.Equals(x.Name, requst.Method, StringComparison.CurrentCultureIgnoreCase));

                // Получаем параметры метода               
                var methodParams = method.GetParameters();
                var methodReturnType = method.ReturnType;
                // Создаем массив с параметрами для передачи их в метод
                var inputParams = new object[methodParams.Length];
                
                var paramSingleInfo = methodParams[0];
                var paramType = paramSingleInfo.ParameterType;

                //Если данные представлены в виде массива данных
                if (IsEmptyParams(requst.Params))
                {
                    if (methodParams.Length > 1)
                    {
                        context.Response.StatusCode = 400;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new JsonRpcError()
                        {
                            RequestId = requst.RequestId,
                            Error = new ErrorDetail()
                            {
                                Code = JsonRpcErrorCode.MethodNotFound,
                                Message = "Method not found."
                            }
                        }));
                        return;
                    }

                }



                if (requst.Params is JArray jArray && methodParams.Length == 1)
                {
                    if (IsCollection(paramSingleInfo))
                    {
                        var parse = jArray.ToObject(paramType);
                        inputParams[0] = parse;
                    }

                    inputParams[0] = jArray[0].ToObject(paramType);
                }
                else if (requst.Params is JArray arrayMany && methodParams.Length > 1)
                {
                    for (int i = 0; i < methodParams.Length; i++)
                       inputParams[i] = arrayMany[i].ToObject(methodParams[i].ParameterType);
                }
                else if(requst.Params is JObject obj)
                {
                    for (int i = 0; i < methodParams.Length; i++)
                        if (obj.TryGetValue(methodParams[i].Name, out JToken value))
                            inputParams[i] = value.ToObject(methodParams[i].ParameterType);

                    if (inputParams.Any(x => x == null))
                    {
                        context.Response.StatusCode = 400;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new JsonRpcError()
                        {
                            RequestId = requst.RequestId,
                            Error = new ErrorDetail()
                            {
                                Code = JsonRpcErrorCode.MethodNotFound,
                                Message = "Method not found."
                            }
                        }));
                        return;
                    }

                }
                
                var methodResult = method.Invoke(requestScopedService, inputParams);

                var response = new JsonRpcResult()
                {
                    RequestId = requst.RequestId,
                    Result = methodResult
                };

                context.Response.StatusCode = 200;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
            }
        }
        public static dynamic Cast(dynamic obj, Type castTo)
        {
            return Convert.ChangeType(obj, castTo);
        }
        private static bool IsCollection(ParameterInfo parameterInfo)
        {
            Type parameterType = parameterInfo.ParameterType;
            if (parameterType.IsGenericType && parameterType.GetGenericTypeDefinition() == typeof(ICollection<>))
            {
                return true;
            }
            else if (parameterType.GetInterface(nameof(ICollection)) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool IsEmptyParams(object @params)
        {
            if (@params == null)
                return true;

            else if (@params is JArray jArray) 
                return jArray.Count == 0;

            return false;
        }
    }
}