using System.Collections.Generic;
using Newtonsoft.Json;

namespace SphaeraJsonRpc.Protocol.ModelMessage
{
    public abstract class JsonRpcMessage
    {
        [JsonProperty("jsonrpc", Required = Required.Always, Order = 0)]
        public string Version { get; set; } = "2.0";

        public abstract EnumTypeMessage TypeMessage { get; }
        public virtual List<T> GetPayload<T>() => new List<T>();
    }
}