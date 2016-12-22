using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreBoot.IServiceFactory
{
    public interface IServiceProvider : IDisposable
    {
        /// <summary>
        /// 创建服务接口，由服务工厂实现
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Managed"></param>
        /// <returns></returns>
        T CreateService<T>(bool Managed = true) where T : IAppService;
    }
}
