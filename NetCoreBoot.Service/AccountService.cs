using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetCoreBoot.IService;
using NetCoreBoot.Entity;
using Chloe;

namespace NetCoreBoot.Service
{
    public class AccountService : ServiceBase, IAccountService
    {

        bool IAccountService.CheckLogin(string userName, string password, out string Userid)
        {
            IQuery<Sys_User> q = this.DbContext.Query<Sys_User>();
            Sys_User sys_user = q.Where(x => x.F_Account == userName).FirstOrDefault();
            Userid = sys_user?.F_Id;
            return true;
        }

        
    }
}
