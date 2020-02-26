using System;
using System.Collections.Generic;

using MF.Data;
using System.Data;
using System.Data.SqlClient;
using MF.Protocol;
using MF.Common.Json;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.Collections;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;

namespace MF.Admin.DAL
{
    public class GuildDAL : BaseDAL
    {
        public List<ClubsModel> GetAllClubsList()
        {
            try
            {
                string param = "{\"module\":\"club\",\"func\":\"all\",\"args\":{}}";
                ClubsRes<List<ClubsModel>> res = PostClubServer<ClubsRes<List<ClubsModel>>>(ClubsURI, param);
                if (res == null) return null;
                return res.msg;
            }
            catch (Exception ex)
            {
                WriteError("post get_all_club_list ex:", ex.Message);
            }
            return null;
        }
        public ClubsModelServer GetClubModelList(string key, string value)
        {
            try
            {
                if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
                    return null;
                string param = "{\"module\":\"club\",\"func\":\"get_club\",\"args\":{\"" + key + "\":\"" + value.Trim() + "\"}}";
                var res = PostClubServer<ClubsModelServer>(ClubsURI, param);
                if (res != null && res.ret == 0 && res.msg != null)
                    SetCacheClubName(res.msg.Id.ToString(),res.msg.Name); 
                return res;
            }
            catch (Exception ex)
            {
                WriteError("post get_club_model_list ex:", ex.Message);
            }
            return null;
        }
        public ClubsListServer GetClubList(string key, string value)
        {
            try
            {
                if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
                    return null;
                string param = "{\"module\":\"club\",\"func\":\"get_club\",\"args\":{\"" + key + "\":\"" + value.Trim() + "\"}}";
                var res= PostClubServer<ClubsListServer>(ClubsURI, param); 
                return res;
            }
            catch (Exception ex)
            {
                WriteError("post get_club_list ex:", ex.Message);
            }
            return null;
        }

