<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MF.KF.UI.M.Default" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
    <head id="Head1" runat="server">
        <title>2255棋牌客服客服后台管理系统</title>
    </head>
    <FRAMESET border=0 frameSpacing=0 rows=100,* frameBorder=no cols=*>
		<FRAME id="top" name="top" src="top.aspx" noResize scrolling=no>
		<FRAMESET border=0 frameSpacing=0 rows=* frameBorder=no cols=165,*>
			<FRAME id="menu" name="menu" src="menu.aspx?do=0" noResize scrolling=no>
			<FRAME id="main" name="main" src="/m/users/userlist.aspx" noResize scrolling=yes>
		</FRAMESET>
	</FRAMESET>
</html>
