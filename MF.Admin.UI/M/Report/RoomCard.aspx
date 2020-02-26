<%@ Page Title=" 报表管理 》 房卡报表" Language="C#" MasterPageFile="~/M/main.Master" AutoEventWireup="true" CodeBehind="RoomCard.aspx.cs" Inherits="MF.Admin.UI.M.Report.RoomCard" %>
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
            var args = ["roomcard",startTime, overTime];
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
            addCell(tr, o.Share, 3);
            addCell(tr, o.Buy, 4);
            addCell(tr, o.UseNum, 5);
            addCell(tr, o.Charge, 6);
            addCell(tr, o.BindPhone, 7);
            addCell(tr, o.QQShare, 8);
            addCell(tr, o.WechatShare, 9);
            addCell(tr, o.Total, 10);
            return tr;
        }
        $(document).ready(function() {
            attachCalenderbox('#starttime', '#overtime', null, new Date().dateAdd("d", -7).Format("yyyy-MM-dd"), new Date().Format("yyyy-MM-dd"));
            var pagerTitles = ["日期", "今日总产出", "注册赠送", "分享赠送", "购买数量", "使用房卡", "充值赠送", "绑定手机赠送", "QQ分享次数", "微信分享次数", "总房卡数"];
            jsonPager.init(ajax.getReportList, [], searchResult, pagerTitles, "list_table", "container", "pager", insertRow);
            jsonPager.dataBind(1, 0);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
            <div class="toolbar">房卡报表</div>
            <div class="search">&nbsp;&nbsp;
                开始日期:<input type="text" id="starttime" class="box w100" readonly="readonly" />
                截止日期:<input type="text" id="overtime" class="box w100" readonly="readonly" />
                <input type="button" value="查询" onclick="search()" class="ui-button-icon-primary" />
            </div>
            <div id="container"></div>
            <div id="pager" class="pager"></div>
            <div class="loading" id="loading"></div>
</asp:Content>
