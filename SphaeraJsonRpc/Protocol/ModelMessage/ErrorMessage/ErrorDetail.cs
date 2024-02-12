using Newtonsoft.Json;
using SphaeraJsonRpc.Protocol.ErrorMessage;

namespace SphaeraJsonRpc.Protocol.ModelMessage.ErrorMessage
{
    public class ErrorDetail
    {
        [JsonProperty("code", Required = Required.Always, Order = 0)]
        public JsonRpcErrorCode Code { get; set; }
        
        [JsonProperty("message", Required = Required.Always, Order = 1)]
        public string Message { get; set; }
        
        [JsonProperty("data", Order = 2, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public object Data { get; set; }
    }
}