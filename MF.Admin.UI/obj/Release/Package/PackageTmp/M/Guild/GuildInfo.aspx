<%@ Page Title="俱乐部管理》查看俱乐部信息" MasterPageFile="~/M/main.Master" Language="C#" AutoEventWireup="true" CodeBehind="GuildInfo.aspx.cs" Inherits="MF.Admin.UI.M.Guild.GuildInfo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server">
    <link href="/common/styles/layer.css" type="text/css" rel="Stylesheet" />
    <script language="javascript" type="text/javascript">
        $(document).ready(function(){
            var aList= document.getElementsByTagName("a");
            for(var i=0;i<aList.length;i++)
            {
                if($.trim(aList[i].href).toLowerCase().indexOf("javascript:")<0 && $.trim(aList[i].href).toLowerCase() !="")
                    aList[i].target="_self";
            }
        });
        function showWin() {
            $("#currentweek").val($("#hidcurrentweek").val());
            $("#lastweek").val($("#hidlastweek").val());
            $("#exp").val($("#hidexp").val());
            $('.theme-popover-mask').show();
            $('.theme-popover').slideDown(200); 
            $('.theme-popover').css("width",380);
            $('.theme-popover').css("left", "55%");
            $('.theme-popover').css("top", "50%");
            $('.theme-poptit .close').click(function() { $('.theme-popover-mask').hide(); $('.theme-popover').slideUp(200);});
        }
        function setactivenum()
        {
            var current = $("#currentweek").val();
            var last = $("#lastweek").val();
            var exp = $("#exp").val();
            //var token = $("#token").val();
            if("<%=guild.ID%>" == "" || parseInt("<%=guild.ID%>") <1)
            {
                $("#lblerr").text("请选择需要修改的俱乐部");
                return;
            }
            if (current == "" || last == "" || exp == "") {
                $("#lblerr").text("请输入需要设置的活跃人数或经验值");
                return;
            } 
            else if (parseInt(current) != current || parseInt(last) != last || parseInt(exp) != exp) {
                $("#lblerr").text("请输入正确的活跃人数或经验值");
                return;
            }
            else if (parseInt(current) > 100000000 || parseInt(last) > 100000000 || parseInt(exp) > 100000000) {
                $("#lblerr").text("活跃人数数量或经验值有误");
                return;
            }
            //else if (token == "") {
            //    $("#lblerr").text("请输入安全令");
            //    return;
            //}
            //ajax.setGuildActive("setguildactive", [parseInt("<%=guild.ID%>"), parseFloat(current), parseFloat(last), parseFloat(exp), token], winresult);
            ajax.setGuildActive("setguildactive", [parseInt("<%=guild.ID%>"), parseFloat(current), parseFloat(last), parseFloat(exp)], winresult);
        }       
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <div class="toolbar">俱乐部详细信息</div>
    <table class="editbox">
        <tr><th>俱乐部名称</th><td><%=guild.Name%></td><th>会长账号</th><td><%=guild.Master%></td></tr>
        <tr><th>俱乐部人数</th><td><%=guild.UserCount %></td><th>经验</th><td><%=guild.Exp%></td></tr>
        <tr><th>本周活跃人数</th><td><%=guild.ActiveUserNumOfCurrentWeek %></td><th>上周活跃人数</th><td><%=guild.ActiveUserNumOfLastWeek %></td></tr>
        <tr><th>创建时间</th><td colspan="3"><%=guild.CreateTime%></td></tr>
        <tr><th></th><td colspan="3"><input type="button" value="修改" onclick="showWin()" class="ui-button-icon-primary" /></td></tr>
    </table>
    <input type="hidden" id="hidcurrentweek" value="<%=guild.ActiveUserNumOfCurrentWeek %>" />
    <input type="hidden" id="hidlastweek" value="<%=guild.ActiveUserNumOfLastWeek %>" />
    <input type="hidden" id="hidexp" value="<%=guild.Exp %>" />
      <!--弹出窗口开始-->
    <div class="theme-popover" style="height: 400px;">
        <div class="theme-poptit">
            <a href="javascript:;" title="关闭" class="close">×</a>
            <h3 id="msgtitle">设置俱乐部活跃人数</h3>
        </div>
        <div class="theme-popbod dform">
            <div id="content">
                <ul>
                    <li>本周活跃：<input class="ipt" type="text" id="currentweek" /></li>
                    <li>上周活跃：<input class="ipt" type="text" id="lastweek" /></li>
                    <li>　经验值：<input class="ipt" type="text" id="exp" /></li>
                 <%--   <li>安&nbsp;全&nbsp;令：<input class="ipt" type="text" id="token" /></li>--%>
                    <li class="err red" id="lblerr"></li>
                    <li><input class="btn btn-primary" type="button" value=" 确 定" onclick="setactivenum()" /></li>
                </ul>

            </div>
        </div>
    </div>
    <div class="theme-popover-mask"></div>
    <!--弹出窗口结束-->
</asp:Content>
