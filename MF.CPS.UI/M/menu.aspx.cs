using MF.Admin.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MF.CPS.UI.M
{

    public partial class menu : BasePage
    {
        static Dictionary<int, string> menuList;
        protected string li = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (menuList == null || menuList.Count < 1) initMenuList();
            try
            {
                int id = 0;
                if (int.TryParse(Request["do"], out id))
                {
                    li = menuList[id];
                }
            }
            catch (Exception ex)
            {
                Base.WriteError("menu.aspx Page_Load Error:", ex.Message);
            }
        }
        void initMenuList()
        {
            menuList = new Dictionary<int, string>();
            menuList.Add(0, @"<li class='menu_title'>报表管理</li> 
        <li class='menu_list'><a href ='/m/report/report.aspx' target='main' class='white'>报表数据</a></li>");
        }
    }
}