using Newtonsoft.Json;

namespace SphaeraJsonRpc.Protocol.ModelMessage.RequestMessage
{
    public abstract class JsonRpcRequest : JsonRpcMessage
    {
        [JsonProperty("id", Required = Required.Always, Order = 1)]
        public object RequestId { get; set; }
        
        [JsonProperty("method", Required = Required.Always, Order = 2)]
        public string Method { get; set; }
    }
}
