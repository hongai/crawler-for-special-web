using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Runtime.InteropServices;
using HtmlAgilityPack;

namespace movie
{

        class Program
    {

        const string NEWS = "news.txt";
        const string RANGE = "range.txt";


        static void Write(string file, string content)//写文件 传递文件名与写的内容
        {
            try
            {
                string time = DateTime.Now.ToString("yyyy年MM月dd日HH时mm分ss秒");
                StreamWriter sw = new StreamWriter(@file,true);
                sw.WriteLine(time + "\r\n");
                sw.Write(content);
                sw.Close();
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("文件无法被找到：", e);
            }
            catch (IOException e)
            {
                Console.WriteLine("输入或输出错误：", e);
            }
            catch (Exception e)
            {
                Console.WriteLine("出现错误，异常值为：\n{0}", e);
            }

        }
        static string range(string url)//票房排名获取
        {
            string strmsg = string.Empty;//格式化内容保存在该变量中
            try
            {
                HtmlWeb web = new HtmlWeb();
                HtmlDocument doc = web.Load(url);
                HtmlAgilityPack.HtmlNode n1 = doc.DocumentNode;
                foreach (var title in n1.Descendants("dd"))
                {
                    foreach (var range in title.Descendants())
                    {
                        if (range.Name == "h3")
                        {
                            strmsg += (range.InnerText.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", ""));
                            strmsg += "\r\n";
                        }
                        else
                            continue;
                    }
                }
                return strmsg;

            }
            catch (WebException e)
            {
                Console.WriteLine("连接失败： {0}", e);
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine("出现错误： {0}", e);
                return null;
            }
        }
        static string news(string url)//1905电影新闻
        {
            string strmsg = string.Empty;
            try
            {
                WebRequest request = WebRequest.Create(url);
                WebResponse response = request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(reader.ReadToEnd());
                reader.Close();
                response.Close();
                HtmlAgilityPack.HtmlNode n1 = doc.GetElementbyId("content");
                foreach (var title in n1.Descendants())
                {
                    if (title.Name == "h3")
                    {
                        strmsg += (title.InnerText.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", ""));
                        strmsg += "\r\n";
                    }
                    else
                        continue;

                }
                return strmsg;

            }
            catch (WebException e)
            {
                Console.WriteLine("连接失败： {0}", e);
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine("出现错误： {0}", e);
                return null;
            }
        }
       /* static string news(string url)//失败的新浪新闻
        {
            url = "http://ent.sina.com.cn/film/";
            string strmsg = string.Empty;
            try
            {
                WebRequest request = WebRequest.Create(url);
                WebResponse response = request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(reader.ReadToEnd());
                reader.Close();
                response.Close();
                HtmlAgilityPack.HtmlNode n1 = doc.GetElementbyId("feedCardContent");
                 foreach (var title in n1.Descendants())
                 {

                        if (title.Name == "h2")
                        {
                             strmsg += (title.InnerText.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", ""));
                             strmsg += "\r\n";
                         }
                         else
                            continue;
                    
                 }
                return strmsg;

            }
            catch (WebException e)
            {
                Console.WriteLine("连接失败： {0}", e);
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine("出现错误： {0}", e);
                return null;
            }
        }*/
        static void Main(string[] args)
        {
            string url1 = "http://movie.mtime.com/boxoffice/";
            string url2 = "http://www.1905.com/film/?fr=homepc_menu_news/";
            string firstdata;
            string seconddata;
            firstdata = range(url1);
            Write(RANGE, firstdata);
            seconddata = news(url2);
            Write(NEWS, seconddata);

        }
    }
}
