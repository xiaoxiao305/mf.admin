using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MF.KF.UI.M.game
{
    public partial class gameincome : BasePage
    {
        protected string time;
        protected string hour;
        protected string min;
        protected string sec;
        protected string etime;
        protected string ehour;
        protected string emin;
        protected string esec;
        protected string gameId;
        protected string roomId;
        protected string chargeId;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request["time"] != null && !string.IsNullOrEmpty(Request["time"].ToString()))
                {
                    DateTime t = DateTime.Parse("2012-10-01").AddSeconds(int.Parse(Request["time"].ToString()));
                    time = t.ToString("yyyy-MM-dd");
                    hour = t.ToString("HH");
                    min = t.ToString("mm");
                    sec = t.ToString("ss");
                }
                if (Request["etime"] != null && !string.IsNullOrEmpty(Request["etime"].ToString()))
                {
                    DateTime et = DateTime.Parse("2012-10-01").AddSeconds(int.Parse(Request["etime"].ToString()));
                    etime = et.ToString("yyyy-MM-dd");
                    ehour = et.ToString("HH");
                    emin = et.ToString("mm");
                    esec = et.ToString("ss");
                }
                else
                {
                    etime = time;
                    ehour = hour;
                    emin = min;
                    esec = sec;
                }
                if (Request["gameId"] != null && !string.IsNullOrEmpty(Request["gameId"].ToString()))
                    gameId = Request["gameId"].ToString();
                if (Request["roomId"] != null && !string.IsNullOrEmpty(Request["roomId"].ToString()))
                    roomId = Request["roomId"].ToString();
                if (Request["chargeId"] != null && !string.IsNullOrEmpty(Request["chargeId"].ToString()))
                    chargeId = Request["chargeId"].ToString();
            }
            catch (Exception ex)
            {
                Admin.BLL.Base.WriteError("gameincome init ex:", ex.Message);
            }
        }
    }
}