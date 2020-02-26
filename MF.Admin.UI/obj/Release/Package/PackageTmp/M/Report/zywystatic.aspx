<%@ Page Title=" 报表管理 》 交易平台报表" Language="C#" MasterPageFile="~/M/main.Master" AutoEventWireup="true" CodeBehind="zywystatic.aspx.cs" Inherits="MF.Admin.UI.M.Report.zywystatic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server">
    <script language="javascript" type="text/javascript">
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
            var args = ["zywystatic",startTime, overTime,$("#region").val()];
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
         var regions = ["", "乐山", "眉山"]
        function toThousands(num) {
            var result = [], counter = 0;
            num = (num || 0).toString().split('');
            for (var i = num.length - 1; i >= 0; i--) {
                counter++;
                result.unshift(num[i]);
                if (!(counter % 3) && i != 0) { result.unshift(','); }
            }
            return result.join('');
        }
        function insertRow(o,tr) {
        addCell = function (tr, text, i) {
            var td = tr.insertCell(i);
            td.innerHTML = text;
        }; 
        addCell(tr, regions[o.Region], 0);
        addCell(tr, o.Start.slice(0, 10), 1);
        //每天用户购买总量
        var BuyTotal = o.AllBalanceProduct + o.AllWechatProduct + o.AllCardPayProduct;
        var BuyTotalDetail = "";
        if (BuyTotal > 0) {
            BuyTotalDetail = "余额：" + (o.AllBalanceProduct > 0 ? toThousands(o.AllBalanceProduct * 10000) : "") + "<br/>"
                + "微信：" + (o.AllWechatProduct > 0 ? toThousands(o.AllWechatProduct*10000) : "") + "<br/>"
                + "银联：" + (o.AllCardPayProduct > 0 ? toThousands(o.AllCardPayProduct * 10000) : "") + "<br/>";
        }
        addCell(tr, toThousands(BuyTotal * 10000) + "<br/>" + BuyTotalDetail, 2);
        //每天用户购买次数
        var BuyCount = o.AllCardPayCount + o.AllWechatPayCount + o.AllBalancePayCount;
        var BuyCountDetail = "";
        if (BuyCount > 0) {
            BuyCountDetail = "余额：" + (o.AllBalancePayCount > 0 ? o.AllBalancePayCount : "") + "<br/>"
                + "微信：" + (o.AllWechatPayCount > 0 ? o.AllWechatPayCount : "") + "<br/>"
                + "银联：" + (o.AllCardPayCount > 0 ? o.AllCardPayCount : "") + "<br/>";
        }
        addCell(tr, BuyCount + "<br/>" + BuyCountDetail, 3); 
        //每天支付给支付平台的费用 
        var PlatFormFee = o.AllWechatPayFee + o.AllCardPayFee;
        var PlatFormFeeDetail = "";
        if (PlatFormFee > 0) {
            PlatFormFeeDetail = "微信：" + (o.AllWechatPayFee > 0 ? (o.AllWechatPayFee / 100).toFixed(2) : "") + "<br/>"
                + "银联：" + (o.AllCardPayFee > 0 ? (o.AllCardPayFee / 100).toFixed(2) : "") + "<br/>";
        }
        addCell(tr, (PlatFormFee / 100).toFixed(2) + "<br/>" + PlatFormFeeDetail, 4);
        var array = ["AllSaleProduct","NewTax", "AllSaleCount", "AllTransferCash", "AllTransferCount","", "VIPMoney", "VIPCount"];
        for (var i = 0; i < array.length; i++) {
            if (i == 0) {
                addCell(tr, toThousands(o[array[i]] * 10000), i + 5);
                continue;
            }
            else if (i == 1 || i == 3 || i==6) {
                addCell(tr, (o[array[i]] / 100).toFixed(2), i + 5);
                continue;
            } else if (i == 4) {
                addCell(tr, o[array[i]] + "<br/>"
                    + "成功：" + o["TransferSuccessCount"] + "<br/>"
                    + "失败扣手续费：" + o["TransferFailedCount"]+"<br/>"
                    + "失败不扣手续费：" + o["TransferErrorCount"] +"<br/>", i + 5);
                continue;
            }
            else if (i == 5) {
                addCell(tr, "成功：" + (o["TransferFeeSuccess"] / 100).toFixed(2) + "<br/>"
                    + "失败:" + (o["TransferFeeFailed"] / 100).toFixed(2), i + 5);
                continue;
            }
            addCell(tr, o[array[i]], i + 5);
        } 
        return tr;
        }
        $(document).ready(function() {
            attachCalenderbox('#starttime', '#overtime', null, new Date().dateAdd("d", -8).Format("yyyy-MM-dd"), new Date().dateAdd("d", -1).Format("yyyy-MM-dd"));
            var pagerTitles = ["区域", "时间", "购买总量(元宝)", "购买笔数（笔）", "支付平台费用(元)","出售总量(元宝)","出售抽水(元)", "出售笔数", "提现总额(元)", "提现笔数（笔）", "提现手续费(元)","VIP费用", "VIP人数"];
            jsonPager.init(ajax.getReportList, [], searchResult, pagerTitles, "list_table", "container", "pager", insertRow);
            jsonPager.dataBind(1, 0);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
            <div class="toolbar">交易平台报表</div>
            <div class="search">&nbsp;&nbsp;
                <input type="text" id="starttime" class="box w100" readonly="readonly" />
                <input type="text" id="overtime" class="box w100" readonly="readonly" />
                区域：<select id="region">
                        <option value=""></option>
                        <option value="1">乐山</option>
                        <option value="2">眉山</option>
                   </select>
                <input type="button" value="查询" onclick="search()" class="ui-button-icon-primary" />
            </div>
            <div id="container"></div>
            <div id="pager" class="pager"></div>
            <div class="loading" id="loading"></div>
</asp:Content>
