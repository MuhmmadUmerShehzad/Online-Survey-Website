Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration

Partial Class ViewResponses
    Inherits System.Web.UI.Page

    Dim con As New SqlConnection(WebConfigurationManager.ConnectionStrings("SurveyDBConnection").ConnectionString)

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("UserRole") Is Nothing Then
            Response.Redirect("Login.aspx")
        End If

        lblUserRole.Text = Session("UserRole").ToString()

        If Not IsPostBack Then
            LoadResponses()
        End If
    End Sub

    Private Sub LoadResponses()
        Try
            Dim role As String = Session("UserRole").ToString()
            Dim userID As String = Session("UserID").ToString()
            
            Dim query As String = "SELECT r.ResponseID, s.Title AS SurveyTitle, ISNULL(u.UserName, 'Anonymous') AS UserName, r.ResponseDate " & _
                                 "FROM Responses r " & _
                                 "JOIN Surveys s ON r.SurveyID = s.SurveyID " & _
                                 "LEFT JOIN Users u ON r.UserID = u.UserID "

            ' Filter based on role
            If role = "Builder" Then
                ' Builders see responses to THEIR surveys
                query &= " WHERE s.CreatedBy = @uid"
            ElseIf role = "Surveyor" Then
                ' Surveyors see THEIR OWN responses
                query &= " WHERE r.UserID = @uid"
            End If
            ' Admins see everything

            Dim cmd As New SqlCommand(query, con)
            If role = "Builder" Or role = "Surveyor" Then
                cmd.Parameters.AddWithValue("@uid", userID)
            End If

            Dim dt As New DataTable()
            con.Open()
            Dim da As New SqlDataAdapter(cmd)
            da.Fill(dt)
            con.Close()

            gvResponses.DataSource = dt
            gvResponses.DataBind()

        Catch ex As Exception
            lblMsg.Text = "Error loading responses: " & ex.Message
            lblMsg.ForeColor = Drawing.Color.Red
        Finally
            If con.State = ConnectionState.Open Then con.Close()
        End Try
    End Sub

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

    Protected Sub btnLogout_Click(sender As Object, e As EventArgs)
        Session.Clear()
        Response.Redirect("Login.aspx")
    End Sub
End Class
