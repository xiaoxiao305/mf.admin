
using MF.Protocol;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading;

namespace MF.ConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                int rowCount = 0;
                DateTime baseTime = DateTime.Parse("2012-10-01");
                long stime = (long)(DateTime.Parse("2019-01-01") - baseTime).TotalSeconds;
                long etime = (long)(DateTime.Now - baseTime).TotalSeconds;
                //List<CurrencyRecord> list = UserBLL.GetCurrcryRecord(10, 1, 0, 0, 0, "", "", 1, stime, etime, 4, out rowCount);
                List<CurrencyRecord> list = UserBLL.GetCurrcryRecord(1000, 1, 0, 0, 0, "", "", 0, 0, 0, 4, out rowCount);
                if (list == null || list.Count < 1)
                {
                    Console.WriteLine("list is null");
                }
                else
                {
                    foreach (var item in list)
                    {
                        Console.WriteLine("item:" + item.Account);
                    }
                    Console.WriteLine("search is ok");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("main ex:" + ex.Message);
            }
            Console.WriteLine("main is stop");
            Console.ReadLine();
        }

    }



}

