using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;


using System.Linq;


namespace MF.Data
{
    /// <summary>
    /// 每日充值报表实体类
    /// </summary>
    [Serializable]
    public partial class NewChargeReport
    {
        /// <summary>
        /// 天数，以2012年为基准记录
        /// </summary>
        public int Day { get; set; }
        /// <summary>
        /// 提交订单数量
        /// </summary>
        public int SubmitNum { get; set; }
        /// <summary>
        /// 提交金额
        /// </summary>
        public decimal SubmitMoney { get; set; }
        /// <summary>
        /// 支付订单数量
        /// </summary>
        public int PayNum { get; set; }
        /// <summary>
        /// 支付金额
        /// </summary>
        public decimal PayMoney { get; set; }
        /// <summary>
        /// 提交订单账户数量
        /// </summary>
        public int SubmitUserNum { get; set; }
        /// <summary>
        /// 首次提交订单账户数量
        /// </summary>
        public int FirstSubmitUserNum { get; set; }
        /// <summary>
        /// 渠道号
        /// </summary>
        public string ChannelId { get; set; }
        /// <summary>
        /// 支付渠道号
        ///   10: "支付宝(官网)"   11: "支付宝(App)";
        ///    40:"微信(官网)";   41: "微信(App)";
        ///   3:"苹果";
        /// </summary>
        public int PayChannel { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? Created { get; set; }
        /// <summary>
        /// 每⽇日充值额度的分布：（根据充值价格和数量) 5元-
        /// </summary>
        public int? PayNum_5 { get; set; }
        /// <summary>
        /// 每⽇日充值额度的分布：（根据充值价格和数量) 5元-10
        /// </summary>
        public int? PayNum_5_10 { get; set; }
        /// <summary>
        /// 每⽇日充值额度的分布：（根据充值价格和数量) 10元-20
        /// </summary>
        public int? PayNum_10_20 { get; set; }
        /// <summary>
        /// 每⽇日充值额度的分布：（根据充值价格和数量) 20元-50
        /// </summary>
        public int? PayNum_20_50 { get; set; }
        /// <summary>
        /// 每⽇日充值额度的分布：（根据充值价格和数量) 50元-100
        /// </summary>
        public int? PayNum_50_100 { get; set; }
        /// <summary>
        /// 每⽇日充值额度的分布：（根据充值价格和数量) 100+
        /// </summary>
        public int? PayNum_100 { get; set; }

    }
    /// <summary>
    /// 每日充值报表实体类
    /// </summary>
    [Serializable]
    public partial class NewChannelChargeReport
    {
        /// <summary>
        /// 天数，以2012年为基准记录
        /// </summary>
        public int Day { get; set; }
        /// <summary>
        /// 2255平台提交订单数量
        /// </summary>
        public int SubmitNum { get; set; }
        /// <summary>
        /// 2255平台提交金额
        /// </summary>
        public decimal SubmitMoney { get; set; }
        /// <summary>
        /// 2255平台支付订单数量
        /// </summary>
        public int PayNum { get; set; }
        /// <summary>
        /// 2255平台支付金额
        /// </summary>
        public decimal PayMoney { get; set; }

