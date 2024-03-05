using System;
using System.Net;
using SphaeraJsonRpc.Protocol.Enums;
using SphaeraJsonRpc.Protocol.ModelMessage.ErrorMessage;

namespace SphaeraJsonRpc.Exceptions
{
    public class JsonRpcHandlerMessageExeption : Exception
    {
        public HttpStatusCode? ErrorCode { get; set; }
        public string ResponseBody { get; set; }
        public string MessageError { get; set; }
        public JsonRpcError JsonRpcError { get; set; }
        
        public JsonRpcHandlerMessageExeption(string message, string responseBody, JsonRpcError jsonRpcError, HttpStatusCode? errorCode = null, Exception messageError = null) 
            : base(message)
        {
            ErrorCode = errorCode;
            ResponseBody = responseBody;
            MessageError = messageError?.InnerException?.Message ?? messageError.Message;
            JsonRpcError = jsonRpcError;
        }
    }
}