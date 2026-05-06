<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TakeSurvey.aspx.vb" Inherits="TakeSurvey" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Take Survey</title>
</head>
<body>
    <form id="form1" runat="server">
        <div style="font-family: Arial; padding: 20px;">
            <h2>Take Survey: <asp:Label ID="lblSurveyTitle" runat="server"></asp:Label></h2>

            <div style="background-color: #e8f4fd; padding: 10px; margin-bottom: 20px; border: 1px solid #b3d7ff; border-radius: 5px;">
                User: <asp:Label ID="lblUserName" runat="server" Font-Bold="true"></asp:Label>
            </div>

            <asp:Label ID="lblMsg" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
            <br /><br />

            <asp:Repeater ID="rptQuestions" runat="server" OnItemDataBound="rptQuestions_ItemDataBound">
                <ItemTemplate>
                    <div style="margin-bottom: 20px; border-bottom: 1px solid #ccc; padding-bottom: 10px;">
                        <asp:HiddenField ID="hfQuestionID" runat="server" Value='<%# Eval("QuestionID") %>' />
                        <span style="font-weight: bold;"><%# Container.ItemIndex + 1 %>. <%# Eval("QuestionText") %></span>
                        <br /><br />
                        <asp:RadioButtonList ID="rblOptions" runat="server" DataTextField="OptionText" DataValueField="OptionID">
                        </asp:RadioButtonList>
                        <asp:RequiredFieldValidator ID="rfvOption" runat="server" 
                            ControlToValidate="rblOptions" 
                            ErrorMessage="Please select an option" 
                            ForeColor="Red" Display="Dynamic">
                        </asp:RequiredFieldValidator>
                    </div>
                </ItemTemplate>
            </asp:Repeater>

            <br />
            <asp:Button ID="btnSubmit" runat="server" Text="Submit Responses" OnClick="btnSubmit_Click" />
            <asp:Button ID="btnBack" runat="server" Text="Back to Surveys" OnClick="btnBack_Click" CausesValidation="false" />
        </div>
    </form>
</body>
</html>
