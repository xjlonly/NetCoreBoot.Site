using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetCoreBoot.Entity;

namespace NetCoreBoot.IService
{
    /// <summary>
    /// 账号相关操作服务
    /// </summary>
    public interface IAccountService : ISysLogService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password">经过MD5加密后的密码</param>
        /// <param name="userInfo"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool CheckLogin(string userName, string password, out Sys_User userInfo, out string msg);
    }
}
