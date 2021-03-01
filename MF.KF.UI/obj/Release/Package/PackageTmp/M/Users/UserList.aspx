<%@ Page Title=" 用户管理 》 玩家数据" Language="C#"  MasterPageFile="~/M/main.Master" AutoEventWireup="true" CodeBehind="UserList.aspx.cs" Inherits="MF.KF.UI.M.Users.UserList" %>
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
            device = function (_device) { if (_device == 0) { return "官网"; } else if (_device == 1) { return "web端"; } else if (_device == 2) { return "客户端"; } else if (_device == 3) { return "iPad"; } else if (_device == 4) { return "Android"; } else if (_device == 5) { return "iPhone"; } else if (_device == 6) { return "Pad"; } else if (_device == 7) { return "手机官网"; } else { return _device; } }
            if(db == 2)//实时
                addCell(tr, "&nbsp;账号:" + o.Account + "　<a href='/M/users/userinfo.aspx?db=2&acc=" + o.Account + "' target='_blank'>实时</a><br/>&nbsp;昵称:" + o.Nickname + "<br/>&nbsp;UID:" + o.ChargeId, 0);
            else
                addCell(tr, "&nbsp;账号:" + o.Account + "　<a href='/M/users/userinfo.aspx?db=1&acc=" + o.Account + "' target='_blank'>备份</a><br/>&nbsp;昵称:" + o.Nickname + "<br/>&nbsp;UID:" + o.ChargeId, 0);
            addCell(tr, state(o.Flag) + "<br/>" + guest(o.Guest), 1);
            addCell(tr, getCurrencyInfo(o.Account,o.Currency), 2);
            addCell(tr, o.Bean, 3);
            addCell(tr, o.RoomCard, 4);
            addCell(tr, o.Name==null?"":(o.Name + "<br/>" + o.Identity), 5);
            addCell(tr, device(o.RegistDevice), 6);
            addCell(tr, o.RegistIp, 7);
            return tr;
        }
        function getCurrencyInfo(acc, currency) {   
            try {
                var isContain = /^ttxhj\d+$/.test(acc);
                if (isContain) {//已匹配到ttxhj0---ttxhj120   @袁磊2021-01-22
                    var index = acc.indexOf("ttxhj");//index=0
                    var num = acc.substring(index+5);//num=1~999
                    if (parseInt(num) > 120)
                        return 2000;
                }
                return currency;
            } catch{
                return currency;
            }
        }
        $(document).ready(function() {
            var pagerTitles = ["账号信息", "状态/属性", "元宝", "金豆", "房卡", "身份信息", "注册设备", "注册IP"]
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
	        <option value="102">USER_2</option>
	        <option value="103">USER_3</option>
	        <option value="104">USER_4</option>
        </select>
        <input id="exact_s" type="checkbox" name="exact" checked="checked" /><label for="exact">精确查找</label>
        <select id="filed_s">
            <option value="1">UID</option>
	        <option value="2" selected="selected">账号</option>
	        <option value="3">昵称</option>
	        <option value="4">手机号码</option>
	        <option value="5">身份证</option>
        </select>
        <input  type="text" id="key_s" class="box"/>
        <input type="button" value="查询" onclick="search(1)" class="ui-button-icon-primary" />
    </div>
    <div class="search">&nbsp;&nbsp;
        <strong class="search_db">实时数据库</strong>
        &nbsp;&nbsp;<select id="filed">
            <option value="1">UID</option>
	        <option value="2" selected="selected">账　号</option>
        </select>&nbsp;&nbsp;
        <input  type="text" id="key" class="box"/>
        <input type="button" value="查询" onclick="search(2)" class="ui-button-icon-primary" />
    </div>
    <div id="container"></div>
    <div id="pager" class="pager"></div>
    <div class="loading" id="loading"></div>
</asp:Content>

