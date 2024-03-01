using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using SphaeraJsonRpc.Extensions;
using SphaeraJsonRpc.Protocol.Enums;
using SphaeraJsonRpc.Protocol.ModelMessage.RequestMessage;

namespace SphaeraJsonRpc.Helpers
{
    public class ParamsHelper
    {
        public static bool ReadObjectParam(JsonRpcRequestServer request, HttpContext context, ParameterInfo[] methodParams,
            JObject obj, object[] inputParams, out object[] empty)
        {
            for (int i = 0; i < methodParams.Length; i++)
                if (obj.TryGetValue(methodParams[i].Name, out JToken value))
                    inputParams[i] = value.ToObject(methodParams[i].ParameterType);

            if (inputParams.Any(x => x == null))
            {
                context.ErrorWriteContext(request, EnumJsonRpcErrorCode.MethodNotFound);
                {
                    empty = Array.Empty<object>();
                    return true;
                }
            }

            empty = new object[] { };
            return false;
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
    }
}
