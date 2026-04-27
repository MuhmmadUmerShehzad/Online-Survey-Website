Partial Class BuilderDashboard
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs)
        If Session("UserRole") Is Nothing OrElse Session("UserRole").ToString() <> "Builder" Then
            Response.Redirect("Login.aspx")
        End If

        lblWelcome.Text = "Welcome Survey Builder!"
    End Sub

    Protected Sub btn_Create_Summary() Handles btnCreateSurvey.Click
        Response.Redirect("CreateSurvey.aspx")
    End Sub

    Protected Sub btnLogout_Click(sender As Object, e As EventArgs)
        Session.Clear()
        Response.Redirect("Login.aspx")
    End Sub
End Class