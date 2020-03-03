<%@ Page Title=" 用户管理 》 用户列表" Language="C#" MasterPageFile="~/M/main.Master"  AutoEventWireup="true" CodeBehind="usertest.aspx.cs" Inherits="MF.Admin.UI.M.Users.usertest" %>
 <asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server">
    <link href="/common/styles/layer.css" type="text/css" rel="Stylesheet" />
    <script language="javascript" type="text/javascript">
        var db = 1;
        function search(dbsource) {
            $("#loading").show();
            db = dbsource;
            var args = null;
            if (dbsource == 1)//备份数据库
            {
                var exact = 0;
                if ($("#exact_s").is(":checked")) exact = 1;
                args = [dbsource, parseInt($("#dbname_s").val()), exact, parseInt($("#filed_s").val()), $("#key_s").val()];
            }
            else if (dbsource == 2)//实时数据库
                args = [dbsource, 0, 1, parseInt($("#filed").val()), $("#key").val()];
            jsonPager.queryArgs = args;
            ajax.getUserList(jsonPager.makeArgs(1), searchResult);
        }
        function searchResult(data) {
            $("#loading").hide();
            if (data.code == 1) {
                jsonPager.data = data.result;
                jsonPager.dataBind(data.index, data.rowCount);
            } else {
                alert(data.msg);
            }
        }
        var types = ["", "管理员", "客服"];
        function insertRow(o, tr) {
            addCell = function (tr, text, i) {
                var td = tr.insertCell(i);
                td.innerHTML = text;
            };
            addCell(tr, o.Id, 0);
            addCell(tr, o.Account, 1);
            addCell(tr, types[o.Type], 2);
            addCell(tr, o.Remark, 3);
            addCell(tr, "<a href='javascript:;' onclick=\"updateUser(" + o.Id + ")\">修改</a>&nbsp;&nbsp;&nbsp;&nbsp;" +
                "<a href='javascript:;' onclick=\"delUser(" + o.Id + ")\">删除</a>", 4);
            return tr;
        }
        function adduser() {
            showAddMoneyWin(1);
            $("#msgtitle").text("添加用户");
        }
        function addUserConfirm() {
            $("#lblerr").text("请输入账号");

        }
        function updateUser() {
            showAddMoneyWin(2);
            $("#msgtitle").text("修改用户");
        }
        function updateUserConfirm() {}
        function delUser(id) {
            if (confirm("确认删除用户")) {

            }
        }
        $(document).ready(function () {
            var pagerTitles = ["ID", "账号", "类型", "备注", "操作"]
            jsonPager.init(null, [], null, pagerTitles, "list_table", "container", "pager", insertRow);
            jsonPager.data = [{
                "Id": 1, "Account": "zhangsan", "Pwd": "e10adc3949ba59abbe56e057f20f883e",
                "Type": 1, "Remark": "修改备注1111"
            }];
            jsonPager.dataBind(1, 1);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <div class="toolbar">用户列表</div>
    <div class="search">&nbsp;&nbsp; 
        <select  id="dbname_s">
	        <option value="-1">用户类型</option>
        </select>
        账号 <input  type="text" id="key_s" class="box"/>
        <input type="button" value="查询" onclick="search(1)" class="ui-button-icon-primary" />
        <input type="button" value="添加用户" onclick="adduser()" class="ui-button-icon-primary oprbutton"/>
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
            <li>　　账　号：<input  type="text" class="box"/></li>
            <li>　　密　码：<input  type="text" class="box"/></li>
            <li>　　类　型：<select class="box" style="height:25px"><option selected="selected">请选择类型</option>
                <option>管理员</option>
                <option>客服</option></select></li>
            <li>　　备　注：<input  type="text" class="box"/></li>
            <li class="err red center" id="lblerr"></li>
            <li class="center"><input class="btn btn-primary" type="button" value=" 确 定" onclick="addUserConfirm()" /></li>
        </ul>
        <ul id="T2">
            <li>　　账　号：<label class="box" style="border:none;display:inline-block;text-align: left;">zhangsan</label></li>
            <li>　　密　码：<input  type="text" class="box"/></li>
            <li>　　类　型：<select class="box" style="height:25px"><option selected="selected">请选择类型</option>
                <option>管理员</option>
                <option>客服</option></select></li>
            <li>　　备　注：<input  type="text" class="box"/></li>
            <li class="err red center" id="lblerr2"></li>
            <li class="center"><input class="btn btn-primary" type="button" value=" 确 定" onclick="updateUserConfirm()" /></li>
        </ul>
    </div> 
</asp:Content>
