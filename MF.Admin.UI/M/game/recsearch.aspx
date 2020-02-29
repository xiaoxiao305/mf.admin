<%@ Page  Title=" 游戏管理 》 游戏录像" MasterPageFile="~/M/main.Master" Language="C#" AutoEventWireup="true" CodeBehind="recsearch.aspx.cs" Inherits="MF.Admin.UI.M.game.recsearch" %>
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
        ajax.getGameRec(jsonPager.makeArgs(1), searchResult);
    }
    function searchResult(data) { 
        $("#loading").hide();
        if (data.code == 1) { 
            downLoad(data.result, 'rec.txt');
        }else{
            alert(data.msg);
        }
    }
    function downLoad(content, fileName) {
        if (!content || content == null || content == "" || content=="null") return;
        var aEle = document.createElement("a");// 创建a标签
        blob = new Blob([content]); 
        aEle.download = fileName;// 设置下载文件的文件名
        aEle.href = window.URL.createObjectURL(blob);
        aEle.click();// 设置点击事件
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <div class="toolbar">游戏录像</div>
    <div class="search">&nbsp;&nbsp;
         日期:<input type="text" id="time" class="box w100" readonly="readonly" />
        <select id="ddlshour"><option value="-1">时</option></select>
        <select id="ddlsmin"><option value="-1">分</option></select>
        至:<select id="ddlehour"><option value="-1">时</option></select>
        <select id="ddlemin"><option value="-1">分</option></select>
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
