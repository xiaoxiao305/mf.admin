using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MF.ConsoleApp
{
  public  class UserBLL
    {
        private static UserDAL dal = new UserDAL();
        public static List<CurrencyRecord> GetCurrcryRecord(long pageSize, long pageIndex, long gameid, long matchid, long type, string account, string chargeid, 
            long checktime, long startTime, long overTime, long searchtype, out int rowCount)
        {
            rowCount = 0;
            try
            {
                chargeid = chargeid.ToUpper();
                var search = new Search<CurrencyRecord>();
                search.PageSize = (int)pageSize;
                search.PageIndex = (int)pageIndex;
                CurrencyRecord model = new CurrencyRecord();
                if (!string.IsNullOrEmpty(account))
                    model.Account = account;
                else
                {
                    if (!string.IsNullOrEmpty(chargeid))
                        model.Account = dal.GetAccByChargeId(chargeid);
                }
                if (gameid >= 0)
                    model.GameId = gameid;
                if (matchid > 0)
                    model.MatchId = matchid;
                if (type > 0)
                    model.Type = type;
                if (checktime == 1)
                {
                    search.IsChkTime = true;
                    search.StartTime = startTime;
                    search.OverTime = overTime;
                }
                search.SearchObj = model;
                List<CurrencyRecord> searchList = dal.GetCurrcryRecord(search, searchtype, out rowCount);
                if (searchList == null || searchList.Count < 1)
                    return null;
                else
                {
                    List<CurrencyRecord> newSearchList = new List<CurrencyRecord>();
                    foreach (CurrencyRecord m in searchList)
                    {
                        if (m.Type != 30)//税
                            newSearchList.Add(m);
                    }
                    return newSearchList;
                }
            }catch(Exception ex)
            {
                Log.WriteError("bll getcurrentyrecord:", ex.Message);
            }
            return null;
        }
    }
}
