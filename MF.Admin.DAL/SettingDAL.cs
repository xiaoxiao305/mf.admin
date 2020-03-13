using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MF.Common.Json;
using MF.Data;
using Newtonsoft.Json;

namespace MF.Admin.DAL
{
    public class SettingDAL : BaseDAL
    {
        public GameServer GameServerList()
        {
            try
            {
                string param = "{\"module\":\"wbc_game_mgr\",\"func\":\"nodes\",\"args\":\"\"}";
                return PostClubServer<GameServer>(WBCUrl, param);
            }
            catch (Exception ex)
            {
                WriteError("SettingDAL FlushMatchGame ex:", ex.Message);
                return null;
            }
        }
        public OprGameServer FlushGameServer(string[] serverList)
        {
            try
            {
                string args = "";
                foreach (string s in serverList)
                {
                    args += "\"" + s + "\",";
                }
                args = args.Substring(0, args.Length - 1);
                string param = "{\"module\":\"wbc_game_mgr\",\"func\":\"reload_config\",\"args\":[" + args + "]}";
                return PostClubServer<OprGameServer>(WBCUrl, param);
            }
            catch (Exception ex)
            {
                WriteError("SettingDAL FlushMatchGame ex:", ex.Message);
                return null;
            }
        }
        public OprGameServer FlushMatchGame()
        {
            try
            {
                string param = "{\"module\":\"wbc_game_mgr\",\"func\":\"reload_game_cache\",\"args\":[]}";
                return PostClubServer<OprGameServer>(WBCUrl, param);
            }
            catch (Exception ex)
            {
                WriteError("SettingDAL FlushMatchGame ex:", ex.Message);
                return null;
            }
        }
        public void SetPushNews(string type, string news)
        {
            try
            {
                string param = "{\"type\":\"" + type + "\",\"args\":{\"audience\":\"all\",\"notification\":{\"alert\":\"" + news + "\"}}}";
                PostClubServer<object>(PUSHURI, param);
            }
            catch (Exception ex)
            {
                WriteError("SettingDAL SetPushNews ex:", ex.Message);
            }

        }


        public ClubsRes<object> SendBroadCast(long unixtime, string msg)
        {
            try
            { 
                string param = "{\"module\":\"broadcast\",\"func\":\"send\",\"args\":" +Json.SerializeObject(new Dictionary<string, object> { { "unixtime", unixtime }, { "msg", msg } }) + "}";
                return PostClubServer<ClubsRes<object>>(RecordServerUrl+"do", param);
            }
            catch (Exception ex)
            {
                WriteError("SettingDAL SetPushNews ex:", ex.Message);
            }
            return null;
        }
    }
}
