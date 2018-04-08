using NetCoreBoot.Entity;
using NetCoreBoot.IService;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreBoot.Service
{
    public class COCService: BaseService, ICOCService
    {
        public bool GetCOCLogList(int Id)
        {
            Log_XiaoLei sys_user = this.Query<Log_XiaoLei>().Where(x => x.Id>= Id).FirstOrDefault();
            return false;
        }

       
    }
}
