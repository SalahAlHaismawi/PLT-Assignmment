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

        While currentToken.Type <> Token.EOF AndAlso Not hasSyntaxError
            ' Check if the current token represents the end statement
            If currentToken.Type = Token.END_STATEMENT Then
                Exit While
            End If

            ' Check if the current token represents a statement
            Select Case currentToken.Type
                Case Token.IDENTIFIER, Token.KEYWORD, Token.IF_KEYWORD, Token.LOG
                    parse_statement()
                Case Else
                    ' Handle syntax error or unexpected token
                    Debug.WriteLine("no statements yet")

                    ' Move to the next token
                    consume(currentToken.Type)
            End Select
        End While

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
        Debug.WriteLine("executed parse_statement")

        ' Parse statements until the end of input or until an error occurs
        Dim hasParsedStatement As Boolean = False

        While currentToken.Type <> Token.EOF AndAlso Not hasSyntaxError
            ' Reset hasParsedStatement for each iteration of the loop
            hasParsedStatement = False

            Select Case currentToken.Type
                Case Token.IDENTIFIER
                    ' Handle identifier statements
                    ' Update currentToken to the next token in the input stream
                    hasParsedStatement = True

                Case Token.KEYWORD
                    parse_declaration() ' Parse a declaration statement
                    ' Update currentToken to the next token in the input stream
                    hasParsedStatement = True

                Case Token.IF_KEYWORD
                    ' Handle if statements
                    ' Update currentToken to the next token in the input stream
                    hasParsedStatement = True

                Case Token.LOG
                    parse_log_statement() ' Parse a log statement
                    ' Update currentToken to the next token in the input stream
                    hasParsedStatement = True



            End Select

            ' Exit the loop if no statements have been parsed during this iteration
            If Not hasParsedStatement Then
                Exit While
            End If
        End While
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
    Private Sub parse_expression()
        ' Here, we're only handling a simple numeric expression
        If currentToken.Type = Token.NUMBER Then
            consume(Token.NUMBER)
        Else

        End If
    End Sub


    Private Sub parse_type()
        If currentToken.Value.ToLower() = "int" AndAlso currentToken.Type = Token.IDENTIFIER Then
            consume(Token.IDENTIFIER) ' Consume the type "int"
        Else
            HandleSyntaxError(currentToken)
        End If
    End Sub

    Private Sub parse_declaration()
        ' Check if the current token represents the expected type
        Debug.WriteLine(currentToken.Value)
        If currentToken.Type = Token.KEYWORD Then
            consume(Token.KEYWORD) ' Consume the "int" keyword
            Debug.WriteLine("went inside declaration if statement")
        Else
            ' Handle error if the type is not "int"
            AddError("Expected 'int' type keyword")
            Exit Sub ' Exit the function if an error occurs
        End If

        Dim variableName As String = currentToken.Value ' Store the variable name
        parse_identifier() ' Parse the variable name

        ' Check if there's an assignment operator "="
        If currentToken.Type = Token.OP AndAlso currentToken.Value = "=" Then
            consume(Token.OP) ' Consume the "="
            parse_expression() ' Parse the expression after "="
            AddParsingResult("Declaration with assignment parsed for variable: " & variableName) ' Add variable name to the result
        Else
            ' If there's no assignment, add the result here
            AddParsingResult("Declaration without assignment parsed for variable: " & variableName)
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