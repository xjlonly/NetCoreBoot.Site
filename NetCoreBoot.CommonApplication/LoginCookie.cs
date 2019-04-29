using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetCoreBoot.Common;

namespace NetCoreBoot.Core
{
    public class LoginCookie : BootCookie<string>
    {
        public string Account { get; set; }
        public string RealName { get; set; }
        public string DepartmentId { get; set; }
        public string DutyId { get; set; }
        public string RoleId { get; set; }
        public string LoginIP { get; set; }
        public DateTime LoginTime { get; set; }
        public bool IsAdmin { get; set; }
    }
}
