using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SphaeraJsonRpc.Exceptions;
using SphaeraJsonRpc.Protocol.Enums;
using SphaeraJsonRpc.Protocol.ModelMessage;
using SphaeraJsonRpc.Protocol.ModelMessage.ErrorMessage;

namespace SphaeraJsonRpc.Extensions
{
    public static class ExtentionsRpcMessages
    {
        public static void HandlerRequest(this HttpContext context,JsonRpcRequest request, object requestScopedService)
        {
            if (!request.IsVersionSuported())
            {
                context.ErrorWriteContext(request, EnumJsonRpcErrorCode.ProtocolNotSupported,$"Supported version protocol: {Constants.Version}");
                return;
            }

            var method = requestScopedService.GetMethod(request);
            if (method is null)
            {
                context.ErrorWriteContext(request, EnumJsonRpcErrorCode.MethodNotFound);
                return;
            }

            var inputParams =  method.MethodInfo.ReadMethodParams(request, context);
            if (inputParams != null)
            {
                try
                {
                    var methodResult = method.MethodInfo.Invoke(requestScopedService, inputParams);
                    context.SuccessWriteContext(request, methodResult);
                }
                catch (Exception e)
                {
                    throw new JsonRpcCallMethodExeption("Ошибка при обработке данных в методе", method.NameMethodContract, e?.InnerException?.Message ?? e.Message);
                }
            }
        }
        
        /// <summary>
        /// Метод расширения для получения результата из сообщения типа JsonRpcResult. Необходимо передать тип данных,
        /// в который нужно преобразовать полученные данные. Если данные не преобразуется, вернётся значение по умолчанию для переданного типа TResult
        /// </summary>
        /// <param name="jsonRpcMessage"></param>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public static TResult TryReadJsonRpcResultMessage<TResult>(this JsonRpcResult jsonRpcMessage)
        {
            try
            {
                object resultData = jsonRpcMessage.Result;
                if (resultData.IsEmptyParams())
                    return default;

                switch (resultData)
                {
                    case JObject obj:
                        return (obj).ToObject<TResult>();
                    case JArray jArray:
                        return (jArray).ToObject<TResult>();
                    case TResult result:
                        return result;
                    default:
                        return default;
                }
            }
            catch (Exception e)
            {
                return default;
            }
            
        }

        public static bool IsEmptyParams(this object @params) => @params switch
            {
                null => true,
                JArray jArray => jArray.Count == 0,
                _ => false
            };

        public static bool IsCollection(this ParameterInfo parameterInfo)
        {
            var parameterType = parameterInfo.ParameterType;
            if (parameterType.IsGenericType && parameterType.GetGenericTypeDefinition() == typeof(ICollection<>))
            {
                return true;
            }
            else if (parameterType.GetInterface(nameof(ICollection)) != null)
            {
                return true;
            }
            return false;
        }

        public static async void ErrorWriteContext(
            this HttpContext context, 
            JsonRpcRequest request,
            EnumJsonRpcErrorCode enumJsonRpcErrorCode,
            object data = null)
        {
            var responseMessage = new JsonRpcError()
            {
                RequestId = request?.RequestId ?? new RequestId(0),
                Error = new ErrorDetail()
                {
                    Code = enumJsonRpcErrorCode,
                    Message = enumJsonRpcErrorCode.GetDescription()
                }
            };
            if (data != null)
                responseMessage.Error.Data = data;
            context.Response.StatusCode = HttpStatusCode.BadRequest.GetCode();
            await context.Response.WriteAsync(JsonConvert.SerializeObject(responseMessage));
        }

        private static async void SuccessWriteContext(
            this HttpContext context, 
            JsonRpcRequest request,
            object resultActionData)
        {
            var response = new JsonRpcResult()
            {
                RequestId = request.RequestId,
                Result = resultActionData
            };
            context.Response.StatusCode = HttpStatusCode.OK.GetCode();
            await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }
}