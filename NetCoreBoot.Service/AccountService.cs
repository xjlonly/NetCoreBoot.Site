using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetCoreBoot.IService;

namespace NetCoreBoot.Service
{
    public class AccountService :IAccountService
    {

        bool IAccountService.CheckLogin(string userName, string password)
        {
            throw new NotImplementedException();
        }
    }
}
