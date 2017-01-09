﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;

namespace NetCoreBoot.Entity
{
    public enum LogType
    {
        [Description("其他")]
        Other = 0,

        [Description("登录")]
        Login = 1,

        [Description("退出")]
        Exit = 2,

        [Description("访问")]
        Visit = 3,

        [Description("新增")]
        Add = 4,

        [Description("删除")]
        Delete = 5,

        [Description("修改")]
        Update = 6,

        [Description("提交")]
        Submit = 7,

        [Description("异常")]
        Exception = 8,

    }
}