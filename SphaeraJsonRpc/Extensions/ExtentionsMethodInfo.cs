using System;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using SphaeraJsonRpc.Attributes;
using SphaeraJsonRpc.Helpers;
using SphaeraJsonRpc.Models;
using SphaeraJsonRpc.Protocol.Enums;
using SphaeraJsonRpc.Protocol.ModelMessage;

namespace SphaeraJsonRpc.Extensions
{
    public static class ExtentionsMethodInfo
    {
        public static MethodJsonRpcInfo GetMethod(this object rpcServer, JsonRpcRequest request)
        {
            foreach (var method in rpcServer.GetType().GetMethods())
            {
                var attribute = method.GetCustomAttribute<JsonRpcMethodAttribute>();
                if (attribute != null && attribute.Name.Equals(request.Method))
                    return new MethodJsonRpcInfo()
                    {
                        NameMethodLocal = method.Name,
                        NameMethodContract = attribute.Name,
                        MethodInfo = method
                    };
            }
            return null;
        }
        public static bool IsVersionSuported(this JsonRpcRequest request) => request.Version.Equals(Constants.Version); 
        public static object[] ReadMethodParams(this MethodInfo method, JsonRpcRequest request, HttpContext context)
        {
            // Получаем параметры метода               
            var methodParams = method.GetParameters();
            var @params = request.Params;
            // Создаем массив с параметрами для передачи их в метод
            var inputParams = new object[methodParams.Length];
                
            //Когда параметры пустые или отсутствуют в сообщении
            if (@params.IsEmptyParams())
            {
                // Если в методе есть параметры а в сообщении нет, то ошибка 
                if (methodParams.Length >= 1)
                {
                    context.ErrorWriteContext(request, EnumJsonRpcErrorCode.InvalidParams);
                    return null;
                }
                return inputParams;
            }
            else switch (@params)
            {
                // Когда параметр в методе один
                case JArray jArray when methodParams.Length == 1:
                {
                    ParamsHelper.ReadOneParam(methodParams, jArray, inputParams);
                    break;
                }
                // Когда параметров в методе больше чем один
                case JArray arrayMany when methodParams.Length > 1:
                {
                    for (int i = 0; i < methodParams.Length; i++)
                        inputParams[i] = arrayMany[i].ToObject(methodParams[i].ParameterType);
                    break;
                }
                // Если параметры в виде объекта и один параметр
                case JObject obj when methodParams.Length == 1:
                {
                    ParamsHelper.ReadOneParam(methodParams, obj, inputParams);
                    break;
                }
                // Если параметры в виде объекта
                case JObject obj:
                {
                    ParamsHelper.ReadObjectParam(request, context, methodParams, obj, inputParams);
                    break;
                }
            }
            return inputParams;
        }
    }
}