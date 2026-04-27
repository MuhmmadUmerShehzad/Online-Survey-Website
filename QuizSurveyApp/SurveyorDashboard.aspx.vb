Partial Class SurveyorDashboard
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs)
        If Session("UserRole") Is Nothing OrElse Session("UserRole").ToString() <> "Surveyor" Then
            Response.Redirect("Login.aspx")
        End If

        lblWelcome.Text = "Welcome Surveyor!"
    End Sub

    Protected Sub btnLogout_Click(sender As Object, e As EventArgs)
        Session.Clear()
        Response.Redirect("Login.aspx")
    End Sub
End Class