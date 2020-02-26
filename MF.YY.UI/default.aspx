<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="MF.YY.UI._default" %>
<%@ Register Assembly="MF.WebControls" Namespace="MF.WebControls" TagPrefix="mf" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>2255棋牌运营后台管理系统</title>
    <script language="javascript" src="/common/js/md5.js" type="text/javascript"></script>
    <link href="/common/styles/style.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
    img{margin-top:20px; border:1px red solid;}
    body{background-color: #3B6EA5; margin-top: 15%; _margin-top: 150px; color: #FFFFFF;overflow:hidden; font-family:微软雅黑; }
    .logo{ background:url(/common/images/window.jpg) no-repeat;}
    </style>
    <script language="javascript" type ="text/javascript">
        $ = function(id) { return document.getElementById(id); }
        function login() {
            if ($("txtAccount").value == "") {
                alert("请输入要登录的账号");
                return false;
            }
            if ($("txtPwd").value == "") {
                alert("请输入登录密码");
                return false;
            }
            if ($("txtCode").value == "") {
                alert("请输入验证码");
                return false;
            }
            if ($("txtDyPwd").value == "") {
                alert("请输入安全令");
                return false;
            }
            $("txtPwd").value = md5($("txtPwd").value);
            return true;
        }
        function checkCapsLock() {
            if (window.event.keyCode > 64 && window.event.keyCode < 91)
                $("txtPwd").title = "保持大写锁定打开可能会使您错误的输入密码";
            else
                $("txtPwd").title = "";
        }
        if (parent.frames.length > 0) parent.location.href = "/";
        document.onkeydown = function(event) {
            var e = event || window.event || arguments.callee.caller.arguments[0];
            if (e && e.keyCode == 13) {
                $("btnLogin").click();
                return false;
            }
        }
    </script>  
</head>
<body>
    <form id="form1" runat="server">
        <table width="600" border="0" align="center" cellpadding="0" cellspacing="0">
        <tr>
            <td width="300" height="300" class="logo">&nbsp;</td>
            <td width="300" align="center">
                <table width="250" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td width="60" height="35" align="left">用户名：</td>
                        <td width="190" height="35" align="left"><asp:TextBox ID="txtAccount" runat="server" CssClass="Box" Width="153"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td height="35" align="left">密 码：</td>
                        <td height="35" align="left"><asp:TextBox ID="txtPwd" runat="server" CssClass="Box" Width="153" TextMode="Password"></asp:TextBox></td>
                    </tr>
                    <tr class="verContainer">
                        <td height="35" align="left">验证码：</td>
                        <td height="35" align="left">
                            <asp:TextBox ID="txtCode" runat="server" CssClass="Box" Width="80"  >
                            </asp:TextBox> <mf:VerificationCode ID="VerificationCode1" runat="server" TextCase="Upper" Width="65" Height="26" CssClass="VerificationCode"/>
                        </td>
                    </tr>
                    <tr>
                        <td height="35" align="left"> 安全令：</td>
                        <td height="35" align="left"><asp:TextBox ID="txtDyPwd" runat="server" CssClass="Box" Width="153"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td height="35" align="left"></td>
                        <td align="left">
                            <asp:Button ID="btnLogin" runat="server" class="submit" OnClientClick="return login();" OnClick="btnLogin_Click"/>
                            <input type="reset" value="" class="reset" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
