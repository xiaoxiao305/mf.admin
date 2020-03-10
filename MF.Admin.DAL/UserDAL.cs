using System;
using System.Collections.Generic;
using MF.Data;
using System.Data;
using System.Net;
using System.Configuration;
using System.IO;
using System.Text;
using System.Data.SqlClient;
using MF.Common.Json;
using MF.Common.Security;
using MF.Protocol;


namespace MF.Admin.DAL
{
    public class R
    {
        public int res { get; set; }
    }
    public class UserDAL : BaseDAL
    {
        #region 获取用户信息
        /// <summary>
        /// 备份库中查询用户信息列表
        /// </summary>
        public List<Users> GetBackUserList(UserSearch search, out int rowCount)
        {
            rowCount = 0;
            if (dbconfig == null || dbconfig.Count < 1)
                return null;
            DataTable dt = null;
            //单库查询
            if (search.DBName != DBName.ALL)
                dt = BaseDAL.GetSearchData(search, search.DBName, out rowCount);
            else
            {
                //全库查询
                foreach (DBConst dbconst in dbconfig)
                {
                    if (dbconst.dbname >= 100)//user库
                    {
                        dt = BaseDAL.GetSearchData(search, (DBName)dbconst.dbname, out rowCount);
                        if (rowCount > 0)
                            break;
                    }
                }
            }
            if (rowCount < 1)
                return null;
            var list = new List<Users>();
            if (dt == null || dt.Rows.Count < 1)
                return null;
            foreach (DataRow dr in dt.Rows)
            {
                var u = makeUsers(dr);
                list.Add(u);
            }
            return list;
        }
        /// <summary>
        /// 生产服务器中查询用户信息列表
        /// </summary>
        public List<Users> GetUserList(string account, string chargeid, out int rowCount)
        {
            rowCount = 0;
            try
            {
                if (!string.IsNullOrEmpty(account))
                    account = account.ToLower();
                if (!string.IsNullOrEmpty(chargeid))
                    chargeid = chargeid.ToUpper();
                SearchCondition<Users> current = SearchCondition<Users>.Current;
                current.Add(p => p.Account, account);
                current.Add(p => p.ChargeId, chargeid);
                var res = PostRecordServer<Users>(RecordServerUrl + "get_user_list", current.ToString());
                if (res == null || res.Code < 1)
                    return null;
                rowCount = res.Code;
                if (res.R[0] != null)
                {

                    if (!string.IsNullOrEmpty(account))
                        SetCacheAccountList(account, res.R[0]);
                    if (!string.IsNullOrEmpty(chargeid))
                        SetCacheChargeList(chargeid, res.R[0]);
                }
                return res.R;
            }
            catch (Exception ex)
            {
                BaseDAL.WriteError("post get_user_list ex:", ex.Message);
            }
            return null;
        }
        /// <summary>
        /// 查询单个用户信息
        /// </summary>
        public Result<Users> GetUserInfo(DBSource dbsource, string account)
        {
            DataTable dt = null;
            var res = new Result<Users>();
            try
            {
                if (dbsource == DBSource.DBBACK)
                {
                    dt = BaseDAL.GetDataTable("mf_P_GetUserInfoByAcc", new SqlParameter[] { new SqlParameter("@account", account) }, DBName.USER_0_DY);
                    if (dt == null || dt.Rows.Count < 1)
                    {
                        res.Message = "getuinfo(acc:" + account + ") is null";
                        return res;
                    }
                    res.R = makeUsers(dt.Rows[0]);
                    res.Code = 1;
                }
                else//请求服务器
                {

                    SearchCondition<Users> current = SearchCondition<Users>.Current;
                    current.Add(p => p.Account, account);
                    var postres = PostRecordServer<Users>(RecordServerUrl + "get_user_info", current.ToString());
                    if (postres == null || postres.Code < 1)
                        return null;
                    res.Code = postres.Code;
                    res.R = postres.R[0];
                }
            }
            catch (Exception e)
            {
                res.Ex = e;
                res.Message = e.Message;
            }
            return res;
        }
        private Users makeUsers(DataRow dr)
        {
            try
            {
                var u = new Users();
                u.Account = dr["Account"].ToString();
                u.ADID = dr["ADID"].ToString();
                if (dr["Currency"] != null && !string.IsNullOrEmpty(dr["Currency"].ToString()))
                    u.Currency = long.Parse(dr["Currency"].ToString());
                if (dr["Flag"] != null && !string.IsNullOrEmpty(dr["Flag"].ToString()))
                    u.Flag = int.Parse(dr["Flag"].ToString());
                if (dr["Guest"] != null && !string.IsNullOrEmpty(dr["Guest"].ToString()))
                    u.Guest = int.Parse(dr["Guest"].ToString());
                if (dr["LastLogin"] != null && !string.IsNullOrEmpty(dr["LastLogin"].ToString()))
                    u.LastLogin = int.Parse(dr["LastLogin"].ToString());
                if (dr["LoginCount"] != null && !string.IsNullOrEmpty(dr["LoginCount"].ToString()))
                    u.LoginCount = int.Parse(dr["LoginCount"].ToString());
                if (dr["Lv"] != null && !string.IsNullOrEmpty(dr["Lv"].ToString()))
                    u.Lv = int.Parse(dr["Lv"].ToString());
                u.Name = dr["Name"].ToString();
                u.Nickname = dr["Nickname"].ToString();
                if (dr["ID"] != null && !string.IsNullOrEmpty(dr["ID"].ToString()))
                    u.ID = int.Parse(dr["ID"].ToString());
                u.RegistArea = dr["RegistArea"].ToString();
                if (dr["RegistDevice"] != null && !string.IsNullOrEmpty(dr["RegistDevice"].ToString()))
                    u.RegistDevice = int.Parse(dr["RegistDevice"].ToString());
                u.RegistIp = dr["RegistIp"].ToString();
                if (dr["Regitime"] != null && !string.IsNullOrEmpty(dr["Regitime"].ToString()))
                    u.Regitime = int.Parse(dr["Regitime"].ToString());
                if (dr["RoomCard"] != null && !string.IsNullOrEmpty(dr["RoomCard"].ToString()))
                    u.RoomCard = int.Parse(dr["RoomCard"].ToString());
                u.Identity = dr["Identity"].ToString();
                u.ChargeId = dr["ChargeId"].ToString();
                if (dr["LoginDevice"] != null && !string.IsNullOrEmpty(dr["LoginDevice"].ToString()))
                    u.LoginDevice = int.Parse(dr["LoginDevice"].ToString());
                u.LastIp = dr["LastIp"].ToString();
                if (dr["Bean"] != null && !string.IsNullOrEmpty(dr["Bean"].ToString()))
                    u.Bean = long.Parse(dr["Bean"].ToString());
                u.DeviceCode = dr["DeviceCode"].ToString();
                if (dr["Exp"] != null && !string.IsNullOrEmpty(dr["Exp"].ToString()))
                    u.Exp = int.Parse(dr["Exp"].ToString());
                u.GUID = dr["GUID"].ToString();
                u.Mobile = dr["Mobile"].ToString();
                u.PhoneKey = dr["PhoneKey"].ToString();
                if (dr["Sex"] != null && !string.IsNullOrEmpty(dr["Sex"].ToString()))
                    u.Sex = int.Parse(dr["Sex"].ToString());
                if (dr["Lock"] != null && !string.IsNullOrEmpty(dr["Lock"].ToString()))
                    u.Lock = int.Parse(dr["Lock"].ToString());
                if (dr["Relief"] != null && !string.IsNullOrEmpty(dr["Relief"].ToString()))
                    u.Relief = int.Parse(dr["Relief"].ToString());
                if (dr["Silver"] != null && !string.IsNullOrEmpty(dr["Silver"].ToString()))
                    u.Silver = long.Parse(dr["Silver"].ToString());
                return u;
            }
            catch (Exception ex)
            {
                WriteError("makeUsers ex:", ex.Message);
                return null;
            }
        }
        #endregion

