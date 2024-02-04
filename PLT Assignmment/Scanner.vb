
Public Class Scanner
    Private currentChar As Char
    Private currentSpelling As String
    Private currentIndex As Integer = 0
    Private currentKind As Integer
    Private inputText As String ' Added inputText field

    Public Sub New(inputText As String)
        Me.inputText = inputText
        currentChar = If(inputText.Length > 0, inputText(0), Chr(0))
    End Sub

    Public Function scan() As Token
        While Char.IsWhiteSpace(currentChar)
            takeIt()
        End While

        If currentIndex >= inputText.Length Then
            Return New Token(Token.EOF, "")
        End If

        currentSpelling = ""
        currentKind = scanToken()
        Debug.WriteLine($"Scanned Token: Type={currentKind}, Spelling={currentSpelling}")

        Return New Token(currentKind, currentSpelling)
    End Function

    Public Function scanToken() As Integer
        Dim state As Integer = 1

        If currentIndex >= inputText.Length Then
            Return Token.EOF
        End If

        If Char.IsLetter(currentChar) Then
            takeIt()
            While Char.IsLetterOrDigit(currentChar)
                takeIt()
            End While
            Dim spellingUpper As String = currentSpelling.ToUpper()
            If spellingUpper = "START" Then
                Return Token.START_STATEMENT
            ElseIf spellingUpper = "END" Then
                Return Token.END_STATEMENT
            Else
                Return Token.IDENTIFIER
            End If
        ElseIf Char.IsDigit(currentChar) Then
            takeIt()
            While Char.IsDigit(currentChar)
                takeIt()
            End While
            Return Token.NUMBER
        ElseIf currentChar = "+" OrElse currentChar = "-" OrElse currentChar = "*" OrElse currentChar = "/" OrElse currentChar = "=" Then
            takeIt()
            Return Token.OP
        ElseIf currentChar = "{" Then
            takeIt()
            Return Token.LEFT_BRACE
        ElseIf currentChar = "}" Then
            takeIt()
            Return Token.RIGHT_BRACE
        ElseIf currentChar = ";" Then
            takeIt()
            Return Token.SEMICOLON
        ElseIf currentChar = "!" Then
            takeIt()
            If currentChar = "S" Then
                takeIt()
                If currentChar = "t" Then
                    takeIt()
                    If currentChar = "a" Then
                        takeIt()
                        If currentChar = "r" Then
                            takeIt()
                            If currentChar = "t" Then
                                takeIt()
                                Return Token.START_STATEMENT
                            End If
                        End If
                    End If
                End If
            ElseIf currentChar = "E" Then
                takeIt()
                If currentChar = "n" Then
                    takeIt()
                    If currentChar = "d" Then
                        takeIt()
                        Return Token.END_STATEMENT
                    End If
                End If
            End If
            Return Token.UNKNOWN
        Else
            takeIt()
            Return Token.UNKNOWN
        End If
        Debug.WriteLine($"Scanning: currentChar={currentChar}, currentSpelling={currentSpelling}")

    End Function

    Private Sub takeIt()
        currentSpelling &= currentChar
        currentIndex += 1
        If currentIndex < inputText.Length Then
            currentChar = inputText(currentIndex)
        Else
            currentChar = Chr(0) ' Null character to indicate the end of input
        End If
    End Sub
End Class
