<%@ Page Language="VB" AutoEventWireup="false" CodeFile="BuilderDashboard.aspx.vb" Inherits="BuilderDashboard" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Builder Dashboard</title>
</head>
<body>
    <form runat="server">
        <h2>Survey Builder Dashboard</h2>

        <asp:Label ID="lblWelcome" runat="server"></asp:Label>
        <br /><br />

        <asp:Button ID="btnCreateSurvey" runat="server" Text="Create Survey" />
        <br /><br />

        <asp:Button ID="btnViewSurveys" runat="server" Text="View My Surveys" />
        <br /><br />

        <asp:Button ID="btnLogout" runat="server" Text="Logout" OnClick="btnLogout_Click" />
    </form>
</body>
</html>