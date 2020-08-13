using MF.Admin.BLL;
using System;
using System.Collections.Generic;

namespace MF.KF.UI.M
{
    public partial class menu : BasePage
    {
        static Dictionary<int, string> menuList;
        protected string li = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (menuList == null || menuList.Count < 1)
                initMenuList();
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
            menuList.Add(0, @"<li class='menu_title'>用户管理</li><li class='menu_list'><a href ='/m/users/userlist.aspx' target='main' class='white'>玩家数据</a></li>
<li class='menu_list'><a href ='/m/currency/AllStrongBox.aspx' target='main' class='white'>二级密码记录</a></li>");
            menuList.Add(1, @"<li class='menu_title'>俱乐部管理</li><li class='menu_list'><a href ='/m/club/club_statistic_day.aspx' target='main' class='white'>俱乐部每日统计</a></li>
        <li class='menu_list'><a href ='/m/club/guildlist.aspx' target='main' class='white'>俱乐部列表</a></li>
        <li class='menu_list'><a href ='/m/club/guildlink.aspx' target='main' class='white'>俱乐部游戏设置</a></li>
        <li class='menu_list'><a href ='/m/club/memberactive.aspx' target='main' class='white'>俱乐部成员活跃</a></li>
        <li class='menu_list'><a href ='/m/club/clubactive.aspx' target='main' class='white'>俱乐部活跃</a></li>
        <li class='menu_list'><a href ='/m/club/clubmembers.aspx' target='main' class='white'>成员所在俱乐部</a></li>
        <li class='menu_list'><a href ='/m/club/hightaxclubs.aspx' target='main' class='white'>高税俱乐部</a></li>
        <li class='menu_list'><a href ='/m/club/clubtax.aspx' target='main' class='white'>俱乐部收益</a></li>");
            string gameLi = @"<li class='menu_title'>游戏管理</li><li class='menu_list'><a href ='/m/game/closeroom.aspx' target='main' class='white'>解散游戏</a></li>
        <li class='menu_list'><a href ='/m/game/flushgameserver.aspx' target='main' class='white'>刷新游戏配置</a></li>
        <li class='menu_list'><a href ='/m/game/gamerec.aspx' target='main' class='white'>游戏录像</a></li>
        <li class='menu_list'><a href ='/m/game/gameincome.aspx' target='main' class='white'>游戏收益查询</a></li>
        <li class='menu_list'><a href ='/m/game/recsearch.aspx' target='main' class='white'>录像查询</a></li>
        <li class='menu_list'><a href ='/m/game/autopatrol.aspx' target='main' class='white'>自动巡场</a></li>
        <li class='menu_list'><a href = '/m/game/newgameuser.aspx' target='main' class='white'>新增有效用户</a></li>
        <li class='menu_list'><a href ='/m/game/gameblacklist.aspx' target='main' class='white'>游戏黑名单</a></li>
        <li class='menu_list'><a href ='/m/game/auditblacklist.aspx' target='main' class='white'>待审核黑名单</a></li>";
            //        //<li class='menu_list'><a href = '/m/game/bindtableuid.aspx' target='main' class='white'>禁止同桌游戏</a></li>
            //<li class='menu_list'><a href ='/m/game/addblackuserlist.aspx' target='main' class='white'>批量添加黑名单</a></li>

            //string back = "";
            //if (isAdmin == 1)
            //{
            //    back = "<li class='menu_list'><a href ='/m/game/gameblacklist.aspx' target='main' class='white'>游戏黑名单</a></li>" +
            //        "<li class='menu_list'><a href ='/m/game/auditblacklist.aspx' target='main' class='white'>待审核黑名单</a></li>";
            //} 
            menuList.Add(2,gameLi);
        }
    }
}
