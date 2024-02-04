Imports System.Linq.Expressions

Public Class Form1
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim current As Integer = 0
        ListBox1.Items.Clear()

        If TextBox1.Text.Length > 0 Then
            While TextBox1.Text(current) = “a” And current <
            TextBox1.Text.Length
                current += 1
                If current = TextBox1.Text.Length Then
                    Exit While
                End If
            End While

            If current = TextBox1.Text.Length Then
                ListBox1.Items.Add("Valid")
            Else
                ListBox1.Items.Add("Invalid")
            End If
        Else
            ListBox1.Items.Add("Invalid")
        End If
    End Sub


    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged

    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged

    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

    End Sub
End Class
