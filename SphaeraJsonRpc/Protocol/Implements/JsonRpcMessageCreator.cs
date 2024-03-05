using System;
using SphaeraJsonRpc.Protocol.Interfaces;
using SphaeraJsonRpc.Protocol.ModelMessage;
using SphaeraJsonRpc.Protocol.ModelMessage.ErrorMessage;

namespace SphaeraJsonRpc.Protocol.Implements
{
    public class JsonRpcMessageCreator : IJsonRpcMessageFactory
    {
        public JsonRpcRequest CreateRequestMessage(string method, object @params, object idMessage) =>
            new JsonRpcRequest()
            {
                RequestId = new RequestId(idMessage).Id,
                Method = method,
                Params = @params
            };
    }
}