        #region 子账号列表
        public List<Users> GetSubUserList(Search<Users> search, out int rowCount)
        {
            rowCount = 0;
            try
            {
                SearchCondition<Users> current = SearchCondition<Users>.Current;
                current.AddPage(search.PageIndex, search.PageSize);
                current.Add(p => p.Master, search.SearchObj.Master);
                var res = PostRecordServer<Users>(RecordServerUrl + "get_user_list", current.ToString());
                if (res == null || res.Code < 1)
                    return null;
                rowCount = res.Code;
                return res.R;
            }
            catch (Exception ex)
            {
                BaseDAL.WriteError("post get_user_list--->subuserlist ex:", ex.Message);
            }
            return null;
        }
        #endregion

        #region 操作用户功能
        //修改密码
        public int UpdatePwd(string account, string pwd, int pwdLv)
        {
            R res = Http.Post<R>(ModifyUserServerUrl, "Password", account, pwd);
            if (res != null)
                return res.res;
            return 0;
        }
        /// <summary>
        /// 设置用户信息
        /// </summary>
        /// <param name="account"></param>
        /// <param name="type">
        /// 1修改密码 2解绑手机号 3解绑安全令 4解除本机锁定 5解除安全令锁定 6冻结账号 7解冻账号</param>
        /// <returns></returns>
        public int SetUserInfo(string account, long type)
        {
            R res = new R();
            if (type == 2)
                res = Http.Post<R>(ModifyUserServerUrl, "UnbindMobile", account);
            else if (type == 3)
                res = Http.Post<R>(ModifyUserServerUrl, "UnbindIukey", account);
            else if (type == 4)
                res = Http.Post<R>(ModifyUserServerUrl, "UnlockLocal", account);
            else if (type == 5)
                res = Http.Post<R>(ModifyUserServerUrl, "UnlockIukey", account);
            else if (type == 6)
                res = Http.Post<R>(ModifyUserServerUrl, "Freeze", account, 0);
            else if (type == 7)
                res = Http.Post<R>(ModifyUserServerUrl, "Freeze", account, 1);
            return res.res;
        }
        //加钱
        public int SetUserMoney(string account, long type, long num)
        {
            R res = new R();
            if (type == 1)
                res = Http.Post<R>(ModifyUserServerUrl, "BackMoney", account, num);
            else if (type == 2)
                res = Http.Post<R>(ModifyUserServerUrl, "BackBean", account, num);
            else if (type == 3)
                res = Http.Post<R>(ModifyUserServerUrl, "BackRoomcard", account, num);
            return res.res;
        }
        #endregion


