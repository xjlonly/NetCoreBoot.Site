using LogUpLoadService.QQHelper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
            //Thread thread1 = new Thread(new ThreadStart(PostMethod));
            //thread1.IsBackground = true;
            //thread1.Start();

            //创建实例
            Socket socketClient = new Socket(SocketType.Stream, ProtocolType.Tcp);
            IPAddress ip = IPAddress.Parse(Ip);
            IPEndPoint point = new IPEndPoint(ip, 2333);
            //进行连接
            socketClient.Connect(point);

            Console.WriteLine("测试QQ发送消息!");
            QQChatWindow qq = new QQChatWindow("12345");
            qq.sendQQMessage("12345", "测试消息");

            while (true)
            {
                DateTime t1 = DateTime.Now;
                timeflag = LogUpLoadBLL.Execute(timeflag, socketClient);
                if (t1.AddMilliseconds(1000) > DateTime.Now)
                {
                    Thread.Sleep(1000);
                }
            }
        }

        public static void PostMethod()
        {
            Console.WriteLine("开启循环访问线程！");
            List<UrlData> list = new List<UrlData>();
            list.Add(new UrlData {
                url = "https://app.tjgaj.gov.cn/appsite/WebAppService.asmx/getrclh",
                json = "{}"
            });
            list.Add(new UrlData
            {
                url = "https://app.tjgaj.gov.cn/appsite/WebAppService.asmx/User_Info",
                json = "{\"UserId\":\"18301210004\",\"PassWord\":\"015525510\"}",
            });

            list.Add(new UrlData
            {
                url = "https://app.tjgaj.gov.cn/appsite/WebAppService.asmx/getTianqi",
                json = "{}",
            });
            List<UrlData> list1 = new List<UrlData>();
            list1.Add(new UrlData
            {
                url = "http://wx.tjgaj.gov.cn/WeixinWebapp/Index/service.aspx",
                json = "",
            });

            //list1.Add(new UrlData
            //{
            //    url = "https://www.baidu.com/",
            //    json = "",
            //});
          
            while (true)
            {
                DateTime t1 = DateTime.Now;
                foreach (UrlData item in list)
                {
                  var rel =  HttpPost(item.url, item.json);
                    if (rel)
                    {
                        Console.WriteLine($"{DateTime.Now}>url:{item.url}访问成功！！！！！！！");
                        QQChatWindow qq = new QQChatWindow("12345");
                        qq.sendQQMessage("12345", $"{item.url}访问成功！");
                    }
                }


                foreach (UrlData item in list1)
                {
                    var rel = HttpGet(item.url,"");
                    if (rel)
                    {
                        Console.WriteLine($"{DateTime.Now}>url:{item.url}访问成功！！！！！！！");
                        QQChatWindow qq = new QQChatWindow("12345");
                        qq.sendQQMessage("12345", $"{item.url}访问成功！");
                    }
                }

                if (t1.AddMilliseconds(1000 * 60) > DateTime.Now)
                {
                    Thread.Sleep(1000 * 60);
                }
            }
        }



        /// <summary>  
        /// POST请求与获取结果  
        /// </summary>  
        public static bool HttpPost(string Url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = postDataStr.Length;
            //request.Timeout = 1000 * 10;
            var httpStatusCode = 200;
            try
            {
                StreamWriter writer = new StreamWriter(request.GetRequestStream(), Encoding.ASCII);
                writer.Write(postDataStr);
                writer.Flush();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                httpStatusCode = (int)response.StatusCode;
                string encoding = response.ContentEncoding;
                if (encoding == null || encoding.Length < 1)
                {
                    encoding = "UTF-8"; //默认编码  
                }
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));
                string retString = reader.ReadToEnd();
            }
            catch(WebException ex)
            {

                Console.WriteLine($"{DateTime.Now}>url:{Url},{ex.Message} ");
                var rsp = ex.Response as HttpWebResponse;
                httpStatusCode = rsp == null ? 0: (int)rsp.StatusCode;
                
               
            }
            if (httpStatusCode == 0)
            {
                return false;
            }
            if (httpStatusCode == 200)
            {
                return true;
            }
            else
            {
                
                Console.WriteLine($"{DateTime.Now}>url:{Url},status:{httpStatusCode} ");
                return false;
            }
        }


        /// <summary>  
        /// GET请求与获取结果  
        /// </summary>  
        public static bool HttpGet(string Url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            var httpStatusCode = 200;
            //request.Timeout = 1000 * 10;
            try {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                httpStatusCode = (int)response.StatusCode;
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
            }
            catch (WebException ex)
            {
                Console.WriteLine($"{DateTime.Now}>url:{Url},{ex.Message} ");
                var rsp = ex.Response as HttpWebResponse;
                httpStatusCode = rsp == null ? 0 : (int)rsp.StatusCode;

            }
            if(httpStatusCode == 0)
            {
                return false;
            }
            if (httpStatusCode == 200)
            {
                return true;
            }
            else
            {
                Console.WriteLine($"{DateTime.Now}>url:{Url}，失败,status:{httpStatusCode} ");
                return false;
            }
        }

    }

    public class UrlData
    {
        public string url { get; set; }
        public string json { get; set; }
    }
}
