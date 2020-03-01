<%@ Page Title=" 游戏管理 》 游戏警告设置" MasterPageFile="~/M/main.Master"Language="C#" AutoEventWireup="true" CodeBehind="gamealertconfig.aspx.cs" Inherits="MF.Admin.UI.M.game.gamealertconfig" %>
 <asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server">
    <style type="text/css">
       li {display:block; text-align:left;padding-left:10%;}
       .center{text-align:center;}
   </style>
    <link href="/common/styles/layer.css" type="text/css" rel="Stylesheet" /> 
    <script language="javascript" type="text/javascript"> 
        var games =  <%=blackGameList %>;  
        
        function setredalert() {
            //川麻
            ajax.setRedAlert("setredalert", ["mahjong4", "10000"], resResult);
        } 
        function resResult(res) {
            console.log("resresult:", res);
        }

        function searchConfig() {
            var pagerTitles = ["添加时间", "游戏", "账号", "UID", "昵称", "值", "档位", "备注", "操作"];
            jsonPager.init(ajax.getRedAlert, [], searchResult, pagerTitles, "list_table", "container", "pager", insertRow);
            jsonPager.dataBind(1, 0);

            jsonPager.queryArgs = [];
            jsonPager.pageSize = 1000;
            ajax.getRedAlert(jsonPager.makeArgs(1), searchResult); 
        }
        function delredalert() {
            ajax.setRedAlert("delredalert", ["mahjong4", "10000"], resResult);
        }

        function search() { 
            var args = ["mahjong4",1,""];
            jsonPager.queryArgs = args;
            jsonPager.pageSize = 1000;
            ajax.getRedAlertPlayer(jsonPager.makeArgs(1), searchResult);
        }
        function searchResult(data) {
            console.log("searchResult data:", data);
            if (data.code == 1) {
                jsonPager.data = data.result;
                jsonPager.dataBind(data.index, data.rowCount);
            } else {
                alert(data.msg);
            }
        }
        function insertRow(o, tr) {
            //addCell = function (tr, text, i) {
            //    var td = tr.insertCell(i);
            //    if (i == 7)
            //        td.style.width = "200px";
            //    td.innerHTML = text;
            //};
            //var date = new Date("2012/10/1");
            //addCell(tr, date.dateAdd("s", o.CreateDate).format("yyyy-MM-dd hh:mm:ss"), 0);
            //addCell(tr, GetGameName(o.GameId), 1);
            //addCell(tr, o.Account, 2);
            //addCell(tr, o.ChargeId, 3);
            //addCell(tr, o.NickName, 4);
            //addCell(tr, o.Value, 5);
            //addCell(tr, getBlackLevelStr(o.Level), 6);
            //if (o.Remark && o.Remark.length > 0 && o.Remark.indexOf('\'') >= 0) {
            //    o.Remark = o.Remark.replace(/\'/g, "");
            //}
            //addCell(tr, o.Remark, 7);
            //addCell(tr, "<a href='javascript:;' onclick='updateBlackUser(" + JSON.stringify(o) + ")'>修改</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" +
            //    "<a href='javascript:;' onclick='setWinMoney(" + JSON.stringify(o) + ")'>删除</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" +
            //    "<a href='javascript:;' onclick='getGameMoney(" + JSON.stringify(o) + ")'>查看输赢值</a><br />" +
            //    "<a href='javascript:;' onclick='downloadLog(" + JSON.stringify(o) + ",2)'>下载录像</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" +
            //    "<a href='javascript:;' onclick='confirmUser(" + JSON.stringify(o) + ")'>确认实锤</a><br/>", 8);
            return tr;
        } 
        //$(document).ready(function () { 
        //    var pagerTitles = ["添加时间", "游戏", "账号", "UID", "昵称", "值", "档位", "备注", "操作"];
        //    jsonPager.init(ajax.getAuditBlackUsers, [], searchResult, pagerTitles, "list_table", "container", "pager", insertRow);
        //    jsonPager.dataBind(1, 0);
        //});
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <div class="toolbar">游戏警告设置</div>
    <div class="search">&nbsp;&nbsp;
        <input type="button" value="添加" onclick="setredalert()" class="ui-button-icon-primary" />　 
        <input type="button" value="查询配置" onclick="searchConfig()" class="ui-button-icon-primary" />　 
        <input type="button" value="删除" onclick="delredalert()" class="ui-button-icon-primary" />　 
        <input type="button" value="查询成员" onclick="search()" class="ui-button-icon-primary" />　 
    </div> 
    <div id="container"></div>
    <div id="pager" class="pager"></div>
    <div class="loading" id="loading"></div> 
</asp:Content>