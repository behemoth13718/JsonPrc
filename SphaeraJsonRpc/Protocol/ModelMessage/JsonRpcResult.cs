﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SphaeraJsonRpc.Protocol.Enums;

namespace SphaeraJsonRpc.Protocol.ModelMessage
{
    public class JsonRpcResult : JsonRpcMessage
    {
        [JsonProperty("result", Required = Required.Always, Order = 1)]
        public object Result { get; set; }
        
        [JsonProperty("id", Required = Required.Always, Order = 2)]
        public object RequestId { get; set; }


        [JsonIgnore]
        public override EnumTypeMessage TypeMessage => EnumTypeMessage.Succsess;

        public override List<T> GetPayload<T>() => ((JArray)Result).ToObject<List<T>>();
    }
}