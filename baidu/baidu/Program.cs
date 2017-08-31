using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using HtmlAgilityPack;

namespace baidu
{
    class Program
    {
        const string SOURCE = "serch.txt";
        const string RESULT = "result.txt";
        const string FORMAT = "format.txt";

        static string Read(string file)//读文件 传递文件名参数file 返回所读内容的一行
        {
            try {
                StreamReader sr = new StreamReader(@file, Encoding.Default);
                string line = sr.ReadLine();
                Console.WriteLine(line.ToString());
                sr.Close();
                return line;
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("文件无法被找到：", e);
                return null;
            }
            catch (IOException e)
            {
                Console.WriteLine("输入或输出错误：", e);
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine("出现错误，异常值为：\n{0}", e);
                return null;
            }

        }

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

        static string link(string url)//通过url进行访问并获得网页内容
        {
            string strmsg = string.Empty;//网页内容保存在该变量中
            try
            {
                WebRequest request = WebRequest.Create(url);
                WebResponse response = request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
                strmsg = reader.ReadToEnd();
                reader.Close();
                response.Close();
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

        static string format(string url)//通过url获得并格式化输出网页内容
        {
            string strmsg = string.Empty;//格式化内容保存在该变量中
            try
            {
                HtmlWeb web = new HtmlWeb();
                HtmlDocument doc = web.Load(url);
                HtmlAgilityPack.HtmlNode n1 = doc.GetElementbyId("content_left");
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
        static void Main(string[] args)
        {
            string line = Read(SOURCE);
            string url = "http://www.baidu.com/s?wd=" + line + "&pn=0&rn=50";
            string strmsg = link(url);
            Write(RESULT, strmsg);
            while (true)
            {
                strmsg = format(url);
                Write(FORMAT, strmsg);
                System.Threading.Thread.Sleep(7200000);

            }
            
        }
    }
}
