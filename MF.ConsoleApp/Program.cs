
using MF.Data;
using MF.Data.ExtendChannel;
using RedisHelp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace MF.ConsoleApp
{ 
    public class Program
    {
        static RedisHelper redis = new RedisHelper(0);
        public static void Main(string[] args)
        {
            List<ExtendChannel> list = MF.Admin.BLL.ChannelBLL.CountRedisData(DateTime.Now);
            if (list == null || list.Count < 1)
            { 
                Console.WriteLine("list is null.");
            return; }
            foreach (ExtendChannel model in list)
            {
                string mstr = string.Format("{0}_{1}_{2} pc:{3}-{4}-{5} android:{6}-{7}-{8} ios:{9}-{10}-{11} seconddown:{12} loadavg:{13} secondavg:{14} stay:{15} networkdata:{16} wifi:{17}",model.Day,model.Channel,model.ChannelNum,
                   model.PCLoad, model.PCDown, model.PCFirstActive,
                   model.AndroidLoad, model.AndroidDown, model.AndroidFirstActive,
                   model.iOSLoad, model.iOSDown, model.iOSFirstActive,
                   model.SecondDown, model.LoadTimeAvg, model.SecondDownTimeAvg,
                   model.Stay,model.NetMobileData,model.NetWifi
                    );
                Console.WriteLine(mstr);
            }


            Console.ReadLine();
        } 
    }



}

