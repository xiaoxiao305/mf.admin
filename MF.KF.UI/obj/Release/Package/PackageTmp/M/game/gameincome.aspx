<%@ Page Title=" 报表管理 》 游戏收益" MasterPageFile="~/M/main.Master" Language="C#" AutoEventWireup="true" CodeBehind="gameincome.aspx.cs" Inherits="MF.KF.UI.M.game.gameincome" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server"> 
<script language="javascript" type="text/javascript">
    var games =  <%=blackGameList %>;
    $(document).ready(function () {  
        attachCalenderbox('#time', null, null);
        for (var h = 0; h < 24; h++) {
            var hStr = h;
            if (h < 10) hStr = "0" + h;
            $("#ddlshour").append("<option value=\"" + h + "\">" + hStr + "</option>");
            $("#ddlehour").append("<option value=\"" + h + "\">" + hStr + "</option>");
        }
         for (var m = 0; m <= 59; m++) {
            var mStr = m;
            if (m < 10) mStr = "0" + m;
            $("#ddlsmin").append("<option value=\"" + m + "\">" + mStr + "</option>");
            $("#ddlemin").append("<option value=\"" + m + "\">" + mStr + "</option>");
        }
        for (var id in games) {
            $("#ddlGame").append("<option value=\"" + id + "\">" + games[id] + "</option>");
        }
        var pagerTitles = ["游戏时间","游戏编号","场ID","包间号","局号", "UID","昵称","本局收益","本局抽水"];
        jsonPager.init(ajax.getGameIncome, [], searchResult, pagerTitles, "list_table", "container", "pager", insertRow);
        jsonPager.dataBind(1,0);
    }); 
    function search() {
        var d = $("#time").val();
        if (d == "") {
            alert("请选择查询日期");
            return;
        }
        var gameid = parseInt($("#ddlGame").val());
        var gameInfo = GetGameRecModel(gameid); 
        if (gameInfo == null) {
            alert("请选择游戏");
            return;
        }
        var chargeid = $("#chargeid").val();
        var roomid = $("#roomid").val();
        if (chargeid == "" && roomid == "") {
            alert("请输入查询UID或包间号");
            return;
        }
        $("#loading").show();
        var sh = $("#ddlshour").val() == "-1" ? "0" : $("#ddlshour").val();
        var sm = $("#ddlsmin").val()=="-1"?"0":$("#ddlsmin").val();
        var start = d + " " + sh + ":" + sm + ":00";
        var eh = $("#ddlehour").val() == "-1" ? "23" : $("#ddlehour").val();
        var em = $("#ddlemin").val()=="-1"?"59":$("#ddlemin").val();
        var end = d + " " + eh + ":" + em + ":59";
        start = new Date(start.replace(/-/g, "/")).dateDiff("s");
        end = new Date(end.replace(/-/g, "/")).dateDiff("s"); 
        var args = [start, end, gameInfo.recurl, chargeid, roomid, $("#number").val()];
        jsonPager.queryArgs = args;
        jsonPager.pageSize = 1000;
        ajax.getGameIncome(jsonPager.makeArgs(1), searchResult);
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
        // ["游戏时间","游戏编号","场ID","包间号","局号", "UID","账号","昵称","本局收益","本局抽水"];
        addCell = function(tr, text, i) {
            var td = tr.insertCell(i);
            td.innerHTML = text;
        };
        addCell(tr, new Date("2012/10/01").dateAdd("s", o.Time).Format("yyyy-MM-dd hh:mm:ss"), 0);
        addCell(tr, o.Child_Game_Index, 1);
        addCell(tr, o.MatchId, 2);
        addCell(tr, o.RoomId, 3);
        addCell(tr, o.Number, 4);
       addCell(tr, o.ChargeIdList.toString().replace(/,/gi, "<br/>"), 5);
        addCell(tr, initNick(o.NickList.toString()), 6);
        addCell(tr, o.IncomeList.toString().replace(/,/gi, "<br/>"),7);
        addCell(tr, o.InterestList.toString().replace(/,/gi, "<br/>"),8);
        return tr;
    }
    function initNick(nickList) {
        if (nickList == "") return "";
        var nickArr = nickList.split(",");
        var list = "";
        for (var j = 0; j < nickArr.length; j++) {
            var nick = getEmoji(nickArr[j]);
            list +=nick+ "</br>";
        }
        return list;
    }
    function getEmoji(nick) {
        if (nick == "") return "";
        if (nick.indexOf("\\U000") >= 0) {//emoji
             var nicks = nick.split(" ");//多个emoji
            var newNick="";
            for (var i = 0; i < nicks.length; i++) {
                var emojiIndex = nicks[i].indexOf("\\U000");
                //console.log("emojiIndex:", emojiIndex);
                if (emojiIndex >= 0) {
                    var before = nicks[i].substring(0, emojiIndex)||"";
                    //console.log("before:", before);
                    var emojiTmp = nicks[i].substring(emojiIndex,emojiIndex+10);
                    //console.log("emojiTmp:", emojiTmp);
                    var emojiStr = emojiTmp.replace("\\U000", "0x"); //6字节emoji
                    //console.log("emojiStr:", emojiStr);
                    var emoji = String.fromCodePoint(emojiStr)||"";
                    //console.log("emoji:", emoji);
                    var after = nicks[i].substring(emojiIndex + 10)||"";
                    //console.log("after:", after);
                    newNick += before + emoji + after+" ";
                    //console.log("newNick:", newNick);
                } else
                    newNick += nicks[i]+" ";
            }
            return newNick;
        }
        else
            return nick;
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <div class="toolbar">游戏收益</div>
    <div class="search">&nbsp;&nbsp;  
        日期:<input type="text" id="time" class="box w100" readonly="readonly" />
        <select id="ddlshour" class="game"><option value="-1">时</option></select>
        <select id="ddlsmin" class="game"><option value="-1">分</option></select>
        至:<select id="ddlehour" class="game"><option value="-1">时</option></select>
        <select id="ddlemin" class="game"><option value="-1">分</option></select>
        <select id="ddlGame" class="game"><option value="-1">请选择游戏</option></select>
        UID：<input type="text" id="chargeid"/>
        包间号:<input type="text" id="roomid"/>
        局号:<input type="text" id="number"/>
        <input type="button" value="查询" onclick="search()" class="ui-button-icon-primary" />　
    </div>
    <p></p>
    <div id="container"></div>
    <div id="pager" class="pager"></div>
    <div class="loading" id="loading"></div>
</asp:Content>
