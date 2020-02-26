using System;
using MF.Data;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using MF.Data.ExtendChannel;
using System.Configuration;
using System.Net;
using System.IO;
using System.Text;
using MF.Common.Security;
using Newtonsoft.Json;
using System.Globalization;
using System.Web.Security;
using System.Web.SessionState;

namespace MF.Admin.DAL
{
    public class ReportDAL : BaseDAL
    {
        public ReportDAL()
        {
            BaseDAL.dbname = DBName.MF_RECORD_DY;
        }
        public List<GameReport> GetGameReport(GameReportSearch search, out int rowCount)
        {
            var dt = GetSearchData(search, DBName.MF_RECORD_DY, out rowCount);
            if (rowCount < 1)
                return null;
            var list = new List<GameReport>();
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    var report = new GameReport();
                    if (dr["Actives"] != null && !string.IsNullOrEmpty(dr["Actives"].ToString()))
                        report.Actives = int.Parse(dr["Actives"].ToString());
                    if (dr["Day"] != null && !string.IsNullOrEmpty(dr["Day"].ToString()))
                        report.Day = int.Parse(dr["Day"].ToString());
                    if (dr["GameId"] != null && !string.IsNullOrEmpty(dr["GameId"].ToString()))
                        report.GameId = int.Parse(dr["GameId"].ToString());
                    if (dr["MatchId"] != null && !string.IsNullOrEmpty(dr["MatchId"].ToString()))
                        report.MatchId = int.Parse(dr["MatchId"].ToString());
                    if (dr["Shrink"] != null && !string.IsNullOrEmpty(dr["Shrink"].ToString()))
                        report.Shrink = long.Parse(dr["Shrink"].ToString());
                    if (dr["Win"] != null && !string.IsNullOrEmpty(dr["Win"].ToString()))
                        report.Win = long.Parse(dr["Win"].ToString());
                    if (dr["Lose"] != null && !string.IsNullOrEmpty(dr["Lose"].ToString()))
                        report.Lose = long.Parse(dr["Lose"].ToString());
                    if (dr["RuleId"] != null && !string.IsNullOrEmpty(dr["RuleId"].ToString()))
                        report.RuleId = long.Parse(dr["RuleId"].ToString());
                    if (dr["ChannelId"] != null && !string.IsNullOrEmpty(dr["ChannelId"].ToString()))
                        report.ChannelId = dr["ChannelId"].ToString();
                    //report.Win = report.Win - report.Lose;//前台需要单独统计win、lose
                    list.Add(report);
                }
            }
            catch (Exception ex)
            {
                WriteError("ReportDAL.GetGameReport Error:", ex.Message, ex.StackTrace, ex.Source);
            }
            return list;
        }
        public List<NewRegReport> GetRegistReport(RegistReportSearch search, out int rowCount)
        {
            var dt = BaseDAL.GetSearchData(search, DBName.MF_RECORD_DY, out rowCount);
            if (rowCount < 1)
                return null;
            var list = new List<NewRegReport>();
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    var report = new NewRegReport();
                    if (dr["AccTotal"] != null && !string.IsNullOrEmpty(dr["AccTotal"].ToString()))
                        report.AccTotal = int.Parse(dr["AccTotal"].ToString());
                    if (dr["AndroidUser"] != null && !string.IsNullOrEmpty(dr["AndroidUser"].ToString()))
                        report.AndroidUser = int.Parse(dr["AndroidUser"].ToString());
                    if (dr["ClientUser"] != null && !string.IsNullOrEmpty(dr["ClientUser"].ToString()))
                        report.ClientUser = int.Parse(dr["ClientUser"].ToString());
                    if (dr["Day"] != null && !string.IsNullOrEmpty(dr["Day"].ToString()))
                        report.Day = int.Parse(dr["Day"].ToString());
                    if (dr["IpadUser"] != null && !string.IsNullOrEmpty(dr["IpadUser"].ToString()))
                        report.IpadUser = int.Parse(dr["IpadUser"].ToString());
                    if (dr["IphoneUser"] != null && !string.IsNullOrEmpty(dr["IphoneUser"].ToString()))
                        report.IphoneUser = int.Parse(dr["IphoneUser"].ToString());
                    if (dr["NewUser"] != null && !string.IsNullOrEmpty(dr["NewUser"].ToString()))
                        report.NewUser = int.Parse(dr["NewUser"].ToString());
                    if (dr["NewVisitor"] != null && !string.IsNullOrEmpty(dr["NewVisitor"].ToString()))
                        report.NewVisitor = int.Parse(dr["NewVisitor"].ToString());
                    if (dr["Relief"] != null && !string.IsNullOrEmpty(dr["Relief"].ToString()))
                        report.Relief = int.Parse(dr["Relief"].ToString());
                    if (dr["SubAccTotal"] != null && !string.IsNullOrEmpty(dr["SubAccTotal"].ToString()))
                        report.SubAccTotal = int.Parse(dr["SubAccTotal"].ToString());
                    if (dr["TouristToUser"] != null && !string.IsNullOrEmpty(dr["TouristToUser"].ToString()))
                        report.TouristToUser = int.Parse(dr["TouristToUser"].ToString());
                    if (dr["WeixinUser"] != null && !string.IsNullOrEmpty(dr["WeixinUser"].ToString()))
                        report.WeixinUser = int.Parse(dr["WeixinUser"].ToString());
                    if (dr["ChannelId"] != null)
                        report.ChannelId = dr["ChannelId"].ToString();
                    list.Add(report);
                }
            }
            catch (Exception ex)
            {
                WriteError("ReportDAL.GetRegistReport Error:", ex.Message);
            }
            return list;
        }
        public List<NewChargeReport> GetChargeReport(ChargeReportSearch search, out int rowCount)
        {
            var dt = GetSearchData(search, DBName.MF_RECORD_DY, out rowCount);
            if (rowCount < 1)
                return null;
            var list = new List<NewChargeReport>();
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    var report = new NewChargeReport();
                    if (dr["Day"] != null && !string.IsNullOrEmpty(dr["Day"].ToString()))
                        report.Day = int.Parse(dr["Day"].ToString());
                    if (dr["PayMoney"] != null && !string.IsNullOrEmpty(dr["PayMoney"].ToString()))
                        report.PayMoney = decimal.Parse(dr["PayMoney"].ToString());
                    if (dr["PayChannel"] != null && !string.IsNullOrEmpty(dr["PayChannel"].ToString()))
                        report.PayChannel = int.Parse(dr["PayChannel"].ToString()); 
                    if (dr["PayNum"] != null && !string.IsNullOrEmpty(dr["PayNum"].ToString()))
                        report.PayNum = int.Parse(dr["PayNum"].ToString());
                    if (dr["SubmitMoney"] != null && !string.IsNullOrEmpty(dr["SubmitMoney"].ToString()))
                        report.SubmitMoney = decimal.Parse(dr["SubmitMoney"].ToString());
                    if (dr["SubmitNum"] != null && !string.IsNullOrEmpty(dr["SubmitNum"].ToString()))
                        report.SubmitNum = int.Parse(dr["SubmitNum"].ToString());
                    if (dr["ChannelId"] != null && !string.IsNullOrEmpty(dr["ChannelId"].ToString()))
                        report.ChannelId = dr["ChannelId"].ToString();
                    list.Add(report);
                }
            }
            catch (Exception ex)
            {
                WriteError("ReportDAL.GetChargeReport Error:", ex.Message);
            }
            return list;
        }
        public List<NewCurrencyReport> GetCurrencyReport(CurrencyReportSearch search, out int rowCount)
        {
            var dt = BaseDAL.GetSearchData(search, DBName.MF_RECORD_DY, out rowCount);
            if (rowCount < 1)
                return null;
            var list = new List<NewCurrencyReport>();
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(GetCurrencyModel(dr));
                }
            }
            catch (Exception ex)
            {
                WriteError("ReportDAL.GetCurrencyReport Error:", ex.Message);
                throw ex;
            }
            return list;
        }
        public NewCurrencyReport GetCurrencyModel(DataRow dr)
        {
            if (dr == null) return null;
            var model = new NewCurrencyReport();
            if (dr["Day"] != null && dr["Day"].ToString() != "")
            {
                model.Day = int.Parse(dr["Day"].ToString());
            }
            if (dr["LeftCurrency"] != null && dr["LeftCurrency"].ToString() != "")
            {
                model.LeftCurrency = long.Parse(dr["LeftCurrency"].ToString());
            }
            if (dr["TakeCurrency"] != null && dr["TakeCurrency"].ToString() != "")
            {
                model.TakeCurrency = long.Parse(dr["TakeCurrency"].ToString());
            }
            if (dr["StrongBoxCurrency"] != null && dr["StrongBoxCurrency"].ToString() != "")
            {
                model.StrongBoxCurrency = long.Parse(dr["StrongBoxCurrency"].ToString());
            }
            if (dr["StrongBoxCount"] != null && dr["StrongBoxCount"].ToString() != "")
            {
                model.StrongBoxCount = long.Parse(dr["StrongBoxCount"].ToString());
            }
            if (dr["StrongBoxCreated"] != null && dr["StrongBoxCreated"].ToString() != "")
            {
                model.StrongBoxCreated = long.Parse(dr["StrongBoxCreated"].ToString());
            }
            if (dr["StrongBoxDistory"] != null && dr["StrongBoxDistory"].ToString() != "")
            {
                model.StrongBoxDistory = long.Parse(dr["StrongBoxDistory"].ToString());
            }
            if (dr["ReliefCurrency"] != null && dr["ReliefCurrency"].ToString() != "")
            {
                model.ReliefCurrency = long.Parse(dr["ReliefCurrency"].ToString());
            }
            if (dr["SystemDeliveryCurrency"] != null && dr["SystemDeliveryCurrency"].ToString() != "")
            {
                model.SystemDeliveryCurrency = long.Parse(dr["SystemDeliveryCurrency"].ToString());
            }
            if (dr["FreeGameCurrency"] != null && dr["FreeGameCurrency"].ToString() != "")
            {
                model.FreeGameCurrency = long.Parse(dr["FreeGameCurrency"].ToString());
            }
            if (dr["ExchangeCurrency"] != null && dr["ExchangeCurrency"].ToString() != "")
            {
                model.ExchangeCurrency = long.Parse(dr["ExchangeCurrency"].ToString());
            }
            if (dr["AdminCurrency"] != null && dr["AdminCurrency"].ToString() != "")
            {
                model.AdminCurrency = long.Parse(dr["AdminCurrency"].ToString());
            }
            if (dr["BuyRoomCurrency"] != null && dr["BuyRoomCurrency"].ToString() != "")
            {
                model.BuyRoomCurrency = long.Parse(dr["BuyRoomCurrency"].ToString());
            }
            if (dr["Created"] != null && dr["Created"].ToString() != "")
            {
                model.Created = DateTime.Parse(dr["Created"].ToString());
            }
            if (dr["Modified"] != null && dr["Modified"].ToString() != "")
            {
                model.Modified = DateTime.Parse(dr["Modified"].ToString());
            }
            if (dr["ChannelId"] != null)
            {
                model.ChannelId = dr["ChannelId"].ToString();
            }
            if (dr["TimingCurrency"] != null && dr["TimingCurrency"].ToString() != "")
            {
                model.TimingCurrency = long.Parse(dr["TimingCurrency"].ToString());
            }
            if (dr["ResurrectionCurrency"] != null && dr["ResurrectionCurrency"].ToString() != "")
            {
                model.ResurrectionCurrency = long.Parse(dr["ResurrectionCurrency"].ToString());
            }
            if (dr["ChargeCurrency"] != null && dr["ChargeCurrency"].ToString() != "")
            {
                model.ChargeCurrency = long.Parse(dr["ChargeCurrency"].ToString());
            }
            if (dr["LastLeftCurrency"] != null && dr["LastLeftCurrency"].ToString() != "")
            {
                model.LastLeftCurrency = long.Parse(dr["LastLeftCurrency"].ToString());
            }
            return model;
        }
        public List<NewBeanReport> GetBeanReport(BeanReportSearch search, out int rowCount)
        {
            var dt = BaseDAL.GetSearchData(search, DBName.MF_RECORD_DY, out rowCount);
            if (rowCount < 1)
                return null;
            var list = new List<NewBeanReport>();
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    var model = new NewBeanReport();
                    if (dr["Day"] != null && dr["Day"].ToString() != "")
                    {
                        model.Day = int.Parse(dr["Day"].ToString());
                    }
                    if (dr["LeftBean"] != null && dr["LeftBean"].ToString() != "")
                    {
                        model.LeftBean = long.Parse(dr["LeftBean"].ToString());
                    }
                    if (dr["ShopExchangeBean"] != null && dr["ShopExchangeBean"].ToString() != "")
                    {
                        model.ShopExchangeBean = long.Parse(dr["ShopExchangeBean"].ToString());
                    }
                    if (dr["TimingBean"] != null && dr["TimingBean"].ToString() != "")
                    {
                        model.TimingBean = long.Parse(dr["TimingBean"].ToString());
                    }
                    if (dr["AdminBean"] != null && dr["AdminBean"].ToString() != "")
                    {
                        model.AdminBean = long.Parse(dr["AdminBean"].ToString());
                    }
                    if (dr["ChannelId"] != null)
                    {
                        model.ChannelId = dr["ChannelId"].ToString();
                    }
                    if (dr["TelephoneFareBean"] != null && dr["TelephoneFareBean"].ToString() != "")
                    {
                        model.TelephoneFareBean = long.Parse(dr["TelephoneFareBean"].ToString());
                    }
                    list.Add(model);
                }
            }
            catch (Exception ex)
            {
                WriteError("ReportDAL.GetBeanReport Error:", ex.Message);
                throw ex;
            }
            return list;
        }
        public List<NewHappyRecordReport> GetHappyRecordReport(HappyRecordReportSearch search, out int rowCount)
        {
            var dt = BaseDAL.GetSearchData(search, DBName.MF_RECORD_DY, out rowCount);
            if (rowCount < 1)
                return null;
            var list = new List<NewHappyRecordReport>();
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    var model = new NewHappyRecordReport();
                    if (dr["Day"] != null && dr["Day"].ToString() != "")
                    {
                        model.Day = int.Parse(dr["Day"].ToString());
                    }
                    if (dr["AdminHappy"] != null && dr["AdminHappy"].ToString() != "")
                    {
                        model.AdminHappy = long.Parse(dr["AdminHappy"].ToString());
                    }
                    if (dr["ChargeHappy"] != null && dr["ChargeHappy"].ToString() != "")
                    {
                        model.ChargeHappy = long.Parse(dr["ChargeHappy"].ToString());
                    }
                    if (dr["GameReturnHappy"] != null && dr["GameReturnHappy"].ToString() != "")
                    {
                        model.GameReturnHappy = long.Parse(dr["GameReturnHappy"].ToString());
                    }
                    if (dr["GameUseHappy"] != null && dr["GameUseHappy"].ToString() != "")
                    {
                        model.GameUseHappy = long.Parse(dr["GameUseHappy"].ToString());
                    }
                    if (dr["LeftHappy"] != null && dr["LeftHappy"].ToString() != "")
                    {
                        model.LeftHappy = long.Parse(dr["LeftHappy"].ToString());
                    }
                    model.ChannelId = dr["ChannelId"].ToString();
                    list.Add(model);
                }
            }
            catch (Exception ex)
            {
                WriteError("ReportDAL.GetHappyRecordReport Error:", ex.Message);
                throw ex;
            }
            return list;
        }
        public NewAdReport GetAdReportModel(DataRow dr)
        {
            if (dr == null) return null;
            var model = new NewAdReport();
            try
            {
                if (dr["day"] != null && dr["day"].ToString() != "")
                {
                    model.Day = int.Parse(dr["day"].ToString());
                }
                if (dr["LoginNum"] != null && dr["LoginNum"].ToString() != "")
                {
                    model.LoginNum = int.Parse(dr["LoginNum"].ToString());
                }
                if (dr["MaxNum"] != null && dr["MaxNum"].ToString() != "")
                {
                    model.MaxNum = int.Parse(dr["MaxNum"].ToString());
                }
                if (dr["ActUserNum"] != null && dr["ActUserNum"].ToString() != "")
                {
                    model.ActUserNum = int.Parse(dr["ActUserNum"].ToString());
                }
                if (dr["ActMatchsUserNum"] != null && dr["ActMatchsUserNum"].ToString() != "")
                {
                    model.ActMatchsUserNum = int.Parse(dr["ActMatchsUserNum"].ToString());
                }
                if (dr["RegMatchsNum"] != null && dr["RegMatchsNum"].ToString() != "")
                {
                    model.RegMatchsNum = int.Parse(dr["RegMatchsNum"].ToString());
                }
                if (dr["_1Day"] != null && dr["_1Day"].ToString() != "")
                {
                    model._1Day = int.Parse(dr["_1Day"].ToString());
                }
                if (dr["_3Day"] != null && dr["_3Day"].ToString() != "")
                {
                    model._3Day = int.Parse(dr["_3Day"].ToString());
                }
                if (dr["_7Day"] != null && dr["_7Day"].ToString() != "")
                {
                    model._7Day = int.Parse(dr["_7Day"].ToString());
                }
                if (dr["M_1Day"] != null && dr["M_1Day"].ToString() != "")
                {
                    model.M_1Day = int.Parse(dr["M_1Day"].ToString());
                }
                if (dr["M_3Day"] != null && dr["M_3Day"].ToString() != "")
                {
                    model.M_3Day = int.Parse(dr["M_3Day"].ToString());
                }
                if (dr["M_4Day"] != null && dr["M_4Day"].ToString() != "")
                {
                    model.M_4Day = int.Parse(dr["M_4Day"].ToString());
                }
                if (dr["M_5Day"] != null && dr["M_5Day"].ToString() != "")
                {
                    model.M_5Day = int.Parse(dr["M_5Day"].ToString());
                }
                if (dr["M_6Day"] != null && dr["M_6Day"].ToString() != "")
                {
                    model.M_6Day = int.Parse(dr["M_6Day"].ToString());
                }
                if (dr["M_7Day"] != null && dr["M_7Day"].ToString() != "")
                {
                    model.M_7Day = int.Parse(dr["M_7Day"].ToString());
                }
                if (dr["M_15Day"] != null && dr["M_15Day"].ToString() != "")
                {
                    model.M_15Day = int.Parse(dr["M_15Day"].ToString());
                }
                if (dr["M_30Day"] != null && dr["M_30Day"].ToString() != "")
                {
                    model.M_30Day = int.Parse(dr["M_30Day"].ToString());
                }
                if (dr["NNM_1Day"] != null && dr["NNM_1Day"].ToString() != "")
                {
                    model.NNM_1Day = int.Parse(dr["NNM_1Day"].ToString());
                }
                if (dr["NNM_3Day"] != null && dr["NNM_3Day"].ToString() != "")
                {
                    model.NNM_3Day = int.Parse(dr["NNM_3Day"].ToString());
                }
                if (dr["NNM_4Day"] != null && dr["NNM_4Day"].ToString() != "")
                {
                    model.NNM_4Day = int.Parse(dr["NNM_4Day"].ToString());
                }
                if (dr["NNM_5Day"] != null && dr["NNM_5Day"].ToString() != "")
                {
                    model.NNM_5Day = int.Parse(dr["NNM_5Day"].ToString());
                }
                if (dr["NNM_6Day"] != null && dr["NNM_6Day"].ToString() != "")
                {
                    model.NNM_6Day = int.Parse(dr["NNM_6Day"].ToString());
                }
                if (dr["NNM_7Day"] != null && dr["NNM_7Day"].ToString() != "")
                {
                    model.NNM_7Day = int.Parse(dr["NNM_7Day"].ToString());
                }
                if (dr["NNM_15Day"] != null && dr["NNM_15Day"].ToString() != "")
                {
                    model.NNM_15Day = int.Parse(dr["NNM_15Day"].ToString());
                }
                if (dr["NNM_30Day"] != null && dr["NNM_30Day"].ToString() != "")
                {
                    model.NNM_30Day = int.Parse(dr["NNM_30Day"].ToString());
                }
                if (dr["GAP_15"] != null && dr["GAP_15"].ToString() != "")
                {
                    model.GAP_15 = int.Parse(dr["GAP_15"].ToString());
                }
                if (dr["ARPU"] != null && dr["ARPU"].ToString() != "")
                {
                    model.ARPU = dr["ARPU"].ToString();
                }
                if (dr["PayRate"] != null && dr["PayRate"].ToString() != "")
                {
                    model.PayRate = dr["PayRate"].ToString();
                }
                if (dr["ARPPU"] != null && dr["ARPPU"].ToString() != "")
                {
                    model.ARPPU = dr["ARPPU"].ToString();
                }
                if (dr["ChargeUserRate"] != null && dr["ChargeUserRate"].ToString() != "")
                {
                    model.ChargeUserRate = dr["ChargeUserRate"].ToString();
                }
                if (dr["ChannelId"] != null)
                {
                    model.ChannelId = dr["ChannelId"].ToString();
                }
                if (dr["Created"] != null && dr["Created"].ToString() != "")
                {
                    model.Created = DateTime.Parse(dr["Created"].ToString());
                }
                if (dr["Modified"] != null && dr["Modified"].ToString() != "")
                {
                    model.Modified = DateTime.Parse(dr["Modified"].ToString());
                }
                if (dr["RegNum"] != null && dr["RegNum"].ToString() != "")
                {
                    model.RegNum = int.Parse(dr["RegNum"].ToString());
                }
            }
            catch (Exception ex)
            {
                WriteError("ReportDAL.GetPromotReport Error:", ex.Message);
                throw ex;
            }
            return model;
        }
        public List<NewAdReport> GetPromotReport(PromotReportSearch search, out int rowCount)
        {
            var dt = BaseDAL.GetSearchData(search, DBName.MF_RECORD_DY, out rowCount);
            if (rowCount < 1)
                return null; 
            var list = new List<NewAdReport>();
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(GetAdReportModel(dr));
                }
            }
            catch (Exception ex)
            {
                WriteError("ReportDAL.GetPromotReport Error:", ex.Message);
                throw ex;
            }
            return list;
        }
        public List<ExtendChannel> GetExtendChannelRecord(ExtendChannelSearch search, out int rowCount)
        {
            var dt = GetSearchData(search, DBName.MF_RECORD_DY, out rowCount);
            if (rowCount < 1)
                return null;
            var list = new List<ExtendChannel>();
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    var report = new ExtendChannel();
                    if (dr["AndroidDown"] != null && !string.IsNullOrEmpty(dr["AndroidDown"].ToString()))
                        report.AndroidDown = int.Parse(dr["AndroidDown"].ToString());
                    if (dr["AndroidFirstActive"] != null && !string.IsNullOrEmpty(dr["AndroidFirstActive"].ToString()))
                        report.AndroidFirstActive = int.Parse(dr["AndroidFirstActive"].ToString());
                    if (dr["AndroidLoad"] != null && !string.IsNullOrEmpty(dr["AndroidLoad"].ToString()))
                        report.AndroidLoad = int.Parse(dr["AndroidLoad"].ToString());
                    report.Channel = dr["Channel"].ToString();
                    report.ChannelNum = dr["ChannelNum"].ToString();
                    if (dr["Day"] != null && !string.IsNullOrEmpty(dr["Day"].ToString()))
                        report.Day = int.Parse(dr["Day"].ToString());
                    if (dr["iOSDown"] != null && !string.IsNullOrEmpty(dr["iOSDown"].ToString()))
                        report.iOSDown = int.Parse(dr["iOSDown"].ToString());
                    if (dr["iOSFirstActive"] != null && !string.IsNullOrEmpty(dr["iOSFirstActive"].ToString()))
                        report.iOSFirstActive = int.Parse(dr["iOSFirstActive"].ToString());
                    if (dr["iOSLoad"] != null && !string.IsNullOrEmpty(dr["iOSLoad"].ToString()))
                        report.iOSLoad = int.Parse(dr["iOSLoad"].ToString());
                    if (dr["LoadTimeAvg"] != null && !string.IsNullOrEmpty(dr["LoadTimeAvg"].ToString()))
                        report.LoadTimeAvg = int.Parse(dr["LoadTimeAvg"].ToString());
                    if (dr["PCDown"] != null && !string.IsNullOrEmpty(dr["PCDown"].ToString()))
                        report.PCDown = int.Parse(dr["PCDown"].ToString());
                    if (dr["PCFirstActive"] != null && !string.IsNullOrEmpty(dr["PCFirstActive"].ToString()))
                        report.PCFirstActive = int.Parse(dr["PCFirstActive"].ToString());
                    if (dr["PCLoad"] != null && !string.IsNullOrEmpty(dr["PCLoad"].ToString()))
                        report.PCLoad = int.Parse(dr["PCLoad"].ToString());
                    if (dr["SecondDown"] != null && !string.IsNullOrEmpty(dr["SecondDown"].ToString()))
                        report.SecondDown = int.Parse(dr["SecondDown"].ToString());
                    if (dr["SecondDownTimeAvg"] != null && !string.IsNullOrEmpty(dr["SecondDownTimeAvg"].ToString()))
                        report.SecondDownTimeAvg = int.Parse(dr["SecondDownTimeAvg"].ToString());
                    if (dr["Stay"] != null && !string.IsNullOrEmpty(dr["Stay"].ToString()))
                        report.Stay = int.Parse(dr["Stay"].ToString());
                    if (dr["Register"] != null && !string.IsNullOrEmpty(dr["Register"].ToString()))
                        report.Register = int.Parse(dr["Register"].ToString());
                    if (dr["NetWifi"] != null && !string.IsNullOrEmpty(dr["NetWifi"].ToString()))
                        report.NetWifi = int.Parse(dr["NetWifi"].ToString());
                    if (dr["NetMobileData"] != null && !string.IsNullOrEmpty(dr["NetMobileData"].ToString()))
                        report.NetMobileData = int.Parse(dr["NetMobileData"].ToString());
                    list.Add(report);
                }
            }
            catch (Exception ex)
            {
                WriteError("ReportDAL.GetExtendChannelRecord Error:", ex.Message, ex.StackTrace, ex.Source);
            }
            return list;
        }
        public void StoreTodayExtendChannelData(List<ExtendChannel> list)
        {
            try
            {
                string log = "";
                foreach (ExtendChannel model in list)
                {
                    log += string.Format("{0}_{1}_{2}_{3}_{4}_{5}_{6}_{7}_{8}_{9}_{10}_{11}_{12}_{13}_{14}_{15}"
                        , model.AndroidDown, model.AndroidFirstActive, model.AndroidLoad,
                       model.Channel, model.ChannelNum, model.Day, model.iOSDown, model.iOSFirstActive, model.iOSLoad,
                       model.LoadTimeAvg, model.PCDown, model.PCFirstActive, model.PCLoad, model.SecondDown, model.SecondDownTimeAvg, model.Stay);
                    WriteDebug("StoreTodayExtendChannelData log is ", log);
                    var args = new SqlParameter[] {
                    new SqlParameter("@AndroidDown",model.AndroidDown),
                    new SqlParameter("@AndroidFirstActive",model.AndroidFirstActive),
                        new SqlParameter("@AndroidLoad",model.AndroidLoad),
                        new SqlParameter("@Channel",model.Channel),
                        new SqlParameter("@ChannelNum",model.ChannelNum),
                        new SqlParameter("@Day",model.Day),
                        new SqlParameter("@iOSDown",model.iOSDown),
                        new SqlParameter("@iOSFirstActive",model.iOSFirstActive),
                        new SqlParameter("@iOSLoad",model.iOSLoad),
                        new SqlParameter("@LoadTimeAvg",model.LoadTimeAvg),
                        new SqlParameter("@PCDown",model.PCDown),
                        new SqlParameter("@PCFirstActive",model.PCFirstActive),
                        new SqlParameter("@PCLoad",model.PCLoad),
                        new SqlParameter("@SecondDown",model.SecondDown),
                        new SqlParameter("@SecondDownTimeAvg",model.SecondDownTimeAvg),
                        new SqlParameter("@Stay",model.Stay)
                };
                    DataHelper.ExecuteProcedure("mf_P_ExtendChannel", args);
                }
            }
            catch (Exception e)
            {
                BaseDAL.WriteError("写入数据库【每日redis推广渠道】数据异常:", e.Message);
            }
        }
        public List<NewQmallReport> GetExchangeReport(NewQmallReportSearch search, out int rowCount)
        {
            var dt = GetSearchData(search, DBName.MF_RECORD_DY, out rowCount);
            if (rowCount < 1)
                return null;
            var list = new List<NewQmallReport>();
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    var report = new NewQmallReport();
                    if (dr["Day"] != null && !string.IsNullOrEmpty(dr["Day"].ToString()))
                        report.Day = int.Parse(dr["Day"].ToString());
                    if (dr["Product"] != null && !string.IsNullOrEmpty(dr["Product"].ToString()))
                        report.Product = int.Parse(dr["Product"].ToString());
                    if (dr["SellNum"] != null && !string.IsNullOrEmpty(dr["SellNum"].ToString()))
                        report.SellNum = int.Parse(dr["SellNum"].ToString());
                    if (dr["ChannelId"] != null)
                        report.ChannelId = dr["ChannelId"].ToString();
                    list.Add(report);
                }
            }
            catch (Exception ex)
            {
                WriteError("ReportDAL.GetExchangeReport Error:", ex.Message, ex.StackTrace, ex.Source);
            }
            return list;
        }
        public List<NewADIDReport> GetAdidRegReport(NewADIDReportSearch search, out int rowCount)
        {
            var dt = GetSearchData(search, DBName.MF_RECORD_DY, out rowCount);
            if (rowCount < 1)
                return null;
            var list = new List<NewADIDReport>();
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    var report = GetNewADIDModel(dr);
                    list.Add(report);
                }
            }
            catch (Exception ex)
            {
                WriteError("ReportDAL.GetAdidRegReport Error:", ex.Message, ex.StackTrace, ex.Source);
            }
            return list;
        }
        public List<NewBaiduAdReport> GetBaiduADReport(NewBaiduAdReportSearch search, out int rowCount)
        {
            var dt = GetSearchData(search, DBName.MF_RECORD_DY, out rowCount);
            if (rowCount < 1)
                return null;
            var list = new List<NewBaiduAdReport>();
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    var report = GetBaiduAdModel(dr);
                    list.Add(report);
                }
            }
            catch (Exception ex)
            {
                WriteError("ReportDAL.NewBaiduAdReport Error:", ex.Message, ex.StackTrace, ex.Source);
            }
            return list;
        }
        public NewBaiduAdReport GetBaiduAdModel(DataRow row)
        {
            NewBaiduAdReport model = new NewBaiduAdReport();
            if (row != null)
            {
                if (row["day"] != null && row["day"].ToString() != "")
                {
                    model.day = int.Parse(row["day"].ToString());
                }
                if (row["LoginNum"] != null && row["LoginNum"].ToString() != "")
                {
                    model.LoginNum = int.Parse(row["LoginNum"].ToString());
                }
                if (row["ActUserNum"] != null && row["ActUserNum"].ToString() != "")
                {
                    model.ActUserNum = int.Parse(row["ActUserNum"].ToString());
                }
                if (row["OneDayLeft"] != null && row["OneDayLeft"].ToString() != "")
                {
                    model.OneDayLeft = int.Parse(row["OneDayLeft"].ToString());
                }
                if (row["ThreeDayLeft"] != null && row["ThreeDayLeft"].ToString() != "")
                {
                    model.ThreeDayLeft = int.Parse(row["ThreeDayLeft"].ToString());
                }
                if (row["SevenDayLeft"] != null && row["SevenDayLeft"].ToString() != "")
                {
                    model.SevenDayLeft = int.Parse(row["SevenDayLeft"].ToString());
                }
                if (row["ChannelId"] != null)
                {
                    model.ChannelId = row["ChannelId"].ToString();
                }
                if (row["Created"] != null && row["Created"].ToString() != "")
                {
                    model.Created = DateTime.Parse(row["Created"].ToString());
                }
                if (row["Modified"] != null && row["Modified"].ToString() != "")
                {
                    model.Modified = DateTime.Parse(row["Modified"].ToString());
                }
                if (row["regNum"] != null && row["regNum"].ToString() != "")
                {
                    model.regNum = int.Parse(row["regNum"].ToString());
                }
                if (row["ADID"] != null)
                {
                    model.ADID = row["ADID"].ToString();
                }
            }
            return model;
        }
        private NewADIDReport GetNewADIDModel(DataRow row)
        {
            NewADIDReport model = new NewADIDReport();
            if (row != null)
            {
                if (row["day"] != null && row["day"].ToString() != "")
                {
                    model.day = int.Parse(row["day"].ToString());
                }
                if (row["ADID"] != null)
                {
                    model.ADID = row["ADID"].ToString();
                }
                if (row["Num"] != null && row["Num"].ToString() != "")
                {
                    model.Num = int.Parse(row["Num"].ToString());
                }
                if (row["ChannelId"] != null)
                {
                    model.ChannelId = row["ChannelId"].ToString();
                }
                if (row["created"] != null && row["created"].ToString() != "")
                {
                    model.created = DateTime.Parse(row["created"].ToString());
                }
                if (row["modified"] != null && row["modified"].ToString() != "")
                {
                    model.modified = DateTime.Parse(row["modified"].ToString());
                }
            }
            return model;
        }

        public List<UserDayReports> GetZywyStaticReport(int start, int end, string area)
        {
            List<UserDayReports> list = new List<UserDayReports>();
            if (string.IsNullOrEmpty(area))
            {
                string regionRes = PostZywy("getRegion", "");
                if (string.IsNullOrEmpty(regionRes)) return null;
                ZywyRes<List<ZywyRegion>> r = Newtonsoft.Json.JsonConvert.DeserializeObject<ZywyRes<List<ZywyRegion>>>(regionRes);
                if (r.code != 0 || r.data == null || r.data.Count < 1)
                {
                    WriteError(r.msg);
                    return null;
                }
                foreach (var region in r.data)
                {
                    var listTmp = GetStaticData(region.ID, start, end);
                    if (listTmp == null || listTmp.Count < 1) continue;
                    list.AddRange(listTmp);
                }
            }
            else
            {
                list= GetStaticData(int.Parse(area), start, end);
            }
            if (list.Count < 1) return list;
            list.Sort();
            return list;
        }
        public static string PostZywy(string protrol, string json)
        {
            string url = ConfigurationManager.AppSettings["zywy_server_url"] + "/" + protrol;
            string sign_key = ConfigurationManager.AppSettings["zywy_key"];
            var request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            //request.ContentLength = data.Length;//会出现请数据被终止的异常 或者使用 coding.GetBytes(data).Length   //原因是汉字编码每一个汉字是2个字节
            //request.Timeout = timeout* 1000;
            request.KeepAlive = true;
            var sw = new StreamWriter(request.GetRequestStream());
            sw.Write(json);
            sw.Close();
            var response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                var res = reader.ReadToEnd();
                return res;
            }
        }
        /// <summary>
        /// 将dateTime格式转换为Unix时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public int DateTimeToUnixTime(DateTime dateTime)
        {
            return (int)(dateTime - TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1))).TotalSeconds;
        }
        public string ToRfc3339String(DateTime dateTime)
        {
            //2019-12-10T23:59:59+08:00
            return dateTime.ToString("yyyy-MM-dd'T'HH:mm:ssK", DateTimeFormatInfo.InvariantInfo);
        }
        public string StortJson(ZywyStatic entity)
        {
            WriteDebug("sortjson json start:");
            JsonSerializerSettings setting = new JsonSerializerSettings();
            setting.NullValueHandling = NullValueHandling.Ignore; //忽略null值; 
            setting.ContractResolver = new PropertySortResolver();
            string json = JsonConvert.SerializeObject(entity, setting);
            WriteDebug("sortjson json:", json);
            return json;
        }
        public List<UserDayReports> GetStaticData(int region, int start, int end)
        {
            try
            {
                WriteLog("dal GetStaticData region:", region.ToString(), " start:", start.ToString(), " end:", end.ToString());
                ZywyStatic z = new ZywyStatic();
                z.Index = new Random().Next(100000000, 999999999).ToString();
                z.Time = DateTimeToUnixTime(DateTime.Now);
                z.Start = ToRfc3339String(new DateTime(2012, 9, 30, 16, 0, 0).AddDays(start).ToLocalTime());
                z.End = ToRfc3339String(new DateTime(2012, 10, 1, 15, 59, 59).AddDays(end).ToLocalTime());
                z.Region = region; 
                 string json_before = StortJson(z);
                string sign_key = ConfigurationManager.AppSettings["zywy_key"];
                z.Key = MD5.Encrypt(json_before + sign_key);
                string json = StortJson(z);
                var postRes = PostZywy("statistics", json);
                if (postRes == "")
                {
                    return null;
                }
                ZywyRes<List<UserDayReports>> list = Newtonsoft.Json.JsonConvert.DeserializeObject<ZywyRes<List<UserDayReports>>>(postRes);
                if (list.code != 0 || list.data == null || list.data.Count < 1)
                {
                    WriteError("get userdayreport err.", list.msg);
                    return null;
                }
                return list.data;
            }catch(Exception ex)
            {
                WriteError("dal GetStaticData ex:", ex.Message);
                return null;
            }
        }

    }
}
