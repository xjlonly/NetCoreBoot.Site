using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetCoreBoot.IService;
using NetCoreBoot.Entity;
using Chloe;
using NetCoreBoot.Common;

namespace NetCoreBoot.Service
{
    public class AccountService : BaseService, IAccountService
    {

        bool IAccountService.CheckLogin(string userName, string password, out Sys_User userInfo, out string msg)
        {
            userName.NotNullOrEmpty();
            password.NotNullOrEmpty();

            userInfo = null;
            msg = null;
            Sys_User sys_user = this.Query<Sys_User>().Where(x => x.F_Account == userName).FirstOrDefault();
            if(sys_user.NotNull())
            {
              if(sys_user.F_EnabledMark)
              {

              }  
            }
            else
            {
                msg = "账户或密码错误!";
            }
            userInfo = sys_user;
            msg = "";
            return true;
        }

        
    }
}
