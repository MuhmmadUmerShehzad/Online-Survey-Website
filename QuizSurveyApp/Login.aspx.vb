Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Web.Configuration

Partial Class Login
    Inherits System.Web.UI.Page

    ' Database connection object using string from Web.config
    Dim con As New SqlConnection(WebConfigurationManager.ConnectionStrings("SurveyDBConnection").ConnectionString)

    ' Event handler for the login button click
    Protected Sub btnLogin_Click(sender As Object, e As EventArgs)

        ' Step 1: Get inputs from textboxes and remove extra spaces
        Dim username As String = txtUsername.Text.Trim()
        Dim password As String = txtPassword.Text.Trim()

        ' Step 2: Validate if fields are empty
        If username = "" Or password = "" Then
            lblError.Text = "Please enter both username and password"
            Exit Sub
        End If

        Try
            ' Step 3: Open connection to the database
            con.Open()

            ' Step 4: Create SQL query to verify user credentials
            Dim query As String = "SELECT UserID, UserRole FROM Users WHERE UserName=@username AND UserPassword=@password"
            Dim cmd As New SqlCommand(query, con)

            ' Step 5: Use parameters to prevent SQL Injection
            cmd.Parameters.AddWithValue("@username", username)
            cmd.Parameters.AddWithValue("@password", password)

            ' Step 6: Execute the query and read results
            Dim reader As SqlDataReader = cmd.ExecuteReader()

            If reader.Read() Then
                ' User found! Store user info in Session variables for global access
                Session("UserID") = reader("UserID").ToString()
                Session("UserRole") = reader("UserRole").ToString()
                Session("UserName") = username

                Dim role As String = reader("UserRole").ToString()

                ' Close reader and connection before redirecting
                reader.Close()
                con.Close()

                ' Step 7: Redirect user to their respective dashboard based on role
                If role = "Admin" Then
                    Response.Redirect("AdminDashboard.aspx")
                ElseIf role = "Builder" Then
                    Response.Redirect("BuilderDashboard.aspx")
                ElseIf role = "Surveyor" Then
                    Response.Redirect("SurveyorDashboard.aspx")
                End If

            Else
                ' If no record found
                lblError.Text = "Invalid username or password"
            End If

        Catch ex As Exception
            ' Show error message if something goes wrong
            lblError.Text = "Database Error: " & ex.Message
        Finally
            ' Always ensure connection is closed
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
        End Try

    End Sub

End Class