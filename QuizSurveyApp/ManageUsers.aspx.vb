
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration

Partial Class ManageUsers
    Inherits System.Web.UI.Page

    ' Global connection object
    Dim con As New SqlConnection(WebConfigurationManager.ConnectionStrings("SurveyDBConnection").ConnectionString)

    ' Page Load event
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        ' Security check for Admin role
        If Session("UserRole") Is Nothing OrElse Session("UserRole").ToString() <> "Admin" Then
            Response.Redirect("Login.aspx")
        End If

        ' Only load users if it's the first time visiting the page (not on button click)
        If Not IsPostBack Then
            LoadUsers()
        End If
    End Sub

    ' Procedure to fetch all users from database and display in GridView
    Private Sub LoadUsers()
        Try
            ' SQL query to get user details
            Dim query As String = "SELECT UserID, UserName, Email, UserRole FROM Users"
            Dim cmd As New SqlCommand(query, con)
            Dim dt As New DataTable()

            ' Open connection, fill data table using adapter, then close
            con.Open()
            Dim da As New SqlDataAdapter(cmd)
            da.Fill(dt)
            con.Close()

            ' Bind the data table to the GridView control
            gvUsers.DataSource = dt
            gvUsers.DataBind()
        Catch ex As Exception
            ' Display error message if database query fails
            lblMsg.Text = "Database Error: " & ex.Message
            lblMsg.ForeColor = Drawing.Color.Red
        Finally
            ' Safety check to close connection
            If con.State = ConnectionState.Open Then con.Close()
        End Try
    End Sub

    ' Event handler to add a new user to the system
    Protected Sub btnAddUser_Click(sender As Object, e As EventArgs)
        Try
            ' SQL Insert command with parameters for safety
            Dim query As String = "INSERT INTO Users (UserName, Email, UserPassword, UserRole) VALUES (@name, @email, @pass, @role)"
            Dim cmd As New SqlCommand(query, con)
            cmd.Parameters.AddWithValue("@name", txtUserName.Text.Trim())
            cmd.Parameters.AddWithValue("@email", txtEmail.Text.Trim())
            cmd.Parameters.AddWithValue("@pass", txtPassword.Text.Trim())
            cmd.Parameters.AddWithValue("@role", ddlRole.SelectedValue)

            ' Execute the command
            con.Open()
            cmd.ExecuteNonQuery()
            con.Close()

            ' Show success message and refresh list
            lblMsg.Text = "User added successfully!"
            lblMsg.ForeColor = Drawing.Color.Green
            LoadUsers()
            ClearForm()
        Catch ex As Exception
            lblMsg.Text = "Error adding user: " & ex.Message
            lblMsg.ForeColor = Drawing.Color.Red
        Finally
            If con.State = ConnectionState.Open Then con.Close()
        End Try
    End Sub

    ' Event handler to delete a user when Delete link is clicked in GridView
    Protected Sub gvUsers_RowDeleting(sender As Object, e As GridViewDeleteEventArgs)
        Try
            ' Get the UserID of the selected row
            Dim userID As Integer = Convert.ToInt32(gvUsers.DataKeys(e.RowIndex).Value)
            Dim query As String = "DELETE FROM Users WHERE UserID = @id"
            Dim cmd As New SqlCommand(query, con)
            cmd.Parameters.AddWithValue("@id", userID)

            ' Execute delete
            con.Open()
            cmd.ExecuteNonQuery()
            con.Close()

            ' Show success message and refresh list
            lblMsg.Text = "User deleted successfully!"
            lblMsg.ForeColor = Drawing.Color.Green
            LoadUsers()
        Catch ex As Exception
            lblMsg.Text = "Error deleting user: " & ex.Message
            lblMsg.ForeColor = Drawing.Color.Red
        Finally
            If con.State = ConnectionState.Open Then con.Close()
        End Try
    End Sub

    ' Helper method to clear the input fields
    Private Sub ClearForm()
        txtUserName.Text = ""
        txtEmail.Text = ""
        txtPassword.Text = ""
    End Sub

    ' Back to dashboard button
    Protected Sub btnDashboard_Click(sender As Object, e As EventArgs)
        Response.Redirect("AdminDashboard.aspx")
    End Sub

    ' Logout logic
    Protected Sub btnLogout_Click(sender As Object, e As EventArgs)
        Session.Clear()
        Response.Redirect("Login.aspx")
    End Sub
End Class
