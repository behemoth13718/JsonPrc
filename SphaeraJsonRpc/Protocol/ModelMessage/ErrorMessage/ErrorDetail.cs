using Newtonsoft.Json;
using SphaeraJsonRpc.Protocol.Enums;

namespace SphaeraJsonRpc.Protocol.ModelMessage.ErrorMessage
{
    public class ErrorDetail
    {
        [JsonProperty("code", Required = Required.Always, Order = 0)]
        public EnumJsonRpcErrorCode Code { get; set; }
        
        [JsonProperty("message", Required = Required.Always, Order = 1)]
        public string Message { get; set; }
        
        [JsonProperty("data", Order = 2, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public object Data { get; set; }
    }
}