
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration

Partial Class TakeSurvey
    Inherits System.Web.UI.Page

    ' Get the connection string from the configuration file
    Dim connectionString As String = WebConfigurationManager.ConnectionStrings("SurveyDBConnection").ConnectionString

    ' Page Load event
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        ' Step 1: Ensure the user is logged in
        If Session("UserID") Is Nothing Then
            Response.Redirect("Login.aspx")
            Exit Sub
        End If

        ' Step 2: Initialize the page on first visit
        If Not IsPostBack Then
            ' Set the username on the page
            lblUserName.Text = Session("UserName").ToString()

            ' Check if a SurveyID is passed in the session
            If Session("SurveyID") IsNot Nothing Then
                ' Call procedure to load the survey details
                LoadSurvey(Convert.ToInt32(Session("SurveyID")))
            Else
                ' If no survey selected, go back to survey list
                Response.Redirect("ViewSurveys.aspx")
            End If
        End If
    End Sub

    ' Procedure to fetch survey title and questions from the database
    Private Sub LoadSurvey(surveyID As Integer)
        Using con As New SqlConnection(connectionString)
            Try
                ' 1. Load the Survey Title
                Dim cmdSurvey As New SqlCommand("SELECT Title FROM Surveys WHERE SurveyID = @sid", con)
                cmdSurvey.Parameters.AddWithValue("@sid", surveyID)
                con.Open()
                Dim title As Object = cmdSurvey.ExecuteScalar()
                
                If title IsNot Nothing Then
                    lblSurveyTitle.Text = title.ToString()
                Else
                    lblMsg.Text = "Survey not found in database."
                    lblMsg.ForeColor = Drawing.Color.Red
                    btnSubmit.Visible = False
                    Return
                End If

                ' 2. Load the Questions for this survey
                Dim cmdQuestions As New SqlCommand("SELECT QuestionID, QuestionText FROM Questions WHERE SurveyID = @sid", con)
                cmdQuestions.Parameters.AddWithValue("@sid", surveyID)
                Dim da As New SqlDataAdapter(cmdQuestions)
                Dim dt As New DataTable()
                da.Fill(dt)

                ' Bind questions to the Repeater control
                rptQuestions.DataSource = dt
                rptQuestions.DataBind()

                ' Check if there are no questions
                If dt.Rows.Count = 0 Then
                    lblMsg.Text = "This survey currently has no questions."
                    lblMsg.ForeColor = Drawing.Color.Red
                    btnSubmit.Visible = False
                End If

            Catch ex As Exception
                ' Handle any database errors
                lblMsg.Text = "Error loading survey: " & ex.Message
                lblMsg.ForeColor = Drawing.Color.Red
            End Try
        End Using
    End Sub

    ' Event triggered for each question to load its multiple choice options
    Protected Sub rptQuestions_ItemDataBound(sender As Object, e As RepeaterItemEventArgs)
        ' Check if the item is a data row (not header/footer)
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            ' Find the controls inside the repeater item
            Dim hfQuestionID As HiddenField = CType(e.Item.FindControl("hfQuestionID"), HiddenField)
            Dim rblOptions As RadioButtonList = CType(e.Item.FindControl("rblOptions"), RadioButtonList)
            Dim questionID As Integer = Convert.ToInt32(hfQuestionID.Value)

            ' Query options for this specific question
            Using con As New SqlConnection(connectionString)
                Dim cmdOptions As New SqlCommand("SELECT OptionID, OptionText FROM Options WHERE QuestionID = @qid", con)
                cmdOptions.Parameters.AddWithValue("@qid", questionID)
                con.Open()
                Dim da As New SqlDataAdapter(cmdOptions)
                Dim dt As New DataTable()
                da.Fill(dt)

                ' Bind options to the RadioButtonList
                rblOptions.DataSource = dt
                rblOptions.DataBind()
            End Using
        End If
    End Sub

    ' Event to handle the final submission of the survey
    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs)
        ' Check if all required fields (validators) are met
        If Not Page.IsValid Then Return

        Dim surveyID As Integer = Convert.ToInt32(Session("SurveyID"))
        Dim userID As Integer = Convert.ToInt32(Session("UserID"))

        Using con As New SqlConnection(connectionString)
            con.Open()
            ' Start a Transaction to ensure all answers are saved or none at all
            Dim trans As SqlTransaction = con.BeginTransaction()

            Try
                ' 1. Insert a new record into the Responses table
                Dim cmdResponse As New SqlCommand("INSERT INTO Responses (SurveyID, UserID, ResponseDate) OUTPUT INSERTED.ResponseID VALUES (@sid, @uid, GETDATE())", con, trans)
                cmdResponse.Parameters.AddWithValue("@sid", surveyID)
                cmdResponse.Parameters.AddWithValue("@uid", userID)
                
                ' Get the newly created ResponseID
                Dim responseID As Integer = Convert.ToInt32(cmdResponse.ExecuteScalar())

                ' 2. Loop through each question in the Repeater and save the selected answer
                For Each item As RepeaterItem In rptQuestions.Items
                    Dim hfQuestionID As HiddenField = CType(item.FindControl("hfQuestionID"), HiddenField)
                    Dim rblOptions As RadioButtonList = CType(item.FindControl("rblOptions"), RadioButtonList)

                    ' If user selected an option
                    If rblOptions.SelectedIndex <> -1 Then
                        Dim cmdAnswer As New SqlCommand("INSERT INTO Answers (ResponseID, QuestionID, SelectedOptionID) VALUES (@rid, @qid, @oid)", con, trans)
                        cmdAnswer.Parameters.AddWithValue("@rid", responseID)
                        cmdAnswer.Parameters.AddWithValue("@qid", Convert.ToInt32(hfQuestionID.Value))
                        cmdAnswer.Parameters.AddWithValue("@oid", Convert.ToInt32(rblOptions.SelectedValue))
                        cmdAnswer.ExecuteNonQuery()
                    End If
                Next

                ' Commit the transaction if everything succeeded
                trans.Commit()
                
                ' Success message and UI reset
                lblMsg.Text = "Thank you! Your responses have been submitted successfully."
                lblMsg.ForeColor = Drawing.Color.Green
                rptQuestions.Visible = False
                btnSubmit.Visible = False

            Catch ex As Exception
                ' Rollback changes if an error occurred
                trans.Rollback()
                lblMsg.Text = "Database Error during submission: " & ex.Message
                lblMsg.ForeColor = Drawing.Color.Red
            End Try
        End Using
    End Sub

    ' Back button logic
    Protected Sub btnBack_Click(sender As Object, e As EventArgs)
        Response.Redirect("ViewSurveys.aspx")
    End Sub
End Class
