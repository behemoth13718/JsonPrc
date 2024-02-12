using System;
using System.Collections.Generic;
using SphaeraJsonRpc.Protocol.ErrorMessage;
using SphaeraJsonRpc.Protocol.Interfaces;
using SphaeraJsonRpc.Protocol.ModelMessage;
using SphaeraJsonRpc.Protocol.ModelMessage.ErrorMessage;

namespace SphaeraJsonRpc.Protocol.Implements
{
    public class JsonRpcMessageCreator : IJsonRpcMessageFactory
    {
        public JsonRpcRequest CreateRequestMessage(string method, IReadOnlyList<object> @params, RequestId idMessage) => new JsonRpcRequest()
        {
            Method = method,
            Params = @params ?? Array.Empty<object>(),
            RequestId = idMessage.ObjectValue
        };

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