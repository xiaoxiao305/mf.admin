<%@ Page Title=" 游戏币管理 》 兑换奖品记录" MasterPageFile="~/M/main.Master" Language="C#" AutoEventWireup="true" CodeBehind="ExchangeRecord.aspx.cs" Inherits="MF.Admin.UI.M.Currency.ExchangeRecord" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server">
    <script language="javascript" type="text/javascript">
        function search() 
        {
            var args = ["<%=account%>",parseInt($("#type").val()),parseInt($("#flag").val())];
            jsonPager.queryArgs = args;
            $("#loading").show();
            ajax.getQmallRecord(jsonPager.makeArgs(1), searchResult);
        }
    function searchResult(data) {
        $("#loading").hide();
        if (data.code == 1) {
            jsonPager.data = data.result;
            jsonPager.dataBind(data.index, data.rowCount);
        }else
            alert(data.msg);
    }
        function insertRow(o, tr) {
            addCell = function(tr, text, i) {
                var td = tr.insertCell(i);
                td.innerHTML = text;
            };
            state = function (_state) { if (_state == 1) { return "成功"; } else return "失败"; };
            addCell(tr, o.Create_Date, 0);
            addCell(tr, o.Account, 1); 
            addCell(tr, o.Charge_Id, 2);
            addCell(tr, o.Product_Name, 3);
            addCell(tr, state(o.status), 4);
            addCell(tr, o.Order_Num, 5);
            addCell(tr, o.User_Input, 6);
            return tr;
        } 
        $(document).ready(function() {
            var pagerTitles = ["时间","账号","UID","商品名称","兑换状态","订单号","充值账号"];
            jsonPager.init(ajax.getQmallRecord, [], searchResult, pagerTitles, "list_table", "container", "pager", insertRow);
            jsonPager.dataBind(1, 0);
            $("#type").val(<%=type%>);
            if("<%=account%>" != "")
                search();
        }); 
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <div class="toolbar">查看用户兑换奖品记录</div>
    <div class="search">&nbsp;&nbsp;
        <select id="type">
	        <option value="-1">所有兑换</option>
            <option value="100">兑换元宝</option>
            <option value="101">兑换其他奖品</option>
        </select>
        <select id="flag">
	        <option value="-1">兑换状态</option>
	        <option value="1">成功</option>
	        <option value="0">失败</option>
        </select>
        <input type="button" value="查询" onclick="search()" class="ui-button-icon-primary" />
    </div>
    <div id="container"></div>
    <div id="pager" class="pager"></div>
    <div class="loading" id="loading"></div>
</asp:Content>
