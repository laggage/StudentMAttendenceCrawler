using HtmlAgilityPack;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StudentMorningAttendenceCrawler.Core
{
    public class LoginException:Exception
    {
        public new string Message { get; set; }
        public new Exception InnerException { get; }
        public LoginException(string msg,Exception ex)
        {
            Message = msg;
            InnerException = ex;
        }
    }

    public class MAttendenceCrawler
    {
        private const string LoginPageUrl = "http://10.28.102.51/student/index.jsp";
        private const string LoginUrl = "http://10.28.102.51/student/checkUser.jsp";
        private const string PEInfoUrl = "http://10.28.102.51/student/queryExerInfo.jsp";

        /// <summary>
        /// 利用线程本地存储技术来保证线程安全
        /// </summary>
        private ThreadLocal<RestClient> client;
        private RestClient _client
        {
            get => client.Value;
            set => client.Value = value;
        }

        public MAttendenceCrawler()
        {
            client = new ThreadLocal<RestClient>(() => new RestClient()
            {
                CookieContainer = new System.Net.CookieContainer(),
                Encoding = Encoding.GetEncoding("gb2312")
            });
        }

        private IRestResponse Login(string userName, string passwd)
        {
            _client.BaseUrl = new Uri(LoginPageUrl);
            RestRequest loginReq = new RestRequest(Method.GET);
            _client.Get(loginReq);

            RestRequest req = new RestRequest(Method.POST);
            req.AddHeader("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8");
            req.AddHeader("Accept-Encoding", "gzip, deflate");
            req.AddHeader("cache-control", "no-cache");
            req.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            //request.AddHeader("Cookie", "EC01F06113A100C26442EE128F42250C");
            req.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/73.0.3683.103 Safari/537.36");
            req.AddParameter("dispalyName", "");
            req.AddParameter("displayPasswd", "");
            req.AddParameter("submit", "%B5%C7++%C2%BC");
            req.AddParameter("userName", userName);
            req.AddParameter("passwd", passwd);

            _client.BaseUrl = new Uri(LoginUrl);

            IRestResponse res = _client.Execute(req);
            if (DecodeHtml(res.RawBytes).Contains("登录失败!&nbsp;请重新登录!"))
                throw new LoginException("登录失败,请检查用户名或密码是否正确", res.ErrorException);
            else if (res.ErrorException != null)
                throw res.ErrorException;
            return res;
        }

        /// <summary>
        /// 调用此方法前必须先调用 Login 方法
        /// </summary>
        /// <returns>服务器响应</returns>
        private IRestResponse GetPEInfoPage()
        {
            _client.BaseUrl = new Uri(PEInfoUrl);
            RestRequest req = new RestRequest(Method.GET);
            IRestResponse res = _client.Get(req);
            if (res.ErrorException != null)
                throw res.ErrorException;
            return res;
        }

        public ExpandoObject GetMAttendenceData(string userName, string pwd)
        {
            try
            {
                IRestResponse res = Login(userName, pwd);
                if (res == null) throw new ArgumentNullException(nameof(res));
                dynamic info = new ExpandoObject();

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(Encoding.GetEncoding("gb2312").GetString(res.RawBytes));
                info.学号 = GetDataItem(doc.DocumentNode, "学号");
                info.姓名 = GetDataItem(doc.DocumentNode, "姓名");
                info.性别 = GetDataItem(doc.DocumentNode, "性别");
                info.年级 = GetDataItem(doc.DocumentNode, "年级");
                info.行政班级 = GetDataItem(doc.DocumentNode, "行政班级");

                doc.LoadHtml(DecodeHtml(GetPEInfoPage().RawBytes));
                info.早操 = GetDataItem(doc.DocumentNode, "早操");
                info.体育俱乐部考勤 = GetDataItem(doc.DocumentNode, "体育俱乐部考勤");
                info.引体向上考勤 = GetDataItem(doc.DocumentNode, "引体向上考勤");
                info.篮球比赛 = GetDataItem(doc.DocumentNode, "篮球比赛");
                info.田径 = GetDataItem(doc.DocumentNode, "田径");
                info.运动会单项 = GetDataItem(doc.DocumentNode, "运动会单项");
                info.竞赛管理 = GetDataItem(doc.DocumentNode, "竞赛管理");
                info.考勤4 = GetDataItem(doc.DocumentNode, "考勤4");
                info.考勤5 = GetDataItem(doc.DocumentNode, "考勤5");
                info.增加次数 = GetDataItem(doc.DocumentNode, "增加次数");
                return info;
            }
            catch(LoginException loginEx)
            {
                throw loginEx;
            }
            catch(Exception e)
            {
                throw new WebException("无法正确解析网页,请确保网络联通", e);
            }
        }

        public IEnumerable<ExpandoObject> GetMAttendenceData(string classNumber,int stuCount = 40)
        {
            List<ExpandoObject> datas = new List<ExpandoObject>();
            Parallel.For(0, stuCount, (i, state) =>
            {
                string key = classNumber + (i + 1).ToString().PadLeft(2, '0');
                try
                {   //这里的异常要及时处理掉，否则会影响其他信息的查询
                    dynamic d = GetMAttendenceData(key, key);
                    datas.Add(d);
                }
                catch
                {
                    return;
                }
            });
            return datas;
        }

        private string DecodeHtml(byte[] data)
        {
            return Encoding.GetEncoding("gb2312").GetString(data);
        }

        public string GetDataItem(HtmlNode docNode, string key)
        {
            HtmlNode node = docNode.SelectNodes("//td").FirstOrDefault(n => n.InnerText.Replace("&nbsp;",string.Empty) == key);
            
            return node?.ParentNode.SelectSingleNode("td[last()]").InnerText.Replace("&nbsp;", string.Empty);
        }

        public static string DataToString(ExpandoObject data,bool formatted = true)
        {
            StringBuilder sb = new StringBuilder();
            foreach(var i in data)
            {
                sb.AppendFormat("{0} : {1}\r\n", i.Key, i.Value);
            }
            return sb.ToString();
        }

    }
}
