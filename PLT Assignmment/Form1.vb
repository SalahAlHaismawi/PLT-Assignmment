Imports System.Linq.Expressions
Imports System.Security.Principal
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

            Dim tokenTypeName As String = GetTokenTypeName(token.Type)

            If IsValidToken(token.Type) Then
                ListBox1.Items.Add($"Valid Token: Type={tokenTypeName}, Spelling={token.Value}")
            Else
                ListBox1.Items.Add($"Invalid Token: Type={tokenTypeName}, Spelling={token.Value}")
            End If
        Loop
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim inputText As String = TextBox1.Text
        Dim syntaxLogic As New SyntaxLogic(inputText)

        ListBox2.Items.Clear()

        syntaxLogic.parse_program()

        ' Retrieve and display the parsing results
        '
        Dim results As List(Of String) = syntaxLogic.GetParsingResults()
        For Each result As String In results
            ListBox2.Items.Add(result)
        Next

        If results.Count = 0 Then
            ListBox2.Items.Add("No parsing results to display.")
        End If

    End Sub




    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged

    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        ' Clear the ListBox

    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click

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
        Dim validTypes() As Integer = {Token.IDENTIFIER, Token.LEFT_BRACE, Token.RIGHT_BRACE, Token.SEMICOLON, Token.START_STATEMENT, Token.END_STATEMENT, Token.NUMBER, Token.IF_KEYWORD, Token.ELSE_KEYWORD, Token.OP, Token.SEPARATOR, Token.LOG, Token.KEYWORD, Token.LEFT_PARENTHESES, Token.RIGHT_PARENTHESES}

        ' Check if the token type is in the list of valid types
        Return validTypes.Contains(tokenType)
    End Function
    Public Function GetTokenTypeName(tokenType As Integer) As String
        Select Case tokenType
            Case Token.EOF
                Return "EOF"
            Case Token.IDENTIFIER
                Return "IDENTIFIER"
            Case Token.LEFT_BRACE
                Return "LEFT_BRACE"
            Case Token.RIGHT_BRACE
                Return "RIGHT_BRACE"
            Case Token.SEMICOLON
                Return "SEMICOLON"
            Case Token.START_STATEMENT
                Return "START_STATEMENT"
            Case Token.END_STATEMENT
                Return "END_STATEMENT"
            Case Token.NUMBER
                Return "NUMBER"
            Case Token.IF_KEYWORD
                Return "IF_KEYWORD"
            Case Token.ELSE_KEYWORD
                Return "ELSE_KEYWORD"
            Case Token.START_KEYWORD
                Return "START_KEYWORD"
            Case Token.END_KEYWORD
                Return "END_KEYWORD"
            Case Token.SEPARATOR
                Return "SEPARATOR"
            Case Token.OP
                Return "OP"
            Case Token.LOG
                Return "LOG"
            Case Token.KEYWORD
                Return "KEYWORD"
            Case Token.UNKNOWN
                Return "UNKNOWN"
            Case Token.LEFT_PARENTHESES
                Return "LEFT_PARENTHESES"
            Case Token.RIGHT_PARENTHESES
                Return "RIGHT_PARENTHESES"
            Case Else
                Return "INVALID_TYPE"
        End Select
    End Function


    Private Sub Label3_Click(sender As Object, e As EventArgs) Handles Label3.Click

    End Sub
End Class
