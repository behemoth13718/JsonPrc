using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SphaeraJsonRpc.Exceptions;
using SphaeraJsonRpc.Options;
using SphaeraJsonRpc.Protocol.Enums;
using SphaeraJsonRpc.Protocol.Interfaces;
using SphaeraJsonRpc.Protocol.ModelMessage;
using SphaeraJsonRpc.Protocol.ModelMessage.ErrorMessage;

namespace SphaeraJsonRpc.Protocol
{
    public class JsonRpc : IJsonRpc
    {
        private readonly HttpClient _httpClient;
        private readonly string _urlService;
        private readonly IJsonRpcMessageFactory _requestMessageFactory;

        public JsonRpc(IHttpClientFactory httpClientFactory, IJsonRpcMessageFactory requestMessageFactory, IOptions<JsonRpcClientOptions> options)
        {
            _httpClient = httpClientFactory?.CreateClient();
            _requestMessageFactory = requestMessageFactory;
            _urlService = options?.Value?.UrlJsonRpcServer;
        }
        
        public async Task<JsonRpcResult> SendMessageAsync(string method, object @params, object idMessage)
        {
            var request =  _requestMessageFactory.CreateRequestMessage(method, @params, idMessage); 
            var requestBody = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(request));
            return await HandlerMessage(requestBody, request);
        }

        private async Task<JsonRpcResult> HandlerMessage(byte[] requestBody, JsonRpcRequest jsonRpcRequest)
        {
            try
            {
                var response = await _httpClient.PostAsync(_urlService, new ByteArrayContent(requestBody));
                var responseBody = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode) 
                    TryGetErrorMessage(responseBody, jsonRpcRequest);

                return TryGetResultMessage(responseBody, jsonRpcRequest);
            }
            catch(JsonRpcHandlerMessageExeption e)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new Exception($"Error sending request: {e.Message}", e?.InnerException);
            }
        }
        private void TryGetErrorMessage(string responseBody, JsonRpcRequest jsonRpcRequest)
        {
            try
            {
                var rpcError = JsonConvert.DeserializeObject<JsonRpcError>(responseBody);
                throw new JsonRpcHandlerMessageExeption("Error parse body by error", responseBody, rpcError);
            }
            catch(JsonRpcHandlerMessageExeption e)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new JsonRpcHandlerMessageExeption(
                    "Parse body by error",
                    responseBody,
                    new JsonRpcError()
                    {
                        Id = jsonRpcRequest.RequestId,
                        Version = jsonRpcRequest.Version,
                        Error = new ErrorDetail()
                        {
                            Code = EnumJsonRpcErrorCode.ParseError,
                            Message = $"Error parse message: {responseBody}"
                        }
                    },
                    messageError: e);
            }
        }
        private JsonRpcResult TryGetResultMessage(string responseBody, JsonRpcRequest jsonRpcRequest)
        {
            try
            {
                return JsonConvert.DeserializeObject<JsonRpcResult>(responseBody);
            }
            catch (Exception e)
            {
                throw new JsonRpcHandlerMessageExeption(
                    "Error parse body by result", 
                    responseBody, 
                    new JsonRpcError()
                    {
                        Id = jsonRpcRequest.RequestId, 
                        Version = jsonRpcRequest.Version, 
                        Error = new ErrorDetail()
                        {
                            Code = EnumJsonRpcErrorCode.ParseError,
                            Message = $"Error parse message: {responseBody}"
                        }
                    }, 
                    messageError: e);
            }
        }
    }
}