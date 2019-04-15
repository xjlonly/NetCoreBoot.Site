using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Common;
using LogUpLoadService.QQHelper;
using System.IO;
using System.Diagnostics;
using System.Net.Sockets;

namespace LogUpLoadService
{
    public class LogUpLoadBLL
    {
        private static bool threadSafe = true;//线程安全
        private static Log5 log = new Log5();
        private static string toName = System.Configuration.ConfigurationManager.AppSettings["ToName"];
        private static string filePath = System.Configuration.ConfigurationManager.AppSettings["FilePath"];
        private static string destinationFile = @"c:\temp\test.log";
        public static QQChatWindow qq = new QQChatWindow("12345");
        private static readonly object _objx = new object();
        public static DBHelper dbhelper = new DBHelper();
        public static int  errcount = 0;
        public static DateTime  Execute(DateTime time, Socket socketClient)
        {

            try
            {
                bool isrewrite = true; // true=覆盖已存在的同名文件,false则反之
                if (!System.IO.File.Exists(destinationFile))
                {
                    Console.WriteLine(destinationFile + "不存在");
                }
                if (!System.IO.File.Exists(filePath))
                {
                    Console.WriteLine(filePath + "不存在");
                }

                System.IO.File.Copy(filePath, destinationFile, isrewrite);
                return  StartTasks(time, socketClient);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString() + ex.StackTrace);
                Console.WriteLine(ex.ToString());
                return DateTime.Now;
            }
        }

        public static DateTime StartTasks(DateTime time, Socket socketClient)
        {

            DateTime lasttime = time;
            if (!System.IO.File.Exists(destinationFile))
            {
                return DateTime.Now;
            }
            String line = "";
            StringBuilder sb = new StringBuilder();
            //var lastRecord = GetLastRecord();
            string words = "模拟器0已经在运行,跳过";
            string cc = "该账号设置不启动，跳过账号0";
            if (errcount > 10)
            {
                qq.sendQQMessage("12345", "该账号设置不启动，跳过账号0");
                List<string> prs = new List<string>() { "ManagementAgentHost", "MEmu", "MEmuConsole", "MEmuHeadless", "MemuService", "MEmuSVC" };
                Process[] pro = Process.GetProcesses();//获取已开启的所有进程     
                                                       //遍历所有查找到的进程
                for (int i = 0; i < pro.Length; i++)
                {
                    Console.WriteLine(pro[i].ProcessName);
                    //判断此进程是否是要查找的进程
                    if (prs.FindIndex(x => x.Equals(pro[i].ProcessName.ToString().ToLower())) >= 0)
                    {
                        pro[i].Kill();//结束进程
                        log.Info($"杀死进程：{pro[i].ProcessName}");
                    }
                }
                errcount = 0;
            }
            using (StreamReader sr = new StreamReader(destinationFile, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();
                    if (!string.IsNullOrEmpty(line))
                    {
                        string[] sps = line.Split('→');
                        string time1 = sps[0].TrimEnd('0').Trim().TrimEnd(':');
                        DateTime timeflag = DateTime.Parse(DateTime.Now.ToShortDateString() + " " + time1);
                        if (timeflag > time)
                        {
                            if (line.Contains("输入密码"))
                            {
                                qq.sendQQMessage("12345", line);

                                Console.WriteLine($"已发送:{line}");
                            }
                            if (line.Contains(words) || line.Contains(cc))
                            {
                                errcount++;
                            }

                            lasttime = timeflag;
                            Console.WriteLine($"已上传:{line}");
                            dbhelper.Add<Log_XiaoLei>(new Log_XiaoLei()
                            {
                                Content = sps[1],
                                CreateTime = DateTime.Now,
                                LogTime = timeflag,
                            });
                        }
                    }
                    var buffter = Encoding.UTF8.GetBytes($"Heart Beat");
                    var temp = socketClient.Send(buffter);
                }
            }
            return lasttime;
            
        }


        private static Log_XiaoLei GetLastRecord()
        {
            string sql = "SELECT * FROM Log_XiaoLei ORDER BY LogTime DESC LIMIT 0, 1;";
            return dbhelper.ExecuteSqlGetFirst<Log_XiaoLei>(sql);
        }
    }
}