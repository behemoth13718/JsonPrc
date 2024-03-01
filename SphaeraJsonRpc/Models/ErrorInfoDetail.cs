using Newtonsoft.Json;

namespace SphaeraJsonRpc.Models
{
    /// <summary>
    /// Класс описывающий ошибки, поддерживаемые сервером RPC
    /// </summary>
    public class ErrorInfoDetail
    {
        [JsonProperty("code")]
        public object Code { get; set; }
        
        [JsonProperty("message")]
        public string Message { get; set; }
        
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}