using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MF.ConsoleApp
{
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
        //public DBSource DBSource { get; set; }
        ///// <summary>
        ///// 查询数据库名称
        ///// </summary>
        //public DBName DBName { get; set; }
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
    [Serializable]
    public class CurrencyRecord
    {
        public int ID { get; set; }
        public long Time { get; set; }
        public string Account { get; set; }
        /// <summary>
        /// 1："报名",
        /// 2："复活",
        /// 3："VIP每日赠送",
        /// 4："IU币兑换",
        /// 5："获胜",
        /// 6："退赛",
        /// 7："兑换战斗力",
        /// 8："邮箱提取",
        /// 9："商城兑换",
        /// 10:"注册赠送",
        /// 11:"推广现金兑换",
        /// 12:"艾普积分兑换",
        /// 13:"开推广礼包",
        /// 14:"投诉保证金",
        /// 15:"战斗力回兑",
        /// 16:"领取救济金",
        /// 17:"保险箱手续费",
        /// 18:"存入保险箱",
        /// 19:"保险箱取出"
        /// 20:"恢复执照分"
        /// 21:"开红包获奖(同时记录扣与加)" 
        /// 22:首次绑定手机送10000元宝
        /// 23:俱乐部任务完成+2000
        /// 24:购买房卡
        /// 25：系统增送
        /// 26:银票兑换
        /// </summary>
        public long Type { get; set; }
        public long Num { get; set; }
        public string IP { get; set; }
        public long MatchId { get; set; }
        public long GameId { get; set; }
        public long RuleId { get; set; }
        public long Original { get; set; }

    }
    [Serializable]
    public class GameBlackUserInfo
    {
        public int Id { get; set; }
        /// <summary>
        ///  游戏场Id
        /// </summary>
        public string GameId { get; set; }
        /// <summary>
        /// 游戏账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        ///   值
        /// </summary>
        public string Value { get; set; }
        public string ChargeId { get; set; }



        /// <summary>
        /// String 挡位  LOW,MIDDLE,HIGH
        /// </summary>
        public string Level { get; set; }
        /// <summary>
        ///    创建日期	2012-10-1的时间戳(可空)
        /// </summary>
        public int? CreateDate { get; set; }
        /// <summary>
        ///   最后的更改日期	2012-10-1的时间戳(可空)
        /// </summary>
        public int? LastUpdateDate { get; set; }

        /// <summary>
        ///  审核标(YES/NO) YES:审核的,NO:未审核
        /// </summary>
        public string Audit { get; set; }
        /// <summary>
        ///  备注
        /// </summary>
        public string Remark { get; set; }


        public string NickName { get; set; }
        public string ClubId { get; set; }
        public string Money { get; set; }
        public string Guid { get; set; }
    }
    [Serializable]
    public class Result<T>
    {
        public int Code { get; set; }
        public T R { get; set; }
        public string Message { get; set; }
        public Exception Ex { get; set; }
    } 
    public class Data
    {
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
}
