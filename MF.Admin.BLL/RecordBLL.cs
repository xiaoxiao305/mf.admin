using System;
using System.Collections.Generic;
using System.Text;
using MF.Admin.DAL;
using MF.Data;

namespace MF.Admin.BLL
{
    public class RecordBLL : Base
    {
        private static RecordDAL dal = new RecordDAL();
        public static List<CurrencyRecord> GetCurrcryRecord(long pageSize, long pageIndex, long gameid, long matchid, long type, string account, string chargeid, long checktime, long startTime, long overTime, long searchtype, out int rowCount)
        {
            chargeid = chargeid.ToUpper();
            rowCount = 0;
            var search = new Search<CurrencyRecord>();
            search.PageSize = (int)pageSize;
            search.PageIndex = (int)pageIndex;
            CurrencyRecord model = new CurrencyRecord();
            if (!string.IsNullOrEmpty(account))
                model.Account = account;
            else
            {
                if (!string.IsNullOrEmpty(chargeid))
                    model.Account = new UserDAL().GetAccByChargeId(chargeid);
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
        }
        public static List<BeanRecord> GetUserBeanRecord(long pageSize, long pageIndex, long gameid, long matchid, long type, string account, long checktime, long startTime, long overTime, out int rowCount)
        {
            rowCount = 0;
            if (string.IsNullOrEmpty(account))
                return null;
            var search = new Search<BeanRecord>();
            search.PageSize = (int)pageSize;
            search.PageIndex = (int)pageIndex;
            BeanRecord model = new BeanRecord();
            model.Account = account;
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
            return dal.GetUserBeanRecord(search, out rowCount);
        }
        public static List<CurrencyRecord> GetStrongBoxRecord(long pageSize, long pageIndex, long type, string account, long checktime, long startTime, long overTime, out int rowCount)
        {
            rowCount = 0;
            if (string.IsNullOrEmpty(account))
                return null;
            var search = new Search<CurrencyRecord>();
            search.PageSize = (int)pageSize;
            search.PageIndex = (int)pageIndex;
            CurrencyRecord model = new CurrencyRecord();
            model.Account = account;
            if (type > 0)
                model.Type = type;
            if (checktime == 1)
            {
                search.IsChkTime = true;
                search.StartTime = startTime;
                search.OverTime = overTime;
            }
            search.SearchObj = model;
            return dal.GetStrongBoxRecord(search, out rowCount);
        }
        public static List<CurrencyRecord> GetRoomCardRecord(long pageSize, long pageIndex, string account, long startTime, long overTime, out int rowCount)
        {
            rowCount = 0;
            if (string.IsNullOrEmpty(account))
                return null;
            var search = new Search<CurrencyRecord>();
            search.PageSize = (int)pageSize;
            search.PageIndex = (int)pageIndex;
            CurrencyRecord model = new CurrencyRecord();
            model.Account = account;
            model.Type = 24;
            search.IsChkTime = true;
            search.StartTime = startTime;
            search.OverTime = overTime;
            search.SearchObj = model;
            return dal.GetCurrcryRecord(search, 1, out rowCount);
        }
        public static List<CurrencyRecord> GetDZCurrencyRecord(long pageSize, long pageIndex, string account, long startTime, long overTime, out int rowCount)
        {
            rowCount = 0;
            if (string.IsNullOrEmpty(account))
                return null;
            var search = new Search<CurrencyRecord>();
            search.PageSize = (int)pageSize;
            search.PageIndex = (int)pageIndex;
            CurrencyRecord model = new CurrencyRecord();
            model.Account = account;
            model.GameId = 52;//德州
            model.MatchId = 95261;//包间
            model.RuleId = 1;//2人
            search.IsChkTime = true;
            search.StartTime = startTime;
            search.OverTime = overTime;
            search.SearchObj = model;
            return dal.GetCurrcryRecord(search, 1, out rowCount);
        }
        public static List<QmallRecord> GetQmallRecord(long pageSize, long pageIndex, string account,
            long type, long flag, out int rowCount)
        {
            rowCount = 0;
            if (string.IsNullOrEmpty(account))
                return null;
            var search = new Search<QmallRecordSearch>();
            search.PageSize = (int)pageSize;
            search.PageIndex = (int)pageIndex;
            QmallRecordSearch model = new QmallRecordSearch();
            model.Account = account;
            object[] types = null;
            if (type > 0)
            {
                if (type == 100)
                    types = new object[] { 5, 6, 7, 8, 9, 10 };
                else
                    types = new object[] { 1, 2, 3, 4, 11, 12, 13, 14, 15, 16, 17, 18 };
                model.Product_Id = types;
            }
            model.status = -1;
            if (flag != -1)
                model.status = flag;
            search.SearchObj = model;
            return dal.GetQmallRecord(search, out rowCount);
        }
        public static List<SystemLog> GetSystemlogRecord(long pageSize, long pageIndex, long type, long flag,
             long exact, long filed, string keyword, long checktime, long startTime, long overTime, out int rowCount)
        {
            rowCount = 0;
            SystemLogSearch search = new SystemLogSearch();
            if (type > 0)
                search.Where += " AND [TYPE] = " + type;
            if (flag >= 0)
                search.Where += " AND OprState =" + flag;
            if (keyword.Trim() != "")
            {
                if (exact == 1)
                {
                    if (filed == 1)
                        search.Where += string.Format(" and Account='{0}'", keyword);
                    else if (filed == 2)
                        search.Where += string.Format(" and IP='{0}'", keyword);
                }
                else
                {
                    if (filed == 1)
                        search.Where += string.Format(" and Account LIKE '%{0}%'", keyword);
                    else if (filed == 2)
                        search.Where += string.Format(" and IP LIKE '%{0}%'", keyword);
                }
            }
            if (checktime == 1 && overTime > startTime)
                search.Where += string.Format(" AND OperTime BETWEEN '{0}' AND '{1}'", BaseDAL.ConvertSpanToDate("s", (int)startTime), BaseDAL.ConvertSpanToDate("s", (int)overTime));
            return dal.GetSystemlogRecord(search, out rowCount);
        }
        public static List<LoginLog> GetLoginLogRecord(long pageSize, long pageIndex, long flag,
            long exact, long filed, string keyword, long checktime, long startTime, long overTime, out int rowCount)
        {
            rowCount = 0;
            LoginLogSearch search = new LoginLogSearch();
            if (flag >= 0)
                search.Where += " AND LoginState =" + flag;
            if (keyword.Trim() != "")
            {
                if (exact == 1)
                {
                    if (filed == 1)
                        search.Where += string.Format(" and Account='{0}'", keyword);
                    else if (filed == 2)
                        search.Where += string.Format(" and IP='{0}'", keyword);
                }
                else
                {
                    if (filed == 1)
                        search.Where += string.Format(" and Account LIKE '%{0}%'", keyword);
                    else if (filed == 2)
                        search.Where += string.Format(" and IP LIKE '%{0}%'", keyword);
                }
            }
            if (checktime == 1 && overTime > startTime)
                search.Where += string.Format(" AND LoginTime BETWEEN '{0}' AND '{1}'", DateTime.Parse("2012-10-01").AddSeconds(startTime), DateTime.Parse("2012-10-01").AddSeconds(overTime));
            return dal.GetLoginLogRecord(search, out rowCount);
        }

    }
}
