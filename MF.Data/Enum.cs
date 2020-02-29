
namespace MF.Data
{
    /// <summary>
    /// 支付平台
    /// </summary>
    public enum PayChannel
    {
        /// <summary>
        /// 官方充值 （官方卡密）
        /// </summary>
        Website = 0,
        /// <summary>
        /// 支付宝_网站提交
        /// </summary>
        Alipay_Web = 10,
        /// <summary>
        /// 支付宝_App提交
        /// </summary>
        Alipay_App = 11,
        /// <summary>
        /// 易宝
        /// </summary>
        Yeepay = 2,
        /// <summary>
        /// 苹果
        /// </summary>
        iOS = 3,
        /// <summary>
        /// 微信_官网提交
        /// </summary>
        Wechat_Web = 40,
        /// <summary>
        /// 微信_App提交
        /// </summary>
        wechat_App = 41
        //其他联营渠道需要时定义
    }

    /// <summary>
    /// 操作日志枚举值
    /// </summary>
    public enum SystemLogEnum
    {
        /// <summary>
        /// 未定义【初始值】
        /// </summary>
        UNDEFINE = 0,
        /// <summary>
        /// 修改密码
        /// </summary>
          UPDATEPWD=1,
          /// <summary>
          /// 解绑手机
          /// </summary>
          UNBINDPHONE=2,
        /// <summary>
        /// 解绑安全令
        /// </summary>
        UNBINDPHONESAFE = 3,
        /// <summary>
        /// 解除本机锁定
        /// </summary>
        UNBINDLOCALLOCK = 4,
        /// <summary>
        /// 解除安全令锁定
        /// </summary>
        UNBINDPHONESAFELOCK = 5,
        /// <summary>
        /// 冻结账号
        /// </summary>
        FREEZE = 6,
        /// <summary>
        /// 解冻账号
        /// </summary>
        UNFREEZE = 7,
        /// <summary>
        /// 充值补单
        /// </summary>
        DEALCHARGERECORD = 8,
        /// <summary>
        /// 设置俱乐部活跃
        /// </summary>
        SETGUILDACTIVE = 9,
        /// <summary>
        /// 设置推荐俱乐部
        /// </summary>
        SETRECOMMENDGUILD = 10,
        /// <summary>
        /// 处理保证金
        /// </summary>
        DEALAPPLYRECORD = 11,
        /// <summary>
        /// 加元宝
        /// </summary>
        ADDCURRENCY = 12,
        /// <summary>
        /// 加金豆
        /// </summary>
        ADDBEN = 13,
        /// <summary>
        /// 加用户房卡
        /// </summary>
        ADDROOMCARD = 14,
        /// <summary>
        /// 加俱乐部房卡
        /// </summary>
        ADDCLUBSROOMCARD = 15,
        /// <summary>
        /// 开启充值活动
        /// </summary>
        OPENCHARGEACTIVE = 16,
        /// <summary>
        /// 关闭充值活动
        /// </summary>
        CLOSECHARGEACTIVE = 17,
        /// <summary>
        /// 添加游戏黑名单
        /// </summary>
        ADDBLACKUSER = 18,
        /// <summary>
        /// 删除游戏黑名单
        /// </summary>
        DELBLACKUSER = 19,
        /// <summary>
        /// 设置输赢值
        /// </summary>
        SETWINMONEY = 20,
        /// <summary>
        /// 修改游戏黑名单
        /// </summary>
        UPDATEBLACKUSER = 21,
        /// <summary>
        /// 审核黑名单【确认实锤】
        /// </summary>
        CONFIRMBLACKUSER = 22,
        /// <summary>
        /// 设置输赢值异常警报
        /// </summary>
        SETREDALERT = 23,
        /// <summary>
        /// 删除输赢值异常警报
        /// </summary>
        DELREDALERT = 24
    }
}
