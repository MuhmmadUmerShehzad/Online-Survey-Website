Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration

Partial Class TakeSurvey
    Inherits System.Web.UI.Page

    Dim connectionString As String = WebConfigurationManager.ConnectionStrings("SurveyDBConnection").ConnectionString

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("UserID") Is Nothing Then
            Response.Redirect("Login.aspx")
            Exit Sub
        End If

        If Not IsPostBack Then
            lblUserRole.Text = Session("UserRole").ToString()
            lblUserName.Text = Session("UserName").ToString()

            If Session("SurveyID") IsNot Nothing Then
                LoadSurvey(Convert.ToInt32(Session("SurveyID")))
            Else
                Response.Redirect("ViewSurveys.aspx")
            End If
        End If
    End Sub

    Private Sub LoadSurvey(surveyID As Integer)
        Using con As New SqlConnection(connectionString)
            Try
                ' Load Survey Title
                Dim cmdSurvey As New SqlCommand("SELECT Title FROM Surveys WHERE SurveyID = @sid", con)
                cmdSurvey.Parameters.AddWithValue("@sid", surveyID)
                con.Open()
                Dim title As Object = cmdSurvey.ExecuteScalar()
                If title IsNot Nothing Then
                    lblSurveyTitle.Text = title.ToString()
                Else
                    lblMsg.Text = "Survey not found."
                    lblMsg.ForeColor = Drawing.Color.Red
                    btnSubmit.Visible = False
                    Return
                End If

                ' Load Questions
                Dim cmdQuestions As New SqlCommand("SELECT QuestionID, QuestionText FROM Questions WHERE SurveyID = @sid", con)
                cmdQuestions.Parameters.AddWithValue("@sid", surveyID)
                Dim da As New SqlDataAdapter(cmdQuestions)
                Dim dt As New DataTable()
                da.Fill(dt)

                rptQuestions.DataSource = dt
                rptQuestions.DataBind()

                If dt.Rows.Count = 0 Then
                    lblMsg.Text = "This survey has no questions."
                    lblMsg.ForeColor = Drawing.Color.Red
                    btnSubmit.Visible = False
                End If

            Catch ex As Exception
                lblMsg.Text = "Error loading survey: " & ex.Message
                lblMsg.ForeColor = Drawing.Color.Red
            End Try
        End Using
    End Sub

    Protected Sub rptQuestions_ItemDataBound(sender As Object, e As RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim hfQuestionID As HiddenField = CType(e.Item.FindControl("hfQuestionID"), HiddenField)
            Dim rblOptions As RadioButtonList = CType(e.Item.FindControl("rblOptions"), RadioButtonList)
            Dim questionID As Integer = Convert.ToInt32(hfQuestionID.Value)

            Using con As New SqlConnection(connectionString)
                Dim cmdOptions As New SqlCommand("SELECT OptionID, OptionText FROM Options WHERE QuestionID = @qid", con)
                cmdOptions.Parameters.AddWithValue("@qid", questionID)
                con.Open()
                Dim da As New SqlDataAdapter(cmdOptions)
                Dim dt As New DataTable()
                da.Fill(dt)

                rblOptions.DataSource = dt
                rblOptions.DataBind()
            End Using
        End If
    End Sub

    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs)
        If Not Page.IsValid Then Return

        Dim surveyID As Integer = Convert.ToInt32(Session("SurveyID"))
        Dim userID As Integer = Convert.ToInt32(Session("UserID"))

        Using con As New SqlConnection(connectionString)
            con.Open()
            Dim trans As SqlTransaction = con.BeginTransaction()

            Try
                ' 1. Insert into Responses
                Dim cmdResponse As New SqlCommand("INSERT INTO Responses (SurveyID, UserID, ResponseDate) OUTPUT INSERTED.ResponseID VALUES (@sid, @uid, GETDATE())", con, trans)
                cmdResponse.Parameters.AddWithValue("@sid", surveyID)
                cmdResponse.Parameters.AddWithValue("@uid", userID)
                Dim responseID As Integer = Convert.ToInt32(cmdResponse.ExecuteScalar())

                ' 2. Insert into Answers
                For Each item As RepeaterItem In rptQuestions.Items
                    Dim hfQuestionID As HiddenField = CType(item.FindControl("hfQuestionID"), HiddenField)
                    Dim rblOptions As RadioButtonList = CType(item.FindControl("rblOptions"), RadioButtonList)

                    If rblOptions.SelectedIndex <> -1 Then
                        Dim cmdAnswer As New SqlCommand("INSERT INTO Answers (ResponseID, QuestionID, SelectedOptionID) VALUES (@rid, @qid, @oid)", con, trans)
                        cmdAnswer.Parameters.AddWithValue("@rid", responseID)
                        cmdAnswer.Parameters.AddWithValue("@qid", Convert.ToInt32(hfQuestionID.Value))
                        cmdAnswer.Parameters.AddWithValue("@oid", Convert.ToInt32(rblOptions.SelectedValue))
                        cmdAnswer.ExecuteNonQuery()
                    End If
                Next

                trans.Commit()
                lblMsg.Text = "Thank you! Your responses have been submitted successfully."
                lblMsg.ForeColor = Drawing.Color.Green
                rptQuestions.Visible = False
                btnSubmit.Visible = False

            Catch ex As Exception
                trans.Rollback()
                lblMsg.Text = "Error submitting responses: " & ex.Message
                lblMsg.ForeColor = Drawing.Color.Red
            End Try
        End Using
    End Sub

    Protected Sub btnBack_Click(sender As Object, e As EventArgs)
        Response.Redirect("ViewSurveys.aspx")
    End Sub
End Class
