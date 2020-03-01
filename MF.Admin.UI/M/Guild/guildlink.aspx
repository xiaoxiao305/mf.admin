<%@ Page Title=" 俱乐部管理》俱乐部游戏设置" MasterPageFile="~/M/main.Master" Language="C#" AutoEventWireup="true"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server">
    <link href="/common/styles/layer.css" type="text/css" rel="Stylesheet" />
    <script language="javascript" type="text/javascript" src="/common/js/game.js"></script>
    <script language="javascript" type="text/javascript">
        function search() {
            var game = $("#game").val();
            if (game == "" || game.length < 5) { alert("请选择游戏名称"); return; }
            var args = [game, $("#round").val()];
            jsonPager.queryArgs = args;
            $("#loading").show();
            ajax.getClubGameLink(jsonPager.makeArgs(1), searchResult);
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
            addCell(tr, o.club_id, 0);
            addCell(tr, o.club_name, 1);
            addCell(tr, o.name, 2);
            addCell(tr, o.phone, 3);
            addCell(tr, o.wechat, 4);
            addCell(tr, o.qq, 5);
            addCell(tr, o.xl, 6);
            return tr;
        }
        $(document).ready(function () {
            initGame();
            initRound();
            var pagerTitles = ["Id", "俱乐部", "游戏", "电话", "微信", "QQ", "闲聊"];
            jsonPager.init(ajax.getGuildList, [], searchResult, pagerTitles, "list_table", "container", "pager", insertRow);
            jsonPager.dataBind(1, 0);
        });
        function initGame() {
            if (gamesconfig == null || gamesconfig.length < 1) { return; }
            var selGame = document.getElementById("game");
            for (var i = 0; i < gamesconfig.length; i++) {
                if (gamesconfig[i]["TopID"] == 0) {
                    selGame.options.add(new Option(gamesconfig[i]["Name"], gamesconfig[i]["Name"]));
                }
                if (gamesconfig[i]["TopID"] != 0) { break; }
            }
        }
        function initRound() {
            if (gamesconfig == null || gamesconfig.length < 1) { return; }
            var selRound = document.getElementById("round");
            var rounds = [];
            for (var i = 0; i < gamesconfig.length; i++) {
                if (gamesconfig[i]["TopID"] == 0) { continue; }
                var isContain = false;
                for (var m = 0; m < rounds.length; m++) {
                    if (rounds[m].Value.toString().indexOf(gamesconfig[i]["Value"].toString()) >= 0) { isContain = true; break; }
                }
                if (!isContain)
                    rounds.push(gamesconfig[i]);
            }
            if (rounds.length < 1) return;
            rounds = rounds.sort(function (a, b) { return (b.Value > a.Value) ? -1 : 1 });
            for (var j = 0; j < rounds.length; j++) {
                selRound.options.add(new Option(rounds[j].Name, rounds[j].Value));
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <div class="toolbar">俱乐部游戏设置</div>
    <div class="search">&nbsp;&nbsp;
        <select id="game">
            <option value="0">请选择游戏名称</option>
        </select>
        <select id="round">
            <option value="0">请选择游戏场次</option>
        </select> 
        <input type="button" value="查询" onclick="search()" class="ui-button-icon-primary" />
    </div>
    <div id="container"></div>
    <div id="pager" class="pager"></div>
    <div class="loading" id="loading"></div> 
</asp:Content>