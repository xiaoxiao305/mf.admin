<%@ Page  Title=" 合作商管理 》 添加合作商账号" MasterPageFile="~/M/main.Master" Language="C#" AutoEventWireup="true" CodeBehind="addusers.aspx.cs" Inherits="MF.YY.UI.M.users.addusers" %>
 <asp:Content ID="Content1" ContentPlaceHolderID="h" runat="server">
     <style>
         .editbox input[type=text] {padding:5px 0 0 5px;
	color:#3B6EA5;
	height:18px;
	width:300px;
	border:1px solid #BBDDE5;
	background-color:#FFFFFF;
         }
         .btnAdd{
         width:80px;
         height:26px;
         letter-spacing:10px;
         }
     </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="p" runat="server">
    <div class="toolbar">合作商信息</div>
    <table class="editbox">
        <tr><th colspan="4">合作商基本资料</th></tr>
        <tr>
            <th>渠道码</th>
            <td><input type="text" id="txtchannel" runat="server" /></td>
            <th>渠道名称</th>
            <td><input type="text" id="txtchannel_name" runat="server" /></td>
        </tr> 
        <tr>
            <th>合作协议</th>
            <td><input type="text" id="txtprotocol" runat="server" /></td>
            <th>合作商证件</th>
            <td><input type="text" id="txtidnum" runat="server" /></td>
        </tr> 
        <tr>
            <th>开户行名称</th>
            <td><input type="text" id="txtbank_name" runat="server" /></td>
            <th>开户行地址</th>
            <td><input type="text" id="txtbank_addr" runat="server" /></td>
        </tr>
        <tr>
            <th>开户行账号</th>
            <td><input type="text" id="txtbank_acc" runat="server" /></td>
            <th>硬件平台</th>
            <td><input type="text" id="txtdevice" runat="server" /></td>
        </tr>
        <tr>
            <th>业务联系人</th>
            <td><input type="text" id="txtbusiness_link" runat="server" /></td>
            <th>联系人电话</th>
            <td><input type="text" id="txttelephone" runat="server" /></td>
        </tr>
        <tr>
            <th>联系人邮箱</th>
            <td><input type="text" id="txtemail" runat="server" /></td>
            <th>联系人QQ</th>
            <td><input type="text" id="txtqq" runat="server" /></td>
        </tr>
        <tr>
            <th>分红比例</th>
            <td><input type="text" id="txtpercent" runat="server" style="width:50px;" />%</td>
            <th>渠道号</th>
            <td><input type="text" id="txtchannel_num" runat="server" /></td>
        </tr>
        <tr><th colspan="4">合作商后台登录账号信息</th></tr>
        <tr>
            <th>账号</th>
            <td><input type="text" id="txtadminaccount" runat="server" /></td>
            <th>密码</th>
            <td><input type="text" id="txtadminpassword" runat="server" /><label style="color:red">修改用户信息时，不显示密码</label></td>
        </tr>
        <tr>
            <th>姓名</th>
            <td><input type="text" id="txtadminname" runat="server" /></td>
            <th>状态</th>
            <td>
                 <select id="ddladminflag" runat="server">
                    <option value="0">冻结</option>
	                <option value="1" selected="selected">正常</option>
                </select>
            </td>
        </tr>
        <tr>
            <th>绑定安全令编号</th>
            <td colspan="3"><input type="text" id="txtadmintoken" runat="server" /></td>
        </tr>
        <tr>
            <th>安&nbsp;全&nbsp;令：</th>
            <td colspan="3"><input type="text" id="txttoken" runat="server" /></td>
        </tr>
        <tr style="text-align:center;">
            <th colspan="4"><asp:Button ID="btnok" runat="server" Text="确定" OnClick="Click" CssClass="btnAdd"/></th>
        </tr>
        <tr style="text-align:center;">
            <th colspan="4"><label class="err red" id="lblerr" runat="server"></label>  </th>
        </tr>
    </table>
    <asp:HiddenField ID="hidadminid" runat="server" />
</asp:Content>
