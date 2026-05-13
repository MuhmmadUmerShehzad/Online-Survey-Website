' ==========================================
' Project: Online Survey System
' Description: Admin Dashboard Page Logic
' Author: University Student
' Date: 2026-05-11
' ==========================================

Partial Class AdminDashboard
    Inherits System.Web.UI.Page

    ' Page Load event triggered when the page is first loaded or refreshed
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        ' Step 1: Security check to ensure only Admins can view this page
        ' Check if Session variable is null or doesn't match the Admin role
        If Session("UserRole") Is Nothing OrElse Session("UserRole").ToString() <> "Admin" Then
            ' If not authorized, kick them back to the login page
            Response.Redirect("Login.aspx")
        End If

        ' Step 2: Display a friendly welcome message for the admin
        lblWelcome.Text = "Welcome Admin!"

    End Sub

    ' Button click event to navigate to User Management page
    Protected Sub btnManageUsers_Click(sender As Object, e As EventArgs) Handles btnManageUsers.Click
        Response.Redirect("ManageUsers.aspx")
    End Sub

    ' Button click event to navigate to Survey Management page
    Protected Sub btnManageSurveys_Click(sender As Object, e As EventArgs) Handles btnManageSurveys.Click
        Response.Redirect("ViewSurveys.aspx")
    End Sub

    ' Button click event to navigate to the page where all survey responses are visible
    Protected Sub btnViewAllResponses_Click() Handles btnViewAllResponses.Click
        Response.Redirect("ViewResponses.aspx")
    End Sub

    ' Logout button logic: Clear the session and return to Login page
    Protected Sub btnLogout_Click(sender As Object, e As EventArgs)
        ' Step 1: Clear all user session data
        Session.Clear()
        ' Step 2: Redirect to login
        Response.Redirect("Login.aspx")
    End Sub
End Class