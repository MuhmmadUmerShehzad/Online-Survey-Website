<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Login.aspx.vb" Inherits="Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
    <style>
        body { font-family: Arial; }
        .container { width: 300px; margin: 100px auto; }
        input { width: 100%; padding: 8px; margin: 5px 0; }
        button { width: 100%; padding: 8px; }
        .error { color: red; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h2>Login</h2>

            <asp:Label ID="lblError" runat="server" CssClass="error"></asp:Label>

            <asp:TextBox ID="txtUsername" runat="server" Placeholder="Username"></asp:TextBox>

            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" Placeholder="Password"></asp:TextBox>

            <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" />
        </div>
    </form>
</body>
</html>