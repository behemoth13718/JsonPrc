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
using SphaeraJsonRpc.Protocol.ModelMessage.RequestMessage;

namespace SphaeraJsonRpc.Extensions
{
    public static class ExtentionsRpcMessages
    {
        public static void HandlerRequest(this HttpContext context,JsonRpcRequestServer request, object requestScopedService)
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
                    context.SuccessWriteContext(request, HttpStatusCode.OK, Constants.MediaType.ApplicationJson, methodResult);
                }
                catch (Exception e)
                {
                    throw new JsonRpcCallMethodExeption("Ошибка при обработке данных в методе", method.NameMethodContract, e?.InnerException?.Message ?? e.Message);
                }
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
            JsonRpcRequestServer request,
            EnumJsonRpcErrorCode enumJsonRpcErrorCode,
            object data = null)
        {
            context.Response.StatusCode = HttpStatusCode.BadRequest.GetCode();
            context.Response.ContentType = Constants.MediaType.ApplicationJson;
            
            var responseMessage = new JsonRpcError()
            {
                RequestId = request?.RequestId ?? 0,
                Error = new ErrorDetail()
                {
                    Code = enumJsonRpcErrorCode,
                    Message = enumJsonRpcErrorCode.GetDescription()
                }
            };
            if (data != null)
                responseMessage.Error.Data = data;
            
            await context.Response.WriteAsync(JsonConvert.SerializeObject(responseMessage));
        }
        
        public static async void SuccessWriteContext(
            this HttpContext context, 
            JsonRpcRequestServer request,
            HttpStatusCode statusCode,
            string mediaType,
            object resultActionData)
        {
            context.Response.StatusCode = statusCode.GetCode();
            context.Response.ContentType = mediaType;
            
            var response = new JsonRpcResult()
            {
                RequestId = request.RequestId,
                Result = resultActionData
            };
            await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }
}