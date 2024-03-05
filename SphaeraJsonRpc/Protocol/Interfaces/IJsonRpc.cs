using System.Threading.Tasks;
using SphaeraJsonRpc.Protocol.ModelMessage;

namespace SphaeraJsonRpc.Protocol.Interfaces
{
    public interface IJsonRpc
    {
        Task<JsonRpcResult> SendMessageAsync(string method, object @params, object idMessage);
    }
}