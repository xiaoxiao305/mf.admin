<%@ Page Title=" 报表管理 》 游戏报表" Language="C#" MasterPageFile="~/M/main.Master" AutoEventWireup="true" CodeBehind="Game.aspx.cs" Inherits="MF.Admin.UI.M.Report.Game" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server">
    <script language="javascript" type="text/javascript">
        var games =  <%=gameDic %>;
        var totalwin = 0;
        var totallose = 0;
        function search() {
            var start=0;
            var over=0;
            totalwin = 0;
            totallose = 0;
            if ($("#starttime").val().trim() == "" || $("#overtime").val().trim() == "") {
                alert("请选择要查询的时间范围");
                return;
            }
            starttime= new Date($("#starttime").val().replace(/-/g, "/")).dateDiff("d");
            overtime=new Date($("#overtime").val().replace(/-/g, "/")).dateDiff("d");
            if (overtime < starttime) {
                alert("查询截止时间不能小于开始时间");
                return;
            }
            $("#loading").show();
            var args = [parseInt($("#game").val()),starttime, overtime];
            jsonPager.queryArgs = args;
            jsonPager.pageSize = 1000;
            ajax.getGameReport(jsonPager.makeArgs(1), searchResult);
        }
        function searchResult(data) {
            $("#loading").hide();
            if (data.code == 1) {
                jsonPager.data = data.result;
                jsonPager.dataBind(data.index, data.rowCount);
            }else{
                alert(data.msg);
            }
            $("#totalwin").text(totalwin);
            $("#totallose").text(totallose);
        }
        function insertRow(o,tr) {
            addCell = function(tr, text, i) {
                var td = tr.insertCell(i);
                td.innerHTML = text;
            };
            var date=new Date("2012/10/1");
            addCell(tr, date.dateAdd("d", o.Day).format("yyyy-MM-dd"), 0);
            addCell(tr,"<a href='scene.aspx?gameid=" + o.GameId + "' target='_blank'>"+games[o.GameId]+"</a>", 1); 
            addCell(tr, o.Actives == 0 ? "" : o.Actives, 2);
            addCell(tr, (o.Win - o.Lose) == 0 ? "" : (o.Win - o.Lose), 3); 
            addCell(tr, o.Shrink == 0 ? "" : o.Shrink, 4);
            totalwin += o.Win;
            totallose += o.Lose;
            return tr;
        }
        $(document).ready(function() {
            attachCalenderbox('#starttime', '#overtime', null,new Date().dateAdd("d", -1).Format("yyyy-MM-dd"), new Date().Format("yyyy-MM-dd"));
            for (var id in games)
                $("#game").append("<option value=\""+id+"\">"+games[id]+"</option>");
            var pagerTitles = ["日期", "游戏名称","今日活跃","今日输赢", "服务费"];
            jsonPager.init(ajax.getGameReport, [], searchResult, pagerTitles, "list_table", "container", "pager", insertRow);
            jsonPager.dataBind(1,0);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
            <div class="toolbar">游戏报表</div>
            <div class="search">&nbsp;&nbsp;
                <select id="game">
	                <option value="-1">所有游戏</option>
                </select>
                开始日期:<input type="text" id="starttime" class="box w100" readonly="readonly" />
                截止日期:<input type="text" id="overtime" class="box w100" readonly="readonly" />
                <input type="button" value="查询" onclick="search()" class="ui-button-icon-primary" />　　　<strong>
                    查询日期内总赢：<span class="red" id="totalwin">0</span></strong>　　　<strong>
                    查询日期内总输：<span class="red" id="totallose">0</span></strong>
            </div>
            <p></p>
            <div id="container"></div>
            <div id="pager" class="pager"></div>
            <div class="loading" id="loading"></div>
</asp:Content>
