using System;

namespace MF.Data
{
    [Serializable]
    public class Search
    {
        /// <summary>
        /// 获取或设置查询页大小，默认30条
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 获取或设置查询页数,默认第1页
        /// </summary>
        public int PageIndex { get; set; }
        public string Fields { get; set; }
        public string PrimaryKey { get; set; }
        public string Table { get; set; }
        public string Where { get; set; }
        //public string OrderBy { get; set; }
        public string OrderBy
        {
            get
            {
                return " ORDER BY " + this.PrimaryKey + " DESC ";
            }
        }
        /// <summary>
        /// 查询数据库来源【备份数据库、现有生产数据库】
        /// </summary>
        public DBSource DBSource { get; set; }
        /// <summary>
        /// 查询数据库名称
        /// </summary>
        public DBName DBName { get; set; }
    }
    [Serializable]
    public class Search<T>
    {
        /// <summary>
        /// 获取或设置查询页大小，默认30条
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 获取或设置查询页数,默认第1页
        /// </summary>
        public int PageIndex { get; set; }
        public string Fields { get; set; }
        public string PrimaryKey { get; set; }
        public string Table { get; set; }
        public string Where { get; set; }
        public string OrderBy { get; set; }
        /// <summary>
        /// 查询数据库来源【备份数据库、现有生产数据库】
        /// </summary>
        public DBSource DBSource { get; set; }
        /// <summary>
        /// 查询数据库名称
        /// </summary>
        public DBName DBName { get; set; }
        /// <summary>
        /// 当前查询的对象
        /// </summary>
        public T SearchObj { get; set; }
        /// <summary>
        /// 是否按时间查询
        /// </summary>
        public bool IsChkTime { get; set; }
        /// <summary>
        /// 查询开始时间
        /// </summary>
        public long StartTime { get; set; }
        /// <summary>
        /// 查询结束时间
        /// </summary>
        public long OverTime { get; set; }
    }
    public enum DBName
    {
        ALL = -1,
        MF_DY = 1,
        MF_RECORD_DY = 2,
        Manage = 3,
        USER_0_DY = 100,
        USER_1_DY = 101,
        USER_21_DY = 121
    }
    public enum DBSource
    {
        /// <summary>
        /// 备份数据库
        /// </summary>
        DBBACK = 1,
        /// <summary>
        /// 现有生产数据库
        /// </summary>
        DBNOW = 2
    }

    [Serializable]
    public class LoginLogSearch : Search
    {
        public LoginLogSearch()
        {
            this.PageIndex = 1;
            this.PageSize = 30;
            this.PrimaryKey = "ID";
            this.Table = "LoginLog";
            this.Fields = "ID,Account,LoginTime,IP,LoginState,[Message]";
            this.Where = " 1=1 ";
        }
    }
    [Serializable]
    public class SystemLogSearch : Search
    {
        public SystemLogSearch()
        {
            this.PageIndex = 1;
            this.PageSize = 30;
            this.PrimaryKey = "ID";
            this.Table = "SystemLog";
            this.Fields = "ID,Account,OperTime,IP,Operation,Page,Remark,OprState,Type";
            this.Where = " 1=1 ";
        }
    }

