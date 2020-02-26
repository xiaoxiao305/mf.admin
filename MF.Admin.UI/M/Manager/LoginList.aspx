<%@ Page Title="" Language="C#" MasterPageFile="~/M/main.Master" AutoEventWireup="true" CodeBehind="LoginList.aspx.cs" Inherits="MF.Admin.UI.M.Manager.LoginList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server">
    <script language="javascript" type="text/javascript">
        function search() {
            var startTime = 0;
            var overTime = 0;
            if ($("#starttime").val().trim() != "" && $("#overtime").val().trim() != "") {
                startTime = new Date($("#starttime").val().replace("-", "/")).dateDiff("s");
                overTime = new Date($("#overtime").val().replace("-", "/") + " 23:59:59").dateDiff("s");
            }
            if (($("#starttime").val().trim() != "" && $("#overtime").val().trim() == "") || ($("#starttime").val().trim()== "" && $("#overtime").val().trim() != "")) {
                alert("请选择要查询的时间范围");
                return;
            }
            var exact = 0;
            if ($("#exact").is(":checked")) exact = 1;
            var args = ["login",exact, parseInt($("#filed").val()),$("#key").val(), startTime, overTime];
            jsonPager.queryArgs = args;
            $("#loading").show();
            ajax.getLogList(jsonPager.makeArgs(1), searchResult);
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
            var date = new Date("2012/10/1")
            addCell(tr, o.Account, 0);
            addCell(tr, date.dateAdd("s", o.LoginTime).format("yyyy-MM-dd hh:mm:ss"), 1);
            addCell(tr, o.IP, 2);
            addCell(tr, (o.Flag==1)?"成功":"失败", 3);
            addCell(tr, o.Message, 4);
            return tr;
        }
        $(document).ready(function() {
            attachCalenderbox('#starttime', '#overtime', null, new Date().dateAdd("d", -7).Format("yyyy-MM-dd"), new Date().Format("yyyy-MM-dd"));
            var pagerTitles = ["账号", "登录时间", "登录IP", "登录状态", "说明"];
            jsonPager.init(ajax.getLogList, [], searchResult, pagerTitles, "list_table", "container", "pager", insertRow);
            jsonPager.dataBind(1, 0);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
            <div class="toolbar">查看管理员登录日志</div>
            <div class="search">&nbsp;&nbsp;
                开始日期:<input type="text" id="starttime" class="box w100" readonly="readonly" />
                截止日期:<input type="text" id="overtime" class="box w100" readonly="readonly" />
                <input id="exact" type="checkbox" name="exact" checked="checked" /><label for="exact">精确查找</label>
                <select id="filed">
	                <option value="1">管理员账号</option>
	                <option value="2">登录IP</option>
                </select>
                <input  type="text" id="key" class="box" />
                <input type="button" value="查询" onclick="search()" class="ui-button-icon-primary" />
            </div>
            <div id="container"></div>
            <div id="pager" class="pager"></div>
            <div class="loading" id="loading"></div>
</asp:Content>
