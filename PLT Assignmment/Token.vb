Public Class Token
    Public Const EOF As Integer = 0
    Public Const IDENTIFIER As Integer = 1
    Public Const LEFT_BRACE As Integer = 2
    Public Const RIGHT_BRACE As Integer = 3
    Public Const SEMICOLON As Integer = 4
    Public Const START_STATEMENT As Integer = 5
    Public Const END_STATEMENT As Integer = 6
    Public Const NUMBER As Integer = 7
    Public Const IF_KEYWORD As Integer = 8
    Public Const ELSE_KEYWORD As Integer = 9
    Public Const START_KEYWORD As Integer = 10
    Public Const END_KEYWORD As Integer = 11
    Public Const SEPARATOR As Integer = 12
    Public Const OP As Integer = 13
    Public Const LOG As Integer = 14
    Public Const KEYWORD As Integer = 15
    Public Const LEFT_PARENTHESES As Integer = 16
    Public Const RIGHT_PARENTHESES As Integer = 17
    Public Const COLON As Integer = 18
    Public Const WHILE_KEYWORD As Integer = 19



    Public Const UNKNOWN As Integer = 20



    Public Property Type As Integer
    Public Property Value As String
    Public Property LineNumber As Integer ' Add line number property


    Public Sub New(type As Integer, value As String, Optional LineNumber As Integer = 0)
        Me.Type = type
        Me.Value = value
        Me.LineNumber = LineNumber
    End Sub

    Public Overrides Function ToString() As String
        Return $"Type: {Type}, Value: {Value}"
    End Function
End Class
