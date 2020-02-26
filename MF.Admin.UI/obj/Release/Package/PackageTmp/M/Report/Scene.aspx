<%@ Page Language="C#"  Title=" 报表管理 》 游戏场报表"  MasterPageFile="~/M/main.Master" AutoEventWireup="true" CodeBehind="Scene.aspx.cs" Inherits="MF.Admin.UI.M.Report.Scene" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server">
    <script language="javascript" type="text/javascript">
        var games = <%=gameDic %>;
        var match= eval(<%=matchDic %>);
        function search() {
            if ($("#starttime").val().trim() == "" || $("#overtime").val().trim() == "") {
                alert("请选择要查询的时间范围");
                return;
            }
            startTime = new Date($("#starttime").val().replace(/-/g, "/")).dateDiff("d");
            overTime = new Date($("#overtime").val().replace(/-/g, "/")).dateDiff("d");
            if (overTime < startTime) {
                alert("查询截止时间不能小于开始时间");
                return;
            }
            $("#loading").show();
            var args = [parseInt($("#game").val()), parseInt($("#matchlist").val()),startTime, overTime];
            jsonPager.queryArgs = args;
            jsonPager.pageSize = 1000;
            ajax.getSceneReport(jsonPager.makeArgs(1), searchResult);
        }
        function searchResult(data) {
            $("#loading").hide();
            if (data.code == 1) {
                jsonPager.data = data.result;
                jsonPager.dataBind(data.index, data.rowCount);
            }else{
                alert(data.msg);
            }
        }
        var matchinfo="";
        function insertRow(o,tr) {
            addCell = function(tr, text, i) {
                var td = tr.insertCell(i);
                td.innerHTML = text;
            };
            getMatchName = function(gameid,matchid) {            
                var list = match[gameid]; 
                if(list ==null)
                    return "";
                for (var i=0;i<list.length;i++){
                    if(matchid== list[i][0]) 
                        return list[i][1];
                } 
                return "";
            }; 
            var date=new Date("2012/10/1")
            addCell(tr, date.dateAdd("d", o.Day).format("yyyy-MM-dd"), 0);
            addCell(tr, games[o.GameId], 1);
            matchinfo="";
            if(o.RuleId >0)
            {
                if(o.RuleId <10)
                    matchinfo = getMatchName(o.GameId,o.MatchId)+"0"+o.RuleId;
                else
                    matchinfo = getMatchName(o.GameId,o.MatchId)+o.RuleId;
            }
            else
                matchinfo = getMatchName(o.GameId,o.MatchId);
            addCell(tr, matchinfo, 2);
            addCell(tr, o.Actives == 0 ? "":o.Actives, 3);            
            addCell(tr, (o.Win - o.Lose) == 0 ? "" : (o.Win - o.Lose), 4); 
            addCell(tr, o.Shrink == 0 ? "" : o.Shrink, 5);
            addCell(tr, o.ChannelId,6);
            return tr;
        }
        function selectGame(id){
            var list = match[id];
            $("#matchlist").empty();
            $("#matchlist").append("<option value=\"-1\">所有游戏场</option>");
            if(list ==null)
                return;
            for(var i =0;i<list.length;i++){
                $("#matchlist").append("<option value=\""+list[i][0]+"\">"+list[i][1]+"</option>");
            }
        } 
        $(document).ready(function() {
            for (var id in games)
                $("#game").append("<option value=\""+id+"\">"+games[id]+"</option>");
            attachCalenderbox('#starttime', '#overtime', null,new Date().dateAdd("d", -1).Format("yyyy-MM-dd"), new Date().Format("yyyy-MM-dd"));
            var pagerTitles = ["日期", "游戏名称", "场名称", "日活跃", "今日输赢", "服务费", "渠道"];
            jsonPager.init(ajax.getSceneReport, [], searchResult, pagerTitles, "list_table", "container", "pager", insertRow);
            jsonPager.dataBind(1, 0);
            var gameid=<%=gameid %>;
            if(gameid>0){
                $("#game").val(gameid);
                search();
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
            <div class="toolbar">场报表</div>
            <div class="search">&nbsp;&nbsp;
                <select id="game" onchange="selectGame(this.value)">
	                <option value="-1">所有游戏</option>
                </select>
                <select id="matchlist">
	                <option value="-1">所有游戏场</option>
                </select>
                开始日期:<input type="text" id="starttime" class="box w100" readonly="readonly" />
                截止日期:<input type="text" id="overtime" class="box w100" readonly="readonly" />
                <input type="button" value="查询" onclick="search()" class="ui-button-icon-primary" />
            </div>
            <div id="container"></div>
            <div id="pager" class="pager"></div>
            <div class="loading" id="loading"></div>
</asp:Content>
