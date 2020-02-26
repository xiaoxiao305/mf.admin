<%@ Page Title=" 用户管理 》 查看用户详细信息" Language="C#" MasterPageFile="~/M/main.Master" AutoEventWireup="true" CodeBehind="UserInfo.aspx.cs" Inherits="MF.KF.UI.M.Users.UserInfo" %>
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
            if (<%=isAdmin%>== 1)
                $("#currencyRecordTag").show();
        });
        var typearray=["","修改密码","解绑手机","解绑安全令","解除本机锁定","解除安全令锁定","冻结账号","解冻账号"];
        function showOpreateWin(t) {
            $('.theme-popover-mask').show();
            $('.theme-popover').slideDown(200); 
            $('.theme-popover').css("width",380);
            $('.theme-popover').css("left", "55%");
            $('.theme-popover').css("top", "50%");
            if(t == 1)//修改密码
                $("#content").html($("#T"+t).html()+$("#opreate").html());
            else
                $("#content").html($("#opreate").html());
            if(t == 6 && "<%=user.Flag%>" != 1)
                t = 7;
            $("#msgtitle").html(typearray[t]);
            $("#hidtype").val(t);
            $('.theme-poptit .close').click(function() { $('.theme-popover-mask').hide(); $('.theme-popover').slideUp(200);});
        }
        function setuserinfo()
        {
            var t =$("#hidtype").val();
            var pwd="";
            var token="";
            if(t == "")
            {
                $("#lblerr").text("请选择操作类型");
                return;
            }
            if(t == 1)//修改密码
            {
                pwd = $("#p1").val();
                if(pwd == "" || $("#p2").val() == "")
                {
                    $("#lblerr").text("请输入重置密码");
                    return;
                }
                else if(pwd != $("#p2").val())
                {
                    $("#lblerr").text("两次密码不一致");
                    return;
                }
            }
            else if(t == 2 && "<%=user.Mobile%>" == "")//解绑手机号
            {
                $("#lblerr").text("该用户尚未绑定手机号");
                return;
            }
            else if(t == 3 &&  "<%=user.PhoneKey%>" == "")//解绑安全令
            {
                $("#lblerr").text("该用户尚未绑定手机安全令");
                return;
            }else if(t == 4 && "<%=user.Lock%>" != 2)//解除本机锁定
            {
                $("#lblerr").text("该用户尚未锁定登录设备");
                return;
            }else if(t == 5 && "<%=user.Lock%>" != 1)//解除安全令锁定
            {
                $("#lblerr").text("该账号尚未被锁定");
                return;
            }
            token= $("#token").val();
            if(token =="")
            {
                $("#lblerr").text("请输入安全令");
                return;
            }
            if(confirm("确认要为该用户进行【"+typearray[t]+"】操作？"))
                ajax.setUserInfo("setuserinfo", ["<%=user.Account%>", parseFloat(t), token, pwd], winresult);
        }
       
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <div class="toolbar">用户信息</div>
    <table class="editbox">
        <tr><th colspan="4">基本资料</th></tr>
        <tr>
            <th>账号</th>
            <td><%=user.Account %></td>
            <th>注册区域</th>
            <td><%=user.RegistArea %></td>
        </tr>
        <tr>
            <th>昵称</th>
            <td><%=user.Nickname %></td>
            <th>救济状态</th>
            <td><%=user.Relief>0?ConvertToDate("d",user.Relief/100).ToString("yyyy-MM-dd"):"" %> 领取<%=user.Relief%100 %>次</td>
        </tr>
        <tr>
            <th>状态</th>
            <td><%=(user.Flag == 1) ? "<span style='color:green'>正常</span>" : "<span style='color:red'>冻结</span>"%></td>
            <th>Guid</th>
            <td><%=user.GUID %></td>
        </tr>
        <tr>
            <th>姓名</th>
            <td><%=user.Name %></td>
            <th>等级</th>
            <td><%=user.Lv %></td>
        </tr>
        <tr>
            <th>性别</th>
            <td><%=user.Sex==1?"男":"女" %></td>
            <th>经验值</th>
            <td><%=user.Exp %></td>
        </tr>
        <tr>
            <th>身份证</th>
            <td><%=user.Identity %></td>
            <th>最后一次登录IP</th>
            <td><%=user.LastIp %></td>
        </tr>
        <tr>
            <th>注册时间</th>
            <td><%=ConvertToDate("s",user.Regitime).ToString("yyyy-MM-dd HH:mm:ss") %></td>
            <th>最后一次登录时间</th>
            <td><%=ConvertToDate("s",user.LastLogin).ToString("yyyy-MM-dd HH:mm:ss") %></td>
        </tr>
        <tr>
            <th>注册IP</th>
            <td><%=user.RegistIp %></td>
            <th>上次登录设备</th>
            <td><%=ConvertDevice(user.LoginDevice) %></td>
        </tr>
        <tr>
            <th>手机</th>
            <td><%=user.Mobile%></td>
            <th>累计登录次数</th>
            <td><%=user.LoginCount %></td>
        </tr>
        <tr>
            <th>账号锁定</th>
            <td><%=ConvertLock(user.Lock) %></td>
            <th>客户端设备号</th>
            <td><%=user.DeviceCode %></td>
        </tr>
        <tr>
            <th>账号属性</th>
            <td><%=ConvertGuest(user.Guest) %></td>
            <th>手机安全令</th>
            <td><%=user.PhoneKey %></td>
        </tr>
        <tr>
            <th>UID</th>
            <td><%=user.ChargeId %></td>
            <th>推广渠道ID</th>
            <td><%=user.ADID %></td>
        </tr>
        <tr>
            <th>注册设备</th>
            <td><%=ConvertDevice(user.RegistDevice)%></td>
            <th>房卡</th>
            <td><%=user.RoomCard%></td>
        </tr>
        <tr>
            <th>元宝</th>
            <td><%=user.Currency%></td>
            <th>金豆</th>
            <td><%=user.Bean%></td>
        </tr>
    </table>
    <div id="uinfodetailopreate" runat="server" class="hide">
        <ul>
            <li>当前元宝数量　　　　<a href="/M/currency/CurrencyRecord.aspx?account=<%=user.Account %>" id="currencyRecordTag" style="display:none;">元宝变更详情</a>　　　<a href="/m/Charge/RecordList.aspx?account=<%=user.Account %>">充值记录</a></li>
        </ul>
        <div class="opreate-a">
            <a href="javascript:;" onclick="showOpreateWin(1)">修改密码</a><a href="javascript:;" onclick="showOpreateWin(2)">解绑手机</a><a href="javascript:;" onclick="showOpreateWin(3)">解绑安全令</a><a href="javascript:;" onclick="showOpreateWin(4)">解除本机锁定</a><a href="javascript:;" onclick="showOpreateWin(5)">解除安全令锁定</a>
        </div>
    </div>
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
            <li>设置密码：<input class="ipt" type="password" id="p1" /></li>
            <li>确认密码：<input class="ipt" type="password" id="p2" /></li>
        </ul>
        <ul id="opreate">
            <li>安&nbsp;全&nbsp;令：<input class="ipt" type="text" id="token" /></li>
            <li class="err red" id="lblerr"></li>
            <li>
                <input class="btn btn-primary" type="button" value=" 确 定" onclick="setuserinfo()" /></li>
        </ul>
        <input type="hidden" id="hidtype" />
    </div>
</asp:Content>
