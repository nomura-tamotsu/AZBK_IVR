Imports System.Collections.Generic
Imports System.Text
Imports System.Security.Cryptography

Public Enum EncryptionAlgorithm
    Des = 1
    Rc2
    Rijndael
    TripleDes
End Enum

Friend Class EncryptTransformer
    Private algorithmID As EncryptionAlgorithm
    Private initVec As Byte()
    Private encKey As Byte()

    Friend Property IV() As Byte()
        Get
            Return initVec
        End Get
        Set(ByVal value As Byte())
            initVec = value
        End Set
    End Property

    Friend ReadOnly Property Key() As Byte()
        Get
            Return encKey
        End Get
    End Property

    Friend Sub New(ByVal algId As EncryptionAlgorithm)
        '使用中のアルゴリズムを保存する
        algorithmID = algId
    End Sub

    Friend Function GetCryptoServiceProvider(ByVal bytesKey As Byte()) As ICryptoTransform
        ' プロバイダを選ぶ
        Select Case algorithmID
            Case EncryptionAlgorithm.TripleDes
                Dim des3 As TripleDES = New TripleDESCryptoServiceProvider()
                des3.Mode = CipherMode.CBC
                des3.Padding = PaddingMode.PKCS7
                'キーが提供されているか調べる
                If bytesKey Is Nothing Then
                    encKey = des3.Key
                Else
                    des3.Key = bytesKey
                    encKey = des3.Key
                End If
                'クライアントが初期化ベクタを提供したか調べる
                If initVec Is Nothing Then
                    'アルゴリズムにベクタを 1 つ作成させる
                    initVec = des3.IV
                Else
                    'ベクタをアルゴリズムに与える
                    des3.IV = initVec
                End If
                Return des3.CreateEncryptor()

            Case Else
                Throw New CryptographicException("アルゴリズム ID '" & Convert.ToString(algorithmID) & "' はサポートされていません。")
        End Select
    End Function

End Class
