<%@ Page Title=" 游戏管理 》 输赢值异常预警"  MasterPageFile="~/M/main.Master" Language="C#" AutoEventWireup="true" CodeBehind="gamealertplayer.aspx.cs" Inherits="MF.Admin.UI.M.game.gamealertplayer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server">
    <style type="text/css">
       li {display:block; text-align:left;padding-left:10%;}
       .center{text-align:center;}
   </style>
    <link href="/common/styles/layer.css" type="text/css" rel="Stylesheet" /> 
    <script language="javascript" type="text/javascript">
        function search() {
            if ($("#game").val() == "") {
                alert("请选择游戏");
                return
            }
            $("#loading").show();
            var gameType = $("#game").val();
            var field = parseInt($("#field").val());
            var args = [gameType, field, $("#account").val()];
            jsonPager.queryArgs = args;
            jsonPager.pageSize = 1000;
            ajax.getRedAlertPlayer(jsonPager.makeArgs(1), searchResult);
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
            addCell(tr, GetGameNameByType(o.type), 0);
            addCell(tr, o.account, 1);
            addCell(tr, o.player_id, 2);
            addCell(tr, o.nick, 3);
            addCell(tr, o.lose, 4);
            return tr;
        }
        $(document).ready(function () {
            for (var id in blackGameMap) {
                if (blackGameMap.hasOwnProperty(id)) {
                    $("#game").append("<option value=\"" + blackGameMap[id].type + "\">" + blackGameMap[id].name + "</option>");
                }
            }
            var pagerTitles = ["游戏", "账号", "UID", "昵称", "输赢值"];
            jsonPager.init(ajax.getRedAlertPlayer, [], searchResult, pagerTitles, "list_table", "container", "pager", insertRow);
            jsonPager.dataBind(1, 0);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <div class="toolbar">输赢值异常预警</div>
    <div class="search">&nbsp;&nbsp;
        <select id="game"><option value="">所有游戏</option></select>
        <select id="field"><option value="1">UID</option><option value="2">账号</option></select>
        <input type="text" id="account"/>
        <input type="button" value="查询" onclick="search()" class="ui-button-icon-primary" />　
        <a href="javascript:;" onclick="javascript:window.location.href='/m/game/gamealertconfig.aspx'">输赢值设置</a>
    </div>
    <p></p>
    <div id="container"></div>
    <div id="pager" class="pager"></div>
    <div class="loading" id="loading"></div>
</asp:Content>