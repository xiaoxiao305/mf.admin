<%@ Page  Title=" 游戏币管理 》 房卡记录"  MasterPageFile="~/M/main.Master" Language="C#" AutoEventWireup="true" CodeBehind="RoomCard.aspx.cs" Inherits="MF.Admin.UI.M.Currency.RoomCard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server">
    <link href="/common/styles/layer.css" type="text/css" rel="Stylesheet" />
    <script language="javascript" type="text/javascript">
        var dicType={1:"定(即)时赛报名",2:"复活",3:"VIP每日赠送",4:"金券兑换", 5:"定(即)时赛获胜", 6:"定(即)时赛退赛",7:"兑换战斗力", 8:"邮箱提取", 9:"商城兑换",10:"注册赠送",15:"战斗力回兑",
                        16:"领取救济",18:"存入保险箱",19:"保险箱取出",21:"开红包" ,22:"绑定手机",23:"完成俱乐部任务",24:"购买房卡",25:"系统赠送",26:"银票兑换",27:"管理员派发",28:"主/子账号互转"};
        function search() {
            var startTime = 0;
            var overTime = 0;
                if ($("#starttime").val().trim() == "" || $("#overtime").val().trim() == "") {
                    alert("请选择要查询的时间范围");
                    return;
                } 
                startTime = new Date($("#starttime").val().replace(/-/g, "/")).dateDiff("s");
                overTime = new Date($("#overtime").val().replace(/-/g, "/") + " 23:59:59").dateDiff("s");
                if (overTime < startTime) {
                    alert("查询截止时间不能小于开始时间");
                    return;
                }
            var args = ["<%=account%>", startTime, overTime];
            jsonPager.queryArgs = args;
            $("#loading").show();
            ajax.getRoomCardRecord(jsonPager.makeArgs(1), searchResult);
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
            getTypeName = function(_type) { 
                if (dicType[_type]) return dicType[_type];
                return _type; 
            };
            addCell(tr, new Date("2012/10/01").dateAdd("s",o.Time).Format("yyyy-MM-dd hh:mm:ss"), 0);
            addCell(tr, o.Account, 1); 
            addCell(tr, getTypeName(o.Type), 2);
            addCell(tr, o.Num, 3);
            addCell(tr, o.Original, 4);
            addCell(tr, o.IP.split(":")[0], 5);
            return tr;
        }
        $(document).ready(function() {
            attachCalenderbox('#starttime', '#overtime', null,new Date().Format("yyyy-MM-dd") , new Date().Format("yyyy-MM-dd"));
            var pagerTitles = ["时间","用户账号","变更类型","变更数量","原元宝数量","IP"];
            jsonPager.init(ajax.getRoomCardRecord, [], searchResult, pagerTitles, "list_table", "container", "pager", insertRow);
            jsonPager.dataBind(1, 0);
            if("<%=account%>" != "")
                search();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <div class="toolbar">查看用户房卡记录</div>
    <div class="search">&nbsp;&nbsp;
        开始:<input type="text" id="starttime" class="box w100" readonly="readonly" />
        截止:<input type="text" id="overtime" class="box w100" readonly="readonly" />
        <input type="button" value="查询" onclick="search()" class="ui-button-icon-primary" />
        <input type="button" value="增加用户房卡" onclick="showAddMoneyWin(1)" class="ui-button-icon-primary oprbutton" />
    </div>
    <div id="container"></div>
    <div id="pager" class="pager"></div>
    <div class="loading" id="loading"></div>
     <!--弹出窗口开始-->
    <div class="theme-popover" style="height: 400px;">
        <div class="theme-poptit">
            <a href="javascript:;" title="关闭" class="close">×</a>
            <h3 id="msgtitle">添加房卡</h3>
        </div>
        <div class="theme-popbod dform">
            <div id="content"></div>
        </div>
    </div>
    <div class="theme-popover-mask"></div>
    <!--弹出窗口结束-->
    <div class="hide">
         <ul id="T1">
            <li>房卡数量：<input class="ipt" type="text" id="num" /></li>
            <li>安&nbsp;全&nbsp;令：<input class="ipt" type="text" id="token" /></li>
            <li class="err red" id="lblerr"></li>
            <li><input class="btn btn-primary" type="button" value=" 确 定" onclick="addusermoney('<%=account %>',3)" /></li>
            <li class="err red">注：该房卡直接派发至用户账上。</li>
        </ul>
        <ul id="T2">
            <li class="red">请仔细核对如下信息，确认是否添加用户房卡</li>
            <li>&nbsp;&nbsp;&nbsp;&nbsp;账&nbsp;&nbsp;&nbsp;&nbsp;号：<%=account %></li>
            <li>房卡数量：<span id="confirmnum"></span></li>                
            <li><input class="btn btn-primary" type="button" value=" 确 定" onclick="confirmopr('<%=account %>',3)" /></li>
        </ul>
        <input type="hidden"  id="hidNum" />
        <input type="hidden"  id="hidToken" />
    </div>
</asp:Content>