    [Serializable]
    public class RegistReportSearch : Search
    {
        public RegistReportSearch()
        {
            this.PageIndex = 1;
            this.PageSize = 30;
            this.PrimaryKey = "[Day]";
            this.Table = "NewRegReport";
            this.Fields = "[day],[NewUser],[NewVisitor],[ClientUser],[IphoneUser],[AndroidUser],[IpadUser],[TouristToUser],[SubAccTotal],[AccTotal],[ChannelId],[Created],[modified],[Relief],[WeixinUser]";
            this.Where = "";
            // this.OrderBy = " ORDER BY [day] DESC ";
        }
    }
    [Serializable]
    public class ChargeReportSearch : Search
    {
        public ChargeReportSearch()
        {
            this.PageIndex = 1;
            this.PageSize = 30;
            this.PrimaryKey = "[Day]";
            this.Table = "NewChargeReport";
            this.Fields = "[Day], SubmitNum, SubmitMoney, PayNum, PayMoney,PayChannel,ChannelId";
            this.Where = "";
        }
    }
    [Serializable]
    public class PayChannelReportSearch : Search
    {
        public PayChannelReportSearch()
        {
            this.PageIndex = 1;
            this.PageSize = 30;
            this.PrimaryKey = "[Day]";
            this.Table = "PayChannelReport";
            this.Fields = "[Day], ChannelId, SubmitNum, SubmitMoney, PayNum, PayMoney";
            this.Where = "";
            // this.OrderBy = " ORDER BY [day] DESC ";
        }
    }
    [Serializable]
    public class CurrencyReportSearch : Search
    {
        public CurrencyReportSearch()
        {
            this.PageIndex = 1;
            this.PageSize = 30;
            this.PrimaryKey = "[Day]";
            this.Table = "NewCurrencyReport";
            this.Fields = "[Day],[LeftCurrency],[TakeCurrency],[StrongBoxCurrency],[StrongBoxCount],[StrongBoxCreated],[ReliefCurrency],[SystemDeliveryCurrency],[FreeGameCurrency],[ExchangeCurrency],[AdminCurrency],[BuyRoomCurrency],[Created],[Modified],[ChannelId],[TimingCurrency],[ResurrectionCurrency],[StrongBoxDistory],[ChargeCurrency],(select max(LeftCurrency) from NewCurrencyReport n2 where n2.Day=(t.Day-1) and  ChannelId='10A') as LastLeftCurrency ";
            this.Where = "";
        }
    }
    [Serializable]
    public class BeanReportSearch : Search
    {
        public BeanReportSearch()
        {
            this.PageIndex = 1;
            this.PageSize = 30;
            this.PrimaryKey = "[Day]";
            this.Table = "NewBeanReport";
            this.Fields = "[Day],[LeftBean],[ShopExchangeBean],[TimingBean],[AdminBean],[ChannelId],[TelephoneFareBean]";
            this.Where = "";
        }
    }
    [Serializable]
    public class HappyRecordReportSearch : Search
    {
        public HappyRecordReportSearch()
        {
            this.PageIndex = 1;
            this.PageSize = 30;
            this.PrimaryKey = "[Day]";
            this.Table = "NewHappyRecordReport";
            this.Fields = "[Day],ChannelId,LeftHappy,ChargeHappy,AdminHappy,GameUseHappy,GameReturnHappy";
            this.Where = "";
        }
    }
    [Serializable]
    public class GameReportSearch : Search
    {
        public GameReportSearch()
        {
            this.PageIndex = 1;
            this.PageSize = 30;
            this.PrimaryKey = "[Day]";
            this.Table = "NewGameReport";
            this.Fields = "[Day], GameId,MatchId, Win,Lose, Shrink,Actives,RuleId,ChannelId";
            this.Where = " 1=1 ";
        }
    }
    [Serializable]
    public class LoginReportSearch : Search
    {
        public LoginReportSearch()
        {
            this.PageIndex = 1;
            this.PageSize = 30;
            this.PrimaryKey = "[Day]";
            this.Table = "LoginReport";
            this.Fields = "[Day],Account,LogTime,IP,[Content],Page";
            this.Where = "";
        }
    }
    [Serializable]
    public class PromotReportSearch : Search
    {
        public PromotReportSearch()
        {
            this.PageIndex = 1;
            this.PageSize = 30;
            this.PrimaryKey = "[Day]";
            this.Table = "NewAdReport";
            this.Fields = "[day],[LoginNum],[MaxNum],[ActUserNum],[ActMatchsUserNum],[RegMatchsNum],[_1Day],[_3Day],[_7Day],[ARPU],[PayRate],[ARPPU],[ChargeUserRate],[ChannelId],[Created],[Modified],RegNum,[M_1Day],[M_3Day],[M_4Day],[M_5Day],[M_6Day],[M_7Day],[M_15Day],[M_30Day],[NNM_1Day],[NNM_3Day],[NNM_4Day],[NNM_5Day],[NNM_6Day],[NNM_7Day],[NNM_15Day],[NNM_30Day],[GAP_15]";
            this.Where = "";
        }
    }
    [Serializable]
    public class RoomCardReportSearch : Search
    {
        public RoomCardReportSearch()
        {
            this.PageIndex = 1;
            this.PageSize = 30;
            this.PrimaryKey = "[Day]";
            this.Table = "RoomCardReport";
            this.Fields = "[Day], Today, Regist, Share, Buy, UseNum, QQShare, WeixinShare, Charge, BindPhone, Total";
            this.Where = "";
        }
    }
    [Serializable]
    public class ChargeRecordSearch : Search<ChargeRecord>
    {
        public ChargeRecordSearch() { }
        /// <summary>
        /// 查询时间类型【可能存在多种时间查询】
        /// 1为支付时间查询 2为提交时间查询
        /// </summary>
        public long TimeType { get; set; }
        /// <summary>
        /// 支付渠道Between
        /// </summary>
        public long StartPayChannel { get; set; }
        /// <summary>
        /// 支付渠道End
        /// </summary>
        public long OverPayChannel { get; set; }
        /// <summary>
        /// 是否为精确查询【1为精确 否则为模糊】
        /// </summary>
        public long IsExact { get; set; }
        /// <summary>
        /// 是否按值查询【模糊？精确？】
        /// </summary>
        public bool IsSearchKey { get; set; }
    }

