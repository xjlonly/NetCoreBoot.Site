using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCoreBoot.Entity;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ManagerBoot.Admin.Controllers
{
    public class CController : Controller
    {

    //    MySQLDBHelper.DapperMySQLHelper dbHelper = new MySQLDBHelper.DapperMySQLHelper();
    //    public IActionResult R()
    //    {
    //        return View();
    //    }

    //    public string GetLastContent(int id)
    //    {
    //        string where = "";
    //        if(id > 0)
    //        {
    //            where = $" where id >={id} " ;
    //        }
    //        string sql = $"SELECT * FROM Log_XiaoLei ORDER BY Id desc  LIMIT 0,10;";
    //        var list = dbHelper.FindToList<Log_XiaoLei>(sql, null, false);
    //        IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
    //        timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
    //        list = list.OrderBy(x => x.Id).ToList();
    //        return Newtonsoft.Json.JsonConvert.SerializeObject(list,timeFormat);
    //    }
    }

}