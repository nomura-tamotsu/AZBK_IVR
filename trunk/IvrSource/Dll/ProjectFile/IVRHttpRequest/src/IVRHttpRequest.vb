Option Strict On
Option Explicit On
Imports System.Net
Imports System.Text
Imports System.IO
Imports System.Collections
Imports System.Security.Cryptography.X509Certificates
Imports System.Reflection
Imports System.Xml
Imports System.Diagnostics


Public Class IVRHttpRequest

#Region "列挙体"
    Private Enum RetryChkRslt As Integer
        Retry = 0       'エクセプションステータスがリトライ対象
        NoRetry = 1     'エクセプションステータスがリトライ不可
    End Enum
#End Region
    'Private Const IVR_INI_PATH As String = "D:\I3\IC\Server\IVRHttpRequest.ini"
    Private Const IVR_INI_PATH As String = "IVRHttpRequest.ini"
    Private Const DLL_ERRCD_OTHER As String = "300"
    Private Const DLL_ERRCD_RETRY As String = "399"

    Private _iniFilePath As String = ""
    Private _iniFileExt As Boolean = False
    Private _iniTimeOut As Integer
    Private _iniStatuses() As String

    Private _debugMode As String = ""
    Private _debugLog As String = ""

    Private _ReqURL As String = "" '接続URL

    Private _XMLPath As String = "" 'XMLパス

    ''' <summary>
    ''' コンストラクタ
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        Dim execPath As String = My.Application.Info.DirectoryPath.ToString()
        _iniFilePath = String.Format("{0}\{1}", execPath, IVR_INI_PATH)
        _iniFileExt = System.IO.File.Exists(_iniFilePath)
        If _iniFileExt = True Then
            Dim wrk As String = GetProfileString(_iniFilePath, "System", "TimeOut")
            _iniTimeOut = TryParseInteger(wrk)
            Dim lsStatusList As String = ""
            lsStatusList = GetProfileString(_iniFilePath, "RetryStatus", "StatusList")
            If Not lsStatusList = "" Then
                _iniStatuses = lsStatusList.Split(","c)
            End If
            _XMLPath = GetProfileString(_iniFilePath, "System", "XMLPath")
        End If
    End Sub

    ''' <summary>
    ''' EJB呼出
    ''' </summary>
    ''' <param name="inEJBInfo1"></param>
    ''' <param name="inEJBInfo2"></param>
    ''' <param name="inApServerNo">1桁目：環境コード、2桁目：APサーバ番号</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ExecEJB(ByVal inEJBInfo1 As String, ByVal inEJBInfo2 As String, ByVal inApServerNo As String) As String
        Try
            'INIファイルの存在チェック
            If _iniFileExt = False Then
                Return String.Format("{0}{1}", DLL_ERRCD_OTHER, "設定ファイルが存在しません")
            End If

            '----------------------------------------------------------------------------------------------------
            Dim envCodeIvr As String = inApServerNo.Remove(inApServerNo.Length - 1, 1)
            _debugMode = GetProfileString(_iniFilePath, "DebugMode", envCodeIvr)
            _debugLog = GetProfileString(_iniFilePath, "DebugMode", "Logput")
            '----------------------------------------------------------------------------------------------------

            '----------------------------------------------------------------------------------------------------
            'デバックログ出力
            DebugLogWrite(envCodeIvr, String.Format("----------------------------------------------------------------------------------------------------"))
            DebugLogWrite(envCodeIvr, String.Format("EJBInfo1:{0}", inEJBInfo1))
            DebugLogWrite(envCodeIvr, String.Format("EJBInfo2:{0}", inEJBInfo2))
            DebugLogWrite(envCodeIvr, String.Format("ApServerNo:{0}", inApServerNo))
            DebugLogWrite(envCodeIvr, String.Format("INIファイルパス：{0}", _iniFilePath))
            '----------------------------------------------------------------------------------------------------

            '----------------------------------------------------------------------------------------------------
            '要求データ情報XML、応答データ情報XMLのフルPathを作成する
            '----------------------------------------------------------------------------------------------------
            'Dim ejbApplicationId As String = StringUtil.SubstringByte(inEJBInfo2, 80, 30).Trim()
            Dim ejbApplicationId As String = inEJBInfo2.Split("|"c)(2)
            Dim requestDataInfoXML As String = ""
            'requestDataInfoXML = String.Format("D:\I3\IC\Resources\{0}\xml\EJBReq_{1}.xml", envCodeIvr, ejbApplicationId)
            requestDataInfoXML = String.Format("{0}\{1}\xml\EJBReq_{2}.xml", _XMLPath, envCodeIvr, ejbApplicationId)
            Dim responseDataInfoXML As String = ""
            'responseDataInfoXML = String.Format("D:\I3\IC\Resources\{0}\xml\EJBRes_{1}.xml", envCodeIvr, ejbApplicationId)
            responseDataInfoXML = String.Format("{0}\{1}\xml\EJBRes_{2}.xml", _XMLPath, envCodeIvr, ejbApplicationId)
            '----------------------------------------------------------------------------------------------------

            '----------------------------------------------------------------------------------------------------
            'EJB要求文字列※項目毎に「|」区切り)を、要求データ情報XMLに従い、固定長文字列に編集する
            '----------------------------------------------------------------------------------------------------
            Dim requestData As String = EditRequestData(requestDataInfoXML, inEJBInfo2)
            '----------------------------------------------------------------------------------------------------

            '----------------------------------------------------------------------------------------------------
            'デバックログ出力
            DebugLogWrite(envCodeIvr, String.Format("編集後EJB要求情報:{0}", requestData))
            '----------------------------------------------------------------------------------------------------

            'リクエストURLを決定
            Dim ReqURL As String = ""       '接続URL
            Dim ReqHostName As String = ""  '接続ホスト名
            Dim strRtn As String = ""

            ReqURL = GetProfileString(_iniFilePath, "EjbURL", inApServerNo)
            _ReqURL = ReqURL
            If ReqURL.Equals("") Then
                Return String.Format("{0}{1}", DLL_ERRCD_OTHER, String.Format("未定義のサーバ番号[{0}]", inApServerNo))
            End If

            ReqHostName = GetProfileString(_iniFilePath, "EjbHostName", inApServerNo)
            If ReqURL.Equals("") Then
                Return String.Format("{0}{1}", DLL_ERRCD_OTHER, String.Format("ホスト名未定義のサーバ番号[{0}]", inApServerNo))
            End If

            '----------------------------------------------------------------------------------------------------
            If _debugMode.Equals("on") Then
                'EJB折返しモードの場合は、Ping確認しない
            Else
                Dim pingErrMsg As String = ""
                If SendPing(ReqHostName, pingErrMsg) = False Then
                    ' Pingが通らないため、迂回
                    'Return String.Format("{0}{1}(HOST名={2}", GetProfileString(_iniFilePath, "System", "PingFailedReturn"), "対象サーバにPingが通りませんでした。", ReqHostName)
                    Return String.Format("{0}{1}({2})(HOST名={3}", GetProfileString(_iniFilePath, "System", "PingFailedReturn"), "対象サーバにPingが通りませんでした。", pingErrMsg, ReqHostName)
                End If
            End If

            '----------------------------------------------------------------------------------------------------
            ' ポスト・データの作成
            'Dim PostData As Byte() = MakePostData_ExecEJB(inEJBInfo1, inEJBInfo2)
            Dim PostData As Byte() = MakePostData_ExecEJB(inEJBInfo1, requestData)

            '----------------------------------------------------------------------------------------------------
            If _debugMode.Equals("on") Then
                'EJB折返しモードの場合
                'Dim debugResponseFile As String = String.Format("D:\I3\IC\Resources\{0}\debug\DebugRes_{1}.xml", envCodeIvr, ejbApplicationId)
                Dim debugResponseFile As String = String.Format("{0}\{1}\debug\DebugRes_{2}.xml", _XMLPath, envCodeIvr, ejbApplicationId)
                strRtn = GetWebResponseDebug(debugResponseFile)
            Else
                strRtn = GetWebResponse(ReqURL, PostData, _iniTimeOut)
            End If

            '----------------------------------------------------------------------------------------------------
            'デバックログ出力
            DebugLogWrite(envCodeIvr, String.Format("EJB応答情報:{0}", strRtn))
            '----------------------------------------------------------------------------------------------------

            '----------------------------------------------------------------------------------------------------
            'EJBからの応答データを、応答データ情報XMLに従い、項目毎に「|」区切りに編集する
            '----------------------------------------------------------------------------------------------------
            If strRtn Is Nothing Then
                strRtn = String.Format("{0}{1}", DLL_ERRCD_OTHER, "レスポンスデータがNULL")
            Else
                If strRtn.Substring(0, 3).Equals(DLL_ERRCD_OTHER) Or strRtn.Substring(0, 3).Equals(DLL_ERRCD_RETRY) Then
                    'DLL内エラー場合は、「|」区切りに編集は行わない
                Else
                    'DLL内エラー以外(300 or 399以外)の場合に、応答データの編集を行う
                    Dim responseData As String = EditResponseData(responseDataInfoXML, strRtn)
                    strRtn = responseData
                End If
            End If
            '----------------------------------------------------------------------------------------------------

            '----------------------------------------------------------------------------------------------------
            'デバックログ出力
            DebugLogWrite(envCodeIvr, String.Format("EJB呼出返却情報:{0}", strRtn))
            '----------------------------------------------------------------------------------------------------

            Return strRtn
        Catch ex As Exception
            Return String.Format("{0}{1}(EJB接続URL={2})", DLL_ERRCD_OTHER, ex.Message, _ReqURL)
        End Try
    End Function

    ''' <summary>
    ''' HTTPリクエスト＆レスポンス取得
    ''' </summary>
    ''' <param name="inReqURL"></param>
    ''' <param name="inPostData"></param>
    ''' <param name="inTimeOut"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetWebResponse(ByVal inReqURL As String, ByVal inPostData As Byte(), ByVal inTimeOut As Integer) As String
        Try
            'サーバからの証明書が認証されていないのを回避
            ServicePointManager.ServerCertificateValidationCallback = New System.Net.Security.RemoteCertificateValidationCallback(AddressOf myCertificateValidation)
            ' リクエストの作成
            Dim req As HttpWebRequest = CType(WebRequest.Create(inReqURL), HttpWebRequest)
            With req
                .Timeout = inTimeOut
                .Method = "POST"
                .ContentType = "application/x-www-form-urlencoded"
                .ContentLength = inPostData.Length
            End With
            ' ポスト・データの書き込み
            Dim reqStream As Stream = req.GetRequestStream()
            With reqStream
                .Write(inPostData, 0, inPostData.Length)
                .Close()
            End With
            ' レスポンスの取得と読み込み
            Dim res As WebResponse = req.GetResponse()
            Dim resStream As Stream = res.GetResponseStream()
            Dim sr As StreamReader = New StreamReader(resStream, Encoding.UTF8)
            Dim sReturn As String = sr.ReadToEnd()
            sr.Close()
            resStream.Close()
            Return sReturn
        Catch exWeb As WebException
            If RetryCheck(exWeb.Status) = RetryChkRslt.Retry Then
                'リトライ対象
                Return String.Format("{0}{1}(EJB接続URL={2})", DLL_ERRCD_RETRY, exWeb.Message, inReqURL)
            End If
            Return String.Format("{0}{1}(EJB接続URL={2})", DLL_ERRCD_OTHER, exWeb.Message, inReqURL)
        Catch ex As Exception
            Return String.Format("{0}{1}(EJB接続URL={2})", DLL_ERRCD_OTHER, ex.Message, inReqURL)
        End Try
    End Function

    ''' <summary>
    ''' ポスト・データの作成
    ''' </summary>
    ''' <param name="inEJBInfo1"></param>
    ''' <param name="inEJBInfo2"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function MakePostData_ExecEJB(ByVal inEJBInfo1 As String, ByVal inEJBInfo2 As String) As Byte()
        Dim param As String = ""
        Dim ht As Hashtable = New Hashtable()
        Dim enc As Encoding = Encoding.UTF8()
        '・appid: EJBの識別名(COMに渡していた値と同じ)
        ht("appid") = inEJBInfo1

        '・mbdata: 要求文字列(COMに渡していた値と同じ)
        ht("mbdata") = System.Web.HttpUtility.UrlEncode(inEJBInfo2, enc)

        For Each k As String In ht.Keys
            param = param & String.Format("{0}={1}&", k, ht(k))
        Next
        'Return Encoding.UTF8.GetBytes(param)
        Return Encoding.ASCII.GetBytes(param)
    End Function

    ''' <summary>
    ''' サーバからの証明書が認証されていないのを回避（HTTPSの場合のみ有効）
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="certificate"></param>
    ''' <param name="chain"></param>
    ''' <param name="sslPolicyErrors"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function myCertificateValidation(ByVal sender As Object, ByVal certificate As X509Certificate, ByVal chain As X509Chain, ByVal sslPolicyErrors As System.Net.Security.SslPolicyErrors) As Boolean
        Select Case sslPolicyErrors
            Case Security.SslPolicyErrors.None 'SSL のポリシー エラーはありません。  
                Return True
            Case Security.SslPolicyErrors.RemoteCertificateChainErrors 'ChainStatus が、空でない配列を返しました。  
                Return True
            Case Security.SslPolicyErrors.RemoteCertificateNameMismatch '証明書名が不一致です。  
                Return True
            Case Security.SslPolicyErrors.RemoteCertificateNotAvailable '証明書が利用できません。  
                Return True
            Case Else
                Return False
        End Select
    End Function

    ''' <summary>
    ''' リトライチェック
    ''' </summary>
    ''' <param name="WebExStatus"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function RetryCheck(ByVal WebExStatus As WebExceptionStatus) As RetryChkRslt
        'INIファイルの存在チェック
        If _iniFileExt = False Then
            Return RetryChkRslt.NoRetry 'リトライ不可
        End If

        'リトライステータス無しの場合
        If _iniStatuses Is Nothing Then
            Return RetryChkRslt.NoRetry 'リトライ不可
        End If

        For i As Integer = 0 To _iniStatuses.Length - 1
            If WebExStatus = TryParseInteger(_iniStatuses(i)) Then
                Return RetryChkRslt.Retry 'リトライ対象
            End If
        Next i
        Return RetryChkRslt.NoRetry 'リトライ不可
    End Function

    ''' <summary>
    ''' 指定されたホストに対してPing 要求を実行します。
    ''' </summary>
    ''' <param name="hostName">Ping要求を実行するホスト名（IPアドレスも可）</param>
    ''' <returns>Pingが実行できれば Trueを返し、そうでなければ Falseを返します。</returns>
    Private Function SendPing(ByVal hostName As String, ByRef errMsg As String) As Boolean
        Try
            'MNB現行はPing応答監視時間を指定しない
            'Dim pingTimeout As Integer = CInt(GetProfileString(_iniFilePath, "System", "PingTimeOut"))
            Dim ping As New NetworkInformation.Ping
            'Dim reply As NetworkInformation.PingReply = ping.Send(hostName, pingTimeout)
            Dim reply As NetworkInformation.PingReply = ping.Send(hostName)

            'GC.KeepAlive(ping)

            If reply.Status.Equals(NetworkInformation.IPStatus.Success) Then
                errMsg = ""
                Return True
            End If

        Catch ex As System.Net.Sockets.SocketException
            errMsg = String.Format("SocketException発生(Message={0})", ex.Message)
            Return False
        Catch ex As System.Net.NetworkInformation.PingException
            errMsg = String.Format("PingException発生(Message={0})", ex.Message)
            Return False
        Catch ex As ArgumentException
            errMsg = String.Format("ArgumentException発生(Message={0})", ex.Message)
            Return False
        Catch ex As System.Exception
            errMsg = String.Format("System.Exception発生(Message={0})", ex.Message)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' 要求データを「|」区切りから、固定長文字列へ変換する
    ''' </summary>
    ''' <param name="xmlPath_">要求データ情報XML</param>
    ''' <param name="inEJBInfo2_">変換前のEJB要求データ</param>
    ''' <returns>変換後EJB要求データ</returns>
    ''' <remarks></remarks>
    Private Function EditRequestData(ByVal xmlPath_ As String, ByVal inEJBInfo2_ As String) As String
        Dim sb_ As New StringBuilder
        Dim returnStr As String = ""
        Dim xmlDoc_ As XmlDocument = Nothing

        Try
            Dim requestItmList() As String = inEJBInfo2_.Split("|"c)

            'XMLファイル読込み
            xmlDoc_ = New XmlDocument
            xmlDoc_.Load(xmlPath_)

            Dim rootNode_ As XmlNode
            rootNode_ = xmlDoc_.SelectSingleNode("ejbRequest")

            Dim length As String = ""
            Dim len As Integer = 0
            Dim itmIndx As Integer = 0
            For Each node As XmlNode In rootNode_.ChildNodes
                If XmlNodeType.Comment.Equals(node.NodeType) Then
                    Continue For
                End If

                length = node.Attributes("length").Value
                Integer.TryParse(length, len)

                sb_.Append(StringUtil.BuildParameterString(requestItmList(itmIndx), len))
                itmIndx += 1
            Next
            returnStr = sb_.ToString()
        Catch ex As Exception
            Throw ex
        Finally
            xmlDoc_ = Nothing
        End Try

        Return returnStr

    End Function

    ''' <summary>
    '''応答データを、応答データ情報XMLに従い、項目毎に「|」区切りに編集する
    ''' </summary>
    ''' <param name="xmlPath_">応答データ情報XML</param>
    ''' <param name="ejbResponse_">変換前のEJB応答データ</param>
    ''' <returns>変換後EJB応答データ</returns>
    ''' <remarks></remarks>
    Private Function EditResponseData(ByVal xmlPath_ As String, ByVal ejbResponse_ As String) As String
        Dim sb_ As New StringBuilder
        Dim returnStr As String = ""
        Dim xmlDoc_ As XmlDocument = Nothing

        Try
            'XMLファイル読込み
            xmlDoc_ = New XmlDocument
            xmlDoc_.Load(xmlPath_)

            Dim rootNode_ As XmlNode
            rootNode_ = xmlDoc_.SelectSingleNode("ejbResponse")

            Dim index_ As Integer = 0

            Dim type As String = ""
            Dim length As String = ""
            Dim value As String = ""
            Dim name As String = ""

            Dim len As Integer = 0
            Dim vecCnt As Integer = 0

            Dim itemList As ArrayList = New ArrayList()

            For Each node As XmlNode In rootNode_.ChildNodes
                If XmlNodeType.Comment.Equals(node.NodeType) Then
                    Continue For
                End If

                type = node.Attributes("type").Value
                length = node.Attributes("length").Value
                value = node.Attributes("value").Value
                name = node.Attributes("name").Value

                Integer.TryParse(length, len)

                Select Case type
                    Case "string"
                        sb_.Append(StringUtil.SubstringByte(ejbResponse_, index_, len))
                        sb_.Append("|")
                        index_ += len

                    Case "vectorCnt"
                        Integer.TryParse(StringUtil.SubstringByte(ejbResponse_, index_, len), vecCnt)
                        itemList.Clear()

                        sb_.Append(StringUtil.SubstringByte(ejbResponse_, index_, len))
                        sb_.Append("|")
                        index_ += len

                    Case "vectorItm"
                        itemList.Add(len)

                    Case "vectorEnd"
                        itemList.Add(len)

                        For i As Integer = 0 To vecCnt - 1
                            For j As Integer = 0 To itemList.Count - 1
                                sb_.Append(StringUtil.SubstringByte(ejbResponse_, index_, CType(itemList(j), Integer)))
                                sb_.Append("|")
                                index_ += CType(itemList(j), Integer)
                            Next j
                        Next i

                    Case Else
                End Select
            Next
            sb_.Remove(sb_.Length - 1, 1)
            returnStr = sb_.ToString()
        Catch ex As Exception
            Throw ex
        Finally
            xmlDoc_ = Nothing
        End Try

        Return returnStr

    End Function

