using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MF.Protocol;
using static MF.ConsoleApp.Data;

namespace MF.ConsoleApp
{
   public class UserDAL
    {
        protected static string RecordServerUrl = ConfigurationManager.AppSettings["RecordServerURI"];
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
    }
}
