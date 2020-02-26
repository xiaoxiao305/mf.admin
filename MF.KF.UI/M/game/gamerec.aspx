<%@ Page Title=" 游戏管理 》 游戏录像" MasterPageFile="~/M/main.Master" Language="C#" AutoEventWireup="true" CodeBehind="gamerec.aspx.cs" Inherits="MF.KF.UI.M.game.gamerec" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server">
<script language="javascript" type="text/javascript">
    var games =  <%=gameDic %>;
    $(document).ready(function () {
        for (var id in games) {
            $(".game").append("<option value=\"" + id + "\">" + games[id] + "</option>");
        }
    });
    function changeGame() {
        $(".hidrecUrl").val("");
        var gameId = $(".game").val();
        if (gameId <1) 
            return;
        var gameInfo = GetGameRecModel(gameId);
        if (gameInfo == null) {
            alert("游戏配置有误");
            return;
        };
        var recurl = gameInfo.recurl;
        if (recurl==""){
            alert("游戏路径配置有误");
            return;
        };
        $(".hidrecUrl").val(recurl);
    }
    function batch_download(ids) {
        var tmp_array = [];
        tmp_array = ids.split(",")

        //download
        $("iframe").remove();  //清除页面上上一次存在的iframe
        window.ids_array = tmp_array;   //定义全局变量用来获取要下载文件的id
        download();
    }

    function download() {
        if (window.ids_array.length > 0) {
            $("body").append("<iframe src=download?file=" + window.ids_array.pop() >+"</iframe >");
            setTimeout(download, 1); //等待1毫秒后执行递归
        }
    }


</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
            <div class="toolbar">下载游戏录像</div>
            <div class="search">&nbsp;&nbsp;
                <select id="ddlGame" runat="server" onchange="changeGame()" class="game"><option value="-1">请选择游戏</option></select>
                <asp:Button Text="下载昨天录像" OnClick="DownYesterday" CssClass="ui-button-icon-primary" runat="server"/>
                <asp:Button Text="下载今天录像" OnClick="DownToday" CssClass="ui-button-icon-primary" runat="server"/>
                <asp:Button Text="下载日志" OnClick="DownWarring" CssClass="ui-button-icon-primary" runat="server"/>
            </div>    
            <div id="container"></div> 
            <div class="loading" id="loading"></div>
    <input type="hidden" id="hidrecUrl" runat="server" class="hidrecUrl"/>
</asp:Content>
