using System.Collections.Generic;
using MF.Data;
using MF.Admin.DAL;
using System;
using System.Linq;
using MF.Data.ExtendChannel;
using System.Data;

namespace MF.Admin.BLL
{
    public class Report : Base
    {
        private static string mychannel = System.Configuration.ConfigurationManager.AppSettings["MyChannel"];
        private static ReportDAL dal = new ReportDAL();
        public static List<GameReport> GetGameReport(long pageSize, long pageIndex, long gameid, long start, long over, out int rowCount)
        {
            List<GameReport> list = GetSceneReport(pageSize, pageIndex, gameid, 0, 0, start, over, out rowCount);
            if (list == null || list.Count < 1)
                return null;
            Dictionary<int, GameReport> dicgame = new Dictionary<int, GameReport>();
            foreach (GameReport report in list)
            {
                if (dicgame.ContainsKey(report.GameId))
                {
                    dicgame[report.GameId].Actives += report.Actives;
                    dicgame[report.GameId].Win += report.Win;
                    dicgame[report.GameId].Lose += report.Lose;
                    dicgame[report.GameId].Shrink += report.Shrink;
                }
                else
                    dicgame.Add(report.GameId, report);
            }
            return new List<GameReport>(dicgame.Values);
        }
        /// <summary>
        /// 获取游戏场记录
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="gameid">游戏ID</param>
        /// <param name="matchid">游戏场ID</param>
        /// <param name="ruleid">游戏ruleid【德州二人包间】</param>
        /// <param name="start">查询开始时间</param>
        /// <param name="over">查询结束时间</param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public static List<GameReport> GetSceneReport(long pageSize, long pageIndex, long gameid, long matchid, long ruleid, long start, long over, out int rowCount)
        {
            var search = new GameReportSearch() { PageIndex = (int)pageIndex, PageSize = (int)pageSize, PrimaryKey = "[Day]" };
            if (gameid > 0)
                search.Where += string.Format(" and GameId={0}", gameid);
            if (matchid > 0)
                search.Where += string.Format(" and MatchId={0}", matchid);
            if (ruleid > 0)
                search.Where += string.Format(" and RuleId={0}", ruleid);
            if (start > 0 && over > 0)
                search.Where += string.Format(" and [Day] between {0} and {1}", start, over);
            return dal.GetGameReport(search, out rowCount);
        }
        public static List<ExtendChannel> GetExtendChannelRecord(long pageSize, long pageIndex,
            string channel, string channelnum, long checktime, long startTime, long overTime, out int rowCount)
        {
            rowCount = 0;
            if (checktime == 2)//redis
            {
                try
                {
                    //WriteLog("checktime:",checktime.ToString());
                    List <ExtendChannel> list = RedisManager.ChannelRedis.CountRedisData(DateTime.Now);//统计今日及时数据
                    if (list == null)
                        return list;
                    if (string.IsNullOrEmpty(channel) || channel.Trim().Equals("-1"))
                    {
                        if (list != null)
                            rowCount = list.Count;
                        return list;
                    }
                    List<ExtendChannel> new_list = new List<ExtendChannel>();
                    foreach (ExtendChannel model in list)
                    {
                        if (channel.Trim().ToUpper().Equals(model.Channel.Trim().ToUpper()))
                        {
                            new_list.Add(model);
                        }
                    }
                    if (new_list != null)
                        rowCount = new_list.Count;
                    return new_list;
                }
                catch (Exception ex)
                {
                    WriteError("get today redisdata ex:", ex.Message);
                    return null;
                }
            }
            else//mf_record
            {
                var search = new ExtendChannelSearch() { PageIndex = (int)pageIndex, PageSize = (int)pageSize, PrimaryKey = "[Day]" };
                if (startTime > 0 && overTime > 0)
                    search.Where += string.Format(" and [Day] between {0} and {1}", startTime, overTime);
                if (!string.IsNullOrEmpty(channel))
                    search.Where += string.Format(" and [Channel]='{0}'", channel);
                if (!string.IsNullOrEmpty(channelnum))
                    search.Where += string.Format(" and [ChannelNum]='{0}'", channelnum);
                return dal.GetExtendChannelRecord(search, out rowCount);
            }
        }
        public static List<ExtendChannel> GetExtendChannelKeywordRecord(long pageSize, long pageIndex,
          long checktime, long startTime, out int rowCount)
        {
            rowCount = 0;
            try
            {
                DateTime time = DateTime.Now;
                if (checktime == 1 && startTime > 0)
                    time = BaseDAL.ConvertSpanToDate("d", (int)startTime);
                List<ExtendChannel> list = RedisManager.ChannelRedis.CountRedisKeywordData(time);//统计今日及时数据
                if (list != null)
                    rowCount = list.Count;
                return list;
            }
            catch (Exception ex)
            {
                WriteError("get today rediskeyworddata ex:", ex.Message);
                return null;
            }
        }

