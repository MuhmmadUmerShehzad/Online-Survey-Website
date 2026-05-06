Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Web.Configuration

Partial Class Login
    Inherits System.Web.UI.Page

    ' Connection string retrieved from Web.config
    Dim con As New SqlConnection(WebConfigurationManager.ConnectionStrings("SurveyDBConnection").ConnectionString)

    Protected Sub btnLogin_Click(sender As Object, e As EventArgs)

        Dim username As String = txtUsername.Text.Trim()
        Dim password As String = txtPassword.Text.Trim()

        If username = "" Or password = "" Then
            lblError.Text = "Please enter username and password"
            Exit Sub
        End If

        Try
            con.Open()

            Dim query As String = "SELECT UserID, UserRole FROM Users WHERE                 UserName=@username AND UserPassword=@password"
            Dim cmd As New SqlCommand(query, con)

            cmd.Parameters.AddWithValue("@username", username)
            cmd.Parameters.AddWithValue("@password", password)

            Dim reader As SqlDataReader = cmd.ExecuteReader()

            If reader.Read() Then

                ' Store session
                Session("UserID") = reader("UserID").ToString()
                Session("UserRole") = reader("UserRole").ToString()
                Session("UserName") = username

                Dim role As String = reader("UserRole").ToString()

                reader.Close()
                con.Close()
                ' Redirect based on role
                If role = "Admin" Then
                    Response.Redirect("AdminDashboard.aspx")
                ElseIf role = "Builder" Then
                    Response.Redirect("BuilderDashboard.aspx")
                ElseIf role = "Surveyor" Then
                    Response.Redirect("SurveyorDashboard.aspx")
                End If

            Else
                lblError.Text = "Invalid username or password"
            End If

        Catch ex As Exception
            lblError.Text = "Error: " & ex.Message
        Finally
            con.Close()
        End Try

    End Sub

End Class