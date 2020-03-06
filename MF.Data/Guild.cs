using System;
using System.Collections.Generic;

namespace MF.Data
{
    [Serializable]
    public class Guild
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Master { get; set; }
        public int UserCount { get; set; }
        public int Exp { get; set; }
        public int ActiveUserNumOfCurrentWeek { get; set; }
        public int ActiveUserNumOfLastWeek { get; set; }
        public DateTime CreateTime { get; set; }
    }
    [Serializable]
    public class GuildUser
    {
        public string Account { get; set; }
        public DateTime JoinDate { get; set; }
        public DateTime LastOnLine { get; set; }
        public string MissionList { get; set; }
        public long MaxMoney { get; set; }
        public string ActiveRec { get; set; }
        public int LastWeekActiveDay { get; set; }
        public string OldGuildIDList { get; set; }
        public int GuildID { get; set; }
        public int ID { get; set; }
        public string Nickname { get; set; }
        public string Icon { get; set; }
        public int MissionState { get; set; }
        public int IsoWeek { get; set; }
        public int ActiveNum { get; set; }
    }
    [Serializable]
    public class ClubsServer
    {
        public object[] msg;
        public int ret;
    }
    [Serializable]
    public class ClubsModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Founder { get; set; }
        public string Create_Date { get; set; }
        public string Head_Img { get; set; }
        public string Post { get; set; }
        public int Status { get; set; }
        public int Room_Card { get; set; }
        public int Members_Count { get; set; }
        public int Type { get; set; }
    }
    [Serializable]
    public class ClubsLink
    {
        public int id { get; set; }
        public string club_name { get; set; }
        public string club_id { get; set; }
        //设置时，存入名称【key、value】
        public string field { get; set; }
        public object value { get; set; }
        //游戏名称
        public string name { get; set; }
        public string r { get; set; }
        //联系方式
        public string phone { get; set; }
        public string wechat { get; set; }
        public string qq { get; set; }
        public string xl { get; set; }
    }
    [Serializable]
    public class MembersActivity
    {
        public int club_id { get; set; }
        public string ymd { get; set; }
        public Dictionary<string,object> activity { get; set; }


        public string member_id { get; set; }        
        public string active { get; set; }
        public string nick_name { get; set; }
    }
    public class Protocol2
    {
        public string module;
        public string func;
        public object[] args;

    }
    [Serializable]  
    public class ClubsModelServer
    {
        public ClubsModel msg;
        public int ret;
    }
    [Serializable]
    public class ClubsListServer
    {
        public ClubsModel[] msg;
        public int ret;
    }
    [Serializable]
    public class ClubsMembersServer
    {
        public int[] msg;
        public int ret;
    }
    [Serializable]
    public class ClubLinkInfo {
        public int clubid;
        public string name;
        public List<ClubsModel> linkclub;
    }
    [Serializable]
    public class ClubsServerRes
    {
        public object msg;
        public int ret;
    }
    [Serializable]
    public class ClubsStatisticRes
    {
        public ClubsStatistic msg;
        public int ret;
    }
    [Serializable]
    public class ClubsRes<T>
    {
        public T msg;
        public int ret;
    }
    [Serializable]
    public class ClubsStatistic
    {
        public long clubid;
        public string clubname;
        public int online;
        public int round;
    }
    [Serializable]
    public class ClubMembers
    {
        /// <summary>
        /// Chargeid
        /// </summary>
        public string memberid { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string nickname { get; set; }
        /// <summary>
        /// 加入时间
        /// </summary>
        public string jointime { get; set; }
        public Dictionary<string, string> clubsinfo { get; set; }
    }


    //game
    [Serializable]
    public class GameBlackUser
    {
        public int Id { get; set; }
        public string GameId { get; set; }
        public string Account { get; set; }
        public string Value { get; set; }
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
    }
    [Serializable]
    public class GameIncome
    { 
        /// <summary>
        /// 游戏时间
        /// </summary>
        public int Time { get; set; }
        public string TimeStr { get; set; }
        /// <summary>
        /// 游戏编号
        /// </summary>
        public string Child_Game_Index { get; set; }
        /// <summary>
        ///游戏场ID
        /// </summary>
        public string MatchId { get; set; }
        /// <summary>
        /// 包间号ID
        /// </summary>
        public string RoomId { get; set; }
        /// <summary>
        /// 局号
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// 玩家ID列表
        /// </summary>
        public List<string> ChargeIdList { get; set; }
        /// <summary>
        /// 玩家昵称列表
        /// </summary>
        public List<string> NickList { get; set; }
        /// <summary>
        /// 本局收益列表
        /// </summary>
        public List<string> IncomeList { get; set; }
        /// <summary>
        /// 本局抽水列表
        /// </summary>
        public List<string> InterestList { get; set; }

    }
    /// <summary>
    /// 自动巡场
    /// </summary>
    [Serializable]
    public class AutoPatrol
    {
        /// <summary>
        /// 游戏时间
        /// </summary>
        public int TimeStamp { get; set; }
        public DateTime DateTime { get; set; }
        /// <summary>
        /// 玩家ID列表
        /// </summary>
        public List<string> ChargeIds { get; set; }
        /// <summary>
        /// 玩家昵称列表
        /// </summary>
        public List<string> NickNames { get; set; }
        /// <summary>
        /// 处理emoji后的--玩家昵称列表
        /// </summary>
        public List<string> NickNamesNew { get; set; }
        /// <summary>
        /// 俱乐部ID列表
        /// </summary>
        public string ClubId { get; set; }
        /// <summary>
        /// 俱乐部ID列表
        /// </summary>
        public List<string> ClubIds { get; set; }
        /// <summary>
        ///游戏ID
        /// </summary>
        public string GameId { get; set; }
        /// <summary>
        ///游戏名称
        /// </summary>
        public string GameName { get; set; }
        /// <summary>
        /// 包间号ID
        /// </summary>
        public string RoomId { get; set; }
        /// <summary>
        /// 局号
        /// </summary>
        public int Round { get; set; }
        /// <summary>
        /// 注册时间戳列表
        /// </summary>
        public List<int> RegiTimes { get; set; }
        /// <summary>
        /// 最后一次登录IP列表
        /// </summary>
        public List<string> LastLoginIps { get; set; }

        
        /// <summary>
        /// 同桌玩家，同桌次数
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 是否与黑名单用户同一俱乐部
        /// 0否 1是
        /// </summary>
        public List<int> IsBlackClub { get; set; }
    }
}
