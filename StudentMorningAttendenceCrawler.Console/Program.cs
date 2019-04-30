using StudentMorningAttendenceCrawler.Core;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net;
using System.Linq;

namespace StudentMorningAttendenceCrawler.Console
{
    using Console = System.Console;
    class Program
    {
        static void Main(string[] args)
        {
            //Dictionary<string, string> para = new Dictionary<string, string>
            //{
            //    {"dispalyName","" },
            //    {"displayPasswd","" },
            //    {"submit","%B5%C7++%C2%BC" },
            //    {"userName","Z05117203" },
            //    {"userPasswd","Z05117203" }
            //};
            //StringBuilder sb = new StringBuilder();
            //foreach(var i in para)
            //{
            //    sb.Append(string.Format("{0}={1}&", i.Key.Escape(), i.Value.Escape()));
            //}


            //CookieContainer cookies = new CookieContainer();
            //HttpWebRequest loginReq = WebRequest.Create(loginPageUrl) as HttpWebRequest;
            //loginReq.CookieContainer = new CookieContainer();
            //loginReq.KeepAlive = true;
            ////loginReq.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3";
            //loginReq.Headers.Add(HttpRequestHeader.AcceptLanguage, "zh-CN,zh;q=0.9,en;q=0.8");
            //loginReq.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
            //loginReq.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/73.0.3683.103 Safari/537.36";
            //loginReq.Method = "Get";

            //HttpWebResponse loginRes = loginReq.GetResponse() as HttpWebResponse;
            //cookies = loginReq.CookieContainer;
            //string cookieStr = cookies.GetCookieHeader(loginReq.RequestUri);
            //System.Console.WriteLine(cookieStr);
            ////loginRes.Close();
            ////loginRes.Dispose();
            //loginReq.AllowAutoRedirect = true;

            //HttpWebRequest req = WebRequest.Create(queryUrl) as HttpWebRequest;
            //req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3";
            //req.AllowAutoRedirect = false;
            //req.ContentType = "application / x - www - form - urlencoded";
            ////req.Host = "10.28.102.51";
            //req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/73.0.3683.103 Safari/537.36";
            //req.Method = "POST";
            //req.KeepAlive = true;
            ////req.CookieContainer = new CookieContainer();
            ////req.CookieContainer.Add(req.RequestUri,cookies.GetCookies(loginReq.RequestUri));
            //req.Headers.Add("Cookie", cookieStr);
            //req.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
            //req.Headers.Add("Origin", "http://10.28.102.51");
            ////req.Headers.Add("Upgrade-Insecure-Requests", "1");
            //req.Headers.Add(HttpRequestHeader.AcceptLanguage, "zh-CN,zh;q=0.9,en;q=0.8");
            //byte[] data = Encoding.Default .GetBytes(sb.ToString().TrimEnd('&'));
            //req.ContentLength = data.Length;
            ////req.ProtocolVersion = HttpVersion.Version11;

            ////string s = req.CookieContainer.GetCookieHeader(req.RequestUri);
            //Stream reqStream = req.GetRequestStream();


            //reqStream.Write(data, 0, data.Length);
            //reqStream.Flush();
            //reqStream.Close();

            //System.Console.WriteLine(Encoding.Default.GetString(data));

            ////System.Console.WriteLine(reader.ReadToEnd());

            //HttpWebResponse res = req.GetResponse() as HttpWebResponse;
            //Stream resStream = res.GetResponseStream();
            //StreamReader streamReader = new StreamReader(resStream, Encoding.Default);
            //string text = streamReader.ReadToEnd();
            //System.Console.WriteLine(text);
            //System.Console.WriteLine("Status : " + res.StatusCode);
            //System.Console.ReadKey();
            //res.Close();
            //res.Dispose();

            //RestClient client = new RestClient
            //{
            //    BaseUrl = new Uri(loginPageUrl),
            //    CookieContainer = new CookieContainer(),
            //    Encoding = Encoding.GetEncoding("gb2312")
            //};

            //RestRequest loginReq = new RestRequest(Method.GET);
            //client.Get(loginReq);

            //RestRequest request = new RestRequest(Method.POST);
            //request.AddHeader("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8");
            //request.AddHeader("Accept-Encoding", "gzip, deflate");
            //request.AddHeader("cache-control", "no-cache");
            //request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            ////request.AddHeader("Cookie", "EC01F06113A100C26442EE128F42250C");
            //request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/73.0.3683.103 Safari/537.36");
            //request.AddParameter("dispalyName", "");
            //request.AddParameter("displayPasswd", "");
            //request.AddParameter("submit", "%B5%C7++%C2%BC");
            //request.AddParameter("userName", "Z05117203");
            //request.AddParameter("passwd", "Z05117203");
            //System.Console.WriteLine(client.CookieContainer.GetCookieHeader(client.BaseUrl));
            //IRestResponse response = client.Execute(request);

            //response.ContentEncoding = "gb2312";
            //string html = Encoding.Default.GetString(response.RawBytes);

            //System.Console.WriteLine(html);
            Program p = new Program();
            p.Run();
        }

