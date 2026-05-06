<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AdminDashboard.aspx.vb" Inherits="AdminDashboard" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Admin Dashboard</title>
</head>
<body>
    <form runat="server">
        <h2>Admin Dashboard</h2>



        <asp:Label ID="lblWelcome" runat="server"></asp:Label>
        <br /><br />

        <asp:Button ID="btnManageUsers" runat="server" Text="Manage Users" />
        <br /><br />

        <asp:Button ID="btnManageSurveys" runat="server" Text="Manage Surveys" />
        <br /><br />

        <asp:Button ID="btnViewAllResponses" runat="server" Text="View All Responses" />
        <br /><br />

        <asp:Button ID="btnLogout" runat="server" Text="Logout" OnClick="btnLogout_Click" />
    </form>
</body>
</html>