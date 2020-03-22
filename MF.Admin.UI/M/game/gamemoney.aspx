<%@ Page Title="游戏管理 》 玩家输赢值" MasterPageFile="~/M/main.Master" Language="C#" AutoEventWireup="true" CodeBehind="gamemoney.aspx.cs" Inherits="MF.Admin.UI.M.game.gamemoney" %>
<asp:Content ID="content1" ContentPlaceHolderID="h" runat="server">
    <link href="/common/styles/layer.css" type="text/css" rel="Stylesheet" /> 
    <script language="javascript" type="text/javascript"> 
        var gameTypesData = [];
        $(document).ready(function () {
            var pagerTitle = ["游戏名称", "UID", "账号", "昵称", "输赢值", "操作"];
            jsonPager.init(ajax.GetUserGameMoney, [], seaResult, pagerTitle, "list_table", "container", "pager", insertRow);
            jsonPager.dataBind(0, 1);

            gameTypesData = [];
            for (var id in blackGameMap) {
                if (blackGameMap.hasOwnProperty(id)) {
                    $("#game").append("<option value=\"" + blackGameMap[id].type + "\">" + blackGameMap[id].name + "</option>");
                    gameTypesData.push(blackGameMap[id].type);
                }
            }
        });
        function search() {
            $("#loading").show();

            var gameType = $("#game").val();
            var gameTypes = gameTypesData;
            if (gameType != "-1") {
                gameTypes = [];
                gameTypes.push(gameType);
            }
            var gameTypeStr = gameTypes.join(",");
            var chargeId = $("#chargeId").val();
            if (chargeId == "") {
                alert("请输入UID");
                return;
            }
            jsonPager.queryArgs = [gameTypeStr, chargeId];
            jsonPager.pageSize = 1000;
            ajax.GetGameMoney(jsonPager.makeArgs(1), seaResult);
        }
        function seaResult(data) {
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
            addCell(tr, GetGameNameByType(o.GameType), 0);
            addCell(tr, o.player_id, 1);
            addCell(tr, o.Account, 2);
            addCell(tr, o.Nickname, 3);
            addCell(tr, o.lose, 4);
            addCell(tr, "<a href='javascript:;' onclick='setWinMoney2(" + JSON.stringify(o) + ")'>修改</a>", 5);
            return tr;
        }
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="p" runat="server">
    <div class="toolbar">玩家输赢值</div>
    <div class="search">&nbsp;&nbsp;
        <select id="game"><option value="-1">所有游戏</option></select>
        UID<input type="text" id="chargeId"/>
        <input type="button" value="查询" onclick="search()" class="ui-button-icon-primary" />
    </div>
    <p></p>
    <div id="container"></div>
    <div class="pager" id="pager"></div>
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
        <ul id="T2">
            <li>　　游　戏：<label id="tgame2"></label></li>
            <li>　　　&nbsp;UID：<label id="tchargeid2"></label></li>
            <li id="valLi2">　　输赢值：<input class="ipt" type="text" id="tval2"/></li>
            <li id="tokenLi2">安　全　令：<input class="ipt" type="text" id="token2" /></li>
            <li class="err red center" id="lblerr2"></li>
            <li class="center"><input class="btn btn-primary" type="button" value=" 确 定" onclick="setWinMoneyConfirm2()" /></li>
        </ul>
    </div> 
</asp:Content> 