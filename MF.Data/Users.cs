using System;

namespace MF.Data
{
    [Serializable]
    public class CacheUser
    {
        public CacheUser() { }
        /// <summary>
        /// 获取或设置用户的唯一ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 获取或设置用户账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 获取或设置用户昵称
        /// </summary>
        public string Nickname { get; set; }
        /// <summary>
        /// 用户UID对应数据库ChargeId
        /// </summary>
        public string ChargeId { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        public int RegTime { get; set; }
    }
    /// <summary>
    /// 用户数据实体类
    /// </summary>
    [Serializable]
    public class Users
    {
        public Users()
        { }
        /// <summary>
        /// 获取或设置用户的唯一ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 获取或设置用户账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 获取或设置用户昵称
        /// </summary>
        public string Nickname { get; set; }
        /// <summary>
        /// 获取或设置用户真实姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 获取或设置用户身份证号码
        /// </summary>
        public string Identity { get; set; }
        /// <summary>
        /// 获取或设置用户性别
        /// </summary>
        public int Sex { get; set; }
        /// <summary>
        /// 获取或设置用户状态
        /// </summary>
        public int Flag { get; set; }
        /// <summary>
        /// 获取或设置用户绑定的手机
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 获取或设置用户绑定的手机安全令Key
        /// </summary>
        public string PhoneKey { get; set; }
        /// <summary>
        /// 获取或设置用户锁定状态
        /// </summary>
        public int Lock { get; set; }
        /// <summary>
        /// 获取或设置用户今日领取的救济次数
        /// </summary>
        public int Relief { get; set; }
        /// <summary>
        /// 获取或设置用户设备号（APP）
        /// </summary>
        public string GUID { get; set; }
        /// <summary>
        /// 获取或设置用户等级
        /// </summary>
        public int Lv { get; set; }
        /// <summary>
        /// 获取或设置用户经验
        /// </summary>
        public int Exp { get; set; }
        /// <summary>
        /// 获取或设置用户游客标识
        /// </summary>
        public int Guest { get; set; }
        /// <summary>
        /// 用户UID对应数据库ChargeId
        /// </summary>
        public string ChargeId { get; set; }
        /// <summary>
        /// 获取或设置用户的推广渠道ID
        /// </summary>
        public string ADID { get; set; }
        /// <summary>
        /// 获取或设置用户的注册时间
        /// </summary>
        public int Regitime { get; set; }
        /// <summary>
        /// 获取或设置用户的注册IP
        /// </summary>
        public string RegistIp { get; set; }
        /// <summary>
        /// 获取或设置用户的注册终端编号
        /// </summary>
        public int RegistDevice { get; set; }
        /// <summary>
        /// 获取或设置用户的注册区域
        /// </summary>
        public string RegistArea { get; set; }
        /// <summary>
        /// 获取或设置用户上一次登录游戏的IP
        /// </summary>
        public string LastIp { get; set; }
        /// <summary>
        /// 获取或设置用户上一次登录游戏的时间
        /// </summary>
        public int LastLogin { get; set; }
        /// <summary>
        /// 获取或设置用户上一次游戏登录的终端编号
        /// </summary>
        public int LoginDevice { get; set; }
        /// <summary>
        /// 获取或设置用户的游戏登录次数
        /// </summary>
        public int LoginCount { get; set; }
        /// <summary>
        /// 获取或设置的用户设备号
        /// </summary>
        public string DeviceCode { get; set; }
        /// <summary>
        /// 获取或设置用户元宝数量
        /// </summary>
        public long Currency { get; set; }
        /// <summary>
        /// 获取或设置用户金豆数量
        /// </summary>
        public long Bean { get; set; }
        /// <summary>
        /// 获取或设置用户房卡数量
        /// </summary>
        public int RoomCard { get; set; }
        /// <summary>
        /// 获取或设置当前用户的主账号
        /// </summary>
        public string Master { get; set; }
        /// <summary>
        /// 获取或设置用户欢乐卡数量
        /// </summary>
        public long Silver { get; set; }








        ///// <summary>
        ///// 获取或设置用户充值总额
        ///// </summary>
        //public int ChargeTotal
        //{ get; set; }
        ///// <summary>
        ///// 获取或设置用户金券数量
        ///// </summary>
        //public int Gold
        //{ get; set; }

        ///// <summary>
        ///// 获取或设置用户头像地址
        ///// </summary>
        //public string Icon
        //{ get; set; }
        ///// <summary>
        ///// 获取或设置用户第三方开放平台唯一ID
        ///// </summary>
        //public string OpenID
        //{ get; set; }
        ///// <summary>
        ///// 获取或设置用户第三方开放平台ID
        ///// </summary>
        //public int OpenPlatform
        //{ get; set; }
        ///// <summary>
        ///// 获取或设置用户绑定的邮箱
        ///// </summary>
        //public string Email
        //{ get; set; }
        ///// <summary>
        ///// 获取或设置用户个性签名
        ///// </summary>
        //public string Intro
        //{ get; set; }
        ///// <summary>
        ///// 获取或设置用户使用手机安全令的推送码
        ///// </summary>
        //public string PushCode
        //{ get; set; }
        ///// <summary>
        ///// 获取或设置用户游戏的在线时长
        ///// </summary>
        //public int OnlineTime
        //{ get; set; }
        ///// <summary>
        ///// 获取或设置用户的活动/任务数据
        ///// </summary>
        //public string Mission
        //{ get; set; }
        ///// <summary>
        ///// 获取或设置用户的月卡到期时间
        ///// </summary>
        //public int MonthCardOverdue { get; set; }
        ///// <summary>
        ///// 获取或设置用户领取月卡奖励的时间与次数
        ///// MonthCardReceive/10 为领取时间（精确到天）
        ///// MonthCardReceive/%10 为领取次数（最多9次）
        ///// </summary>
        //public int MonthCardReceive { set; get; }
        ///// <summary>
        ///// 获取或设置用户上一次活跃时间(2012-10-01到当前的天数)
        ///// </summary>
        //public int TodayActive { get; set; }
        ///// <summary>
        ///// 获取或设置是否AI账号
        ///// </summary>
        //public bool IsBot { get; set; }
    }
}
