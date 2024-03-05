using System;
using System.Globalization;
using Newtonsoft.Json;

namespace SphaeraJsonRpc.Protocol.ModelMessage
{
    public class RequestId
    {
        public object Id { get; set; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="RequestId"/> struct.
        /// </summary>
        /// <param name="id">The ID of the request.</param>
        public RequestId(object id)
        {
            if(id is int)
                Id = (int)id;
            else if(id is string)
                Id = id.ToString();
            else
                Id = 0;
        }
    }
}