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
                    parse_expression()
                    ' Handle identifier statements
                    ' Update currentToken to the next token in the input stream
                    hasParsedStatement = True

                Case Token.KEYWORD
                    parse_declaration() ' Parse a declaration statement
                    ' Update currentToken to the next token in the input stream
                    hasParsedStatement = True

                Case Token.IF_KEYWORD
                    parse_if_statement()
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
        ' Start by expecting an identifier
        If currentToken.Type = Token.IDENTIFIER Then
            parse_identifier() ' Parse the identifier

            ' Expect an assignment operator next
            If currentToken.Type = Token.OP Then
                consume(Token.OP) ' Consume the assignment operator

                ' Parse the right-hand side of the assignment
                parse_value_or_expression()
            Else
                AddError("Expected assignment operator '=' after identifier")
            End If
        Else
            AddError("Expected identifier at the beginning of the expression")
        End If

        ' Add parsing result indicating that an expression was parsed
        AddParsingResult("Expression parsed successfully")
    End Sub

    Private Sub parse_value_or_expression()
        ' This method should parse either a simple value (like a number)
        ' or a more complex expression. The implementation will depend on
        ' the complexity of your language's syntax.
        Select Case currentToken.Type
            Case Token.NUMBER
                consume(Token.NUMBER)
                ' Add cases for other types of values or expressions
                ' ...
            Case Else
                AddError("Expected a value or expression after '='")
        End Select
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
            ' Check if there's a semicolon after the expression
            If currentToken.Type = Token.SEMICOLON Then
                consume(Token.SEMICOLON) ' Consume the semicolon
                AddParsingResult("Declaration with assignment parsed for variable: " & variableName) ' Add variable name to the result
            Else
                ' Handle error if semicolon is missing
                AddError("Expected ';' after expression in declaration")
            End If
        Else
            ' If there's no assignment, add the result here
            AddParsingResult("Declaration without assignment parsed for variable: " & variableName)
            ' Check if there's a semicolon after the variable name
            If currentToken.Type = Token.SEMICOLON Then
                consume(Token.SEMICOLON) ' Consume the semicolon
            Else
                ' Handle error if semicolon is missing
                AddError("Expected ';' after variable name in declaration")
            End If
        End If
    End Sub
    Private Sub parse_if_statement()
        consume(Token.IF_KEYWORD) ' Consume the "if" keyword

        ' Check for opening parenthesis after the "if" keyword
        If currentToken.Type = Token.LEFT_PARENTHESES Then
            consume(Token.LEFT_PARENTHESES) ' Consume the opening parenthesis

            ' Parse the condition expression inside the parentheses
            parse_expression()

            ' Check for closing parenthesis after the condition expression
            If currentToken.Type = Token.RIGHT_PARENTHESES Then
                consume(Token.RIGHT_PARENTHESES) ' Consume the closing parenthesis
            Else
                ' Handle error if closing parenthesis is missing
                AddError("Expected ')' after condition expression")
                Exit Sub ' Exit the function if an error occurs
            End If
        Else
            ' Handle error if opening parenthesis is missing
            AddError("Expected '(' after 'if' keyword")
            Exit Sub ' Exit the function if an error occurs
        End If

        ' Check for opening brace '{' after the condition
        If currentToken.Type = Token.LEFT_BRACE Then
            consume(Token.LEFT_BRACE) ' Consume the opening brace

            ' Parse statements inside the if block
            While currentToken.Type <> Token.RIGHT_BRACE And currentToken.Type <> Token.EOF
                parse_statement() ' Parse each statement inside the block
            End While

            ' Check for closing brace '}'
            If currentToken.Type = Token.RIGHT_BRACE Then
                consume(Token.RIGHT_BRACE) ' Consume the closing brace
            Else
                AddError("Expected '}' at the end of if block")
                Exit Sub
            End If
        Else
            AddError("Expected '{' after if condition")
            Exit Sub
        End If
        If currentToken.Type = Token.ELSE_KEYWORD Then
            consume(Token.ELSE_KEYWORD) ' Consume the "else" keyword

            ' Check for opening brace '{' after the "else" keyword
            If currentToken.Type = Token.LEFT_BRACE Then
                consume(Token.LEFT_BRACE) ' Consume the opening brace

                ' Parse statements inside the else block
                While currentToken.Type <> Token.RIGHT_BRACE And currentToken.Type <> Token.EOF
                    parse_statement() ' Parse each statement inside the block
                End While

                ' Check for closing brace '}'
                If currentToken.Type = Token.RIGHT_BRACE Then
                    consume(Token.RIGHT_BRACE) ' Consume the closing brace
                Else
                    AddError("Expected '}' at the end of else block")
                    Exit Sub
                End If
            Else
                AddError("Expected '{' after else keyword")
                Exit Sub
            End If
        End If
        AddParsingResult("If statement with block parsed successfully")

    End Sub




    Private tokens As List(Of Token)
    Private currentPosition As Integer

    Private Function peekNextToken() As Token
        ' Check if there is a next token
        If currentPosition < tokens.Count - 1 Then
            ' Return the next token without advancing the currentPosition
            Return tokens(currentPosition + 1)
        Else
            ' If there are no more tokens, return a special EOF (End Of File) token or similar
            Return New Token(Token.EOF, "")
        End If
    End Function




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