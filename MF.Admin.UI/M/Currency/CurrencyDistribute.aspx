<%@ Page Title="游戏币管理》元宝分布" Language="C#" MasterPageFile="~/M/main.Master" AutoEventWireup="true" CodeBehind="CurrencyDistribute.aspx.cs" Inherits="MF.Admin.UI.M.Currency.CurrencyDistribute" %>
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
            var args = ["currencydistribute", startTime, overTime];
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
            addCell(tr, o.Zero, 1);
            addCell(tr, o.Thousand, 2);
            addCell(tr, o.FiveThousand, 3);
            addCell(tr, o.TwentyThousand, 4);
            addCell(tr, o.HundredThousand, 5);
            addCell(tr, o.Million, 6);
            addCell(tr, o.MillionPlus, 7);
            return tr; 
        }
        $(document).ready(function() {
            attachCalenderbox('#starttime', '#overtime', null, new Date().dateAdd("d", -7).Format("yyyy-MM-dd"), new Date().Format("yyyy-MM-dd"));
            var pagerTitles = ["日期", "0元宝/人数", "1～1000元宝/人数", "1001～5000元宝/人数", "5001～2万/人数", "2万～10万/人数", "10万～100万/人数", "100万以上/人数"];
            jsonPager.init(ajax.getReportList, [], searchResult, pagerTitles, "list_table", "container", "pager", insertRow);
            jsonPager.dataBind(1, 0);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
            <div class="toolbar">元宝分布</div>
            <div class="search">&nbsp;&nbsp;
                开始日期:<input type="text" id="starttime" class="box w100" readonly="readonly" />
                截止日期:<input type="text" id="overtime" class="box w100" readonly="readonly" />
                <input type="button" value="查询" onclick="search()" class="ui-button-icon-primary" />
            </div>
            <div id="container"></div>
            <div id="pager" class="pager"></div>
            <div class="loading" id="loading"></div>
</asp:Content>
