using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SphaeraJsonRpc.Protocol.ModelMessage.ErrorMessage
{
    public class JsonRpcError : JsonRpcMessage
    {
        [JsonProperty("error", Required = Required.Always, Order = 1)]
        public ErrorDetail Error { get; set; }

        [JsonProperty("id", Required = Required.Always, Order = 2)]
        public object RequestId { get; set; }
        
        [JsonIgnore]
        public override EnumTypeMessage TypeMessage => EnumTypeMessage.Error;
        public override string ToString()
        {
            return new JObject
            {
                new JProperty("id", RequestId),
                new JProperty("error", new JObject
                {
                    new JProperty("code", this.Error?.Code),
                    new JProperty("message", this.Error?.Message),
                }),
            }.ToString(Newtonsoft.Json.Formatting.None);
        }
    }
}