using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MF.Data
{
    public class GameServer
    {
        public string[] msg;
        public int ret;
    }
    public class OprGameServer
    {
        public string msg;
        public int ret;
    }
    public class GameServerList
    {
        public string gname;
    }

    //public class ClubRoomSettingModel3
    //{
    //    public int child_game_id { get; set; }
    //    public string club_id { get; set; }
    //    public int game_id { get; set; }
    //    public int index { get; set; }
    //    public int mode { get; set; }
    //    public string setting { get; set; }
    //    public int type { get; set; }
    //    //playerId
    //    //public string player_id { get; set; }
    //}

    //public class ClubRoomSettingModel2
    //{
    //    public int child_game_id { get; set; }
    //    public string club_id { get; set; }
    //    public int game_id { get; set; }
    //    public int index { get; set; }
    //    public int mode { get; set; }
    //    public string setting { get; set; }
    //    public RoomSettingInfo2 settingModel { get; set; }
    //    public int type { get; set; }
    //    //playerId
    //    public string player_id { get; set; }
    //}
    //public class RoomSettingInfo2
    //{
    //    public List<SettingData> settingData { get; set; }
    //    public List<ViewData> viewData { get; set; }
    //    public string[] ruleStr { get; set; }
    //    public int floorNum { get; set; }
    //    public string floorName { get; set; }
    //    public int floorID { get; set; }
    //    public int gameIcon { get; set; }
    //}




    public class ClubRoomSettingModel2
    {
        public int child_game_id { get; set; }
        public string club_id { get; set; }
        public int game_id { get; set; }
        public int index { get; set; }
        public int mode { get; set; }
        public string setting { get; set; }
        public int type { get; set; }
        public string player_id { get; set; }
    }

    public class ClubRoomSettingModel
    {
        public int child_game_id { get; set; }
        public string club_id { get; set; }
        public int game_id { get; set; }
        public int index { get; set; }
        public int mode { get; set; }
        public string setting { get; set; }
        //public RoomSettingInfo settingModel { get; set; }
        public int type { get; set; }
        //playerId
        public string player_id { get; set; }
    }
    public class RoomSettingInfo
    {
        public List<SettingData> settingData { get; set; }
        public List<ViewData> viewData { get; set; }
        public string[] ruleStr { get; set; }
        public int floorNum { get; set; }
        public string floorName { get; set; }
        public int floorID { get; set; }
        public int gameIcon { get; set; }
    }
    public class ViewData
    {
        public int dianGangHua { get; set; }
        public int gameModel { get; set; }
        public int ruleIndex { get; set; }
        public int matchID { get; set; }
        public string botomSocre { get; set; }
        public string enterNum { get; set; }
        public int maxMul { get; set; }
    }
    public class SettingData
    {
        public int patternType { get; set; }
        public int dianGangHua { get; set; }
        public int matchID { get; set; }
        public string botomSocre { get; set; }
        public string enterNum { get; set; }
        public int maxMul { get; set; }
        public string setGameName { get; set; }//string
        public string clubId { get; set; }
        public int realGameId { get; set; }
        public int mainGameId { get; set; }
        public int child_game_index { get; set; }
        public int itemGameType { get; set; }
        public string gameName { get; set; }
        public int gameType { get; set; }
        public int floorID { get; set; }
    }
    public class ClubRoomSetInfo
    {
        /// <summary>
        /// 无楼层
        /// </summary>
        public const int NONE_FLOOR = -1;//无楼层
        private const string KEY_SETTING = "settingData";
        private const string KEY_VIEW = "viewData";
        private const string KEY_RULE = "ruleStr";
        public const string KEY_FLOOR_NUM = "floorNum";
        public const string KEY_FLOOR_NAME = "floorName";
        public const string KEY_FLOOR_ID = "floorID";
        /// <summary>
        /// 俱乐部ID
        /// </summary>
        public string clubID = "";
        /// <summary>
        /// 楼层编号
        /// </summary>
        public int floorID = -1;
        /// <summary>
        /// 楼层数
        /// </summary>
        public int floorNum = NONE_FLOOR;
        /// <summary>
        /// 楼层名称
        /// </summary>
        public string floorName = "";
        /// <summary>
        /// 游戏图标
        /// </summary>
        public int gameIcon = 0;
        /// <summary>
        /// 规则描述
        /// </summary>
        public List<string> ruleStr = new List<string>();
        /// <summary>
        /// 设置信息
        /// </summary>
        public List<Dictionary<string, object>> settingInfo = new List<Dictionary<string, object>>();
        /// <summary>
        /// 显示数据
        /// </summary>
        public List<Dictionary<string, object>> viewInfo = new List<Dictionary<string, object>>();
    }
}
