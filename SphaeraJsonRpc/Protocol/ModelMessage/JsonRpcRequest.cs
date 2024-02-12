using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SphaeraJsonRpc.Protocol.ModelMessage
{
    public class JsonRpcRequest : JsonRpcMessage
    {
        [JsonProperty("id", Required = Required.Always, Order = 1)]
        public object RequestId { get; set; } 
        
        [JsonProperty("method", Required = Required.Always, Order = 2)]
        public string Method { get; set; }
        
        [JsonProperty("params", Required = Required.Always, Order = 3)]
        public object Params { get; set; }
        
        [JsonIgnore]
        public override EnumTypeMessage TypeMessage => EnumTypeMessage.Request;
        
        public override List<T> GetPayload<T>() => ((JArray)Params).ToObject<List<T>>();
    }
}