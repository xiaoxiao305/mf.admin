<%@ Page Title="游戏管理》用户总留存率" Language="C#" MasterPageFile="~/M/main.Master" AutoEventWireup="true" CodeBehind="UserRetain.aspx.cs" Inherits="MF.Admin.UI.M.Report.UserRetain" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server">
    <script language="javascript" type="text/javascript">
       
        function search() {
            if ($("#starttime").val().trim() == "" || $("#overtime").val().trim() == "") {
                alert("请选择要查询的时间范围");
                return;
            }
            startTime = new Date($("#starttime").val().replace("-", "/")).dateDiff("s");
            overTime = new Date($("#overtime").val().replace("-", "/") + " 23:59:59").dateDiff("s");
            if (overTime < startTime) {
                alert("查询截止时间不能小于开始时间");
                return;
            }
            $("#loading").show();
            var args = [startTime, overTime];
            jsonPager.queryArgs = args;
            ajax.getUserRetain(jsonPager.makeArgs(1), searchResult);
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
        function insertRow(o,tr) {
            addCell = function(tr, text, i) {
                var td = tr.insertCell(i);
                td.innerHTML = text;
            };
            for (var i = 0; i < 5; i++)
                addCell(tr, o[i], i);
            return tr;
        }
        
        $(document).ready(function() {
            attachCalenderbox('#starttime', '#overtime', null, new Date().Format("yyyy-MM-dd"), new Date().Format("yyyy-MM-dd"));
            var pagerTitles = ["用户量","次留存","3日留存","7日留存","30日留存"];
            jsonPager.init(ajax.getUserRetain, [], searchResult, pagerTitles, "list_table", "container", "pager", insertRow);
            jsonPager.dataBind(1, 0);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
            <div class="toolbar">用户总留存率</div>
            <div class="search">&nbsp;&nbsp;
                注册开始日期:<input type="text" id="starttime" class="box w100" readonly="readonly" />
                注册截止日期:<input type="text" id="overtime" class="box w100" readonly="readonly" />
                <input type="button" value="查询" onclick="search()" class="ui-button-icon-primary" />
            </div>
            <div id="container"></div>
            <div id="pager" class="pager"></div>
            <div class="loading" id="loading"></div>
</asp:Content>