using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreBoot.IService
{
    public interface IUserService : IBaseService, ISysLogService
    {
        //IAccountService Account { get; } 
    }
}
