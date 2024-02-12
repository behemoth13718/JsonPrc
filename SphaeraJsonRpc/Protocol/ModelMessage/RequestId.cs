using System;
using System.Globalization;
using Newtonsoft.Json;

namespace SphaeraJsonRpc.Protocol.ModelMessage
{
    public class RequestId : IEquatable<RequestId>
    {
        [JsonProperty()]
        public long? Number { get; }
        public string String { get; }
        public bool IsNull { get; }
        
        internal object ObjectValue => (object)this.Number ?? this.String;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="RequestId"/> struct.
        /// </summary>
        /// <param name="id">The ID of the request.</param>
        public RequestId(long id)
        {
            Number = id;
            String = null;
            IsNull = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestId"/> struct.
        /// </summary>
        /// <param name="id">The ID of the request.</param>
        public RequestId(string id)
        {
            String = id;
            Number = null;
            IsNull = id is null;
        }

        
        
        public override bool Equals(object obj) => obj is RequestId other && this.Equals(other);
        public override int GetHashCode() => this.Number?.GetHashCode() ?? this.String?.GetHashCode() ?? 0;
        public override string ToString() => this.Number?.ToString(CultureInfo.InvariantCulture) ?? this.String ?? (this.IsNull ? "(null)" : "(not specified)");
        public bool Equals(RequestId other) => this.Number == other.Number && this.String == other.String && this.IsNull == other.IsNull;
        
        internal static RequestId Parse(object value)
        {
            return
                value is null ? default :
                value is long l ? new RequestId(l) :
                value is string s ? new RequestId(s) :
                value is int i ? new RequestId(i) :
                throw new JsonSerializationException("Unexpected type for id property: " + value.GetType().Name);
        }
    }
}