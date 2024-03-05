using System.ComponentModel.DataAnnotations;

namespace SphaeraJsonRpc.Options
{
    public class JsonRpcClientOptions
    {
        [Required]
        [Url]
        public string UrlJsonRpcServer { get; set; }
    }
}