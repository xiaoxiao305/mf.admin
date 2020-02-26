<%@ Page Title="" Language="C#" MasterPageFile="~/M/main.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="MF.Admin.UI.M.Report.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server">
    <script language="javascript" type="text/javascript">
        function search() {
            if ($("#starttime").val().trim() == "" || $("#overtime").val().trim() == "") {
                alert("请选择要查询的时间范围");
                return;
            }
            startTime = new Date($("#starttime").val().replace("-", "/")).dateDiff("d");
            overTime = new Date($("#overtime").val().replace("-", "/")).dateDiff("d");
            if (overTime < startTime) {
                alert("查询截止时间不能小于开始时间");
                return;
            }
            $("#loading").show();
            var args = ["login",startTime, overTime];
            jsonPager.queryArgs = args;
            ajax.getReportList(jsonPager.makeArgs(1), searchResult);
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
            var date=new Date("2012/10/1")
            addCell(tr, date.dateAdd("d", o.Day).format("yyyy-MM-dd"), 0);
            addCell(tr, o.Today, 1);
            addCell(tr, o.Regist, 2);
            addCell(tr, o.Guest, 3);
            addCell(tr, o.WebSite, 4);
            addCell(tr, o.M_WebSite, 5);
            addCell(tr, o.Client, 6);
            addCell(tr, o.Web, 7);
            addCell(tr, o.iOS, 8);
            addCell(tr, o.Android, 9);
            addCell(tr, o.Wechat, 10);
            addCell(tr, o.Channel, 11);
            addCell(tr, o.ChannelGuest, 12);
            addCell(tr, o.Effective, 13);
            addCell(tr, o.GuestToUser, 14);
            addCell(tr, o.TodayGuestToUser, 15);
            addCell(tr, o.Relief, 16);
            addCell(tr, o.ChildNum, 17);
            addCell(tr, o.Total, 18);
            return tr;
        }
        $(document).ready(function() {
            attachCalenderbox('#starttime', '#overtime', null, new Date().dateAdd("d", -7).Format("yyyy-MM-dd"), new Date().Format("yyyy-MM-dd"));
            var pagerTitles = ["日期", "新增总数", "注册总数", "游客总数", "官网注册", "手机官网注册", "客户端", "Web端", "iOS", "Android", "微信注册", "渠道总注册", "渠道游客数", "有效注册", "游客转正总数", "当天游客转正","救济人数", "新增子账号", "总用户数"]
            jsonPager.init(ajax.getReportList, [], searchResult, pagerTitles, "list_table", "container", "pager", insertRow);
            jsonPager.dataBind(1,0);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
            <div class="toolbar">注册报表</div>
            <div class="search">&nbsp;&nbsp;
                开始日期:<input type="text" id="starttime" class="box w100" readonly="readonly" />
                截止日期:<input type="text" id="overtime" class="box w100" readonly="readonly" />
                <input type="button" value="查询" onclick="search()" class="ui-button-icon-primary" />
            </div>
            <div id="container"></div>
            <div id="pager" class="pager"></div>
            <div class="loading" id="loading"></div>
</asp:Content>
