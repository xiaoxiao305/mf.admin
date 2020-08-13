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
using System.Linq;


namespace MF.Admin.BLL
{

    public class GameBLL : Base
    {
        private static GameDAL dal = new GameDAL();
        private static UserDAL userDal = new UserDAL();
        private static GuildDAL guildDal = new GuildDAL();
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
                WriteLog("readjson:", jsonText);
                Dictionary<string, Dictionary<string, object>> jsonObj = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, object>>>(jsonText);
                if (jsonObj != null && jsonObj.ContainsKey(key)) return jsonObj[key];
            }
            return null;
        }

        //黑名单
        //public static List<GameBlackUserInfo> GetBlackUsersDetail2(List<GameBlackUserInfo> list, string gameType)
        //{
        //    try
        //    {
        //        if (list == null || list.Count < 1) return null;
        //        List<string> accList = new List<string>();
        //        foreach (var item in list)
        //        {
        //            accList.Add(item.Account);
        //        }
        //        List<Dictionary<string, string>> infoList = userDal.GetUserInfoList(accList.ToArray());
        //        if (infoList == null || infoList.Count < 1) return list;
        //        List<GameBlackUserInfo> newList2 = new List<GameBlackUserInfo>();
        //        foreach (GameBlackUserInfo info in list)
        //        {
        //            foreach (Dictionary<string, string> dic in infoList)
        //            {
        //                if (info.Account.ToLower().Equals(dic["Account"].ToString()))
        //                {
        //                    info.NickName = dic["Nickname"].ToString();
        //                    if (dic["ChargeId"] != null && !string.IsNullOrEmpty(dic["ChargeId"].ToString()))
        //                    {
        //                        info.ChargeId = dic["ChargeId"].ToString();
        //                        Dictionary<string, object> listinfos = guildDal.GetMembersList(dic["ChargeId"].ToString());
        //                        if (listinfos != null && listinfos.Count > 0 && listinfos.ContainsKey("clubs"))
        //                        {
        //                            List<string> club_ids = (listinfos["clubs"] as Newtonsoft.Json.Linq.JArray).ToObject<List<string>>();
        //                            if (club_ids != null && club_ids.Count > 0)
        //                                info.ClubId = "[" + String.Join(",", club_ids) + "]";
        //                            //money
        //                            //Base.WriteLog("gameType:", gameType);
        //                            if (string.IsNullOrEmpty(gameType))
        //                            {
        //                                //Base.WriteLog("jsonpath 111:", gameType);
        //                                string jsonPath = HttpContext.Current.Server.MapPath("/common/js/gameback.json");
        //                                //Base.WriteLog("jsonpath:", jsonPath);
        //                                Dictionary<string, object> gamebackObj = Readjson(info.GameId, jsonPath);
        //                                if (gamebackObj != null && gamebackObj.ContainsKey("type"))
        //                                    gameType = gamebackObj["type"].ToString();
        //                                //Base.WriteLog("gameType:", gameType);
        //                            }
        //                            if (string.IsNullOrEmpty(gameType)) break;
        //                            Dictionary<string, object> moneyDic = dal.GetWinnMoney(gameType, dic["ChargeId"].ToString());
        //                            if (moneyDic != null && moneyDic.ContainsKey("lose"))
        //                            {
        //                                info.Money = moneyDic["lose"].ToString();
        //                            }
        //                        }
        //                    }
        //                    break;
        //                }
        //            }
        //            newList2.Add(info);
        //        }
        //        return newList2;
        //    }
        //    catch (Exception ex)
        //    {
        //        Base.WriteError("GetBlackUsersDetail ex:", ex.Message);
        //        return list;
        //    }
        //}
        public static string GetUserGameMoney(string chargeId, string gameType)
        {
            try
            {
                if (string.IsNullOrEmpty(chargeId) || string.IsNullOrEmpty(gameType)) return "0";
                Dictionary<string, object> clubInfos = guildDal.GetMembersList(chargeId);
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

        //public static List<GameBlackUserInfo> GetBlackUsersDetail(List<GameBlackUserInfo> list)
        //{
        //    try
        //    {
        //        if (list == null || list.Count < 1) return null;

        //        List<string> accList = new List<string>();
        //        foreach (var blackUser in list)
        //        {
        //            if (Cache.CacheAccountList != null &&
        //                Cache.CacheAccountList.ContainsKey(blackUser.Account.ToLower()))
        //                continue;
        //            accList.Add(blackUser.Account);
        //        }
        //        if (accList.Count > 0)
        //            userDal.GetUserInfoList(accList.ToArray());
        //        List<GameBlackUserInfo> newList2 = new List<GameBlackUserInfo>();
        //        foreach (GameBlackUserInfo info in list)
        //        {
        //            info.NickName = userDal.GetNickByAcc(info.Account);
        //            if (string.IsNullOrEmpty(info.ChargeId))
        //                info.ChargeId = userDal.GetChargeIdByAcc(info.Account);
        //            newList2.Add(info);
        //        }
        //        return newList2;
        //    }
        //    catch (Exception ex)
        //    {
        //        Base.WriteError("GetBlackUsersDetail ex:", ex.Message);
        //        return list;
        //    }
        //}
        /// <summary>
        /// 获取黑名单列表
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="gameType"></param>
        /// <param name="account"></param> 
        /// <returns></returns>
        //public static List<GameBlackUserInfo> GetGameBlackUsers(long gameId, long field, string value)
        //{
        //    //if (gameId < 1 || string.IsNullOrEmpty(gameType))
        //    //    return null;
        //    string account = field == 2 ? value : "";
        //    string chargeid = field == 1 ? value : "";
        //    List<GameBlackUserInfo> list = GetGameBlackUsersData(gameId, account, chargeid);
        //    if (list == null || list.Count < 1) return null;
        //    //return list;
        //    return GetBlackUsersDetail(list);
        //}
        //public static List<GameBlackUserInfo> GetAuditBlackUsers(long gameId, long field, string value)
        //{
        //    string account = field == 2 ? value : "";
        //    string chargeid = field == 1 ? value : "";
        //    List<GameBlackUserInfo> list = dal.GetGameBlackUsers(gameId, account, chargeid, 2);
        //    if (list == null || list.Count < 1) return null;
        //    //return list;
        //    return GetBlackUsersDetail(list);
        //}

        //public static List<GameBlackUserInfo> GetGameBlackUsersData(long gameId, string account, string chargeid)
        //{
        //    try
        //    {
        //        //if (gameId < 1) return null;
        //        return dal.GetGameBlackUsers(gameId, account, chargeid, 1);
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteError("GuildBLL GetGameBlackUsers ex:", ex.Message);
        //    }
        //    return null;
        //}
        public static Dictionary<string, string> AddBlackUser(string[] gameidList, string chargeId,
            string[] valueList, string[] levelStrList, string remark, long isConfirm)
        {
            try
            {
                if (gameidList == null || gameidList.Length < 1 || string.IsNullOrEmpty(chargeId)
                || valueList == null || valueList.Length < 1
                || levelStrList == null || levelStrList.Length < 1) return null;
                string gameId = "";
                string value = "";
                string levelStr = "";
                string account = "";
                if (isConfirm == 1)
                    account = userDal.GetAccByChargeId(chargeId);
                for (int i = 0; i < gameidList.Length; i++)
                {
                    gameId = gameidList[i];
                    value = valueList[i];
                    levelStr = levelStrList[i];
                    value = "[" + value + "]";
                    var r = AddBlackUser(gameId, chargeId, value, levelStr, remark);
                    if (isConfirm == 1)
                    {
                        if (r != null)
                        {
                            if (r.ContainsKey("succeed") && r.ContainsKey("message"))
                            {
                                if (bool.Parse(r["succeed"].ToString()))
                                    ConfirmBlackUser(account, chargeId, "NONE_" + gameId);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteError("BLL AddBlackUser ex:", ex.Message, " gameidList:", String.Join(",", gameidList),
                    " chargeId:", chargeId, " valueList:", String.Join(",", valueList)
                    , " levelStrList:", String.Join(",", levelStrList), " remark:", remark);
            }
            return null;
        }
        public static Dictionary<string, string> AddBlackUserRange(string[] gameidList, string[] chargeId,
    string[] valueList, string[] levelStrList, string remark, long isConfirm)
        {
            try
            {
                if (gameidList == null || gameidList.Length < 1
                    || chargeId == null || chargeId.Length < 1
                || valueList == null || valueList.Length < 1
                || levelStrList == null || levelStrList.Length < 1) return null;
                Dictionary<string, string> r = AddBlackUserRange(gameidList, chargeId, valueList, levelStrList, remark);
                if (r != null)
                {
                    if (r.ContainsKey("succeed") && r.ContainsKey("message"))
                    {
                        if (bool.Parse(r["succeed"].ToString()) && isConfirm == 1)
                            ConfirmBlackUserRange(gameidList, chargeId);
                    }
                }
            }
            catch (Exception ex)
            {
                WriteError("BLL AddBlackUserRange ex:", ex.Message, " gameidList:", String.Join(",", gameidList),
                    " chargeId:", chargeId.ToString(), " valueList:", String.Join(",", valueList)
                    , " levelStrList:", String.Join(",", levelStrList), " remark:", remark);
            }
            return null;
        }
        public static Dictionary<string, string> AddBlackUserRange(string[] gameidList, string[] chargeId,
      string[] valueList, string[] levelStrList, string remark)
        {
            try
            {
                AjaxResult<bool> res = new AjaxResult<bool>() { code = 0, msg = "" };
                Dictionary<string, string> r = dal.AddBlackUserRange(gameidList, chargeId, valueList, levelStrList, remark);
                int oprState = 0;
                string msg = string.Format("操作游戏{0}，添加黑名单【{1}】值为【{2}】", string.Join(",", gameidList)
                    , string.Join(",", chargeId), string.Join(",", valueList));
                if (r != null)
                {
                    if (r.ContainsKey("succeed") && r.ContainsKey("message"))
                    {
                        if (bool.Parse(r["succeed"].ToString()))
                        {
                            msg += "成功";
                            oprState = 1;
                        }
                        else
                            msg += "失败" + r["message"];
                    }
                    else
                        msg += "失败 res is err";
                }
                AdminBLL.WriteSystemLog(CurrentUser.Account, ClientIP, msg, "BLL.AddBlackUser", oprState, SystemLogEnum.ADDBLACKUSER);
                return r;
            }
            catch (Exception ex)
            {
                WriteError("BLL AddBlackUserRange ex:", ex.Message, " gameId:", string.Join(",", gameidList)
                    , " chargeId:", string.Join(",", chargeId), " value:", string.Join(",", valueList)
                    , " levelStr:", string.Join(",", levelStrList), " remark:", remark);
            }
            return null;
        }
        public static Dictionary<string, string> AddBlackUser(string gameId, string chargeId, string value, string levelStr, string remark)
        {
            try
            {
                AjaxResult<bool> res = new AjaxResult<bool>() { code = 0, msg = "" };
                Dictionary<string, string> r = dal.AddBlackUser(gameId, chargeId.Trim(), value.Trim(), levelStr, remark);
                int oprState = 0;
                string msg = string.Format("操作游戏{0}，添加黑名单【{1}】值为【{2}】失败。", gameId, chargeId, value);
                if (r != null)
                {
                    if (r.ContainsKey("succeed") && r.ContainsKey("message"))
                    {
                        if (bool.Parse(r["succeed"].ToString()))
                        {
                            msg = string.Format("操作游戏{0}，添加黑名单【{1}】值为【{2}】成功", gameId, chargeId, value);
                            AdminBLL.WriteSystemLog(CurrentUser.Account, ClientIP, msg, "BLL.AddBlackUser", oprState, SystemLogEnum.ADDBLACKUSER);
                            return r;
                        }
                        else
                            msg += r["message"];
                    }
                    else
                        msg += " res is err";
                }
                AdminBLL.WriteSystemLog(CurrentUser.Account, ClientIP, msg, "AjaxRequest.AddBlackUser", oprState, SystemLogEnum.ADDBLACKUSER);
            }
            catch (Exception ex)
            {
                WriteError("BLL AddBlackUser ex:", ex.Message, " gameId:", gameId, " chargeId:", chargeId, " value:", value
                    , " levelStr:", levelStr, " remark:", remark);
            }
            return null;
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
            try
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
            catch (Exception ex)
            {
                WriteError("BLL ConfirmBlackUser ex:", ex.Message);
            }
            return null;
        }
        public static Dictionary<string, string> ConfirmBlackUserRange(string[] gameids, string[] chargeids)
        {
            try
            {
                if (gameids == null || gameids.Length < 1
                    || chargeids == null || chargeids.Length < 1) return null;
                Dictionary<string, string> r = dal.ConfirmBlackUserRange(gameids, chargeids);
                int oprState = 0;
                string msg = string.Format("批量审核黑名单，游戏：{0},UID：{1}.确认实锤", string.Join(",", gameids), string.Join(",", chargeids));
                if (r != null)
                {
                    if (r.ContainsKey("succeed") && r.ContainsKey("message"))
                    {
                        if (bool.Parse(r["succeed"].ToString()))
                        {
                            oprState = 1;
                            msg += "成功。";
                        }
                        else
                            msg += "失败" + r["message"];
                    }
                    else
                        msg += "失败 res is err";
                }
                AdminBLL.WriteSystemLog(CurrentUser.Account, ClientIP, msg, "GameBLL.ConfirmBlackUser", oprState, SystemLogEnum.CONFIRMBLACKUSER);
                return r;
            }
            catch (Exception ex)
            {
                WriteError("BLL ConfirmBlackUser ex:", ex.Message);
            }
            return null;
        }

        public static Dictionary<string, string> DelBlackUser(string gameId, string account)
        {
            try
            {
                if (string.IsNullOrEmpty(gameId) || string.IsNullOrEmpty(account)) return null;
                Dictionary<string, string> r = dal.DelBlackUser(gameId, account);
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
                else
                    msg += " res is null";
                AdminBLL.WriteSystemLog(CurrentUser.Account, ClientIP, msg, "BLL.DelBlackUser", oprState, SystemLogEnum.DELBLACKUSER);
                return r;
            }
            catch (Exception ex)
            {
                WriteError("BLL DelBlackUser ex:", ex.Message, " gameid:", gameId, " account:", account);
            }
            return null;
        }
        public static Dictionary<string, object> SetWinnMoney(string type, string chargeid, string value)
        {
            try
            {
                if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(chargeid) || string.IsNullOrEmpty(type)) return null;
                Dictionary<string, object> r = dal.SetWinnMoney(type, chargeid, value);
                int oprState = 0;
                string msg = string.Format("设置玩家{0}，游戏【{1}】，对应输赢值为{2}失败。", chargeid, type, value);
                if (r != null)
                {
                    if (r.ContainsKey("ret"))
                    {
                        if (r["ret"].ToString() == "0")
                        {
                            oprState = 1;
                            msg = string.Format("设置玩家{0}，游戏【{1}】，对应输赢值为{2}成功。", chargeid, type, value);
                        }
                        else
                            msg += r["msg"];
                    }
                    else
                        msg += "res is err";
                }
                else
                    msg += "res is null";
                AdminBLL.WriteSystemLog(CurrentUser.Account, ClientIP, msg, "AjaxRequest.SetWinnMoney", oprState, SystemLogEnum.SETWINMONEY);
                return r;
            }
            catch (Exception ex)
            {
                WriteError("BLL SetWinnMoney ex:", ex.Message, " chargeid:", chargeid, " type:", type, " value:", value);
            }
            return null;
        }
        //删除用户黑名单+设置输赢值
        public static Dictionary<string, object> DelBlackUser(string gameId, string account, string type, string player_id, string value)
        {
            if (string.IsNullOrEmpty(gameId) || string.IsNullOrEmpty(account) || string.IsNullOrEmpty(type)) return null;
            //del
            Dictionary<string, string> r = DelBlackUser(gameId, account);
            int oprState = 0;
            string msg = "删除黑名单失败";
            if (r != null && r.ContainsKey("succeed") && r.ContainsKey("message") && bool.Parse(r["succeed"].ToString()))
            {
                //set
                if (!string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(player_id))
                {
                    var res = SetWinnMoney(type, player_id, value);
                    if (res != null && res.ContainsKey("ret") && res["ret"].ToString() == "0")
                    {
                        oprState = 1;
                        msg = "删除黑名单成功";
                    }
                    else
                        msg = "设置玩家输赢值失败";
                }
                else
                {
                    oprState = 1;
                    msg = "删除黑名单成功";
                }
            }
            return new Dictionary<string, object>() { { "ret", oprState }, { "msg", msg } };
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
        public static List<GameIncome> GetGameIncome2(long start, long end, string type, string chargeid, string roomid, string number)
        {
            try
            {
                string listStr = dal.GetGameIncome2(start, end, type, chargeid, roomid, number);
                listStr = listStr.Replace("\\U000", "[emoji]").Replace("\\u000", "[emoji]");
                List<GameIncome> list = new List<GameIncome>();
                list = JsonConvert.DeserializeObject<List<GameIncome>>(listStr);
                return list;
            }
            catch (Exception ex2)
            {
                Base.WriteError("GetGameIncome2 Deserialize Convert ex：", ex2.Message);
            }
            return null;
        }
        public static List<GameIncome> GetGameIncome(long start, long end, string type, string chargeid, string roomid, string number)
        {
            try
            {
                if (start < 1 || end < 1 || string.IsNullOrEmpty(type) || (string.IsNullOrEmpty(chargeid) && string.IsNullOrEmpty(roomid)))
                    return null;
                List<GameIncome> list = GetGameIncome2(start, end, type, chargeid, roomid, number);
                if (list == null || list.Count < 1) return list;
                //用户缓存
                //List<List<string>> chargeIdList = list.Select(t => t.ChargeIdList).ToList();
                //if (chargeIdList != null && chargeIdList.Count > 0)
                //{
                //    List<string> allChargeIds = new List<string>();
                //    foreach (List<string> item in chargeIdList)
                //    {
                //        allChargeIds.AddRange(item);
                //    }
                //    allChargeIds = allChargeIds.ToList().Except(Cache.CacheChargeidList.Keys.ToArray()).ToList();
                //    if (allChargeIds != null && allChargeIds.Count > 0)//差集>0
                //        userDal.QueryUserList(allChargeIds.ToArray());
                //}
                List<GameIncome> newList = new List<GameIncome>();
                foreach (GameIncome income in list)
                {
                    if (income == null) continue;
                    if (income.NickList.Count > 0)
                    {
                        List<string> nickNewList = new List<string>();
                        for (int i = 0; i < income.NickList.Count; i++)
                        {
                            string newNick = income.NickList[i];
                            if (newNick.IndexOf("[emoji]") >= 0)
                                newNick = newNick.Replace("[emoji]", "\\U000");
                            nickNewList.Add(newNick);
                        }
                        income.NickList = nickNewList;
                    }
                    newList.Add(income);
                }
                return newList;
            }
            catch (Exception ex)
            {
                Base.WriteError("GetGameIncome ex:", ex.Message);
            }
            return null;
        }
        //自动巡场
        private static List<AutoPatrol> GetAutoPatrolModel(string listStr)
        {
            List<AutoPatrol> list = null;
            if (!string.IsNullOrEmpty(listStr))
            {
                listStr = listStr.Replace("\\U000", "[emoji]").Replace("\\u000", "[emoji]");
                if (!string.IsNullOrEmpty(listStr))
                {
                    try
                    {
                        list = JsonConvert.DeserializeObject<List<AutoPatrol>>(listStr);
                    }
                    catch (Exception ex2)
                    {
                        Base.WriteError("GetAutoPatrolModel Deserialize Convert ex：", ex2.Message);
                    }
                }
            }
            return list;
        }
        public static List<AutoPatrol> GetLastGameRecords2(string gameIds)
        {
            try
            {
                //获取巡场数据+同桌巡场数据
                string listStr = dal.GetLastGameRecords();
                List<AutoPatrol> list = GetAutoPatrolModel(listStr);
                string listStr2 = dal.GetDeskMates();
                List<AutoPatrol> list2 = GetAutoPatrolModel(listStr2);
                if (list2 != null)
                {
                    if (list == null)
                        list = new List<AutoPatrol>();
                    list.AddRange(list2);
                }
                if (list == null || list.Count < 1) return null;
                //设置俱乐部+用户缓存
                List<List<string>> chargeIdList = list.Select(t => t.ChargeIds).ToList();
                if (chargeIdList != null && chargeIdList.Count > 0)
                {
                    List<string> allChargeIds = new List<string>();
                    foreach (List<string> item in chargeIdList)
                    {
                        allChargeIds.AddRange(item);
                    }
                    guildDal.GetClubByChargeId(allChargeIds);
                    userDal.QueryUserList(allChargeIds.ToArray());
                }
                //重组数据
                List<AutoPatrol> newList = new List<AutoPatrol>();
                string nick = "";
                foreach (AutoPatrol patrol in list)
                {
                    if (gameIds.Contains(patrol.GameId)) continue;
                    if (patrol == null) continue;
                    if ((patrol.ChargeIds == null || patrol.ChargeIds.Count < 1) && (patrol.NickNames == null || patrol.NickNames.Count < 1)) continue;
                    if (patrol.Count < 1 && patrol.ChargeIds.Count == 2) continue;//巡场数据，屏蔽双人游戏+2人玩游戏
                    List<string> nickNewList = new List<string>();
                    Dictionary<string, string> nickClub = new Dictionary<string, string>();
                    for (int i = 0; i < patrol.ChargeIds.Count; i++)
                    {
                        //俱乐部
                        if (patrol.ClubIds == null)
                            patrol.ClubIds = new List<string>();
                        List<string> clubs = guildDal.GetCacheClubIdFromCache(patrol.ChargeIds[i]);
                        if (clubs != null && clubs.Count > 0)
                        {
                            string clubStr = String.Join(",", clubs).Replace(",", ". ");
                            patrol.ClubIds.Add(clubStr);
                        }
                        else
                            patrol.ClubIds.Add("");
                        //注册时间、最后一次登录IP
                        if (patrol.RegiTimes == null)
                            patrol.RegiTimes = new List<int>();
                        //if (patrol.LastLoginIps == null)
                        //    patrol.LastLoginIps = new List<string>();
                        if (patrol.GUIDList == null)
                            patrol.GUIDList = new List<string>();
                        Users cacheUser = userDal.GetCacheUserByChargeIdFromCache(patrol.ChargeIds[i]);
                        if (cacheUser != null)
                        {
                            patrol.RegiTimes.Add(cacheUser.Regitime);
                            //patrol.LastLoginIps.Add(cacheUser.LastIp); 
                            patrol.GUIDList.Add(cacheUser.GUID);
                        }
                        else
                        {
                            patrol.RegiTimes.Add(0);
                            //patrol.LastLoginIps.Add("");
                            patrol.GUIDList.Add("");
                        }
                        if (patrol.NickNames != null && patrol.NickNames.Count > 0)
                        {
                            nick = patrol.NickNames[i];
                            //黑名单
                            if (nick.IndexOf("黑名单") >= 0)
                            {
                                if (!string.IsNullOrEmpty(patrol.ClubIds[i]) && !nickClub.ContainsKey(patrol.ClubIds[i]))
                                    nickClub.Add(patrol.ClubIds[i], nick);
                            }
                            //巡场数据，屏蔽都是黑名单玩家
                            if (patrol.Count > 0 || (patrol.Count < 1 && nickClub.Count != patrol.NickNames.Count))
                            {
                                //昵称
                                string newNick = (nick.IndexOf("[emoji]") >= 0) ? nick.Replace("[emoji]", "\\U000") : nick;
                                nickNewList.Add(newNick);
                                patrol.NickNamesNew = nickNewList;
                                //标注同一俱乐部
                                if (patrol.IsBlackClub == null)
                                    patrol.IsBlackClub = new List<int>();
                                if (nickClub.Count > 0)
                                {
                                    bool isContain = false;
                                    for (int clubI = 0; clubI < patrol.ClubIds.Count; clubI++)
                                    {
                                        if (string.IsNullOrEmpty(patrol.ClubIds[clubI])) continue;
                                        if (nickClub.ContainsKey(patrol.ClubIds[clubI]))
                                        {
                                            if (patrol.ClubIds[i] == patrol.ClubIds[clubI]
                                                && patrol.NickNames[i] != nickClub[patrol.ClubIds[clubI]])
                                            {
                                                isContain = true;
                                                break;
                                            }
                                        }
                                    }
                                    if (isContain)
                                        patrol.IsBlackClub.Add(1);
                                    else
                                        patrol.IsBlackClub.Add(0);
                                }
                                else
                                    patrol.IsBlackClub.Add(0);
                            }
                        }
                    }
                    newList.Add(patrol);
                }
                return newList;
            }
            catch (Exception ex)
            {
                Base.WriteError("GetLastGameRecords2 ex:", ex.Message);
            }
            return null;
        }
        public static List<AutoPatrol> GetLastGameRecords()
        {
            WriteLog("GetLastGameRecords start:", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
            try
            {
                //获取巡场数据+同桌巡场数据
                string listStr = dal.GetLastGameRecords();
                List<AutoPatrol> list = GetAutoPatrolModel(listStr);
                string listStr2 = dal.GetDeskMates();
                List<AutoPatrol> list2 = GetAutoPatrolModel(listStr2);
                if (list2 != null)
                {
                    if (list == null)
                        list = new List<AutoPatrol>();
                    list.AddRange(list2);
                }
                if (list == null || list.Count < 1) return null;
                //设置俱乐部+用户缓存
                List<List<string>> chargeIdList = list.Select(t => t.ChargeIds).ToList();
                if (chargeIdList != null && chargeIdList.Count > 0)
                {
                    List<string> allChargeIds = new List<string>();
                    foreach (List<string> item in chargeIdList)
                    {
                        allChargeIds.AddRange(item);
                    }
                    guildDal.GetClubByChargeId(allChargeIds);
                    userDal.QueryUserList(allChargeIds.ToArray());
                }
                //重组数据
                List<AutoPatrol> newList = new List<AutoPatrol>();
                foreach (AutoPatrol patrol in list)
                {
                    if (patrol == null) continue;
                    if (patrol.NickNames != null && patrol.NickNames.Count > 0)
                    {
                        List<string> nickNewList = new List<string>();
                        int blackCount = 0;
                        Dictionary<string, string> nickClub = new Dictionary<string, string>();
                        int index = 0;
                        foreach (string nick in patrol.NickNames)
                        {
                            //黑名单判断
                            if (nick.IndexOf("黑名单") >= 0)
                            {
                                nickClub.Add(patrol.ClubIds[index], nick);
                                blackCount++;
                            }
                            string newNick = (nick.IndexOf("[emoji]") >= 0) ? nick.Replace("[emoji]", "\\U000") : nick;
                            nickNewList.Add(newNick);
                            index++;
                        }
                        if (patrol.Count < 1 && blackCount == patrol.NickNames.Count) continue;//巡场数据，屏蔽同时在黑名单内的用户 
                        patrol.NickNames = nickNewList;
                        if (blackCount > 0)
                        {
                            if (patrol.IsBlackClub == null)
                                patrol.IsBlackClub = new List<int>();
                            index = 0;
                            for (int i = 0; i < patrol.ClubIds.Count; i++)
                            {
                                if (nickClub.ContainsKey(patrol.ClubIds[i]))
                                {
                                    if (patrol.NickNames[i] != nickClub[patrol.ClubIds[i]])
                                    {
                                        patrol.IsBlackClub.Add(1);//标注同一俱乐部
                                    }
                                }
                                else
                                    patrol.IsBlackClub.Add(0);
                            }
                        }
                    }
                    if (patrol.ChargeIds != null && patrol.ChargeIds.Count > 0)
                    {
                        if (patrol.Count < 1 && patrol.ChargeIds.Count == 2) continue;//巡场数据，屏蔽双人游戏+2人玩游戏
                        foreach (string chargeid in patrol.ChargeIds)
                        {
                            //重组俱乐部数据
                            if (patrol.ClubIds == null)
                                patrol.ClubIds = new List<string>();
                            List<string> clubs = guildDal.GetCacheClubIdFromCache(chargeid);
                            if (clubs != null && clubs.Count > 0)
                            {
                                string clubStr = String.Join(",", clubs).Replace(",", ". ");
                                patrol.ClubIds.Add(clubStr);
                            }
                            else
                                patrol.ClubIds.Add("");
                            //重组用户数据【注册时间、最后一次登录IP】
                            if (patrol.RegiTimes == null)
                                patrol.RegiTimes = new List<int>();
                            if (patrol.LastLoginIps == null)
                                patrol.LastLoginIps = new List<string>();
                            Users cacheUser = userDal.GetCacheUserByChargeIdFromCache(chargeid);
                            if (cacheUser != null)
                            {
                                patrol.RegiTimes.Add(cacheUser.Regitime);
                                patrol.LastLoginIps.Add(cacheUser.LastIp);
                            }
                            else
                            {
                                patrol.RegiTimes.Add(0);
                                patrol.LastLoginIps.Add("");
                            }
                        }
                    }
                    newList.Add(patrol);
                }
                WriteLog("GetLastGameRecords end:", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                return newList;
            }
            catch (Exception ex)
            {
                Base.WriteError("GetLastGameRecords ex:", ex.Message);
            }
            WriteLog("GetLastGameRecords end222:", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
            return null;
        }
        //同桌玩家
        public static List<AutoPatrol> GetDeskMates()
        {
            try
            {
                string listStr = dal.GetDeskMates();
                if (string.IsNullOrEmpty(listStr)) return null;
                listStr = listStr.Replace("\\U000", "[emoji]").Replace("\\u000", "[emoji]");
                if (string.IsNullOrEmpty(listStr)) return null;
                List<AutoPatrol> list = new List<AutoPatrol>();
                try
                {
                    list = JsonConvert.DeserializeObject<List<AutoPatrol>>(listStr);
                }
                catch (Exception ex2)
                {
                    Base.WriteError("GetDeskMates Deserialize Convert ex：", ex2.Message);
                    return null;
                }
                if (list == null || list.Count < 1) return list;
                //设置俱乐部+用户缓存
                //List<List<string>> chargeIdList = list.Select(t => t.ChargeIds).ToList();
                //if (chargeIdList != null && chargeIdList.Count > 0)
                //{
                //    List<string> allChargeIds = chargeIdList.ConvertAll(obj => string.Format("{0}", obj));
                //    List<string> clubChargeIds = new List<string>();
                //    List<string> userChargeIds = new List<string>();
                //    List<string> clubCacheData = new List<string>();
                //    Users userCacheData = new Users();
                //    foreach (string chargeIdSearch in allChargeIds)
                //    {
                //        clubCacheData = guildDal.GetCacheClubIdFromCache(chargeIdSearch);
                //        if (clubCacheData == null || clubCacheData.Count < 1)
                //            clubChargeIds.Add(chargeIdSearch);
                //        userCacheData = userDal.GetCacheUserByChargeIdFromCache(chargeIdSearch);
                //        if (userCacheData == null)
                //            userChargeIds.Add(chargeIdSearch);
                //    }
                //    if(clubChargeIds.Count>0)
                //    guildDal.GetClubByChargeId(clubChargeIds);
                //    if (userChargeIds.Count > 0)
                //        userDal.GetMemberInfo(userChargeIds.ToArray());
                //}
                ////获取黑名单数据
                //List<GameBlackUserInfo> blackList =new List<GameBlackUserInfo>();
                //List<GameBlackUserInfo> auditBlackList = new List<GameBlackUserInfo>();
                //List<GameBlackUserInfo> blackListTmp = new List<GameBlackUserInfo>();
                //List<GameBlackUserInfo> auditBlackListTmp = new List<GameBlackUserInfo>();
                //int gameId = 0;
                //foreach (AutoPatrol patrol in list)
                //{
                //    if (patrol == null || string.IsNullOrEmpty(patrol.GameId) || patrol.GameId=="") continue;
                //    int.TryParse(patrol.GameId, out gameId);
                //    if (gameId < 1) continue;
                //    blackList.AddRange(GetGameBlackUsersData(gameId, "", ""));
                //    auditBlackList.AddRange(GetAuditBlackUsers(gameId, 0, ""));
                //} 
                ////重组数据
                List<AutoPatrol> newList = new List<AutoPatrol>();
                //IEnumerable<GameBlackUserInfo> isBlackList = null;
                //IEnumerable<GameBlackUserInfo> isAuditBlackList = null;
                //int blackUserCount=0;
                foreach (AutoPatrol patrol in list)
                {
                    if (patrol == null) continue;
                    //if (patrol.ChargeIds != null && patrol.ChargeIds.Count > 0)
                    //{
                    //    if (patrol.ChargeIds.Count == 2) continue;//屏蔽双人游戏+2人玩游戏
                    //    foreach (string chargeid in patrol.ChargeIds)
                    //    {
                    //        //黑名单判断
                    //        if (blackList != null && blackList.Count > 0)
                    //            isBlackList = blackList.Where(a => a.ChargeId.ToUpper().Contains(chargeid)).ToList();
                    //        if (isBlackList != null && isBlackList.Count() > 0)
                    //            blackUserCount++;
                    //        else
                    //        {
                    //            if (auditBlackList != null && auditBlackList.Count > 0)
                    //                isAuditBlackList = auditBlackList.Where(a => a.ChargeId.ToUpper().Contains(chargeid)).ToList();
                    //            if (isAuditBlackList != null && isAuditBlackList.Count() > 0)
                    //                blackUserCount++;
                    //        }
                    //        if (blackUserCount == patrol.ChargeIds.Count) continue;//屏蔽同时在黑名单内的用户
                    //        //重组俱乐部数据
                    //        if (patrol.ClubIds == null)
                    //            patrol.ClubIds = new List<string>();
                    //        List<string> clubs = guildDal.GetCacheClubIdFromCache(chargeid);
                    //        if (clubs != null && clubs.Count > 0)
                    //        {
                    //            string clubStr = String.Join(",", clubs).Replace(",", ". ");
                    //            patrol.ClubIds.Add(clubStr);
                    //        }
                    //        else
                    //            patrol.ClubIds.Add("");
                    //        //重组用户数据【注册时间、最后一次登录IP】
                    //        if (patrol.RegiTimes == null)
                    //            patrol.RegiTimes = new List<int>();
                    //        if (patrol.LastLoginIps == null)
                    //            patrol.LastLoginIps = new List<string>();
                    //        Users cacheUser = userDal.GetCacheUserByChargeIdFromCache(chargeid);
                    //        if (cacheUser != null)
                    //        {
                    //            patrol.RegiTimes.Add(cacheUser.Regitime);
                    //            patrol.LastLoginIps.Add(cacheUser.LastIp);
                    //        }
                    //        else
                    //        {
                    //            patrol.RegiTimes.Add(0);
                    //            patrol.LastLoginIps.Add("");
                    //        }
                    //    }
                    //}
                    if (patrol.NickNames != null && patrol.NickNames.Count > 0)
                    {
                        List<string> nickNewList = new List<string>();
                        foreach (string nick in patrol.NickNames)
                        {
                            string newNick = (nick.IndexOf("[emoji]") >= 0) ? nick.Replace("[emoji]", "\\U000") : nick;
                            nickNewList.Add(newNick);
                        }
                        patrol.NickNames = nickNewList;
                    }
                    newList.Add(patrol);
                }
                return newList;
            }
            catch (Exception ex)
            {
                Base.WriteError("GetDeskMates ex:", ex.Message);
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
                if (type.ToLower().Equals("paodekuai"))//标准跑得快 数据源存的账号  2020-06-09 @李文波
                    chargeid = new UserDAL().GetAccByChargeId(chargeid);
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

        public static Dictionary<string, object> SetRedAlert(string gameType, string value)
        {
            if (string.IsNullOrEmpty(gameType) || string.IsNullOrEmpty(value)) return null;
            return dal.SetRedAlert(gameType, value);
        }
        public static Dictionary<string, object> DelRedAlert(string gameType)
        {
            if (string.IsNullOrEmpty(gameType)) return null;
            return dal.DelRedAlert(gameType);
        }
        public static Dictionary<string, object> GetRedAlert()
        {
            return dal.GetRedAlert();
        }
        public static List<Dictionary<string, object>> GetRedAlertPlayer(string[] gameIds, string[] gameTypes, long field, string value, long time)
        {
            List<Dictionary<string, object>> newList = new List<Dictionary<string, object>>();
            value = value.ToUpper();
            try
            {
                //设置【游戏输赢值配置缓存】
                if (Cache.CacheRedAlert != null && Cache.CacheRedAlert.Count > 0)
                {
                    List<string> newListGameTypes = gameTypes.ToList().Except(Cache.CacheRedAlert.Keys.ToArray()).ToList();
                    if (newListGameTypes != null && newListGameTypes.Count > 0)//差集>0
                        dal.GetRedAlert();
                }
                else
                    dal.GetRedAlert();
                var index = 0;
                foreach (var gameType in gameTypes)
                {
                    string gameValue = dal.GetCacheRedAlertFromCache(gameType);
                    if (string.IsNullOrEmpty(gameValue)) continue;
                    var res = dal.GetRedAlertPlayer(gameType, gameValue);
                    if (res == null || !res.ContainsKey(gameType)) continue;
                    List<Dictionary<string, object>> list = res[gameType];
                    //组装没有缓存的chargeid集合
                    List<object> chargeIdList = list.Select(t => t.ContainsKey("player_id") ? t["player_id"] : "").ToList();
                    List<GameBlackUserInfo> blackList = null;
                    if (chargeIdList != null && chargeIdList.Count > 0)
                    {
                        List<string> newChargeIdList = chargeIdList.ConvertAll(obj => string.Format("{0}", obj));
                        userDal.QueryUserList(newChargeIdList.ToArray());
                        guildDal.GetClubByChargeId(newChargeIdList);
                        blackList = dal.GetGameBlackUsersRange(new long[] { int.Parse(gameIds[index]) }, null, newChargeIdList.ToArray(), null, 0);
                    }
                    //List<GameBlackUserInfo> blackList = dal.GetGameBlackUsers(int.Parse(gameIds[index]), "", "", 1);
                    //List<GameBlackUserInfo> auditBlackList = dal.GetGameBlackUsers(int.Parse(gameIds[index]), "", "", 2);

                    index++;
                    //重组数据
                    Users cacheUser = new Users();
                    string account = "", nick = "", player_id = "";
                    int regiTime = 0, blackType = 0;
                    foreach (var item in list)
                    {
                        account = ""; nick = ""; regiTime = 0; blackType = 0;
                        if (!item.ContainsKey("player_id") || item["player_id"] == null
                            || item["player_id"].ToString() == "" || item["player_id"].ToString().ToUpper() == "NULL") continue;
                        player_id = item["player_id"].ToString().ToUpper();
                        cacheUser = userDal.GetCacheUserByChargeIdFromCache(player_id);
                        if (cacheUser != null)
                        {
                            account = cacheUser.Account;
                            nick = cacheUser.Nickname;
                            regiTime = cacheUser.Regitime;
                        }
                        if (time > 0 && regiTime < time) continue;
                        if (!string.IsNullOrEmpty(value))
                        {
                            if (field == 1 && !player_id.Equals(value)) continue;
                            if (field == 2 && !account.ToUpper().Equals(value)) continue;
                        }
                        item.Add("account", account);
                        item.Add("nick", nick);
                        item.Add("regTime", regiTime);
                        item.Add("clubId", guildDal.GetCacheClubId(player_id));
                        IEnumerable<GameBlackUserInfo> isBlackList = null;
                        if (blackList != null && blackList.Count > 0)
                            isBlackList = blackList.Where(a => a.ChargeId.ToUpper().Contains(player_id)).ToList();
                        if (isBlackList != null && isBlackList.Count() > 0)
                            blackType = 1;
                        //else
                        //{
                        //    IEnumerable<GameBlackUserInfo> isAuditBlackList = null;
                        //    if (auditBlackList != null && auditBlackList.Count > 0)
                        //        isAuditBlackList = auditBlackList.Where(a => a.ChargeId.ToUpper().Contains(player_id)).ToList();
                        //    if (isAuditBlackList != null && isAuditBlackList.Count() > 0)
                        //        blackType = 2;
                        //}
                        item.Add("blackType", blackType);
                        newList.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                WriteError("BLL GetRedAlertPlayer ex:", ex.Message);
            }
            return newList;
        }
        public static List<Dictionary<string, object>> GetGameMoney(string[] gameTypes, string chargeId)
        {
            if (string.IsNullOrEmpty(chargeId)) return null;
            chargeId = chargeId.ToUpper();
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            var chargeids = new string[] { chargeId };
            var chargeExcept = chargeids.Except(Cache.CacheChargeidList.Keys.ToArray()).ToArray();
            if (chargeExcept != null && chargeExcept.Length > 0)
                userDal.QueryUserList(chargeExcept);
            Users u = userDal.GetCacheUserByChargeId(chargeId);
            foreach (string gameType in gameTypes)
            {
                Dictionary<string, object> moneyDic = dal.GetWinnMoney(gameType, chargeId);
                if (moneyDic == null) continue;
                if (u != null)
                {
                    moneyDic.Add("Account", u.Account);
                    moneyDic.Add("Nickname", u.Nickname);
                }
                else
                {
                    moneyDic.Add("Account", "");
                    moneyDic.Add("Nickname", "");
                }
                moneyDic.Add("GameType", gameType);
                list.Add(moneyDic);
            }
            return list;

        }



        //public static GameBlackUserInfoNew GetGameBlackUsersDataNew(long pageSize, long pageIndex, long gameId, string account, string chargeid)
        //{
        //    try
        //    {
        //        //if (gameId < 1) return null;
        //        return dal.GetGameBlackUsersNew(pageSize,  pageIndex, gameId, account, chargeid, 1);
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteError("GuildBLL GetGameBlackUsersDataNew ex:", ex.Message);
        //    }
        //    return null;
        //}
        //public static GameBlackUserInfoNew GetBlackUsersDetailNew(GameBlackUserInfoNew data)
        //{
        //    try
        //    {
        //        List<GameBlackUserInfo> list = data.Items;
        //        if (list == null || list.Count < 1) return data;
        //        List<string> accList = new List<string>();
        //        foreach (var blackUser in list)
        //        {
        //            if (Cache.CacheAccountList != null &&
        //                Cache.CacheAccountList.ContainsKey(blackUser.Account.ToLower()))
        //                continue;
        //            accList.Add(blackUser.Account);
        //        }
        //        if (accList.Count > 0)
        //            userDal.GetUserInfoList(accList.ToArray());
        //        List<GameBlackUserInfo> newList2 = new List<GameBlackUserInfo>();
        //        foreach (GameBlackUserInfo info in list)
        //        {
        //            info.NickName = userDal.GetNickByAcc(info.Account);
        //            if (string.IsNullOrEmpty(info.ChargeId))
        //                info.ChargeId = userDal.GetChargeIdByAcc(info.Account);
        //            newList2.Add(info);
        //        }
        //         data.Items= newList2; 
        //    }
        //    catch (Exception ex)
        //    {
        //        Base.WriteError("GetBlackUsersDetail ex:", ex.Message);
        //    }
        //    return data;
        //}




        /// <summary>
        /// 获取黑名单列表【不分页】
        /// </summary>
        /// <param name="gameId">游戏ID</param>
        /// <param name="field">1chargeid  2account</param>
        /// <param name="value">查询值</param>
        /// <param name="type">1黑名单  2待审核黑名单</param>
        /// <returns></returns>
        public static List<GameBlackUserInfo> GetBlackUsers(long gameId, long field, string value, int type)
        {
            string account = field == 2 ? value : "";
            string chargeid = field == 1 ? value : "";
            List<GameBlackUserInfo> list = dal.GetGameBlackUsers(gameId, account, chargeid, type);
            if (list == null || list.Count < 1) return null;
            return ResetBlackUserModel(list);
        }

        /// <summary>
        /// 获取黑名单列表【分页】
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="gameId">游戏ID</param>
        /// <param name="field">1chargeid  2account</param>
        /// <param name="value">查询值</param>
        /// <param name="type">1黑名单  2待审核黑名单</param>
        /// <returns></returns>
        public static GameBlackUserInfoNew GetGameBlackUsersNew(long pageSize, long pageIndex, long gameId, long field, string value, int type)
        {
            try
            {
                //if (gameId < 1 || string.IsNullOrEmpty(gameType))
                //    return null;
                //获取数据
                string account = field == 2 ? value : "";
                string chargeid = field == 1 ? value : "";
                GameBlackUserInfoNew res = dal.GetGameBlackUsersNew(pageSize, pageIndex, gameId, account, chargeid, type);
                if (res == null || res.Items == null || res.Items.Count < 1) return null;
                res.Items = ResetBlackUserModel(res.Items);
                return res;
            }
            catch (Exception ex)
            {
                WriteError("BLL GetGameBlackUsersNew ex:", ex.Message);
            }
            return null;
        }

        /// <summary>
        /// 重组黑名单信息【添加昵称、chargeid的显示】
        /// </summary>
        /// <param name="list">原有黑名单集合</param>
        /// <returns></returns>
        public static List<GameBlackUserInfo> ResetBlackUserModel(List<GameBlackUserInfo> list)
        {
            try
            {
                if (list == null || list.Count < 1) return null;
                //找集 去重
                List<string> accList = list.Select(s => s.Account).Distinct().ToList();
                if (Cache.CacheAccountList != null && Cache.CacheAccountList.Count > 0)
                {
                    //找差集
                    accList = accList.Except(Cache.CacheAccountList.Keys.ToArray()).ToList();
                }
                if (accList != null && accList.Count > 0)
                    userDal.GetUserInfoList(accList.ToArray());
                List<GameBlackUserInfo> newList = new List<GameBlackUserInfo>();
                Users cacheUser = null;
                foreach (GameBlackUserInfo info in list)
                {
                    cacheUser = userDal.GetCacheUserByAccountFromCache(info.Account);
                    info.NickName = cacheUser == null ? "" : cacheUser.Nickname;
                    //info.ChargeId = cacheUser == null ? "" : cacheUser.ChargeId;
                    newList.Add(info);
                }
                return newList;
            }
            catch (Exception ex)
            {
                Base.WriteError("ResetBlackUserModel ex:", ex.Message);
            }
            return list;
        }


        public static List<NewGameUsers> GetNewGameUsers(long pageSize, long pageIndex,
            long time, long gameId, long field, string value, out int rowCount)
        {
            rowCount = 0;
            try
            {
                value = value.Trim();
                var search = new NewGameUsersSearch();
                search.PageSize = (int)pageSize;
                search.PageIndex = (int)pageIndex;
                search.Where = " 1=1 ";
                if (!string.IsNullOrEmpty(value) && field == 1)
                    search.Where += " and ChargeId='" + value + "'";
                if (gameId > 0)
                    search.Where += " and GameId=" + gameId;
                List<NewGameUsers> list = dal.GetNewGameUsers(search, out rowCount);
                if (list == null || list.Count < 1) return null;
                string[] chargeids = list.Select(t => t.ChargeId).ToArray();
                UserBLL.SetChargeIdCache(chargeids);
                guildDal.GetClubByChargeId(chargeids.ToList());
                List<NewGameUsers> newList = new List<NewGameUsers>();
                foreach (NewGameUsers record in list)
                {
                    //Account NickName Guid  LoginIP
                    Users u = userDal.GetCacheUserByChargeIdFromCache(record.ChargeId);
                    if (u != null)
                    {
                        record.Account = u.Account;
                        record.NickName = u.Nickname;
                        record.Guid = u.GUID;
                        record.LoginIP = u.LastIp;
                        record.RegDate = u.Regitime;
                    }
                    else
                    {
                        record.Account = record.NickName = record.Guid = record.LoginIP = "";
                        record.RegDate = 0;
                    }
                    if (!string.IsNullOrEmpty(value) && field == 2 && !record.Account.Equals(value)) continue;
                    if (time > 0)
                    {
                        var s = (long)(DateTime.Parse(DateTime.Parse("2012-10-01").AddSeconds(time).ToString("yyyy-MM-dd") + " 00:00:00") - DateTime.Parse("2012-10-01")).TotalSeconds;
                        var e = (long)(DateTime.Parse(DateTime.Parse("2012-10-01").AddSeconds(time).ToString("yyyy-MM-dd") + " 23:59:59") - DateTime.Parse("2012-10-01")).TotalSeconds; ;
                        if (record.GameDate < s || record.GameDate > e) continue;
                    }
                    //ClubId
                    List<string> clubs = guildDal.GetCacheClubIdFromCache(record.ChargeId);
                    if (clubs != null && clubs.Count > 0)
                        record.ClubId = String.Join(",", clubs);
                    else
                        record.ClubId = "";
                    newList.Add(record);
                }
                return newList;
            }
            catch (Exception ex)
            {
                WriteError("bll GetNewGameUsers ex:", ex.Message);
            }
            return null;
        }

        public static List<GameMutual> GetMutualList(string[] players)
        {
            try
            {
                WriteLog(" players.Length :", players.Length.ToString());
                ClubsRes<object> res = null;
                if (players == null || players.Length < 1)
                    res = dal.GetAllMutualList();
                else
                {
                    res = dal.GetMutualList(players);
                }
                if (res == null || res.ret != 0 || res.msg==null) return null;
                var res2 = res.msg as IDictionary<string, JToken>;
                if (res2 == null || res2.Keys==null ||res2.Keys.Count < 1) return null;
                List<GameMutual> list = new List<GameMutual>();
                foreach (string key in res2.Keys)
                {
                    GameMutual gm = new GameMutual();
                    gm.player_id_0 = key;
                    if (res2[key] == null || res2[key].ToString() == "[]") continue;
                    gm.player_id_1 = res2[key].ToString();
                    list.Add(gm);
                }
                return list;
            }
            catch (Exception ex)
            {
                WriteError("GAMEBLL GetMutualList ex:", ex.Message);
            }
            return null;
        }


        public static ClubsServerRes AddMutual(List<string> players)
        {
            try
            {
                if (players == null || players.Count < 1) return null;
                ClubsServerRes csr = dal.AddMutual(players);
                string msg = "";
                if (csr != null && csr.ret == 0)
                    msg = string.Format("添加禁止同桌游戏ID【{0}】成功", JsonConvert.SerializeObject(players));
                else
                    msg = string.Format("添加禁止同桌游戏ID【{0}】失败", JsonConvert.SerializeObject(players));
                AdminBLL.WriteSystemLog(CurrentUser.Account, ClientIP, msg, "GameBLL.AddMutual", (csr != null && csr.ret == 0) ? 1 : 0, SystemLogEnum.ADDMUTUAL);
                return csr;
            }
            catch (Exception ex)
            {
                WriteError("Gamebll AddMutual ex:", ex.Message);
            }
            return null;
        }
        public static ClubsServerRes DelMutual(List<string> players)
        {
            try
            {
                if (players == null || players.Count < 1) return null;
                ClubsServerRes csr = dal.DelMutual(players);
                string msg = "";
                if (csr != null && csr.ret == 0)
                    msg = string.Format("删除禁止同桌游戏ID【{0}】成功", JsonConvert.SerializeObject(players));
                else
                    msg = string.Format("删除禁止同桌游戏ID【{0}】失败", JsonConvert.SerializeObject(players));
                AdminBLL.WriteSystemLog(CurrentUser.Account, ClientIP, msg, "GameBLL.DelMutual", (csr != null && csr.ret == 0) ? 1 : 0, SystemLogEnum.DELMUTUAL);
                return csr;
            }
            catch (Exception ex)
            {
                WriteError("Gamebll DelMutual ex:", ex.Message);
            }
            return null;
        }
    }
}
