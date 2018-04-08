using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WatchDog
{
    class Program
    {
        private static DateTime _reTime = DateTime.Now;
        private static object obj = new object();
        private static string ClientAppPath = System.Configuration.ConfigurationManager.AppSettings["ClientAppPath"];
        private static string ClientAppName = System.Configuration.ConfigurationManager.AppSettings["ClientAppName"];
        private static DateTime reTime
        {
            set
            {
                lock (obj)
                {
                    _reTime = value;
                }
            }

            get
            {
                return _reTime;
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Watch Dog!");
            Socket serverSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            IPAddress ip = IPAddress.Any;
            IPEndPoint point = new IPEndPoint(ip, 2333);
            //socket绑定监听地址
            serverSocket.Bind(point);
            Console.WriteLine("Listen Success:" + ip + ":2333");
            //设置同时连接个数
            serverSocket.Listen(10);

            //利用线程后台执行监听,否则程序会假死
            Thread thread = new Thread(Listen);
            thread.IsBackground = true;
            thread.Start(serverSocket);

            //利用线程后台执行监听,否则程序会假死
            Thread threadbeat = new Thread(HeartbeatCheck);
            threadbeat.IsBackground = true;
            threadbeat.Start();

            Console.Read();
        }

        /// <summary>
        /// 监听连接
        /// </summary>
        /// <param name="o"></param>
        static void Listen(object o)
        {
            var serverSocket = o as Socket;
            reTime = DateTime.Now;
            while (true)
            {
                //等待连接并且创建一个负责通讯的socket
                var send = serverSocket.Accept();
                //获取链接的IP地址
                var sendIpoint = send.RemoteEndPoint.ToString();
                Console.WriteLine($"{DateTime.Now}:{sendIpoint} Connection");
                //开启一个新线程不停接收消息
                Thread thread = new Thread(Recive);
                thread.IsBackground = true;
                thread.Start(send);  
            }
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="o"></param>
        static void Recive(object o)
        {
            try {
                var send = o as Socket;
                while (true)
                {
                    //获取发送过来的消息容器
                    byte[] buffer = new byte[1024 * 1024 * 5];
                    var effective = send.Receive(buffer);
                    //有效字节为0则跳过
                    if (effective == 0)
                    {
                        break;
                    }
                    var str = Encoding.UTF8.GetString(buffer, 0, effective);
                    if (!string.IsNullOrEmpty(str))
                    {

                        reTime = DateTime.Now;
                        str = ".a..";
                    }
                    Console.WriteLine(DateTime.Now + ":"+ str);
                    Thread.Sleep(5000);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(DateTime.Now + ":"+ex.Message);
                return;
            }
        }


        static void HeartbeatCheck()
        {
            Console.WriteLine("守护线程开启！");
            bool flag = true;
            while (true)
            {
                if (reTime.AddMinutes(1) < DateTime.Now)
                {
                    Console.WriteLine("检测不到客户端在线，重启客户端程序！");
                    Process[] pro = Process.GetProcesses();//获取已开启的所有进程     
                    //遍历所有查找到的进程
                    for (int i = 0; i < pro.Length; i++)
                    {
                        //Console.WriteLine(pro[i].ProcessName);
                        //判断此进程是否是要查找的进程
                        if (ClientAppName.ToLower() == pro[i].ProcessName.ToString().ToLower())
                        {
                            pro[i].Kill();//结束进程
                            Console.WriteLine($"杀死进程：{pro[i].ProcessName}");
                        }
                    }
                    flag = false;
                    //重启程序
                    Process.Start(ClientAppPath);
                }
                if (flag)
                {
                    Thread.Sleep(10000);
                }
                else
                {
                    Thread.Sleep(6000);
                }
                
            }
        }

    }
}
