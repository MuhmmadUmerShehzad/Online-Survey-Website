
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration

Partial Class ViewResponses
    Inherits System.Web.UI.Page

    ' Connection object initialized with connection string from Web.config
    Dim con As New SqlConnection(WebConfigurationManager.ConnectionStrings("SurveyDBConnection").ConnectionString)

    ' Page Load event
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        ' Step 1: Authorization check
        If Session("UserRole") Is Nothing Then
            Response.Redirect("Login.aspx")
        End If

        ' Step 2: Load data only on the initial page load
        If Not IsPostBack Then
            LoadResponses()
        End If
    End Sub

    ' Procedure to fetch responses from database with role-based filtering
    Private Sub LoadResponses()
        Try
            Dim role As String = Session("UserRole").ToString()
            Dim userID As String = Session("UserID").ToString()
            
            ' Base SQL query using JOINs to get Survey Title and User Name
            Dim query As String = "SELECT r.ResponseID, s.Title AS SurveyTitle, ISNULL(u.UserName, 'Anonymous') AS UserName, r.ResponseDate " & _
                                 "FROM Responses r " & _
                                 "JOIN Surveys s ON r.SurveyID = s.SurveyID " & _
                                 "LEFT JOIN Users u ON r.UserID = u.UserID "

            ' Step 1: Filter results based on who is logged in
            ' A "Builder" should only see responses for the surveys THEY created
            If role = "Builder" Then
                query &= " WHERE s.CreatedBy = @uid"
            
            ' A "Surveyor" should only see the responses THEY submitted
            ElseIf role = "Surveyor" Then
                query &= " WHERE r.UserID = @uid"
            End If
            ' Note: Admins do not have a filter, so they see ALL responses

            Dim cmd As New SqlCommand(query, con)

            ' Step 2: Add parameters if a filter was applied
            If role = "Builder" Or role = "Surveyor" Then
                cmd.Parameters.AddWithValue("@uid", userID)
            End If

            ' Step 3: Fill DataTable and bind to GridView
            Dim dt As New DataTable()
            con.Open()
            Dim da As New SqlDataAdapter(cmd)
            da.Fill(dt)
            con.Close()

            gvResponses.DataSource = dt
            gvResponses.DataBind()

        Catch ex As Exception
            ' Error handling for database issues
            lblMsg.Text = "Data Load Error: " & ex.Message
            lblMsg.ForeColor = Drawing.Color.Red
        Finally
            ' Ensure the database connection is closed
            If con.State = ConnectionState.Open Then con.Close()
        End Try
    End Sub

    ' Navigation logic back to dashboard
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

    ' Logout logic: clear session and redirect
    Protected Sub btnLogout_Click(sender As Object, e As EventArgs)
        Session.Clear()
        Response.Redirect("Login.aspx")
    End Sub
End Class
