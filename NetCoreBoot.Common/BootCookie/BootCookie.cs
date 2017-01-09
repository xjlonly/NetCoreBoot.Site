using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreBoot.Common
{
    public class BootCookie<T> : IBootCookie
    {
        //用户标识
        public T UserId { get; set; }
    }
}
