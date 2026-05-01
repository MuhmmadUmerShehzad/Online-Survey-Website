Partial Class AdminDashboard
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("UserRole") Is Nothing OrElse Session("UserRole").ToString() <> "Admin" Then
            Response.Redirect("Login.aspx")
        End If

        lblWelcome.Text = "Welcome Admin!"
        lblUserRole.Text = Session("UserRole").ToString()
    End Sub

    Protected Sub btnManageUsers_Click(sender As Object, e As EventArgs) Handles btnManageUsers.Click
        Response.Redirect("ManageUsers.aspx")
    End Sub

    Protected Sub btnManageSurveys_Click(sender As Object, e As EventArgs) Handles btnManageSurveys.Click
        Response.Redirect("ViewSurveys.aspx")
    End Sub

    Protected Sub btnViewAllResponses_Click() Handles btnViewAllResponses.Click
        Response.Redirect("ViewResponses.aspx")
    End Sub

    Protected Sub btnLogout_Click(sender As Object, e As EventArgs)
        Session.Clear()
        Response.Redirect("Login.aspx")
    End Sub
End Class