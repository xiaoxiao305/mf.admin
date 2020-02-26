using MF.Admin.BLL;
using System;
using System.Collections.Generic;

namespace MF.Admin.UI.M
{
    public partial class menu :BasePage
    {
        static Dictionary<int, string> menuList;
        protected string li = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (menuList == null || menuList.Count < 1) initMenuList();
            try {
                int id = 0;
                if (int.TryParse(Request["do"], out id)) {
                    li = menuList[id];
                }
            }
            catch(Exception ex) {
                Base.WriteError("menu.aspx Page_Load Error:",ex.Message);
            }
        }
        void initMenuList()
        {
            menuList = new Dictionary<int, string>();
            menuList.Add(0, @"<li class='menu_title'>用户管理</li> 
        <li class='menu_list'><a href ='/m/users/userlist.aspx' target='main' class='white'>玩家数据</a></li>");
            menuList.Add(1, @"<li class='menu_title'>充值管理</li> 
        <li class='menu_list'><a href ='/m/charge/recordlist.aspx' target='main' class='white'>充值明细</a></li>");
            menuList.Add(2, @"<li class='menu_title'>俱乐部管理</li> 
        <li class='menu_list'><a href ='/m/guild/guildlist.aspx' target='main' class='white'>俱乐部列表</a></li>
        <li class='menu_list'><a href ='/m/guild/clublink.aspx' target='main' class='white'>俱乐部关联</a></li>
<li class='menu_list'><a href ='/m/guild/leagueclubmembers.aspx' target='main' class='white'>联盟俱乐部成员</a></li>
<li class='menu_list'><a href ='/m/guild/clubmembers.aspx' target='main' class='white'>成员所在俱乐部</a></li>
<li class='menu_list'><a href ='/m/guild/clubactive.aspx' target='main' class='white'>俱乐部活跃</a></li>
<li class='menu_list'><a href ='/m/guild/clubmemberslist.aspx' target='main' class='white'>俱乐部成员</a></li>");
            menuList.Add(3, @"<li class='menu_title'>游戏管理</li> 
        <li class='menu_list'><a href ='/m/report/game.aspx' target='main' class='white'>游戏报表</a></li>
        <li class='menu_list'><a href ='/m/report/scene.aspx' target='main' class='white'>场数据</a></li>
        <li class='menu_list'><a href ='/m/setting/flushgameserver.aspx' target='main' class='white'>刷新游戏配置</a></li>
        <li class='menu_list'><a href ='/m/game/closeroom.aspx' target='main' class='white'>解散游戏</a></li>
        <li class='menu_list'><a href ='/m/game/roomlist.aspx' target='main' class='white'>重置包间配置</a></li>
        <li class='menu_list'><a href ='/m/game/gameblacklist.aspx' target='main' class='white'>游戏黑名单</a></li>
        <li class='menu_list'><a href = '/m/game/auditblacklist.aspx' target='main' class='white'>待审核黑名单</a></li>");
            menuList.Add(4, @"<li class='menu_title'>报表管理</li> 
        <li class='menu_list'><a href ='/m/report/RegReport.aspx' target='main' class='white'>注册报表</a></li>
        <li class='menu_list'><a href ='/m/report/charge.aspx' target='main' class='white'>充值报表</a></li>        
        <li class='menu_list'><a href ='/m/report/currency.aspx' target='main' class='white'>元宝报表</a></li>
        <li class='menu_list'><a href ='/m/report/promot.aspx' target='main' class='white'>推广报表</a></li> 
        <li class='menu_list'><a href ='/m/report/zywystatic.aspx' target='main' class='white'>交易平台报表</a></li>");
            menuList.Add(5, @"<li class='menu_title'>系统管理</li> 
        <li class='menu_list'><a href ='/m/SysManage/oprlog.aspx' target='main' class='white'>操作日志</a></li>
        <li class='menu_list'><a href ='/m/SysManage/loginlog.aspx' target='main' class='white'>登录日志</a></li>
        <li class='menu_list'><a href ='/m/SysManage/PushNews.aspx' target='main' class='white'>设置推送消息</a></li>");
        }
    }
}
