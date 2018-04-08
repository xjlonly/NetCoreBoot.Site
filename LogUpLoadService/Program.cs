using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LogUpLoadService
{
    static class Program
    {
        private static string Ip = System.Configuration.ConfigurationManager.AppSettings["ServerIP"];
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main()
        {
            Console.Title = "同步日志服务！";
            Console.WriteLine("开始运行！");
            DateTime timeflag = DateTime.Now;

            //创建实例
            Socket socketClient = new Socket(SocketType.Stream, ProtocolType.Tcp);
            IPAddress ip = IPAddress.Parse(Ip);
            IPEndPoint point = new IPEndPoint(ip, 2333);
            //进行连接
            socketClient.Connect(point);
            while (true)
            {
                DateTime t1 = DateTime.Now;
                timeflag = LogUpLoadBLL.Execute(timeflag,  socketClient);
                if (t1.AddMilliseconds(1000) > DateTime.Now)
                {
                    Thread.Sleep(1000);
                }
            }
        }
    }
}
