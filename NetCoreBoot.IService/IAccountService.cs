using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreBoot.IService
{
    /// <summary>
    /// 账号相关操作服务
    /// </summary>
    public interface IAccountService
    {
        bool CheckLogin(string userName, string password);
    }
}
