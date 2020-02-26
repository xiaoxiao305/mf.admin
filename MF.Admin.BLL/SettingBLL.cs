using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MF.Admin.DAL;
using MF.Data;

namespace MF.Admin.BLL
{
    public class SettingBLL : Base
    {
        private static SettingDAL dal = new SettingDAL();
        public static List<GameServerList> GameServerList()
        {
            GameServer gs = dal.GameServerList();
            if (gs == null || gs.ret != 0 || gs.msg == null || gs.msg.Length < 1)
                return null;
            List<GameServerList> gsls = new List<Data.GameServerList>();
            foreach (string msg in gs.msg)
            {
                gsls.Add(new GameServerList() { gname = msg });
            }
            return gsls;
        }
        public static OprGameServer FlushGameServer(string[] serverList)
        {
            return dal.FlushGameServer(serverList);
        }
        public static OprGameServer FlushMatchGame()
        {
            return dal.FlushMatchGame();
        }
        public static void SetPushNews(long type, string news)
        {
            if (type < 1 || string.IsNullOrEmpty(news))
                return ;
            string s = type == 1 ? "android" : type == 2 ? "ios" : "undifined";
            dal.SetPushNews(s, news);
        }
    }
}
