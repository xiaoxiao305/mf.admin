using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MF.Admin.UI.M.game
{
    public partial class gameincome : BasePage
    {
        protected string time;
        protected string hour;
        protected string min;
        protected string sec;
        protected string gameId;
        protected string roomId;
        protected void Page_Load(object sender, EventArgs e)
        {
            //time = " + o.TimeStamp
            //    + "&gameId=" + o.GameId + "&roomId=" + o.RoomId +
            if (Request["time"] != null && !string.IsNullOrEmpty(Request["time"].ToString()))
            {
                DateTime t = DateTime.Parse("2012-10-01").AddSeconds(int.Parse(Request["time"].ToString()));
                time = t.ToString("yyyy-MM-dd");
                hour = t.ToString("HH");
                min = t.ToString("mm");
                sec = t.ToString("ss");
            }
            if (Request["gameId"] != null && !string.IsNullOrEmpty(Request["gameId"].ToString()))
                gameId = Request["gameId"].ToString();
            if (Request["roomId"] != null && !string.IsNullOrEmpty(Request["roomId"].ToString()))
                roomId = Request["roomId"].ToString();
            Admin.BLL.Base.WriteLog("gameincome request parm:", string.Format("time:{0},hour:{1},min:{2},gameid:{3},roomid:{4}", time, hour, min, gameId, roomId));
        }
    }
}