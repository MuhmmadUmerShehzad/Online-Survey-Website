<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SurveyorDashboard.aspx.vb" Inherits="SurveyorDashboard" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Surveyor Dashboard</title>
</head>
<body>
    <form runat="server">
        <h2>Surveyor Dashboard</h2>

        <asp:Label ID="lblWelcome" runat="server"></asp:Label>
        <br /><br />

        <asp:Button ID="btnTakeSurvey" runat="server" Text="Take Survey" />
        <br /><br />

        <asp:Button ID="btnMyResponses" runat="server" Text="My Responses" />
        <br /><br />

        <asp:Button ID="btnLogout" runat="server" Text="Logout" OnClick="btnLogout_Click" />
    </form>
</body>
</html>