        /// <summary>
        /// 批量chargeid查询批量用户
        /// </summary>
        public ClubsRes<Dictionary<string, object>> QueryUserList(string[] member_id)
        {
            try
            {
                string param = "{\"module\":\"query\",\"func\":\"get\",\"args\":" +
                    Json.SerializeObject(new Dictionary<string, object> { { "fields",
                        new string[] { "Nickname", "Icon", "Account", "Regitime", "LastIp","GUID", "Identity", "Name" } }, { "id", member_id } }) + "}";
                var res = PostClubServer<ClubsRes<Dictionary<string, object>>>(RecordServerUrl + "do", param);
                if (res != null && res.ret == 0)
                {
                    List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
                    foreach (string uid in res.msg.Keys)
                    {
                        var r2 = res.msg[uid] as IDictionary<string, Newtonsoft.Json.Linq.JToken>;
                        if (r2 == null || r2.Count < 1) continue;
                        //cache
                        Users cacheUser = new Users()
                        {
                            Account = r2["Account"].ToString(),
                            ChargeId = uid,
                            Nickname = r2["Nickname"].ToString(),
                            Regitime = (r2["Regitime"] == null || r2["Regitime"].ToString() == "") ? 0 : int.Parse(r2["Regitime"].ToString()),
                            LastIp = r2["LastIp"].ToString(),
                            GUID = r2["GUID"].ToString(),
                            Identity = r2["Identity"].ToString(),
                            Name = r2["Name"].ToString()
                        };
                        SetCacheAccountList(cacheUser.Account, cacheUser);
                        SetCacheChargeList(uid, cacheUser);
                    }
                }
                return res;
            }
            catch (Exception ex)
            {
                WriteError("QueryUserList ex:", ex.Message, member_id);
            }
            return null;
        }
        public List<Dictionary<string, string>> GetUserInfoList(string[] accounts)
        {
            string param = "{\"module\":\"query\",\"func\":\"get\",\"args\":" + Json.SerializeObject(new Dictionary<string, object> { { "fields", new string[] { "Nickname", "ChargeId", "Account", "Regitime", "LastIp", "GUID" } }, { "accounts", accounts } }) + "}";
            var v = PostClubServer<ClubsRes<Dictionary<string, object>>>(RecordServerUrl + "do", param);
            if (v == null || v.ret != 0) return null;
            List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
            string acc = "", chargeid = "", nick = "", guid = "";
            int regTime = 0;
            foreach (string uid in v.msg.Keys)
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                var info = new Dictionary<string, object>();
                var res = v.msg[uid] as IDictionary<string, Newtonsoft.Json.Linq.JToken>;
                if (res == null || res.Count < 1) break;
                acc = res["Account"] == null ? "" : res["Account"].ToString();
                chargeid = res["ChargeId"] == null ? "" : res["ChargeId"].ToString();
                nick = res["Nickname"] == null ? "" : res["Nickname"].ToString();
                regTime = res["Regitime"] == null ? 0 : int.Parse(res["Regitime"].ToString());
                guid = res["GUID"] == null ? "" : res["GUID"].ToString();
                dic.Add("UID", uid);
                dic.Add("Account", acc);
                dic.Add("ChargeId", chargeid);
                dic.Add("Nickname", nick);
                dic.Add("Regitime", regTime.ToString());
                list.Add(dic);
                //cache
                Users cacheUser = new Users()
                {
                    Account = acc,
                    ChargeId = chargeid,
                    Nickname = nick,
                    Regitime = regTime,
                    LastIp = res["LastIp"] == null ? "" : res["LastIp"].ToString(),
                    GUID = guid
                };
                SetCacheAccountList(acc, cacheUser);
                SetCacheChargeList(chargeid, cacheUser);
            }
            return list;
        }































