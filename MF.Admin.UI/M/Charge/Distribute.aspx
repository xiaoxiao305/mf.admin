<%@ Page Title="充值管理》充值分布" Language="C#" MasterPageFile="~/M/main.Master" AutoEventWireup="true" CodeBehind="Distribute.aspx.cs" Inherits="MF.Admin.UI.M.Charge.Distribute" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server">
    <script language="javascript" type="text/javascript">
        var type=<%=type %>;
        function search() {
            var startTime = 0;
            var overTime = 0;
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
            var args = [type,startTime, overTime];
            jsonPager.queryArgs = args;
            $("#loading").show();
            ajax.getChargeDistribute(jsonPager.makeArgs(1), searchResult);
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
             addCell(tr, new Date("2012/10/1").dateAdd("d", o.Day).Format("yyyy-MM-dd"), 0);
            addCell(tr, o.First, 1);
            addCell(tr, o.Six, 2);
            addCell(tr, o.Twenty, 3);
            addCell(tr, o.Fifty , 4);
            addCell(tr, o.Hundred, 5);
            addCell(tr, o.HundredPlus, 6);
            return tr;
        }
        $(document).ready(function() {
            attachCalenderbox('#starttime', '#overtime', null, new Date().Format("yyyy-MM-dd"), new Date().Format("yyyy-MM-dd"));
            var pagerTitles = ["日期", "今日首充/人数","6元以下/人数", "7～20元/人数", "20～50元/人数", "50～100元/人数", "100元以上/人数"];
            jsonPager.init(ajax.getChargeDistribute, [], searchResult, pagerTitles, "list_table", "container", "pager", insertRow);
            jsonPager.dataBind(1, 0);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
            <div class="toolbar"><%=title %></div>
            <div class="search">&nbsp;&nbsp;
                开始:<input type="text" id="starttime" class="box w100" readonly="readonly" />
                截止:<input type="text" id="overtime" class="box w100" readonly="readonly" />
                <input type="button" value="查询" onclick="search()" class="ui-button-icon-primary" />
            </div>
            <div id="container"></div>
            <div id="pager" class="pager"></div>
            <div class="loading" id="loading"></div>
</asp:Content>
