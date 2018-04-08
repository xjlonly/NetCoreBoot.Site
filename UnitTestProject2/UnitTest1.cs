using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using LogUpLoadService;
namespace UnitTestProject2
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            DBHelper dbHelper = new DBHelper();
            var res = dbHelper.Add<Log_XiaoLei>(new Log_XiaoLei {
                CreateTime = DateTime.Now,
                Content = "测擦擦擦试内容",
                LogTime = DateTime.Now
            });
            string sx = "SELECT * FROM Log_XiaoLei ;";
            var sc =  dbHelper.Query<Log_XiaoLei>(sx);
            Assert.IsTrue(res);
        }

        [TestMethod]
        public void TestMethod2()
        {
            //LogUpLoadBLL bll = new LogUpLoadBLL();
            //bll.StartTasks();
        }
    }
}
