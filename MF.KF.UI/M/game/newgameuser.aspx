<%@ Page Title=" 游戏管理 》 新增有效用户"  Language="C#" MasterPageFile="~/M/main.Master" AutoEventWireup="true" CodeBehind="newgameuser.aspx.cs" Inherits="MF.KF.UI.M.game.newgameuser" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server">
    <link href="/common/styles/layer.css" type="text/css" rel="Stylesheet" /> 
    <script language="javascript" type="text/javascript">
        var games =  <%=blackGameList %>;
        function search() {
            $("#loading").show();
            var gameid = parseInt($("#game").val());
            console.log(gameid);
            var field = parseInt($("#field").val());
            var s = new Date($("#time").val().replace(/-/g, "/")).dateDiff("s");
            var args = [s, gameid, field, $("#value").val()];
            jsonPager.queryArgs = args;
            jsonPager.pageSize = 100;
            ajax.getNewGameUsers(jsonPager.makeArgs(1), searchResult);
        }
        function searchResult(data) {
            $("#loading").hide();
            if (data.code == 1) {
                jsonPager.data = data.result;
                jsonPager.dataBind(data.index, data.rowCount);
            } else {
                alert(data.msg);
            }
        }
        function insertRow(o, tr) {
            addCell = function (tr, text, i) {
                var td = tr.insertCell(i);
                td.innerHTML = text;
            };
            if (o.GameId.length != 4){
            var date = new Date("2012/10/1");
            var regdate = date.dateAdd("s", o.RegDate);
            addCell(tr, regdate.format("yyyy-MM-dd hh:mm:ss"), 0);
            addCell(tr, new Date("1970/01/01").dateAdd("s", (o.GameDate + 8 * 60 * 60)).format("yyyy-MM-dd hh:mm:ss"), 1);
            var s = new Date(regdate.format("yyyy-MM-dd 00:00:00")).dateDiff('s');
            var e = new Date(regdate.format("yyyy-MM-dd 23:59:59")).dateDiff('s');
            var g = games[o.GameId] ? games[o.GameId] == "undefined" ? o.GameId : games[o.GameId] : o.GameId;
            addCell(tr, "<a href='/m/game/gameincome.aspx?time=" + s + "&etime=" + e + "&gameId=" + o.GameId + "&chargeId=" + o.ChargeId + "' target='_blank'>" + g + "</a>", 2);
            addCell(tr, "<a href='/M/currency/CurrencyRecord.aspx?chargeid=" + o.ChargeId + "' target='_blank'>" + o.ChargeId + "</a>", 3);
            addCell(tr, o.Account, 4);
            addCell(tr, o.NickName, 5);
            addCell(tr, o.ClubId, 6);
            addCell(tr, o.Guid, 7);
            addCell(tr, o.LoginIP, 8);
            addCell(tr, "<a href='javascript:;' onclick='getGameMoney(" + JSON.stringify(o).replace(/\'/g, "&apos;") + ")'>输赢值</a>", 9);
                return tr;
            } return null;
        }
        $(document).ready(function () {
            for (var id in games) {
                $("#game").append("<option value=\"" + id + "\">" + games[id] + "</option>");
            }
            var pagerTitles = ["注册时间", "游戏时间", "游戏", "UID", "账号", "昵称", "友谊圈", "GUID", "登录IP", "操作"];
            jsonPager.init(ajax.getNewGameUsers, [], searchResult, pagerTitles, "list_table", "container", "pager", insertRow);
            jsonPager.dataBind(1, 0);
            attachCalenderbox('#time', null, null, null, null);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <div class="toolbar">新增有效用户列表</div>
    <div class="search">&nbsp;&nbsp;
        <select id="game"><option value="-1">所有游戏</option></select>
        <input type="text" id="time" class="box w100" readonly="readonly" />
        <select id="field"><option value="1">UID</option><option value="2">账号</option></select>
        <input type="text" id="value"/>
        <input type="button" value="查询" onclick="search()" class="ui-button-icon-primary" />　
    </div>
    <p></p>
    <div id="container"></div>
    <div id="pager" class="pager"></div>
    <div class="loading" id="loading"></div>
     <!--弹出窗口开始-->
    <div class="theme-popover" style="height: 430px;">
        <div class="theme-poptit">
            <a href="javascript:;" title="关闭" class="close">×</a>
            <h3 id="msgtitle"></h3>
        </div>
        <div class="theme-popbod dform">
            <div id="content"></div>
        </div>
    </div>
    <div class="theme-popover-mask"></div>
    <!--弹出窗口结束-->
    <div class="hide"> 
        <ul id="T5">
            <li>　　游　戏：<label id="lblgame5"></label></li>
            <li>　　账　号：<label id="lblaccount5"></label></li>
            <li>　　　UID:<label id="lblchargeid5"></label></li> 
            <li>　　输赢值：<label id="lblMoney5"></label></li>
        </ul>
    </div> 
</asp:Content>
