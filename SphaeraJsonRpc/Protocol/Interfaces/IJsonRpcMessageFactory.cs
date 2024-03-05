using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using SphaeraJsonRpc.Protocol.ModelMessage;
using SphaeraJsonRpc.Protocol.ModelMessage.ErrorMessage;

namespace SphaeraJsonRpc.Protocol.Interfaces
{
    public interface IJsonRpcMessageFactory
    {
        /// <summary>
        /// Creates an instance of <see cref="JsonRpcRequestServer"/> suitable for transmission over the <see cref="IJsonRpcMessageFormatter"/>.
        /// </summary>
        /// <returns>An instance of <see cref="JsonRpcRequestServer"/>.</returns>
        JsonRpcRequest CreateRequestMessage(string method, object @params, object idMessage);
    }
}