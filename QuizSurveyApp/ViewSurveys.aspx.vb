
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration

Partial Class ViewSurveys
    Inherits System.Web.UI.Page

    ' Create connection using the Web.config connection string
    Dim con As New SqlConnection(WebConfigurationManager.ConnectionStrings("SurveyDBConnection").ConnectionString)

    ' Page Load event
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            ' Step 1: Check if the user is logged in
            If Session("UserRole") Is Nothing Then
                Response.Redirect("Login.aspx")
                Exit Sub
            End If

            ' Optional: Display UserID for debugging (typical in student projects)
            If Session("UserID") IsNot Nothing Then
                lblMsg.Text = "Current User ID: " & Session("UserID").ToString()
            End If

            ' Step 2: Load the surveys list on first load
            If Not IsPostBack Then
                LoadSurveys()
            End If

        Catch ex As Exception
            lblMsg.Text = "System Error: " & ex.Message
        End Try
    End Sub

    ' Procedure to fetch surveys from database with filtering based on user role
    Private Sub LoadSurveys()
        Try
            ' Default query to select all surveys
            Dim query As String = "SELECT SurveyID, Title, CreatedDate FROM Surveys"

            ' Step 1: Apply role-based filters
            ' If the user is a "Builder", they should only see surveys they created
            If Session("UserRole").ToString() = "Builder" Then
                query &= " WHERE CreatedBy = @uid"
            
            ' If the user is a "Surveyor", they should only see surveys they HAVEN'T taken yet
            ElseIf Session("UserRole").ToString() = "Surveyor" Then
                query &= " WHERE SurveyID NOT IN (SELECT SurveyID FROM Responses WHERE UserID = @uid)"
            End If

            Dim cmd As New SqlCommand(query, con)

            ' Step 2: Add parameters if filtering is applied
            If Session("UserRole").ToString() = "Builder" OrElse Session("UserRole").ToString() = "Surveyor" Then
                cmd.Parameters.AddWithValue("@uid", Session("UserID"))
            End If

            ' Step 3: Fetch data into a DataTable
            Dim dt As New DataTable()
            con.Open()
            Dim da As New SqlDataAdapter(cmd)
            da.Fill(dt)
            con.Close()

            ' Step 4: Bind data to the GridView
            gvSurveys.DataSource = dt
            gvSurveys.DataBind()

            ' Step 5: UI adjustment - Show "Take Survey" button ONLY for Surveyors
            If Session("UserRole").ToString() = "Surveyor" Then
                gvSurveys.Columns(3).Visible = True
            Else
                gvSurveys.Columns(3).Visible = False
            End If

        Catch ex As Exception
            ' Show descriptive error message if database access fails
            lblMsg.Text = "Database Error: " & ex.Message
            lblMsg.ForeColor = Drawing.Color.Red
        Finally
            ' Ensure connection is closed even if an error occurs
            If con.State = ConnectionState.Open Then con.Close()
        End Try
    End Sub

    ' Handle clicks on the GridView buttons (like "Take Survey")
    Protected Sub gvSurveys_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            ' Check if the clicked command is "TakeSurvey"
            If e.CommandName = "TakeSurvey" Then
                ' Get the SurveyID from command arguments
                Dim surveyID As String = e.CommandArgument.ToString()
                ' Store SurveyID in session so the next page knows which survey to load
                Session("SurveyID") = surveyID
                ' Redirect to the survey taking page
                Response.Redirect("TakeSurvey.aspx")
            End If
        Catch ex As Exception
            lblMsg.Text = "Command Error: " & ex.Message
        End Try
    End Sub

    ' Navigation back to user's dashboard
    Protected Sub btnDashboard_Click(sender As Object, e As EventArgs)
        Dim role As String = Session("UserRole").ToString()
        If role = "Admin" Then
            Response.Redirect("AdminDashboard.aspx")
        ElseIf role = "Builder" Then
            Response.Redirect("BuilderDashboard.aspx")
        ElseIf role = "Surveyor" Then
            Response.Redirect("SurveyorDashboard.aspx")
        End If
    End Sub

    ' Logout logic: Clear session and redirect to login page
    Protected Sub btnLogout_Click(sender As Object, e As EventArgs)
        Session.Clear()
        Response.Redirect("Login.aspx")
    End Sub
End Class