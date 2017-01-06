using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NetCoreBoot.Admin
{
    public class DIService
    {
        /// <summary>
        /// 服务类型(服务接口)
        /// </summary>
        public string ServiceType { get; set; }
        /// <summary>
        /// 服务实现类型
        /// </summary>
        public string ImplementationType { get; set; }
        /// <summary>
        /// 服务生命周期
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public ServiceLifetime Lifetime { get; set; }
    }
}