        public List<Dictionary<string, object>> GetClubmembersList(string club_id, string member_id)
        {
            if (string.IsNullOrEmpty(club_id)) return null;
            string param = "{\"module\":\"club_member\",\"func\":\"get_member_all_data\",\"args\":{\"club_id\":\"" + club_id + "\"}}";
            Base.WriteLog("dal getclubmembers param:", param);
            var res = PostClubServer<ClubsRes<Dictionary<string, object>[]>>(ClubsURI, param);
            if (res == null || res.ret != 0) return null;
            var data = res.msg;
            var members = new List<Dictionary<string, object>>();
            foreach (var m1 in data)
            {
                Base.WriteLog("dal getclubmembers m1 :", m1);
                if (!string.IsNullOrEmpty(member_id) && m1["member_id"].ToString().ToUpper() != member_id.ToUpper()) continue;
                var clubMember = GetClubMemberModel(m1);
                if (clubMember == null || clubMember.Count < 1) continue;
                clubMember.Add("clubname", GetCacheClubName(club_id));
                members.Add(clubMember);
                Base.WriteLog("dal getclubmembers for add end.");
            }
            Base.WriteLog("dal getclubmembers end.");
            return members;
        }
        public static Dictionary<string, object> GetClubMemberModel(Dictionary<string, object> m)
        {
            Base.WriteLog("dal GetClubMemberModel :");
            var info = new Dictionary<string, object>();
            var user = m["user"] as IDictionary<string, Newtonsoft.Json.Linq.JToken>;
            if (user == null || user.Count < 1)
            {
                WriteError("获取友谊圈成员出错,成员信息不存在2.", m["user"], m["user"].GetType().ToString());
                return null;
            }
            Base.WriteLog("dal GetClubMemberModel user:", user, " m:", m, " nick:", user["Nickname"]);
            info.Add("user", m["user"]);
            info.Add("id", m["member_id"]);//成员ID
            //info.Add("sponsor", m["sponsor"]);//推广人ID
            //info.Add("icon", ConvertIcon(user["Icon"].ToString()));//头像
            info.Add("nickname", user["Nickname"].ToString());//昵称
            //info.Add("num", m["activity"]);//活跃
            //info.Add("type", m["type"]);//成员类型 0等待加入 1普通成员   2管理员   255会长
            info.Add("join_date", m["join_date"]);
            //info.Add("joinscore", 0);//可提现推广值
            //info.Add("dividends_this_week", m["dividends_this_week"].ToString());//本周推广值【无法提现】(已抽成)
            //info.Add("flag", m["tag"].ToString().ToLower() == "undefind" ? "4" : m["tag"]);//管理员标记是否退款？暂定
            //info.Add("dividends", m["dividends"].ToString());//本周之前推广值【可提现】
            //info.Add("day_statistic", m["day_statistic"]);//每日统计【tax(原始贡献值，未抽成)、积分、胜利局数、游戏局数】
            //info.Add("tax_week", m["tax_week"]);//本周贡献(未抽成)
            //var items = m["items"] as Dictionary<string, object>;//道具信息 已废弃？暂定
            //info.Add("coin", 0);
            //info.Add("credit", 0);
            //info.Add("ban", 0);
            //info.Add("tag", 0);//标记用户
            //                   //items【room_card 房卡、credit  信用分、ban  是否禁止参加游戏、coin 金币】
            //if (items.ContainsKey("ban")) info["ban"] = items["ban"];//目前使用中
            //if (items.ContainsKey("tag")) info["tag"] = items["tag"];//目前使用中
            //if (items.ContainsKey("coin")) info["coin"] = items["coin"];
            //if (items.ContainsKey("credit")) info["credit"] = items["credit"];
            Base.WriteLog("dal GetClubMemberModel info id end:", info["id"]);
            return info;
        }
        public static string ConvertIcon(string icon)
        {
            if (icon.Trim() == "" || icon == "i84") icon = "i1";
            if (icon[0] == 'i') return string.Format("http://res.2255.cn/face/{0}.png", icon.Substring(1).PadLeft(3, '0'));
            if (icon.Length > 4)
            {
                if (icon.Substring(0, 4) == "http") return icon;
                if (icon.Substring(0, 4) == "role") return string.Format("http://res.2255.cn/role{0}.jpg", icon);
                if (icon.Substring(0, 3) == "uc_") return string.Format("http://res.2255.cn/face/m{0}.png", icon.Substring(1));
            }
            return string.Format("http://res.2255.cn/face/m{0}.png", icon.Substring(1));
        }
        public IDictionary<string, Newtonsoft.Json.Linq.JToken> GetLeagueClubmembersList(string club_id, string member_id)
        {
            try
            {
                if (string.IsNullOrEmpty(club_id))
                    return null;
                string m = "";
                if (!string.IsNullOrEmpty(member_id))
                    m = ",\"member_id\":\"" + member_id + "\"";
                string param = "{\"module\":\"club_member\",\"func\":\"members_clubs\",\"args\":{\"club_id\":\"" + club_id + "\"" + m + "}}";
                var res = PostClubServer<ClubsRes<object[]>>(ClubsURI, param);
                if (res == null || res.ret != 0) return null;
                return res.msg[1] as IDictionary<string, Newtonsoft.Json.Linq.JToken>;
            }
            catch (Exception ex)
            {
                WriteError("GuildBLL GetLeagueClubmembersList ex:", ex.Message);
            }
            return null;
        }
        public Dictionary<string, object> GetMembersList(string member_id)
        {
            try
            {
                if (string.IsNullOrEmpty(member_id))
                    return null;
                string param = "{\"module\":\"club_member\",\"func\":\"get_player_clubs\",\"args\":{\"player_id\":\"" + member_id + "\"}}";
                var res = PostClubServer<ClubsRes<Dictionary<string, object>>>(ClubsURI, param);
                if (res == null || res.ret != 0) return null;
                return res.msg as Dictionary<string, object>;
            }
            catch (Exception ex)
            {
                WriteError("GuildDAL GetMembersList ex:", ex.Message);
            }
            return null;
        }
        public Dictionary<string, object> GetMemberActive(string club_id, string day)
        {
            if (string.IsNullOrEmpty(club_id) || string.IsNullOrEmpty(day)) return null;
            string param = "{\"module\":\"club_member\",\"func\":\"club_activity_day\",\"args\":{\"club_id\":\"" + club_id + "\",\"ymd\":\"" + day + "\"}}";
            var res = PostClubServer<ClubsRes<MembersActivity>>(ClubsURI, param);
            if (res.ret == 0) return res.msg.activity;
            return null;
        }
        public Dictionary<string, object> GetClubActive(long pageSize, long pageIndex, long order)
        {
            if (pageSize < 1 || pageIndex < 1) return null;
            string rank = "asc";
            if (order == -1) rank = "desc";
            string param = "{\"module\":\"club_dividends\",\"func\":\"rank\",\"args\":{\"size\":" + pageSize + ",\"page\":" + pageIndex + ",\"order\":\"" + rank + "\"}}";
            var res = PostClubServer<ClubsRes<Dictionary<string, object>>>(ClubsURI, param);
            if (res.ret == 0) return res.msg;
            return null;
        }
        public Dictionary<string, object> GetClubActiveCount()
        {
            string param = "{\"module\":\"club_dividends\",\"func\":\"rank_count\",\"args\":{}}";
            var res = PostClubServer<ClubsRes<Dictionary<string, object>>>(ClubsURI, param);
            if (res.ret == 0) return res.msg;
            return null;
        }
        public List<ClubsLink> GetClubLinkByGame(string game, string round)
        {
            if (string.IsNullOrEmpty(game)) return null;
            string param = "{\"module\":\"club_kv\",\"func\":\"get_club_by_game\",\"args\":{\"name\":\"" + game + "\",\"r\":\"" + round + "\"}}";
            var res = PostClubServer<ClubsRes<List<ClubsLink>>>(ClubsURI, param);
            if (res.ret == 0)
                return res.msg;
            return null;
        }
        public Dictionary<string, object> GetClubBaseInfo(string[] clubIds)
        {
            if (clubIds.Length < 1) return null;
            Protocol2 p = new Protocol2();
            p.module = "club";
            p.func = "club_info";
            p.args = clubIds;
            var res = PostClubServer<ClubsRes<Dictionary<string, object>>>(ClubsURI, Json.SerializeObject(p));
            if (res.ret == 0) return res.msg;
            return null;
        }
        public Dictionary<string, ClubsLink> GetLinkInfoByGame(string[] clubIds)
        {
            if (clubIds.Length < 1) return null;
            Protocol2 p = new Protocol2();
            p.module = "club_kv";
            p.func = "select";
            p.args = clubIds;
            var res = PostClubServer<ClubsRes<List<List<ClubsLink>>>>(ClubsURI, Json.SerializeObject(p));
            if (res.ret != 0) return null;
            Dictionary<string, ClubsLink> dic = new Dictionary<string, ClubsLink>();
            foreach (List<ClubsLink> items in res.msg)
            {
                foreach (ClubsLink item in items)
                {
                    if (item.field == "game") continue;
                    ClubsLink link = new ClubsLink();
                    if (dic.ContainsKey(item.club_id))
                        link = dic[item.club_id];
                    else
                        dic.Add(item.club_id, link);
                    if (item.field == "phone") link.phone = item.value.ToString();
                    else if (item.field == "qq") link.qq = item.value.ToString();
                    else if (item.field == "wechat") link.wechat = item.value.ToString();
                    else if (item.field == "xl") link.xl = item.value.ToString();
                    dic[item.club_id] = link;
                }
            }
            return dic;
        }
        public ClubsServer GetClubRoomCard(int clubId)
        {
            try
            {
                if (clubId < 1)
                    return null;
                string param = "{\"module\":\"club_items\",\"func\":\"get\",\"args\":{\"club_id\":" + clubId + ",\"player_id\":0}}";
                return PostClubServer<ClubsServer>(ClubsURI, param);
            }
            catch (Exception ex)
            {
                WriteError("post GetClubRoomCard ex:", ex.Message);
            }
            return null;
        }
        public ClubsMembersServer GetClubMembers(int clubId)
        {
            try
            {
                if (clubId < 1)
                    return null;
                string param = "{\"module\":\"club_member\",\"func\":\"members_count\",\"args\":{\"club_id\":" + clubId + "}}";
                return PostClubServer<ClubsMembersServer>(ClubsURI, param);
            }
            catch (Exception ex)
            {
                WriteError("post GetClubMembers ex:", ex.Message);
            }
            return null;
        }
        public Result<Guild> GetGuildInfo(int id)
        {
            Result<Guild> res = new Result<Guild>();
            try
            {
                var postres = PostRecordServer<Guild>(GuildUrl + "get_guild_list", SearchCondition.Current.AddInt("ID", id).ToString());
                if (postres != null && postres.Code > 0)
                {
                    res.Code = postres.Code;
                    res.R = postres.R[0];
                }
            }
            catch (Exception ex)
            {
                BaseDAL.WriteError("post get_guild_list--->guildinfo ex:", ex.Message);
            }
            return res;
        }
        Guild makeGuildInfo(DataRow dr, bool _isList)
        {
            var r = new Guild();
            if (dr["ID"] != null && !string.IsNullOrEmpty(dr["ID"].ToString()))
                r.ID = int.Parse(dr["ID"].ToString());
            if (dr["CreateTime"] != null && !string.IsNullOrEmpty(dr["CreateTime"].ToString()))
                r.CreateTime = DateTime.Parse(dr["CreateTime"].ToString());
            if (dr["Exp"] != null && !string.IsNullOrEmpty(dr["Exp"].ToString()))
                r.Exp = int.Parse(dr["Exp"].ToString());
            r.Master = dr["Master"].ToString();
            r.Name = dr["Name"].ToString();
            if (dr["UserCount"] != null && !string.IsNullOrEmpty(dr["UserCount"].ToString()))
                r.UserCount = int.Parse(dr["UserCount"].ToString());
            if (dr["ActiveUserNumOfLastWeek"] != null && !string.IsNullOrEmpty(dr["ActiveUserNumOfLastWeek"].ToString()))
                r.ActiveUserNumOfLastWeek = int.Parse(dr["ActiveUserNumOfLastWeek"].ToString());
            if (dr["ActiveUserNumOfCurrentWeek"] != null && !string.IsNullOrEmpty(dr["ActiveUserNumOfCurrentWeek"].ToString()))
                r.ActiveUserNumOfCurrentWeek = int.Parse(dr["ActiveUserNumOfCurrentWeek"].ToString());
            return r;
        }
        public List<GuildApplyRecord> GetGuildApplyList(GuildApplyRecordSearch search, out int rowCount)
        {
            rowCount = 0;
            try
            {
                SearchCondition<GuildApplyRecord> current = SearchCondition<GuildApplyRecord>.Current;
                GuildApplyRecord model = search.SearchObj;
                current.AddPage(search.PageIndex, search.PageSize);
                current.AddNumber(p => p.Flag, model.Flag);
                if (search.IsSearchKey)
                {
                    if (search.IsExact == 1)
                    {
                        current.Add(p => p.OrderNo, model.OrderNo);
                        current.Add(p => p.Account, model.Account);
                        current.Add(p => p.GuildName, model.GuildName);
                        current.Add(p => p.TransId, model.TransId);
                        current.Add(p => p.AlipayAccount, model.AlipayAccount);
                    }
                    else
                    {
                        current.Add(p => p.OrderNo, TOpeart.LK, model.OrderNo);
                        current.Add(p => p.Account, TOpeart.LK, model.Account);
                        current.Add(p => p.GuildName, TOpeart.LK, model.GuildName);
                        current.Add(p => p.TransId, TOpeart.LK, model.TransId);
                        current.Add(p => p.AlipayAccount, TOpeart.LK, model.AlipayAccount);
                    }
                }
                if (search.IsChkTime)
                {
                    if (search.TimeType == 1)
                        current.AddBetween(p => p.CreateDate, search.StartTime, search.OverTime);
                    else if (search.TimeType == 2)
                        current.AddBetween(p => p.PayDate, search.StartTime, search.OverTime);
                    else if (search.TimeType == 3)
                        current.AddBetween(p => p.ApplyRefundTime, search.StartTime, search.OverTime);
                }
                var res = PostRecordServer<GuildApplyRecord>(GuildUrl + "get_applyguild_list", current.ToString());
                if (res == null || res.Code < 1)
                    return null;
                rowCount = res.Code;
                return res.R;
            }
            catch (Exception ex)
            {
                BaseDAL.WriteError("post get_applyguild_list ex:", ex.Message);
            }
            return null;
        }
        public int SetGuildActive(long id, long currentnum, long lastnum, long exp)
        {
            R res = Http.Post<R>(GuildUrl + "update_guild", id, currentnum, lastnum, exp);
            if (res != null)
                return res.res;
            return 0;
        }
        public int SetSuggestGuild(List<object> list)
        {
            string param = JsonConvert.SerializeObject(list);
            string param_key = (WebKey + param).ToLower();
            string sign = MF.Common.Security.MD5.Encrypt(param_key, Encoding.UTF8);
            List<object> newlist = new List<object>();
            newlist.AddRange(list);
            newlist.Add(sign);
            param = JsonConvert.SerializeObject(newlist.ToArray());
            R res = Http.Post<R>(GuildUrl + "set_suggest_guild", param);
            if (res != null)
                return res.res;
            return 0;
        }
        public Result<GuildApplyRecord> GetGuildApplyInfo(int id)
        {
            Result<GuildApplyRecord> res = new Result<GuildApplyRecord>();
            try
            {
                var postres = PostRecordServer<GuildApplyRecord>(GuildUrl + "get_applyguild_list", SearchCondition.Current.AddInt("ID", id).ToString());
                if (postres != null && postres.Code > 0)
                {
                    res.Code = postres.Code;
                    res.R = postres.R[0];
                }
            }
            catch (Exception ex)
            {
                BaseDAL.WriteError("post get_applyguild_list--->GetGuildApplyInfo ex:", ex.Message);
            }
            return res;
        }
        public int SetApplyGuildFlag(long id, string adminuser, string memo)
        {
            R res = Http.Post<R>(GuildUrl + "deal_apply_refund", id, 4, adminuser, memo);
            if (res != null)
                return res.res;
            return 0;
        }
        public ClubsServer AddClubRoomcard(long clubId, long count)
        {
            if (clubId < 0 || count < 1)
                return null;
            string param = "{\"module\":\"club_items\",\"func\":\"add\",\"args\":{\"club_id\":" + clubId + ",\"count\":" + count + ",\"name\":\"room_card\",\"player_id\":0}}";
            return PostClubServer<ClubsServer>(ClubsURI, param);
        }
        public ClubsMembersServer GetRecommondclubslist()
        {
            try
            {
                string param = "{\"module\":\"club\",\"func\":\"get_recommend\",\"args\":[]}";
                return PostClubServer<ClubsMembersServer>(ClubsURI, param);
            }
            catch (Exception ex)
            {
                WriteError("post GetRecommondclubslist ex:", ex.Message);
            }
            return null;
        }
        public ClubsMembersServer SetRecomClubs(object[] clubsId, long type)
        {
            try
            {

                if (type < 1 || clubsId == null || clubsId.Length < 1)
                    return null;
                WriteDebug(clubsId.Length.ToString());
                if (clubsId.Length > 0)
                    WriteDebug(clubsId[0].ToString());
                string param = "";
                if (type == 1)//增加推荐俱乐部
                    param = "{\"module\":\"club\",\"func\":\"add_recommend\",\"args\":[" + String.Join(",", clubsId) + "]}";
                else if (type == 2)//删除推荐俱乐部
                    param = "{\"module\":\"club\",\"func\":\"del_recommend\",\"args\":[" + String.Join(",", clubsId) + "]}";
                return PostClubServer<ClubsMembersServer>(ClubsURI, param);
            }
            catch (Exception ex)
            {
                WriteError("post SetRecomClubs ex:", ex.Message);
            }
            return null;
        }