        public List<Dictionary<string, object>> GetMemberInfo(string[] member_id)
        {
            var v = QueryUserList(member_id);
            if (v == null || v.ret != 0) return null;
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            foreach (string uid in v.msg.Keys)
            {
                var info = new Dictionary<string, object>();
                var res = v.msg[uid] as IDictionary<string, Newtonsoft.Json.Linq.JToken>;
                if (res == null || res.Count < 1) break;
                info.Add("id", uid);
                info.Add("nickname", res["Nickname"].ToString());
                list.Add(info);
            }
            return list;
        }
        public Dictionary<string, string> GetMemberNickName(string[] member_id)
        {
            var v = QueryUserList(member_id);
            if (v == null || v.ret != 0) return null;
            Dictionary<string, string> list = new Dictionary<string, string>();
            foreach (string uid in v.msg.Keys)
            {
                var info = new Dictionary<string, object>();
                var res = v.msg[uid] as IDictionary<string, Newtonsoft.Json.Linq.JToken>;
                if (res == null || res.Count < 1) break;
                list.Add(uid, res["Nickname"].ToString());
            }
            return list;
        }



        public static void SetCacheAccountList(string account, Users cacheUser)
        {
            try
            {
                if (string.IsNullOrEmpty(account) || account == "" || cacheUser == null) return;
                account = account.ToLower();
                if (Cache.CacheAccountList != null && Cache.CacheAccountList.ContainsKey(account))
                    Cache.CacheAccountList[account] = cacheUser;
                else
                    Cache.CacheAccountList.Add(account, cacheUser);
            }
            catch (Exception ex)
            {
                Base.WriteError("SetCacheAccountList ex:", ex.Message, "account:", account);
            }
        }
        public static void SetCacheChargeList(string chargeid, Users cacheUser)
        {
            try
            {
                if (string.IsNullOrEmpty(chargeid) || chargeid == "" || cacheUser == null) return;
                chargeid = chargeid.ToUpper();
                if (Cache.CacheChargeidList != null && Cache.CacheChargeidList.ContainsKey(chargeid))
                    Cache.CacheChargeidList[chargeid] = cacheUser;
                else
                    Cache.CacheChargeidList.Add(chargeid, cacheUser);
            }
            catch (Exception ex)
            {
                Base.WriteError("SetCacheChargeList ex:", ex.Message, "chargeid:", chargeid);
            }
        }
        public Users GetCacheUserByChargeId(string chargeId)
        {
            if (string.IsNullOrEmpty(chargeId)) return null;
            chargeId = chargeId.ToUpper();
            if (Cache.CacheChargeidList != null && Cache.CacheChargeidList.Count > 0
                && Cache.CacheChargeidList.ContainsKey(chargeId))
                return Cache.CacheChargeidList[chargeId];
            int row = 0;
            List<Users> list = GetUserList("", chargeId, out row);
            if (list == null || list.Count < 1) return null;
            return list[0];
        }
        public Users GetCacheUserByChargeIdFromCache(string chargeId)
        {
            if (string.IsNullOrEmpty(chargeId)) return null;
            chargeId = chargeId.ToUpper();
            if (Cache.CacheChargeidList != null && Cache.CacheChargeidList.Count > 0
                && Cache.CacheChargeidList.ContainsKey(chargeId))
                return Cache.CacheChargeidList[chargeId];
            return null;
        }
        public Users GetCacheUserByAccountFromCache(string account)
        {
            if (string.IsNullOrEmpty(account)) return null;
            account = account.ToLower();
            if (Cache.CacheAccountList != null && Cache.CacheAccountList.Count > 0
                && Cache.CacheAccountList.ContainsKey(account))
                return Cache.CacheAccountList[account];
            return null;
        }
        public string GetAccByChargeId(string chargeId)
        {
            if (string.IsNullOrEmpty(chargeId)) return "";
            chargeId = chargeId.ToUpper();
            if (Cache.CacheChargeidList != null && Cache.CacheChargeidList.Count > 0 && Cache.CacheChargeidList.ContainsKey(chargeId))
            {
                Users cacheUser = Cache.CacheChargeidList[chargeId];
                if (cacheUser != null)
                    return cacheUser.Account;
            }
            int row = 0;
            List<Users> list = GetUserList("", chargeId, out row);
            if (list == null || list.Count < 1) return "";
            return list[0].Account;
        }
        public string GetAccByChargeIdFrom(string chargeId)
        {
            if (string.IsNullOrEmpty(chargeId)) return "";
            chargeId = chargeId.ToUpper();
            if (Cache.CacheChargeidList != null && Cache.CacheChargeidList.Count > 0 && Cache.CacheChargeidList.ContainsKey(chargeId))
            {
                Users cacheUser = Cache.CacheChargeidList[chargeId];
                if (cacheUser != null)
                    return cacheUser.Account;
            }
            return "";
        }
        public string GetChargeIdByAcc(string account)
        {
            if (string.IsNullOrEmpty(account)) return "";
            account = account.ToLower();
            if (Cache.CacheAccountList != null && Cache.CacheAccountList.Count > 0 && Cache.CacheAccountList.ContainsKey(account))
            {
                Users cacheUser = Cache.CacheAccountList[account];
                if (cacheUser != null)
                    return cacheUser.ChargeId;
            }
            int row = 0;
            List<Users> list = GetUserList(account, "", out row);
            if (list == null || list.Count < 1) return "";
            return list[0].ChargeId;
        }
        public string GetNickByAcc(string account)
        {
            if (string.IsNullOrEmpty(account)) return "";
            account = account.ToLower();
            if (Cache.CacheAccountList != null && Cache.CacheAccountList.Count > 0 && Cache.CacheAccountList.ContainsKey(account))
            {
                Users cacheUser = Cache.CacheAccountList[account];
                if (cacheUser != null)
                    return cacheUser.Nickname;
            }
            int row = 0;
            List<Users> list = GetUserList(account, "", out row);
            if (list == null || list.Count < 1) return "";
            return list[0].Nickname;
        }





