<%@ Page   Title=" 合作商管理 》 合作商账号信息" MasterPageFile="~/M/main.Master" Language="C#" AutoEventWireup="true" CodeBehind="userslist.aspx.cs" Inherits="MF.YY.UI.M.users.userslist" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server">
    <script language="javascript" type="text/javascript">
        function search() {
            $("#loading").show();
            var exact = 0;
            if ($("#exact").is(":checked")) exact = 1;
            var args = [exact,parseInt($("#filed").val()), $("#key").val()];
            jsonPager.queryArgs = args;
            ajax.getCPSUserList(jsonPager.makeArgs(1), searchResult);
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
            userState = function (state, admin_id) {
                if (admin_id < 1) return "";
                if (state == 1) return "正常";else if (state == 0) return "冻结";else return "";
            };
            addCell(tr, o.channel, 0);
            addCell(tr, o.channel_name, 1);
            addCell(tr, o.channel_num, 2);
            addCell(tr, o.device, 3);
            addCell(tr, o.business_link, 4);
            addCell(tr, o.telephone, 5);
            addCell(tr, o.email, 6);
            addCell(tr, o.qq, 7);
            addCell(tr, o.percent == 0 || o.percent == null ? "" : o.percent + "%", 8);
            addCell(tr, o.admin_account == null ? "" : o.admin_account, 9);
            addCell(tr, userState(o.admin_flag,o.admin_id), 10);
            addCell(tr, "<a href='/m/users/addusers.aspx?id=" + o.id + "'>编辑</a>", 11);
            return tr;
        }
        $(document).ready(function() {
            attachCalenderbox('#starttime', '#overtime', null, new Date().dateAdd("d", -7).Format("yyyy-MM-dd"), new Date().Format("yyyy-MM-dd"));
            var pagerTitles = ["渠道码", "渠道名称", "渠道号", "硬件平台", "联系人", "电话", "邮箱", "QQ", "分红比例", "后台登录账号", "状态", "操作"];
            jsonPager.init(ajax.getCPSUserList, [], searchResult, pagerTitles, "list_table", "container", "pager", insertRow);
            jsonPager.dataBind(1, 0);
            var t = "<%=type%>";
            if (t == 1)
                search();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
            <div class="toolbar">合作商账号列表</div>
            <div class="search">&nbsp;&nbsp; 
                <input id="exact" type="checkbox" name="exact" checked="checked" /><label for="exact">精确查找</label>
                <select id="filed">
                    <option value="1">渠道码</option>
	                <option value="2" selected="selected">渠道名称</option>
                </select>
                <input  type="text" id="key" class="box"/>
                <input type="button" value="查询" onclick="search()" class="ui-button-icon-primary" />
            </div>
            <div id="container"></div>
            <div id="pager" class="pager"></div>
            <div class="loading" id="loading"></div>
</asp:Content>
