using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;

namespace mf.token
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //读取网络上的txt文件
                WebClient client = new WebClient();
                string configUrl = ConfigurationManager.AppSettings["configUrl"];
                if (string.IsNullOrEmpty(configUrl)) return;
                configUrl += "admin_token.json";
                byte[] buffer = client.DownloadData(configUrl);
                string res = UTF8Encoding.UTF8.GetString(buffer);
                if (string.IsNullOrEmpty(res))
                {
                    Console.WriteLine("配置文件错误1");
                    return;
                }
                if (res.Length > 2)
                    res = res.Substring(1);
                List<AdminInfo> list = JsonConvert.DeserializeObject<List<AdminInfo>>(res);
                if (list == null || list.Count < 1)
                {
                    Console.WriteLine("配置文件错误2");
                    return;
                }
                //账号信息
                string path = @"enter.txt";
                string inputAcc = "";
                if (File.Exists(path))
                    inputAcc = File.ReadAllText(path);//读取
                else
                {
                    Console.WriteLine("请输入账号：");//输入
                    inputAcc = Console.ReadLine();
                    File.WriteAllText(path, inputAcc);//写入
                }
                if (string.IsNullOrEmpty(inputAcc)) return;
                string needToken = "";
                foreach (AdminInfo info in list)
                {
                    if (info.Account.ToLower().Equals(inputAcc.Trim().ToLower()))
                    {
                        needToken = info.Token.Trim().ToUpper();
                        break;
                    }
                }
                if (string.IsNullOrEmpty(needToken))
                {
                    Console.WriteLine("您输入的账号有误。");
                    return;
                }
                Console.WriteLine("*****************************");
                List<string> keys = Security.Create(needToken);
                foreach (string key in keys)
                {
                    Console.WriteLine(Security.GetToken(key));
                }
                Console.WriteLine("*****************************");

            }
            catch (Exception ex)
            {
                Console.WriteLine("出错啦：" + ex.Message);
            }
            finally
            {
                Console.WriteLine("\n按任意键关闭窗口");
                Console.ReadLine();
            }
        }
    }
}