        public static List<ExtendKeywords> GetGameKeywordsList(out int rowCount)
        {
            rowCount = 0;
            List<ExtendKeywords> list = KeywordsBLL.GetKeywordsList();
            if (list != null && list.Count > 0)
                rowCount = list.Count;
            return list;
        }

        public static object GetReportList(string method, int pageSize, int pageIndex, int start, int over, string channel, out int rowCount)
        {
            Base.WriteLog("GetReportList m:", method, " ps:", pageSize.ToString(), " pz:", pageIndex.ToString(), " start:", start.ToString(), " over:", over.ToString(), " channel:", channel);
            rowCount = 0;
            try
            {
                switch (method)
                {
                    case "regist":
                        var search = new RegistReportSearch() { PageIndex = pageIndex, PageSize = pageSize, Where = string.Format("[Day] between {0} and {1}", start, over) };
                        if (!string.IsNullOrEmpty(channel) && channel != "-1")
                            search.Where += string.Format(" AND ChannelId = '{0}'", channel);
                        return dal.GetRegistReport(search, out rowCount);
                    case "charge":
                        var charge = new ChargeReportSearch() { PageIndex = pageIndex, PageSize = pageSize, PrimaryKey = "[Day]", Where = string.Format("[Day] between {0} and {1}", start, over) };
                        if (!string.IsNullOrEmpty(channel) && channel != "-1")
                            charge.Where += string.Format(" AND (ChannelId = '{1}' OR (ChannelId = '{0}' AND DAY IN (SELECT DAY FROM NewChargeReport WHERE 1=1 AND ChannelId = '{1}')))", mychannel, channel);
                        return GetChargeReport(charge, out rowCount);
                    case "currency":
                        var currency = new CurrencyReportSearch() { PageIndex = pageIndex, PageSize = pageSize, PrimaryKey = "[Day]", Where = string.Format("[Day] between {0} and {1}", start, over) };
                        if (!string.IsNullOrEmpty(channel) && channel != "-1")
                            currency.Where += string.Format(" AND ChannelId = '{0}'", channel);
                        return dal.GetCurrencyReport(currency, out rowCount);
                    case "bean":
                        var bean = new BeanReportSearch() { PageIndex = pageIndex, PageSize = pageSize, PrimaryKey = "[Day]", Where = string.Format("[Day] between {0} and {1}", start, over) };
                        if (!string.IsNullOrEmpty(channel) && channel != "-1")
                            bean.Where += string.Format(" AND ChannelId = '{0}'", channel);
                        return dal.GetBeanReport(bean, out rowCount);
                    case "promot":

                        return GetPromotReport(pageSize, pageIndex, start, over, channel, out rowCount);
                    case "happycard":
                        var happyrecord = new HappyRecordReportSearch() { PageIndex = pageIndex, PageSize = pageSize, PrimaryKey = "[Day]", Where = string.Format("[Day] between {0} and {1}", start, over) };
                        if (!string.IsNullOrEmpty(channel) && channel != "-1")
                            happyrecord.Where += string.Format(" AND ChannelId = '{0}'", channel);
                        return dal.GetHappyRecordReport(happyrecord, out rowCount);
                    case "zywystatic":
                        return dal.GetZywyStaticReport(start, over, channel);
                }
            }catch(Exception ex)
            {
                Base.WriteError("report ex:", ex.Message, "m:", method, " ps:", pageSize.ToString(), " pz:", pageIndex.ToString(), " start:", start.ToString(), " over:", over.ToString(), " channel:", channel);
            }
            return null;
        }
        public static List<NewAdReport> GetPromotReport(int pageSize, int pageIndex, int start, int over, string channel, out int rowCount)
        {
            var promot = new PromotReportSearch() { PageIndex = pageIndex, PageSize = pageSize, PrimaryKey = "[Day]", Where = string.Format("[Day] between {0} and {1}", start, over) };
            if (!string.IsNullOrEmpty(channel) && channel != "-1")
                promot.Where += string.Format(" AND ChannelId = '{0}'", channel);
            var dt = BaseDAL.GetSearchData(promot, DBName.MF_RECORD_DY, out rowCount);
            if (rowCount < 1 || dt==null || dt.Rows ==null || dt.Rows.Count <1)
                return null;
            var currency = new CurrencyReportSearch() { PageIndex = 1, PageSize = 10000, PrimaryKey = "[Day]", Where = string.Format("[Day] between {0} and {1}", start, over) };
            if (!string.IsNullOrEmpty(channel) && channel != "-1")
                currency.Where += string.Format(" AND ChannelId = '{0}'", channel);
            int rowCount2 = 0;
            var dt2 = BaseDAL.GetSearchData(currency, DBName.MF_RECORD_DY, out rowCount2);
            var items = new List<NewAdReport>();
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    var model=dal.GetAdReportModel(dr);
                    if (model == null) continue;
                    if (rowCount2 > 0 && dt2 !=null && dt2.Rows !=null)
                    {
                        foreach (DataRow dr2 in dt2.Rows)
                        {
                            var cm = dal.GetCurrencyModel(dr2);
                            if (cm == null || cm.Day != model.Day) continue;
                            model.TakeCurrency = cm.TakeCurrency;
                        }                     
                    }
                    items.Add(model);
                }
            }
            catch (Exception ex)
            {
                WriteError("ReportDAL.GetPromotReport Error:", ex.Message);
                throw ex;
            }
            return items;
        }
        public static List<NewQmallReportCount> GetExchangeReport(long pageSize, long pageIndex, long product, long checktime, long startTime, long overTime, string channel, out int rowCount)
        {
            rowCount = 0;
            var search = new NewQmallReportSearch() { PageIndex = (int)pageIndex, PageSize = (int)pageSize, PrimaryKey = "[Day]" };
            if (product > 0)
                search.Where += " AND Product = " + product;
            if (checktime == 1 && startTime > 0 && overTime > 0)
                search.Where += string.Format(" and [Day] between {0} and {1}", startTime, overTime);
            if (!string.IsNullOrEmpty(channel) && channel != "-1")
                search.Where += string.Format(" AND ChannelId = '{0}'", channel);
            List<NewQmallReport> listtemp = dal.GetExchangeReport(search, out rowCount);
            if (listtemp == null || listtemp.Count < 1)
                return null;
            NewQmallReportCount model = null;
            Dictionary<int, NewQmallReportCount> dic = new Dictionary<int, NewQmallReportCount>();
            foreach (NewQmallReport temp in listtemp)
            {
                if (dic.ContainsKey(temp.Day))
                {
                    model = dic[temp.Day];
                    model = SetNewQmall(temp.Product, temp.SellNum, model);
                    dic[temp.Day] = model;
                }
                else
                {
                    model = new NewQmallReportCount();
                    model.Day = temp.Day;
                    model.ChannelId = temp.ChannelId;
                    model = SetNewQmall(temp.Product, temp.SellNum, model);
                    dic.Add(temp.Day, model);
                }
            }
            List<NewQmallReportCount> list = dic.Values.ToList<NewQmallReportCount>();
            rowCount = list.Count;
            return list;
        }
        public static List<NewADIDReport> GetAdidRegReport(long pageSize, long pageIndex, long channel, long device, long gamelist, long checktime, long startTime, long overTime, out int rowCount)
        {
            var search = new NewADIDReportSearch() { PageIndex = (int)pageIndex, PageSize = (int)pageSize, PrimaryKey = "[Day]" };
            string adid = "";
            if (channel > 0)
                adid += channel;
            if (device > 0)
                adid += device;
            if (gamelist > 0)
                adid += gamelist;
            if (!string.IsNullOrEmpty(adid))
                search.Where += string.Format(" AND ADID LIKE '{0}%'", adid);
            if (checktime == 1 && startTime > 0 && overTime > 0)
                search.Where += string.Format(" and [Day] between {0} and {1}", startTime, overTime);
            return dal.GetAdidRegReport(search, out rowCount);
        }
        public static List<NewBaiduAdReport> GetBaiduADReport(long pageSize, long pageIndex, long channel, long device, long gamelist, long checktime, long startTime, long overTime, out int rowCount)
        {
            var search = new NewBaiduAdReportSearch() { PageIndex = (int)pageIndex, PageSize = (int)pageSize, PrimaryKey = "[Day]" };
            if (channel > 0)
                search.Where += string.Format(" AND SUBSTRING (ADID,1, 2)  = '{0}'", channel);
            if (device > 0)
                search.Where += string.Format(" AND SUBSTRING(ADID,3, 1) = '{0}'", device);
            if (gamelist > 0)
                search.Where += string.Format(" AND SUBSTRING(ADID,4, 2) = '{0}'", gamelist);
            if (checktime == 1 && startTime > 0 && overTime > 0)
                search.Where += string.Format(" and [Day] between {0} and {1}", startTime, overTime);
            return dal.GetBaiduADReport(search, out rowCount);
        }
        public static List<NewPromotChargeReport> GetPromotChargeReportList(int pageSize, int pageIndex, string channel, int start, int over, out int rowCount)
        {
            var promotcharge = new PromotReportSearch() { PageIndex = pageIndex, PageSize = pageSize, PrimaryKey = "[Day]", Where = string.Format("[Day] between {0} and {1}", start, over) };
            var chargesearch = new ChargeReportSearch() { PageIndex = pageIndex, PageSize = pageSize, PrimaryKey = "[Day]", Where = string.Format("[Day] between {0} and {1}", start, over) };
            //-1 所有渠道 -2单个渠道登录获取channelid有误
            if (string.IsNullOrEmpty(channel) || channel == "-2")
            {
                rowCount = 0;
                return null;
            }
            if (!string.IsNullOrEmpty(channel) && channel != "-1")
            {
                promotcharge.Where += string.Format(" AND ChannelId = '{0}'", channel);
                chargesearch.Where += string.Format(" AND ChannelId = '{0}'", channel);
            }
            Dictionary<string, NewPromotChargeReport> dic = new Dictionary<string, NewPromotChargeReport>();
            string dickey;
            //promot
            List<NewAdReport> promotlist = dal.GetPromotReport(promotcharge, out rowCount);
            if (promotlist != null && promotlist.Count > 0)
            {
                foreach (NewAdReport promotmodel in promotlist)
                {
                    dickey = promotmodel.ChannelId + "_" + promotmodel.Day.ToString();
                    if (string.IsNullOrEmpty(promotmodel.ChannelId) || dic.ContainsKey(dickey))
                        continue;
                    NewPromotChargeReport m = new NewPromotChargeReport()
                    {
                        Day = promotmodel.Day,
                        LoginNum = promotmodel.LoginNum,
                        MaxNum = promotmodel.MaxNum,
                        ActUserNum = promotmodel.ActUserNum,
                        RegNum = promotmodel.RegNum,
                        OneDayLeft = promotmodel._1Day,
                        ThreeDayLeft = promotmodel._3Day,
                        SevenDayLeft = promotmodel._7Day,
                        ARPU = promotmodel.ARPU,
                        PayRate = promotmodel.PayRate,
                        ARPPU = promotmodel.ARPPU,
                        ChargeUserRate = promotmodel.ChargeUserRate,
                        ChannelId = promotmodel.ChannelId
                    };
                    dic.Add(dickey, m);
                }
            }
            //charge
            List<NewChargeReport> chargelist = dal.GetChargeReport(chargesearch, out rowCount);
            if (chargelist != null && chargelist.Count > 0)
            {
                foreach (NewChargeReport chargemodel in chargelist)
                {
                    dickey = chargemodel.ChannelId + "_" + chargemodel.Day.ToString();
                    if (string.IsNullOrEmpty(chargemodel.ChannelId))
                        continue;
                    if (dic.ContainsKey(dickey))
                    {
                        dic[dickey].SubmitNum += chargemodel.SubmitNum;
                        dic[dickey].SubmitMoney += chargemodel.SubmitMoney;
                        dic[dickey].PayNum += chargemodel.PayNum;
                        dic[dickey].PayMoney += chargemodel.PayMoney;
                    }
                    else
                    {
                        NewPromotChargeReport m2 = new NewPromotChargeReport()
                        {
                            Day = chargemodel.Day,
                            SubmitNum = chargemodel.SubmitNum,
                            SubmitMoney = chargemodel.SubmitMoney,
                            PayNum = chargemodel.PayNum,
                            PayMoney = chargemodel.PayMoney,
                            ChannelId = chargemodel.ChannelId
                        };
                        dic.Add(dickey, m2);
                    }
                }
            }
            if (dic.Values != null && dic.Values.Count > 0)
            {
                rowCount = dic.Keys.Count;
                return dic.Values.ToList<NewPromotChargeReport>();
            }
            else
                return null;
        }
        private static NewQmallReportCount SetNewQmall(long Product, long SellNum, NewQmallReportCount model)
        {
            int[] productval = new int[] { 0, 100000, 200000, 500000, 1000000, 50000, 100000, 200000, 500000, 1000000, 2000000, 20000, 60000, 100000, 500000, 1000000, 2000000, 5000000, 8000000 };
            long val = SellNum * productval[Product];
            switch (Product)
            {
                case 1:
                    model.TenMobile = model.TenMobile.HasValue ? model.TenMobile.Value + val : val;
                    break;
                case 2:
                    model.TwentyMobile = model.TwentyMobile.HasValue ? model.TwentyMobile.Value + val : val;
                    break;
                case 3:
                    model.FiftyMobile = model.FiftyMobile.HasValue ? model.FiftyMobile.Value + val : val;
                    break;
                case 4:
                    model.HundredMobile = model.HundredMobile.HasValue ? model.HundredMobile.Value + val : val;
                    break;
                case 5:
                    model.FiveCurrency = model.FiveCurrency.HasValue ? model.FiveCurrency.Value + val : val;
                    break;
                case 6:
                    model.TenCurrency = model.TenCurrency.HasValue ? model.TenCurrency.Value + val : val;
                    break;
                case 7:
                    model.TwentyCurrency = model.TwentyCurrency.HasValue ? model.TwentyCurrency.Value + val : val;
                    break;
                case 8:
                    model.FiftyCurrency = model.FiftyCurrency.HasValue ? model.FiftyCurrency.Value + val : val;
                    break;
                case 9:
                    model.HundredCurrency = model.HundredCurrency.HasValue ? model.HundredCurrency.Value + val : val;
                    break;
                case 10:
                    model.TwoHundredCurrency = model.TwoHundredCurrency.HasValue ? model.TwoHundredCurrency.Value + val : val;
                    break;
                case 11:
                    model.TwentyRoomCard = model.TwentyRoomCard.HasValue ? model.TwentyRoomCard.Value + val : val;
                    break;
                case 12:
                    model.HundredRoomCard = model.HundredRoomCard.HasValue ? model.HundredRoomCard.Value + val : val;
                    break;
                case 13:
                    model.TenJingdong = model.TenJingdong.HasValue ? model.TenJingdong.Value + val : val;
                    break;
                case 14:
                    model.FiftyJingdong = model.FiftyJingdong.HasValue ? model.FiftyJingdong.Value + val : val;
                    break;
                case 15:
                    model.HundredJingdong = model.HundredJingdong.HasValue ? model.HundredJingdong.Value + val : val;
                    break;
                case 16:
                    model.TwoHundredJingdong = model.TwoHundredJingdong.HasValue ? model.TwoHundredJingdong.Value + val : val;
                    break;
                case 17:
                    model.FiveHundredJingdong = model.FiveHundredJingdong.HasValue ? model.FiveHundredJingdong.Value + val : val;
                    break;
                case 18:
                    model.EightHundredJingdong = model.EightHundredJingdong.HasValue ? model.EightHundredJingdong.Value + val : val;
                    break;
            }
            return model;
        }
        private static List<NewChannelChargeReport> GetChargeReport(ChargeReportSearch charge, out int rowCount)
        {
            List<NewChargeReport> list = dal.GetChargeReport(charge, out rowCount);
            if (rowCount < 1)
                return null;
            Dictionary<int, NewChannelChargeReport> dic = new Dictionary<int, NewChannelChargeReport>();
            NewChannelChargeReport reportmodel = null;
            string channelbefore = "";
            foreach (NewChargeReport model in list)
            {
                if (string.IsNullOrEmpty(model.ChannelId))
                    continue;
                channelbefore = model.ChannelId.Trim().ToUpper().Substring(0, 3);
                if (dic.ContainsKey(model.Day))
                {
                    reportmodel = dic[model.Day];
                    if (channelbefore.Equals(mychannel.ToUpper())) //2255
                    {
                        reportmodel.PayNum += model.PayNum;
                        reportmodel.PayMoney += model.PayMoney;
                        reportmodel.SubmitNum += model.SubmitNum;
                        reportmodel.SubmitMoney += model.SubmitMoney;
                        if (model.PayChannel == 10 || model.PayChannel == 11)//支付宝
                            reportmodel.AlipayPayMoney += model.PayMoney;
                        else if (model.PayChannel == 40 || model.PayChannel == 41)//微信
                            reportmodel.WeixinPayMoney += model.PayMoney;
                        else if (model.PayChannel == 3)//苹果
                            reportmodel.IosPayMoney += model.PayMoney;
                    }
                    //else//channel
                    //{
                    //    reportmodel.ChannelPayNum += model.PayNum;
                    //    reportmodel.ChannelPayMoney += model.PayMoney;
                    //    reportmodel.ChannelSubmitNum += model.SubmitNum;
                    //    reportmodel.ChannelSubmitMoney += model.SubmitMoney;
                    //    reportmodel.ChannelId = model.ChannelId;
                    //}
                    dic[model.Day] = reportmodel;
                }
                else
                {
                    if (channelbefore.Equals(mychannel.ToUpper()))//2255
                    {
                        reportmodel = new NewChannelChargeReport()
                        {
                            Day = model.Day,
                            PayNum = model.PayNum,
                            PayMoney = model.PayMoney,
                            SubmitNum = model.SubmitNum,
                            SubmitMoney = model.SubmitMoney,
                            AlipayPayMoney = (model.PayChannel == 10 || model.PayChannel == 11)?model.PayMoney:0,
                            WeixinPayMoney = (model.PayChannel == 40 || model.PayChannel == 41)?model.PayMoney:0,
                            IosPayMoney = (model.PayChannel == 3)? model.PayMoney:0
                    };
                    }
                    //else//channel
                    //{
                    //    reportmodel = new NewChannelChargeReport()
                    //    {
                    //        Day = model.Day,
                    //        ChannelPayNum = model.PayNum,
                    //        ChannelPayMoney = model.PayMoney,
                    //        ChannelSubmitNum = model.SubmitNum,
                    //        ChannelSubmitMoney = model.SubmitMoney,
                    //        ChannelId = model.ChannelId
                    //    };
                    //}
                    dic.Add(model.Day, reportmodel);
                }
            }
            rowCount = dic.Count;
            return new List<NewChannelChargeReport>(dic.Values);
        }
    }
}