        /// <summary>
        /// 支付宝支付金额
        /// </summary>
        public decimal AlipayPayMoney { get; set; }
        /// <summary>
        /// 微信支付金额
        /// </summary>
        public decimal WeixinPayMoney { get; set; }
        /// <summary>
        /// 苹果支付金额
        /// </summary>
        public decimal IosPayMoney { get; set; }
        /// <summary>
        /// 大额充值金额
        /// </summary>
        public decimal MaxPayMoney { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ChannelId { get; set; }


        //暂时取消渠道统计2020-01-02
        /// <summary>
        /// 渠道提交订单数量
        /// </summary>
        public int ChannelSubmitNum { get; set; }
        /// <summary>
        /// 渠道提交金额
        /// </summary>
        public decimal ChannelSubmitMoney { get; set; }
        /// <summary>
        /// 渠道支付订单数量
        /// </summary>
        public int ChannelPayNum { get; set; }
        /// <summary>
        /// 渠道支付金额
        /// </summary>
        public decimal ChannelPayMoney { get; set; }
    }
    /// <summary>
    /// 每日元宝报表实体类
    /// </summary>
    /// <summary>
    /// NewCurrencyReport:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class NewCurrencyReport
    {
        /// <summary>
        /// 
        /// </summary>
        public int Day { get; set; }
        /// <summary>
        /// 每⽇日剩余元宝总数量（含保险箱，⼦子账号）
        /// </summary>
        public long? LeftCurrency { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long? TakeCurrency { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long? StrongBoxCurrency { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long? StrongBoxCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long? StrongBoxCreated { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long? StrongBoxDistory { get; set; }
        /// <summary>
        /// 每⽇日免费产出元宝数 领取救济
        /// </summary>
        public long? ReliefCurrency { get; set; }
        /// <summary>
        /// 每⽇日免费产出元宝数 系统赠送
        /// </summary>
        public long? SystemDeliveryCurrency { get; set; }
        /// <summary>
        /// 每⽇日免费产出元宝数 免费比赛
        /// </summary>
        public long? FreeGameCurrency { get; set; }
        /// <summary>
        /// 每⽇日商城兑换元宝的数
        /// </summary>
        public long? ExchangeCurrency { get; set; }
        /// <summary>
        /// 每⽇日管理员发放
        /// </summary>
        public long? AdminCurrency { get; set; }
        /// <summary>
        /// 每⽇日购买房卡的数
        /// </summary>
        public long? BuyRoomCurrency { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? Created { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? Modified { get; set; }
        /// <summary>
        /// 渠道号
        /// </summary>
        public string ChannelId { get; set; }
        /// <summary>
        /// 每日定时赛实际发元宝总金额
        /// </summary>
        public long? TimingCurrency { get; set; }
        /// <summary>
        /// 每⽇定时赛复活费收入
        /// </summary>
        public long? ResurrectionCurrency { get; set; }
        /// <summary>
        /// 官方充值数量
        /// </summary>
        public long? ChargeCurrency { get; set; }
        /// <summary>
        /// 货币消耗数量=前一日的剩余元宝数量+当日充值数量-当日剩余元宝数量
        /// </summary>
        public long? LastLeftCurrency { get; set; }
    }
    /// <summary>
    /// 注册报表实体类
    /// </summary>
    [Serializable]
    public partial class NewRegReport
    {
        /// <summary>
        /// 和2012年比较的天数
        /// </summary>
        public int? Day { get; set; }
        /// <summary>
        /// 新增加用户数量
        /// </summary>
        public int? NewUser { get; set; }
        /// <summary>
        /// 新游客
        /// </summary>
        public int? NewVisitor { get; set; }
        /// <summary>
        /// 客户端注册人数
        /// </summary>
        public int? ClientUser { get; set; }
        /// <summary>
        /// Iphone注册
        /// </summary>
        public int? IphoneUser { get; set; }
        /// <summary>
        /// Android用户
        /// </summary>
        public int? AndroidUser { get; set; }
        /// <summary>
        /// Ipad用户数量
        /// </summary>
        public int? IpadUser { get; set; }
        /// <summary>
        /// 游客转正数量
        /// </summary>
        public int? TouristToUser { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? SubAccTotal { get; set; }
        /// <summary>
        /// 总用户数量
        /// </summary>
        public int? AccTotal { get; set; }
        /// <summary>
        /// 渠道号
        /// </summary>
        public string ChannelId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? Created { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? modified { get; set; }
        /// <summary>
        /// 救济数量
        /// </summary>
        public int? Relief { get; set; }
        /// <summary>
        /// 微信注册人数
        /// </summary>
        public int? WeixinUser { get; set; }
    }
    /// <summary>
    /// 金豆报表实体类
    /// </summary>
    [Serializable]
    public partial class NewBeanReport
    {
        /// <summary>
        /// 
        /// </summary>
        public int Day { get; set; }
        /// <summary>
        /// 每日剩余金豆总数
        /// </summary>
        public long? LeftBean { get; set; }
        /// <summary>
        /// 每日金豆消耗总数
        /// </summary>
        public long? ShopExchangeBean { get; set; }
        /// <summary>
        /// 每日定时赛实际发金豆总金额
        /// </summary>
        public long? TimingBean { get; set; }
        /// <summary>
        /// 每日管理员发放
        /// </summary>
        public long? AdminBean { get; set; }
        /// <summary>
        /// 渠道编码
        /// </summary>
        public string ChannelId { get; set; }
        /// <summary>
        /// 话费赛产出
        /// </summary>
        public long? TelephoneFareBean { get; set; }
        //对外【京东、淘宝、话费】 100【昨天】  200【今天】
    }

    /// <summary>
    /// 每日欢乐卡报表实体类
    /// </summary>
    /// <summary>
    /// NewCurrencyReport:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class NewHappyRecordReport
    {
        /// <summary>
        /// 
        /// </summary>
        public int Day { get; set; }
        /// <summary>
        /// 每⽇日剩余欢乐卡总数量（含保险箱，⼦子账号）
        /// </summary>
        public long? LeftHappy { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long? ChargeHappy { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long? AdminHappy { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long? GameUseHappy { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long? GameReturnHappy { get; set; }
        /// <summary>
        /// 渠道号
        /// </summary>
        public string ChannelId { get; set; }

    }
    /// <summary>
    /// 游戏数据日报表实体类
    /// </summary>
    [Serializable]
    public class GameReport
    {
        /// <summary>
        /// 
        /// </summary>
        public int Day { get; set; }
        /// <summary>
        /// 获取或设置游戏ID
        /// </summary>
        public int GameId { get; set; }
        /// <summary>
        /// 获取或设置游戏场ID
        /// </summary>
        public int MatchId { get; set; }
        /// <summary>
        /// 获取或设置指定游戏的活跃人数
        /// </summary>
        public int Actives { get; set; }
        /// <summary>
        /// 获取或设置指定游戏每日的赢数量
        /// </summary>
        public long Win { get; set; }
        /// <summary>
        /// 获取或设置指定游戏每日的输数量
        /// </summary>
        public long Lose { get; set; }
        /// <summary>
        /// 获取或设置指定游戏收取的服务费
        /// </summary>
        public long Shrink { get; set; }
        /// <summary>
        /// 游戏ruleid
        /// </summary>
        public long RuleId { get; set; }
        /// <summary>
        /// 渠道id号
        /// </summary>
        public string ChannelId { get; set; }
    }
    /// <summary>
    ///  推广渠道报表
    /// </summary>
    [Serializable]
    public partial class NewAdReport
    {
        /// <summary>
        /// 和2012年比较的天数
        /// </summary>
        public int? Day { get; set; }
        /// <summary>
        /// 每⽇日登录⽤用户数量（包含新增注册，需要去重，去除子账号数据）
        /// </summary>
        public int? LoginNum { get; set; }
        /// <summary>
        /// 峰值在线⼈人数。现在没有哈
        /// </summary>
        public int? MaxNum { get; set; }
        /// <summary>
        /// ⽇活跃⽤用户数量
        /// </summary>
        public int? ActUserNum { get; set; }
        /// <summary>
        /// 游戏场活跃⽤用户数量
        /// </summary>
        public int? ActMatchsUserNum { get; set; }
        /// <summary>
        /// 新增用户活跃
        /// </summary>
        public int? RegMatchsNum { get; set; }
        /// <summary>
        /// 次日留存
        /// </summary>
        public int? _1Day { get; set; }
        /// <summary>
        /// 3日留存
        /// </summary>
        public int? _3Day { get; set; }
        /// <summary>
        /// 7日留存的人数
        /// </summary>
        public int? _7Day { get; set; }
        /// <summary>
        /// 【指定游戏】次日留存
        /// </summary>
        public int? M_1Day { get; set; }
        /// <summary>
        /// 【指定游戏】3日留存
        /// </summary>
        public int? M_3Day { get; set; }
        /// <summary>
        /// 【指定游戏】4日留存的人数
        /// </summary>
        public int? M_4Day { get; set; }
        /// <summary>
        /// 【指定游戏】5日留存的人数
        /// </summary>
        public int? M_5Day { get; set; }
        /// <summary>
        /// 【指定游戏】6日留存的人数
        /// </summary>
        public int? M_6Day { get; set; }
        /// <summary>
        /// 【指定游戏】7日留存的人数
        /// </summary>
        public int? M_7Day { get; set; }
        /// <summary>
        /// 【指定游戏】15日留存的人数
        /// </summary>
        public int? M_15Day { get; set; }
        /// <summary>
        /// 【指定游戏】30日留存的人数
        /// </summary>
        public int? M_30Day { get; set; }
        /// <summary>
        /// 【非指定游戏】次日留存
        /// </summary>
        public int? NNM_1Day { get; set; }
        /// <summary>
        /// 【非指定游戏】3日留存
        /// </summary>
        public int? NNM_3Day { get; set; }
        /// <summary>
        /// 【非指定游戏】4日留存的人数
        /// </summary>
        public int? NNM_4Day { get; set; }
        /// <summary>
        /// 【非指定游戏】5日留存的人数
        /// </summary>
        public int? NNM_5Day { get; set; }
        /// <summary>
        /// 【非指定游戏】6日留存的人数
        /// </summary>
        public int? NNM_6Day { get; set; }
        /// <summary>
        /// 【非指定游戏】7日留存的人数
        /// </summary>
        public int? NNM_7Day { get; set; }
        /// <summary>
        /// 【非指定游戏】15日留存的人数
        /// </summary>
        public int? NNM_15Day { get; set; }
        /// <summary>
        /// 【非指定游戏】30日留存的人数
        /// </summary>
        public int? NNM_30Day { get; set; }
        /// <summary>
        /// 两次游戏时间间隔超过15天人数
        /// </summary>
        public int? GAP_15 { get; set; }
        
        /// <summary>
        /// 总充值数量/活跃⽤用户
        /// </summary>
        public string ARPU { get; set; }
        /// <summary>
        /// 付费⽤用户数/登录⽤用户数（去重）
        /// </summary>
        public string PayRate { get; set; }
        /// <summary>
        /// 总充值数量/付费⽤用户数
        /// </summary>
        public string ARPPU { get; set; }
        /// <summary>
        /// 每⽇日的付费率 总充值⼈人数/活跃⽤用户
        /// </summary>
        public string ChargeUserRate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ChannelId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? Created { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? Modified { get; set; }
        /// <summary>
        /// 新增数量=新增人数+游客人数
        /// </summary>
        public int? RegNum { get; set; }
        //元宝消耗数量
        public long? TakeCurrency { get; set; }
    }
    [Serializable]
    public class NewQmallReport
    {
        /// <summary>
        /// 
        /// </summary>
        public int Day { get; set; }
        /// <summary>
        /// 奖品ID
        /// </summary>
        public int Product { get; set; }
        /// <summary>
        /// 兑换数量
        /// </summary>
        public int SellNum { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ChannelId { get; set; }
    }
        [Serializable]
    public class NewQmallReportCount {
        /// <summary>
        /// 
        /// </summary>
        public int Day { get; set; }
        /// <summary>
        /// 实物总和
        /// </summary>
        public long? KindSum { get; set; }
        /// <summary>
        /// 10元话费
        /// </summary>
        public long? TenMobile { get; set; }
        /// <summary>
        /// 20元话费
        /// </summary>
        public long? TwentyMobile { get; set; }
        /// <summary>
        /// 50元话费
        /// </summary>
        public long? FiftyMobile { get; set; }
        /// <summary>
        /// 100元话费
        /// </summary>
        public long? HundredMobile { get; set; }
        /// <summary>
        /// 5万元宝
        /// </summary>
        public long? FiveCurrency { get; set; }
        /// <summary>
        /// 10万元宝
        /// </summary>
        public long? TenCurrency { get; set; }
        /// <summary>
        /// 20万元宝
        /// </summary>
        public long? TwentyCurrency { get; set; }
        /// <summary>
        /// 50万元宝
        /// </summary>
        public long? FiftyCurrency { get; set; }
        /// <summary>
        /// 100万元宝
        /// </summary>
        public long? HundredCurrency { get; set; }
        /// <summary>
        /// 200万元宝
        /// </summary>
        public long? TwoHundredCurrency { get; set; }
        /// <summary>
        /// 20张房卡
        /// </summary>
        public long? TwentyRoomCard { get; set; }
        /// <summary>
        /// 100张房卡
        /// </summary>
        public long? HundredRoomCard { get; set; }
        /// <summary>
        /// 10元京东卡
        /// </summary>
        public long? TenJingdong { get; set; }
        /// <summary>
        /// 50元京东卡
        /// </summary>
        public long? FiftyJingdong { get; set; }
        /// <summary>
        /// 100元京东卡
        /// </summary>
        public long? HundredJingdong { get; set; }
        /// <summary>
        /// 200元京东卡
        /// </summary>
        public long? TwoHundredJingdong { get; set; }
        /// <summary>
        /// 500元京东卡
        /// </summary>
        public long? FiveHundredJingdong { get; set; }
        /// <summary>
        /// 800元京东卡
        /// </summary>
        public long? EightHundredJingdong { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ChannelId { get; set; }
    }
    /// <summary>
	/// NewADIDReport:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
    public partial class NewADIDReport
    {
        public NewADIDReport()
        { }
        #region Model
        private int? _day;
        private string _adid;
        private int? _num;
        private string _channelid;
        private DateTime? _created;
        private DateTime? _modified;
        /// <summary>
        /// 
        /// </summary>
        public int? day
        {
            set { _day = value; }
            get { return _day; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ADID
        {
            set { _adid = value; }
            get { return _adid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Num
        {
            set { _num = value; }
            get { return _num; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ChannelId
        {
            set { _channelid = value; }
            get { return _channelid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? created
        {
            set { _created = value; }
            get { return _created; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? modified
        {
            set { _modified = value; }
            get { return _modified; }
        }
        #endregion Model

    }
    /// <summary>
	/// NewBaiduAdReport:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
    public partial class NewBaiduAdReport
    {
        public NewBaiduAdReport()
        { }
        #region Model
        private int? _day;
        private int? _loginnum = 0;
        private int? _actusernum = 0;
        private int? _onedayleft = 0;
        private int? _threedayleft = 0;
        private int? _sevendayleft;
        private string _channelid;
        private DateTime? _created;
        private DateTime? _modified;
        private int? _regnum;
        private string _adid;
        /// <summary>
        /// 和2012年比较的天数
        /// </summary>
        public int? day
        {
            set { _day = value; }
            get { return _day; }
        }
        /// <summary>
        /// 每⽇日登录⽤用户数量（包含新增注册，需要去重，去除子账号数据）
        /// </summary>
        public int? LoginNum
        {
            set { _loginnum = value; }
            get { return _loginnum; }
        }
        /// <summary>
        /// ⽇活跃⽤用户数量
        /// </summary>
        public int? ActUserNum
        {
            set { _actusernum = value; }
            get { return _actusernum; }
        }
        /// <summary>
        /// 次日留存
        /// </summary>
        public int? OneDayLeft
        {
            set { _onedayleft = value; }
            get { return _onedayleft; }
        }
        /// <summary>
        /// 3日留存
        /// </summary>
        public int? ThreeDayLeft
        {
            set { _threedayleft = value; }
            get { return _threedayleft; }
        }
        /// <summary>
        /// 7日留存的人数
        /// </summary>
        public int? SevenDayLeft
        {
            set { _sevendayleft = value; }
            get { return _sevendayleft; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ChannelId
        {
            set { _channelid = value; }
            get { return _channelid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? Created
        {
            set { _created = value; }
            get { return _created; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? Modified
        {
            set { _modified = value; }
            get { return _modified; }
        }
        /// <summary>
        /// 每日注册用户数量
        /// </summary>
        public int? regNum
        {
            set { _regnum = value; }
            get { return _regnum; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ADID
        {
            set { _adid = value; }
            get { return _adid; }
        }
        #endregion Model
    }
    /// <summary>
    ///  推广渠道报表
    /// </summary>
    [Serializable]
    public partial class NewPromotChargeReport
    {
        /// <summary>
        /// 和2012年比较的天数
        /// </summary>
        public int? Day { get; set; }
        /// <summary>
        /// 每⽇日登录⽤用户数量（包含新增注册，需要去重，去除子账号数据）
        /// </summary>
        public int? LoginNum { get; set; }
        /// <summary>
        /// 峰值在线⼈人数。现在没有哈
        /// </summary>
        public int? MaxNum { get; set; }
        /// <summary>
        /// ⽇活跃⽤用户数量
        /// </summary>
        public int? ActUserNum { get; set; }
        /// <summary>
        /// 次日留存
        /// </summary>
        public int? OneDayLeft { get; set; }
        /// <summary>
        /// 3日留存
        /// </summary>
        public int? ThreeDayLeft { get; set; }
        /// <summary>
        /// 7日留存的人数
        /// </summary>
        public int? SevenDayLeft { get; set; }
        /// <summary>
        /// 总充值数量/活跃⽤用户
        /// </summary>
        public string ARPU { get; set; }
        /// <summary>
        /// 付费⽤用户数/登录⽤用户数（去重）
        /// </summary>
        public string PayRate { get; set; }
        /// <summary>
        /// 总充值数量/付费⽤用户数
        /// </summary>
        public string ARPPU { get; set; }
        /// <summary>
        /// 每⽇日的付费率 总充值⼈人数/活跃⽤用户
        /// </summary>
        public string ChargeUserRate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ChannelId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? Created { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? Modified { get; set; }
        /// <summary>
        /// 新增数量=新增人数+游客人数
        /// </summary>
        public int? RegNum { get; set; }

        /// <summary>
        /// 提交订单数量
        /// </summary>
        public int SubmitNum { get; set; }
        /// <summary>
        /// 提交金额
        /// </summary>
        public decimal SubmitMoney { get; set; }
        /// <summary>
        /// 支付订单数量
        /// </summary>
        public int PayNum { get; set; }
        /// <summary>
        /// 支付金额
        /// </summary>
        public decimal PayMoney { get; set; }
    }
    /// <summary>
    /// 交易平台报表
    /// </summary>
    [Serializable]
    public class UserDayReports : IComparable<UserDayReports>
    {
        public int CompareTo(UserDayReports other)
         {
             return other.Start.CompareTo(this.Start); // 年龄升序
         }
    /// <summary>
    /// 
    /// </summary>
    public int ID { get; set; }
        /// <summary>
        /// 区域
        /// </summary>
        public int Region { get; set; }
        /// <summary>
        /// 玩家数
        /// </summary>
        public long NewPlayerCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long NewOrderCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long NewCompleteOrderCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long NewSaleCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long NewCompleteSaleCount { get; set; }
        /// <summary>
        /// 出售订单收的税
        /// </summary>
        public long NewTax { get; set; }
        /// <summary>
        /// VIP总个数
        /// </summary>
        public long VIPCount { get; set; }
        /// <summary>
        /// VIP支付的金额
        /// </summary>
        public long VIPMoney { get; set; }
        /// <summary>
        /// 出售订单金额
        /// </summary>
        public long AllSaleMoney { get; set; }
        /// <summary>
        /// 出售订单个数
        /// </summary>
        public long AllSaleCount { get; set; }
        /// <summary>
        /// 完成订单金额
        /// </summary>
        public long AllOrderMoney { get; set; }
        /// <summary>
        ///  完成订单个数
        /// </summary>
        public long AllOrderCount { get; set; }
        /// <summary>
        /// 用户总余额
        /// </summary>
        public long AllBalance { get; set; }
        /// <summary>
        /// 卡支付总额
        /// </summary>
        public long AllCardPay { get; set; }
        /// <summary>
        /// 卡支付个数
        /// </summary>
        public long AllCardPayCount { get; set; }
        /// <summary>
        /// 微信支付总额
        /// </summary>
        public long AllWechatPay { get; set; }
        /// <summary>
        /// 微信支付个数
        /// </summary>
        public long AllWechatPayCount { get; set; }
        /// <summary>
        /// 余额支付总额
        /// </summary>
        public long AllBalancePay { get; set; }
        /// <summary>
        /// 余额支付总个数
        /// </summary>
        public long AllBalancePayCount { get; set; }
        /// <summary>
        /// 提现总额
        /// </summary>
        public long AllTransferCash { get; set; }
        /// <summary>
        /// 提现成功手续费
        /// </summary>
        public long TransferFeeSuccess { get; set; }
        /// <summary>
        /// 提现失败手续费
        /// </summary>
        public long TransferFeeFailed { get; set; }
        /// <summary>
        /// 提现个数
        /// </summary>
        public long AllTransferCount { get; set; }
        /// <summary>
        /// 提现成功个数
        /// </summary>
        public long TransferSuccessCount { get; set; }
        /// <summary>
        ///  提现回调失败个数 扣手续费
        /// </summary>
        public long TransferFailedCount { get; set; }
        /// <summary>
        /// 提现提交失败个数 不扣手续费
        /// </summary>
        public long TransferErrorCount { get; set; }
        /// <summary>
        /// 卡支付道具个数
        /// </summary>
        public long AllCardPayProduct { get; set; }
        /// <summary>
        /// 微信支付道具个数
        /// </summary>
        public long AllWechatProduct { get; set; }
        /// <summary>
        /// 余额支付道具个数
        /// </summary>
        public long AllBalanceProduct { get; set; }
        /// <summary>
        /// 出售订单道具个数
        /// </summary>
        public long AllSaleProduct { get; set; }
        /// <summary>
        /// 统计开始时间
        /// </summary>
        public string Start { get; set; }
        /// <summary>
        /// 统计结束时间
        /// </summary>
        public DateTime End { get; set; }
        /// <summary>
        /// 银联支付 支付给易支付费用【0.46%】
        /// </summary>
        public long AllCardPayFee { get; set; }
        /// <summary>
        ///  微信支付 支付给易支付费用【0.75%】
        /// </summary>
        public long AllWechatPayFee { get; set; }
         
    }

    [Serializable]
    public class ZywyRegion
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
    /*交易平台后台数据*/
    [Serializable]
    public class ZywyRes<T>
    {
        public int code { get; set; }
        public T data { get; set; }
        public string msg { get; set; }
    }
    [Serializable]
    public class ZywyStatic
    {
        public string Key { get; set; }
        //随机数
        public string Index { get; set; }
        public int Time { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public int Region { get; set; }
    } /// <summary>
      /// 输出到json字符串时，属性名称按照字典顺序排序输出
      /// </summary>
    public class PropertySortResolver : DefaultContractResolver
    {
        /// <summary>
        /// 属性名称按照字典顺序排序输出
        /// </summary>
        /// <param name="type"></param>
        /// <param name="memberSerialization"></param>
        /// <returns></returns>
        protected override IList<JsonProperty> CreateProperties(Type type,
                MemberSerialization memberSerialization)
        {
            IList<JsonProperty> list = base.CreateProperties(type, memberSerialization);
            return list.OrderBy(a => a.PropertyName).ToList();
        }
    }


}
