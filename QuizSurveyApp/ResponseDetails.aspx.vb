Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration

Partial Class ResponseDetails
    Inherits System.Web.UI.Page

    Dim con As New SqlConnection(WebConfigurationManager.ConnectionStrings("SurveyDBConnection").ConnectionString)

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("UserRole") Is Nothing Then
            Response.Redirect("Login.aspx")
        End If

        lblUserRole.Text = Session("UserRole").ToString()

        If Not IsPostBack Then
            If Request.QueryString("rid") IsNot Nothing Then
                LoadDetails(Request.QueryString("rid"))
            Else
                Response.Redirect("ViewResponses.aspx")
            End If
        End If
    End Sub

    Private Sub LoadDetails(responseID As String)
        Try
            ' Get Header Info
            Dim queryHeader As String = "SELECT s.Title, ISNULL(u.UserName, 'Anonymous') AS UserName, r.ResponseDate " &
                                       "FROM Responses r " &
                                       "JOIN Surveys s ON r.SurveyID = s.SurveyID " &
                                       "LEFT JOIN Users u ON r.UserID = u.UserID " &
                                       "WHERE r.ResponseID = @rid"

            Dim cmdHeader As New SqlCommand(queryHeader, con)
            cmdHeader.Parameters.AddWithValue("@rid", responseID)
            
            con.Open()
            Dim reader As SqlDataReader = cmdHeader.ExecuteReader()
            If reader.Read() Then
                lblSurveyInfo.Text = "Survey: " & reader("Title").ToString()
                lblUserInfo.Text = "Responded by: " & reader("UserName").ToString() & " on " & Convert.ToDateTime(reader("ResponseDate")).ToString("g")
            End If
            reader.Close()
            con.Close()

            ' Get Answers
            Dim queryAnswers As String = "SELECT q.QuestionText, o.OptionText AS SelectedOption " & _
                                        "FROM Answers a " & _
                                        "JOIN Questions q ON a.QuestionID = q.QuestionID " & _
                                        "JOIN Options o ON a.SelectedOptionID = o.OptionID " & _
                                        "WHERE a.ResponseID = @rid"
            
            Dim cmdAnswers As New SqlCommand(queryAnswers, con)
            cmdAnswers.Parameters.AddWithValue("@rid", responseID)
            
            Dim dt As New DataTable()
            con.Open()
            Dim da As New SqlDataAdapter(cmdAnswers)
            da.Fill(dt)
            con.Close()

            gvDetails.DataSource = dt
            gvDetails.DataBind()

        Catch ex As Exception
            lblMsg.Text = "Error loading details: " & ex.Message
            lblMsg.ForeColor = Drawing.Color.Red
        Finally
            If con.State = ConnectionState.Open Then con.Close()
        End Try
    End Sub

    Protected Sub btnBack_Click(sender As Object, e As EventArgs)
        Response.Redirect("ViewResponses.aspx")
    End Sub
End Class
