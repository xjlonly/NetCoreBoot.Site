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
        bool IAccountService.CheckLogin(string userName, string passWord, out Sys_User userInfo, out string msg)
        {
            userName.NotNullOrEmpty();
            passWord.NotNullOrEmpty();

            userInfo = null;
            msg = null;
            Sys_User sys_user = this.Query<Sys_User>().Where(x => x.F_Account == userName).FirstOrDefault();
            userInfo = sys_user;
            if (sys_user.NotNull())
            {
              if(sys_user.F_EnabledMark == true)
              {
                    
                    this.Query<UserLogOnEntity>().Where(p => p.F_Id == sys_user.F_Id).FirstOrDefault();
                    
              }
              else
                {
                    msg = "账户被系统锁定,请联系管理员!";
                }
            }
            else
            {
                msg = "账户或密码错误!";
            }
            

            return true;
        }

        
    }
}
