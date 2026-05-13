<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Login.aspx.vb" Inherits="Login" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title>Login Survey System</title>
    <style>
        /* Basic Styling for the Login Page */
        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background-color: #f4f4f9;
        }

        .login-container {
            width: 350px;
            margin: 100px auto;
            padding: 20px;
            background-color: white;
            border-radius: 8px;
            box-shadow: 0 4px 8px rgba(0,0,0,0.1);
        }

        h2 {
            text-align: center;
            color: #333;
        }

        .input-field {
            width: 100%;
            padding: 10px;
            margin: 10px 0;
            border: 1px solid #ccc;
            border-radius: 4px;
            box-sizing: border-box; /* Ensures padding doesn't affect width */
        }

        .login-button {
            width: 100%;
            padding: 10px;
            background-color: #007bff;
            color: white;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            font-size: 16px;
        }

        .login-button:hover {
            background-color: #0056b3;
        }

        .error-message {
            color: red;
            font-size: 14px;
            display: block;
            margin-bottom: 10px;
            text-align: center;
        }

        .info-box {
            margin-top: 20px;
            padding: 10px;
            border: 1px dashed #007bff;
            background-color: #f0f7ff;
            border-radius: 5px;
            font-size: 0.85em;
        }
    </style>
</head>

<body>
    <form id="form1" runat="server">
        <div class="login-container">
            <h2>User Login</h2>

            <!-- Error message label -->
            <asp:Label ID="lblError" runat="server" CssClass="error-message"></asp:Label>

            <!-- Login Inputs -->
            <asp:TextBox ID="txtUsername" runat="server" CssClass="input-field" Placeholder="Username"></asp:TextBox>
            <asp:TextBox ID="txtPassword" runat="server" CssClass="input-field" TextMode="Password" Placeholder="Password"></asp:TextBox>

            <!-- Submit Button -->
            <asp:Button ID="btnLogin" runat="server" Text="Sign In" CssClass="login-button" OnClick="btnLogin_Click" />

            <!-- Test credentials for evaluator -->
            <div class="info-box">
                <strong>Project Test Credentials:</strong><br />
                Admin: <code>admin</code> / <code>123</code><br />
                Builder: <code>builder</code> / <code>123</code><br />
                Surveyor: <code>umer</code> / <code>123</code>
            </div>
        </div>
    </form>
</body>

</html>