' ==========================================
' Project: Online Survey System
' Description: Survey Creation Page Logic
' Author: University Student
' Date: 2026-05-11
' ==========================================

Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration

Partial Class CreateSurvey
    Inherits System.Web.UI.Page

    Dim con As New SqlConnection(WebConfigurationManager.ConnectionStrings("SurveyDBConnection").ConnectionString)


    Protected Function GetTable() As DataTable

        If ViewState("Questions") Is Nothing Then
            Dim dt As New DataTable()
            dt.Columns.Add("Question")
            dt.Columns.Add("Type")
            dt.Columns.Add("Opt1")
            dt.Columns.Add("Opt2")
            dt.Columns.Add("Opt3")
            dt.Columns.Add("Opt4")

            ViewState("Questions") = dt

        End If

        Return CType(ViewState("Questions"), DataTable)

    End Function

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("UserRole") Is Nothing OrElse Session("UserRole").ToString() <> "Builder" Then
            Response.Redirect("Login.aspx")
        End If



        If Not IsPostBack Then
            GetTable()
        End If
    End Sub

    Protected Sub ddlType_SelectedIndexChanged(sender As Object, e As EventArgs)
        If ddlType.SelectedValue = "TrueFalse" Then
            pnlExtraOptions.Visible = False
            txtOption1.Text = "True"
            txtOption2.Text = "False"
            txtOption1.ReadOnly = True
            txtOption2.ReadOnly = True
        Else
            pnlExtraOptions.Visible = True
            txtOption1.Text = ""
            txtOption2.Text = ""
            txtOption1.ReadOnly = False
            txtOption2.ReadOnly = False
        End If
    End Sub

    Protected Sub btnAdd_Click(sender As Object, e As EventArgs)

        Dim dt As DataTable = GetTable()

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

        ' Clear fields
        txtQuestion.Text = ""
        txtOption1.Text = ""
        txtOption2.Text = ""
        txtOption3.Text = ""
        txtOption4.Text = ""
        ddlType.SelectedIndex = 0
        pnlExtraOptions.Visible = True
        txtOption1.ReadOnly = False
        txtOption2.ReadOnly = False

    End Sub

    Protected Sub btnSaveSurvey_Click(sender As Object, e As EventArgs)
        Dim dt As DataTable = GetTable()

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
        ViewState("Questions") = Nothing
        GetTable()
        txtSurveyTitle.Text = ""
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
