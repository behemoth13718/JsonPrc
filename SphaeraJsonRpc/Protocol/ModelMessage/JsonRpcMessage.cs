using System.Collections.Generic;
using Newtonsoft.Json;
using SphaeraJsonRpc.Protocol.Enums;

namespace SphaeraJsonRpc.Protocol.ModelMessage
{
    public abstract class JsonRpcMessage
    {
        [JsonProperty("jsonrpc", Required = Required.Always, Order = 0)]
        public string Version { get; set; } = "2.0";

        [JsonIgnore]
        public abstract EnumTypeMessage TypeMessage { get; }
    }
}