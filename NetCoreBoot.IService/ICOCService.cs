﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreBoot.IService
{
    public interface ICOCService : IBaseService, ISysLogService
    {
        bool GetCOCLogList(int Id);
    }
}
