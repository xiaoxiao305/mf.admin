<%@ Page Title=" 充值管理 》 充值记录" Language="C#" MasterPageFile="~/M/main.Master" AutoEventWireup="true" CodeBehind="RecordList.aspx.cs" Inherits="MF.KF.UI.M.Charge.RecordList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server">
   <style type="text/css">
       li {display:block; text-align:left;padding-left:10%;}
       .center{text-align:center;}
   </style>
    <link href="/common/styles/layer.css" type="text/css" rel="Stylesheet" />
    <script language="javascript" type="text/javascript">
        function search() {
            var exact = $("#exact").is(":checked")?1:0;
            var checktime = $("#time").is(":checked") ? 1 : 0;
            var timeType = $("#radioSubmit").is(":checked") ? 1 :$("#radioPay").is(":checked")?2: 0; 
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
            var args = ["<%=account%>", parseInt($("#flag").val()), parseInt($("#channel").val()), exact, parseInt($("#filed").val()), $("#key").val(), checktime, timeType, startTime, overTime];
            jsonPager.queryArgs = args;
            $("#loading").show();
            ajax.getChargeRecord(jsonPager.makeArgs(1), searchResult);
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
            state = function(_state) { if (_state == 0) { return "等待支付"; } else if (_state == 1) { return "支付成功"; } else if (_state == 2) { return "支付失败"; } else { return _state; } };
            payname = function(id) {
                switch (id) {
                    case 0:
                        return "官网充值";
                    case 10:
                        return "支付宝(官网)";
                    case 11:
                        return "支付宝(App)";
                    case 2:
                        return "易宝";
                    case 3:
                        return "苹果";
                    case 40:
                        return "微信(官网)";
                    case 41:
                        return "微信(App)";
                    default:
                        return "联运渠道";
                }
            };
            addCell(tr, o.OrderNo, 0);
            addCell(tr, o.Account, 1);
            addCell(tr, new Date("2012/10/1").dateAdd("s", o.CreateDate).Format("yyyy-MM-dd hh:mm:ss"), 2);
            addCell(tr, o.SumbitMoney, 3);
            addCell(tr, state(o.Flag), 4);
            addCell(tr, (o.PayDate > 0) ? new Date("2012/10/1").dateAdd("s", o.PayDate).Format("yyyy-MM-dd hh:mm:ss") : "", 5);
            addCell(tr, o.PayMoney, 6);
            addCell(tr, o.PlatformTransId==null?"":"<div style='width:220px;white-space:nowrap;text-overflow:ellipsis;overflow:hidden;' title='"+o.PlatformTransId+"'>"+o.PlatformTransId+"</div>", 7);
            addCell(tr, payname(o.PayChannel), 8);
            addCell(tr, (o.Channel > 0) ? o.Channel : "", 9);
            return tr;
        }
        $(document).ready(function() {
            attachCalenderbox('#starttime', '#overtime', null, new Date().Format("yyyy-MM-dd"), new Date().Format("yyyy-MM-dd"));
            showTimeBox($$("time"));
            var pagerTitles = ["订单号","用户账号","提交时间","提交金额","订单状态","支付时间","支付金额","平台交易号","支付平台","联运渠道ID"];
            jsonPager.init(ajax.getChargeRecord, [], searchResult, pagerTitles, "list_table", "container", "pager", insertRow);
            jsonPager.dataBind(1, 0);
            if("<%=account%>" != "")
                search();
        });
        function showChargeOrderWin(order, acc, m, time, paychannel) {
            $("#tacc").text(acc);
            $("#torder").text(order);
            $("#tmoney").text(m);
            $("#ttime").text(new Date("2012/10/1").dateAdd("s", time).Format("yyyy-MM-dd hh:mm:ss"));
            //$("#msgtitle").text("补单");
            $("#hidpaychannel").val(paychannel);
            showAddMoneyWin(1);
        }
        function dealChargeOrder()
        {
            var account = $("#tacc").text();
            var order = $("#torder").text();
            var money = $("#tmoney").text();
            var paychannel = $("#hidpaychannel").val();
            var token = $("#token").val();
            if (order == "") {
                $("#lblerr").text("订单号有误");
                return;
            }
            else if (money == "") {
                $("#lblerr").text("金额有误");
                return;
            }
            else if (paychannel == "") {
                $("#lblerr").text("支付平台有误");
                return;
            }
            else if (token == "") {
                $("#lblerr").text("请输入安全令");
                return;
            }
            ajax.dealChargeOrder("dealchargeorder", [account,order,parseFloat(money),parseFloat(paychannel),token], winresult);
        } 
   
        function chkCloseChargeActive()
        {
            var status = $("#chkClose").is(":checked");
            if (status) {
                $("#stime").attr("disabled", true);
                $("#etime").attr("disabled", true);
                $("#stime").css("background-color", "#d6d6d6");
                $("#etime").css("background-color", "#d6d6d6");
            }
            else {
                $("#stime").attr("disabled",false);
                $("#etime").attr("disabled", false);
                $("#stime").css("background-color", "#ffffff");
                $("#etime").css("background-color", "#ffffff");
            }
        }
        function setChargeActive(account, type) {
            var status = $("#chkClose").is(":checked")?2:1;
            var stime = $("#stime").val();
            var etime = $("#etime").val();
            var token = $("#token2").val();
            if (stime == "" || etime == "") {
                $("#lblerr2").text("请选择活动开始和结束时间");
                return;
            }
            else if (token == "") {
                $("#lblerr2").text("请输入安全令");
                return;
            }
            ajax.setChargeActiveState("setchargeactivestate", [status, stime, etime, token], winresult);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <div class="toolbar">充值记录</div>
    <div class="search">&nbsp;&nbsp;
        <select  id="flag">
	        <option value="-1">订单状态</option>
	        <option value="0">等待支付</option>
	        <option value="1">支付成功</option>
	        <option value="2">支付失败</option>
        </select>
        <select id="channel">
	        <option value="-1">充值渠道</option>
	        <option value="1">支付宝</option>
	        <option value="10">支付宝(官网)</option>
	        <option value="11">支付宝(App)</option>
	        <option value="2">易宝</option>
	        <option value="3">iOS</option>
	        <option value="4">微信</option>
	        <option value="40">微信(官网)</option>
	        <option value="41">微信(App)</option>
	        <option value="5">联运渠道</option>
        </select>
        <input id="exact" type="checkbox" checked="checked" /><label for="exact">精确查找</label>
        <select id="filed">
	        <option value="1">订单号</option>
	        <option value="2">平台交易号</option>
        </select>
        <input  type="text" id="key" class="box" />
        <input id="time" type="checkbox"  onclick="showTimeBox(this)" /><label for="time">时间</label>
        <input type="button" value="查询" onclick="search()" class="ui-button-icon-primary" />
        <div id="divTime" class="date" >
            <input id="radioSubmit" type="radio" value="1" name="timegroup" checked="checked"/><label for="radioSubmit">提交时间</label>
            <input id="radioPay" type="radio" value="2" name="timegroup" /><label for="radioPay">支付时间</label>&nbsp;&nbsp;&nbsp;&nbsp;
            开始:<input type="text" id="starttime" class="box w100" readonly="readonly" />
            截止:<input type="text" id="overtime" class="box w100" readonly="readonly" />
        </div>
    </div>
    <div id="container"></div>
    <div id="pager" class="pager"></div>
    <div class="loading" id="loading"></div>
     <!--弹出窗口开始-->
    <div class="theme-popover" style="height: 400px;">
        <div class="theme-poptit">
            <a href="javascript:;" title="关闭" class="close">×</a>
            <h3 id="msgtitle"></h3>
        </div>
        <div class="theme-popbod dform">
            <div id="content"></div>
        </div>
    </div>
    <div class="theme-popover-mask"></div>
    <!--弹出窗口结束-->
    <div class="hide"> 
        <ul id="T1">
            <li>　　账　号：<label id="tacc"></label></li>
            <li>订　单　号：<label id="torder"></label></li>
            <li>　　金　额：<label id="tmoney"></label></li>
            <li>　　时　间：<label id="ttime"></label></li>
            <li>安　全　令：<input class="ipt" type="text" id="token" /></li>
            <li class="err red center" id="lblerr"></li>
            <li class="center"><input class="btn btn-primary" type="button" value=" 确 定" onclick="dealChargeOrder()" /></li>
        </ul>
        <ul id="T2">
            <li id="stateTitle" class="red" style="font-weight:bold;"></li>
            <li>开始时间：<input type="text" id="stime" class="ipt"  readonly="readonly"/>　0:00:00</li>
            <li>结束时间：<input type="text" id="etime" class="ipt" readonly="readonly"/>　23:59:59</li>
            <li>&nbsp;安&nbsp;全&nbsp;令：<input class="ipt" type="text" id="token2" /></li>
            <li>　　　　&nbsp;<input id="chkClose" type="checkbox" onclick="chkCloseChargeActive()" />关闭活动</li>
            <li class="err red center" id="lblerr2"></li>
            <li class="center"><input class="btn btn-primary" type="button" value=" 确 定" onclick="setChargeActive()" /></li>
        </ul>
        <input type="hidden" id="hidpaychannel" />
    </div> 
</asp:Content>
