<%@ Page Language="VB" AutoEventWireup="false" CodeFile="CreateSurvey.aspx.vb" Inherits="CreateSurvey" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Create Survey</title>
</head>
<body>
    <form id="form1" runat="server">

        <h2>Create Survey</h2>



        <asp:Label ID="lblMsg" runat="server" ForeColor="Green"></asp:Label>
        <br /><br />

        <!-- Survey Title -->
        <asp:TextBox ID="txtSurveyTitle" runat="server" Placeholder="Enter Survey Title"></asp:TextBox>
        <br /><br />

        <!-- Question -->
        <asp:TextBox ID="txtQuestion" runat="server" Placeholder="Enter Question"></asp:TextBox>
        <br /><br />

        <!-- Question Type -->
        <asp:DropDownList ID="ddlType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
            <asp:ListItem Text="MCQ" Value="MCQ"></asp:ListItem>
            <asp:ListItem Text="True/False" Value="TrueFalse"></asp:ListItem>
        </asp:DropDownList>
        <br /><br />

        <!-- Options (for MCQ) -->
        <asp:TextBox ID="txtOption1" runat="server" Placeholder="Option 1"></asp:TextBox><br />
        <asp:TextBox ID="txtOption2" runat="server" Placeholder="Option 2"></asp:TextBox><br />
        
        <asp:Panel ID="pnlExtraOptions" runat="server">
            <asp:TextBox ID="txtOption3" runat="server" Placeholder="Option 3"></asp:TextBox><br />
            <asp:TextBox ID="txtOption4" runat="server" Placeholder="Option 4"></asp:TextBox>
        </asp:Panel>
        <br /><br />

        <asp:Button ID="btnAdd" runat="server" Text="Add Question" OnClick="btnAdd_Click" />
        <br /><br />

        <!-- Questions List -->
        <asp:GridView ID="gvQuestions" runat="server" AutoGenerateColumns="True"></asp:GridView>
        <br />

        <asp:Button ID="btnSaveSurvey" runat="server" Text="Save Survey" OnClick="btnSaveSurvey_Click" />

        <br /><br />
        <asp:Button ID="btnDashboard" runat="server" Text="Back to Dashboard" OnClick="btnDashboard_Click" />
        <asp:Button ID="btnLogout" runat="server" Text="Logout" OnClick="btnLogout_Click" />
    </form>

</body>
</html>