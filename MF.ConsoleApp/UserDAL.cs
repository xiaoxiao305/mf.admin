using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MF.Protocol;
using static MF.ConsoleApp.Data;
using Newtonsoft.Json;
using MF.Common.Json;

namespace MF.ConsoleApp
{
   public class UserDAL
    {
        protected static string RecordServerUrl = ConfigurationManager.AppSettings["RecordServerURI"];

        public Users GetCacheUserByChargeIdFromCache(string chargeId)
        {
            if (string.IsNullOrEmpty(chargeId)) return null;
            chargeId = chargeId.ToUpper();
            if (Cache.CacheChargeidList != null && Cache.CacheChargeidList.Count > 0
                && Cache.CacheChargeidList.ContainsKey(chargeId))
                return Cache.CacheChargeidList[chargeId];
            return null;
        }

        public string GetAccByChargeId(string chargeId)
        {
            if (string.IsNullOrEmpty(chargeId)) return "";
            chargeId = chargeId.ToUpper();
            int row = 0;
            List<Users> list = GetUserList("", chargeId, out row);
            if (list == null || list.Count < 1) return "";
            return list[0].Account;
        }
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
                var res = PostHelper.PostRecordServer<Users>(RecordServerUrl + "get_user_list", current.ToString());
                if (res == null || res.Code < 1)
                    return null;
                rowCount = res.Code;
                return res.R;
            }
            catch (Exception ex)
            {
                Console.WriteLine("post get_user_list ex:" + ex.Message);
            }
            return null;
        }



        public List<CurrencyRecord> GetCurrcryRecord(Search<CurrencyRecord> search, long searchtype, out int rowCount)
        {
            rowCount = 0;
            try
            {
                SearchCondition<CurrencyRecord> current = SearchCondition<CurrencyRecord>.Current;
                CurrencyRecord model = search.SearchObj;
                current.AddPage(search.PageIndex, search.PageSize);
                current.Add(p => p.Account, model.Account);
                current.AddInt(p => p.GameId, model.GameId);
                current.AddInt(p => p.MatchId, model.MatchId);
                current.AddInt(p => p.RuleId, model.RuleId);
                current.AddInt(p => p.Type, model.Type);
                if (search.IsChkTime)
                    current.AddBetween(p => p.Time, search.StartTime, search.OverTime);
                string protocolname = searchtype == 1 ? "get_currency_list"
                    : searchtype == 2 ? "get_bean_list"
                    :searchtype == 3 ? "get_happycard_list"
                : searchtype == 4 ? "get_strongbox_list" : "";
                Log.WriteLog("protocolname:", protocolname);
                Console.WriteLine("protocolname:"+ protocolname);
                if (string.IsNullOrEmpty(protocolname)) return null;
                var res = PostHelper.PostRecordServer<CurrencyRecord>(RecordServerUrl + protocolname, current.ToString());
                if (res == null || res.Code < 1)
                    return null;
                rowCount = res.Code;
                return res.R;
            }
            catch (Exception ex)
            {
                Log.WriteError("post get_currency_list ex:", ex.Message);
            }
            return null;
        }
        public ClubsRes<Dictionary<string, object>> QueryUserList(string[] member_id)
        {
            string param = "{\"module\":\"query\",\"func\":\"get\",\"args\":" +
                Json.SerializeObject(new Dictionary<string, object> { { "fields",
                        new string[] { "Nickname", "Icon", "Account", "Regitime", "LastIp","GUID", "Identity", "Name","Mobile" } }, { "id", member_id } }) + "}";
            var res = PostHelper.PostClubServer<ClubsRes<Dictionary<string, object>>>(RecordServerUrl + "do", param);
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
                        Name = r2["Name"].ToString(),
                        Mobile = r2["Mobile"].ToString()
                    };
                    SetCacheAccountList(cacheUser.Account, cacheUser);
                    SetCacheChargeList(uid, cacheUser);
                }
            }
            return res;
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
    }
}
