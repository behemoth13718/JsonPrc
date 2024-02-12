using System.Collections.Generic;
using System.Threading.Tasks;
using SphaeraJsonRpc.Protocol.ModelMessage;

namespace SphaeraJsonRpc.Protocol.Interfaces
{
    public interface IJsonRpc
    {
        Task<JsonRpcMessage> SendMessageAsync(string method, IReadOnlyList<object> @params, RequestId idMessage);
        JsonRpc SetUrlService(string urlService);
    }
}