    [Serializable]
    public class GuildApplyRecordSearch : Search<GuildApplyRecord>
    {
        public GuildApplyRecordSearch() { }
        /// <summary>
        /// 是否为精确查询【1为精确 否则为模糊】
        /// </summary>
        public long IsExact { get; set; }
        /// <summary>
        /// 是否按值查询【模糊？精确？】
        /// </summary>
        public bool IsSearchKey { get; set; }
        /// <summary>
        /// 查询时间类型【可能存在多种时间查询】
        /// 1为创建时间查询 2为支付时间查询 3为申请退款时间
        /// </summary>
        public long TimeType { get; set; }
    }

    [Serializable]
    public class UserSearch : Search
    {
        public UserSearch()
        {
            this.PageIndex = 1;
            this.PageSize = 20;
            this.PrimaryKey = "[ID]";
            this.Table = "Users";
            this.Fields = "ID,Account,NickName,[Name],[Identity],Sex,Flag,Mobile,PhoneKey,Lock,Relief,[GUID],Lv,[Exp],Guest,ChargeId,ADID,Regitime,RegistIp,RegistDevice,RegistArea,LastIp,LastLogin,LoginDevice,LoginCount,DeviceCode,Currency,Bean,RoomCard,Silver";
            this.Where = "";
        }
    }
    //[Serializable]
    //public class CurrencyRecordSearch : Search<CurrencyRecord>
    //{
    //    public CurrencyRecordSearch()
    //    {
    //        this.PageIndex = 1;
    //        this.PageSize = 30;
    //        this.PrimaryKey = "[ID]";
    //        this.Table = "CurrcryRecord";
    //        this.Fields = "[Time],Account,GameId,MatchId,[Type],Num,Original,IP";
    //       // this.Where = "";
    //        this.OrderBy = "";
    //    }
    //}
    [Serializable]
    public class BeanRecordSearch : Search
    {
        public BeanRecordSearch()
        {
            this.PageIndex = 1;
            this.PageSize = 30;
            this.PrimaryKey = "[ID]";
            this.Table = "BeanRecord";
            this.Fields = "ID,Time, Account, Type, Num, IP, MatchId, GameId, Original";
            this.Where = "";
        }
    }

