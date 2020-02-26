<%@ Page Title=" 游戏币管理 》 德州2人包间记录" MasterPageFile="~/M/main.Master" Language="C#" AutoEventWireup="true" CodeBehind="DZCurrencyRecord.aspx.cs" Inherits="MF.Admin.UI.M.Currency.DZCurrencyRecord" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server">
    <link href="/common/styles/layer.css" type="text/css" rel="Stylesheet" />
    <script language="javascript" type="text/javascript">
        var match= eval(<%=matchDic %>);
        var games=<%=gameDic %>;
        function search() {
            var startTime = 0;
            var overTime = 0;
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
            var args = ["<%=account%>", startTime, overTime];
            jsonPager.queryArgs = args;
            $("#loading").show();
            ajax.getDZCurrencyRecord(jsonPager.makeArgs(1), searchResult);
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
            addCell(tr, getMatchName(o.GameId.toString(),o.MatchId.toString())?getMatchName(o.GameId.toString(),o.MatchId.toString()):"", 3);
            addCell(tr, getTypeName(o.Type), 4);
            addCell(tr, o.Num, 5);
            addCell(tr, o.Original, 6);
            addCell(tr, o.IP.split(":")[0], 7);
            return tr;
        }
        $(document).ready(function() {
            attachCalenderbox('#starttime', '#overtime', null,new Date().Format("yyyy-MM-dd") , new Date().Format("yyyy-MM-dd"));
            var pagerTitles = ["时间","用户账号","游戏名称","场名称","变更类型","变更数量","原元宝数量","IP"];
            jsonPager.init(ajax.getDZCurrencyRecord, [], searchResult, pagerTitles, "list_table", "container", "pager", insertRow);
            jsonPager.dataBind(1, 0);
            if("<%=account%>" != "")
                search();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <div class="toolbar">查看德州2人包间记录</div>
    <div class="search">&nbsp;&nbsp;
        开始:<input type="text" id="starttime" class="box w100" readonly="readonly" />
        截止:<input type="text" id="overtime" class="box w100" readonly="readonly" />
        <input type="button" value="查询" onclick="search()" class="ui-button-icon-primary" />
    </div>
    <div id="container"></div>
    <div id="pager" class="pager"></div>
    <div class="loading" id="loading"></div>
</asp:Content>
