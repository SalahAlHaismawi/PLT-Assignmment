Imports System.Text

Public Class SyntaxLogic
    Private scanner As Scanner
    Private currentToken As Token
    Private parsingResults As List(Of String)
    Private hasSyntaxError As Boolean = False

    Public Sub New(inputText As String)
        scanner = New Scanner(inputText)
        currentToken = scanner.scan()
        parsingResults = New List(Of String)()
    End Sub
    Private Sub consume(expectedType As Integer, Optional expectedValue As String = "")
        If currentToken.Type = expectedType AndAlso (String.IsNullOrEmpty(expectedValue) OrElse currentToken.Value = expectedValue) Then
            Debug.WriteLine($"Consumed Token: Type={currentToken.Type}, Value={currentToken.Value}, LineNumber={currentToken.LineNumber}")
            currentToken = scanner.scan()
        Else
            ' Handle error directly here
            AddError($"Expected {Form1.GetTokenTypeName(expectedType)}")
        End If
    End Sub

    Public Sub parse_program()
        parse_class()
        parse_statement()
        parse_end()

        ' Check if there are no errors
        If Not hasSyntaxError Then
            AddParsingResult("Valid syntax.") ' Inform successful syntax parsing
        Else
            AddParsingResult("Invalid syntax")
        End If
    End Sub



    Private Sub parse_class()
        Debug.WriteLine("Entering parse_class. Current token: " & currentToken.Type & ", Line: " & currentToken.LineNumber)
        If currentToken.Type = Token.START_STATEMENT AndAlso currentToken.Value.Equals("!Class") Then
            consume(Token.START_STATEMENT) ' Consume "!Class"
            ' Check if there's an identifier after "!Class"
            If currentToken.Type = Token.IDENTIFIER Then
                parse_identifier() ' Parse the class name
                AddParsingResult("Class parsed successfully") ' Inform success
                ' Now, let's parse statements inside the class

            Else
                ' Handle error directly here
                AddParsingResult("Error: Expected an identifier after '!Class'")
            End If
        Else
            ' Handle error directly here
            AddParsingResult("Error: Expected '!class'")
        End If
    End Sub

    Private Sub parse_end()
    If currentToken.Type = Token.END_STATEMENT Then
        consume(Token.END_STATEMENT) ' Consume "!End"

        ' Check if there's an identifier after "!End"
        If currentToken.Type = Token.IDENTIFIER Then
            parse_identifier() ' Parse the class name to match the opening
            AddParsingResult("End statement parsed successfully") ' Inform success
        Else
            ' Handle error directly here at the current line number
            AddError("Expected identifier after '!End'")
        End If
    Else
        ' Handle error directly here at the current line number
        AddError("Expected '!End'")
    End If
End Sub


    Private Sub parse_identifier()
        If currentToken.Type = Token.IDENTIFIER Then

            consume(Token.IDENTIFIER)
        Else

        End If
    End Sub
    Private Sub parse_statement()
        Select Case currentToken.Type
            Case Token.IDENTIFIER

            Case Token.IF_KEYWORD

            Case Token.LOG ' Check if it's a log statement
                parse_log_statement()
            Case Else
                HandleSyntaxError(currentToken)
        End Select
    End Sub
    Private Sub parse_log_statement()
        ' Ensure the current token is the 'log' keyword
        If currentToken.Type = Token.LOG Then
            consume(Token.LOG) ' Consume the 'log' keyword

            ' Expect and consume the opening brace
            If currentToken.Type = Token.LEFT_BRACE Then
                consume(Token.LEFT_BRACE)

                ' Now consume the contents of the log message until the closing brace
                ' Assuming that log messages don't have nested braces
                While currentToken.Type <> Token.RIGHT_BRACE And currentToken.Type <> Token.EOF
                    ' Consume all tokens inside the braces; this is a simplification
                    ' Depending on your language, you may need to parse expressions inside the log
                    consume(currentToken.Type)
                End While

                ' Expect and consume the closing brace
                If currentToken.Type = Token.RIGHT_BRACE Then
                    consume(Token.RIGHT_BRACE)

                    ' Optional: Expect and consume the statement terminator (semicolon)
                    If currentToken.Type = Token.SEMICOLON Then
                        consume(Token.SEMICOLON)
                        AddParsingResult("Log statement parsed successfully") ' Inform success
                    Else
                        ' Handle error directly here at the current line number
                        AddError("Expected ';' after log statement")
                    End If
                Else
                    ' Handle error directly here at the current line number
                    AddError("Expected ']' after log statement")
                End If
            Else
                ' Handle error directly here at the current line number
                AddError("Expected '{' after 'log'")
            End If
        Else
            ' Handle error directly here at the current line number
            AddError("Expected 'log'")
        End If
    End Sub






    Private Sub AddParsingResult(result As String)
        Dim lineText As String = scanner.GetLineFromInput(currentToken.LineNumber)
        parsingResults.Add("Line number: " & currentToken.LineNumber & " - " & result)

    End Sub


    Public Function GetParsingResults() As List(Of String)
        Return parsingResults
    End Function

    ' Method to add a result to the parsing results with line information

    Private Sub HandleSyntaxError(token As Token)
        Dim errorMessage As String = "Syntax Error at line " & scanner.tokenLine
        AddParsingResult(errorMessage)
        hasSyntaxError = True

        ' Now, you can increment the line number for the next line


        ' Optional: Skip to the next valid token or recovery point
    End Sub
    Private Sub AddError(message As String)
        Dim errorMsg As String = $"Error at line {scanner.tokenLine}: {message}"
        parsingResults.Add(errorMsg)
        hasSyntaxError = True
    End Sub
End Class