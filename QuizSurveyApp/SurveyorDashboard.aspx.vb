' ==========================================
' Project: Online Survey System
' Description: Surveyor Dashboard Page Logic
' Author: University Student
' Date: 2026-05-11
' ==========================================

Partial Class SurveyorDashboard
    Inherits System.Web.UI.Page

    ' Page Load event logic
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        ' Check if user is logged in as 'Surveyor'
        If Session("UserRole") Is Nothing OrElse Session("UserRole").ToString() <> "Surveyor" Then
            ' If not, redirect to Login
            Response.Redirect("Login.aspx")
        End If

        ' Display welcome message
        lblWelcome.Text = "Welcome Surveyor!"

    End Sub

    ' Button click to browse and take surveys
    Protected Sub btnTakeSurvey_Click(sender As Object, e As EventArgs) Handles btnTakeSurvey.Click
        Response.Redirect("ViewSurveys.aspx")
    End Sub

    ' Button click to view my previous responses
    Protected Sub btnMyResponses_Click() Handles btnMyResponses.Click
        Response.Redirect("ViewResponses.aspx")
    End Sub

    ' Logout logic: Clear session and redirect
    Protected Sub btnLogout_Click(sender As Object, e As EventArgs)
        Session.Clear()
        Response.Redirect("Login.aspx")
    End Sub
End Class