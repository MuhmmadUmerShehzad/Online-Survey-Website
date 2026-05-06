<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ResponseDetails.aspx.vb" Inherits="ResponseDetails" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Response Details</title>
</head>
<body>
    <form id="form1" runat="server">
        <h2>Response Details</h2>


        <asp:Label ID="lblMsg" runat="server"></asp:Label>
        <br /><br />

        <asp:Label ID="lblSurveyInfo" runat="server" Font-Size="Large" Font-Bold="true"></asp:Label>
        <br />
        <asp:Label ID="lblUserInfo" runat="server"></asp:Label>
        <br /><br />

        <asp:GridView ID="gvDetails" runat="server" AutoGenerateColumns="False">
            <Columns>
                <asp:BoundField DataField="QuestionText" HeaderText="Question" />
                <asp:BoundField DataField="SelectedOption" HeaderText="User Answer" />
            </Columns>
        </asp:GridView>

        <br /><br />
        <asp:Button ID="btnBack" runat="server" Text="Back to Responses" OnClick="btnBack_Click" />
    </form>
</body>
</html>
