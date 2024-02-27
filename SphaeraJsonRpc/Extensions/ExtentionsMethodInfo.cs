using System;
using System.Linq;
using SphaeraJsonRpc.Protocol.ModelMessage.RequestMessage;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SphaeraJsonRpc.Protocol.ErrorMessage;
using SphaeraJsonRpc.Protocol.ModelMessage.ErrorMessage;

namespace SphaeraJsonRpc.Extensions
{
    public static class ExtentionsMethodInfo
    {
        public static MethodInfo GetMethod(this object rpcServer, JsonRpcRequestServer request) =>
            rpcServer.GetType()
            .GetMethods()
            .FirstOrDefault(x => string.Equals(x.Name, request.Method, StringComparison.CurrentCultureIgnoreCase));

        public static async Task<object[]> HandlerParams(this MethodInfo method, JsonRpcRequestServer request, HttpContext context)
        {
            // Получаем параметры метода               
            var methodParams = method.GetParameters();
            var @params = request.Params;
            // Создаем массив с параметрами для передачи их в метод
            var inputParams = new object[methodParams.Length];
                
            //Если данные представлены в виде массива данных
            if (@params.IsEmptyParams())
            {
                if (methodParams.Length > 1)
                {
                    context.Response.StatusCode = 400;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(new JsonRpcError()
                    {
                        RequestId = request.RequestId,
                        Error = new ErrorDetail()
                        {
                            Code = JsonRpcErrorCode.MethodNotFound,
                            Message = "Method not found."
                        }
                    }));

                    return inputParams = new object[0];
                }
            }
            else if (@params is JArray jArray && methodParams.Length == 1)
            {
                var paramSingleInfo = methodParams[0];
                var paramType = paramSingleInfo.ParameterType;

                if (paramSingleInfo.IsCollection())
                {
                    var parse = jArray.ToObject(paramType);
                    inputParams[0] = parse;
                }

                inputParams[0] = jArray[0].ToObject(paramType);
            }
            else if (@params is JArray arrayMany && methodParams.Length > 1)
            {
                for (int i = 0; i < methodParams.Length; i++)
                    inputParams[i] = arrayMany[i].ToObject(methodParams[i].ParameterType);
            }
            else if(@params is JObject obj)
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
                            RequestId = request.RequestId,
                            Error = new ErrorDetail()
                            {
                                Code = JsonRpcErrorCode.MethodNotFound,
                                Message = "Method not found."
                            }
                        }));
                        inputParams = null;
                    }
                }
            return inputParams;
        }
    }
}