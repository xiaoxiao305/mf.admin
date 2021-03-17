<%@ Page Title=" 游戏管理 》 游戏收益" MasterPageFile="~/M/main.Master" Language="C#" AutoEventWireup="true" CodeBehind="gameincome.aspx.cs" Inherits="MF.KF.UI.M.game.gameincome" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server"> 
<script language="javascript" type="text/javascript">
    var games =  <%=blackGameList %>;
    var time = "<%=time%>";
    var ho = "<%=hour%>";
    var min = "<%=min%>";
    var sec = "<%=sec%>";
    var eho = "<%=ehour%>";
    var emin = "<%=emin%>";
    var esec = "<%=esec%>";

    var gameId = "<%=gameId%>";
    var roomId = "<%=roomId%>";
    var chargeId = "<%=chargeId%>";
    var oprGame = null;
    $(document).ready(function () {
        if (time != "")
            attachCalenderbox('#time', null, null, new Date(time).Format("yyyy-MM-dd"), null);
        else
            attachCalenderbox('#time', null, null, null, null);
        var isSelected = "";
        for (var h = 0; h < 24; h++) {
            var hStr = h;
            if (h < 10) hStr = "0" + h;
            isSelected = "";
            if (hStr == ho) isSelected = "selected";
            $("#ddlshour").append("<option value=\"" + h + "\" " + isSelected + ">" + hStr + "</option>");
            isSelected = "";
            if (hStr == eho) isSelected = "selected";
            $("#ddlehour").append("<option value=\"" + h + "\" " + isSelected + ">" + hStr + "</option>");
        }
        for (var m = 0; m <= 59; m++) {
            var mStr = m;
            if (m < 10) mStr = "0" + m;
            isSelected = "";
            if (mStr == min) isSelected = "selected";
            $("#ddlsmin").append("<option value=\"" + m + "\" " + isSelected + ">" + mStr + "</option>");
            isSelected = "";
            if (mStr == emin) isSelected = "selected";
            $("#ddlemin").append("<option value=\"" + m + "\" " + isSelected + ">" + mStr + "</option>");
        }
        for (var s = 0; s <= 59; s++) {
            var sStr = s;
            if (s < 10) sStr = "0" + s;
            isSelected = "";
            if (sStr == sec) isSelected = "selected";
            $("#ddlssec").append("<option value=\"" + s + "\" " + isSelected + ">" + sStr + "</option>");
            isSelected = "";
            if (sStr == esec) isSelected = "selected";
            $("#ddlesec").append("<option value=\"" + s + "\" " + isSelected + ">" + sStr + "</option>");
        }
        for (var id in games) {
            isSelected = "";
            if (id == gameId) isSelected = "selected";
            $("#ddlGame").append("<option value=\"" + id + "\" " + isSelected + ">" + games[id] + "</option>");
        }
        $("#roomid").val(roomId);
        $("#chargeid").val(chargeId);
        var pagerTitles = ["游戏时间", "游戏编号", "场ID", "包间号", "局号", "UID", "昵称", "本局收益", "本局抽水", "录像"];
        jsonPager.init(ajax.getGameIncome, [], searchResult, pagerTitles, "list_table", "container", "pager", insertRow);
        jsonPager.dataBind(1, 0);
        if (time != "" && gameId != "")
            search(1);
    });
    function search(type) {
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
        if (chargeid.substring(0, 3).trim().toUper == "AAA") {
            alert("请输入正确的UID");
            return;
        }
        $("#loading").show();
        var sh = $("#ddlshour").val() == "-1" ? "0" : $("#ddlshour").val();
        var sm = $("#ddlsmin").val() == "-1" ? "0" : $("#ddlsmin").val();
        var ss = $("#ddlssec").val() == "-1" ? "0" : $("#ddlssec").val();
        var start = d + " " + sh + ":" + sm + ":" + ss;
        var eh = $("#ddlehour").val() == "-1" ? "23" : $("#ddlehour").val();
        var em = $("#ddlemin").val() == "-1" ? "59" : $("#ddlemin").val();
        var es = $("#ddlesec").val() == "-1" ? "59" : $("#ddlesec").val();
        var end = d + " " + eh + ":" + em + ":" + es;
        start = new Date(start.replace(/-/g, "/")).dateDiff("s");
        end = new Date(end.replace(/-/g, "/")).dateDiff("s");
        oprGame = gameid;
        var args = [start, end, gameInfo.recurl, chargeid, roomid, $("#number").val()];
        jsonPager.queryArgs = args;
        jsonPager.pageSize = 1000;
        if (type == 1)
            ajax.getGameIncome(jsonPager.makeArgs(1), searchResult);
        else if (type == 2)
            ajax.getGameRec(jsonPager.makeArgs(1), searchLogResult);
        else if (type == 3) {
            var args = [start, end, gameInfo.recurl, chargeid, roomid, $("#number").val()];
            ajax.getGameRec(jsonPager.makeArgs(1), searchLogResult);
        }
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
        // ["游戏时间","游戏编号","场ID","包间号","局号", "UID","账号","昵称","本局收益","本局抽水"];
        addCell = function (tr, text, i) {
            var td = tr.insertCell(i);
            td.innerHTML = text;
        };
        addCell(tr, new Date("2012/10/01").dateAdd("s", o.Time).Format("yyyy-MM-dd hh:mm:ss"), 0);
        addCell(tr, o.Child_Game_Index, 1);
        addCell(tr, o.MatchId, 2);
        addCell(tr, o.RoomId, 3);
        addCell(tr, o.Number, 4);
        var chargeidStr = "";
        if (o.ChargeIdList && o.ChargeIdList.length > 0) {
            for (var i = 0; i < o.ChargeIdList.length; i++) {
                chargeidStr += "<a href='/M/currency/CurrencyRecord.aspx?chargeid=" + o.ChargeIdList[i] + "' target='_blank'>" + o.ChargeIdList[i] + "</a><br/>";
            }
        }
        addCell(tr, chargeidStr, 5);
        addCell(tr, initNick(o.NickList.toString()), 6);
        addCell(tr, o.IncomeList.toString().replace(/,/gi, "<br/>"), 7);
        addCell(tr, o.InterestList.toString().replace(/,/gi, "<br/>"), 8);
		var t =new Date(new Date("2012/10/01").dateAdd("s", o.Time).Format("yyyy-MM-dd")).dateDiff("s")-28800;//time yyyy-MM-dd 0:00:00
        addCell(tr, "<a href='javascript:;' onclick=\"downUIDLog(" + t+ ",'" + o.ChargeIdList[0] + "','" + o.RoomId + "','" + o.Number + "')\">下载</a>", 9);
        return tr;
    }
    function downUIDLog(time, chargeid, roomid, number) {
        $("#loading").show();
        if (oprGame == null || oprGame == "") return;
        var gameInfo = GetGameRecModel(oprGame);
        if (gameInfo == null) return;
        var endtime=time+86399;//time yyyy-MM-dd 0:00:00 endtime yyyy-MM-dd 23:59:59
        var args = [time, endtime, gameInfo.recurl, chargeid, roomid, number];
        jsonPager.queryArgs = args;
        jsonPager.pageSize = 1000;
        ajax.getGameRec(jsonPager.makeArgs(1), searchLogResult);
    }
    function searchLogResult(data) {
        $("#loading").hide();
        if (data.code == 1) {
            downLoad(data.result, 'rec.txt');
        } else {
            alert(data.msg);
        }
    }
    function downLoad(content, fileName) {
        if (!content || content == null || content == "" || content == "null") return;
        var aEle = document.createElement("a");// 创建a标签
        blob = new Blob([content]);
        aEle.download = fileName;// 设置下载文件的文件名
        aEle.href = window.URL.createObjectURL(blob);
        aEle.click();// 设置点击事件
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <div class="toolbar">游戏收益</div>
    <div class="search">&nbsp;&nbsp;  
        日期:<input type="text" id="time" class="box w100" readonly="readonly" />
        <select id="ddlshour" class="game"><option value="-1">时</option></select>
        <select id="ddlsmin" class="game"><option value="-1">分</option></select>
        <select id="ddlssec" class="game"><option value="-1">秒</option></select>
        至:<select id="ddlehour" class="game"><option value="-1">时</option></select>
        <select id="ddlemin" class="game"><option value="-1">分</option></select>
        <select id="ddlesec" class="game"><option value="-1">秒</option></select>
        <select id="ddlGame" class="game"><option value="-1">请选择游戏</option></select>
        UID：<input type="text" id="chargeid" style="width:100px;"/>
        包间号:<input type="text" id="roomid" style="width:100px;"/>
        局号:<input type="text" id="number" style="width:100px;"/>
        <input type="button" value="查询" onclick="search(1)" class="ui-button-icon-primary" />　
        <input type="button" value="批量下载" onclick="search(2)" class="ui-button-icon-primary" />　
    </div>
    <p></p>
    <div id="container"></div>
    <div id="pager" class="pager"></div>
    <div class="loading" id="loading"></div>
</asp:Content>
