<%@ Page Title=" 报表管理 》 支付平台充值报表" Language="C#" MasterPageFile="~/M/main.Master" AutoEventWireup="true" CodeBehind="PayChannel.aspx.cs" Inherits="MF.Admin.UI.M.Report.PayChannel" %>
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
            var args = [parseInt($("#channel").val()),$("#key").val(),startTime, overTime];
            jsonPager.queryArgs = args;
            ajax.getPayChannelReport(jsonPager.makeArgs(1), searchResult);
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
            convertChannelToName=function(id) {
            switch (id) {
                case 0:
                    return "官网充值";
                case 10:
                    return "支付宝(网站)";
                case 11:
                    return "支付宝(App)";
                case 2:
                    return "易宝";
                case 3:
                    return "苹果";
                case 40:
                    return "微信(网站)";
                case 41:
                    return "微信(App)";
                default:
                    return id;
                }
            };
            var date=new Date("2012/10/1")
            addCell(tr, date.dateAdd("d", o.Day).format("yyyy-MM-dd"), 0);
            addCell(tr, convertChannelToName(o.ChannelId), 1);
            addCell(tr, o.SubmitNum, 2);
            addCell(tr, o.SubmitMoney, 3);
            addCell(tr, o.PayNum, 4);
            addCell(tr, o.PayMoney, 5);
           
            return tr;
        }
        function selectChannel(id) {
            if (id == "5")
                $("#channelid").show();
               else
                   $("#channelid").hide();
        }
        $(document).ready(function() {
            attachCalenderbox('#starttime', '#overtime', null, new Date().Format("yyyy-MM-dd"), new Date().Format("yyyy-MM-dd"));
            var pagerTitles = ["日期", "支付渠道", "提交笔数", "提交金额", "支付笔数", "支付金额"];
            jsonPager.init(ajax.getPayChannelReport, [], searchResult, pagerTitles, "list_table", "container", "pager", insertRow);
            jsonPager.dataBind(1, 0);
        });
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
            <div class="toolbar">支付渠道充值报表</div>
            <div class="search">&nbsp;&nbsp;
                <select id="channel" onchange="selectChannel(this.value);">
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
                <span id="channelid" style="display:none;">联运渠道ID:<input type="text" class="box" id="key" /> </span> 
                开始日期:<input type="text" id="starttime" class="box w100" readonly="readonly" />
                截止日期:<input type="text" id="overtime" class="box w100" readonly="readonly" />
                <input type="button" value="查询" onclick="search()" class="ui-button-icon-primary" />
            </div>
            <div id="container"></div>
            <div id="pager" class="pager"></div>
            <div class="loading" id="loading"></div>
</asp:Content>
