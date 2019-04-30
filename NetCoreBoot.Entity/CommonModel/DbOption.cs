using System;
using System.Collections.Generic;
using System.Text;
using NetCoreBoot.Entity.Enums;

namespace NetCoreBoot.Entity.CommonModel
{
    public class DbOption
    {
        public string DbType { get; set; }

        public string ConnectionString { get; set; }
    }
}
