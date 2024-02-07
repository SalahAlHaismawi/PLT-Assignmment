
Public Class Scanner
    Private currentChar As Char
    Private currentSpelling As String
    Private currentIndex As Integer = 0
    Private currentKind As Integer
    Private inputText As String ' Added inputText field
    Private lineNumber As Integer = 1 ' Line number tracking
    Private previousChar As Char = Chr(0) ' Initialize to null character
    Private lineNumberBeforeError As Integer ' Rename tokenLine to lineNumberBeforeError
    Public tokenLine As Integer = 0


    Public Sub New(inputText As String)
        Me.inputText = inputText
        currentChar = If(inputText.Length > 0, inputText(0), Chr(0))
    End Sub

    Public Function scan() As Token
        tokenLine = lineNumber ' Get the current line number before scanning
        Debug.WriteLine("index before checking if correct" & lineNumber)
        While Char.IsWhiteSpace(currentChar)
            takeIt()
        End While

        If currentIndex >= inputText.Length Then
            Return New Token(Token.EOF, "", tokenLine)
        End If

        currentSpelling = ""
        currentKind = scanToken()

        ' Check for errors and handle them
        If currentKind = Token.UNKNOWN Then
            ' Handle the error and move to the next character

            HandleError("Unknown token")
            takeIt() ' Skip the invalid character
            ' After handling the error, call scan again to get the next token
            Return scan()
        End If

        Debug.WriteLine($"Scanned Token: Type={currentKind}, Spelling={currentSpelling}")

        ' Include the line number when returning a new Token
        Return New Token(currentKind, currentSpelling, tokenLine)
    End Function

    Private Sub HandleError(message As String)
        ' Handle the error here, you can log the error message or take appropriate actions
        Debug.WriteLine($"Error at line {lineNumber}: {message}")
        ' You can implement error recovery logic here if needed
    End Sub

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

            ' Check for specific keywords and return corresponding tokens
            Select Case spellingUpper
                Case "CLASS"
                    Return Token.START_STATEMENT
                Case "END"
                    Return Token.END_STATEMENT
                Case "LOG"
                    Return Token.LOG  ' Handling for the 'log' keyword
                Case "INT" ' Add support for the "int" keyword
                    Return Token.KEYWORD
                Case Else
                    Return Token.IDENTIFIER
            End Select
        ElseIf Char.IsDigit(currentChar) Then
            Return handleNumber()
        ElseIf "+-*/=<>()[]".Contains(currentChar) Then
            Return handleOperator()
        ElseIf currentChar = "!" Then
            Return handleExclamation()
        ElseIf currentChar = ";" Then
            takeIt()
            Return Token.SEMICOLON
        ElseIf currentChar = "," Then
            takeIt()
            Return Token.SEPARATOR
        ElseIf currentChar = "{" Then
            takeIt()
            Return Token.LEFT_BRACE
        ElseIf currentChar = "}" Then
            takeIt()
            Return Token.RIGHT_BRACE
        Else
            ' Handle unknown characters
            takeIt()
            Return Token.UNKNOWN
        End If
    End Function

    Private Function handleNumber() As Integer
        takeIt()
        While Char.IsDigit(currentChar)
            takeIt()
        End While
        Return Token.NUMBER
    End Function

    Private Function handleOperator() As Integer
        takeIt()
        ' Handle cases like <= or >=
        If (currentChar = "=" AndAlso "+-<>=!".Contains(previousChar)) Then
            takeIt()
        End If
        Return Token.OP
    End Function

    Private Function handleExclamation() As Integer
        takeIt()
        If Char.IsLetter(currentChar) Then
            While Char.IsLetter(currentChar)
                takeIt()
            End While
            If currentSpelling.ToUpper() = "!CLASS" Then
                Return Token.START_STATEMENT
            ElseIf currentSpelling.ToUpper() = "!END" Then
                Return Token.END_STATEMENT
            Else
                Return Token.UNKNOWN
            End If
        Else
            Return Token.UNKNOWN
        End If
    End Function

    Private Sub takeIt()
        currentSpelling &= currentChar

        ' Check if the current character is a newline character
        If currentChar = vbCr OrElse currentChar = vbLf Then
            ' Increment line number if a newline is encountered
            If Not (currentChar = vbLf AndAlso previousChar = vbCr) Then
                lineNumber += 1
            End If
        End If

        ' Update previousChar for newline check
        previousChar = currentChar

        ' Advance to the next character
        currentIndex += 1
        If currentIndex < inputText.Length Then
            currentChar = inputText(currentIndex)
        Else
            currentChar = Chr(0) ' Null character to indicate the end of input
        End If
    End Sub
    Public Function GetLineFromInput(index As Integer) As String
        ' Split the input text into lines
        Dim lines As String() = inputText.Split(New String() {vbCr & vbLf, vbLf}, StringSplitOptions.None)

        ' Calculate the line number based on the token's index
        Dim lineNumber As Integer = 1
        For i As Integer = 0 To index
            If i >= inputText.Length Then
                Exit For
            End If

            If inputText(i) = vbCr OrElse inputText(i) = vbLf Then
                lineNumber += 1
            End If
        Next

        ' Check if the line number is within the range of available lines
        If lineNumber > 0 AndAlso lineNumber <= lines.Length Then
            Return lines(lineNumber - 1) ' Array is 0-indexed, so subtract 1
        Else
            Return "Line number out of range"
        End If
    End Function
End Class
