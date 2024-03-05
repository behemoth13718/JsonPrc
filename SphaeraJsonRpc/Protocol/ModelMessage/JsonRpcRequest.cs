using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SphaeraJsonRpc.Protocol.Enums;

namespace SphaeraJsonRpc.Protocol.ModelMessage
{
    public class JsonRpcRequest : JsonRpcMessage
    {
        [JsonProperty("id", Required = Required.Always, Order = 1)]
        public object RequestId { get; set; }
        
        [JsonProperty("method", Required = Required.Always, Order = 2)]
        public string Method { get; set; }
        
        [JsonProperty("params",  Order = 3)]
        public object Params { get; set; }
        
        [JsonIgnore]
        public override EnumTypeMessage TypeMessage => EnumTypeMessage.Request;
        
        public override string ToString()
        {
            return new JObject
            {
                new JProperty("id", RequestId),
                new JProperty("method", Method),
                new JProperty("params", Params),
            }.ToString(Newtonsoft.Json.Formatting.None);
        }
    }
}
