using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using SphaeraJsonRpc.Protocol.ErrorMessage;
using SphaeraJsonRpc.Protocol.ModelMessage;
using SphaeraJsonRpc.Protocol.ModelMessage.ErrorMessage;
using SphaeraJsonRpc.Protocol.ModelMessage.RequestMessage;

namespace SphaeraJsonRpc.Protocol.Interfaces
{
    public interface IJsonRpcMessageFactory
    {
        /// <summary>
        /// Creates an instance of <see cref="JsonRpcRequestServer"/> suitable for transmission over the <see cref="IJsonRpcMessageFormatter"/>.
        /// </summary>
        /// <returns>An instance of <see cref="JsonRpcRequestServer"/>.</returns>
        JsonRpcRequestClient CreateRequestClientMessage(string method, IReadOnlyList<object> @params, RequestId idMessage);
        
        /// <summary>
        /// Creates an instance of <see cref="JsonRpcRequestServer"/> suitable for transmission over the <see cref="IJsonRpcMessageFormatter"/>.
        /// </summary>
        /// <returns>An instance of <see cref="JsonRpcRequestServer"/>.</returns>
        JsonRpcRequestServer CreateRequestServerMessage(string method, JArray @params, RequestId idMessage);

        /// <summary>
        /// Creates an instance of <see cref="JsonRpcError"/> suitable for transmission over the <see cref="IJsonRpcMessageFormatter"/>.
        /// </summary>
        /// <returns>An instance of <see cref="JsonRpcError"/>.</returns>
        JsonRpcError CreateErrorMessage();

        /// <summary>
        /// Creates an instance of <see cref="JsonRpcResult"/> suitable for transmission over the <see cref="IJsonRpcMessageFormatter"/>.
        /// </summary>
        /// <returns>An instance of <see cref="JsonRpcResult"/>.</returns>
        //JsonRpcResult<TResult> CreateResultMessage();
    }
}