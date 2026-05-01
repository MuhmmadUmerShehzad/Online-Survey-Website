Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration

Partial Class ViewSurveys
    Inherits System.Web.UI.Page

    Dim con As New SqlConnection(WebConfigurationManager.ConnectionStrings("SurveyDBConnection").ConnectionString)


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try

            If Session("UserRole") Is Nothing Then
                Response.Redirect("Login.aspx")
                Exit Sub
            End If

            ' BUG FIX #1 — Role label moved BEFORE the UserID line.
            ' Previously, if Session("UserID") was Nothing, line
            '   lblMsg.Text = "UserID: " & Session("UserID").ToString()
            ' threw a NullReferenceException, which the empty Catch swallowed
            ' silently — so lblUserRole.Text was never reached and stayed blank.
            lblUserRole.Text = Session("UserRole").ToString()

            If Session("UserID") IsNot Nothing Then
                lblMsg.Text = "UserID: " & Session("UserID").ToString()
            Else
                lblMsg.Text = "UserID: N/A"
            End If

            If Not IsPostBack Then
                LoadSurveys()
            End If

        Catch ex As Exception
            ' BUG FIX #2 — Never leave Catch blocks empty.
            ' Empty Catch was silently swallowing ALL exceptions (DB errors,
            ' null-reference errors, etc.), making both the role label and the
            ' GridView appear empty with no indication of what went wrong.
            lblMsg.Text = "Error: " & ex.Message
        End Try
    End Sub

    Private Sub LoadSurveys()

        Try
            Dim query As String = "SELECT SurveyID, Title, CreatedDate FROM Surveys"

            ' Builders only see their own surveys, others see all
            If Session("UserRole").ToString() = "Builder" Then
                query &= " WHERE CreatedBy = @uid"
            End If

            Dim cmd As New SqlCommand(query, con)

            If Session("UserRole").ToString() = "Builder" Then
                cmd.Parameters.AddWithValue("@uid", Session("UserID"))
            End If

            Dim dt As New DataTable()

            con.Open()
            Dim da As New SqlDataAdapter(cmd)
            da.Fill(dt)

            gvSurveys.DataSource = dt
            gvSurveys.DataBind()

            ' Hide "Take Survey" button for Builders
            If Session("UserRole").ToString() = "Surveyor" Then
                gvSurveys.Columns(3).Visible = True
            Else
                gvSurveys.Columns(3).Visible = False
            End If

        Catch ex As Exception
            ' Surface DB/query errors so they are visible during development
            lblMsg.Text = "Survey load error: " & ex.Message
        Finally
            con.Close()
        End Try

    End Sub

    Protected Sub gvSurveys_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            If e.CommandName = "TakeSurvey" Then
                Dim surveyID As String = e.CommandArgument.ToString()
                Session("SurveyID") = surveyID
                Response.Redirect("TakeSurvey.aspx")
            End If
        Catch ex As Exception
            lblMsg.Text = "Error: " & ex.Message
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