Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration

Partial Class ManageUsers
    Inherits System.Web.UI.Page

    Dim con As New SqlConnection(WebConfigurationManager.ConnectionStrings("SurveyDBConnection").ConnectionString)

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("UserRole") Is Nothing OrElse Session("UserRole").ToString() <> "Admin" Then
            Response.Redirect("Login.aspx")
        End If

        lblUserRole.Text = Session("UserRole").ToString()

        If Not IsPostBack Then
            LoadUsers()
        End If
    End Sub

    Private Sub LoadUsers()
        Try
            Dim query As String = "SELECT UserID, UserName, Email, UserRole FROM Users"
            Dim cmd As New SqlCommand(query, con)
            Dim dt As New DataTable()

            con.Open()
            Dim da As New SqlDataAdapter(cmd)
            da.Fill(dt)
            con.Close()

            gvUsers.DataSource = dt
            gvUsers.DataBind()
        Catch ex As Exception
            lblMsg.Text = "Error: " & ex.Message
            lblMsg.ForeColor = Drawing.Color.Red
        Finally
            If con.State = ConnectionState.Open Then con.Close()
        End Try
    End Sub

    Protected Sub btnAddUser_Click(sender As Object, e As EventArgs)
        Try
            Dim query As String = "INSERT INTO Users (UserName, Email, UserPassword, UserRole) VALUES (@name, @email, @pass, @role)"
            Dim cmd As New SqlCommand(query, con)
            cmd.Parameters.AddWithValue("@name", txtUserName.Text.Trim())
            cmd.Parameters.AddWithValue("@email", txtEmail.Text.Trim())
            cmd.Parameters.AddWithValue("@pass", txtPassword.Text.Trim())
            cmd.Parameters.AddWithValue("@role", ddlRole.SelectedValue)

            con.Open()
            cmd.ExecuteNonQuery()
            con.Close()

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

    Protected Sub gvUsers_RowDeleting(sender As Object, e As GridViewDeleteEventArgs)
        Try
            Dim userID As Integer = Convert.ToInt32(gvUsers.DataKeys(e.RowIndex).Value)
            Dim query As String = "DELETE FROM Users WHERE UserID = @id"
            Dim cmd As New SqlCommand(query, con)
            cmd.Parameters.AddWithValue("@id", userID)

            con.Open()
            cmd.ExecuteNonQuery()
            con.Close()

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

    Private Sub ClearForm()
        txtUserName.Text = ""
        txtEmail.Text = ""
        txtPassword.Text = ""
    End Sub

    Protected Sub btnDashboard_Click(sender As Object, e As EventArgs)
        Response.Redirect("AdminDashboard.aspx")
    End Sub

    Protected Sub btnLogout_Click(sender As Object, e As EventArgs)
        Session.Clear()
        Response.Redirect("Login.aspx")
    End Sub
End Class
