<%@ Page Title="报表管理 》 游戏巡场" MasterPageFile="~/M/main.Master" Language="C#" AutoEventWireup="true" CodeBehind="autopatrol.aspx.cs" Inherits="MF.KF.UI.M.game.autopatrol" %>
<asp:Content ID="content1" ContentPlaceHolderID="h" runat="server">
    <script language="javascript" type="text/javascript">   
        var blackGameList =<%=blackGameList%>;
        $(document).ready(function () {
            for (var id in blackGameList) {
                $("#game").append("<input type='checkbox' name='chkGame' id=" + id
                    + " value=" + id + "><label for='" + id + "'>" + blackGameList[id] + "</label>&nbsp;&nbsp;&nbsp;");
            }
            var pagerTitle = ["游戏时间", "游戏名称", "包间号", "UID", "昵称", "俱乐部", "注册时间", "最后一次登录IP"];
            jsonPager.init(ajax.GetLastGameRecords, [], seaResult, pagerTitle, "list_table", "container", "pager", insertRow);
            jsonPager.dataBind(0, 1);
             search();
            setInterval(function () {
                search();
            }, 600000);
        });
        function search() {
            $("#loading").show();
            var gameids = [];
            if ($("#hideGme").is(":checked")) {
                $("input:checkbox[name='chkGame']:checked").each(function () {
                    if ($(this).val() > 0)
                        gameids.push($(this).val());
                });
            }
            var gameidsStr = gameids.join(",");
            jsonPager.queryArgs = [gameidsStr];
            jsonPager.pageSize = 1000;
            ajax.GetLastGameRecords(jsonPager.makeArgs(1), seaResult);
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
                if (o.Count > 0) {//同桌数据
                    tr.style.background = "DarkSeaGreen";
                }
                var td = tr.insertCell(i);
                td.innerHTML = text;
            };
            addCell(tr, new Date("2012/10/01").dateAdd("s", o.TimeStamp).Format("yyyy-MM-dd hh:mm:ss"), 0);
            addCell(tr, o.GameName, 1);
            addCell(tr, "<a href='/m/game/gameincome.aspx?time=" + o.TimeStamp + "&gameId=" + o.GameId + "&roomId=" + o.RoomId + "' target='_blank'>" + o.RoomId + "</a>", 2);
            var chargeIdStr = "";
            var stime = new Date(new Date().Format("yyyy-MM-dd 00:00:00")).dateDiff("s");
            for (var r in o.ChargeIds) {
                chargeIdStr += "<a href='/m/game/gameincome.aspx?time=" + stime + "&etime=" + o.TimeStamp + "&gameId=" + o.GameId + "&chargeId=" + o.ChargeIds[r] + "' target='_blank'>" + o.ChargeIds[r] + "</a><br/>";
            }
            addCell(tr, chargeIdStr, 3);
            addCell(tr, initNick(o.NickNamesNew.toString()), 4);
            var clubStr = "";
            for (var r2 in o.IsBlackClub) {
                if (o.IsBlackClub[r2] == 1)
                    clubStr += "<label style='color:red;'>" + o.ClubIds[r2] + "</label>";
                else
                    clubStr += o.ClubIds[r2];
                clubStr += "<br/>"
            }
            addCell(tr, clubStr, 5);
            var regiTimesStr = "";
            for (var r2 in o.RegiTimes) {
                if (o.RegiTimes[r2] > 0)
                    regiTimesStr += new Date("2012/10/01").dateAdd("s", o.RegiTimes[r2]).Format("yyyy-MM-dd hh:mm:ss");
                regiTimesStr += "<br/>"
            }
            addCell(tr, regiTimesStr, 6);
            addCell(tr, o.LastLoginIps.toString().replace(/,/gi, "<br/>"), 7);
            return tr;
        }
        function showgame() {
            if ($("#game").is(':hidden'))
                $("#game").show();
            else
                $("#game").hide();
        }
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="p" runat="server">
    <div class="toolbar">游戏巡场</div>
    <div class="search">&nbsp;&nbsp;
        <div style="display: inline-block;"><input type="checkbox" id="hideGme" onchange="showgame()"/>屏蔽游戏
        <label id="game" style="display:none;"></label></div>
        <input type="button" value="查询" onclick="search()" class="ui-button-icon-primary" style="display: inline-block;" />
    </div>
    <p></p>
    <div id="container"></div>
    <div class="pager" id="pager"></div>
    <div class="loading" id="loading"></div>
</asp:Content> 