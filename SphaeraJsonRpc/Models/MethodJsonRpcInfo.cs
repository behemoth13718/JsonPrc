using System.Reflection;

namespace SphaeraJsonRpc.Models
{
    public class MethodJsonRpcInfo
    {
        public string NameMethodLocal { get; set; }
        public string NameMethodContract { get; set; }
        public MethodInfo MethodInfo { get; set; }
    }
}