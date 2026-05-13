<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ManageUsers.aspx.vb" Inherits="ManageUsers" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Manage Users - Admin Panel</title>
    <style>
        body { font-family: 'Segoe UI', Tahoma, sans-serif; background-color: #f0f2f5; padding: 20px; }
        .admin-box { background: #fff; padding: 20px; border-radius: 8px; box-shadow: 0 4px 6px rgba(0,0,0,0.1); margin-bottom: 20px; }
        h2, h3 { color: #333; }
        .input-ctrl { padding: 8px; margin: 5px; border: 1px solid #ccc; border-radius: 4px; }
        .btn { padding: 8px 15px; border-radius: 4px; border: none; cursor: pointer; }
        .btn-primary { background-color: #007bff; color: white; }
        .btn-secondary { background-color: #6c757d; color: white; }
        .grid-style { width: 100%; border-collapse: collapse; margin-top: 15px; }
        .grid-style th, .grid-style td { padding: 12px; border: 1px solid #ddd; text-align: left; }
        .grid-style th { background-color: #007bff; color: white; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="admin-box">
            <h2>User Management System</h2>
            <asp:Label ID="lblMsg" runat="server" Font-Bold="true"></asp:Label>
        </div>

        <!-- Section: Create New User -->
        <div class="admin-box">
            <h3>Add New System User</h3>
            <asp:TextBox ID="txtUserName" runat="server" CssClass="input-ctrl" Placeholder="Username"></asp:TextBox>
            <asp:TextBox ID="txtEmail" runat="server" CssClass="input-ctrl" Placeholder="Email"></asp:TextBox>
            <asp:TextBox ID="txtPassword" runat="server" CssClass="input-ctrl" TextMode="Password" Placeholder="Password"></asp:TextBox>
            <asp:DropDownList ID="ddlRole" runat="server" CssClass="input-ctrl">
                <asp:ListItem Text="Admin" Value="Admin"></asp:ListItem>
                <asp:ListItem Text="Builder" Value="Builder"></asp:ListItem>
                <asp:ListItem Text="Surveyor" Value="Surveyor"></asp:ListItem>
            </asp:DropDownList>
            <asp:Button ID="btnAddUser" runat="server" Text="Create User" CssClass="btn btn-primary" OnClick="btnAddUser_Click" />
        </div>

        <!-- Section: View and Manage Users -->
        <div class="admin-box">
            <h3>Registered Users</h3>
            <asp:GridView ID="gvUsers" runat="server" CssClass="grid-style" AutoGenerateColumns="False" 
                OnRowDeleting="gvUsers_RowDeleting" DataKeyNames="UserID">
                <Columns>
                    <asp:BoundField DataField="UserID" HeaderText="User ID" />
                    <asp:BoundField DataField="UserName" HeaderText="Username" />
                    <asp:BoundField DataField="Email" HeaderText="Email Address" />
                    <asp:BoundField DataField="UserRole" HeaderText="Role" />
                    <asp:CommandField ShowDeleteButton="True" DeleteText="Delete User" ControlStyle-ForeColor="Red" />
                </Columns>
            </asp:GridView>
        </div>

        <!-- Navigation -->
        <div>
            <asp:Button ID="btnDashboard" runat="server" Text="Return to Dashboard" CssClass="btn btn-secondary" OnClick="btnDashboard_Click" />
            <asp:Button ID="btnLogout" runat="server" Text="Log Out" CssClass="btn btn-secondary" style="background-color:#dc3545;" OnClick="btnLogout_Click" />
        </div>
    </form>
</body>
</html>
