Imports System.Text

Public NotInheritable Class StringUtil
    ''' <summary>
    ''' インスタンスから部分文字列を取得します。検索は、指定した文字位置から開始されます。
    ''' </summary>
    ''' <param name="target">対象となる<see cref="System.String" /></param>
    ''' <param name="startIndex">このインスタンス内の部分文字列の開始文字位置。</param>
    ''' <param name="length">取得したい文字列の長さ</param>
    ''' <returns></returns>
    ''' <exception cref="ArgumentOutOfRangeException">
    ''' startIndex と length を足した数が、このインスタンス内にない位置を示しています。
    ''' または startIndex または length が 0 未満です。
    ''' </exception>
    Public Shared Function SubstringByte(ByVal target As String, ByVal startIndex As Integer, ByVal length As Integer) As String
        Try
            Dim byteShiftJIS As Byte() = Encoding.GetEncoding("Shift_JIS").GetBytes(target)
            Return Encoding.Default.GetString(byteShiftJIS, startIndex, length).Trim()
        Catch ex As ArgumentOutOfRangeException
            Return ""
        End Try
    End Function

    ''' <summary>
    ''' このインスタンス内の文字を左寄せし、指定した文字列のバイト数になるまで、右側に空白を埋め込みます。
    ''' </summary>
    ''' <param name="target">対象となる<see cref="System.String" /></param>
    ''' <param name="totalWidth">結果として生成される文字列のバイト数</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function PadByteRight(ByVal target As String, ByVal totalWidth As Integer) As String
        Dim byteShiftJIS As Byte() = Encoding.GetEncoding("Shift_JIS").GetBytes(target)
        Dim blank As String = (String.Empty).PadLeft(totalWidth - byteShiftJIS.Length)
        Return Encoding.Default.GetString(byteShiftJIS, 0, byteShiftJIS.Length) & blank
    End Function

    Public Shared Function BuildParameterString(ByVal target As String, ByVal length As Integer) As String
        If StringUtil.IsNothingOrEmpty(target) Then
            Return StringUtil.PadByteRight(String.Empty, length)
        Else
            Return StringUtil.PadByteRight(target, length)
        End If
    End Function

    Public Shared Function IsNothingOrEmpty(ByVal target As String) As Boolean
        If target Is Nothing Then Return True
        If String.Empty.Equals(target) Then Return True
        Return False
    End Function

End Class
