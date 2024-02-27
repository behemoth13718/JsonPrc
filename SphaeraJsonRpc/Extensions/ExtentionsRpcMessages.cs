using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace SphaeraJsonRpc.Extensions
{
    public static class ExtentionsRpcMessages
    {
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

    }
}