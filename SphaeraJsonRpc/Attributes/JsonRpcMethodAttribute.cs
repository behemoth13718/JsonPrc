using System;

namespace SphaeraJsonRpc.Attributes
{
    public class JsonRpcMethodAttribute : Attribute
    {
        public string Name { get; set; }
    }
}