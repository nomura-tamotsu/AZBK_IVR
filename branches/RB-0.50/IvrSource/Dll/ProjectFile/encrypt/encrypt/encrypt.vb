''' <summary>
''' パスワード暗号化
''' </summary>
''' <remarks></remarks>
Public Class encrypt
    Public Function encode(ByVal strInPass As String, ByVal dummy1 As String, ByVal dummy2 As String, ByRef strEncPass As String) As Integer
        Dim encPass As String = ""
        Dim rtn As Integer = 9
        Try
            Dim enc As Encryptor = New Encryptor()
            encPass = enc.EncryptStr(strInPass)
            rtn = 0
        Catch ex As Exception
            encPass = ex.Message.ToString()
            rtn = 9
        End Try
        strEncPass = encPass
        Return rtn
    End Function
End Class
