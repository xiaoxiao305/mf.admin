
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
                
             List<ClubsModel> list=   new ClubBLL().GetAllClubsList();
                if(list==null || list.Count < 1)
                {
                    Console.WriteLine("list is null");
                    return;
                }
                Console.WriteLine("list.count"+list.Count.ToString());
                //俱乐部昵称，俱乐部ID，群主身份证，群主姓名，群成员总数，群本周收益。
                Log.WriteLog("俱乐部昵称", "  ", "俱乐部ID", "  ", "群主姓名", "  ", "群主身份证", "  ", "手机号", "  ", "群成员总数", "  ", "群本周收益");
                foreach (var item in list)
                {
                    Log.WriteLog(item.Name, "  ", item.Id, "  ", item.FounderName, "  ", item.FounderIdentity, "  ", item.Mobile,
                        "  ", item.Members_Count, "  ", item.dividends==null?0:item.dividends.Week);
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

