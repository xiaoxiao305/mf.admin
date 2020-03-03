using System;

namespace MF.Data
{
    [Serializable]
    public class CacheUser2
    {
        public CacheUser2() { }
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
        public int Regitime { get; set; }
        /// <summary>
        ///用户最后一次登录ip
        /// </summary>
        public string LastLoginIp { get; set; }
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
    }
}
