<%@ Page Title="游戏币管理》查看用户金豆变更记录" Language="C#" MasterPageFile="~/M/main.Master" AutoEventWireup="true" CodeBehind="BeanRecord.aspx.cs" Inherits="MF.Admin.UI.M.Currency.BeanRecord" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server">
    <script language="javascript" type="text/javascript">
        var match=eval(<%=matchDic %>);
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
            var args = [parseInt($("#game").val()), parseInt($("#matchlist").val()),parseInt($("#type").val()),"<%=account%>","", checktime, startTime, overTime,2];
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
        function insertRow(o, tr) {
            addCell = function(tr, text, i) {
                var td = tr.insertCell(i);
                td.innerHTML = text;
            };
            getTypeName = function(_type) { 
                if (dicType[_type]) return dicType[_type];
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
            addCell(tr, getMatchName(o.GameId.toString(),o.MatchId.toString()), 3); 
            addCell(tr, getTypeName(o.Type), 4);
            addCell(tr, o.Num, 5);
            addCell(tr, o.Original, 6);
            addCell(tr, o.IP, 7);
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
            attachCalenderbox('#starttime', '#overtime', null, new Date().Format("yyyy-MM-dd"), new Date().Format("yyyy-MM-dd"));
            for (var id in games)
                $("#game").append("<option value=\""+id+"\">"+games[id]+"</option>");
            var pagerTitles = ["时间","用户账号","游戏名称","场名称","变更类型","变更数量","原金豆数量","IP"];
            jsonPager.init(ajax.getCurrencyRecord, [], searchResult, pagerTitles, "list_table", "container", "pager", insertRow);
            jsonPager.dataBind(1, 0);
            if("<%=account%>" != "") search();
        });        
    </script>
    <link href="/common/styles/layer.css" type="text/css" rel="Stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <div class="toolbar">查看用户金豆记录</div>
    <div class="search">&nbsp;&nbsp;
        <select id="game" onchange="selectGame(this.value)">
	        <option value="-1">所有游戏</option>
        </select>
        <select id="matchlist">
	        <option value="-1">所有游戏场</option>
        </select>
        <select  id="type">
	        <option value="-1">变更类型</option>
	        <option value="1">比赛产出</option>
	        <option value="2">任务赠送</option>
	        <option value="3">商城兑换</option>
	        <option value="4">单机版赠送</option>
	        <option value="5">管理员发放</option>
        </select>
        <input id="time" type="checkbox"  onclick="showTimeBox(this)"/><label for="time">时间</label>
        <span id="divTime" class="date" >
            开始:<input type="text" id="starttime" class="box w100" readonly="readonly" />
            截止:<input type="text" id="overtime" class="box w100" readonly="readonly" />
        </span>
        <input type="button" value="查询" onclick="search()" class="ui-button-icon-primary" />
        <input type="button" value="增加金豆" onclick="showAddMoneyWin(1)" class="ui-button-icon-primary oprbutton" />
    </div>
    <div id="container"></div>
    <div id="pager" class="pager"></div>
    <div class="loading" id="loading"></div>
    <!--弹出窗口开始-->
    <div class="theme-popover" style="height: 400px;">
        <div class="theme-poptit">
            <a href="javascript:;" title="关闭" class="close">×</a>
            <h3 id="msgtitle">添加金豆</h3>
        </div>
        <div class="theme-popbod dform">
            <div id="content"></div>
        </div>
    </div>
    <div class="theme-popover-mask"></div>
    <!--弹出窗口结束-->
    <div class="hide">
         <ul id="T1">
            <li>金豆数量：<input class="ipt" type="text" id="num" /></li>
            <li>安&nbsp;全&nbsp;令：<input class="ipt" type="text" id="token" /></li>
            <li class="err red" id="lblerr"></li>
            <li><input class="btn btn-primary" type="button" value=" 确 定" onclick="addusermoney('<%=account %>',2)" /></li>
            <li class="err red">注：该金豆直接派发至用户账上。</li>
        </ul>
        <ul id="T2">
            <li class="red">请仔细核对如下信息，确认是否添加金豆</li>
            <li>&nbsp;&nbsp;&nbsp;&nbsp;账&nbsp;&nbsp;&nbsp;&nbsp;号：<%=account %></li>
            <li>金豆数量：<span id="confirmnum"></span></li>                
            <li><input class="btn btn-primary" type="button" value=" 确 定" onclick="confirmopr('<%=account %>',2)" /></li>
        </ul>
        <input type="hidden"  id="hidNum" />
        <input type="hidden"  id="hidToken" />
    </div>
</asp:Content>
