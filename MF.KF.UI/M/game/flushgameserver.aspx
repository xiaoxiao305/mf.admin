<%@ Page Title=" 管理设置 》 刷新游戏服务器" MasterPageFile="~/M/main.Master" Language="C#" AutoEventWireup="true" CodeBehind="flushgameserver.aspx.cs" Inherits="MF.KF.UI.M.game.flushgameserver" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server">
<script language="javascript" type="text/javascript">
    function search() {
        $("#loading").show(); 
        jsonPager.queryArgs = [];
        jsonPager.pageSize = 1000;
        ajax.getGameServerList(jsonPager.makeArgs(1), searchResult);
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
        var date=new Date("2012/10/1")
        addCell(tr, "<input type='checkbox' name='gname' value='"+o.gname+"'/>", 0);
        addCell(tr, o.gname, 1);
        return tr;
    }
    function flushGameServer()
    {
        jqchk();
    }
    function flushMatchGame()
    {
        if (confirm("确认要刷新游戏数据库【MatchGame】配置？"))
            ajax.flushMatchGame("flushmatchgame", chkResult);
    }
    function jqchk() { //jquery获取复选框值
        var chk_value = [];
        $('input[name="gname"]:checked').each(function () {
            chk_value.push($(this).val());
        });
        if (chk_value.length == 0)
        {
            alert("你还没有选择任何服务器");
            return;
        }
        if (confirm("确认要刷新所选游戏服务器配置？"))
            ajax.flushGameServer("flushgameserver", [chk_value.toString()], chkResult);
    }
    function chkResult(res) {
        $("#loading").hide(); 
        if (res.code == 1)
            alert("success");
        else
            alert(res.code);
    }

    $(document).ready(function() {
        var pagerTitles = ["选择", "服务器名称"];
        jsonPager.init(ajax.getGameServerList, [], searchResult, pagerTitles, "list_table", "container", "pager", insertRow);
        jsonPager.dataBind(1, 0);
    });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <div class="toolbar">刷新游戏服务器</div>
    <div class="search">&nbsp;&nbsp;
        <input type="button" value="获取服务器列表" onclick="search()" class="ui-button-icon-primary" />
        <input type="button" value="刷新游戏服务器配置" onclick="flushGameServer()" class="ui-button-icon-primary" />
        <input type="button" value="刷新游戏数据库配置" onclick="flushMatchGame()" class="ui-button-icon-primary" />
    </div>    
    <div id="container"></div>
    <div id="pager" class="pager"></div>
    <div class="loading" id="loading"></div>
</asp:Content>

