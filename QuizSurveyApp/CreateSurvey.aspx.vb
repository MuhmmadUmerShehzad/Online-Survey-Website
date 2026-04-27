Imports System.Data
Imports System.Data.SqlClient

Partial Class CreateSurvey
    Inherits System.Web.UI.Page

    Dim con As New SqlConnection("Data Source=DESKTOP-6IIQ3JH\SQLEXPRESS;Initial Catalog=SurveyDB;Integrated Security=True;TrustServerCertificate=True")

    Private Sub CreateTable()
        Dim dt As New DataTable()
        dt.Columns.Add("Question")
        dt.Columns.Add("Type")
        dt.Columns.Add("Opt1")
        dt.Columns.Add("Opt2")
        dt.Columns.Add("Opt3")
        dt.Columns.Add("Opt4")
        ViewState("Questions") = dt
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs)
        If Session("UserRole") Is Nothing OrElse Session("UserRole").ToString() <> "Builder" Then
            Response.Redirect("Login.aspx")
        End If

        If Not IsPostBack Then
            CreateTable()
        End If
    End Sub

    Protected Sub btnAdd_Click(sender As Object, e As EventArgs)
        Dim dt As DataTable = CType(ViewState("Questions"), DataTable)

        Dim row As DataRow = dt.NewRow()
        row("Question") = txtQuestion.Text
        row("Type") = ddlType.SelectedValue
        row("Opt1") = txtOption1.Text
        row("Opt2") = txtOption2.Text
        row("Opt3") = txtOption3.Text
        row("Opt4") = txtOption4.Text

        dt.Rows.Add(row)

        ViewState("Questions") = dt
        gvQuestions.DataSource = dt
        gvQuestions.DataBind()

        txtQuestion.Text = ""
        txtOption1.Text = ""
        txtOption2.Text = ""
        txtOption3.Text = ""
        txtOption4.Text = ""
    End Sub

    Protected Sub btnSaveSurvey_Click(sender As Object, e As EventArgs)
        Dim dt As DataTable = CType(ViewState("Questions"), DataTable)

        con.Open()

        ' Insert Survey
        Dim cmdSurvey As New SqlCommand("INSERT INTO Surveys (Title, CreatedBy) OUTPUT INSERTED.SurveyID VALUES (@title, @user)", con)
        cmdSurvey.Parameters.AddWithValue("@title", txtSurveyTitle.Text)
        cmdSurvey.Parameters.AddWithValue("@user", Session("UserID"))

        Dim surveyID As Integer = Convert.ToInt32(cmdSurvey.ExecuteScalar())

        ' Insert Questions & Options
        For Each r As DataRow In dt.Rows

            Dim cmdQ As New SqlCommand("INSERT INTO Questions (SurveyID, QuestionText, QuestionType) OUTPUT INSERTED.QuestionID VALUES (@sid,@q,@type)", con)
            cmdQ.Parameters.AddWithValue("@sid", surveyID)
            cmdQ.Parameters.AddWithValue("@q", r("Question").ToString())
            cmdQ.Parameters.AddWithValue("@type", r("Type").ToString())

            Dim questionID As Integer = Convert.ToInt32(cmdQ.ExecuteScalar())

            If r("Type").ToString() = "MCQ" Then
                Dim options() As String = {r("Opt1"), r("Opt2"), r("Opt3"), r("Opt4")}

                For Each opt As String In options
                    If opt <> "" Then
                        Dim cmdOpt As New SqlCommand("INSERT INTO Options (QuestionID, OptionText) VALUES (@qid,@opt)", con)
                        cmdOpt.Parameters.AddWithValue("@qid", questionID)
                        cmdOpt.Parameters.AddWithValue("@opt", opt)
                        cmdOpt.ExecuteNonQuery()
                    End If
                Next
            Else
                ' True/False default options
                Dim cmdT As New SqlCommand("INSERT INTO Options (QuestionID, OptionText) VALUES (@qid,'True'),(@qid,'False')", con)
                cmdT.Parameters.AddWithValue("@qid", questionID)
                cmdT.ExecuteNonQuery()
            End If

        Next

        con.Close()

        lblMsg.Text = "Survey Saved Successfully!"
        gvQuestions.DataSource = Nothing
        gvQuestions.DataBind()
        CreateTable()
        txtSurveyTitle.Text = ""
    End Sub

End Class