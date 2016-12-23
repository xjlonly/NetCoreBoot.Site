using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NetCoreBoot.Admin
{
    public class DIService
    {
        public string ServiceType { get; set; }
        public string ImplementationType { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public ServiceLifetime Lifetime { get; set; }
    }
}
