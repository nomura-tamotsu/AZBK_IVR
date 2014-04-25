Option Strict On
Option Explicit On
Module CmnString
    '::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    '共通関数(文字列操作)
    '新規作成：2006/09/21(VB2005版)
    '修正履歴：
    '::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
#Region "API"
#Region "設定ファイル値獲得用"
    Declare Function GetPrivateProfileString Lib "KERNEL32.DLL" Alias "GetPrivateProfileStringA" ( _
        ByVal lpAppName As String, _
        ByVal lpKeyName As String, ByVal lpDefault As String, _
        ByVal lpReturnedString As StringBuilder, ByVal nSize As Integer, _
        ByVal lpFileName As String) As Integer
    Declare Function GetPrivateProfileStringByByteArray Lib "KERNEL32.DLL" Alias "GetPrivateProfileStringA" ( _
          ByVal lpAppName As String, _
          ByVal lpKeyName As String, ByVal lpDefault As String, _
          ByVal lpReturnedString As Byte(), ByVal nSize As Integer, _
          ByVal lpFileName As String) As Integer
#End Region
#End Region
#Region "関数"
#Region "設定ファイル読み込み"
    '-----------------------------------------------------------------------------
    ' @(n)
    '
    ' 関数名    : GetProfileString
    '
    ' 機能      : 設定ファイル読み込み
    '
    ' 引き数    : ARG1 -
    '
    ' 機能説明  : INIファイル名を指定して設定ファイルを読み込みます
    '
    ' 備考      :
    '-----------------------------------------------------------------------------
    Public Function GetProfileString(ByVal asIniName As String, ByVal Section As String, ByVal Entry As String) As String
        Dim strReturn As String = ""
        Dim lstrN As StringBuilder = New StringBuilder(1024)

        GetPrivateProfileString(Section, Entry, "", lstrN, lstrN.Capacity, asIniName)
        strReturn = lstrN.ToString()

        Return strReturn
    End Function
#End Region
#Region "設定ファイル読み込み(指定セクションのキーの一覧を得る)"
    '-----------------------------------------------------------------------------
    ' @(n)
    '
    ' 関数名    : GetProfileStringKey
    '
    ' 機能      : 設定ファイル読み込み(指定セクションのキーの一覧を得る)
    '
    ' 引き数    : ARG1 -
    '
    ' 機能説明  : INIファイル名を指定して設定ファイルを読み込みます(指定セクションのキーの一覧を得る)
    '
    ' 備考      :","区切りで取得する
    '-----------------------------------------------------------------------------
    Public Function GetProfileStringKey(ByVal asIniName As String, ByVal Section As String) As String
        Dim strReturn As String = ""

        Dim ar1(1024) As Byte
        Dim resultSize1 As Integer

        resultSize1 = GetPrivateProfileStringByByteArray(Section, Nothing, "default", ar1, ar1.Length, asIniName)
        If resultSize1 > 0 Then
            Dim result1 As String = System.Text.Encoding.Default.GetString(ar1, 0, resultSize1 - 1)
            Dim key As String = result1.Replace(Microsoft.VisualBasic.Chr(0), ",")
            strReturn = key
        End If


        Return strReturn
    End Function
#End Region
#Region "後スペース編集"
    '-----------------------------------------------------------------------------
    ' @(n)
    '
    ' 機能　　  : 後スペース埋め処理
    '
    ' 返り値　  : 文字列(スペース埋め)
    '
    ' 引き数　  : 文字列[strChara],バイト数[iByteSize]
    '
    ' 機能説明  : 渡された文字列を、バイト数で後スペース編集し、値を戻す
    '
    ' 備考　　  :
    '
    '-----------------------------------------------------------------------------
    Public Function SetAfterSpace(ByVal strChara As String, ByVal iByteSize As Integer) As String
        Dim strReturn As String = strChara

        strReturn = SetStringByte(strChara, iByteSize, False, " ")

        Return strReturn

    End Function
#End Region
#Region "前ゼロ編集"
    '-----------------------------------------------------------------------------
    ' @(n)
    '
    ' 機能　　  : 前ゼロ埋め処理
    '
    ' 返り値　  : 文字列(ゼロ埋め)
    '
    ' 引き数　  : 文字列[strChara],バイト数[iByteSize]
    '
    ' 機能説明  : 渡された文字列を、バイト数で前ゼロ編集し、値を戻す
    '
    ' 備考　　  :
    '
    '-----------------------------------------------------------------------------
    Public Function SetBeforeZero(ByVal strChara As String, ByVal iByteSize As Integer) As String
        Dim strReturn As String = strChara

        strReturn = SetStringByte(strChara, iByteSize, True, "0")


        Return strReturn
    End Function
