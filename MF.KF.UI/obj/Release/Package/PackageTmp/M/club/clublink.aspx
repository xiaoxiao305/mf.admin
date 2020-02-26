<%@ Page Title=" 俱乐部管理》俱乐部关联" MasterPageFile="~/M/main.Master" Language="C#" AutoEventWireup="true" CodeBehind="clublink.aspx.cs" Inherits="MF.KF.UI.M.club.clublink" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server">
    <link href="/common/styles/layer.css" type="text/css" rel="Stylesheet" />
    <script language="javascript" type="text/javascript">
        function search() {
            var args = [parseInt($("#key").val())];
            jsonPager.queryArgs = args;
            $("#loading").show();
            ajax.getGuildLinkList(jsonPager.makeArgs(1), searchResult);
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
            addCell = function (tr, text, i) {
                var td = tr.insertCell(i);
                if (i != 2) {
                      td.innerHTML = text;
                } else {
                    var newText = "";
                    for (var j = 0; j < text.linkclub.length; j++) {                    
                        newText += text.linkclub[j].Id + "," + text.linkclub[j].Name + "&nbsp;&nbsp;&nbsp;      <a href='javascript:void (0);' onclick=\"delclublinkwin(" + o.clubid + ",'"+o.name+"',"+text.linkclub[j].Id+",'"+text.linkclub[j].Name+"');\">删除关联</a>  <br/>";
                        td.innerHTML = newText;
                    }
                }
            };
            addCell(tr, o.clubid, 0);
            addCell(tr, o.name, 1);
            addCell(tr,o, 2);
            return tr;
        }
        $(document).ready(function() {
            var pagerTitles = ["Id", "名称", "关联俱乐部"];
            jsonPager.init(ajax.getGuildLinkList, [], searchResult, pagerTitles, "list_table", "container", "pager", insertRow);
            jsonPager.dataBind(1, 0);
        }); 
        
        function addclublink(clubid,clublinkid,token)
        {
            if ($("#clubid").val() == "") {
                $("#lblerr").html("请输入关联俱乐部Id。");
                return;
            } else if ($("#clublinkid").val() == "") {
                $("#lblerr").html("请输入被关联俱乐部Id。");
                return;
            } 
            ajax.setClubLink("setclublink", [1,parseInt($("#clubid").val()), parseInt($("#clublinkid").val())], winresult);
        }
        function delclublinkwin(clubid,name,clublinkid,clublinkname)
        {
            $("#lblClubId").html(clubid);
            $("#lblName1").html(name);
            $("#lblClubLinkId").html(clublinkid);
            $("#lblNameLink").html(clublinkname);
            showAddMoneyWin(2);
        }
        function delclublink() { 
            ajax.setClubLink("setclublink", [2,parseInt($("#lblClubId").html()), parseInt($("#lblClubLinkId").html())], winresult);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <div class="toolbar">俱乐部关联记录列表</div>
    <div class="search">&nbsp;&nbsp;
        俱乐部Id：
        <input  type="text" id="key" class="box" />
        <input type="button" value="查询" onclick="search()" class="ui-button-icon-primary" />
        <input type="button" value="设置俱乐部关联" onclick="showAddMoneyWin(1)" class="ui-button-icon-primary oprbutton"/>
    </div>
    <div id="container"></div>
    <div id="pager" class="pager"></div>
    <div class="loading" id="loading"></div>
    <!--弹出窗口开始-->
    <div class="theme-popover" style="height: 400px;">
        <div class="theme-poptit">
            <a href="javascript:;" title="关闭" class="close">×</a>
            <h3 id="msgtitle">设置俱乐部关联</h3>
        </div>
        <div class="theme-popbod dform">
            <div id="content"></div>
        </div>
    </div>
    <div class="theme-popover-mask"></div>
    <!--弹出窗口结束-->
    <div class="hide">
         <ul id="T1">
            <li style="text-align:left;">&nbsp;&nbsp;&nbsp;　　　俱乐部Id：<input class="ipt" type="text" id="clubid" /></li>
            <li>&nbsp;&nbsp;关联俱乐部Id：<input class="ipt" type="text" id="clublinkid" /></li> 
            <li class="err red" id="lblerr"></li>
            <li><input class="btn btn-primary" type="button" value=" 确 定" onclick="addclublink()" /></li>
        </ul>
        <ul id="T2">
            <li class="red">请仔细核对如下信息，确认是否删除俱乐部关联</li>
            <li>俱乐部Id：<label id="lblClubId" style="color:#d5932b;" class=""></label></li>
            <li>俱乐部名称：<label id="lblName1" style="color:#d5932b;"></label></li>
            <li>关联俱乐部Id：<span id="lblClubLinkId" style="color:#d5932b;"></span></li>                
            <li>关联俱乐部名称：<span id="lblNameLink" style="color:#d5932b;"></span></li>     
            <li class="err red" id="lblerr2"></li>
            <li><input class="btn btn-primary" type="button" value=" 确 定" onclick="delclublink()" /></li>
        </ul>
    </div>
</asp:Content>