        public ClubsServer GetGuildLinkList(long clubId)
        {
            try
            {
                if (clubId < 1)
                    return null;
                string param = "{\"module\":\"club\",\"func\":\"get_links\",\"args\":{\"club_id\":" + clubId + "}}";
                return PostClubServer<ClubsServer>(ClubsURI, param);
            }
            catch (Exception ex)
            {
                WriteError("post get_club_link_list ex:", ex.Message);
            }
            return null;
        }
        public ClubsServerRes CloseRoom(string RoomId)
        {
            try
            {
                if (string.IsNullOrEmpty(RoomId))
                    return null;
                //string param = "{\"module\":\"club_game\",\"func\":\"close_room\",\"args\":{\"room_id\":\"" + RoomId + "\"}}";
                string param = "{\"module\":\"club_game\",\"func\":\"close_room\",\"args\":{\"room_id\":" + int.Parse(RoomId) + "}}";
                return PostClubServer<ClubsServerRes>(ClubsURI, param);
            }
            catch (Exception ex)
            {
                WriteError("post set_club_link ex:", ex.Message);
            }
            return null;
        }
        public ClubsServerRes SetClubLink(long type, long clubId, long clubLinkId)
        {
            try
            {
                if (type < 1 || clubId < 1 || clubLinkId < 1)
                    return null;
                string func = "";
                if (type == 1)
                    func = "add_links";
                else if (type == 2)
                    func = "del_links";
                string param = "{\"module\":\"club\",\"func\":\"" + func + "\",\"args\":{\"club_id\":" + clubId + ",\"link_id\":" + clubLinkId + "}}";
                return PostClubServer<ClubsServerRes>(ClubsURI, param);
            }
            catch (Exception ex)
            {
                WriteError("post set_club_link ex:", ex.Message);
            }
            return null;
        }
        public ClubsServerRes VerifyGuildStatus(long clubId, long status)
        {
            try
            {
                if (clubId < 1)
                    return null;
                string param = "";
                if (status == 10000)//解散俱乐部
                {
                    param = "{\"module\":\"club\",\"func\":\"delete\",\"args\":{\"club_id\":" + clubId + "}}";
                }
                else
                {
                    param = "{\"module\":\"club\",\"func\":\"agree_clubs\",\"args\":{\"club_id\":" + clubId + ",\"status\":" + status + "}}";
                }
                return PostClubServer<ClubsServerRes>(ClubsURI, param);
            }
            catch (Exception ex)
            {
                WriteError("post verify_club_status ex:", ex.Message);
            }
            return null;
        }