    [Serializable]
    public class GuildSearch : Search<Guild>
    {
        public GuildSearch() { }
        /// <summary>
        /// 是否为精确查询【1为精确 否则为模糊】
        /// </summary>
        public long IsExact { get; set; }
        /// <summary>
        /// 是否按值查询【模糊？精确？】
        /// </summary>
        public bool IsSearchKey { get; set; }
        /// <summary>
        /// 查询开始时间
        /// </summary>
        public DateTime StartTimeGuild { get; set; }
        /// <summary>
        /// 查询结束时间
        /// </summary>
        public DateTime OverTimeGuild { get; set; }
    }
    [Serializable]
    public class GuildUserSearch : Search
    {
        public GuildUserSearch()
        {
            this.PageIndex = 1;
            this.PageSize = 30;
            this.PrimaryKey = "[ID]";
            this.Table = "GuildUser";
            this.Fields = "ID,Account,JoinDate, LastOnLine,LastWeekActiveDay,GuildID,Nickname,ActiveNum";
            this.Where = "";
        }
    }
    [Serializable]
    public class ChargeDistributeSearch : Search
    {
        public ChargeDistributeSearch()
        {
            this.PageIndex = 1;
            this.PageSize = 30;
            this.PrimaryKey = "[Day]";
            this.Table = "ChargeDistribute";
            this.Fields = "[Day],First,Six,Twenty,Fifty,Hundred,HundredPlus";
            this.Where = "";
        }
    }
    [Serializable]
    public class GiftSearch : Search
    {
        public GiftSearch()
        {
            this.PageIndex = 1;
            this.PageSize = 30;
            this.PrimaryKey = "[ID]";
            this.Table = "Gift";
            this.Fields = "g.ID,g.Name,Amount,[Type],[State],Bean";
            this.Where = "";
        }
    }
    [Serializable]
    public class UserAwardSearch : Search
    {
        public UserAwardSearch()
        {
            this.PageIndex = 1;
            this.PageSize = 30;
            this.PrimaryKey = "[ID]";
            this.Table = "UserAward";
            this.Fields = "[ID],Account,NickName,PrizeName,Num,GetPrizeTime,HonoreeTime,SendTime,[State],[Receive],[Source],[PrizeId],[ReceiveTime],[AuditTime]";
            this.Where = "";
        }
    }
    [Serializable]
    public class QmallRecordSearch : Search
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 商品名称id集合
        /// </summary>
        public object[] Product_Id { get; set; }
        /// <summary>
        /// 兑换状态
        /// </summary>
        public long status { get; set; }
    }
    [Serializable]
    public class ExtendChannelSearch : Search
    {
        public ExtendChannelSearch()
        {
            this.PageIndex = 1;
            this.PageSize = 20;
            this.PrimaryKey = "[Day]";
            this.Table = "NewExtendChannelReport";
            this.Fields = "[Day],[Channel],[ChannelNum],[PCLoad],[AndroidLoad],[iOSLoad],[LoadTimeAvg],[Stay],[PCDown],[AndroidDown],[iOSDown],[PCFirstActive],[AndroidFirstActive],[iOSFirstActive],[SecondDown],[SecondDownTimeAvg],Register,NetWifi,NetMobileData";
            this.Where = " 1=1 ";
        }
    }
    [Serializable]
    public class NewQmallReportSearch : Search
    {
        public NewQmallReportSearch()
        {
            this.PageIndex = 1;
            this.PageSize = 20;
            this.PrimaryKey = "[Day]";
            this.Table = "NewQmallReport";
            this.Fields = "[Day],Product,SellNum,ChannelId";
            this.Where = " 1=1 ";
        }
    }
    public class NewADIDReportSearch : Search
    {
        public NewADIDReportSearch()
        {
            this.PageIndex = 1;
            this.PageSize = 20;
            this.PrimaryKey = "[Day]";
            this.Table = "NewADIDReport";
            this.Fields = "[day],[ADID],[Num],[ChannelId],[created],[modified]";
            this.Where = " 1=1 ";
        }
    }
    public class NewBaiduAdReportSearch : Search
    {
        public NewBaiduAdReportSearch()
        {
            this.PageIndex = 1;
            this.PageSize = 20;
            this.PrimaryKey = "[Day]";
            this.Table = "NewBaiduAdReport";
            this.Fields = " [day],[LoginNum],[ActUserNum],[OneDayLeft],[ThreeDayLeft],[SevenDayLeft],[ChannelId],[Created],[Modified],[regNum],[ADID]";
            this.Where = " 1=1 ";
        }
    }

    [Serializable]
    public class CPSUsersSearch : Search
    {
        public CPSUsersSearch()
        {
            this.PageIndex = 1;
            this.PageSize = 20;
            this.PrimaryKey = "id";
            this.Table = "CPSUsers";
            this.Fields = "id,channel,channel_name,channel_num,idnum,device,bank_name,bank_addr,bank_acc,business_link,telephone,email,qq,[percent],protocol,admin_id";
            this.Where = "";
        }
    }
    [Serializable]
    public class ClubStatisticSearch : Search
    {
        public ClubStatisticSearch()
        {
            this.PageIndex = 1;
            this.PageSize = 30;
            this.PrimaryKey = "[ID]";
            this.Table = "ClubStatistic";
            this.Fields = "[ID], AdminAcc,ClubIdsInfo,EditTime";
            this.Where = " 1=1 ";
        }
    }

}
