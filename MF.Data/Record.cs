using System;
using Newtonsoft.Json;
using MF.Data.Converter;
using System.Collections.Generic;
using System.Linq;

namespace MF.Data
{
    [Serializable]
    public class SuggestGuild
    {
        public int guildid { get; set; }
        public int minval { get; set; }
        public int maxval { get; set; }
    }
    [Serializable]
    public class GameInfo
    {
        public int gameid { get; set; }
        public int matchid { get; set; }
        public string gamename { get; set; }
        public string matchname { get; set; }
    }
    [Serializable]
    public partial class LoginLog
    {
        /// <summary>
        /// 
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime LoginTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int LoginState { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Message { get; set; }
    }
    /// <summary>
    /// 操作日志
    /// </summary>
    [Serializable]
    public class SystemLog
    {
        /// <summary>
        /// 
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime OperTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Operation { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Page { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long? OprState { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long? Type { get; set; }
    }


    /// <summary>
    ///充值订单实体类
    /// </summary>
    [Serializable]
    public class ChargeRecord
    {
        /// <summary>
        /// 获取或设置充值订单的唯一ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 获取或设置充值订单的唯一订单号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 获取或设置充值订单的账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 获取或设置充值订单的提交时间
        /// </summary>
        public long CreateDate { get; set; }
        /// <summary>
        /// 获取或设置充值订单提交金额
        /// </summary>
        [JsonConverter(typeof(FloatConverter))]
        public float SumbitMoney { get; set; }
        /// <summary>
        /// 获取或设置充值订单的状态
        /// 0:等待支付,
        /// 1:支付成功
        /// 2:支付失败
        /// </summary>
        public long Flag { get; set; }
        /// <summary>
        /// 获取或设置充值订单的支付时间
        /// </summary>
        public long PayDate { get; set; }
        /// <summary>
        /// 获取或设置充值订单的支付金额
        /// </summary>
        [JsonConverter(typeof(FloatConverter))]
        public float PayMoney { get; set; }
        /// <summary>
        /// 获取或设置充值订单的支付平台交易号
        /// </summary>
        public string PlatformTransId { get; set; }
        /// <summary>
        /// 获取或设置充值订单的支付平台ID
        /// 0:官方，1:支付宝，2:易宝，3:苹果，4:微信，5:联营渠道
        /// </summary>
        public long PayChannel { get; set; }
        /// <summary>
        /// 获取或设置充值订单的联运渠道ID
        /// 此标识在联运渠道充值时必须填写
        /// </summary>
        public int Channel { get; set; }




        ///// <summary>
        ///// 获取或设置充值订单提交IP
        ///// </summary>
        //public string CreateIp { get; set; }
        ///// <summary>
        /////  获取或设置充值订单的支付方式
        /////  0:平台支付，1：官方卡密，2:神州行充值卡,3:骏网一卡通,4:电信卡，5:联通卡。。。
        ///// </summary>
        //public int PayMode { get; set; }
        ///// <summary>
        ///// 获取或设置生成订单的设备编号
        ///// 0:官网，1:web，2:client，3:iPad，4:Android，5:iPhone，6:Pad,7:手机官网
        ///// </summary>
        //public int Device { get; set; }
        ///// <summary>
        ///// 获取或设置充值订单活动类型
        ///// </summary>
        //public int MissionType { get; set; }
        ///// <summary>
        ///// 获取或设置此笔订单可以兑换的金券数量
        ///// </summary>
        //public int Gold { get; set; }

    }
    /// <summary>
    /// 俱乐部申请保证金实体类
    /// </summary>
    [Serializable]
    public class GuildApplyRecord
    {
        /// <summary>
        /// 获取或设置俱乐部保证金申请记录ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 获取或设置俱乐部保证金申请订单号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 获取或设置俱乐部保证金申请的账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 获取或设置俱乐部ID
        /// </summary>
        public int GuildID { get; set; }
        /// <summary>
        /// 获取或设置俱乐部名称
        /// </summary>
        public string GuildName { get; set; }
        /// <summary>
        /// 获取或设置俱乐部保证金状态
        /// 0 等待支付 1支付成功 2已创建俱乐部  3申请退款中  4已退款
        /// </summary>
        public long Flag { get; set; }
        /// <summary>
        /// 获取或设置俱乐部保证金提交时间
        /// </summary>
        public int CreateDate { get; set; }
        /// <summary>
        /// 获取或设置俱乐部保证金支付时间
        /// </summary>
        public int PayDate { get; set; }
        /// <summary>
        /// 获取或设置俱乐部保证金金额
        /// </summary>
        public int Money { get; set; }
        /// <summary>
        /// 获取或设置俱乐部保证金提交IP
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        /// 获取或设置俱乐部保证金平台交易号
        /// </summary>
        public string TransId { get; set; }
        /// <summary>
        /// 获取或设置俱乐部保证金申请退款时间
        /// </summary>
        public int ApplyRefundTime { get; set; }
        /// <summary>
        /// 获取或设置俱乐部保证金退款时间
        /// </summary>
        public int RefundTime { get; set; }
        /// <summary>
        /// 获取或设置俱乐部保证金退款人 
        /// </summary>
        public string RefundUser { get; set; }
        /// <summary>
        /// 获取或设置俱乐部保证金退款的支付宝账号
        /// </summary>
        public string AlipayAccount { get; set; }
        /// <summary>
        /// 获取或设置俱乐部保证金退款的支付宝持有人名称
        /// </summary>
        public string AlipayName { get; set; }
        /// <summary>
        /// 获取或设置俱乐部保证金退款的说明
        /// </summary>
        public string Memo { get; set; }
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
    public class StrongBoxRecord
    { 
         public int ID { get; set; }
        public long Date { get; set; }
        public string Account { get; set; }
        /// <summary>
        /// 保险箱号
        /// </summary>
        public string BoxId { get; set; }
        /// <summary>
        ///  1:创建 2：存入 3：取出 4：销毁 5：找回 
        /// </summary>
        public long Type { get; set; }
        public long Currency { get; set; }
        public string Memo { get; set; }
        public string ChargeId { get; set; } 
        public Users info { get; set; }
    }
    [Serializable]
    public class BeanRecord
    {
        public int ID { get; set; }
        public long Time { get; set; }
        public string Account { get; set; }
        /// <summary>
        /// 1:"比赛产出",5:"商城兑换", 6:"任务赠送",12:"单机赠送",8:"管理员发放"
        /// </summary>
        public long Type { get; set; }
        public long Num { get; set; }
        public string IP { get; set; }
        public long MatchId { get; set; }
        public long GameId { get; set; }
        public long Original { get; set; }
    }
    [Serializable]
    public class HappyCardRecord
    {
        public int ID { get; set; }
        public long Time { get; set; }
        public string Account { get; set; }
        /// <summary>
        /// 1:"报名（游戏消耗）"  6:"游戏退赛" 26:"兑换（充值赠送）"
        /// </summary>
        public long Type { get; set; }
        public long Num { get; set; }
        public string IP { get; set; }
        public long MatchId { get; set; }
        public long GameId { get; set; }
        public long Original { get; set; }
    }
    /// <summary>
    /// 兑换元宝记录
    /// </summary>
    [Serializable]
    public class QmallRecord
    {
        public int ID { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 用户UID
        /// </summary>
        public string Charge_Id { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public string Create_Date { get; set; }
        /// <summary>
        /// 商品名称id
        /// </summary>
        public object[] ProductIds { get; set; }
        public long Product_Id { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string Product_Name { get; set; }
        /// <summary>
        /// 兑换状态
        /// </summary>
        public long status { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string Order_Num { get; set; }
        /// <summary>
        /// 用户输入
        /// </summary>
        public string User_Input { get; set; }
    }


    public class ChargeActive
    {
        /// <summary>
        /// 0尚未开启 1活动进行中 2关闭活动  3活动尚未生效 4活动已结束
        /// </summary>
        public int Res { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }







    /// <summary>
    /// 实现泛型集合到数组对象转换的静态扩展类
    /// </summary>
    public static class ConverterExtension
    {
        /// <summary>
        /// 实现泛型集合到数组对象转换的静态扩展方法
        /// </summary>
        /// <typeparam name="T">泛型对象</typeparam>
        /// <param name="lines">泛型集合</param>
        /// <param name="lambdas">需要转换的泛型对象属性</param>
        /// <returns>数据对象</returns>
        public static object[,] To2DArray<T>(this List<T> lines, params Func<T, object>[] lambdas)
        {
            var array = new object[lines.Count(), lambdas.Count()];
            var lineCounter = 0;
            lines.ForEach(line =>
            {
                for (var i = 0; i < lambdas.Length; i++)
                {
                    array[lineCounter, i] = lambdas[i](line);
                }
                lineCounter++;
            });

            return array;
        }
    }
}