        public List<ClubsStatistic> ClubStatisticDay(string time, long clubId, string account)
        {
            try
            {
                if ((clubId != -1 && clubId < 1) || string.IsNullOrEmpty(time) || string.IsNullOrEmpty(account))
                    return null;
                List<ClubsStatistic> clubslist = new List<ClubsStatistic>();
                string param = "";
                ClubsStatisticRes serverres = null;
                ClubsModelServer clubnameres = null;
                ClubsStatistic info = null;
                if (clubId == -1)//查询全部
                {
                    string[] clubsinfo = GetClubStatistic(account);
                    long id = 0;
                    if (clubsinfo == null || clubsinfo.Length < 1)
                        return null;
                    foreach (string idstr in clubsinfo)
                    {
                        id = long.Parse(idstr);
                        if (id < 1)
                            continue;
                        //clubname
                        param = "{\"module\":\"club\",\"func\":\"select\",\"args\":{\"id\":" + id + "}}";
                        clubnameres = PostClubServer<ClubsModelServer>(ClubsURI, param);
                        if (clubnameres == null || clubnameres.ret != 0 || clubnameres.msg == null)
                            continue;
                        info = new ClubsStatistic();
                        info.clubid = id;
                        info.clubname = clubnameres.msg.Name;
                        //clubinfo
                        param = "{\"module\":\"club_log\",\"func\":\"day_statistic\",\"args\":{\"club_id\":" + id + ",\"ymd\":\"" + time + "\"}}";
                        serverres = PostClubServer<ClubsStatisticRes>(ClubsURI, param);
                        if (serverres == null || serverres.ret != 0 || serverres.msg == null)
                            continue;
                        info.online = serverres.msg.online;
                        info.round = serverres.msg.round;

                        clubslist.Add(info);
                    }
                    return clubslist;
                }
                else//输入俱乐部id查询
                {
                    //clubname
                    param = "{\"module\":\"club\",\"func\":\"select\",\"args\":{\"id\":" + clubId + "}}";
                    clubnameres = PostClubServer<ClubsModelServer>(ClubsURI, param);
                    if (clubnameres == null || clubnameres.ret != 0 || clubnameres.msg == null)
                    {
                        clubslist.Add(info);
                        return clubslist;
                    }
                    info = new ClubsStatistic();
                    info.clubid = clubId;
                    info.clubname = clubnameres.msg.Name;
                    //clubinfo
                    param = "{\"module\":\"club_log\",\"func\":\"day_statistic\",\"args\":{\"club_id\":" + clubId + ",\"ymd\":\"" + time + "\"}}";
                    serverres = PostClubServer<ClubsStatisticRes>(ClubsURI, param);
                    if (serverres == null || serverres.ret != 0 || serverres.msg == null)
                        return null;
                    info.online = serverres.msg.online;
                    info.round = serverres.msg.round;
                    //addclubdata
                    AddClubStatistic(clubId, account);
                    clubslist.Add(info);
                    return clubslist;
                }
            }
            catch (Exception ex)
            {
                WriteError("ClubStatisticDay ex:", ex.Message);
            }
            return null;
        }
        public string[] GetClubStatistic(string account)
        {
            int rowCount = 0;
            try
            {
                var search = new ClubStatisticSearch() { PrimaryKey = "[id]" };
                if (!string.IsNullOrEmpty(account))
                    search.Where += string.Format(" and AdminAcc='{0}'", account);
                var dt = GetSearchData(search, DBName.Manage, out rowCount);
                if (rowCount < 1)
                    return null;
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0] != null && dt.Rows[0]["ClubIdsInfo"] != null && !string.IsNullOrEmpty(dt.Rows[0]["ClubIdsInfo"].ToString()))
                    return dt.Rows[0]["ClubIdsInfo"].ToString().Split(',');
            }
            catch (Exception ex)
            {
                WriteError("GuildDAL.GetClubStatistic Error:", ex.Message, ex.StackTrace, ex.Source);
            }
            return null;
        }

        public void AddClubStatistic(long clubid, string account)
        {
            try
            {
                if (string.IsNullOrEmpty(account) || clubid < 1)
                    return;
                var args = new SqlParameter[] {
                    new SqlParameter("@account",account),
                    new SqlParameter("@clubid",clubid) };
                BaseDAL.dbname = DBName.Manage;//必须优先设置需要使用数据库
                DataHelper.ExecuteProcedure("mf_P_AddClubStatistic", args);
            }
            catch (Exception e)
            {
                BaseDAL.WriteError("AddClubStatistic ex:", e.Message);
            }
        }
        public void DelClubStatisticClubId(long clubid, string account)
        {
            try
            {
                if (string.IsNullOrEmpty(account) || clubid < 1)
                    return;
                var args = new SqlParameter[] {
                    new SqlParameter("@account",account),
                    new SqlParameter("@clubid",clubid) };
                BaseDAL.dbname = DBName.Manage;//必须优先设置需要使用数据库
                DataHelper.ExecuteProcedure("mf_P_DelClubStatistic", args);
            }
            catch (Exception e)
            {
                BaseDAL.WriteError("DelClubStatisticClubId ex:", e.Message);
            }
        }
        public string GetCacheClubName(string clubId)
        {
            try
            {
                if (string.IsNullOrEmpty(clubId)) return "";
                if (Cache.CacheClubName != null && Cache.CacheClubName.ContainsKey(clubId))
                    return Cache.CacheClubName[clubId];
                ClubsModelServer gs = GetClubModelList("Id", clubId);
                if (gs != null && gs.ret == 0 && gs.msg != null)
                    return gs.msg.Name;
            }catch(Exception ex)
            {
                Base.WriteError("GetCacheClubName ex:", ex.Message);
            }
            return "";
        }
        public void SetCacheClubName(string clubId,string name)
        { 
            try
            {
                if (string.IsNullOrEmpty(clubId) || string.IsNullOrEmpty(name)) return;
                if (Cache.CacheClubName == null || !Cache.CacheClubName.ContainsKey(clubId))
                    Cache.CacheClubName.Add(clubId, name);
                else
                    Cache.CacheClubName[clubId] = name;
            }catch(Exception ex)
            {
                Base.WriteError("setcacheclubname ex:", ex.Message);
            }
        }
    }
}
