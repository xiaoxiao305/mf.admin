<%@ Page Title="俱乐部管理》俱乐部保证金记录" Language="C#" MasterPageFile="~/M/main.Master" AutoEventWireup="true" CodeBehind="ApplyRecord.aspx.cs" Inherits="MF.Admin.UI.M.Guild.ApplyRecord" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server">
    <script language="javascript" type="text/javascript">
        function search() {
            var exact = $("#exact").is(":checked")?1:0;
            var checktime = $("#time").is(":checked")?1:0;
            var timeType = $("#radioPay").is(":checked")?1:0;
            var startTime = 0;
            var overTime = 0;
            if (checktime==1) {
                if ($("#starttime").val().trim() == "" || $("#overtime").val().trim() == "") {
                    alert("请选择要查询的时间范围");
                    return;
                }
                startTime = new Date($("#starttime").val().replace(/-/g, "/")).dateDiff("s");
                overTime = new Date($("#overtime").val().replace(/-/g, "/") + " 23:59:59").dateDiff("s");
                if (overTime < startTime) {
                    alert("查询截止时间不能小于开始时间");
                    return;
                }
            }
            var args = [parseInt($("#flag").val()), exact, parseInt($("#filed").val()), $("#key").val(),checktime, timeType, startTime, overTime];
            jsonPager.queryArgs = args;
            $("#loading").show();
            ajax.getGuildApplyRecord(jsonPager.makeArgs(1), searchResult);
        }
        function searchResult(data) {
            $("#loading").hide();
            if (data.code == 1) {
                jsonPager.data = data.result;
                jsonPager.dataBind(data.index,data.rowCount);
            } else {
                alert(data.msg);
            }
        }
        function insertRow(o,tr) {
            addCell = function(tr, text, i) {
                var td = tr.insertCell(i);
                td.innerHTML = text;
            };
            state = function(_state) { if (_state == 0) { return "等待支付"; } else if (_state == 1) { return "支付成功"; } else if (_state == 2) { return "已创建俱乐部"; } else if (_state == 3) { return "申请退款中"; } else if (_state == 4) { return "已退款"; } else { return _state; } };
            addCell(tr, o.OrderNo, 0);
            addCell(tr, o.Account, 1);
            addCell(tr, state(o.Flag), 2);
            addCell(tr, o.GuildName == null ? "" : o.GuildName, 3);
            addCell(tr, new Date("2012/10/1").dateAdd("s", o.CreateDate).Format("yyyy-MM-dd hh:mm:ss"), 4);
            addCell(tr, o.PayDate>0?new Date("2012/10/1").dateAdd("s", o.PayDate).Format("yyyy-MM-dd hh:mm:ss"):"", 5);
            addCell(tr, o.TransId == null ? "" : o.TransId, 6);
            addCell(tr, o.ApplyRefundTime>0?new Date("2012/10/1").dateAdd("s", o.ApplyRefundTime).Format("yyyy-MM-dd hh:mm:ss"):"", 7);
            addCell(tr, o.AlipayAccount == null ? "" : o.AlipayAccount, 8);
            addCell(tr, o.AlipayName == null ? "" : o.AlipayName, 9);
            addCell(tr, o.Memo == null ? "" : o.Memo, 10);
            addCell(tr, (o.Flag == 3) ? "<a href='ApplyInfo.aspx?id=" + o.ID + "'>退款</a>" : "", 11);
            return tr;
        } 
        $(document).ready(function() {
            attachCalenderbox('#starttime', '#overtime', null, new Date().Format("yyyy-MM-dd"), new Date().Format("yyyy-MM-dd"));
            showTimeBox($$("time"));
            var pagerTitles = ["订单号", "账号", "状态", "俱乐部名称","提交时间", "付款时间", "支付宝交易号", "申请退款时间", "退款支付宝账号", "支付宝姓名","备注","退款"];
            jsonPager.init(ajax.getGuildApplyRecord, [], searchResult, pagerTitles, "list_table", "container", "pager", insertRow);
            jsonPager.dataBind(1, 0);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
            <div class="toolbar">俱乐部保证金记录列表</div>
            <div class="search">&nbsp;&nbsp;
                <select id="flag">
	                <option value="-1">订单状态</option>
	                <option value="0">等待支付</option>
	                <option value="1">支付成功</option>
	                <option value="2">已创建俱乐部</option>
	                <option value="3">申请退款</option>
	                <option value="4">已退款</option>
                </select>
                <input id="exact" type="checkbox" name="exact" checked="checked" /><label for="exact">精确查找</label>
                <select id="filed">
	                <option value="1">订单号</option>
	                <option value="2">用户账号</option>
	                <option value="3">俱乐部名称</option>
	                <option value="4">支付宝交易号</option>
	                <option value="5">支付宝账号</option>
                </select>
                <input  type="text" id="key" class="box" />
                <input id="time" type="checkbox"  onclick="showTimeBox(this)" /><label for="time">时间</label>
                <input type="button" value="查询" onclick="search()" class="ui-button-icon-primary" />
                <div id="divTime" class="date" >
                    <input id="radioCreate" type="radio" value="1" checked="checked" name="timegroup" /><label for="radioPay">创建时间</label>
                    <input id="radioPay" type="radio" value="2" name="timegroup" /><label for="radioPay">付款时间</label>
                    <input id="radioRefund" type="radio" name="timegroup" value="3" /><label for="radioRefund">申请退款时间</label>&nbsp;&nbsp;&nbsp;&nbsp;
                    开始:<input type="text" id="starttime" class="box w100" readonly="readonly" />
                    截止:<input type="text" id="overtime" class="box w100" readonly="readonly" />
                </div>
            </div>
            <div id="container"></div>
            <div id="pager" class="pager"></div>
            <div class="loading" id="loading"></div>
</asp:Content>