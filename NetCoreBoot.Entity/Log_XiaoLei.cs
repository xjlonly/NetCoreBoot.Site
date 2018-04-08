using System;
using Chloe.Entity;

namespace NetCoreBoot.Entity
{
    public class Log_XiaoLei
    {
        [Column(IsPrimaryKey = true)]
        public int Id { get; set; }
        public DateTime? LogTime { get; set; }
        public string Content { get; set; }
        public DateTime CreateTime { get; set; }
     
    }
}
