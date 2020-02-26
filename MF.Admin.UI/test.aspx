<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="MF.Admin.UI.test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
      <script>
       
        function gethdinfo()
        {
            var hd = document.getElementById("hd").GetHDInfo();
            alert(hd);
            document.getElementById("hidhd").value = hd;
            alert(document.getElementById("hidhd").value);
            window.location.href = "/test.aspx?hd=" + hd;
            return false;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        
        <object id="hd" classid="clsid:BDFBCF85-06FB-43a0-9B99-83C6C60BE7C6" style="display:none;" codebase="Active.cab#version=1,0,1"></object>
    <div>
        <asp:FileUpload  ID="uploadFile" runat="server"/>
        <asp:Button ID="btnLoad" runat="server" Text="上传" OnClick="LoadFile" /><br />
        <asp:HiddenField  ID="hidhd" runat="server"/>
        <asp:Button ID="btnHd" runat="server" Text="验证HD"  OnClientClick="gethdinfo()" OnClick="Click" />
        <input type="button" onclick="gethdinfo()" value="验证HD2" />
    </div>
    </form>
</body>
</html>
