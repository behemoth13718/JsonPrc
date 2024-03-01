using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SphaeraJsonRpc.Protocol.Enums;

namespace SphaeraJsonRpc.Protocol.ModelMessage.RequestMessage
{
    public class JsonRpcRequestClient : JsonRpcRequest
    {
        [JsonProperty("params", Required = Required.Always, Order = 3)]
        public JToken Params { get; set; }

        [JsonIgnore]
        public override EnumTypeMessage TypeMessage => EnumTypeMessage.Request;

        public override List<T> GetPayload<T>() => ((JArray)Params).ToObject<List<T>>();
    }
}