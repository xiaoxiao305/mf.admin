using MF.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace MF.Admin.DAL
{
    public class Cache
    {
        public static Dictionary<string, List<string[]>> MatchList { get; set; }
        public static Dictionary<string, string> GameList { get; set; }
        /// <summary>
        /// {account:Users}
        /// </summary>
        public static Dictionary<string, Users> CacheAccountList = new Dictionary<string, Users>();
        /// <summary>
        /// {chargeid:Users}
        /// </summary>
        public static Dictionary<string, Users> CacheChargeidList = new Dictionary<string, Users>();
        /// <summary>
        /// {clubId:clubName}
        /// </summary>
        public static Dictionary<string, string> CacheClubName = new Dictionary<string, string>();
        /// <summary>
        /// {chargeId:List<clubId>}
        /// </summary>
        public static Dictionary<string, List<string>> CacheClubId = new Dictionary<string, List<string>>();
        /// <summary>
        /// {gameType:Value}
        /// </summary>
        public static Dictionary<string, string> CacheRedAlert = new Dictionary<string, string>();

        private static string configpath = ConfigurationManager.AppSettings["configpath"];
        public static void LoadGameList()
        {
            try
            {
                GameList = new Dictionary<string, string>();
                MatchList = new Dictionary<string, List<string[]>>();
                Result<List<GameInfo>> res = BaseDAL.ReadExcel<GameInfo>(configpath + "gameinfo.xls");
                if (res == null || res.R == null || res.R.Count < 1)
                    return;
                foreach (GameInfo model in res.R)
                {
                    if (!GameList.ContainsKey(model.gameid.ToString()))
                        GameList.Add(model.gameid.ToString(), model.gamename);
                    if (!MatchList.ContainsKey(model.gameid.ToString()))
                        MatchList.Add(model.gameid.ToString(), new List<string[]>() { new string[] { model.matchid.ToString(), model.matchname } });
                    else
                    {
                        List<string[]> listtemp = MatchList[model.gameid.ToString()];
                        listtemp.Add(new string[] { model.matchid.ToString(), model.matchname });
                        MatchList[model.gameid.ToString()] = listtemp;
                    }
                }
            }
            catch (Exception ex)
            {
                BaseDAL.WriteError("LoadGameList ex:",ex.Message);
            }
        }
    }
}
