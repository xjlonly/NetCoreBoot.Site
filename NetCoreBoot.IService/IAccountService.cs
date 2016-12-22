using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetCoreBoot.IServiceFactory;

namespace NetCoreBoot.IService
{
    public interface IAccountService : IAppService
    {
        bool CheckLogin(string userName, string password);
    }
}
