using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using SphaeraJsonRpc.Protocol.ErrorMessage;
using SphaeraJsonRpc.Protocol.Interfaces;
using SphaeraJsonRpc.Protocol.ModelMessage;
using SphaeraJsonRpc.Protocol.ModelMessage.ErrorMessage;
using SphaeraJsonRpc.Protocol.ModelMessage.RequestMessage;

namespace SphaeraJsonRpc.Protocol.Implements
{
    public class JsonRpcMessageCreator : IJsonRpcMessageFactory
    {
        public JsonRpcRequestServer CreateRequestServerMessage(string method, JArray @params, RequestId idMessage) => new JsonRpcRequestServer()
        {
            Method = method,
            Params = @params ?? new JArray(),
            RequestId = idMessage.ObjectValue
        };

        public JsonRpcRequestClient CreateRequestClientMessage(string method, IReadOnlyList<object> @params, RequestId idMessage)
        {
            throw new NotImplementedException();
        }
        public JsonRpcError CreateErrorMessage()
        {
            throw new System.NotImplementedException();
        }

        // public JsonRpcResult CreateResultMessage()
        // {
        //     throw new System.NotImplementedException();
        // }
    }
}