#End Region
#Region "文字列編集"
    '-----------------------------------------------------------------------------
    ' @(n)
    '
    ' 機能　　  : 文字列編集
    '
    ' 返り値　  : 文字列
    '
    ' 引き数　  : 文字列[strChara],バイト数[iByteSize]
    '　　　　　　　前に加えるか後に加えるか[bBefore],加える文字[AddString]
    '
    ' 機能説明  : 
    '
    ' 備考　　  :
    '
    '-----------------------------------------------------------------------------
    Public Function SetStringByte(ByVal strChara As String, ByVal iByteSize As Integer, ByVal bBefore As Boolean, ByVal AddString As String) As String
        Dim strReturn As String = strChara
        Do While gLenByte(strReturn) < iByteSize
            If bBefore Then
                strReturn = AddString & strReturn
            Else
                strReturn = strReturn & AddString
            End If
        Loop
        If gLenByte(strReturn) > iByteSize Then
            If bBefore Then
                strReturn = gRightByte(strReturn, iByteSize)
            Else
                strReturn = gLeftByte(strReturn, iByteSize)
            End If
        End If
        Return strReturn
    End Function
#End Region
#Region "文字列が空白であれば、0を返す"
    '-----------------------------------------------------------------------------
    ' @(n)
    '
    ' 機能　　  : 数値データ取得
    '
    ' 返り値　  : 文字列
    '
    ' 引き数　  : 文字列[strChara]
    '
    ' 機能説明  : 与えられた文字列が空白であれば、0を返す
    '
    ' 備考　　  :
    '
    '-----------------------------------------------------------------------------
    Public Function SetNullToZero(ByVal strChara As String) As String
        Dim strReturn As String = strChara

        If String.IsNullOrEmpty(strReturn) Then strReturn = "0"

        Return strReturn
    End Function
#End Region
#Region "StringをLongに変換"
	'-----------------------------------------------------------------------------
	' @(n)
	'
	' 機能　　  : StringをLongに変換
	'
	' 返り値　  : Long
	'
	' 引き数　  : 文字列[strChara]
	'
	' 機能説明  :  StringをLongに変換できなければ、0を返す
	'
	' 備考　　  :
	'
	'---------------------------- -------------------------------------------------
	Public Function TryParseLong(ByVal strChara As String) As Long
		Dim bReturn As Long = 0
        If Not Long.TryParse(strChara, bReturn) Then
            bReturn = 0
        End If

		Return bReturn
	End Function
#End Region
#Region "StringをIntegerに変換"
    '-----------------------------------------------------------------------------
    ' @(n)
    '
    ' 機能　　  : StringをIntegerに変換
    '
    ' 返り値　  : Integer
    '
    ' 引き数　  : 文字列[strChara]
    '
    ' 機能説明  :  StringをIntegerに変換できなければ、0を返す
    '
    ' 備考　　  :
    '
    '---------------------------- -------------------------------------------------
    Public Function TryParseInteger(ByVal strChara As String) As Integer
        Dim bReturn As Integer = 0
        If Not Integer.TryParse(strChara, bReturn) Then
            bReturn = 0
        End If

        Return bReturn
    End Function
#End Region
#Region "StringをShortに変換"
    '-----------------------------------------------------------------------------
    ' @(n)
    '
    ' 機能　　  : StringをShortに変換
    '
    ' 返り値　  : Short
    '
    ' 引き数　  : 文字列[strChara]
    '
    ' 機能説明  :  StringをIntegerに変換できなければ、0を返す
    '
    ' 備考　　  :
    '
    '-----------------------------------------------------------------------------
    Public Function TryParseShort(ByVal strChara As String) As Short
        Dim bReturn As Short = 0
        If Not Short.TryParse(strChara, bReturn) Then
            bReturn = 0
        End If

        Return bReturn
    End Function
