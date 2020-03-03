<%@ Page Title="游戏管理 》 玩家输赢值" MasterPageFile="~/M/main.Master" Language="C#" AutoEventWireup="true" CodeBehind="gamemoney.aspx.cs" Inherits="MF.Admin.UI.M.game.gamemoney" %>
<asp:Content ID="content1" ContentPlaceHolderID="h" runat="server">
    <script language="javascript" type="text/javascript"> 
        var gameTypesData = [];
        $(document).ready(function () {
            var pagerTitle = ["游戏名称", "UID", "账号", "昵称", "输赢值"];
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
            addCell(tr, o.NickName, 3);
            addCell(tr, o.lose, 4);
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
</asp:Content> 