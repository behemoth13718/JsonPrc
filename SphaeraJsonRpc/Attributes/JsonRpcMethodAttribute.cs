using System;

namespace SphaeraJsonRpc.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class JsonRpcMethodAttribute : Attribute
    {
        public string Name { get; set; }

        public JsonRpcMethodAttribute(string name)
        {
            Name = name;
        }
    }
}