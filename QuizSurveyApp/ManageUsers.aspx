<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ManageUsers.aspx.vb" Inherits="ManageUsers" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Manage Users</title>
</head>
<body>
    <form id="form1" runat="server">
        <h2>User Management</h2>


        <asp:Label ID="lblMsg" runat="server"></asp:Label>
        <br /><br />

        <!-- Add User Section -->
        <div>
            <h3>Add New User</h3>
            <asp:TextBox ID="txtUserName" runat="server" Placeholder="Username"></asp:TextBox>
            <asp:TextBox ID="txtEmail" runat="server" Placeholder="Email"></asp:TextBox>
            <asp:TextBox ID="txtPassword" runat="server" Placeholder="Password"></asp:TextBox>
            <asp:DropDownList ID="ddlRole" runat="server">
                <asp:ListItem Text="Admin" Value="Admin"></asp:ListItem>
                <asp:ListItem Text="Builder" Value="Builder"></asp:ListItem>
                <asp:ListItem Text="Surveyor" Value="Surveyor"></asp:ListItem>
            </asp:DropDownList>
            <asp:Button ID="btnAddUser" runat="server" Text="Add User" OnClick="btnAddUser_Click" />
        </div>
        <br /><br />

        <!-- User List Section -->
        <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="False" 
            OnRowDeleting="gvUsers_RowDeleting" DataKeyNames="UserID">
            <Columns>
                <asp:BoundField DataField="UserID" HeaderText="ID" />
                <asp:BoundField DataField="UserName" HeaderText="Username" />
                <asp:BoundField DataField="Email" HeaderText="Email" />
                <asp:BoundField DataField="UserRole" HeaderText="Role" />
                <asp:CommandField ShowDeleteButton="True" DeleteText="Delete" />
            </Columns>
        </asp:GridView>

        <br /><br />
        <asp:Button ID="btnDashboard" runat="server" Text="Back to Dashboard" OnClick="btnDashboard_Click" />
        <asp:Button ID="btnLogout" runat="server" Text="Logout" OnClick="btnLogout_Click" />
    </form>
</body>
</html>
