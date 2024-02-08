Imports System.Text

Public Class SyntaxLogic
    Private scanner As Scanner
    Private currentToken As Token
    Private parsingResults As List(Of String)
    Private hasSyntaxError As Boolean = False
    Private className As String

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
            ' Check if the current token end 
            If currentToken.Type = Token.END_STATEMENT Then
                Exit While
            End If


            Select Case currentToken.Type
                Case Token.IDENTIFIER, Token.KEYWORD, Token.IF_KEYWORD, Token.LOG
                    parse_statement()
                Case Else

                    Debug.WriteLine("no statements yet")

                    ' Move to the next token
                    consume(currentToken.Type)
            End Select
        End While

        parse_end()


        If Not hasSyntaxError Then
            AddParsingResult("Valid syntax.") '
        Else
            AddParsingResult("Invalid syntax")
        End If
    End Sub




    Private Sub parse_class()
        Debug.WriteLine("Entering parse_class. Current token: " & currentToken.Type & ", Line: " & currentToken.LineNumber)
        If currentToken.Type = Token.START_STATEMENT AndAlso currentToken.Value.Equals("!Class") Then
            consume(Token.START_STATEMENT)
            If currentToken.Type = Token.IDENTIFIER Then
                className = currentToken.Value ' Save the class name
                parse_identifier() ' Parse the class name
                AddParsingResult("Class '" & className & "' parsed successfully")
            Else

                AddParsingResult("Error: Expected an identifier after '!Class'")
            End If
        Else

            AddParsingResult("Error: Expected '!class'")
        End If
    End Sub

    Private Sub parse_end()
        If currentToken.Type = Token.END_STATEMENT Then
            consume(Token.END_STATEMENT)

            ' Check if there's an identifier after "!End"
            If currentToken.Type = Token.IDENTIFIER Then
                Dim endClassName As String = currentToken.Value
                If endClassName.Equals(className) Then
                    parse_identifier()
                    AddParsingResult("End statement for class '" & endClassName & "' parsed successfully")
                Else
                    ' Class names dont match
                    AddError("Class name mismatch: '" & className & "' at the start and '" & endClassName & "' at the end")
                End If
            Else

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

        ' Parse statements until the end of input or until an error
        Dim hasParsedStatement As Boolean = False

        While currentToken.Type <> Token.EOF AndAlso Not hasSyntaxError

            hasParsedStatement = False

            Select Case currentToken.Type
                Case Token.IDENTIFIER
                    parse_expression()

                    hasParsedStatement = True

                Case Token.KEYWORD
                    parse_declaration()
                    hasParsedStatement = True

                Case Token.IF_KEYWORD
                    parse_if_statement()

                    hasParsedStatement = True

                Case Token.LOG
                    parse_log_statement()
                    hasParsedStatement = True
                Case Token.WHILE_KEYWORD
                    parse_while_statement()
                    hasParsedStatement = True



            End Select

            If Not hasParsedStatement Then
                Exit While
            End If
        End While
    End Sub
    Private Sub parse_log_statement()

        If currentToken.Type = Token.LOG Then
            consume(Token.LOG)


            If currentToken.Type = Token.LEFT_BRACE Then
                consume(Token.LEFT_BRACE)


                While currentToken.Type <> Token.RIGHT_BRACE And currentToken.Type <> Token.EOF

                    consume(currentToken.Type)
                End While

                ' Expect and consume the closing brace
                If currentToken.Type = Token.RIGHT_BRACE Then
                    consume(Token.RIGHT_BRACE)


                    If currentToken.Type = Token.SEMICOLON Then
                        consume(Token.SEMICOLON)
                        AddParsingResult("Log statement parsed successfully")
                    Else

                        AddError("Expected ';' after log statement")
                    End If
                Else

                    AddError("Expected ']' after log statement")
                End If
            Else

                AddError("Expected '{' after 'log'")
            End If
        Else

            AddError("Expected 'log'")
        End If
    End Sub
    Private Sub parse_expression()

        Debug.WriteLine("entered parse expression")

        If currentToken.Type = Token.KEYWORD AndAlso currentToken.Value = "int" Then

            consume(Token.KEYWORD)
            AddError("Unexpected 'int' keyword in expression")
            Exit Sub
        End If


        If currentToken.Type = Token.IDENTIFIER Then
            parse_identifier()


            If currentToken.Type = Token.OP Then
                consume(Token.OP)


                parse_value_or_expression()
            Else
                AddError("Expected operator after identifier")
            End If
        Else
            AddError("Expected identifier at the beginning of the expression")
        End If

        AddParsingResult("Expression parsed successfully")
    End Sub

    Private Sub parse_value_or_expression()

        Select Case currentToken.Type
            Case Token.IDENTIFIER
                parse_identifier()
            Case Token.NUMBER
                consume(Token.NUMBER)
            Case Else
                AddError("Expected a value or expression")
        End Select
    End Sub


    Private Sub parse_type()
        If currentToken.Value.ToLower() = "int" AndAlso currentToken.Type = Token.IDENTIFIER Then
            consume(Token.IDENTIFIER)
        Else
            HandleSyntaxError(currentToken)
        End If
    End Sub

    Private Sub parse_declaration()

        If currentToken.Type = Token.KEYWORD AndAlso currentToken.Value = "int" Then
            consume(Token.KEYWORD)
        Else

            AddError("Expected 'int' type keyword")
            Exit Sub ' Exit the function if an error occurs
        End If

        ' Ensure next token is an identifier
        If currentToken.Type <> Token.IDENTIFIER Then
            AddError("Expected variable name after 'int'")
            Exit Sub
        End If

        Dim variableName As String = currentToken.Value
        consume(Token.IDENTIFIER)

        ' Check for an assignment operator 
        If currentToken.Type = Token.OP AndAlso currentToken.Value = "=" Then
            consume(Token.OP)

            parse_assignment_rhs()
        End If


        ' Check for a semicolon
        If currentToken.Type = Token.SEMICOLON Then
            consume(Token.SEMICOLON) ' Consume 
        Else

            AddError("Expected ';' at the end of declaration")
            Exit Sub
        End If

        AddParsingResult("Declaration parsed for variable: " & variableName)
    End Sub
    Private Sub parse_assignment_rhs()


        Select Case currentToken.Type
            Case Token.NUMBER
                consume(Token.NUMBER) ' Handle  value
            Case Token.IDENTIFIER
                parse_identifier()
            Case Else
                AddError("Expected a value or identifier after '=' in declaration")
        End Select
    End Sub
    Private Sub parse_if_statement()
        consume(Token.IF_KEYWORD)


        If currentToken.Type = Token.LEFT_PARENTHESES Then
            consume(Token.LEFT_PARENTHESES)


            parse_expression()


            If currentToken.Type = Token.RIGHT_PARENTHESES Then
                consume(Token.RIGHT_PARENTHESES)
            Else

                AddError("Expected ')' after condition expression")
                Exit Sub ' Exit the function if an error occurs
            End If
        Else

            AddError("Expected '(' after 'if' keyword")
            Exit Sub
        End If


        If currentToken.Type = Token.LEFT_BRACE Then
            consume(Token.LEFT_BRACE)


            While currentToken.Type <> Token.RIGHT_BRACE And currentToken.Type <> Token.EOF
                parse_statement()
            End While

            ' Check for closing brace '}'
            If currentToken.Type = Token.RIGHT_BRACE Then
                consume(Token.RIGHT_BRACE)
            Else
                AddError("Expected '}' at the end of if block")
                Exit Sub
            End If
        Else
            AddError("Expected '{' after if condition")
            Exit Sub
        End If
        If currentToken.Type = Token.ELSE_KEYWORD Then
            consume(Token.ELSE_KEYWORD)


            If currentToken.Type = Token.LEFT_BRACE Then
                consume(Token.LEFT_BRACE)

                ' Parse statements inside the else block
                While currentToken.Type <> Token.RIGHT_BRACE And currentToken.Type <> Token.EOF
                    parse_statement() ' Parse each statement inside the block
                End While

                ' Check for closing brace '}'
                If currentToken.Type = Token.RIGHT_BRACE Then
                    consume(Token.RIGHT_BRACE)
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


    Private Sub parse_while_statement()
        consume(Token.WHILE_KEYWORD)

        If currentToken.Type = Token.LEFT_PARENTHESES Then
            consume(Token.LEFT_PARENTHESES) ' Consume the opening (

            ' Parse the condition
            parse_expression()


            If currentToken.Type = Token.RIGHT_PARENTHESES Then
                consume(Token.RIGHT_PARENTHESES) ' Consume 
            Else
                ' Handle error if closing parenthesis is missing
                AddError("Expected ')' after condition expression")
                Exit Sub
            End If
        Else
            ' Handle error if opening parenthesis is missing
            AddError("Expected '(' after 'while' keyword")
            Exit Sub ' Exit the Error
        End If

        ' Check for  { 
        If currentToken.Type = Token.LEFT_BRACE Then
            consume(Token.LEFT_BRACE) ' Consume 

            ' Parse statements inside the while loop
            While currentToken.Type <> Token.RIGHT_BRACE And currentToken.Type <> Token.EOF
                parse_statement() ' Parse each statement inside block
            End While

            ' Check for closing brace '}'
            If currentToken.Type = Token.RIGHT_BRACE Then
                consume(Token.RIGHT_BRACE) ' Consume 
            Else
                AddError("Expected '}' at the end of while block")
                Exit Sub
            End If
        Else
            AddError("Expected '{' after while condition")
            Exit Sub
        End If

        AddParsingResult("While loop parsed successfully")
    End Sub






    Private tokens As List(Of Token)
    Private currentPosition As Integer

    Private Function peekNextToken() As Token
        ' Check if there is a next token
        If currentPosition < tokens.Count - 1 Then

            Return tokens(currentPosition + 1)
        Else

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


    Private Sub HandleSyntaxError(token As Token)
        Dim errorMessage As String = "Syntax Error at line " & scanner.tokenLine
        AddParsingResult(errorMessage)
        hasSyntaxError = True


    End Sub
    Private Sub AddError(message As String)
        Dim errorMsg As String = $"Error at line {scanner.tokenLine}: {message}"
        parsingResults.Add(errorMsg)
        hasSyntaxError = True
    End Sub
End Class