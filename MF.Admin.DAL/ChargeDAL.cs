using System;
using System.Collections.Generic;
using MF.Data;
using MF.Common.Json;
using System.Text;
using MF.Common.Security;
using System.Net;
using System.IO;
using MF.Protocol;

namespace MF.Admin.DAL
{
    class Result
    {
        public int Res;
    }
    public class ChargeDAL : BaseDAL
    {
        public List<ChargeRecord> GetChargeRecordList(ChargeRecordSearch search, out int rowCount)
        {
            rowCount = 0;
            try
            {
                SearchCondition<ChargeRecord> current = SearchCondition<ChargeRecord>.Current;
                ChargeRecord model = search.SearchObj;
                current.AddPage(search.PageIndex, search.PageSize);
                current.Add(p => p.Account, model.Account);
                current.AddNumber(p => p.Flag, model.Flag);
                if (model.PayChannel > 0)
                {
                    if (model.PayChannel == 1 || model.PayChannel == 4)
                        current.AddBetween(p => p.PayChannel, search.StartPayChannel, search.OverPayChannel);
                    else
                        current.AddInt(p => p.PayChannel, model.PayChannel);
                }
                if (search.IsSearchKey)
                {
                    if (search.IsExact == 1)
                    {
                        current.Add(p => p.OrderNo, model.OrderNo);
                        current.Add(p => p.PlatformTransId, model.PlatformTransId);
                    }
                    else
                    {
                        current.Add(p => p.OrderNo, TOpeart.LK, model.OrderNo);
                        current.Add(p => p.PlatformTransId, TOpeart.LK, model.PlatformTransId);
                    }
                }
                if (search.IsChkTime)
                {
                    if (search.TimeType == 1)
                        current.AddBetween(p => p.CreateDate, search.StartTime, search.OverTime);
                    else if (search.TimeType == 2)
                        current.AddBetween(p => p.PayDate, search.StartTime, search.OverTime);
                }
                var res = PostRecordServer<ChargeRecord>(ChargeServerUrl + "get_chargerecord_list", current.ToString());
                if (res == null || res.Code < 1)
                    return null;
                rowCount = res.Code;
                return res.R;
            }
            catch (Exception ex)
            {
                BaseDAL.WriteError("post get_chargerecord_list ex:", ex.Message);
            }
            return null;
        }

        public int DealChargeOrder(string order, long money, long payChannel)
        {
            try
            {
                Result res = Http.PostCharge<Result>(ChargeServerUrl + "dealchargerecord", order, money, "", 1, payChannel);
                if (res != null)
                    return res.Res;
            }
            catch (Exception ex)
            {
                WriteError("DAL DealChargeOrder ex:", ex.Message);
            }
            return 0;
        }

        public int SetChargeActiveState(int status, long stime, long etime)
        {
            try
            {
                Result res = Http.PostCharge<Result>(ChargeServerUrl + "sp_setchargeactivestate", status, stime, etime);
                if (res != null)
                    return res.Res;
            }
            catch (Exception ex)
            {
                WriteError("DAL SetChargeActiveState ex:", ex.Message);
            }
            return 0;
        }

        public ChargeActive GetChargeActiveState()
        {
            try
            {
                //var res = PostRecordServer<ChargeActive>(ChargeServerUrl + "sp_getchargeactivestate", SearchCondition<ChargeRecord>.Current.ToString());
               return PostClubServer<ChargeActive>(ChargeServerUrl + "sp_getchargeactivestate", SearchCondition<ChargeRecord>.Current.ToString());
            }
            catch (Exception ex)
            {
                WriteError("DAL GetChargeActiveState ex:", ex.Message);
            }
            return null;
        }
    }
}