#End Region
#Region "文字列の指定されたバイト位置以降のすべて(または指定したサイズ)の文字列を返します。"
    '-----------------------------------------------------------------------------
    ' @(n)
    '
    ' 機能　　  : 文字列の指定されたバイト位置以降のすべての文字列を返します。
    '
    ' 返り値　  : 指定されたバイト位置以降のすべての文字列。
    '
    ' 引き数　  : stTarget    取り出す元になる文字列。
    '            iStart      取り出しを開始する位置。
    ' 機能説明  : VS2005に変換で使用
    '
    ' 備考　　  :
    '
    '-----------------------------------------------------------------------------
    Public Function gMidByte(ByVal stTarget As String, ByVal iStart As Integer) As String
        Dim bResult As String = ""
        If String.IsNullOrEmpty(stTarget) Then
            bResult = ""
        Else
            If iStart > gLenByte(stTarget) OrElse iStart < 0 Then
                bResult = ""
            Else
                Dim hEncoding As System.Text.Encoding = System.Text.Encoding.GetEncoding("Shift_JIS")
                Dim bBytes As Byte() = hEncoding.GetBytes(stTarget)
                bResult = hEncoding.GetString(bBytes, iStart - 1, bBytes.Length - iStart + 1)
            End If
        End If
        Return bResult
    End Function
    Public Function gMidByte _
    (ByVal stTarget As String, ByVal iStart As Integer, ByVal iByteSize As Integer) As String
        Dim bResult As String = ""
        If String.IsNullOrEmpty(stTarget) Then
            bResult = ""
        Else
            If iStart > gLenByte(stTarget) OrElse iStart < 0 OrElse iByteSize < 0 Then
                bResult = ""
            Else
                Dim hEncoding As System.Text.Encoding = System.Text.Encoding.GetEncoding("Shift_JIS")
                Dim bBytes As Byte() = hEncoding.GetBytes(stTarget)
                If iByteSize > gLenByte(stTarget) OrElse gLenByte(stTarget) < iStart + iByteSize Then
                    bResult = hEncoding.GetString(bBytes, iStart - 1, bBytes.Length - iStart + 1)
                Else
                    bResult = hEncoding.GetString(bBytes, iStart - 1, iByteSize)
                End If
            End If
        End If
        Return bResult
    End Function
#End Region
#Region "文字列の指定された文字列のバイト数を返します。"
    '-----------------------------------------------------------------------------
    ' @(n)
    '
    ' 機能　　  : 文字列の指定された文字列のバイト数を返します。
    '
    ' 返り値　  : 指定された文字列のバイト数
    '
    ' 引き数　  : stTarget    文字列。
    ' 機能説明  : VS2005に変換で使用
    '
    ' 備考　　  :
    '
    '-----------------------------------------------------------------------------
    Public Function gLenByte(ByVal stTarget As String) As Integer
        Dim bResult As Integer = 0
        If String.IsNullOrEmpty(stTarget) Then
            bResult = 0
        Else
            bResult = System.Text.Encoding.GetEncoding("Shift_JIS").GetByteCount(stTarget)
        End If
        Return bResult
    End Function
#End Region
#Region "文字列の左端から指定したバイト数分の文字列を返します。"
    '-----------------------------------------------------------------------------
    ' @(n)
    '
    ' 機能　　  : 文字列の左端から指定したバイト数分の文字列を返します。
    '
    ' 返り値　  : 左端から指定されたバイト数分の文字列。
    '
    ' 引き数　  : stTarget    取り出す元になる文字列。
    '            iByteSize   取り出すバイト数。
    ' 機能説明  : VS2005に変換で使用
    '
    ' 備考　　  :
    '
    '-----------------------------------------------------------------------------
    Public Function gLeftByte(ByVal stTarget As String, ByVal iByteSize As Integer) As String
        Dim bResult As String = ""
        If String.IsNullOrEmpty(stTarget) Then
            bResult = ""
        Else
            bResult = gMidByte(stTarget, 1, iByteSize)
        End If
        Return bResult
    End Function
#End Region
#Region "文字列の右端から指定したバイト数分の文字列を返します。"
    '-----------------------------------------------------------------------------
    ' @(n)
    '
    ' 機能　　  : 文字列の右端から指定したバイト数分の文字列を返します。
    '
    ' 返り値　  : 右端から指定されたバイト数分の文字列。
    '
    ' 引き数　  : stTarget    取り出す元になる文字列。
    '            iByteSize   取り出すバイト数。
    ' 機能説明  : VS2005に変換で使用
    '
    ' 備考　　  :
    '
    '-----------------------------------------------------------------------------
    Public Function gRightByte(ByVal stTarget As String, ByVal iByteSize As Integer) As String
        Dim bResult As String = ""
        Dim iStart As Integer = iByteSize
        If String.IsNullOrEmpty(stTarget) Then
            bResult = ""
        Else
            If iByteSize < 0 Then
                bResult = ""
            Else
                Dim hEncoding As System.Text.Encoding = System.Text.Encoding.GetEncoding("Shift_JIS")
                Dim bBytes As Byte() = hEncoding.GetBytes(stTarget.PadLeft(iByteSize))
                iStart = UBound(bBytes) + 1 - iByteSize 'バイト数
                bResult = hEncoding.GetString(bBytes, iStart, iByteSize)
                bResult = bResult.Trim
            End If
        End If
        Return bResult
    End Function
#End Region
#End Region
End Module
