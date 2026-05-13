' ==========================================
' Project: Online Survey System
' Description: Builder Dashboard Page Logic
' Author: University Student
' Date: 2026-05-11
' ==========================================

Partial Class BuilderDashboard
    Inherits System.Web.UI.Page

    ' Page Load event logic
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        ' Check if the user is logged in as a 'Builder'
        If Session("UserRole") Is Nothing OrElse Session("UserRole").ToString() <> "Builder" Then
            ' If not authorized, send them back to login page
            Response.Redirect("Login.aspx")
        End If

        ' Display a welcome message to the builder
        lblWelcome.Text = "Welcome Survey Builder!"

    End Sub

    ' Redirect to the Create Survey page
    Protected Sub btn_Create_Summary() Handles btnCreateSurvey.Click
        Response.Redirect("CreateSurvey.aspx")
    End Sub

    ' Redirect to the View Surveys page to see created surveys
    Protected Sub Btn_View_Survey() Handles btnViewSurveys.Click
        Response.Redirect("ViewSurveys.aspx")
    End Sub

    ' Redirect to the View Responses page to see results
    Protected Sub btnViewResponses_Click() Handles btnViewResponses.Click
        Response.Redirect("ViewResponses.aspx")
    End Sub

    ' Logout: Clear session and redirect to Login
    Protected Sub btnLogout_Click(sender As Object, e As EventArgs)
        Session.Clear()
        Response.Redirect("Login.aspx")
    End Sub
End Class