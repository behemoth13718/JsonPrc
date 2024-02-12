using System.Collections.Generic;
using SphaeraJsonRpc.Protocol.ErrorMessage;
using SphaeraJsonRpc.Protocol.ModelMessage;
using SphaeraJsonRpc.Protocol.ModelMessage.ErrorMessage;

namespace SphaeraJsonRpc.Protocol.Interfaces
{
    public interface IJsonRpcMessageFactory
    {
        /// <summary>
        /// Creates an instance of <see cref="JsonRpcRequest"/> suitable for transmission over the <see cref="IJsonRpcMessageFormatter"/>.
        /// </summary>
        /// <returns>An instance of <see cref="JsonRpcRequest"/>.</returns>
        JsonRpcRequest CreateRequestMessage(string method, IReadOnlyList<object> @params, RequestId idMessage);

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