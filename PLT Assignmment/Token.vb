Public Class Token
    Public Const EOF As Integer = 0
    Public Const IDENTIFIER As Integer = 1
    Public Const LEFT_BRACE As Integer = 2
    Public Const RIGHT_BRACE As Integer = 3
    Public Const SEMICOLON As Integer = 4
    Public Const START_STATEMENT As Integer = 5
    Public Const END_STATEMENT As Integer = 6
    Public Const NUMBER As Integer = 7
    Public Const IF_KEYWORD As Integer = 10
    Public Const ELSE_KEYWORD As Integer = 11
    Public Const START_KEYWORD As Integer = 12
    Public Const END_KEYWORD As Integer = 13
    Public Const OP As Integer = 15
    Public Const UNKNOWN As Integer = 16


    Public Property Type As Integer
    Public Property Value As String

    Public Sub New(type As Integer, value As String)
        Me.Type = type
        Me.Value = value
    End Sub

    Public Overrides Function ToString() As String
        Return $"Type: {Type}, Value: {Value}"
    End Function
End Class
