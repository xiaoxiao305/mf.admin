<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="top.aspx.cs" Inherits="MF.CPS.UI.M.top" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="/common/styles/style.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        table{border:0; border-collapse:collapse;padding:0px;width:100%;}
        .t{background:#3B6EA5;width:100%; }
        .line{background:url(/common/images/top_line.gif);}
    </style>
     <script language="javascript" type="text/javascript"  src="/common/js/jquery1.8.1.js"></script> 
    <script language="javascript" type="text/javascript"  src="/common/js/ajax.js"></script>
   <script type="text/javascript" language="javascript">
       function selectMenu(index, defaultUrl) {
           parent.menu.location = "menu.aspx?do=" + index;
           parent.main.location = defaultUrl;
           var list = document.getElementsByTagName("span");
           for (var i = 0; i < list.length; i++) {
               list[i].className = "menu_unselect";
           }
           list[index].className = "menu_select";
       }
    </script>
</head>
<body>
    <form id="form1" runat="server">
      <table class="t">
      <tr>
        <td height="84"><table>
          <tr>
            <td height="84" width="165"><img src="/common/images/logo.gif" width="160" height="84" alt="2255棋牌" /></td>
            <td><table width="100%" border="0" cellspacing="0" cellpadding="0">
              <tr>
                <td height="34" align="left" valign="bottom" style="padding-right:20px;color:#FFFFFF;"><%=loginInfo%><a href="#" style="color:#FFFFFF;text-decoration:underline; float:right" onclick="ajax.signOut();" >退出系统</a></td>
              </tr>
              <tr>
                <td height="50" align="left" valign="bottom">
			        <div class="topmenu">
			            <ul>
			                <li><span onclick="selectMenu(0,'/m/report/report.aspx')">报表管理</span></li>
			            </ul>
			        </div>
			    </td>
              </tr>
            </table></td>
          </tr>
        </table></td>
      </tr>
      <tr>
        <td height="16" class="line">&nbsp;</td>
      </tr>
    </table>
    </form>
</body>
</html>
