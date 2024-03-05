using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using SphaeraJsonRpc.Extensions;
using SphaeraJsonRpc.Protocol.Enums;
using SphaeraJsonRpc.Protocol.ModelMessage;

namespace SphaeraJsonRpc.Helpers
{
    public class ParamsHelper
    {
        public static void ReadObjectParam(JsonRpcRequest request, HttpContext context, ParameterInfo[] methodParams,
            JObject obj, object[] inputParams)
        {
            for (int i = 0; i < methodParams.Length; i++)
                if (obj.TryGetValue(methodParams[i].Name, out JToken value))
                    inputParams[i] = value.ToObject(methodParams[i].ParameterType);

            if (inputParams.Any(x => x == null))
            {
                inputParams = new object[] { };
                context.ErrorWriteContext(request, EnumJsonRpcErrorCode.MethodNotFound);
            }

        }

        public static void ReadOneParam(ParameterInfo[] methodParams, JArray jArray, object[] inputParams)
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
        public static void ReadOneParam(ParameterInfo[] methodParams, JObject jObject, object[] inputParams)
        {
            try
            {
                var paramSingleInfo = methodParams[0];
                var paramType = paramSingleInfo.ParameterType;
                inputParams[0] = jObject.ToObject(paramType);
            }
            catch (Exception e)
            {
                inputParams = new object[] { };
            }
        }
    }
}
