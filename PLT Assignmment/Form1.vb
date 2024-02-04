Imports System.Linq.Expressions
Imports System.Text.RegularExpressions

Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.MaximizeBox = False
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim inputText As String = TextBox1.Text
        Dim scanner As New Scanner(inputText)

        ListBox1.Items.Clear()

        Do
            Dim token As Token = scanner.scan()
            If token.Type = Token.EOF Then
                Exit Do
            End If

            If IsValidToken(token.Type) Then
                ListBox1.Items.Add($"Valid Token: {token.Value}")
            Else
                ListBox1.Items.Add($"Invalid Token: {token.Value}")
            End If
        Loop
    End Sub


    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged

    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        ' Clear the ListBox

    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub PictureBox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click

    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click

    End Sub

    Private Sub Label2_Click(sender As Object, e As EventArgs) Handles Label2.Click

    End Sub
    Private Function IsValidToken(tokenType As Integer) As Boolean
        ' Define a list of valid token types
        Dim validTypes() As Integer = {Token.IDENTIFIER, Token.LEFT_BRACE, Token.RIGHT_BRACE, Token.SEMICOLON, Token.START_STATEMENT, Token.END_STATEMENT, Token.NUMBER, Token.IF_KEYWORD, Token.ELSE_KEYWORD, Token.OP}

        ' Check if the token type is in the list of valid types
        Return validTypes.Contains(tokenType)
    End Function

    Private Sub Label3_Click(sender As Object, e As EventArgs) Handles Label3.Click

    End Sub
End Class
