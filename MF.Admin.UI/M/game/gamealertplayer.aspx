<%@ Page Title=" 游戏管理 》 输赢值异常预警"  MasterPageFile="~/M/main.Master" Language="C#" AutoEventWireup="true" CodeBehind="gamealertplayer.aspx.cs" Inherits="MF.Admin.UI.M.game.gamealertplayer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server">
    <style type="text/css">
       li {display:block; text-align:left;padding-left:10%;}
       .center{text-align:center;}
   </style>
    <link href="/common/styles/layer.css" type="text/css" rel="Stylesheet" /> 
    <script language="javascript" type="text/javascript">
        var gameTypesData = [];
        var gameIdData = [];
        function search() {
            var pagerTitles = ["游戏", "账号", "UID", "昵称", "注册时间","俱乐部", "输赢值","黑名单"];
            jsonPager.init(ajax.getRedAlertPlayer, [], searchResult, pagerTitles, "list_table", "container", "pager", insertRow);
            jsonPager.dataBind(1, 0);

            $("#loading").show();
            var gameType = $("#game").val();
            var gameTypes = gameTypesData;
            var gameIds = gameIdData;
            if (gameType != "-1") {
                gameTypes = [];
                gameTypes.push(gameType);
                gameIds = [];
                var model = GetGameBlackModelByType(gameType);
                if (model && model.matchid)
                    gameIds.push(model.matchid);
            }
            var gameTypeStr = gameTypes.join(",");
            var gameIdStr = gameIds.join(",");
            var field = parseInt($("#field").val());
            var time = $("#time").val();
            var stime = new Date(time.replace(/-/g, "/")).dateDiff("s");
            var args = [gameIdStr,gameTypeStr, field, $("#account").val(), stime];
            jsonPager.queryArgs = args;
            jsonPager.pageSize = 1000;
            ajax.getRedAlertPlayer(jsonPager.makeArgs(1), searchResult);
        }
        function searchResult(data) {
            $("#loading").hide();
            if (data.code == 1) {
                var data2 = resetResultData(data.result);
                jsonPager.data = data2;
                jsonPager.dataBind(data.index, data.rowCount);
            } else {
                alert(data.msg);
            }
        }

        function resetResultData(data) {
            var orderId = $("#order").val();//注册时间 输赢值
            var orderType = $("#orderType").is(':checked');//倒序
            if (orderId == 1) {
                if (orderType)
                    return data.sort(function (a, b) { return b.regTime - a.regTime; });
                else
                    return data.sort(function (a, b) { return a.regTime - b.regTime; });

            } else if (orderId == 2) {
                if (orderType)
                    return data.sort(function (a, b) { return b.lose - a.lose; });
                else
                    return data.sort(function (a, b) { return a.lose - b.lose; });

            }
            return data;
        }
        var blackTypes = ["","黑名单","待审核"];
        function insertRow(o, tr) {
            addCell = function (tr, text, i) {
                var td = tr.insertCell(i);
                td.innerHTML = text;
            };
            var gameModel = GetGameBlackModelByType(o.type);
            var gameName = (gameModel && gameModel != null) ? gameModel.name : "";
            var gameId = (gameModel && gameModel != null) ? gameModel.matchid : 0;
            //var etime = new Date().dateDiff("s");
            //var etime = (new Date("2012/10/1")).dateAdd("s", o.regTime).format("yyyy-MM-dd 00:00:00")
            var etime = new Date((new Date("2012/10/1")).dateAdd("s", o.regTime).format("yyyy-MM-dd 23:59:59")).dateDiff("s");
            console.log("etime:", etime);
            addCell(tr, gameName, 0);
            addCell(tr, o.account, 1);
            addCell(tr, "<a href='/m/game/gameincome.aspx?time=" + o.regTime + "&etime=" + etime + "&gameId=" + gameId + "&chargeId=" + o.player_id + "' target='_blank'>" + o.player_id + "</a>", 2); 
            addCell(tr, o.nick, 3);
            addCell(tr, (new Date("2012/10/1")).dateAdd("s", o.regTime).format("yyyy-MM-dd hh:mm:ss"), 4);
            addCell(tr, o.clubId, 5);
            addCell(tr, o.lose, 6);
            addCell(tr, blackTypes[o.blackType], 7);
            return tr;
        }
        $(document).ready(function () {
            gameTypesData = [];
            for (var id in blackGameMap) {
                if (blackGameMap.hasOwnProperty(id)) {
                    $("#game").append("<option value=\"" + blackGameMap[id].type + "\">" + blackGameMap[id].name + "</option>");
                    $("#game1").append("<option value=\"" + blackGameMap[id].type + "\">" + blackGameMap[id].name + "</option>");
                    gameTypesData.push(blackGameMap[id].type);
                    gameIdData.push(id);
                }
            }
            attachCalenderbox('#time', null, null, new Date().dateAdd("d",-2).Format("yyyy-MM-dd"), null);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <div class="toolbar">输赢值异常预警</div>
    <div class="search">&nbsp;&nbsp;
        <select id="game"><option value="-1">所有游戏</option></select>
        <select id="field"><option value="1">UID</option><option value="2">账号</option></select>
        <input type="text" id="account"/>
        注册时间<input type="text" id="time" class="box w100" readonly="readonly" />至今
        <select id="order"><option value="1">注册时间</option><option value="2">输赢值</option></select>
        <input type="checkbox" checked="checked" id="orderType"  />倒序
        <input type="button" value="查询" onclick="search()" class="ui-button-icon-primary" />
        <input type="button" value="查询输赢值配置" onclick="searchAlertConfig()" class="ui-button-icon-primary"  style="margin-left:30px;"/><input type="button" value="设置输赢值" onclick="setRedAlert()" class="ui-button-icon-primary oprbutton"/> 
    </div>
    <p></p>
    <div id="container"></div>
    <div id="pager" class="pager"></div>
    <div class="loading" id="loading"></div>

    
     <!--弹出窗口开始-->
    <div class="theme-popover" style="height: 400px;">
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
        <ul id="T1">
            <li>　　游　戏：<select id="game1"><option value="">请选择游戏</option></select></li>
            <li>　　输赢值：<input type="text" id="value"/>元宝</li> 
            <li class="err red center" id="lblerr"></li>
            <li class="center"><input class="btn btn-primary" type="button" value=" 确 定" onclick="setRedAlertConfirm()" /></li>
        </ul> 
    </div> 
</asp:Content>