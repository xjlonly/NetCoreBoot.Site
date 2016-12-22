using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreBoot.IServiceFactory
{
    /// <summary>
    /// 服务基础接口，所有服务继承此接口
    /// </summary>
    public interface IAppService : IDisposable
    {
    }
}
