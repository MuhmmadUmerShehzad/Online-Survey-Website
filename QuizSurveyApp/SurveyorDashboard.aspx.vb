Partial Class SurveyorDashboard
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("UserRole") Is Nothing OrElse Session("UserRole").ToString() <> "Surveyor" Then
            Response.Redirect("Login.aspx")
        End If

        lblWelcome.Text = "Welcome Surveyor!"
        lblUserRole.Text = Session("UserRole").ToString()
    End Sub

    Protected Sub btnTakeSurvey_Click(sender As Object, e As EventArgs) Handles btnTakeSurvey.Click
        Response.Redirect("ViewSurveys.aspx")
    End Sub

    Protected Sub btnMyResponses_Click() Handles btnMyResponses.Click
        Response.Redirect("ViewResponses.aspx")
    End Sub

    Protected Sub btnLogout_Click(sender As Object, e As EventArgs)
        Session.Clear()
        Response.Redirect("Login.aspx")
    End Sub
End Class