using System;
using System.Collections.Generic;
using MF.Data;
using MF.Admin.DAL;
namespace MF.Admin.BLL
{
    public class ChargeBLL : Base
    {
        private static ChargeDAL dal = new ChargeDAL();
        public static List<ChargeRecord> GetChargeRecordList(long pageSize, long pageIndex,
            string account, long flag, long channel, long exact, long filed, string key, long checktime,
            long timeType, long startTime, long overTime, out int rowCount)
        {
            rowCount = 0;
            var search = new ChargeRecordSearch();
            search.PageSize = (int)pageSize;
            search.PageIndex = (int)pageIndex;
            ChargeRecord model = new ChargeRecord();
            if (!string.IsNullOrEmpty(account))
                model.Account = account;
            model.Flag = flag;//-1为查询全部
            if (channel > 0)
            {
                model.PayChannel = channel;
                if (channel == 1)
                {
                    search.StartPayChannel = 10;
                    search.OverPayChannel = 11;
                }
                else if (channel == 4)
                {
                    search.StartPayChannel = 40;
                    search.OverPayChannel = 41;
                }
            }
            if (!string.IsNullOrEmpty(key.Trim()))
            {
                search.IsSearchKey = true;
                search.IsExact = exact;
                if (filed == 1)
                    model.OrderNo = key;
                else if (filed == 2)
                    model.PlatformTransId = key;
            }
            if (checktime == 1)
            {
                search.IsChkTime = true;
                search.TimeType = timeType;
                search.StartTime = startTime;
                search.OverTime = overTime;
            }
            search.SearchObj = model;
            return dal.GetChargeRecordList(search, out rowCount);
        }
        public static int DealChargeOrder(string order, long money, long payChannel)
        {
            return dal.DealChargeOrder(order, money, payChannel);
        }
        public static int SetChargeActiveState(int status, long stime, long etime)
        {
            if (status < 1 || (status == 1 && ((stime < 1 && etime < 1) || (etime < stime))))
                return -2001;
            return dal.SetChargeActiveState(status, stime, etime);
        }
        public static ChargeActive GetChargeActiveState()
        {
            // 0尚未开启 1活动进行中 2关闭活动  3活动尚未生效 4活动已结束
            ChargeActive ca = dal.GetChargeActiveState();
            if (ca == null)
                return ca;
            int nowSpan = ConvertDateToSpan(DateTime.Now, "s");
            if (ca.Start == -2 && ca.End == -2)
                ca.Res = 0;
            else if (nowSpan >= ca.Start && nowSpan <= ca.End)
                ca.Res = 1;
            else if (ca.Start == 0 && ca.End == 0)
                ca.Res = 2;
            else if (nowSpan < ca.Start)
                ca.Res = 3;
            else if (nowSpan > ca.End)
                ca.Res = 4;
            if (ca.Start > 0 && ca.End > 0)
            {
                ca.StartTime = ConvertSpanToDate("s", ca.Start).ToString("yyyy-MM-dd");
                ca.EndTime = ConvertSpanToDate("s", ca.End).ToString("yyyy-MM-dd");
            }
            return ca;
        }
    }
}
