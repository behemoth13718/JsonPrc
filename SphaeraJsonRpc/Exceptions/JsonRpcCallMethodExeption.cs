using System;

namespace SphaeraJsonRpc.Exceptions
{
    public class JsonRpcCallMethodExeption : Exception
    {
        public string MethodName { get; set; }
        public string OriginalMessage { get; set; }
        public JsonRpcCallMethodExeption(string message, string methodName, string originalMessage)
            : base(message)
        {
            MethodName = methodName;
            OriginalMessage = originalMessage;
        }
    }
}