        internal enum QueryMode {
            QueryByClass,
            QUeryByPerson
        }

        internal void GetQueryMode(out QueryMode mode)
        {
            mode = QueryMode.QUeryByPerson;
            int res = 0;
            Console.WriteLine("1.班级查询.");
            Console.WriteLine("2.个人查询");
            while (!int.TryParse(Console.ReadLine(), out res) && 
                !(res == 1 || res == 2)) ;
            mode = res == 1 ? QueryMode.QueryByClass : QueryMode.QUeryByPerson;
        }

        internal void Run()
        {
            while(true)
            {
                Console.Clear();
                GetQueryMode(out QueryMode mode);
                switch (mode)
                {
                    case QueryMode.QUeryByPerson:
                        QueryByPerson();
                        break;
                    case QueryMode.QueryByClass:
                        QueryByClass();
                        break;
                }
            }
            
        }

        internal void QueryByPerson()
        {
            Console.Clear();
            GetUserInfo(out string number, out string pwd);
            MAttendenceCrawler crawler = new MAttendenceCrawler();
            try
            {
                dynamic data = crawler.GetMAttendenceData(number, pwd);
                string dataStr = MAttendenceCrawler.DataToString(data);
                Console.WriteLine(dataStr);
            }
            catch (LoginException loginEx)
            {
                WriteLine("登录失败 : " + loginEx.Message);
            }
            catch (WebException ex)
            {
                WriteLine("获取信息失败 : " + ex.Message);
            }

            Console.ReadKey();
        }

        internal void QueryByClass()
        {
            Console.Clear();
            string classNum = string.Empty;
            int stuNum = 0;
            bool inputValidate = false;
            while(!inputValidate)
            {
                Console.Write("输入班级行政号 : ");
                classNum = Console.ReadLine();
                if (classNum.Length > 7 || classNum.Length < 6)
                {
                    WriteLine("班级行政号只可能是6位或者7位!");
                    inputValidate = false;
                    continue;
                }
                
                Console.Write("输入班级人数 : ");
                if (!int.TryParse(Console.ReadLine(), out stuNum))
                {
                    WriteLine("班级人数输入有误!");
                    inputValidate = false;
                    continue;
                }
                inputValidate = true;
            }
            Console.WriteLine();

            MAttendenceCrawler crawler = new MAttendenceCrawler();
            try
            {
                IEnumerable<ExpandoObject> datas = crawler.GetMAttendenceData(classNum, stuNum);
                datas = datas.OrderBy(
                    d => d.FirstOrDefault(s => s.Key == "学号").Value as string, StringComparer.OrdinalIgnoreCase).ToList();

                foreach (dynamic d in datas)
                {
                    Console.WriteLine(MAttendenceCrawler.DataToString(d));
                }
            }
            catch (LoginException e)
            {
                WriteLine("获取信息失败,请检查网络连接和班级行政代码是否正确.内部错误信息 : " + e.Message);
            }
            catch (Exception e)
            {
                WriteLine("获取信息失败,请检查网络连接和班级行政代码是否正确.内部错误信息 : "+e.Message);
            }
            finally
            {
                Console.ReadKey();
            }
        }

        public static void GetUserInfo(out string number, out string pwd)
        {
            number = pwd = string.Empty;
            bool inputValid = false;
            while (!inputValid)
            {
                Console.Write("输入学号 : ");
                number = Console.ReadLine();
                if(number.Length > 9 || number.Length < 8)
                {
                    WriteLine("学号只能是8位或9位");
                    inputValid = false;
                    continue;
                }
                Console.Write("\r\n输入密码 : ");
                pwd = Console.ReadLine();
                inputValid = true;
            }
            Console.WriteLine();
        }

        public static void WriteLine(string text,ConsoleColor textColor = ConsoleColor.DarkRed)
        {
            ConsoleColor origin = Console.ForegroundColor;
            Console.ForegroundColor = textColor;
            Console.WriteLine(text);
            Console.ForegroundColor = origin;
        }
    }
}
