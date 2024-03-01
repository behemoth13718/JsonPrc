using System.ComponentModel;
using SphaeraJsonRpc.Attributes;

namespace SphaeraJsonRpc.Protocol.Enums
{
    public enum EnumJsonRpcErrorCode : int
    {
        /// <summary>
        /// Invalid JSON was received by the server. An error occurred on the server while parsing the JSON text.
        /// </summary>
        [Description("Parse error.")]
        [ErrorEnumDescription("Invalid JSON was received by the server. An error occurred on the server while parsing the JSON text.")]
        ParseError = -32700,

        /// <summary>
        /// The JSON sent is not a valid Request object.
        /// </summary>
        [Description("Invalid Request.")]
        [ErrorEnumDescription("The JSON sent is not a valid Request object.")]
        InvalidRequest = -32600,

        /// <summary>
        /// The method does not exist / is not available.
        /// </summary>
        [Description("Method not found.")]
        [ErrorEnumDescription("The method does not exist / is not available.")]
        MethodNotFound = -32601,

        /// <summary>
        /// Invalid method parameter(s).
        /// </summary>
        [Description("Invalid params.")]
        [ErrorEnumDescription("Invalid method parameter(s).")]
        InvalidParams = -32602,

        /// <summary>
        /// Internal JSON-RPC error.
        /// </summary>
        [Description("Internal error.")]
        [ErrorEnumDescription("Internal JSON-RPC error.")]
        InternalError = -32603,

        /// <summary>
        /// Execution of the server method was aborted due to a cancellation request from the client.
        /// </summary>
        [Description("Request canceled.")]
        [ErrorEnumDescription("Execution of the server method was aborted due to a cancellation request from the client.")]
        RequestCanceled = -32800,
        
        /// <summary>
        /// Indicates a response could not be serialized as the application intended.
        /// </summary>
        [Description("Serialization failure.")]
        [ErrorEnumDescription("Indicates a response could not be serialized as the application intended.")]
        ResponseSerializationFailure = -32003,

        /// <summary>
        /// Indicates the RPC call was made but the target threw an exception.
        /// The <see cref="JsonRpcError.ErrorDetail.Data"/> included in the <see cref="JsonRpcError.Error"/> should be interpreted as an <see cref="Exception"/> serialized via <see cref="ISerializable"/>.
        /// </summary>
        [Description("Error call method with exeption.")]
        [ErrorEnumDescription("Indicates the RPC call was made but the target threw an exception.")]
        InvocationErrorWithException = -32004,
        
        /// <summary>
        /// Execution of the server method was aborted due to a cancellation request from the client.
        /// </summary>
        [Description("Protocol not supported.")]
        [ErrorEnumDescription("Execution of the server method was aborted due to a cancellation request from the client.")]
        ProtocolNotSupported = -32005,
    }
}