#Region "デバック用"
    ''' <summary>
    ''' デバック用応答データ作成
    ''' </summary>
    ''' <param name="xmlPath_"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetWebResponseDebug(ByVal xmlPath_ As String) As String
        Dim sb_ As New StringBuilder
        Dim returnStr As String = ""
        Dim xmlDoc_ As XmlDocument = Nothing

        Try
            'XMLファイル読込み
            xmlDoc_ = New XmlDocument
            xmlDoc_.Load(xmlPath_)

            Dim rootNode_ As XmlNode
            rootNode_ = xmlDoc_.SelectSingleNode("ejbResponse")

            Dim length As String = ""
            Dim len As Integer = 0
            Dim value As String = ""
            For Each node As XmlNode In rootNode_.ChildNodes
                If XmlNodeType.Comment.Equals(node.NodeType) Then
                    Continue For
                End If

                length = node.Attributes("length").Value
                value = node.Attributes("value").Value

                Integer.TryParse(length, len)

                sb_.Append(StringUtil.BuildParameterString(value, len))
            Next
            returnStr = sb_.ToString()
        Catch ex As Exception
            Throw ex
        Finally
            xmlDoc_ = Nothing
        End Try

        Return returnStr

    End Function

    ''' <summary>
    ''' デバックログ出力
    ''' </summary>
    ''' <param name="envCodeIvr_">環境コード</param>
    ''' <param name="messageStr_">出力メッセージ</param>
    ''' <remarks></remarks>
    Public Sub DebugLogWrite(ByVal envCodeIvr_ As String, ByVal messageStr_ As String)
        Try
            If Not _debugLog.Equals("on") Then
                Return
            End If

            'ログ出力ディレクトリ
            Dim logFileDir As String = String.Format("{0}\{1}\debug\", _XMLPath, envCodeIvr_)
            If Not System.IO.Directory.Exists(logFileDir) Then
                '出力ディレクトリが存在しない場合は、何もしない
                Return
            End If

            'ログファイル名編集
            Dim strFullPath As String = String.Format("{0}DebugLog.txt", logFileDir)
            Dim appendFlg As Boolean = True

            'ログ出力メッセージ編集
            Dim strLogMessage As String = ""
            Dim strDateTime As String = String.Format("{0}.{1}", DateTime.Now.ToString, DateTime.Now.Millisecond.ToString("000"))
            strLogMessage = String.Format("{0},{1}", strDateTime, messageStr_)

            '調査ログ出力
            Using logFile As New System.IO.StreamWriter(strFullPath, appendFlg, System.Text.Encoding.Default)
                logFile.WriteLine(strLogMessage)
                logFile.Flush()
                logFile.Close()
            End Using

        Catch ex As Exception
            '例外発生時は何も行わない。
        End Try

    End Sub

#End Region

End Class
