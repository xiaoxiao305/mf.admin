
using MF.Protocol;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Linq;
using MF.Common.Json;

namespace MF.ConsoleApp
{
    public class Program
    {
        public static int DateTimeToUnixTime(DateTime dateTime)
        {
            return (int)(dateTime - TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        protected static string RecordServerUrl = ConfigurationManager.AppSettings["RecordServerURI"];
        protected static string AccountURI = ConfigurationManager.AppSettings["AccountURI"];
        public static void Main(string[] args)
        {
            try
            {
                int t = 1584109410;
                DateTime d = DateTime.Parse("1970/01/01").AddSeconds(t);
                Console.WriteLine("d:" + d.ToString("yyyy-MM-dd HH:mm:ss"));
                //broadcast
                //send
                //#{<<"unixtime">> := UT， <<"id">> :=  ID, <<"msg">> := Msg} 参数你发这个 
    //            string msg = "sendbroadcast test";
    //            long unixtime = DateTimeToUnixTime(DateTime.Now);
    //            string param = "{\"module\":\"broadcast\",\"func\":\"send\",\"args\":" +
    //Json.SerializeObject(new Dictionary<string, object> { { "unixtime", unixtime }, { "msg", msg } }) + "}";
    //            var res2 = PostHelper.PostClubServer<ClubsRes<object>>(AccountURI, param);
    //            if (res2 != null && res2.ret==0)
    //                Console.WriteLine("res is ok");
    //            else
    //                Console.WriteLine("res is err:"+res2.msg);
            }
            catch (Exception ex)
            {
                Console.WriteLine("main ex:" + ex.Message);
            }
            Console.WriteLine("main is stop");
            Console.ReadLine();
        }
        public static void GetALLClubData()
        {
            List<ClubsModel> list = new ClubBLL().GetAllClubsList();
            if (list == null || list.Count < 1)
            {
                Console.WriteLine("list is null");
                return;
            }
            Console.WriteLine("list.count" + list.Count.ToString());
            //俱乐部昵称，俱乐部ID，群主身份证，群主姓名，群成员总数，群本周收益。
            Log.WriteLog("俱乐部昵称", "  ", "俱乐部ID", "  ", "群主姓名", "  ", "群主身份证", "  ", "手机号", "  ", "群成员总数", "  ", "群本周收益");
            foreach (var item in list)
            {
                Log.WriteLog(item.Name, "  ", item.Id, "  ", item.FounderName, "  ", item.FounderIdentity, "  ", item.Mobile,
                    "  ", item.Members_Count, "  ", item.dividends == null ? 0 : item.dividends.Week);
            }
        }

    }


}

