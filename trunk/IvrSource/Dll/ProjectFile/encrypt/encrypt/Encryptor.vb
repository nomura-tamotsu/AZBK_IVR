Imports System.Collections.Generic
Imports System.Text
Imports System.Security.Cryptography
Imports System.IO

Friend Class EncryptionKey
    Public Const Key As String = "1YqUBwqS8LsRHbc+xM1GFteL"
    Public Const IV As String = "s8OR9FE/"
End Class


Public Class Encryptor
    Public Sub New()
        transformer = New EncryptTransformer(EncryptionAlgorithm.TripleDes)
    End Sub

    Private transformer As EncryptTransformer
    Private initVec As Byte()
    Private encKey As Byte()

    Public Function EncryptStr(ByVal strInPass As String) As String
        Dim plainText As Byte() = Nothing
        Dim cipherText As Byte() = Nothing
        Dim strOutPass As String = ""
        Dim key As Byte() = Encoding.ASCII.GetBytes(EncryptionKey.Key)
        Me.IV = Encoding.ASCII.GetBytes(EncryptionKey.IV)

        plainText = Encoding.UTF8.GetBytes(strInPass)
        cipherText = Me.Encrypt(plainText, key)
        strOutPass = Convert.ToBase64String(cipherText)

        Return strOutPass
    End Function

    Private Function Encrypt(ByVal bytesData As Byte(), ByVal bytesKey As Byte()) As Byte()
        '暗号化されたデータを保持するストリームを設定する
        Dim memStreamEncryptedData As New MemoryStream()

        transformer.IV = initVec
        Dim transform As ICryptoTransform = transformer.GetCryptoServiceProvider(bytesKey)
        Dim encStream As New CryptoStream(memStreamEncryptedData, transform, CryptoStreamMode.Write)
        Try
            'データを暗号化してメモリ ストリームに書き込む
            encStream.Write(bytesData, 0, bytesData.Length)
        Catch ex As Exception
            Throw New Exception("暗号化されたデータをストリームに書き込み中にエラーが発生しました: " & vbLf & ex.Message)
        End Try
        'クライアントが取得できるように初期化ベクタとキーを設定する
        encKey = transformer.Key
        initVec = transformer.IV
        encStream.FlushFinalBlock()
        encStream.Close()

        'データを送り返す
        Return memStreamEncryptedData.ToArray()
    End Function

    Private Property IV() As Byte()
        Get
            Return initVec
        End Get
        Set(ByVal value As Byte())
            initVec = value
        End Set
    End Property

    Private ReadOnly Property Key() As Byte()
        Get
            Return encKey
        End Get
    End Property
End Class
