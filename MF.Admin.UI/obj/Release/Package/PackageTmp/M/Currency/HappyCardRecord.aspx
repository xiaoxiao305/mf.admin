<%@ Page Title=" 游戏币管理 》 欢乐卡记录" MasterPageFile="~/M/main.Master" Language="C#" AutoEventWireup="true" CodeBehind="CurrencyRecord.aspx.cs" Inherits="MF.Admin.UI.M.Currency.CurrencyRecord" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server">
    <link href="/common/styles/layer.css" type="text/css" rel="Stylesheet" />
    <script language="javascript" type="text/javascript">
        var match= eval(<%=matchDic %>);
        var games=<%=gameDic %>;
        function search() {
            var checktime = $("#time").is(":checked") ? 1 : 0;
            var startTime = 0;
            var overTime = 0;
            if (checktime==1) {
                if ($("#starttime").val().trim() == "" || $("#overtime").val().trim() == "") {
                    alert("请选择要查询的时间范围");
                    return;
                } 
                startTime = new Date($("#starttime").val().replace(/-/g, "/")).dateDiff("s");
                overTime = new Date($("#overtime").val().replace(/-/g, "/") + " 23:59:59").dateDiff("s");
                if (overTime < startTime) {
                    alert("查询截止时间不能小于开始时间");
                    return;
                }
            }
            var args = [parseInt($("#game").val()), parseInt($("#matchlist").val()),parseInt($("#type").val()),"<%=account%>","", checktime, startTime, overTime,3];
            jsonPager.queryArgs = args;
            $("#loading").show();
            ajax.getCurrencyRecord(jsonPager.makeArgs(1), searchResult);
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
        var dicHappycardType = {1: "报名",  6: "退赛", 26: "充值赠送"};
        function insertRow(o, tr) {
            addCell = function(tr, text, i) {
                var td = tr.insertCell(i);
                td.innerHTML = text;
            };
            getTypeName = function(_type) { 
                if (dicHappycardType[_type]) return dicHappycardType[_type];
                return _type; 
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
            addCell(tr, new Date("2012/10/01").dateAdd("s",o.Time).Format("yyyy-MM-dd hh:mm:ss"), 0);
            addCell(tr, o.Account, 1); 
            addCell(tr, games[o.GameId.toString()]?games[o.GameId.toString()]:"", 2);
            addCell(tr, getMatchName(o.GameId.toString(),o.MatchId.toString())?getMatchName(o.GameId.toString(),o.MatchId.toString()):"", 3);
            addCell(tr, getTypeName(o.Type), 4);
            addCell(tr, o.Num, 5);
            addCell(tr, o.Original, 6);
            addCell(tr, o.IP==""?"":o.IP, 7);
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
            attachCalenderbox('#starttime', '#overtime', null,new Date().Format("yyyy-MM-dd") , new Date().Format("yyyy-MM-dd"));
            showTimeBox($$("time"));
            for (var id in games)
                $("#game").append("<option value=\""+id+"\">"+games[id]+"</option>");
            var pagerTitles = ["时间","用户账号","游戏名称","场名称","变更类型","变更数量","原欢乐卡数量","IP"];
            jsonPager.init(ajax.getCurrencyRecord, [], searchResult, pagerTitles, "list_table", "container", "pager", insertRow);
            jsonPager.dataBind(1, 0);
            if("<%=account%>" != "")
                search();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
            <div class="toolbar">查看用户欢乐卡记录</div>
            <div class="search">&nbsp;&nbsp;
               <select id="game" onchange="selectGame(this.value)">
	                <option value="-1">所有游戏</option>
                </select>
                <select id="matchlist">
	                <option value="-1">所有游戏场</option>
                </select>
                <select id="type">
	                <option value="-1">变更类型</option>
	                <option value="1">报名</option>
	                <option value="6">退赛</option>
	                <option value="26">充值赠送</option>
                </select>
                <input id="time" type="checkbox"  onclick="showTimeBox(this)" /><label for="time">时间</label>
                <span id="divTime" class="date" >
                    开始:<input type="text" id="starttime" class="box w100" readonly="readonly" />
                    截止:<input type="text" id="overtime" class="box w100" readonly="readonly" />
                </span>
                <input type="button" value="查询" onclick="search()" class="ui-button-icon-primary" />                
            </div>
            <div id="container"></div>
            <div id="pager" class="pager"></div>
            <div class="loading" id="loading"></div>
</asp:Content>
