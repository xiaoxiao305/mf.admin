<%@ Page Language="C#" MasterPageFile="~/M/main.Master" AutoEventWireup="true" CodeBehind="UserList.aspx.cs" Inherits="MF.Admin.UI.M.Users.UserList"  Title="用户管理 》 用户列表" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server">
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
                args = [dbsource,0, 1, parseInt($("#filed").val()), $("#key").val()];
            jsonPager.queryArgs = args;
            ajax.getUserList(jsonPager.makeArgs(1), searchResult);
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
                if (i==0)
                  td.style.textAlign = "left";
            };
            state = function (_state) { if (_state == 1) { return "正常"; } else { return "冻结"; } };
            guest = function (_guest) { if (_guest == 1) { return "游客"; } else if (_guest == 0) { return "注册用户"; } else if (_guest == 2) { return "游客转正"; } else { return _guest; } };
            deviceType = function (_device) { if (_device == 2) { return "PC"; } else if (_device == 3) { return "iPad"; } else if (_device == 5) { return "iOS"; } else if (_device == 6) { return "Android"; } else { return _device; } }
            if(db == 2)//实时
                addCell(tr, "&nbsp;账号:" + o.Account + "　<a href='/M/users/userinfo.aspx?db=2&acc=" + encodeURI(o.Account) + "' target='_blank'>实时</a><br/>&nbsp;昵称:" + o.Nickname + "<br/>&nbsp;UID:" + o.ChargeId, 0);
            else
                addCell(tr, "&nbsp;账号:" + o.Account + "　<a href='/M/users/userinfo.aspx?db=1&acc=" + encodeURI(o.Account) + "' target='_blank'>备份</a><br/>&nbsp;昵称:" + o.Nickname + "<br/>&nbsp;UID:" + o.ChargeId, 0);
            addCell(tr, state(o.Flag) + "<br/>" + guest(o.Guest), 1);
            addCell(tr, o.Currency, 2);
            addCell(tr, o.Bean, 3);
            addCell(tr, o.RoomCard, 4);
            addCell(tr, o.Silver, 5);
            addCell(tr, o.Name == null ? "" : (o.Name + "<br/>" + o.Identity), 6);
            addCell(tr, deviceType(o.RegistDevice), 7);
            addCell(tr, o.RegistIp, 8);
            addCell(tr, o.GUID, 9);
            return tr;
        }
        $(document).ready(function() {
            var pagerTitles = ["账号信息", "状态/属性", "元宝", "金豆", "房卡", "欢乐卡", "身份信息", "注册设备", "注册IP","GUID"]
            jsonPager.init(ajax.getUserList,[],searchResult,pagerTitles, "list_table", "container", "pager", insertRow);
            jsonPager.dataBind(1,0);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
            <div class="toolbar">用户列表</div>
            <div class="search">&nbsp;&nbsp;
                <strong class="search_db">备份数据库</strong>
               <select  id="dbname_s">
	                <option value="-1">全库查找</option>
	                <option value="100" selected="selected">USER_0</option>
	                <option value="101">USER_1</option>
	                <option value="121">USER_21</option>
                </select>
                <input id="exact_s" type="checkbox" name="exact" checked="checked" /><label for="exact">精确查找</label>
                <select id="filed_s">
                    <option value="1">UID</option>
	                <option value="2" selected="selected">账号</option>
	                <option value="3">昵称</option>
	                <option value="4">手机号码</option>
	                <option value="5">身份证</option>
	                <option value="6">GUID</option>
                </select>
                <input  type="text" id="key_s" class="box"/>
                <input type="button" value="查询" onclick="search(1)" class="ui-button-icon-primary" />
            </div>
            <div class="search">&nbsp;&nbsp;
                <strong class="search_db">及时数据</strong>
                &nbsp;&nbsp;<select id="filed">
                    <option value="1">UID</option>
	                <option value="2" selected="selected">账　号</option>
                    <option value="3">GUID</option>
                </select>&nbsp;&nbsp;
                <input  type="text" id="key" class="box"/>
                <input type="button" value="查询" onclick="search(2)" class="ui-button-icon-primary" />
            </div>
            <div id="container"></div>
            <div id="pager" class="pager"></div>
            <div class="loading" id="loading"></div>
</asp:Content>
