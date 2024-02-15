using SphaeraJsonRpc.Attributes;

namespace JsonRpcApp
{
    public class RpcServerService : IRpcService
    {
        public int Sum(int a, int b) => a + b;
    }

    public interface IRpcService
    {
        [JsonRpcMethod(Name = "sum")]
        public int Sum(int a, int b);
    }
}