        /// <summary>
        /// 批量chargeids获取批量用户
        /// </summary>
        /// <param name="chargeids">chargeid集合</param>
        /// <param name="fields">需要查询字段数组集合</param>
        /// <returns></returns>
        public ClubsRes<List<Users>> GetUsersByChargeIds(string[] chargeids)
        {
            string param = "{\"module\":\"query\",\"func\":\"get\",\"args\":" + Json.SerializeObject(new Dictionary<string, object> { { "fields",
                        new string[] {"ChargeId", "Nickname", "Icon", "Account", "Regitime", "LastIp","GUID", "Identity", "Name" } }, { "id", chargeids } }) + "}";
            return QueryUserListByServers(param);
        }
        public ClubsRes<List<Users>> GetUsersByAccounts(string[] accounts)
        {
            string param = "{\"module\":\"query\",\"func\":\"get\",\"args\":" + Json.SerializeObject(new Dictionary<string, object> { { "fields", new string[] { "Nickname", "ChargeId", "Account", "Regitime", "LastIp", "GUID" } }, { "accounts", accounts } }) + "}";
            return QueryUserListByServers(param);
        }
        public ClubsRes<List<Users>> QueryUserListByServers(string param)
        {
            try
            {
                if (string.IsNullOrEmpty(param)) return null;
                var res = PostClubServer<ClubsRes<Dictionary<string, object>>>(RecordServerUrl + "do", param);
                if (res == null || res.ret != 0 || res.msg == null || res.msg.Keys == null || res.msg.Keys.Count < 1) return null;
                List<Users> list = new List<Users>();
                foreach (string uid in res.msg.Keys)
                {
                    var r2 = res.msg[uid] as IDictionary<string, Newtonsoft.Json.Linq.JToken>;
                    if (r2 == null || r2.Count < 1) continue;
                    Users cacheUser = new Users()
                    {
                        Account = r2["Account"] == null ? "" : r2["Account"].ToString(),
                        ChargeId = r2["ChargeId"] == null ? "" : r2["ChargeId"].ToString(),
                        Nickname = r2["Nickname"] == null ? "" : r2["Nickname"].ToString(),
                        Regitime = (r2["Regitime"] == null || r2["Regitime"].ToString() == "") ? 0 : int.Parse(r2["Regitime"].ToString()),
                        LastIp = r2["LastIp"] == null ? "" : r2["LastIp"].ToString(),
                        GUID = r2["GUID"] == null ? "" : r2["GUID"].ToString(),
                        Identity = r2["Identity"] == null ? "" : r2["Identity"].ToString(),
                        Name = r2["Name"] == null ? "" : r2["Name"].ToString(),
                    };
                    SetCacheAccountList(cacheUser.Account, cacheUser);
                    SetCacheChargeList(cacheUser.ChargeId, cacheUser);
                    list.Add(cacheUser);
                }
            }
            catch (Exception ex)
            {
                WriteError("QueryUserListByServers ex:", ex.Message);
            }
            return null;
        }


    }
}
