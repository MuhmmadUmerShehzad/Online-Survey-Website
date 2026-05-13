<%@ Page Language="VB" AutoEventWireup="false" CodeFile="CreateSurvey.aspx.vb" Inherits="CreateSurvey" %>


<!DOCTYPE html>
<html>
<head runat="server">
    <title>Create Survey - Survey System</title>
    <style>
        body { font-family: 'Segoe UI', Arial, sans-serif; padding: 20px; background-color: #f9f9f9; }
        .form-section { background: white; padding: 20px; border-radius: 5px; box-shadow: 0 2px 5px rgba(0,0,0,0.1); margin-bottom: 20px; }
        .input-group { margin-bottom: 15px; }
        label { display: block; margin-bottom: 5px; font-weight: bold; }
        input[type="text"], select { width: 100%; padding: 8px; border: 1px solid #ddd; border-radius: 4px; box-sizing: border-box; }
        .btn { padding: 10px 15px; border: none; border-radius: 4px; cursor: pointer; color: white; }
        .btn-add { background-color: #28a745; }
        .btn-save { background-color: #007bff; }
        .btn-back { background-color: #6c757d; }
        .btn-logout { background-color: #dc3545; }
        .grid-view { width: 100%; border-collapse: collapse; margin-top: 15px; }
        .grid-view th, .grid-view td { border: 1px solid #ddd; padding: 8px; text-align: left; }
        .grid-view th { background-color: #f2f2f2; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <h2>Survey Builder Tool</h2>
        <hr />

        <asp:Label ID="lblMsg" runat="server" Font-Bold="true"></asp:Label>

        <div class="form-section">
            <h4>Step 1: Survey Title</h4>
            <div class="input-group">
                <asp:TextBox ID="txtSurveyTitle" runat="server" Placeholder="e.g. Student Satisfaction Survey"></asp:TextBox>
            </div>
        </div>

        <div class="form-section">
            <h4>Step 2: Add Questions</h4>
            
            <div class="input-group">
                <label>Question Text:</label>
                <asp:TextBox ID="txtQuestion" runat="server" Placeholder="Enter your question here..."></asp:TextBox>
            </div>

            <div class="input-group">
                <label>Type of Question:</label>
                <asp:DropDownList ID="ddlType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
                    <asp:ListItem Text="Multiple Choice (4 Options)" Value="MCQ"></asp:ListItem>
                    <asp:ListItem Text="True / False" Value="TrueFalse"></asp:ListItem>
                </asp:DropDownList>
            </div>

            <div class="input-group">
                <label>Options:</label>
                <asp:TextBox ID="txtOption1" runat="server" Placeholder="Option 1"></asp:TextBox>
                <asp:TextBox ID="txtOption2" runat="server" Placeholder="Option 2"></asp:TextBox>
                
                <asp:Panel ID="pnlExtraOptions" runat="server">
                    <asp:TextBox ID="txtOption3" runat="server" Placeholder="Option 3"></asp:TextBox>
                    <asp:TextBox ID="txtOption4" runat="server" Placeholder="Option 4"></asp:TextBox>
                </asp:Panel>
            </div>

            <asp:Button ID="btnAdd" runat="server" Text="Add to List" CssClass="btn btn-add" OnClick="btnAdd_Click" />
        </div>

        <div class="form-section">
            <h4>Step 3: Preview & Save</h4>
            <!-- Questions List Grid -->
            <asp:GridView ID="gvQuestions" runat="server" CssClass="grid-view" AutoGenerateColumns="True">
                <HeaderStyle BackColor="#007bff" ForeColor="White" />
            </asp:GridView>
            <br />
            <asp:Button ID="btnSaveSurvey" runat="server" Text="Finish and Save Survey" CssClass="btn btn-save" OnClick="btnSaveSurvey_Click" />
        </div>

        <div style="margin-top: 30px;">
            <asp:Button ID="btnDashboard" runat="server" Text="Dashboard" CssClass="btn btn-back" OnClick="btnDashboard_Click" />
            <asp:Button ID="btnLogout" runat="server" Text="Sign Out" CssClass="btn btn-logout" OnClick="btnLogout_Click" />
        </div>
    </form>
</body>
</html>