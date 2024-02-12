using SphaeraJsonRpc.Protocol.ModelMessage.ErrorMessage;

namespace SphaeraJsonRpc.Protocol.ErrorMessage
{
    public enum JsonRpcErrorCode : int
    {
        /// <summary>
        /// Indicates the RPC call was made but the target threw an exception.
        /// The <see cref="JsonRpcError.ErrorDetail.Data"/> included in the <see cref="JsonRpcError.Error"/> is likely to be <see cref="CommonErrorData"/>.
        /// </summary>
        InvocationError = -32000,

        /// <summary>
        /// Indicates a request was made to a remotely marshaled object that never existed or has already been disposed of.
        /// </summary>
        NoMarshaledObjectFound = -32001,

        /* We're skipping -32002 because LSP uses it at the app level: https://github.com/microsoft/vscode-languageserver-node/issues/672 */

        /// <summary>
        /// Indicates a response could not be serialized as the application intended.
        /// </summary>
        ResponseSerializationFailure = -32003,

        /// <summary>
        /// Indicates the RPC call was made but the target threw an exception.
        /// The <see cref="JsonRpcError.ErrorDetail.Data"/> included in the <see cref="JsonRpcError.Error"/> should be interpreted as an <see cref="Exception"/> serialized via <see cref="ISerializable"/>.
        /// </summary>
        InvocationErrorWithException = -32004,

        /// <summary>
        /// Invalid JSON was received by the server. An error occurred on the server while parsing the JSON text.
        /// </summary>
        ParseError = -32700,

        /// <summary>
        /// The JSON sent is not a valid Request object.
        /// </summary>
        InvalidRequest = -32600,

        /// <summary>
        /// The method does not exist / is not available.
        /// </summary>
        MethodNotFound = -32601,

        /// <summary>
        /// Invalid method parameter(s).
        /// </summary>
        InvalidParams = -32602,

        /// <summary>
        /// Internal JSON-RPC error.
        /// </summary>
        InternalError = -32603,

        /// <summary>
        /// Execution of the server method was aborted due to a cancellation request from the client.
        /// </summary>
        RequestCanceled = -32800,
    }
}