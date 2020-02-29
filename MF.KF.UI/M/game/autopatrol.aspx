﻿<%@ Page Title="报表管理 》 游戏巡场" MasterPageFile="~/M/main.Master" Language="C#" AutoEventWireup="true" CodeBehind="autopatrol.aspx.cs" Inherits="MF.KF.UI.M.game.autopatrol" %>
<asp:Content ID="content1" ContentPlaceHolderID="h" runat="server">
    <script language="javascript" type="text/javascript">        
        $(document).ready(function () { 
            var pagerTitle = ["游戏时间", "游戏名称", "包间号", "UID", "昵称","俱乐部"];
            jsonPager.init(ajax.GetLastGameRecords, [], seaResult, pagerTitle, "list_table", "container", "pager", insertRow);
            jsonPager.dataBind(0, 1);
            search();
            setInterval(function () {
                search();
            }, 600000);
        });
        function search() {
            $("#loading").show();
            jsonPager.queryArgs = [];
            jsonPager.pageSize = 1000;
            ajax.GetLastGameRecords(jsonPager.makeArgs(1), seaResult);
        }
        function seaResult(data) {
            $("#loading").hide();
            if (data.code == 1) {
                jsonPager.data = data.result;
                jsonPager.dataBind(data.index, data.rowCount);
            } else {
                alert(data.msg);
            }
        }
        function insertRow(o, tr) {
            addCell = function (tr, text, i) {
                var td = tr.insertCell(i);
                td.innerHTML = text;
            };
            addCell(tr, new Date("2012/10/01").dateAdd("s", o.TimeStamp).Format("yyyy-MM-dd hh:mm:ss"), 0);
            addCell(tr, o.GameName, 1);  
            addCell(tr, "<a href='/m/game/gameincome.aspx?time=" + o.TimeStamp + "&gameId=" + o.GameId + "&roomId=" + o.RoomId + "'>" + o.RoomId + "</a>", 2); 
            addCell(tr, o.ChargeIds.toString().replace(/,/gi, "<br/>"), 3);
            addCell(tr, initNick(o.NickNames.toString()), 4);
            addCell(tr, o.ClubIds.toString().replace(/,/gi, "<br/>"), 5);
            return tr;
        }
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="p" runat="server">
    <div class="toolbar">游戏巡场</div>
    <div class="search">&nbsp;&nbsp;
        <input type="button" value="查询" onclick="search()" class="ui-button-icon-primary" />
    </div>
    <p></p>
    <div id="container"></div>
    <div class="pager" id="pager"></div>
    <div class="loading" id="loading"></div>
</asp:Content> 