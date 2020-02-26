using System;
using System.Collections.Generic;
using MF.Data;
using System.Web;
using System.Web.Security;
using MF.Common.Security;
using MF.Common.Json;
using System.Threading;
using MF.Admin.DAL;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;

namespace MF.Admin.BLL
{

    public class GameBLL : Base
    {
        private static GameDAL dal = new GameDAL();
        /// <summary>
        /// 游戏场列表
        /// </summary>
        static Dictionary<string, List<string[]>> matchMap;
        /// <summary>
        /// 游戏列表
        /// </summary>
        static Dictionary<string, string> gameMap;
        /// <summary>
        /// 游戏列表【用于游戏黑名单】
        /// </summary>
        static List<Dictionary<string, string>> gameListForBlack;
        static GameBLL()
        {
        }
        public static Dictionary<string, List<string[]>> GetMatchMap()
        {
            if (matchMap == null || matchMap.Count < 1)
                matchMap = dal.GetMatchMap();
            return matchMap;
        }
        /// <summary>
        /// 游戏列表【游戏id,游戏名称】
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetGameMap()
        {
            if (gameMap == null || gameMap.Count < 1)
                gameMap = dal.GetGameMap();
            return gameMap;
        }
        /// <summary>
        /// 游戏列表【游戏id,游戏名称】-----游戏黑名单
        /// </summary>
        /// <returns></returns>
        public static List<Dictionary<string, string>> GetGameListForBlack()
        {
            if (gameListForBlack == null || gameListForBlack.Count < 1)
                gameListForBlack = dal.GetGameListForBlack();
            return gameListForBlack;
        }
        public static ClubsServerRes SetGameSetting(string clubId)
        {
            try
            {
                //获取参考俱乐部setting值
                if (string.IsNullOrEmpty(clubId)) return null;
                List<ClubsModel> clubListTmp = GuildBLL.GetGuildList(3, clubId);
                if (clubListTmp == null || clubListTmp.Count < 1 || clubListTmp[0] == null) return null;
                ClubsModel clubRefer = clubListTmp[0];
                List<ClubRoomSettingModel> setingModelList = GetRoomSetting(clubRefer.Founder, clubRefer.Id.ToString());
                if (setingModelList == null || setingModelList.Count < 1) return null;
                //获取所有俱乐部
                List<ClubsModel> clubList = GuildBLL.GetAllClubsList();
                if (clubList == null || clubList.Count < 1) return null;
                foreach (ClubsModel club in clubList)
                {
                    if (club.Type != 4) continue;
                    foreach (ClubRoomSettingModel model in setingModelList)
                    {
                        if (model == null) continue;
                        model.club_id = club.Id.ToString();
                        try
                        {
                            ClubRoomSettingModel newSettingRes = SetRoomSetting(model);
                            if (newSettingRes == null)
                                WriteError("SetRoomSetting res err.", JsonConvert.SerializeObject(model));
                        }
                        catch (Exception e)
                        {
                            WriteError("SetRoomSetting res ex.", e.Message, JsonConvert.SerializeObject(model));
                            continue;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteError("SetGameSetting ex:", ex.Message);
                return new ClubsServerRes() { ret = 1 };
            }
            return new ClubsServerRes() { ret = 0 };
        }
        /// <summary>
        /// 读取JSON文件
        /// </summary>
        /// <param name="key">JSON文件中的key值</param>
        /// <returns>JSON文件中的value值</returns>
        public static Dictionary<string, object> Readjson(string key, string filePath)
        { 
            using (StreamReader file = File.OpenText(filePath))
            {
                var jsonText = file.ReadToEnd();
                Dictionary<string, Dictionary<string, object>> jsonObj = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, object>>>(jsonText);
                if (jsonObj != null && jsonObj.ContainsKey(key)) return jsonObj[key];
            }
            return null;
        }

        //黑名单
        public static List<GameBlackUserInfo> GetBlackUsersDetail2(List<GameBlackUserInfo> list, string gameType)
        {
            try
            {
                if (list == null || list.Count < 1) return null;
                List<string> accList = new List<string>();
                foreach (var item in list)
                {
                    accList.Add(item.Account);
                }
                List<Dictionary<string, string>> infoList = new UserDAL().GetUserInfoList(accList.ToArray());
                if (infoList == null || infoList.Count < 1) return list;
                List<GameBlackUserInfo> newList2 = new List<GameBlackUserInfo>();
                foreach (GameBlackUserInfo info in list)
                {
                    foreach (Dictionary<string, string> dic in infoList)
                    {
                        if (info.Account.ToLower().Equals(dic["Account"].ToString()))
                        {
                            info.NickName = dic["Nickname"].ToString();
                            if (dic["ChargeId"] != null && !string.IsNullOrEmpty(dic["ChargeId"].ToString()))
                            {
                                info.ChargeId = dic["ChargeId"].ToString();
                                Dictionary<string, object> listinfos = new GuildDAL().GetMembersList(dic["ChargeId"].ToString());
                                if (listinfos != null && listinfos.Count > 0 && listinfos.ContainsKey("clubs"))
                                {
                                    List<string> club_ids = (listinfos["clubs"] as Newtonsoft.Json.Linq.JArray).ToObject<List<string>>();
                                    if (club_ids != null && club_ids.Count > 0)
                                        info.ClubId = "[" + String.Join(",", club_ids) + "]";
                                    //money
                                    //Base.WriteLog("gameType:", gameType);
                                    if (string.IsNullOrEmpty(gameType))
                                    {
                                        //Base.WriteLog("jsonpath 111:", gameType);
                                        string jsonPath = HttpContext.Current.Server.MapPath("/common/js/gameback.json");
                                        //Base.WriteLog("jsonpath:", jsonPath);
                                        Dictionary<string, object> gamebackObj = Readjson(info.GameId, jsonPath);
                                        if (gamebackObj != null && gamebackObj.ContainsKey("type"))
                                            gameType = gamebackObj["type"].ToString();
                                        //Base.WriteLog("gameType:", gameType);
                                    }
                                    if (string.IsNullOrEmpty(gameType)) break;
                                    Dictionary<string, object> moneyDic = dal.GetWinnMoney(gameType, dic["ChargeId"].ToString());
                                    if (moneyDic != null && moneyDic.ContainsKey("lose"))
                                    {
                                        info.Money = moneyDic["lose"].ToString();
                                    }
                                }
                            }
                            break;
                        }
                    }
                    newList2.Add(info);
                }
                return newList2;
            }
            catch (Exception ex)
            {
                Base.WriteError("GetBlackUsersDetail ex:", ex.Message);
                return list;
            }
        }
        public static string GetUserGameMoney(string chargeId, string gameType)
        {
            try
            {
                if (string.IsNullOrEmpty(chargeId) || string.IsNullOrEmpty(gameType)) return "0";
                Dictionary<string, object> clubInfos = new GuildDAL().GetMembersList(chargeId);
                if (clubInfos == null || clubInfos.Count < 1 || !clubInfos.ContainsKey("clubs")) return "0";
                List<string> club_ids = (clubInfos["clubs"] as JArray).ToObject<List<string>>();
                if (club_ids == null || club_ids.Count < 1) return "0";
                string ClubId = "[" + String.Join(",", club_ids) + "]";
                Dictionary<string, object> moneyDic = dal.GetWinnMoney(gameType, chargeId);
                if (moneyDic == null || !moneyDic.ContainsKey("lose")) return "0";
                return moneyDic["lose"].ToString();
            }
            catch (Exception ex)
            {
                Base.WriteError("GetUserGameMoney ex:", ex.Message);
                return "";
            }
        }

        public static List<GameBlackUserInfo> GetBlackUsersDetail(List<GameBlackUserInfo> list)
        {
            try
            {
                if (list == null || list.Count < 1) return null;
                List<string> accList = new List<string>();
                foreach (var blackUser in list)
                {
                    if (Cache.CacheAccountList != null &&
                        Cache.CacheAccountList.ContainsKey(blackUser.Account.ToLower()))
                        continue;
                    accList.Add(blackUser.Account);
                }
                if (accList.Count > 0)
                    new UserDAL().GetUserInfoList(accList.ToArray());
                List<GameBlackUserInfo> newList2 = new List<GameBlackUserInfo>();
                foreach (GameBlackUserInfo info in list)
                {
                    info.NickName = new UserDAL().GetNickByAcc(info.Account);
                    if (string.IsNullOrEmpty(info.ChargeId))
                        info.ChargeId = new UserDAL().GetChargeIdByAcc(info.Account);
                    newList2.Add(info);
                }
                return newList2;
            }
            catch (Exception ex)
            {
                Base.WriteError("GetBlackUsersDetail ex:", ex.Message);
                return list;
            }
        }
        /// <summary>
        /// 获取黑名单列表
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="gameType"></param>
        /// <param name="account"></param> 
        /// <returns></returns>
        public static List<GameBlackUserInfo> GetGameBlackUsers(long gameId,long field, string value)
        {
            //if (gameId < 1 || string.IsNullOrEmpty(gameType))
            //    return null;
            string account = field == 2 ? value : "";
            string chargeid = field == 1 ? value : ""; 
            List<GameBlackUserInfo> list = GetGameBlackUsers(gameId,  account,chargeid);
            if (list == null || list.Count < 1) return null;
            //return list;
            return GetBlackUsersDetail(list);
        }
        public static List<GameBlackUserInfo> GetAuditBlackUsers(long gameId, long field, string value)
        {
            string account = field == 2 ? value : "";
            string chargeid = field == 1 ? value : "";
            List<GameBlackUserInfo> list = dal.GetGameBlackUsers(gameId,account,chargeid, 2);
            if (list == null || list.Count < 1) return null;
            //return list;
            return GetBlackUsersDetail(list);
        }
        public static List<GameBlackUserInfo> GetGameBlackUsers(long gameId, string account, string chargeid)
        {
            try
            {
                //if (gameId < 1) return null;
                return dal.GetGameBlackUsers(gameId, account,chargeid, 1);
            }
            catch (Exception ex)
            {
                WriteError("GuildBLL GetMembersList ex:", ex.Message);
            }
            return null;
        }
        public static Dictionary<string, string> AddBlackUser(string gameId, string account, string value, string levelStr, string remark)
        {
            if (string.IsNullOrEmpty(gameId) || string.IsNullOrEmpty(account) || string.IsNullOrEmpty(value) || string.IsNullOrEmpty(levelStr)) return null;
            return dal.AddBlackUser(gameId, account.Trim(), value.Trim(), levelStr, remark);
        }
        public static Dictionary<string, string> UpdateBlackUser(string gameId, string account, string chargeid, string value, string levelStr, string remark)
        {
            if (string.IsNullOrEmpty(gameId) || string.IsNullOrEmpty(account) || string.IsNullOrEmpty(value) || string.IsNullOrEmpty(levelStr)) return null;
            Dictionary<string, string> r = dal.UpdateBlackUser(gameId, account, chargeid, value.Trim(), levelStr, remark);
            int oprState = 0;
            string msg = string.Format("操作游戏{0}，修改黑名单【{1}】值为【{2}】【{3}】失败。", gameId, account, value, remark);
            if (r != null)
            {
                if (r.ContainsKey("succeed") && r.ContainsKey("message"))
                {
                    if (bool.Parse(r["succeed"].ToString()))
                    {
                        oprState = 1;
                        msg = string.Format("操作游戏{0}，修改黑名单【{1}】值为【{2}】【{3}】成功", gameId, account, value, remark);
                    }
                    else
                        msg += r["message"];
                }
                else
                    msg += " res is err";
            }
            AdminBLL.WriteSystemLog(CurrentUser.Account, ClientIP, msg, "GameBLL.UpdateBlackUser", oprState, SystemLogEnum.UPDATEBLACKUSER);
            return r;
        }
        public static Dictionary<string, string> ConfirmBlackUser(string account, string chargeid, string confirmData)
        {
            if (string.IsNullOrEmpty(account) || string.IsNullOrEmpty(chargeid) || string.IsNullOrEmpty(confirmData)) return null;
            Dictionary<string, string> r = dal.ConfirmBlackUser(account, chargeid, confirmData);
            int oprState = 0;
            string msg = string.Format("审核黑名单，确认实锤{0}数据为{1}失败。", chargeid, confirmData);
            if (r != null)
            {
                if (r.ContainsKey("succeed") && r.ContainsKey("message"))
                {
                    if (bool.Parse(r["succeed"].ToString()))
                    {
                        oprState = 1;
                        msg = string.Format("审核黑名单，确认实锤{0}数据为{1}成功。", chargeid, confirmData);
                    }
                    else
                        msg += r["message"];
                }
                else
                    msg += " res is err";
            }
            AdminBLL.WriteSystemLog(CurrentUser.Account, ClientIP, msg, "GameBLL.ConfirmBlackUser", oprState, SystemLogEnum.CONFIRMBLACKUSER);
            return r;
        }

        public static Dictionary<string, string> DelBlackUser(string gameId, string account)
        {
            if (string.IsNullOrEmpty(gameId) || string.IsNullOrEmpty(account)) return null;
            return dal.DelBlackUser(gameId, account);
        }
        public static Dictionary<string, object> SetWinnMoney(string gameId, string account, string type, string player_id, string value)
        {
            if (string.IsNullOrEmpty(gameId) || string.IsNullOrEmpty(account) || string.IsNullOrEmpty(type)) return null;
            //del
            Dictionary<string, string> r = DelBlackUser(gameId, account);
            int oprState = 0;
            string msg = string.Format("操作游戏{0}，删除黑名单【{1}】失败。", gameId, account);
            if (r != null)
            {
                if (r.ContainsKey("succeed") && r.ContainsKey("message"))
                {
                    if (bool.Parse(r["succeed"].ToString()))
                    {
                        oprState = 1;
                        msg = string.Format("操作游戏{0}，删除黑名单【{1}】成功", gameId, account);
                    }
                    else
                        msg += r["message"];
                }
                else
                    msg += " res is err";
            }
            AdminBLL.WriteSystemLog(CurrentUser.Account, ClientIP, msg, "AjaxRequest.DelBlackUser", oprState, SystemLogEnum.ADDBLACKUSER);
            //set
            if (!string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(player_id) && oprState == 1)
                return dal.SetWinnMoney(type, player_id, value);
            else
            {
                msg = oprState == 1 ? "" : msg;
                return new Dictionary<string, object>() { { "ret", (oprState == 1 ? 0 : 1) }, { "msg", msg } };
            }
        }



        private static List<ClubRoomSettingModel> GetRoomSetting(string playerId, string clubId)
        {
            if (string.IsNullOrEmpty(playerId) || string.IsNullOrEmpty(clubId)) return null;
            return dal.GetRoomSetting(playerId, clubId);
        }
        private static ClubRoomSettingModel SetRoomSetting(ClubRoomSettingModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.club_id)) return null;
            return dal.SetRoomSetting(model);
        }
        private static ClubRoomSettingModel ResetSettingModel(string[] matchId, ClubRoomSettingModel model)
        {
            if (model == null || matchId.Length < 1) return null;
            bool isContain = false;
            for (int i = 0; i < matchId.Length; i++)
            {
                //SettingData
                if (string.IsNullOrEmpty(model.setting)) break;
                Dictionary<string, object> settingDic = JsonConvert.DeserializeObject<Dictionary<string, object>>(model.setting);
                if (settingDic == null) break;
                if (settingDic.ContainsKey("settingData"))
                {
                    List<Dictionary<string, object>> settingDataDic = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(settingDic["settingData"].ToString());
                    if (settingDataDic != null && settingDataDic.Count > 0)
                    {
                        for (int j = 0; j < settingDataDic.Count; j++)
                        {
                            if (settingDataDic[j].ContainsKey("matchID") && settingDataDic[j]["matchID"].ToString() == matchId[i])
                            {
                                settingDataDic.Remove(settingDataDic[j]);
                                isContain = true;
                                settingDic["settingData"] = Json.SerializeObject(settingDataDic);
                                break;
                            }
                        }
                    }
                }
                //viewdata
                if (settingDic.ContainsKey("viewData"))
                {
                    List<Dictionary<string, object>> viewDataDic = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(settingDic["viewData"].ToString());
                    if (viewDataDic != null && viewDataDic.Count > 0)
                    {
                        for (int m = 0; m < viewDataDic.Count; m++)
                        {
                            if (viewDataDic[m].ContainsKey("matchID") && viewDataDic[m]["matchID"].ToString() == matchId[i])
                            {
                                viewDataDic.Remove(viewDataDic[m]);
                                isContain = true;
                                settingDic["viewData"] = Json.SerializeObject(viewDataDic);
                                break;
                            }
                        }
                    }
                }

                if (isContain)
                {
                    //删除需要删除的场设置
                    //info2.settingData = newSetting;
                    //info2.viewData = newView; 
                    model.setting = Json.SerializeObject(settingDic);
                    //重新设置配置信息
                    ClubRoomSettingModel2 model2 = new ClubRoomSettingModel2()
                    {
                        child_game_id = model.child_game_id,
                        club_id = model.club_id,
                        game_id = model.game_id,
                        index = model.index,
                        mode = model.mode,
                        setting = model.setting,
                        type = model.type,
                        player_id = model.player_id
                    };
                    //ClubRoomSettingModel newSettingRes = SetRoomSetting(model2);
                    //if (newSettingRes == null)
                    //    WriteError("SetRoomSetting res err.", JsonConvert.SerializeObject(model));
                }
            }
            return null;
        }



        //游戏收益
        public static List<GameIncome> GetGameIncome(long start, long end, string type, string chargeid, string roomid, string number)
        {
            try
            {
                if (start < 1 || end < 1 || string.IsNullOrEmpty(type) || (string.IsNullOrEmpty(chargeid) && string.IsNullOrEmpty(roomid)))
                    return null;
                //List<GameIncome> list= dal.GetGameIncome(start, end, type, chargeid, roomid, number);
                string listStr = dal.GetGameIncome2(start, end, type, chargeid, roomid, number);
                listStr = listStr.Replace("\\U000", "[emoji]").Replace("\\u000", "[emoji]");
                List<GameIncome> list = new List<GameIncome>();
                try
                {
                    list = JsonConvert.DeserializeObject<List<GameIncome>>(listStr);
                }
                catch (Exception ex2)
                {
                    Base.WriteError("GetGameIncome Deserialize Convert ex：", ex2.Message);
                    return null;
                }
                if (list == null || list.Count < 1) return list;
                List<GameIncome> newList = new List<GameIncome>();
                foreach (GameIncome income in list)
                {
                    if (income != null && income.NickList.Count > 0)
                    {
                        List<string> nickNewList = new List<string>();
                        foreach (string nick in income.NickList)
                        {
                            string newNick = nick;
                            if (nick.IndexOf("[emoji]") >= 0)
                            {
                                newNick = nick.Replace("[emoji]", "\\U000");
                                //newNick = Emoji.GetEmoji(newNick);//暂时不可用---前端转emoji
                                //WriteError("newnick2222:", newNick);
                            }
                            nickNewList.Add(newNick);
                        }
                        income.NickList = nickNewList;
                    }
                    newList.Add(income);
                }
                return newList;
                //GameIncome gi = new GameIncome()
                //{
                //    ChargeIdList = new List<string> { "10A0000000000", "10A1111111111", "10A2222222222" },
                //    Child_Game_Index = "childgameindex0000",
                //    IncomeList = new List<string> { "income000", "income111", "income222" },
                //    InterestList = new List<string> { "interest000", "interest111", "interest222" },
                //    MatchId = "96421",
                //    NickList = new List<string> { "nick000", "nick111", "nick222" },
                //    Number = "number0",
                //    RoomId = "room0",
                //    Time = 229564799,
                //};
                //GameIncome gi2 = new GameIncome()
                //{
                //    ChargeIdList = new List<string> { "10A3333333333", "10A4444444444", "10A5555555555" },
                //    Child_Game_Index = "childgameindex111",
                //    IncomeList = new List<string> { "income333", "income444", "income555" },
                //    InterestList = new List<string> { "interest333", "interest444", "interest555" },
                //    MatchId = "96422",
                //    NickList = new List<string> { "nick333", "nick444", "nick555" },
                //    Number = "number2",
                //    RoomId = "room2",
                //    Time = 229564799,
                //};
                //List<GameIncome> list = new List<GameIncome>();
                //list.Add(gi);
                //list.Add(gi2);
                //return list;
            }
            catch (Exception ex)
            {
                Base.WriteError("GetGameIncome ex:", ex.Message);
            }
            return null;
        }
        //游戏录像
        public static string GetGameRec(long start, long end, string type, string chargeid, string roomid, string number)
        {
            try
            {
                if (start < 1 || end < 1 || string.IsNullOrEmpty(type) || (string.IsNullOrEmpty(chargeid) && string.IsNullOrEmpty(roomid)))
                    return null;
                return dal.GetGameRec(start, end, type, chargeid, roomid, number);
                //FileStream fsr = new FileStream("d:/1.txt", FileMode.Open);
                //byte[] readBytes = new byte[1024 * 1024];
                //int count = fsr.Read(readBytes, 0, readBytes.Length);
                //string readStr = Encoding.UTF8.GetString(readBytes, 0, count);
                //fsr.Close();
                //return readStr;
            }
            catch (Exception ex)
            {
                Base.WriteError("GetGameRec ex:", ex.Message);
            }
            return null;
        }

    }
}
