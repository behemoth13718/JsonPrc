using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SphaeraJsonRpc.Protocol.ErrorMessage;
using SphaeraJsonRpc.Protocol.Interfaces;
using SphaeraJsonRpc.Protocol.ModelMessage;
using SphaeraJsonRpc.Protocol.ModelMessage.ErrorMessage;

namespace SphaeraJsonRpc.Protocol
{
    public class JsonRpc : IJsonRpc
    {
        private readonly HttpClient _httpClient;
        private string _urlService;
        private readonly IJsonRpcMessageFactory _requestMessageFactory;
        

        public JsonRpc(IHttpClientFactory httpClientFactory, IJsonRpcMessageFactory requestMessageFactory)
        {
            _httpClient = httpClientFactory?.CreateClient();
            _requestMessageFactory = requestMessageFactory;
        }

        public JsonRpc SetUrlService(string urlService)
        {
            _urlService = urlService; 
            return this;
        }

        public async Task<JsonRpcMessage> SendMessageAsync(string method, IReadOnlyList<object> @params, RequestId idMessage)
        {
            var request =  _requestMessageFactory.CreateRequestMessage(method, @params, idMessage); 
            
            var requestBody = JsonConvert.SerializeObject(request);
            var buffer = Encoding.UTF8.GetBytes(requestBody);

            var response = await _httpClient.PostAsync(_urlService, new ByteArrayContent(buffer));
            var responseBody = await response.Content.ReadAsStringAsync();

            var jsonObject = JObject.Parse(responseBody);
            
            if(jsonObject.ContainsKey("error"))
                return JsonConvert.DeserializeObject<JsonRpcError>(responseBody);
            
            return JsonConvert.DeserializeObject<JsonRpcResult>(responseBody);
        }
    }

    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }
    }
}