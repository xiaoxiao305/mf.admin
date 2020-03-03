<%@ Page Title="报表管理 》 游戏巡场" MasterPageFile="~/M/main.Master" Language="C#" AutoEventWireup="true" CodeBehind="autopatrol.aspx.cs" Inherits="MF.KF.UI.M.game.autopatrol" %>
<asp:Content ID="content1" ContentPlaceHolderID="h" runat="server">
    <script language="javascript" type="text/javascript">        
        $(document).ready(function () {
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
            jsonPager.queryArgs = [];
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
                if (o.Count > 0) {//同桌巡场
                    tr.style.background = "red";
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
            addCell(tr, initNick(o.NickNames.toString()), 4);
            addCell(tr, o.ClubIds.toString().replace(/,/gi, "<br/>"), 5);
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
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="p" runat="server">
    <div class="toolbar">游戏巡场</div>
    <div class="search">&nbsp;&nbsp;
        <input type="button" value="查询" onclick="search()" class="ui-button-icon-primary" />
    </div>
    <p></p>
    <div id="container"></div>
    <div class="pager" id="pager"></div>
    <div class="loading" id="loading"></div>
</asp:Content> 