<%@ Page Title=" 报表管理 》 推广报表" Language="C#" MasterPageFile="~/M/main.Master" AutoEventWireup="true" CodeBehind="report.aspx.cs" Inherits="MF.CPS.UI.M.report.report" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server">
    <script language="javascript" type="text/javascript">
        var channel =  "<%=UserChannel %>";
        function search() {
            if ($("#starttime").val().trim() == "" || $("#overtime").val().trim() == "") {
                alert("请选择要查询的时间范围");
                return;
            }
            startTime = new Date($("#starttime").val().replace(/-/g, "/")).dateDiff("d");
            overTime = new Date($("#overtime").val().replace(/-/g, "/")).dateDiff("d");
            if (overTime < startTime) {
                alert("查询截止时间不能小于开始时间");
                return;
            }
            $("#loading").show();
            var args = [channel,startTime, overTime];
            jsonPager.queryArgs = args;
            ajax.getPromotChargeList(jsonPager.makeArgs(1), searchResult);
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
            addCell(tr, date.dateAdd("d", o.Day).format("yyyy-MM-dd"), 0);
            addCell(tr, o.LoginNum > 0 ? o.LoginNum:"", 1);
            addCell(tr, o.MaxNum > 0 ? o.MaxNum : "", 2);
            addCell(tr, o.ActUserNum > 0 ? o.ActUserNum:"", 3);
            addCell(tr, o.RegNum > 0 ? o.RegNum:"", 4);
            if (o.OneDayLeft > 0 && o.RegNum > 0)
                addCell(tr, Math.floor((o.OneDayLeft / o.RegNum) * 10000) / 100 + "%", 5);
            else
                addCell(tr, "", 5);
            if (o.ThreeDayLeft > 0 && o.RegNum > 0)
                addCell(tr, Math.floor((o.ThreeDayLeft / o.RegNum) * 10000) / 100 + "%", 6);
            else
                addCell(tr, "", 6);
            if (o.SevenDayLeft > 0 && o.RegNum > 0)
                addCell(tr, Math.floor((o.SevenDayLeft / o.RegNum) * 10000) / 100 + "%", 7);
            else
                addCell(tr, "", 7);
            addCell(tr, o.ARPU == 0 || o.ARPU == null ? "" : o.ARPU, 8);
            addCell(tr, o.ARPPU == 0 || o.ARPPU == null ? "" : o.ARPPU, 9);
            addCell(tr, o.PayRate == 0 || o.PayRate == null ? "" : o.PayRate + "%", 10);
            addCell(tr, o.ChargeUserRate == 0 || o.ChargeUserRate == null? "" : o.ChargeUserRate + "%", 11);
            addCell(tr, o.SubmitNum > 0 ? o.SubmitNum : "", 12);
            addCell(tr, o.SubmitMoney > 0 ? o.SubmitMoney : "", 13);
            addCell(tr, o.PayNum > 0 ? o.PayNum : "", 14);
            addCell(tr, o.PayMoney > 0 ? o.PayMoney : "", 15);
            return tr;
        }
        $(document).ready(function() {
            attachCalenderbox('#starttime', '#overtime', null, new Date().dateAdd("d", -7).Format("yyyy-MM-dd"), new Date().Format("yyyy-MM-dd"));
            var pagerTitles = ["日期", "登录数量", "在线峰值", "⽇活跃", "新增数量", "次日留存", "3日留存", "7日留存", "ARPU", "ARPPU", "付费率(登录)", "付费率(活跃)", "提交笔数", "提交总额", "支付笔数", "支付总额"];
            jsonPager.init(ajax.getPromotChargeList, [], searchResult, pagerTitles, "list_table", "container", "pager", insertRow);
            jsonPager.dataBind(1, 0);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
            <div class="toolbar">推广报表</div>
            <div class="search">&nbsp;&nbsp;
                开始日期:<input type="text" id="starttime" class="box w100" readonly="readonly" />
                截止日期:<input type="text" id="overtime" class="box w100" readonly="readonly" />
                <input type="button" value="查询" onclick="search()" class="ui-button-icon-primary" />
            </div>
            <div id="container"></div>
            <div id="pager" class="pager"></div>
            <div class="loading" id="loading"></div>
</asp:Content>
