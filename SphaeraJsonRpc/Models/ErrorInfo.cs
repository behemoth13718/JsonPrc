using System.Collections.Generic;

namespace SphaeraJsonRpc.Models
{
    public class ErrorInfo
    {
        public IEnumerable<ErrorInfoDetail> ErrorInfoDetail { get; set; }
    }
}