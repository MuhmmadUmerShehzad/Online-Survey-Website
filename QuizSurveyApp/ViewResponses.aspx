<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ViewResponses.aspx.vb" Inherits="ViewResponses" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>User Responses</title>
</head>
<body>
    <form id="form1" runat="server">
        <h2>User Responses</h2>


        <asp:Label ID="lblMsg" runat="server"></asp:Label>
        <br /><br />

        <asp:GridView ID="gvResponses" runat="server" AutoGenerateColumns="False" 
            EmptyDataText="No responses found.">
            <Columns>
                <asp:BoundField DataField="ResponseID" HeaderText="Response ID" />
                <asp:BoundField DataField="SurveyTitle" HeaderText="Survey Title" />
                <asp:BoundField DataField="UserName" HeaderText="Responded By" />
                <asp:BoundField DataField="ResponseDate" HeaderText="Date" DataFormatString="{0:g}" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:HyperLink runat="server" NavigateUrl='<%# "ResponseDetails.aspx?rid=" & Eval("ResponseID") %>' Text="View Details" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

        <br /><br />
        <asp:Button ID="btnDashboard" runat="server" Text="Back to Dashboard" OnClick="btnDashboard_Click" />
        <asp:Button ID="btnLogout" runat="server" Text="Logout" OnClick="btnLogout_Click" />
    </form>
</body>
</html>
