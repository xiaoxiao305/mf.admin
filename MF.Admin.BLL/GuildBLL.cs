using System;
using System.Collections.Generic;
using MF.Data;
using MF.Admin.DAL;
using Newtonsoft.Json;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace MF.Admin.BLL
{
    public class GuildBLL : Base
    {
        private static GuildDAL dal = new GuildDAL();

        //获取所有俱乐部
        public static List<ClubsModel> GetAllClubsList()
        {
            return dal.GetAllClubsList();
        }
        /// <summary>
        /// 获取俱乐部列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="exact">是否精确查询</param>
        /// <param name="field">查询字段类型【1name 2founder 3 id】</param>
        /// <param name="key">查询字段值</param>
        /// <param name="checktime">是否按时间查询</param>
        /// <param name="startTime">查询开始时间</param>
        /// <param name="overTime">查询结束时间</param>
        public static List<ClubsModel> GetGuildList(long filed, string value)
        {
            try
            {
                if (filed < 1 || string.IsNullOrEmpty(value))
                    return null;
                string key = "";
                switch (filed)
                {
                    case 1:
                        key = "name";
                        break;
                    case 2:
                        key = "founder";
                        break;
                    case 3:
                        key = "id";
                        break;
                }
                List<ClubsModel> clubsList = new List<ClubsModel>();
                if (filed == 2)//可能返回多个值
                {
                    ClubsListServer cls = dal.GetClubList(key, value);
                    if (cls != null && cls.ret == 0)
                    {
                        foreach (ClubsModel m in cls.msg)
                        {
                            clubsList.Add(m);
                        }
                    }
                }
                else
                {
                    ClubsModelServer cms = dal.GetClubModelList(key, value);
                    if (cms != null && cms.ret == 0)
                    {
                        clubsList.Add(cms.msg);
                    }
                }
                if (clubsList == null || clubsList.Count < 0)
                    return null;
                List<ClubsModel> newClubsList = new List<ClubsModel>();
                foreach (ClubsModel cm in clubsList)
                {
                    cm.Room_Card = GetClubRoomCard(cm.Id);
                    cm.Members_Count = GetClubMembers(cm.Id);
                    newClubsList.Add(cm);
                }
                return newClubsList;
            }
            catch (Exception ex)
            {
                WriteError("GuildBLL GetGuildList ex:", ex.Message);
            }
            return null;
        }
        public static List<Dictionary<string, object>> GetClubmembersList(string club_id, string member_id)
        {
            try
            {
                //Base.WriteLog("bll getclubmembers start.");
                if (string.IsNullOrEmpty(club_id))
                    return null;
                return dal.GetClubmembersList(club_id, member_id);
            }
            catch (Exception ex)
            {
                WriteError("GuildBLL GetClubmembersList ex:", ex.Message);
            }
            return null;
        }
        public static List<ClubMembers> GetLeagueClubmembersList(string club_id, string member_id)
        {
            try
            {
                if (string.IsNullOrEmpty(club_id))
                    return null;
                var list = dal.GetLeagueClubmembersList(club_id, member_id);
                if (list == null || list.Count < 1) return null;
                var uinfos = new UserDAL().GetMemberNickName(list.Keys.ToArray());
                //
                List<ClubMembers> newlist = new List<ClubMembers>();
                Dictionary<string, object> d = new Dictionary<string, object>();
                List<Dictionary<string, object>> infos = new List<Dictionary<string, object>>();
                foreach (var items in list)
                {
                    ClubMembers member = new ClubMembers();
                    member.memberid = items.Key;
                    if (uinfos != null && uinfos.Count > 0)
                    {
                        foreach (string uid in uinfos.Keys)
                        {
                            if (!uid.ToUpper().Equals(items.Key.ToUpper())) continue;
                            member.nickname = uinfos[uid];
                            break;
                        }
                    }
                    List<string> club_ids = new List<string>();
                    foreach (var clubId in items.Value)
                    {
                        club_ids.Add(clubId.ToString());
                    }
                    Dictionary<string, object> clubinfos = dal.GetClubBaseInfo(club_ids.ToArray());
                    if (clubinfos == null || clubinfos.Count < 1) continue;
                    Dictionary<string, string> club = new Dictionary<string, string>();
                    foreach (string clubId in clubinfos.Keys)
                    {
                        club.Add(clubId, (clubinfos[clubId] as IDictionary<string, Newtonsoft.Json.Linq.JToken>)["name"].ToString());

                    }
                    member.clubsinfo = club;
                    newlist.Add(member);
                }
                return newlist;
            }
            catch (Exception ex)
            {
                WriteError("GuildBLL GetMembersList ex:", ex.Message);
            }
            return null;
        }
        public static List<ClubsModel> GetMembersList(long filed, string key)
        {
            try
            {
                string member_id = "";
                switch (filed)
                {
                    case 1:
                        member_id = key;
                        break;
                    case 2:
                        int rowCount = 0;
                        var ulist = new UserDAL().GetUserList(key, "", out rowCount);
                        if (rowCount < 1)
                            return null;
                        member_id = ulist[0].ChargeId;
                        break;
                }
                if (string.IsNullOrEmpty(member_id)) return null;
                var listinfos = dal.GetMembersList(member_id);
                if (listinfos == null || listinfos.Count < 1 || !listinfos.ContainsKey("clubs")) return null;
                List<string> club_ids = (listinfos["clubs"] as Newtonsoft.Json.Linq.JArray).ToObject<List<string>>();
                Dictionary<string, object> clubinfos = dal.GetClubBaseInfo(club_ids.ToArray());
                if (clubinfos == null || clubinfos.Count < 1) return null;
                List<ClubsModel> clubs = new List<ClubsModel>();
                foreach (string clubId in clubinfos.Keys)
                {
                    ClubsModel club = new ClubsModel();
                    club.Name = (clubinfos[clubId] as IDictionary<string, Newtonsoft.Json.Linq.JToken>)["name"].ToString();
                    club.Id = int.Parse(clubId);
                    club.Founder = member_id;//临时存储当前查询者
                    clubs.Add(club);
                }
                return clubs;
            }
            catch (Exception ex)
            {
                WriteError("GuildBLL GetMembersList ex:", ex.Message);
            }
            return null;
        }

        public static List<MembersActivity> GetMemberActive(string club_id, string day)
        {
            if (string.IsNullOrEmpty(club_id) || string.IsNullOrEmpty(day)) return null;
            Dictionary<string, object> v = dal.GetMemberActive(club_id, day);
            if (v == null || v.Count < 1) return null;
            List<Dictionary<string, object>> infos = new UserDAL().GetMemberInfo(v.Keys.ToArray());
            List<MembersActivity> list = new List<MembersActivity>();
            foreach (string member in v.Keys)
            {
                var m = new MembersActivity() { member_id = member, active = v[member].ToString() };
                if (infos != null && infos.Count > 0)
                {
                    for (int i = 0; i < infos.Count; i++)
                    {
                        if (infos[i].ContainsKey("id") && infos[i]["id"].ToString().ToUpper().Equals(member.ToUpper()))
                        {
                            m.nick_name = infos[i]["nickname"].ToString();
                            break;
                        }
                    }
                }
                list.Add(m);
            }
            return list;
        }
        public static List<MembersActivity> GetClubActive(long pageSize, long pageIndex, long order)
        {
            if (pageSize < 1 || pageIndex < 1) return null;
            Dictionary<string, object> v = dal.GetClubActive(pageSize, pageIndex, order);
            if (v == null || v.Count < 1) return null;
            List<List<long>> list = JsonConvert.DeserializeObject<List<List<long>>>(v["rank"].ToString());
            if (list == null || list.Count < 1) return null;
            Dictionary<string, MembersActivity> dic = new Dictionary<string, MembersActivity>();
            foreach (List<long> l in list)
            {
                if (!dic.ContainsKey(l[0].ToString()))
                    dic.Add(l[0].ToString(), new MembersActivity() { club_id = int.Parse(l[0].ToString()), active = l[1].ToString() });
                else
                    dic[l[0].ToString()] = new MembersActivity() { club_id = int.Parse(l[0].ToString()), active = l[1].ToString() };
            }
            var v2 = dal.GetClubBaseInfo(dic.Keys.ToArray());
            if (v2 == null || v2.Count < 1) return dic.Values.ToList<MembersActivity>();
            foreach (string club_id in dic.Keys)
            {
                foreach (string club_id2 in v2.Keys)
                {
                    if (club_id == club_id2)
                    {
                        dic[club_id].nick_name = JsonConvert.DeserializeObject<ClubsModel>(v2[club_id2].ToString()).Name;
                        break;
                    }
                }
            }
            return dic.Values.ToList<MembersActivity>();
        }
        public static Dictionary<string, object> GetClubActiveCount()
        {
            return dal.GetClubActiveCount();
        }

        public static List<ClubsLink> GetClubGameLinkList(string game, string round)
        {
            if (string.IsNullOrEmpty(game)) return null;
            List<ClubsLink> list = dal.GetClubLinkByGame(game, round);
            if (list == null || list.Count < 1) return list;
            List<string> clubsId = new List<string>();
            foreach (ClubsLink item in list)
            {
                clubsId.Add(item.club_id.ToString());
            }
            if (clubsId.Count < 1) return list;
            Dictionary<string, object> baseInfo = dal.GetClubBaseInfo(clubsId.ToArray());
            if (baseInfo == null || baseInfo.Count < 1) return list;
            Dictionary<string, ClubsLink> linklist = dal.GetLinkInfoByGame(clubsId.ToArray());
            if (linklist == null || linklist.Count < 1) return list;
            List<ClubsLink> newList = new List<ClubsLink>();
            foreach (ClubsLink info in list)
            {
                foreach (string clubId2 in baseInfo.Keys)
                {
                    if (clubId2 == info.club_id)
                    {
                        var d = baseInfo[clubId2] as IDictionary<string, Newtonsoft.Json.Linq.JToken>;
                        if (d != null && d.ContainsKey("name"))
                            info.club_name = d["name"].ToString();
                        break;
                    }
                }
                foreach (string clubId in linklist.Keys)
                {
                    if (clubId == info.club_id)
                    {
                        info.phone = linklist[clubId].phone;
                        info.qq = linklist[clubId].qq;
                        info.wechat = linklist[clubId].wechat;
                        info.xl = linklist[clubId].xl;
                        break;
                    }
                }
                newList.Add(info);
            }
            return newList;
        }
        private static int GetClubRoomCard(int clubId)
        {
            if (clubId < 1)
                return 0;
            ClubsServer cs = dal.GetClubRoomCard(clubId);
            if (cs == null || cs.ret != 0 || cs.msg == null || cs.msg.Length < 2)
                return 0;
            object obj = cs.msg[1];
            ClubsModel cm = JsonConvert.DeserializeObject<ClubsModel>(obj.ToString().Replace("\r\n", "").Replace(" ", ""));
            if (cm == null || cm.Room_Card < 1)
                return 0;
            return cm.Room_Card;
        }
        private static ClubsRes<object> GetClubTax(int clubId, long time)
        {
            if (clubId < 1)
                return null;
            return dal.GetClubTax(clubId, time);
        }
        private static int GetClubMembers(int clubId)
        {
            if (clubId < 1)
                return 0;
            ClubsMembersServer cms = dal.GetClubMembers(clubId);
            if (cms == null || cms.ret != 0 || cms.msg.Length < 2)
                return 0;
            return cms.msg[1];
        }
        public static Result<Guild> GetGuildInfo(int id)
        {
            if (id < 1)
                return null;
            return dal.GetGuildInfo(id);
        }
        public static List<GuildApplyRecord> GetGuildApplyList(long pageSize, long pageIndex, long flag, long exact, long filed, string key, long checktime, long timeType, long startTime, long overTime, out int rowCount)
        {
            rowCount = 0;
            var search = new GuildApplyRecordSearch();
            search.PageSize = (int)pageSize;
            search.PageIndex = (int)pageIndex;
            GuildApplyRecord model = new GuildApplyRecord();
            model.Flag = flag;
            if (!string.IsNullOrEmpty(key.Trim()))
            {
                search.IsSearchKey = true;
                search.IsExact = exact;
                if (filed == 1)
                    model.OrderNo = key;
                else if (filed == 2)
                    model.Account = key;
                else if (filed == 3)
                    model.GuildName = key;
                else if (filed == 4)
                    model.TransId = key;
                else if (filed == 5)
                    model.AlipayAccount = key;
            }
            if (checktime == 1)
            {
                search.IsChkTime = true;
                search.TimeType = timeType;
                search.StartTime = startTime;
                search.OverTime = overTime;
            }
            search.SearchObj = model;
            return dal.GetGuildApplyList(search, out rowCount);
        }
        public static int SetGuildActive(long id, long currentnum, long lastnum, long exp)
        {
            int res = dal.SetGuildActive(id, currentnum, lastnum, exp);
            string msg = "";
            if (res == 1)
                msg = string.Format("设置俱乐部id【{0}】活跃人数成功", id);
            else
                msg = string.Format("设置俱乐部id【{0}】活跃人数失败", id);
            AdminBLL.WriteSystemLog(CurrentUser.Account, ClientIP, msg, "GuildBLL.SetGuildActive", res == 1 ? res : 0, SystemLogEnum.SETGUILDACTIVE);
            return res;
        }
        public static int SetSuggestGuild(List<object> list, string filename)
        {
            int res = dal.SetSuggestGuild(list);
            string msg = "";
            if (res == 1)
                msg = string.Format("设置设置推荐俱乐部【{0}】成功", filename);
            else
                msg = string.Format("设置设置推荐俱乐部【{0}】失败", filename);
            AdminBLL.WriteSystemLog(CurrentUser.Account, ClientIP, msg, "GuildBLL.SetSuggestGuild", res == 1 ? res : 0, SystemLogEnum.SETRECOMMENDGUILD);
            return res;
        }
        public static Result<GuildApplyRecord> GetGuildApplyInfo(int id)
        {
            if (id < 1)
                return null;
            return dal.GetGuildApplyInfo(id);
        }
        public static int SetApplyGuildFlag(long id, string adminuser, string memo)
        {
            int res = dal.SetApplyGuildFlag(id, adminuser, memo);
            string msg = "";
            if (res == 1)
                msg = string.Format("处理保证金记录id【{0}】退款成功", id);
            else
                msg = string.Format("处理保证金记录id【{0}】退款失败", id);
            AdminBLL.WriteSystemLog(CurrentUser.Account, ClientIP, msg, "GuildBLL.SetApplyGuildFlag", res == 1 ? res : 0, SystemLogEnum.DEALAPPLYRECORD);
            return res;
        }
        /// <summary>
        /// 添加俱乐部房卡
        /// </summary>
        /// <param name="clubId">俱乐部id</param>
        /// <param name="count">房卡数量</param>
        /// <returns>
        ///  GameServer
        ///  成功时 ret = 0,  msg = [club_id, {"room_card": 1000}]
        ///  错误时 ret != 0, msg = Error
        /// </returns>
        public static ClubsServer AddClubRoomcard(long clubId, long count)
        {
            try
            {
                if (clubId < 0 || count < 1)
                    return null;
                ClubsServer cs = dal.AddClubRoomcard(clubId, count);
                string msg = string.Format("为俱乐部【Id:{0}】添加【{1}】房卡{2}", clubId, count, cs.ret == 0 ? "成功" : "失败");
                AdminBLL.WriteSystemLog(CurrentUser.Account, ClientIP, msg, "AddClubRoomcard", cs.ret == 0 ? 1 : 0, SystemLogEnum.ADDCLUBSROOMCARD);
                return cs;
            }
            catch (Exception ex)
            {
                WriteError("AddClubRoomcard ex:", ex.Message);
            }
            return null;
        }
        public static List<ClubsModel> GetRecommondclubslist()
        {
            ClubsMembersServer cms = dal.GetRecommondclubslist();
            if (cms == null || cms.ret != 0)
                return null;
            List<ClubsModel> newList = new List<ClubsModel>();
            List<ClubsModel> idList;
            foreach (int clubId in cms.msg)
            {
                idList = GetGuildList(3, clubId.ToString());
                if (idList != null && idList.Count > 0)
                    newList.AddRange(idList);
            }
            return newList;
        }
        public static int SetRecomClubs(object[] clubsId, long type)
        {
            if (type < 1 || clubsId == null || clubsId.Length < 1)
                return -2001;
            ClubsMembersServer cms = dal.SetRecomClubs(clubsId, type);
            if (cms == null || cms.ret != 0)
                return cms == null ? 0 : cms.ret;
            return 1;
        }

        /// <summary>
        /// 俱乐部关联信息
        /// </summary>
        public static ClubLinkInfo GetGuildLinkList(long clubId)
        {
            if (clubId < 1)
                return null;
            ClubsServer cs = dal.GetGuildLinkList(clubId);
            if (cs == null || cs.ret != 0 || cs.msg == null || cs.msg.Length < 2)
                return null;
            int currentClubId = 0;
            int.TryParse(cs.msg[0].ToString(), out currentClubId);
            List<ClubsModel> currentClubList = GetGuildList(3, currentClubId.ToString());
            if (currentClubList == null || currentClubList.Count < 1 || currentClubList[0] == null)
                return null;


            ClubLinkInfo clubLink = new ClubLinkInfo();
            clubLink.clubid = currentClubId;
            clubLink.name = currentClubList[0].Name;

            Newtonsoft.Json.Linq.JArray jArray = (Newtonsoft.Json.Linq.JArray)cs.msg[1];
            if (jArray == null || jArray.Count < 1)
                return null;
            string[] linkClubIds = new string[jArray.Count];
            int i = 0;
            foreach (string item in jArray)
            {
                linkClubIds[i++] = item;

            }
            List<ClubsModel> clubLinkList = null;
            List<ClubsModel> clubLinkList2 = new List<ClubsModel>();
            foreach (string linkClubId in linkClubIds)
            {
                clubLinkList = GetGuildList(3, linkClubId);
                if (clubLinkList != null && clubLinkList.Count > 0 && clubLinkList[0] != null)
                    clubLinkList2.Add(clubLinkList[0]);
            }
            clubLink.linkclub = clubLinkList2;
            return clubLink;
        }
        public static ClubsServerRes SetClubLink(long type, long clubId, long clubLinkId)
        {
            if (type < 1 || clubId < 1 || clubLinkId < 1)
                return null;
            return dal.SetClubLink(type, clubId, clubLinkId);
        }
        public static ClubsServerRes VerifyGuildStatus(long clubId, long status)
        {
            try
            {
                if (clubId < 1)
                    return null;
                ClubsServerRes csr = dal.VerifyGuildStatus(clubId, status);
                string msg = "";
                string s = status == 10000 ? "解散俱乐部" : status == 1 ? "设置俱乐部生效" : status == 0 ? "设置俱乐部失效" : "";
                if (csr != null && csr.ret == 0)
                    msg = string.Format(s + "ID【{0}】成功", clubId);
                else
                    msg = string.Format(s + "ID【{0}】失败", clubId);
                AdminBLL.WriteSystemLog(CurrentUser.Account, ClientIP, msg, "GuildBLL.VerifyGuildStatus", (csr != null && csr.ret == 0) ? 1 : 0, SystemLogEnum.VERIFYGUILDSTATUS);
                return csr;
            }
            catch (Exception ex)
            {
                WriteError("VerifyGuildStatus ex:", ex.Message);
            }
            return null;
        }

        public static List<ClubsStatistic> ClubStatisticDay(string time, long clubId)
        {
            if ((clubId != -1 && clubId < 1) || string.IsNullOrEmpty(time))
                return null;
            return dal.ClubStatisticDay(time, clubId, CurrentUser.Account);
        }


        public static void DelClubStatisticClubId(long clubId, string account)
        {
            if (clubId < 1 || string.IsNullOrEmpty(account))
                return;
            dal.DelClubStatisticClubId(clubId, account);
        }
        public static ClubsServerRes CloseRoom(string RoomId)
        {
            if (string.IsNullOrEmpty(RoomId))
                return null;
            return dal.CloseRoom(RoomId);
        }


        public static ClubsServerRes ClubMemberOpt(string member_id, string club_id, string func)
        {
            ClubsModelServer cms = dal.GetClubModelList("id", club_id);
            if (cms == null || cms.ret != 0) return null;
            return dal.ClubMemberOpt(member_id, club_id, cms.msg.Founder, func);
        }
        public static ClubsServerRes ExistLeague(string club_id)
        {
            if (string.IsNullOrEmpty(club_id)) return null;
            var cs = dal.ExistLeague(club_id);
            var oprState = 0; var msg = "退出俱乐部【" + club_id + "】联盟失败";
            if (cs != null && cs.ret == 0)
            {
                oprState = 1;
                msg = "退出俱乐部【" + club_id + "】联盟成功";
            }
            AdminBLL.WriteSystemLog(CurrentUser.Account, ClientIP, msg, "AjaxRequest.ExistLeague", oprState, SystemLogEnum.EXISTLEAGUE);
            return cs;
        }


        public static List<ClubsModel> GetHighTaxClub()
        {
            ClubsRes<object> res = dal.GetHighTaxClub();
            if (res == null || res.msg == null) return null;
            List<ClubsModel> list = new List<ClubsModel>();
            var msgRes = res.msg as IDictionary<string, JToken>;
            if (msgRes == null || msgRes.Count < 1 || !msgRes.ContainsKey("Clubs")) return null;
            List<int> clubs = (msgRes["Clubs"] as JArray).ToObject<List<int>>();
            ClubsModel model;
            foreach (int clubId in clubs)
            {
                model = new ClubsModel();
                model.Id = clubId;
                model.Name = dal.GetCacheClubName(clubId.ToString());
                model.Members_Count = GetClubMembers(clubId);
                //设置的最高税额
                ClubsRes<object> r = dal.GetClubHighTax(clubId);
                if (r.ret == 0)
                {
                    var msgRes2 = r.msg as IDictionary<string, JToken>;
                    if (msgRes2 != null && msgRes2.Count > 0 && msgRes2.ContainsKey("Tax"))
                        model.Max_Tax = long.Parse(msgRes2["Tax"].ToString());
                }
                list.Add(model);
            }
            return list;
        }
        public static ClubsServerRes AddHighTaxClub(int[] clubIds)
        {
            try
            {
                if (clubIds.Length < 1)
                    return null;
                ClubsServerRes csr = dal.AddHighTaxClub(clubIds);
                string msg = "";
                if (csr != null && csr.ret == 0)
                    msg = string.Format("添加高税俱乐部ID【{0}】成功", JsonConvert.SerializeObject(clubIds));
                else
                    msg = string.Format("添加高税俱乐部ID【{0}】失败", JsonConvert.SerializeObject(clubIds));
                AdminBLL.WriteSystemLog(CurrentUser.Account, ClientIP, msg, "GuildBLL.AddHighTaxClub", (csr != null && csr.ret == 0) ? 1 : 0, SystemLogEnum.ADDHIGHTAXCLUB);
                return csr;
            }
            catch (Exception ex)
            {
                WriteError("AddHighTaxClub ex:", ex.Message);
            }
            return null;
        }
        public static ClubsServerRes SetHighTaxClub(int clubId, long maxTax)
        {
            try
            {
                if (clubId < 1 || maxTax < 0)
                    return null;
                ClubsServerRes csr = dal.SetHighTaxClub(clubId, maxTax);
                string msg = "";
                if (csr != null && csr.ret == 0)
                    msg = string.Format("设置高税俱乐部ID【{0}】最高税额为【{1}】成功", clubId, maxTax);
                else
                    msg = string.Format("设置高税俱乐部ID【{0}】最高税额为【{1}】失败", clubId, maxTax);
                AdminBLL.WriteSystemLog(CurrentUser.Account, ClientIP, msg, "GuildBLL.SetHighTaxClub", (csr != null && csr.ret == 0) ? 1 : 0, SystemLogEnum.SETHIGHTAXCLUB);
                return csr;
            }
            catch (Exception ex)
            {
                WriteError("SetHighTaxClub ex:", ex.Message);
            }
            return null;
        }
        public static ClubsServerRes DelHighTaxClub(object[] clubIds)
        {
            try
            {
                if (clubIds.Length < 1)
                    return null;
                ClubsServerRes csr = dal.DelHighTaxClub(clubIds);
                string msg = "";
                if (csr != null && csr.ret == 0)
                    msg = string.Format("删除高税俱乐部ID【{0}】成功", JsonConvert.SerializeObject(clubIds));
                else
                    msg = string.Format("删除高税俱乐部ID【{0}】失败", JsonConvert.SerializeObject(clubIds));
                AdminBLL.WriteSystemLog(CurrentUser.Account, ClientIP, msg, "GuildBLL.AddHighTaxClub", (csr != null && csr.ret == 0) ? 1 : 0, SystemLogEnum.DELHIGHTAXCLUB);
                return csr;
            }
            catch (Exception ex)
            {
                WriteError("DelHighTaxClub ex:", ex.Message);
            }
            return null;
        }
        public static List<ClubsModel> GetClubTaxList(string club_id, long time)
        {
            try
            {
                if (string.IsNullOrEmpty(club_id)) return null;
                ClubsModelServer cms = dal.GetClubModelList("id", club_id);
                if (cms == null || cms.ret != 0) return null;
                List<ClubsModel> newClubsList = new List<ClubsModel>();
                ClubsModel cm = cms.msg;
                ClubsRes<object> taxRes = GetClubTax(cm.Id, time);
                if (taxRes == null || taxRes.ret != 0) return null;
                var msgRes = taxRes.msg as IDictionary<string, JToken>;
                if (msgRes == null) return null;
                foreach (string resTime in msgRes.Keys)
                {
                    ClubsModel cm3 = new ClubsModel();
                    cm3.Id = cm.Id;
                    cm3.Name = cm.Name;
                    cm3.TaxTime = resTime;
                    var timeRes = msgRes[resTime] as IDictionary<string, JToken>;
                    if (timeRes.ContainsKey("Activity_member_count") && timeRes["Activity_member_count"] != null)
                        cm.Activity_Member_Count = long.Parse(timeRes["Activity_member_count"].ToString());
                    if (timeRes.ContainsKey("tax") && timeRes["tax"] != null)
                        cm3.Tax = long.Parse(timeRes["tax"].ToString());
                    if (timeRes.ContainsKey("tax_round") && timeRes["tax_round"] != null)
                        cm3.Tax_Round = long.Parse(timeRes["tax_round"].ToString());
                    newClubsList.Add(cm3);
                }
                return newClubsList;
            }
            catch (Exception ex)
            {
                WriteError("GuildBLL GetClubTaxList ex:", ex.Message);
            }
            return null;
        }
    }
}
