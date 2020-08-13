<%@ Page Title=" 游戏管理》禁止同桌游戏" MasterPageFile="~/M/main.Master"  Language="C#" AutoEventWireup="true" CodeBehind="bindtableuid.aspx.cs" Inherits="MF.KF.UI.M.game.bindtableuid" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server">
    <link href="/common/styles/layer.css" type="text/css" rel="Stylesheet" />
    <script language="javascript" type="text/javascript">
        function search() {
            var players = $("#players").val();
            if (players == "") return;
            var args = [players];
            jsonPager.queryArgs = args;
            $("#loading").show();
            ajax.getMutualList(jsonPager.makeArgs(1), searchResult);
        }
        function searchResult(data) {
            $("#loading").hide();
            if (data.code == 1) {
                console.log("result:", data.result);
                jsonPager.data = data.result;
                jsonPager.dataBind(data.index, data.rowCount);
            } else {
                alert(data.msg);
            }
        }
        function insertRow(o, tr) {
            addCell = function(tr, text, i) {
                var td = tr.insertCell(i);
                td.innerHTML = text;
            };
            addCell(tr, o.player_id_0 , 0);
            addCell(tr, o.player_id_1, 1);
            var oJson = JSON.stringify(o);
            addCell(tr, "<a href='javascript:;' onclick='delMutual(" + oJson + ")'>删除</a>", 2);
            return tr;
        }
        $(document).ready(function() {
            var pagerTitles = ["玩家1", "玩家2","操作"];
            jsonPager.init(ajax.getMutualList, [], searchResult, pagerTitles, "list_table", "container", "pager", insertRow);
            jsonPager.dataBind(1, 0);
        });
        function addmutual(){
            var player_id_0 = $("#txtplayer_id_0").val();
            var player_id_1 = $("#txtplayer_id_1").val();
            var token = $("#token").val();
            if (player_id_0 == "" || player_id_1=="") {
                $("#lblerr").text("请输入玩家ID");
                return;
            }
            else if (token == "") {
                $("#lblerr").text("请输入安全令");
                return;
            }
            ajax.addMutual("addmutual", [player_id_0, player_id_1, token], winresult);
        }
        function delMutual(o) {
            console.log("objJson:", o);
            if (o.player_id_0 == "" || o.player_id_1 == "") return;
            if (confirm("确认删除禁止同桌游戏玩家ID【" + o.player_id_0 + "," + o.player_id_1+"】吗？"))
                ajax.delMutual("delmutual", [o.player_id_0, o.player_id_1], winresult);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <div class="toolbar">禁止同桌列表</div>
    <div class="search">&nbsp;&nbsp;
        玩家Id<input  type="text" id="players" class="box"/> <label style="color:red">多个ID以英文逗号（,）隔开</label>
        <input type="button" value="查询" onclick="search()" class="ui-button-icon-primary" />
        <input type="button" value="添加禁止同桌游戏ID" onclick="showAddMoneyWin(1)" class="ui-button-icon-primary oprbutton"/>
        <input type="button" value="刷新游戏服务器" onclick="window.location.href = '/m/game/flushgameserver.aspx'" class="ui-button-icon-primary oprbutton"/>
    </div>
    <div id="container"></div>
    <div id="pager" class="pager"></div>
    <div class="loading" id="loading"></div>
      <!--弹出窗口开始-->
    <div class="theme-popover" style="height: 400px;">
        <div class="theme-poptit">
            <a href="javascript:;" title="关闭" class="close">×</a>
            <h3 id="msgtitle">添加禁止同桌游戏ID</h3>
        </div>
        <div class="theme-popbod dform">
            <div id="content"></div>
        </div>
    </div>
    <div class="theme-popover-mask"></div>
    <!--弹出窗口结束-->
    <div class="hide">
         <ul id="T1">
            <li>玩家ID-1：<input class="ipt" type="text" id="txtplayer_id_0" /></li>
             <li>玩家ID-2：<input class="ipt" type="text" id="txtplayer_id_1" /></li>
             <li><label style="color:red">玩家2-ID，若为多个，以英文逗号（,）隔开</label></li>
            <li>安&nbsp;全&nbsp;令：<input class="ipt" type="text" id="token" /></li>
            <li class="err red" id="lblerr"></li>
            <li><input class="btn btn-primary" type="button" value=" 确 定" onclick="addmutual()" /></li>
        </ul>  
    </div>
</asp:Content>