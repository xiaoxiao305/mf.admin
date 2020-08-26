using System;
using System.Collections.Generic;
using System.Web;
using System.Web.SessionState;
using MF.Common.Json;
using MF.Common.Security;
using MF.Data;
using System.Text.RegularExpressions;
using System.Linq;
using System.Web.Configuration;
using System.Configuration;
using MF.Data.ExtendChannel;
using MF.Admin.DAL;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

namespace MF.Admin.BLL
{
    public class AjaxResult<T>
    {
        public int code { get; set; }
        public T result { get; set; }
        public string msg { get; set; }
    }
    public class PagerResult<T>
    {
        public int code { get; set; }
        public T result { get; set; }
        public string msg { get; set; }
        public int rowCount { get; set; }
        public int index { get; set; }
    }
    public class AjaxRequest : Base, IHttpHandler, IRequiresSessionState
    {
        public bool IsReusable { get { return false; } }
        HttpRequest Request;
        HttpResponse Response;
        HttpSessionState Session;
        Dictionary<string, string> functions;
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                Request = context.Request;
                Response = context.Response;
                Session = context.Session;
                string method = Request["m"];
                string args = Request["args"];
                Run(method, args);
            }
            catch (Exception e)
            {
                WriteError("AjaxRequest.ProcessRequest:Exception(", e.Message, ")", context.Request.Url.PathAndQuery);
            }
        }
        void initFunction()
        {
            if (functions == null || functions.Count < 1)
            {
                functions = new Dictionary<string, string>();
                functions.Add("userlist", "GetUserList");
                functions.Add("currencyrecord", "GetCurrencyRecord");
                functions.Add("beanrecord", "GetUserBeanRecord");
                functions.Add("strongboxrecord", "GetStrongBoxRecord");
                functions.Add("allstrongboxrecord", "GetAllStrongBoxRecord");
                functions.Add("chargerecord", "GetChargeRecord");
                functions.Add("subuserlist", "GetSubUserList");
                functions.Add("qmallrecord", "GetQmallRecord");
                functions.Add("guildlist", "GetGuildList");
                functions.Add("getleagueclubmemberslist", "GetLeagueClubmembersList");
                functions.Add("getmemberslist", "GetMembersList");
                functions.Add("clubgamelink", "GetClubGameLinkList");
                functions.Add("guildapplyrecord", "GetGuildApplyList");
                functions.Add("gamereport", "GetGameReport");
                functions.Add("scenereport", "GetSceneReport");
                functions.Add("report", "GetReportList");
                functions.Add("roomcardrecord", "GetRoomCardRecord");
                functions.Add("dzcurrencyrecord", "GetDZCurrencyRecord");
                functions.Add("setuserinfo", "SetUserInfo");
                functions.Add("setusermoneychk", "SetUserMoneyChk");
                functions.Add("setusermoney", "SetUserMoney");
                functions.Add("dealchargeorder", "DealChargeOrder");
                functions.Add("out", "SignOut");
                functions.Add("setguildactive", "SetGuildActive");
                functions.Add("extendchannelrecord", "GetExtendChannelRecord");
                functions.Add("setapplyguildflag", "SetApplyGuildFlag");
                functions.Add("exchangereport", "GetExchangeReport");
                functions.Add("systemlogrecord", "GetSystemlogRecord");
                functions.Add("loginlogrecord", "GetLoginlogRecord");
                functions.Add("adidregreport", "GetAdidRegReport");
                functions.Add("baiduadreport", "GetBaiduADReport");
                functions.Add("cpsuserlist", "GetCPSUserList");
                functions.Add("promotcharge", "GetPromotChargeReportList");
                functions.Add("getadminhdinfo", "GetAdminHDInfo");
                functions.Add("extendchannelkeywordrecord", "GetExtendChannelKeywordRecord");
                functions.Add("gamekeywordslist", "GetGameKeywordsList");
                functions.Add("setgamekeyword", "SetGameKeyword");
                functions.Add("gameserverlist", "GetGameServerList");
                functions.Add("flushgameserver", "FlushGameServer");
                functions.Add("flushmatchgame", "FlushMatchGame");
                functions.Add("setclubsroomcard", "SetClubsRoomCard");
                functions.Add("setchargeactivestate", "SetChargeActiveState");
                functions.Add("getchargeactivestate", "GetChargeActiveState");
                functions.Add("getrecommondclubslist", "GetRecommondClubsList");
                functions.Add("setrecomclubs", "SetRecomClubs");
                functions.Add("setpushnews", "SetPushNews");
                functions.Add("guildlinklist", "GetGuildLinkList");
                functions.Add("setclublink", "SetClubLink");
                functions.Add("verifyguildstatus", "VerifyGuildStatus");
                functions.Add("clubstatisticday", "ClubStatisticDay");
                functions.Add("delclubstatisticclubid", "DelClubStatisticClubId");
                functions.Add("getmemberactive", "GetMemberActive");
                functions.Add("getclubactive", "GetClubActive");
                functions.Add("getclubactivecount", "GetClubActiveCount");
                functions.Add("closeroom", "CloseRoom");
                functions.Add("setgamesetting", "SetGameSetting");


                functions.Add("getgameblackusers", "GetGameBlackUsers");
                functions.Add("addblackuser", "AddBlackUserRange");
                functions.Add("delblackuser", "DelBlackUser");
                functions.Add("setwinnmoney", "SetWinnMoney");
                functions.Add("setwinnmoney2", "SetWinnMoney2");
                functions.Add("getgameincome", "GetGameIncome");
                functions.Add("getgamerec", "GetGameRec");

                functions.Add("getauditblackusers", "GetAuditBlackUsers");
                functions.Add("updateblackuser", "UpdateBlackUser");
                functions.Add("confirmblackuser", "ConfirmBlackUser");
                functions.Add("getusergamemoney", "GetUserGameMoney");



                functions.Add("getclubmemberslist", "GetClubmembersList");
                functions.Add("getlastgamerecords", "GetLastGameRecords");
                functions.Add("getdeskmates", "GetDeskMates");

                //游戏输赢值异常警报
                functions.Add("setredalert", "SetRedAlert");
                functions.Add("getredalert", "GetRedAlert");
                functions.Add("delredalert", "DelRedAlert");
                functions.Add("getredalertplayer", "GetRedAlertPlayer");
                functions.Add("getgamemoney", "GetGameMoney");



                functions.Add("sendbroadcast", "SendBroadCast");
                //新增有效用户
                functions.Add("getnewgameusers", "GetNewGameUsers");
                //踢出俱乐部成员
                functions.Add("kickClubMembers", "KickClubMembers");
                //退出俱乐部联盟
                functions.Add("existleague", "ExistLeague");

                //高税俱乐部
                functions.Add("gethightaxclub", "GetHighTaxClub");
                functions.Add("addhightaxclub", "AddHighTaxClub");
                functions.Add("delhightaxclub", "DelHighTaxClub");
                functions.Add("sethightaxclub", "SetHighTaxClub");
                //俱乐部税收列表
                functions.Add("getclubtaxlist", "GetClubTaxList");
                //禁止同桌玩家列表
                functions.Add("getmutuallist", "GetMutualList");
                functions.Add("addmutual", "AddMutual");
                functions.Add("delmutual", "DelMutual");


            }
        }
        public void DelMutual(string players_id_0, string players_id_1)
        {
            try
            {
                if (string.IsNullOrEmpty(players_id_0) || string.IsNullOrEmpty(players_id_1))
                {
                    Response.Write("{\"code\":0,\"msg\":\"参数有误\"}");
                    return;
                }
                WriteLog("del players_id_1:", players_id_1);
                string[] players = Json.DeserializeObject<string[]>(players_id_1);
                ClubsServerRes cs = null;
                for (int i = 0; i < players.Length; i++)
                {
                    WriteLog("del iii:", players[i]);
                    List<string> list = new List<string>() { players_id_0, players[i].Trim() };
                    cs = GameBLL.DelMutual(list);
                }
                if (cs != null && cs.ret == 0)
                    Response.Write("{\"code\":1,\"msg\":\"操作成功\"}");
                else
                    Response.Write("{\"code\":0,\"msg\":\"操作失败\"}");
            }
            catch (Exception ex)
            {
                Response.Write("{\"code\":0,\"msg\":\"操作失败:" + ex.Message + "\"}");
            }
        }

        public void AddMutual(string players_id_0, string players_id_1, string token)
        {
            try
            {
                if (string.IsNullOrEmpty(players_id_0) || string.IsNullOrEmpty(players_id_1) || string.IsNullOrEmpty(token))
                {
                    Response.Write("{\"code\":0,\"msg\":\"参数有误\"}");
                    return;
                }
                if (!CheckToken(token))
                {
                    Response.Write("{\"code\":0,\"msg\":\"安全令有误\"}");
                    return;
                }
                string[] new_players_id_1 = players_id_1.Split(',');
                ClubsServerRes cs = null;
                for (int i = 0; i < new_players_id_1.Length; i++)
                {
                    List<string> list = new List<string>() { players_id_0, new_players_id_1[i] };
                    list.Sort();
                    cs = GameBLL.AddMutual(list);
                }
                if (cs != null && cs.ret == 0)
                    Response.Write("{\"code\":1,\"msg\":\"操作成功\"}");
                else
                    Response.Write("{\"code\":0,\"msg\":\"操作失败\"}");
            }
            catch (Exception ex)
            {
                Response.Write("{\"code\":0,\"msg\":\"操作失败:" + ex.Message + "\"}");
            }
        }
        public void GetMutualList(long pageSize, long pageIndex, string players)
        {
            var res = new PagerResult<List<GameMutual>>();
            string[] strs = new string[] { };
            if (!string.IsNullOrEmpty(players))
                strs = players.Split(',');
            var list = GameBLL.GetMutualList(strs);
            res.result = list;
            res.code = 1;
            res.msg = "";
            res.index = (int)pageIndex;
            if (list != null)
                res.rowCount = list.Count;
            string json = Json.SerializeObject(res);
            WriteLog(" ajax GetMutualList json:", json);
            Response.Write(json);
        }
        public void GetClubTaxList(long pageSize, long pageIndex, string club_id, long time)
        {
            var res = new PagerResult<List<ClubsModel>>();
            var list = GuildBLL.GetClubTaxList(club_id, time);
            res.result = list;
            res.code = 1;
            res.msg = "";
            res.index = (int)pageIndex;
            if (list != null)
                res.rowCount = list.Count;
            string json = Json.SerializeObject(res);
            Response.Write(json);
        }
        public void DelHighTaxClub(object[] clubsId)
        {
            try
            {
                if (clubsId == null || clubsId.Length < 1)
                {
                    Response.Write("{\"code\":0,\"msg\":\"参数有误\"}");
                    return;
                }
                //if (!CheckToken(token))
                //{
                //    Response.Write("{\"code\":0,\"msg\":\"安全令有误\"}");
                //    return;
                //}
                //string[] clubIdsTmp = clubIds.Split(',');
                //int[] newClubIds = Array.ConvertAll<string, int>(clubIdsTmp, delegate (string s) { return int.Parse(s); });
                ClubsServerRes cs = GuildBLL.DelHighTaxClub(clubsId);
                if (cs != null && cs.ret == 0)
                    Response.Write("{\"code\":1,\"msg\":\"操作成功\"}");
                else
                    Response.Write("{\"code\":0,\"msg\":\"操作失败\"}");
            }
            catch (Exception ex)
            {
                Response.Write("{\"code\":0,\"msg\":\"操作失败:" + ex.Message + "\"}");
            }
        }
        public void AddHighTaxClub(string clubIds, long maxTax, string token)
        {
            try
            {
                //maxTax 单位【2255游戏币】
                if (string.IsNullOrEmpty(clubIds) || maxTax < 0 || string.IsNullOrEmpty(token))
                {
                    Response.Write("{\"code\":0,\"msg\":\"参数有误\"}");
                    return;
                }
                if (!CheckToken(token))
                {
                    Response.Write("{\"code\":0,\"msg\":\"安全令有误\"}");
                    return;
                }
                string[] clubIdsTmp = clubIds.Split(',');
                int[] newClubIds = Array.ConvertAll<string, int>(clubIdsTmp, delegate (string s) { return int.Parse(s); });
                ClubsServerRes cs = GuildBLL.AddHighTaxClub(newClubIds);
                if (cs != null && cs.ret == 0)
                {
                    for (int i = 0; i < newClubIds.Length; i++)
                    {
                        GuildBLL.SetHighTaxClub(newClubIds[i], maxTax);
                    }
                    Response.Write("{\"code\":1,\"msg\":\"操作成功\"}");
                }
                else
                    Response.Write("{\"code\":0,\"msg\":\"操作失败\"}");
            }
            catch (Exception ex)
            {
                Response.Write("{\"code\":0,\"msg\":\"操作失败:" + ex.Message + "\"}");
            }
        }
        public void SetHighTaxClub(long clubId, long maxTax, string token)
        {
            try
            {
                //maxTax 单位【2255游戏币】
                if (clubId < 1 || maxTax < 0 || string.IsNullOrEmpty(token))
                {
                    Response.Write("{\"code\":0,\"msg\":\"参数有误\"}");
                    return;
                }
                if (!CheckToken(token))
                {
                    Response.Write("{\"code\":0,\"msg\":\"安全令有误\"}");
                    return;
                }
                ClubsServerRes cs = GuildBLL.SetHighTaxClub(int.Parse(clubId.ToString()), maxTax);
                if (cs != null && cs.ret == 0)
                    Response.Write("{\"code\":1,\"msg\":\"操作成功\"}");

                else
                    Response.Write("{\"code\":0,\"msg\":\"操作失败\"}");
            }
            catch (Exception ex)
            {
                Response.Write("{\"code\":0,\"msg\":\"操作失败:" + ex.Message + "\"}");
            }
        } 
        public void GetHighTaxClub(long pageSize, long pageIndex)
        {
            var res = new PagerResult<List<ClubsModel>>();
            var list = GuildBLL.GetHighTaxClub();
            res.result = list;
            res.code = 1;
            res.msg = "";
            res.index = (int)pageIndex;
            if (list != null)
                res.rowCount = list.Count;
            string json = Json.SerializeObject(res);
            Response.Write(json);
        }
        public void ExistLeague(string club_id)
        {
            ClubsServerRes cs = GuildBLL.ExistLeague(club_id);
            var oprState = 0; var msg = "操作失败";
            if (cs != null && cs.ret == 0)
            {
                oprState = 1;
                msg = "操作成功";
            }
            Response.Write("{\"code\":" + oprState + ",\"msg\":\"" + msg + "\"}");
        }
        public void KickClubMembers(string member_id, string club_id)
        {
            ClubsServerRes cs = GuildBLL.ClubMemberOpt(member_id, club_id, "kick");
            var oprState = 0; var msg = "踢出俱乐部【" + club_id + "】成员【" + member_id + "】操作失败";
            if (cs != null && cs.ret == 0)
            {
                oprState = 1;
                msg = "踢出俱乐部【" + club_id + "】成员【" + member_id + "】操作成功";
            }
            AdminBLL.WriteSystemLog(CurrentUser.Account, ClientIP, msg, "AjaxRequest.KickClubMembers", oprState, SystemLogEnum.KICKCLUBMEMBERS);
            Response.Write("{\"code\":" + oprState + ",\"msg\":\"" + msg + "\"}");
        }
        public void GetNewGameUsers(long PageSize, long pageIndex, long time, long gameId, long field, string value)
        {
            try
            {
                var res = new PagerResult<List<NewGameUsers>>();
                var list = GameBLL.GetNewGameUsers(PageSize, pageIndex, time, gameId, field, value, out int rowCount);
                res.result = list;
                res.code = 1;
                res.msg = "";
                res.index = (int)pageIndex;
                res.rowCount = rowCount;
                string json = Json.SerializeObject(res);
                Response.Write(json);
            }
            catch (Exception ex)
            {
                WriteError("ajax GetNewGameUsers ex:", ex.Message);
            }
        }
        public void SendBroadCast(long time, string content)
        {
            try
            {
                if (string.IsNullOrEmpty(content) || time < 1)
                {
                    Response.Write("{\"code\":0,\"msg\":\"参数有误\"}");
                    return;
                }
                DateTime dt = DateTime.Parse("1970/01/01").AddSeconds(time);
                WriteLog("args time:", time.ToString(), " content:", content, " convertime:", dt.ToString("yyyy-MM-dd HH:mm:ss"));
                var t = (int)(dt - TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1))).TotalSeconds;
                ClubsRes<object> r = SettingBLL.SendBroadCast(t, content);
                Response.Write("{\"code\":" + (r != null && r.ret == 0 ? "1" : "0") + ",\"msg\":\"" + (r == null ? " err " : r.msg) + "\"}");
                return;
            }
            catch (Exception ex)
            {
                WriteError("SendBroadCast ex:" + ex.Message + ",time:", time.ToString(), " content:", content);
            }
        }

        public void GetGameMoney(long pageSize, long pageIndex, string gameTypes, string chargeId)
        {
            try
            {
                var res = new PagerResult<List<Dictionary<string, object>>>();
                var list = GameBLL.GetGameMoney(gameTypes.Split(','), chargeId);
                res.result = list;
                res.code = 1;
                res.msg = "";
                res.index = (int)pageIndex;
                if (list != null)
                    res.rowCount = list.Count;
                string json = Json.SerializeObject(res);
                Response.Write(json);
            }
            catch (Exception ex)
            {
                WriteError("GetUserGameMoney ajax ex :", ex.Message);
            }
        }
        public void GetRedAlertPlayer(long pageSize, long pageIndex, string gameIds, string gameTypes, long field, string value, long time)
        {
            try
            {
                var res = new PagerResult<List<Dictionary<string, object>>>();
                var list = GameBLL.GetRedAlertPlayer(gameIds.Split(','), gameTypes.Split(','), field, value, time);
                res.result = list;
                res.code = 1;
                res.msg = "";
                res.index = (int)pageIndex;
                if (list != null)
                    res.rowCount = list.Count;
                string json = Json.SerializeObject(res);
                Response.Write(json);
            }
            catch (Exception ex)
            {
                WriteError("GetRedAlertPlayer ajax ex :", ex.Message);
            }
        }
        public void GetRedAlert(long pageSize, long pageIndex)
        {
            try
            {
                var res = new PagerResult<Dictionary<string, object>>();
                var list = GameBLL.GetRedAlert();
                res.result = list;
                res.code = 1;
                res.msg = "";
                res.index = (int)pageIndex;
                if (list != null)
                    res.rowCount = list.Count;
                string json = Json.SerializeObject(res);
                Response.Write(json);
            }
            catch (Exception ex)
            {
                WriteError("GetRedAlert ajax ex :", ex.Message);
            }
        }
        public void DelRedAlert(string gameType)
        {
            try
            {
                if (string.IsNullOrEmpty(gameType))
                {
                    Response.Write("{\"code\":0,\"msg\":\"参数有误\"}");
                    return;
                }
                AjaxResult<bool> res = new AjaxResult<bool>() { code = 0, msg = "" };
                Dictionary<string, object> r = GameBLL.DelRedAlert(gameType);
                int oprState = 0;
                string msg = string.Format("删除游戏{0}输赢值异常警报配置失败。", gameType);
                if (r != null)
                {
                    if (r.ContainsKey("ret"))
                    {
                        if (r["ret"].ToString() == "0")
                        {
                            oprState = 1;
                            msg += "成功";
                        }
                        else
                            msg += "失败。" + r["msg"];
                    }
                    else
                        msg += "失败";
                }
                AdminBLL.WriteSystemLog(CurrentUser.Account, ClientIP, msg, "AjaxRequest.DelRedAlert", oprState, SystemLogEnum.DELREDALERT);
                Response.Write("{\"code\":" + oprState + ",\"msg\":\"" + msg + "\"}");
                return;
            }
            catch (Exception ex)
            {
                WriteError("DelRedAlert ex:" + ex.Message + ",gameType:", gameType);
            }
        }
        public void SetRedAlert(string gameType, string value)
        {
            try
            {
                if (string.IsNullOrEmpty(gameType) || string.IsNullOrEmpty(value))
                {
                    Response.Write("{\"code\":0,\"msg\":\"参数有误\"}");
                    return;
                }
                AjaxResult<bool> res = new AjaxResult<bool>() { code = 0, msg = "" };
                Dictionary<string, object> r = GameBLL.SetRedAlert(gameType, value);
                int oprState = 0;
                string msg = string.Format("设置游戏{0}输赢值异常警报配置为{1}", gameType, value);
                if (r != null)
                {
                    if (r.ContainsKey("ret"))
                    {
                        if (r["ret"].ToString() == "0")
                        {
                            oprState = 1;
                            msg += "成功";
                        }
                        else
                            msg += "失败。" + r["msg"];
                    }
                    else
                        msg += "失败";
                }
                AdminBLL.WriteSystemLog(CurrentUser.Account, ClientIP, msg, "AjaxRequest.SetRedAlert", oprState, SystemLogEnum.SETREDALERT);
                Response.Write("{\"code\":" + oprState + ",\"msg\":\"" + msg + "\"}");
                return;
            }
            catch (Exception ex)
            {
                WriteError("SetRedAlert ex:" + ex.Message + ",gameType:", gameType, " value:", value);
            }
        }
        public void GetClubmembersList(long pageSize, long pageIndex, string club_id, string member_id)
        {
            //Base.WriteLog("ajax getclubmembers club_id:", club_id, " uid:", member_id);
            var res = new PagerResult<List<Dictionary<string, object>>>();
            var list = GuildBLL.GetClubmembersList(club_id.Trim(), member_id.Trim());
            res.result = list;
            res.code = 1;
            res.msg = "";
            res.index = (int)pageIndex;
            if (list != null)
                res.rowCount = list.Count;
            string json = Json.SerializeObject(res);
            //Base.WriteLog("ajax getclubmembers end:", json);
            Response.Write(json);
        }

        public void GetGameRec(long pageSize, long pageIndex, long start, long end, string type, string chargeid, string roomid, string number)
        {
            var res = new PagerResult<string>();
            var list = GameBLL.GetGameRec(start, end, type, chargeid.Trim(), roomid.Trim(), number.Trim());
            res.result = list;
            res.code = 1;
            res.msg = "";
            res.index = (int)pageIndex;
            res.rowCount = 0;
            string json = Json.SerializeObject(res);
            Response.Write(json);
        }
        public void GetGameIncome(long pageSize, long pageIndex, long start, long end, string type, string chargeid, string roomid, string number)
        {
            try
            {
                var res = new PagerResult<List<GameIncome>>();
                var list = GameBLL.GetGameIncome(start, end, type, chargeid.Trim(), roomid.Trim(), number.Trim());
                res.result = list;
                res.code = 1;
                res.msg = "";
                res.index = (int)pageIndex;
                if (list != null)
                    res.rowCount = list.Count;
                string json = Json.SerializeObject(res);
                Response.Write(json);
            }
            catch (Exception ex)
            {
                WriteError("GetGameIncome ajax ex :", ex.Message);
            }
        }
        public void GetDeskMates(long pageSize, long pageIndex)
        {
            try
            {
                var res = new PagerResult<List<AutoPatrol>>();
                var list = GameBLL.GetDeskMates();
                res.result = list;
                res.code = 1;
                res.msg = "";
                res.index = (int)pageIndex;
                if (list != null)
                    res.rowCount = list.Count;
                string json = Json.SerializeObject(res);
                Response.Write(json);
            }
            catch (Exception ex)
            {
                WriteError("GetDeskMates ajax ex :", ex.Message);
            }
        }
        public void GetLastGameRecords(long pageSize, long pageIndex, string gameIds)
        {
            try
            {
                var res = new PagerResult<List<AutoPatrol>>();
                var list = GameBLL.GetLastGameRecords2(gameIds);
                res.result = list;
                res.code = 1;
                res.msg = "";
                res.index = (int)pageIndex;
                if (list != null)
                    res.rowCount = list.Count;
                string json = Json.SerializeObject(res);
                Response.Write(json);
            }
            catch (Exception ex)
            {
                WriteError("GetLastGameRecords ajax ex :", ex.Message);
            }
        }
        //public void GetGameBlackUsers(long pageSize, long pageIndex, long gameId, long field, string value)
        //{
        //    var res = new PagerResult<List<GameBlackUserInfo>>();
        //    var list = GameBLL.GetGameBlackUsers(gameId, field, value.Trim());
        //    res.result = list;
        //    res.code = 1;
        //    res.msg = "";
        //    res.index = (int)pageIndex;
        //    if (list != null)
        //        res.rowCount = list.Count;
        //    string json = Json.SerializeObject(res);
        //    Response.Write(json);
        //}
        /// <summary>
        /// 获取黑名单用户
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="gameId">游戏ID</param>
        /// <param name="field">1chargeid  2account</param>
        /// <param name="value">查询对应值</param>
        public void GetGameBlackUsers(long pageSize, long pageIndex, long gameId, long field, string value)
        {
            var res = new PagerResult<List<GameBlackUserInfo>>();
            var list = GameBLL.GetGameBlackUsersNew(pageSize, pageIndex, gameId, field, value.Trim(), 1);
            res.result = (list == null || list.Items == null) ? null : list.Items;
            res.code = 1;
            res.msg = "";
            res.index = (int)pageIndex;
            if (list != null)
                res.rowCount = list.TotalCount;
            string json = Json.SerializeObject(res);
            Response.Write(json);
        }
        /// <summary>
        /// 获取待审核黑名单列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="gameId"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        public void GetAuditBlackUsers(long pageSize, long pageIndex, long gameId, long field, string value)
        {
            var res = new PagerResult<List<GameBlackUserInfo>>();
            var list = GameBLL.GetBlackUsers(gameId, field, value, 2);
            res.result = list;
            res.code = 1;
            res.msg = "";
            res.index = (int)pageIndex;
            if (list != null)
                res.rowCount = list.Count;
            string json = Json.SerializeObject(res);
            Response.Write(json);
        }
        /// <summary>
        /// 设置输赢值
        /// </summary> 
        public void SetWinnMoney(string gameId, string account, string type, string player_id, string value, string token)
        {
            AjaxResult<bool> res = new AjaxResult<bool>() { code = 0, msg = "" };
            if (string.IsNullOrEmpty(gameId) || string.IsNullOrEmpty(type))
            {
                Response.Write("{\"code\":0,\"msg\":\"游戏ID有误\"}");
                return;
            }
            else if (string.IsNullOrEmpty(account))
            {
                Response.Write("{\"code\":0,\"msg\":\"账号有误\"}");
                return;
            }
            //else if (string.IsNullOrEmpty(value))//黑名单删除---设置输赢值---【保留】功能----value为空
            //{
            //    Response.Write("{\"code\":0,\"msg\":\"输赢值有误\"}");
            //    return;
            //}
            if (!string.IsNullOrEmpty(token) && !Base.IsDebug)
            {
                if (!Token.CheckToken(token, Base.CurrentUser.Token))
                {
                    Response.Write("{\"code\":0,\"msg\":\"安全令有误\"}");
                    return;
                }
            }
            Dictionary<string, object> r = GameBLL.DelBlackUser(gameId, account, type, player_id, value);
            if (r != null && r.ContainsKey("ret") && r.ContainsKey("msg"))
                Response.Write("{\"code\":" + r["ret"] + ",\"msg\":\"" + r["msg"] + "\"}");
            else
                Response.Write("{\"code\":0,\"msg\":\"删除黑名单失败\"}");
            return;
        }
        public void SetWinnMoney2(string type, string player_id, string value, string token)
        {
            AjaxResult<bool> res = new AjaxResult<bool>() { code = 0, msg = "" };
            if (string.IsNullOrEmpty(type))
            {
                Response.Write("{\"code\":0,\"msg\":\"游戏ID有误\"}");
                return;
            }
            else if (string.IsNullOrEmpty(value))
            {
                Response.Write("{\"code\":0,\"msg\":\"输赢值有误\"}");
                return;
            }
            if (!string.IsNullOrEmpty(token) && !Base.IsDebug)
            {
                if (!Token.CheckToken(token, Base.CurrentUser.Token))
                {
                    Response.Write("{\"code\":0,\"msg\":\"安全令有误\"}");
                    return;
                }
            }
            Dictionary<string, object> r = GameBLL.SetWinnMoney(type, player_id, value);
            if (r != null && r.ContainsKey("ret") && r.ContainsKey("msg"))
            {
                res.code = int.Parse(r["ret"].ToString()) == 0 ? 1 : 0;
                res.msg = r["msg"].ToString();
                Response.Write(Json.SerializeObject(res));
            }
            else
                Response.Write("{\"code\":0,\"msg\":\"设置输赢值失败\"}");
            return;
        }
        /// <summary>
        /// 删除游戏黑名单
        /// </summary> 
        public void DelBlackUser(string gameId, string account, string token)
        {
            AjaxResult<bool> res = new AjaxResult<bool>() { code = 0, msg = "" };
            if (string.IsNullOrEmpty(gameId))
            {
                Response.Write("{\"code\":0,\"msg\":\"游戏ID有误\"}");
                return;
            }
            else if (string.IsNullOrEmpty(account))
            {
                Response.Write("{\"code\":0,\"msg\":\"账号有误\"}");
                return;
            }
            else if (string.IsNullOrEmpty(token))
            {
                Response.Write("{\"code\":0,\"msg\":\"安全令为空\"}");
                return;
            }
            if (!Base.IsDebug)
            {
                if (!Token.CheckToken(token, Base.CurrentUser.Token))
                {
                    Response.Write("{\"code\":0,\"msg\":\"安全令有误\"}");
                    return;
                }
            }
            Dictionary<string, string> r = GameBLL.DelBlackUser(gameId, account);
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
            Response.Write("{\"code\":" + oprState + ",\"msg\":\"" + msg + "\"}");
            return;
        }
        public void GetUserGameMoney(string chargeId, string gameType)
        {
            AjaxResult<bool> res = new AjaxResult<bool>() { code = 0, msg = "" };
            if (string.IsNullOrEmpty(chargeId) || string.IsNullOrEmpty(gameType))
            {
                Response.Write("{\"code\":0,\"msg\":\"参数错误\"}");
                return;
            }
            string money = GameBLL.GetUserGameMoney(chargeId, gameType);
            Response.Write("{\"code\":1,\"msg\":\"" + money + "\"}");
        }
        public void ConfirmBlackUser(string gameId, string account, string chargeid, string value, string levelStr, string remark, string confirmData, string token)
        {
            AjaxResult<bool> res = new AjaxResult<bool>() { code = 0, msg = "" };
            if (string.IsNullOrEmpty(gameId))
            {
                Response.Write("{\"code\":0,\"msg\":\"游戏ID有误\"}");
                return;
            }
            else if (string.IsNullOrEmpty(account))//老账号chargeid可为空
            {
                Response.Write("{\"code\":0,\"msg\":\"账号有误\"}");
                return;
            }
            else if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(levelStr))
            {
                Response.Write("{\"code\":0,\"msg\":\"设置值有误\"}");
                return;
            }
            if (!string.IsNullOrEmpty(token) && !Base.IsDebug)
            {
                if (!Token.CheckToken(token, Base.CurrentUser.Token))
                {
                    Response.Write("{\"code\":0,\"msg\":\"安全令有误\"}");
                    return;
                }
            }
            string msg = "";
            int resCode = 0;
            Dictionary<string, string> r = GameBLL.UpdateBlackUser(gameId, account, chargeid, value, levelStr, remark);
            if (r == null || !r.ContainsKey("succeed") || !r.ContainsKey("message") || !bool.Parse(r["succeed"].ToString()))
            {
                msg = string.Format("操作游戏{0}，确认实锤时修改黑名单【{1}】值为【{2}】【{3}】失败。", gameId, account, value, remark);
                Response.Write("{\"code\":0,\"msg\":\"" + msg + "\"}");
                return;
            }
            Dictionary<string, string> r2 = GameBLL.ConfirmBlackUser(account, chargeid, confirmData);
            if (r2 == null || !r2.ContainsKey("succeed") || !r2.ContainsKey("message") || !bool.Parse(r2["succeed"].ToString()))
                msg = string.Format("审核黑名单，确认实锤{0}数据为{1}失败。", chargeid, confirmData);
            else
            {
                msg = account;
                resCode = 1;
            }
            Response.Write("{\"code\":" + resCode + ",\"msg\":\"" + msg + "\"}");
        }
        public void UpdateBlackUser(string gameId, string account, string chargeid, string value, string levelStr, string remark, string token)
        {
            AjaxResult<bool> res = new AjaxResult<bool>() { code = 0, msg = "" };
            if (string.IsNullOrEmpty(gameId))
            {
                Response.Write("{\"code\":0,\"msg\":\"游戏ID有误\"}");
                return;
            }
            //else if (string.IsNullOrEmpty(account)|| string.IsNullOrEmpty(chargeid))
            else if (string.IsNullOrEmpty(account))//老账号chargeid可为空
            {
                Response.Write("{\"code\":0,\"msg\":\"账号有误\"}");
                return;
            }
            else if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(levelStr))
            {
                Response.Write("{\"code\":0,\"msg\":\"设置值有误\"}");
                return;
            }
            else if (string.IsNullOrEmpty(remark))
            {
                Response.Write("{\"code\":0,\"msg\":\"备注为空\"}");
                return;
            }
            //else if (string.IsNullOrEmpty(token))
            //{
            //    Response.Write("{\"code\":0,\"msg\":\"安全令为空\"}");
            //    return;
            //}
            if (!string.IsNullOrEmpty(token) && !Base.IsDebug)
            {
                if (!Token.CheckToken(token, Base.CurrentUser.Token))
                {
                    Response.Write("{\"code\":0,\"msg\":\"安全令有误\"}");
                    return;
                }
            }
            int resCode = 0;
            string msg = "";
            Dictionary<string, string> r = GameBLL.UpdateBlackUser(gameId, account, chargeid, value, levelStr, remark);
            if (r == null || !r.ContainsKey("succeed") || !r.ContainsKey("message") || !bool.Parse(r["succeed"].ToString()))
                msg = string.Format("操作游戏{0}，修改黑名单【{1}】值为【{2}】【{3}】失败。", gameId, account, value, remark);
            else
            {
                resCode = 1;
                Dictionary<string, string> dic = new Dictionary<string, string>() { { "Account", account }, { "Value", value }, { "Level", levelStr }, { "Remark", remark } };
                msg = JsonConvert.SerializeObject(dic);
                Response.Write("{\"code\":" + resCode + ",\"msg\":" + msg + "}");
                return;

                //Base.WriteLog("update msg:", msg);
                //msg = string.Format("{\"Account\":\"{0}\",\"Value\":\"{1}\",\"Level\":\"{2}\",\"Remark\":\"{3}\"}", account, value, levelStr, remark);
                //Base.WriteLog("update msg2222:", msg);
                msg = "{'Account':'" + account + "','Value':'" + value + "','Level':'" + levelStr + "','Remark':'" + remark + "'}";
            }
            Response.Write("{\"code\":" + resCode + ",\"msg\":\"" + msg + "\"}");
        }
        /// <summary>
        /// 添加游戏黑名单 [parseInt(game), acc, val, token], winresult);
        /// </summary> 
        public void AddBlackUser(string gameIds, string chargeId, string values, string levelStrs, string remark, long isConfirm)
        {
            string[] gameidList = gameIds.Split('|');
            string[] valueList = values.Split('|');
            string[] levelStrList = levelStrs.Split('|');
            if (gameidList.Length < 1)
            {
                Response.Write("{\"code\":0,\"msg\":\"游戏ID有误\"}");
                return;
            }
            else if (string.IsNullOrEmpty(chargeId))
            {
                Response.Write("{\"code\":0,\"msg\":\"UID有误\"}");
                return;
            }
            else if (valueList.Length < 1 || levelStrList.Length < 1)
            {
                Response.Write("{\"code\":0,\"msg\":\"设置值有误\"}");
                return;
            }
            else if (string.IsNullOrEmpty(remark))
            {
                Response.Write("{\"code\":0,\"msg\":\"备注为空\"}");
                return;
            }
            //if (!string.IsNullOrEmpty(token) && !Base.IsDebug)
            //{
            //    if (!Token.CheckToken(token, Base.CurrentUser.Token))
            //    {
            //        Response.Write("{\"code\":0,\"msg\":\"安全令有误\"}");
            //        return;
            //    }
            //}
            GameBLL.AddBlackUser(gameidList, chargeId, valueList, levelStrList, remark, isConfirm);
            Response.Write("{\"code\":1,\"msg\":\"\"}");
        }
        public void AddBlackUserRange(string gameIds, string chargeId, string values, string levelStrs, string remark, long isConfirm)
        {
            string[] gameidList = gameIds.Split('|');
            string[] valueList = values.Split('|');
            string[] levelStrList = levelStrs.Split('|');
            if (gameidList.Length < 1)
            {
                Response.Write("{\"code\":0,\"msg\":\"游戏ID有误\"}");
                return;
            }
            else if (string.IsNullOrEmpty(chargeId))
            {
                Response.Write("{\"code\":0,\"msg\":\"UID有误\"}");
                return;
            }
            else if (valueList.Length < 1 || levelStrList.Length < 1)
            {
                Response.Write("{\"code\":0,\"msg\":\"设置值有误\"}");
                return;
            }
            else if (string.IsNullOrEmpty(remark))
            {
                Response.Write("{\"code\":0,\"msg\":\"备注为空\"}");
                return;
            }
            //if (!string.IsNullOrEmpty(token) && !Base.IsDebug)
            //{
            //    if (!Token.CheckToken(token, Base.CurrentUser.Token))
            //    {
            //        Response.Write("{\"code\":0,\"msg\":\"安全令有误\"}");
            //        return;
            //    }
            //}
            GameBLL.AddBlackUserRange(gameidList, chargeId.Split(','), valueList, levelStrList, remark, isConfirm);
            Response.Write("{\"code\":1,\"msg\":\"\"}");
        }

        public void SetGameSetting(string clubId)
        {
            if (string.IsNullOrEmpty(clubId))
            {
                Response.Write("{\"code\":0,\"msg\":\"参数有误\"}");
                return;
            }
            ClubsServerRes cs = GameBLL.SetGameSetting(clubId);
            if (cs != null && cs.ret == 0)
                Response.Write("{\"code\":1,\"msg\":\"操作成功\"}");
            else
                Response.Write("{\"code\":0,\"msg\":\"操作失败\"}");
        }
        public void CloseRoom(string RoomId)
        {
            if (string.IsNullOrEmpty(RoomId))
            {
                Response.Write("{\"code\":0,\"msg\":\"参数有误\"}");
                return;
            }
            ClubsServerRes cs = GuildBLL.CloseRoom(RoomId);
            if (cs != null && cs.ret == 0)
                Response.Write("{\"code\":1,\"msg\":\"操作成功\"}");
            else
                Response.Write("{\"code\":0,\"msg\":\"操作失败\"}");
        }
        public void DelClubStatisticClubId(long clubId)
        {
            AjaxResult<bool> res = new AjaxResult<bool>() { code = 0, msg = "" };
            if (clubId < 1)
            {
                Response.Write("{\"code\":0,\"msg\":\"参数有误\"}");
                return;
            }
            GuildBLL.DelClubStatisticClubId(clubId, CurrentUser.Account);
            Response.Write("{\"code\":1,\"msg\":\"删除成功\"}");
        }
        public void ClubStatisticDay(long pageSize, long pageIndex, string tt, long clubId)
        {
            string time = DateTime.Parse(tt).ToString("yyyyMMdd");
            if ((clubId != -1 && clubId < 1) || string.IsNullOrEmpty(tt))
            {
                Response.Write("{\"code\":0,\"msg\":\"参数有误\"}");
                return;
            }
            var res = new PagerResult<List<ClubsStatistic>>();
            List<ClubsStatistic> list = GuildBLL.ClubStatisticDay(time, clubId);
            res.result = list;
            res.code = 1;
            res.msg = "";
            res.index = 1;
            res.rowCount = list == null ? 0 : list.Count;
            string json = Json.SerializeObject(res);
            Response.Write(json);
        }
        public void VerifyGuildStatus(long clubId, long status)
        {
            if (clubId < 1)
            {
                Response.Write("{\"code\":0,\"msg\":\"参数有误\"}");
                return;
            }
            //if (!CheckToken(token))
            //{
            //    Response.Write("{\"code\":0,\"msg\":\"安全令有误\"}");
            //    return;
            //}
            ClubsServerRes cs = GuildBLL.VerifyGuildStatus(clubId, status);
            if (cs != null && cs.ret == 0)
                Response.Write("{\"code\":1,\"msg\":\"操作成功\"}");
            else
                Response.Write("{\"code\":0,\"msg\":\"操作失败\"}");
        }
        public void SetClubLink(long type, long clubId, long clubLinkId)
        {
            if (type < 1 || clubId < 1 || clubLinkId < 1)
            {
                Response.Write("{\"code\":0,\"msg\":\"参数有误\"}");
                return;
            }
            ClubsServerRes cs = GuildBLL.SetClubLink(type, clubId, clubLinkId);
            if (cs != null && cs.ret == 0)
                Response.Write("{\"code\":1,\"msg\":\"操作成功\"}");
            else
                Response.Write("{\"code\":0,\"msg\":\"操作失败\"}");
        }
        /// <summary>
        /// 获取俱乐部关联列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="field">查询字段类型</param>
        /// <param name="key">查询字段值</param>
        public void GetGuildLinkList(long pageSize, long pageIndex, long clubid)
        {
            var res = new PagerResult<List<ClubLinkInfo>>();
            List<ClubLinkInfo> list = new List<ClubLinkInfo>();
            list.Add(GuildBLL.GetGuildLinkList(clubid));
            res.result = list;
            res.code = 1;
            res.msg = "";
            res.index = (int)pageIndex;
            res.rowCount = list.Count;
            string json = Json.SerializeObject(res);
            Response.Write(json);
        }
        private bool CheckToken(string token)
        {
            //2018-11-20 删除除加钱以外的操作安全令的验证 @凯子
            if (Base.IsDebug)
                return true;
            else
                return Token.CheckToken(token, Base.CurrentUser.Token);
        }
        public void SetPushNews(long type, string news, string token)
        {
            if (type < 1 || string.IsNullOrEmpty(news) || string.IsNullOrEmpty(token))
            {
                Response.Write("{\"code\":0,\"msg\":\"参数有误\"}");
                return;
            }
            if (!CheckToken(token))
            {
                Response.Write("{\"code\":0,\"msg\":\"安全令有误\"}");
                return;
            }
            SettingBLL.SetPushNews(type, news);
            Response.Write("{\"code\":1,\"msg\":\"操作成功\"}");
        }
        public void SetRecomClubs(object[] clubsId, long type)
        {
            if (type < 1 || clubsId == null || clubsId.Length < 1)
            {
                Response.Write("{\"code\":0,\"msg\":\"参数有误\"}");
                return;
            }
            //if (!CheckToken(token))
            //{
            //    Response.Write("{\"code\":0,\"msg\":\"安全令有误\"}");
            //    return;
            //}
            int res = GuildBLL.SetRecomClubs(clubsId, type);
            if (res == 1)
                Response.Write("{\"code\":1,\"msg\":\"操作成功\"}");
            else
                Response.Write("{\"code\":" + res + ",\"msg\":\"操作失败\"}");
            return;
        }
        public void GetRecommondClubsList(long pageSize, long pageIndex)
        {
            var res = new PagerResult<List<ClubsModel>>();
            var list = GuildBLL.GetRecommondclubslist();
            res.result = list;
            res.code = 1;
            res.msg = "";
            res.index = (int)pageIndex;
            res.rowCount = list.Count;
            string json = Json.SerializeObject(res);
            Response.Write(json);
        }
        /// <summary>
        /// 获取充值活动状态
        /// </summary>
        public void GetChargeActiveState()
        {
            ChargeActive ca = ChargeBLL.GetChargeActiveState();
            Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(ca));
            return;
        }
        /// <summary>
        /// 设置充值活动
        /// </summary>
        /// <param name="status">1开启 2关闭</param>
        /// <param name="stime">活动开始时间</param>
        /// <param name="etime">活动结束时间</param>
        public void SetChargeActiveState(long status, string stime, string etime, string token)
        {
            AjaxResult<bool> res = new AjaxResult<bool>() { code = 0, msg = "" };
            if (status == 1 && (string.IsNullOrEmpty(stime) || string.IsNullOrEmpty(etime)))
            {
                Response.Write("{\"code\":0,\"msg\":\"开启充值活动时间有误\"}");
                return;
            }
            else if (string.IsNullOrEmpty(token))
            {
                Response.Write("{\"code\":0,\"msg\":\"安全令为空\"}");
                return;
            }
            if (!Base.IsDebug)
            {
                if (!Token.CheckToken(token, Base.CurrentUser.Token))
                {
                    Response.Write("{\"code\":0,\"msg\":\"安全令有误\"}");
                    return;
                }
            }
            long st = BaseDAL.ConvertDateToSpan(DateTime.Parse(stime + " 0:00:00"), "s");
            long et = BaseDAL.ConvertDateToSpan(DateTime.Parse(etime + " 23:59:59"), "s") + 1;
            if (st < 0 || et < 0)
            {
                Response.Write("{\"code\":0,\"msg\":\"开启充值活动时间有误\"}");
                return;
            }
            int state = ChargeBLL.SetChargeActiveState((int)status, st, et);
            string msg = status == 1 ? "开启" : status == 2 ? "关闭" : "设置";
            if (state == 1)
                msg = string.Format("{0}充值活动成功", msg);
            else
                msg = string.Format("{0}充值活动失败", msg);
            AdminBLL.WriteSystemLog(CurrentUser.Account, ClientIP, msg, "SetChargeActiveState", state == 1 ? state : 0, (status == 1 ? SystemLogEnum.OPENCHARGEACTIVE : status == 2 ? SystemLogEnum.CLOSECHARGEACTIVE : SystemLogEnum.UNDEFINE));
            Response.Write("{\"code\":" + state + ",\"msg\":\"\"}");
            return;
        }
        public void FlushMatchGame()
        {
            OprGameServer ogs = SettingBLL.FlushMatchGame();
            if (ogs == null)
            {
                Response.Write("{\"code\":9999,\"msg\":\"\"}");
                return;
            }
            else if (ogs.ret == 0)
            {
                Response.Write("{\"code\":1,\"msg\":\"\"}");
                return;
            }
            else
            {
                Response.Write("{\"code\":" + ogs.ret + ",\"msg\":\"\"}");
                return;
            }
        }
        public void FlushGameServer(string gameNames)
        {
            if (string.IsNullOrEmpty(gameNames))
            {
                Response.Write("{\"code\":0,\"msg\":\"未选择任何服务器\"}");
                return;
            }
            OprGameServer ogs = SettingBLL.FlushGameServer(gameNames.Split(','));
            if (ogs == null)
            {
                Response.Write("{\"code\":9999,\"msg\":\"\"}");
                return;
            }
            else if (ogs.ret == 0)
            {
                Response.Write("{\"code\":1,\"msg\":\"\"}");
                return;
            }
            else
            {
                Response.Write("{\"code\":" + ogs.ret + ",\"msg\":\"\"}");
                return;
            }
        }
        public void GetGameServerList(long pageSize, long pageIndex)
        {
            var res = new PagerResult<List<GameServerList>>();
            var list = SettingBLL.GameServerList();
            res.result = list;
            res.code = 1;
            res.msg = "";
            res.index = (int)pageIndex;
            res.rowCount = list.Count;
            string json = Json.SerializeObject(res);
            Response.Write(json);
        }
        public void SetGameKeyword(long id, long state)
        {
            if (id < 1 || state < 1)
            {
                Response.Write("{\"code\":0,\"msg\":\"参数有误\"}");
                return;
            }
            int res = KeywordsBLL.SetKeywords((int)id, (int)state);
            if (res == 1)
                Response.Write("{\"code\":1,\"msg\":\"操作成功\"}");
            else
                Response.Write("{\"code\":" + res + ",\"msg\":\"操作失败\"}");
            return;
        }
        public void GetGameKeywordsList(long pageSize, long pageIndex)
        {
            var res = new PagerResult<List<ExtendKeywords>>();
            int rowCount = 0;
            var o = Report.GetGameKeywordsList(out rowCount);
            res.code = 1;
            res.index = (int)pageIndex;
            res.result = o;
            res.rowCount = rowCount;
            res.msg = "";
            string json = Json.SerializeObject(res);
            Response.Write(json);
        }
        public void GetAdminHDInfo(string hd)
        {
            //AjaxResult<bool> res = new AjaxResult<bool>() { code = 0, msg = "" };
            //if (string.IsNullOrEmpty(hd))
            //{
            //    Response.Write("{\"code\":0,\"msg\":\"本地设备信息配置有误\"}");
            //    return;
            //}
            //if (Session["administrator"] == null)
            //{
            //    Response.Write("{\"code\":-2,\"msg\":\"登录已过期\"}");
            //    return;

            //}
            //else
            //{
            //    Administrator curadmin = (Administrator)Session["administrator"];
            //    bool chkres = Token.CheckMD5(hd, curadmin.hd);
            //    if (chkres)
            //    {
            Response.Write("{\"code\":1,\"msg\":\"\"}");
            return;
            //}
            //else
            //{
            //    Response.Write("{\"code\":-1,\"msg\":\"设备信息有误\"}");
            //    return;
            //}
            // }
        }
        public void GetPromotChargeReportList(long pageSize, long pageIndex, string channel, long start, long over)
        {
            var res = new PagerResult<Object>();
            int rowCount = 0;
            var o = Report.GetPromotChargeReportList((int)pageSize, (int)pageIndex, channel, (int)start, (int)over, out rowCount);
            res.code = 1;
            res.index = (int)pageIndex;
            res.result = o;
            res.rowCount = rowCount;
            res.msg = "";
            string json = Json.SerializeObject(res);
            Response.Write(json);
        }
        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="exact">是否精确查询</param>
        /// <param name="queryFiled">查询字段名</param>
        /// <param name="keyword">查询字段值</param>
        public void GetCPSUserList(long pageSize, long pageIndex, long exact, long queryFiled, string keyword)
        {
            var res = new PagerResult<List<CPSUsersAdmin>>();
            int rowCount = 0;
            var list = CPSUsersBLL.GetCPSUserList(pageSize, pageIndex, exact, queryFiled, keyword.Trim(), out rowCount);
            res.result = list;
            res.code = 1;
            res.msg = "";
            res.index = (int)pageIndex;
            res.rowCount = rowCount;
            string json = Json.SerializeObject(res);
            Response.Write(json);
        }
        public void GetBaiduADReport(long pageSize, long pageIndex, long channel, long device, long gamelist, long checktime, long startTime, long overTime)
        {
            int rowCount = 0;
            var res = new PagerResult<List<NewBaiduAdReport>>();
            var list = Report.GetBaiduADReport(pageSize, pageIndex, channel, device, gamelist, checktime, startTime, overTime, out rowCount);
            res.result = list;
            res.code = 1;
            res.msg = "";
            res.index = (int)pageIndex;
            res.rowCount = rowCount;
            string json = Json.SerializeObject(res);
            Response.Write(json);
        }
        public void GetAdidRegReport(long pageSize, long pageIndex, long channel, long device, long gamelist, long checktime, long startTime, long overTime)
        {
            int rowCount = 0;
            var res = new PagerResult<List<NewADIDReport>>();
            var list = Report.GetAdidRegReport(pageSize, pageIndex, channel, device, gamelist, checktime, startTime, overTime, out rowCount);
            res.result = list;
            res.code = 1;
            res.msg = "";
            res.index = (int)pageIndex;
            res.rowCount = rowCount;
            string json = Json.SerializeObject(res);
            Response.Write(json);
        }
        /// <summary>
        /// 登录日志记录
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="checktime">是否按时间查询</param>
        /// <param name="startTime">查询开始时间</param>
        /// <param name="overTime">查询结束时间</param>
        public void GetLoginlogRecord(long pageSize, long pageIndex, long flag,
            long exact, long filed, string key, long checktime, long startTime, long overTime)
        {
            int rowCount = 0;
            var res = new PagerResult<List<LoginLog>>();
            var list = RecordBLL.GetLoginLogRecord(pageSize, pageIndex, flag, exact, filed, key, checktime, startTime, overTime, out rowCount);
            res.result = list;
            res.code = 1;
            res.msg = "";
            res.index = (int)pageIndex;
            res.rowCount = rowCount;
            string json = Json.SerializeObject(res);
            Response.Write(json);
        }
        /// <summary>
        /// 操作日志记录
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="account">账号</param>
        /// <param name="checktime">是否按时间查询</param>
        /// <param name="startTime">查询开始时间</param>
        /// <param name="overTime">查询结束时间</param>
        public void GetSystemlogRecord(long pageSize, long pageIndex, long type, long flag,
            long exact, long filed, string key, long checktime, long startTime, long overTime)
        {
            int rowCount = 0;
            var res = new PagerResult<List<SystemLog>>();
            var list = RecordBLL.GetSystemlogRecord(pageSize, pageIndex, type, flag, exact, filed, key, checktime, startTime, overTime, out rowCount);
            res.result = list;
            res.code = 1;
            res.msg = "";
            res.index = (int)pageIndex;
            res.rowCount = rowCount;
            string json = Json.SerializeObject(res);
            Response.Write(json);
        }
        public void SetApplyGuildFlag(long id, string memo)
        {
            AjaxResult<bool> res = new AjaxResult<bool>() { code = 0, msg = "" };
            if (id < 1)
            {
                Response.Write("{\"code\":0,\"msg\":\"保证金记录id有误\"}");
                return;
            }
            int state = GuildBLL.SetApplyGuildFlag(id, CurrentUser.Account, memo);
            Response.Write("{\"code\":" + state + ",\"msg\":\"\"}");
            return;
        }
        public void SetGuildActive(long id, long currentnum, long lastnum, long exp, string token)
        {
            AjaxResult<bool> res = new AjaxResult<bool>() { code = 0, msg = "" };
            if (id < 1)
            {
                Response.Write("{\"code\":0,\"msg\":\"俱乐部id有误\"}");
                return;
            }
            else if (currentnum < 0 || currentnum > 100000000
                || lastnum < 0 || lastnum > 100000000
                || exp < 0 || exp > 100000000)
            {
                Response.Write("{\"code\":0,\"msg\":\"活跃人数数量有误\"}");
                return;
            }
            else if (string.IsNullOrEmpty(token))
            {
                Response.Write("{\"code\":0,\"msg\":\"安全令为空\"}");
                return;
            }
            if (!Base.IsDebug)
            {
                if (!Token.CheckToken(token, Base.CurrentUser.Token))
                {
                    Response.Write("{\"code\":0,\"msg\":\"安全令有误\"}");
                    return;
                }
            }
            int state = GuildBLL.SetGuildActive(id, currentnum, lastnum, exp);
            Response.Write("{\"code\":" + state + ",\"msg\":\"\"}");
            return;
        }
        public void SignOut()
        {
            AdminBLL.SignOut();
            Response.Write("{\"code\":1}");
        }
        public void GetExchangeReport(long pageSize, long pageIndex, long qmalltype, long checktime, long startTime, long overTime, string channel)
        {
            int rowCount = 0;
            var res = new PagerResult<List<NewQmallReportCount>>();
            var list = Report.GetExchangeReport(pageSize, pageIndex, qmalltype, checktime, startTime, overTime, channel, out rowCount);
            res.result = list;
            res.code = 1;
            res.msg = "";
            res.index = (int)pageIndex;
            res.rowCount = rowCount;
            string json = Json.SerializeObject(res);
            Response.Write(json);
        }
        public void GetExtendChannelRecord(long pageSize, long pageIndex, string channel, string channelnum, long checktime, long startTime, long overTime)
        {
            int rowCount = 0;
            var redisRes = new PagerResult<List<ExtendChannel>>();
            var list = Report.GetExtendChannelRecord(pageSize, pageIndex, channel, channelnum, checktime, startTime, overTime, out rowCount);
            redisRes.result = list;
            redisRes.code = 1;
            redisRes.msg = "";
            redisRes.index = (int)pageIndex;
            redisRes.rowCount = rowCount;
            string json = Json.SerializeObject(redisRes);
            Response.Write(json);
        }
        public void GetExtendChannelKeywordRecord(long pageSize, long pageIndex, long checktime, long startTime)
        {
            int rowCount = 0;
            var redisRes = new PagerResult<List<ExtendChannel>>();
            var list = Report.GetExtendChannelKeywordRecord(pageSize, pageIndex, checktime, startTime, out rowCount);
            redisRes.result = list;
            redisRes.code = 1;
            redisRes.msg = "";
            redisRes.index = (int)pageIndex;
            redisRes.rowCount = rowCount;
            string json = Json.SerializeObject(redisRes);
            Response.Write(json);
        }

        /// <summary>
        /// 报表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="method"></param>
        /// <param name="start"></param>
        /// <param name="over"></param>
        public void GetReportList(long pageSize, long pageIndex, string method, long start, long over, string channel)
        {
            var res = new PagerResult<Object>();
            int rowCount = 0;
            var o = Report.GetReportList(method, (int)pageSize, (int)pageIndex, (int)start, (int)over, channel, out rowCount);
            res.code = 1;
            res.index = (int)pageIndex;
            res.result = o;
            res.rowCount = rowCount;
            res.msg = "";
            string json = Json.SerializeObject(res);
            Response.Write(json);
        }
        /// <summary>
        /// 获取游戏场数据报表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="gameid">游戏id</param>
        /// <param name="matchid">场id</param>
        /// <param name="start">查询开始时间</param>
        /// <param name="over">查询结束时间</param>
        public void GetSceneReport(long pageSize, long pageIndex, long gameid, long matchid, long start, long over)
        {
            var res = new PagerResult<Object>();
            int rowCount = 0;
            var o = Report.GetSceneReport(pageSize, pageIndex, gameid, matchid, 0, start, over, out rowCount);
            res.code = 1;
            res.index = (int)pageIndex;
            res.result = o;
            res.rowCount = rowCount;
            res.msg = "";
            string json = Json.SerializeObject(res);
            Response.Write(json);
        }
        /// <summary>
        /// 获取游戏报表数据
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="gameid">游戏id</param>
        /// <param name="start">查询开始时间</param>
        /// <param name="over">查询结束时间</param>
        public void GetGameReport(long pageSize, long pageIndex, long gameid, long start, long over)
        {
            var res = new PagerResult<Object>();
            int rowCount = 0;
            var o = Report.GetGameReport(pageSize, pageIndex, gameid, start, over, out rowCount);
            res.code = 1;
            res.index = (int)pageIndex;
            res.result = o;
            res.rowCount = rowCount;
            res.msg = "";
            string json = Json.SerializeObject(res);
            Response.Write(json);
        }
        /// <summary>
        /// 俱乐部保证金记录
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="flag">订单状态</param>
        /// <param name="exact">是否精确查询</param>
        /// <param name="field">查询字段名</param>
        /// <param name="key">查询字段值</param>
        /// <param name="checktime">是否按时间查询</param>
        /// <param name="timeType">查询时间类型</param>
        /// <param name="startTime">查询开始时间</param>
        /// <param name="overTime">查询结束时间</param>
        public void GetGuildApplyList(long pageSize, long pageIndex, long flag, long exact, long field, string key, long checktime, long timeType, long startTime, long overTime)
        {
            int rowCount = 0;
            var res = new PagerResult<List<GuildApplyRecord>>();
            var list = GuildBLL.GetGuildApplyList(pageSize, pageIndex, flag, exact, field, key, checktime, timeType, startTime, overTime, out rowCount);
            res.result = list;
            res.code = 1;
            res.msg = "";
            res.index = (int)pageIndex;
            res.rowCount = rowCount;
            string json = Json.SerializeObject(res);
            Response.Write(json);
        }
        /// <summary>
        /// 获取俱乐部列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="field">查询字段类型</param>
        /// <param name="key">查询字段值</param>
        public void GetGuildList(long pageSize, long pageIndex, long filed, string key)
        {
            var res = new PagerResult<List<ClubsModel>>();
            var list = GuildBLL.GetGuildList(filed, key);
            res.result = list;
            res.code = 1;
            res.msg = "";
            res.index = (int)pageIndex;
            if (list != null)
                res.rowCount = list.Count;
            string json = Json.SerializeObject(res);
            Response.Write(json);
        }
        public void GetMemberActive(long pageSize, long pageIndex, string club_id, string day)
        {
            var res = new PagerResult<List<MembersActivity>>();
            var list = GuildBLL.GetMemberActive(club_id, day);
            res.result = list;
            res.code = 1;
            res.msg = "";
            res.index = (int)pageIndex;
            if (list != null)
                res.rowCount = list.Count;
            string json = Json.SerializeObject(res);
            Response.Write(json);
        }
        public void GetClubActive(long pageSize, long pageIndex, long order)
        {
            var res = new PagerResult<List<MembersActivity>>();
            var list = GuildBLL.GetClubActive(pageSize, pageIndex, order);
            res.result = list;
            res.code = 1;
            res.msg = "";
            res.index = (int)pageIndex;
            if (list != null)
                res.rowCount = list.Count;
            string json = Json.SerializeObject(res);
            Response.Write(json);
        }
        public void GetClubActiveCount(long pageSize, long pageIndex)
        {
            var res = new PagerResult<Dictionary<string, object>>();
            var list = GuildBLL.GetClubActiveCount();
            res.result = list;
            res.code = 1;
            res.msg = "";
            res.index = (int)pageIndex;
            if (list != null)
                res.rowCount = list.Count;
            string json = Json.SerializeObject(res);
            Response.Write(json);
        }

        public void GetLeagueClubmembersList(long pageSize, long pageIndex, string club_id, string member_id)
        {
            var res = new PagerResult<List<ClubMembers>>();
            var list = GuildBLL.GetLeagueClubmembersList(club_id.Trim(), member_id.Trim());
            res.result = list;
            res.code = 1;
            res.msg = "";
            res.index = (int)pageIndex;
            if (list != null)
                res.rowCount = list.Count;
            string json = Json.SerializeObject(res);
            Response.Write(json);
        }

        public void GetMembersList(long pageSize, long pageIndex, long filed, string key)
        {
            var res = new PagerResult<List<ClubsModel>>();
            var list = GuildBLL.GetMembersList(filed, key.Trim());
            res.result = list;
            res.code = 1;
            res.msg = "";
            res.index = (int)pageIndex;
            if (list != null)
                res.rowCount = list.Count;
            string json = Json.SerializeObject(res);
            Response.Write(json);
        }

        //获取俱乐部联系方式
        public void GetClubGameLinkList(long pageSize, long pageIndex, string game, string r)
        {
            var res = new PagerResult<List<ClubsLink>>();
            var list = GuildBLL.GetClubGameLinkList(game, r);
            res.result = list;
            res.code = 1;
            res.msg = "";
            res.index = (int)pageIndex;
            if (list != null)
                res.rowCount = list.Count;
            string json = Json.SerializeObject(res);
            Response.Write(json);
        }
        /// <summary>
        /// 金豆兑换元宝记录
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="account"></param>
        /// <param name="UIDFirst">用户存在哪个user库</param>
        public void GetQmallRecord(long pageSize, long pageIndex, string account, long type, long flag)
        {
            int rowCount = 0;
            var res = new PagerResult<List<QmallRecord>>();
            var list = RecordBLL.GetQmallRecord(pageSize, pageIndex, account, type, flag, out rowCount);
            res.result = list;
            res.code = 1;
            res.msg = "";
            res.index = (int)pageIndex;
            res.rowCount = rowCount;
            string json = Json.SerializeObject(res);
            Response.Write(json);
        }
        /// <summary>
        /// 补单
        /// </summary>
        /// <param name="order">订单号</param>
        /// <param name="token">手机安全令</param>
        public void DealChargeOrder(string account, string order, long money, long payChannel, string token)
        {
            AjaxResult<bool> res = new AjaxResult<bool>() { code = 0, msg = "" };
            if (string.IsNullOrEmpty(account))
            {
                Response.Write("{\"code\":0,\"msg\":\"账号有误\"}");
                return;
            }
            else if (string.IsNullOrEmpty(order))
            {
                Response.Write("{\"code\":0,\"msg\":\"订单号有误\"}");
                return;
            }
            else if (money < 1)
            {
                Response.Write("{\"code\":0,\"msg\":\"金额有误\"}");
                return;
            }
            else if (payChannel < 1)
            {
                Response.Write("{\"code\":0,\"msg\":\"支付平台有误\"}");
                return;
            }
            else if (string.IsNullOrEmpty(token))
            {
                Response.Write("{\"code\":0,\"msg\":\"安全令为空\"}");
                return;
            }
            if (!Base.IsDebug)
            {
                if (!Token.CheckToken(token, Base.CurrentUser.Token))
                {
                    Response.Write("{\"code\":0,\"msg\":\"安全令有误\"}");
                    return;
                }
            }
            int state = ChargeBLL.DealChargeOrder(order, money, payChannel);
            string msg = "";
            if (state == 1)
                msg = string.Format("操作账号{0}【补单】【{1}】成功", account, order);
            else
                msg = string.Format("操作账号{0}【补单】【{1}】失败", account, order);
            AdminBLL.WriteSystemLog(CurrentUser.Account, ClientIP, msg, "AjaxRequest.DealChargeOrder", state == 1 ? state : 0, SystemLogEnum.DEALCHARGERECORD);
            Response.Write("{\"code\":" + state + ",\"msg\":\"\"}");
            return;
        }

        /// <summary>
        /// 设置用户货币
        /// </summary>
        /// <param name="account"></param>
        /// <param name="type">1元宝 2金豆 3用户房卡 4俱乐部房卡</param>
        /// <param name="num"></param>
        public void SetUserMoney(string account, long type, long num, string token)
        {
            AjaxResult<bool> res = new AjaxResult<bool>() { code = 0, msg = "" };
            if (string.IsNullOrEmpty(account))
            {
                Response.Write("{\"code\":0,\"msg\":\"账号有误\"}");
                return;
            }
            else if (type != 1 && type != 2 && type != 3)
            {
                Response.Write("{\"code\":0,\"msg\":\"操作有误\"}");
                return;
            }
            else if (num < 1 || num > 100000000)
            {
                Response.Write("{\"code\":0,\"msg\":\"数量有误\"}");
                return;
            }
            else if (string.IsNullOrEmpty(token))
            {
                Response.Write("{\"code\":0,\"msg\":\"安全令为空\"}");
                return;
            }
            if (!IsDebug)
            {
                if (!Token.CheckToken(token, CurrentUser.Token))
                {
                    Response.Write("{\"code\":0,\"msg\":\"安全令有误\"}");
                    return;
                }
            }
            int state = UserBLL.SetUserMoney(account, type, num);
            Response.Write("{\"code\":" + state + ",\"msg\":\"\"}");
            return;
        }
        /// <summary>
        /// 增加俱乐部房卡
        /// </summary>
        /// <param name="clubId">俱乐部id</param>
        /// <param name="num">房卡数量</param>
        public void SetClubsRoomCard(long clubId, long num, string token)
        {
            AjaxResult<bool> res = new AjaxResult<bool>() { code = 0, msg = "" };
            if (clubId < 1)
            {
                Response.Write("{\"code\":0,\"msg\":\"俱乐部id有误\"}");
                return;
            }
            else if (num < 1 || num > 100000000)
            {
                Response.Write("{\"code\":0,\"msg\":\"房卡数量有误\"}");
                return;
            }
            else if (string.IsNullOrEmpty(token))
            {
                Response.Write("{\"code\":0,\"msg\":\"安全令为空\"}");
                return;
            }
            if (!IsDebug)
            {
                if (!Token.CheckToken(token, CurrentUser.Token))
                {
                    Response.Write("{\"code\":0,\"msg\":\"安全令有误\"}");
                    return;
                }
            }
            ClubsServer cs = GuildBLL.AddClubRoomcard(clubId, num);
            if (cs == null)
            {
                Response.Write("{\"code\":-1,\"msg\":\"\"}");
                return;
            }
            else
            {
                if (cs.ret != 0)
                {
                    if (cs.ret == 1)
                        cs.ret = -201;
                    Response.Write("{\"code\":" + cs.ret + ",\"msg\":\"" + cs.msg + "\"}");
                    return;
                }
                else
                    Response.Write("{\"code\":1,\"msg\":\"\"}");
            }
            return;
        }
        /// <summary>
        /// 设置用户货币前进行验证
        /// </summary>
        /// <param name="account"></param>
        /// <param name="type">1元宝 2金豆</param>
        /// <param name="num"></param>
        /// <param name="token"></param>
        public void SetUserMoneyChk(string account, long num, string token)
        {
            AjaxResult<bool> res = new AjaxResult<bool>() { code = 0, msg = "" };
            if (string.IsNullOrEmpty(account))
            {
                Response.Write("{\"code\":0,\"msg\":\"账号有误\"}");
                return;
            }
            else if (num < 1 || num > 100000000)
            {
                Response.Write("{\"code\":0,\"msg\":\"数量有误\"}");
                return;
            }
            else if (string.IsNullOrEmpty(token))
            {
                Response.Write("{\"code\":0,\"msg\":\"安全令为空\"}");
                return;
            }
            if (!IsDebug)
            {
                if (!Token.CheckToken(token, Base.CurrentUser.Token))
                {
                    Response.Write("{\"code\":0,\"msg\":\"安全令有误\"}");
                    return;
                }
            }
            Response.Write("{\"code\":1,\"msg\":\"\"}");
            return;
        }
        /// <summary>
        /// 设置用户信息
        /// </summary>
        /// <param name="account">用户账号</param>
        /// <param name="type">操作用户类型
        /// 1修改密码 2解绑手机号 3解绑安全令 4解除本机锁定 5解除安全令锁定 6冻结账号 7解冻账号
        /// </param>
        /// <param name="token">安全令</param>
        /// <param name="pwd">修改密码时的参数</param>
        public void SetUserInfo(string account, long type, string token, string pwd)
        {
            if (string.IsNullOrEmpty(account))
            {
                Response.Write("{\"code\":0,\"msg\":\"账号有误\"}");
                return;
            }
            else if (type < 1 || type > 7)
            {
                Response.Write("{\"code\":0,\"msg\":\"操作类型有误\"}");
                return;
            }
            else if (string.IsNullOrEmpty(token))
            {
                Response.Write("{\"code\":0,\"msg\":\"安全令为空\"}");
                return;
            }
            if (!Base.IsDebug)
            {
                if (!Token.CheckToken(token, Base.CurrentUser.Token))
                {
                    Response.Write("{\"code\":0,\"msg\":\"安全令有误\"}");
                    return;
                }
            }
            int state = 0;
            if (type == 1)
            {
                if (string.IsNullOrEmpty(pwd))
                {
                    Response.Write("{\"code\":0,\"msg\":\"密码为空\"}");
                    return;
                }
                state = UserBLL.UpdatePwd(account, pwd);
                Response.Write("{\"code\":" + state + ",\"msg\":\"\"}");
                return;
            }
            else
            {
                state = UserBLL.SetUserInfo(account, type);
                Response.Write("{\"code\":" + state + ",\"msg\":\"\"}");
                return;
            }
        }
        /// <summary>
        /// 用户房卡变更记录
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="type">元宝变更类型</param>
        /// <param name="account">账号</param>
        /// <param name="checktime">是否按时间查询</param>
        /// <param name="startTime">查询开始时间</param>
        /// <param name="overTime">查询结束时间</param>
        public void GetDZCurrencyRecord(long pageSize, long pageIndex, string account, long startTime, long overTime)
        {
            int rowCount = 0;
            var res = new PagerResult<List<CurrencyRecord>>();
            var list = RecordBLL.GetDZCurrencyRecord(pageSize, pageIndex, account, startTime, overTime, out rowCount);
            res.result = list;
            res.code = 1;
            res.msg = "";
            res.index = (int)pageIndex;
            res.rowCount = rowCount;
            string json = Json.SerializeObject(res);
            Response.Write(json);
        }
        /// <summary>
        /// 用户房卡变更记录
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="type">元宝变更类型</param>
        /// <param name="account">账号</param>
        /// <param name="checktime">是否按时间查询</param>
        /// <param name="startTime">查询开始时间</param>
        /// <param name="overTime">查询结束时间</param>
        public void GetRoomCardRecord(long pageSize, long pageIndex, string account, long startTime, long overTime)
        {
            int rowCount = 0;
            var res = new PagerResult<List<CurrencyRecord>>();
            var list = RecordBLL.GetRoomCardRecord(pageSize, pageIndex, account, startTime, overTime, out rowCount);
            res.result = list;
            res.code = 1;
            res.msg = "";
            res.index = (int)pageIndex;
            res.rowCount = rowCount;
            string json = Json.SerializeObject(res);
            Response.Write(json);
        }
        /// <summary>
        /// 用户二级密码变更记录
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="type">元宝变更类型</param>
        /// <param name="account">账号</param>
        /// <param name="checktime">是否按时间查询</param>
        /// <param name="startTime">查询开始时间</param>
        /// <param name="overTime">查询结束时间</param>
        public void GetStrongBoxRecord(long pageSize, long pageIndex, long type, string account, long checktime, long startTime, long overTime)
        {
            int rowCount = 0;
            var res = new PagerResult<List<CurrencyRecord>>();
            var list = RecordBLL.GetStrongBoxRecord(pageSize, pageIndex, type, account, checktime, startTime, overTime, out rowCount);
            res.result = list;
            res.code = 1;
            res.msg = "";
            res.index = (int)pageIndex;
            res.rowCount = rowCount;
            string json = Json.SerializeObject(res);
            Response.Write(json);
        }
        //[parseInt($("#type").val()), checktime, startTime, overTime,chargeid,account];
        public void GetAllStrongBoxRecord(long pageSize, long pageIndex, long type, long checktime, long startTime, long overTime, string chargeid, string account)
        {
            int rowCount = 0;
            var res = new PagerResult<List<StrongBoxRecord>>();
            var list = RecordBLL.GetAllStrongBoxRecord(pageSize, pageIndex, type, checktime, startTime, overTime, chargeid, account, out rowCount);
            res.result = list;
            res.code = 1;
            res.msg = "";
            res.index = (int)pageIndex;
            res.rowCount = rowCount;
            string json = Json.SerializeObject(res);
            Response.Write(json);
        }
        /// <summary>
        /// 获取用户子账号列表
        /// </summary>
        public void GetSubUserList(long pageSize, long pageIndex, string account)
        {
            var res = new PagerResult<List<Users>>();
            int rowCount = 0;
            var list = UserBLL.GetSubUserList(pageSize, pageIndex, account, out rowCount);
            res.result = list;
            res.code = 1;
            res.msg = "";
            res.index = (int)pageIndex;
            res.rowCount = rowCount;
            string json = Json.SerializeObject(res);
            Response.Write(json);
        }
        /// <summary>
        /// 用户金豆变更记录
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="gameid">游戏id</param>
        /// <param name="matchid">游戏场id</param>
        /// <param name="type">变更类型</param>
        /// <param name="account">账号</param>
        /// <param name="checktime">是否通过时间查询</param>
        /// <param name="startTime">查询开始时间</param>
        /// <param name="overTime">查询结束时间</param>
        public void GetUserBeanRecord(long pageSize, long pageIndex, long gameid, long matchid, long type, string account, long checktime, long startTime, long overTime)
        {
            int rowCount = 0;
            var res = new PagerResult<List<BeanRecord>>();
            var list = RecordBLL.GetUserBeanRecord(pageSize, pageIndex, gameid, matchid, type, account, checktime, startTime, overTime, out rowCount);
            res.result = list;
            res.code = 1;
            res.msg = "";
            res.index = (int)pageIndex;
            res.rowCount = rowCount;
            string json = Json.SerializeObject(res);
            Response.Write(json);
        }
        /// <summary>
        /// 用户充值记录
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="account">账号</param>
        /// <param name="flag">充值状态</param>
        /// <param name="channel">充值渠道</param>
        /// <param name="exact">是否精确查询</param>
        /// <param name="filed">查询字段名</param>
        /// <param name="key">查询字段值</param>
        /// <param name="checktime">是否按时间查询</param>
        /// <param name="timeType">查询时间类型</param>
        /// <param name="start">查询开始时间</param>
        /// <param name="over">查询结束时间</param>
        public void GetChargeRecord(long pageSize, long pageIndex, string account, long flag, long channel, long exact, long filed, string key, long checktime, long timeType, long start, long over)
        {
            int rowCount = 0;
            var res = new PagerResult<List<ChargeRecord>>();
            var list = ChargeBLL.GetChargeRecordList(pageSize, pageIndex, account, flag, channel, exact, filed, key, checktime, timeType, start, over, out rowCount);
            res.result = list;
            res.code = 1;
            res.msg = "";
            res.index = (int)pageIndex;
            res.rowCount = rowCount;
            string json = Json.SerializeObject(res);
            Response.Write(json);
        }
        /// <summary>
        /// 用户元宝变更记录
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="gameid">游戏id</param>
        /// <param name="matchid">游戏场id</param>
        /// <param name="type">元宝变更类型</param>
        /// <param name="account">账号</param>
        /// <param name="checktime">是否按时间查询</param>
        /// <param name="startTime">查询开始时间</param>
        /// <param name="overTime">查询结束时间</param>
        public void GetCurrencyRecord(long pageSize, long pageIndex, long gameid, long matchid, long type, string account, string chargeid, long checktime, long startTime, long overTime, long searchtype)
        {
            int rowCount = 0;
            var res = new PagerResult<List<CurrencyRecord>>();
            var list = RecordBLL.GetCurrcryRecord(pageSize, pageIndex, gameid, matchid, type, account, chargeid, checktime, startTime, overTime, searchtype, out rowCount);
            res.result = list;
            res.code = 1;
            res.msg = "";
            res.index = (int)pageIndex;
            res.rowCount = rowCount;
            string json = Json.SerializeObject(res);
            Response.Write(json);
        }
        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="dbsource"></param>
        /// <param name="dbname"></param>
        /// <param name="exact">是否精确查询</param>
        /// <param name="queryFiled">查询字段名</param>
        /// <param name="keyword">查询字段值</param>
        public void GetUserList(long pageSize, long pageIndex, long dbsource, long dbname, long exact, long queryFiled, string keyword)
        {
            var res = new PagerResult<List<Users>>();
            int rowCount = 0;
            var list = UserBLL.GetUserList(pageSize, pageIndex, dbsource, dbname, exact, queryFiled, keyword.Trim(), out rowCount);
            res.result = list;
            res.code = 1;
            res.msg = "";
            res.index = (int)pageIndex;
            res.rowCount = rowCount;
            string json = Json.SerializeObject(res);
            Response.Write(json);
        }
        void Run(string funcName, string _args)
        {
            if (functions == null || functions.Count < 1) initFunction();
            if (!AdminBLL.CheckIsLogin())
            {
                var res = new AjaxResult<string>() { code = 0, result = "", msg = "请登录后操作" };
                string json = Json.SerializeObject(res);
                Response.Write(json);
                return;
            }
            if (this.functions.ContainsKey(funcName))
            {
                try
                {
                    var method = this.GetType().GetMethod(functions[funcName]);
                    var args = Json.DeserializeObject<object[]>(_args);
                    if (method.IsStatic)
                        method.Invoke(null, args);
                    else
                        method.Invoke(this, args);
                }
                catch (Exception e)
                {
                    Base.WriteError(funcName, _args, "Error:", e.Message);
                    var res = new AjaxResult<string>() { code = 0, result = "", msg = "服务器错误:" + e.Message };
                    string json = Json.SerializeObject(res);
                    Response.Write(json);
                }
            }
            else
            {
                var res = new AjaxResult<string>() { code = 0, result = "", msg = "错误的方法" };
                string json = Json.SerializeObject(res);
                Response.Write(json);
                Base.WriteError(funcName, "不存在,args:", _args);
            }
        }
    }
}
