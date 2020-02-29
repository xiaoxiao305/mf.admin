using MF.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
namespace MF.Admin.DAL
{
    public class GameDAL : BaseDAL
    {
        static GameDAL()
        {
        }
        public Dictionary<string, List<string[]>> GetMatchMap()
        {
            if (Cache.MatchList == null || Cache.MatchList.Count < 1)
                Cache.LoadGameList();
            return Cache.MatchList;
        }
        public Dictionary<string, string> GetGameMap()
        {
            if (Cache.GameList == null || Cache.GameList.Count < 1)
                Cache.LoadGameList();
            return Cache.GameList;
        }
        public List<Dictionary<string, string>> GetGameListForBlack()
        {
            try
            {
                List<Dictionary<string, string>> res = GetServer<List<Dictionary<string, string>>>(BlackURI + "get", "");
                if (res == null) return null;
                return res;
            }
            catch (Exception ex)
            {
                WriteError("get GetGameListForBlack ex:", ex.Message);
            }
            return null;
        }
        public List<ClubRoomSettingModel> GetRoomSetting(string playerId, string clubId)
        {
            try
            {
                if (string.IsNullOrEmpty(playerId) || string.IsNullOrEmpty(clubId)) return null;
                string param = "{\"module\":\"club_room\",\"func\":\"get_room_setting\",\"args\":{\"player_id\":\""
                    + playerId + "\",\"club_id\":\"" + clubId + "\",\"game_id\":0,\"child_game_id\":0}}";
                ClubsRes<List<ClubRoomSettingModel>> res = PostClubServer<ClubsRes<List<ClubRoomSettingModel>>>(ClubsURI, param);
                if (res == null) return null;
                return res.msg;
            }
            catch (Exception ex)
            {
                WriteError("post get_room_setting ex(clubId:", clubId, "):", ex.Message);
            }
            return null;
        }
        public ClubRoomSettingModel SetRoomSetting(ClubRoomSettingModel model)
        {
            try
            {
                if (model == null || string.IsNullOrEmpty(model.club_id)) return null;
                string param = "{\"module\":\"club_room\",\"func\":\"set_room_setting\",\"args\":" + JsonConvert.SerializeObject(model) + "}";
                return PostClubServer<ClubRoomSettingModel>(ClubsURI, param);
            }
            catch (Exception ex)
            {
                WriteError("post set_room_setting ex:", ex.Message);
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="account"></param>
        /// <param name="audit">1黑名单列表  2待审核黑名单列表</param>
        /// <returns></returns>
        public List<GameBlackUserInfo> GetGameBlackUsers(long gameId, string account,string chargeid,int audit)
        {
            string par = "gameId={0}&account={1}&chargeid={2}&audit={3}";
            string gameid = gameId > 0?gameId.ToString():"";
            string auditTag = audit == 2 ? "NO" : "YES";
            par = string.Format(par, gameid, account, chargeid, auditTag);
            //WriteLog("getblackuser par:", par);
            var res = Post<List<GameBlackUserInfo>>(BlackURI + "getusers", par);
            return res;
        } 
        public Dictionary<string, string> AddBlackUser(string gameId, string account,string value, string levelStr, string remark)
        {
            string param = string.Format("gameId={0}&chargeid={1}&value={2}&level={3}&remark={4}", gameId, account, value,levelStr,remark);
            var res = Post<Dictionary<string, string>>(BlackURI + "adduser", param);
            return res;
        }
        public Dictionary<string, string> UpdateBlackUser(string gameId, string account,string chargeid, string value, string levelStr, string remark)
        {
            string param = string.Format("gameId={0}&account={1}&chargeid={2}&value={3}&level={4}&remark={5}", gameId, account,chargeid, value,levelStr,remark);
            var res = Post<Dictionary<string, string>>(BlackURI + "updateuser", param);
            return res;
        }
        public Dictionary<string, string> ConfirmBlackUser(string account,string chargeid, string confirmData)
        {
            string param = string.Format("account={0}&chargeid={1}&indexs={2}",account, chargeid, confirmData);
            var res = Post<Dictionary<string, string>>(ShiChuiURI + "api/game/shichui", param);
            return res;
        }
        public Dictionary<string, string> DelBlackUser(string gameId, string account)
        {
            if (string.IsNullOrEmpty(gameId) || string.IsNullOrEmpty(account)) return null;
            string param = string.Format("gameId={0}&account={1}", gameId, account);
            var res = Post<Dictionary<string, string>>(BlackURI + "deleteuser", param);
            return res;
        }
        //设置输赢值
        public Dictionary<string, object> SetWinnMoney(string type, string player_id, string value)
        {
            try
            {
                if (string.IsNullOrEmpty(type) || string.IsNullOrEmpty(player_id) || string.IsNullOrEmpty(value)) return null;
                string param = "{\"module\":\"winner\",\"func\":\"black_list\",\"args\":{\"player_id\":\"" + player_id + "\",\"type\":\"" + type + "\",\"win\":0,\"lose\":" + value + ",\"bwin\":0}}";
                return PostClubServer<Dictionary<string,object>>(GameCoinURI, param);
            }
            catch (Exception ex)
            {
                WriteError("post SetWinnMoney ex:", ex.Message);
            }
            return null;
        }
        //当前输赢值
        public Dictionary<string, object> GetWinnMoney(string type, string player_id)
        {
            try
            {
                if (string.IsNullOrEmpty(type) || string.IsNullOrEmpty(player_id)) return null;
                string param = "{\"module\":\"winner\",\"func\":\"get\",\"args\":{\"player_id\":\"" + player_id + "\",\"type\":\"" + type + "\"}}";
                var r= PostClubServer<ClubsRes<Dictionary<string, object>>>(GameCoinURI, param);
                if (r != null && r.ret == 0)
                    return r.msg;
                return null;
            }
            catch (Exception ex)
            {
                WriteError("post GetWinnMoney ex:", ex.Message);
            }
            return null;
        }

        //游戏收益
        public List<GameIncome> GetGameIncome(long start, long end, string type, string chargeid, string roomid, string number)
        {
            string args = string.Format("start={0}&end={1}&gameid={2}&chargeid={3}&roomid={4}&number={5}", start, end, type, chargeid, roomid, number);
            var res = Post<List<GameIncome>>(GameIncome + "income", args);
            return res;
        }
        public string GetGameIncome2(long start, long end, string type, string chargeid, string roomid, string number)
        {
            string args = string.Format("start={0}&end={1}&gameid={2}&chargeid={3}&roomid={4}&number={5}", start, end, type, chargeid, roomid, number);
            var res = Post(GameIncome + "income", args);
            return res;
        }

        public string GetLastGameRecords()
        {
            string res = GetServer(ShiChuiURI + "api/game/getlastgamerecords", "");
            return res;
        }
        
        public string GetGameRec(long start, long end, string type, string chargeid, string roomid, string number)
        {
            string args = string.Format("start={0}&end={1}&gameid={2}&chargeid={3}&roomid={4}&number={5}", start, end, type, chargeid, roomid, number);
            var res = Post(GameIncome + "recsearch", args);
            return res;
        }
    }
}
