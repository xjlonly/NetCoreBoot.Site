using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetCoreBoot.Entity;

namespace NetCoreBoot.IService
{
    public interface ISysLogService
    {
        //void Log(LogType logtype, string moduleName, bool? result, string description);

        void Log(string userid, string account, string realName, string ip, LogType logtype, string moduleName, bool? result, string description);

        //Task LogSync(LogType logtype, string moduleName, bool? result, string description);

        Task LogSync(string userid, string account, string realName, string ip, LogType logtype, string moduleName, bool? result, string description);
    }
}
