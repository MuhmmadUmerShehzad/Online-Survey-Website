<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ViewSurveys.aspx.vb" Inherits="ViewSurveys" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>View Surveys</title>
</head>
<body>
    <form id="form1" runat="server">

        <h2>Available Surveys</h2>



        <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label>
        <br /><br />

        <asp:GridView ID="gvSurveys" runat="server" 
            AutoGenerateColumns="False"
            OnRowCommand="gvSurveys_RowCommand"
            EmptyDataText="No surveys found.">
            <Columns>
                <asp:BoundField DataField="SurveyID" HeaderText="ID" />
                <asp:BoundField DataField="Title" HeaderText="Survey Title" />
                <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button runat="server" 
                            Text="Take Survey" 
                            CommandName="TakeSurvey" 
                            CommandArgument='<%# Eval("SurveyID") %>' />
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