Option Strict On
Option Explicit On
Module CmnString
    '::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    '���ʊ֐�(�����񑀍�)
    '�V�K�쐬�F2006/09/21(VB2005��)
    '�C�������F
    '::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
#Region "API"
#Region "�ݒ�t�@�C���l�l���p"
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
#Region "�֐�"
#Region "�ݒ�t�@�C���ǂݍ���"
    '-----------------------------------------------------------------------------
    ' @(n)
    '
    ' �֐���    : GetProfileString
    '
    ' �@�\      : �ݒ�t�@�C���ǂݍ���
    '
    ' ������    : ARG1 -
    '
    ' �@�\����  : INI�t�@�C�������w�肵�Đݒ�t�@�C����ǂݍ��݂܂�
    '
    ' ���l      :
    '-----------------------------------------------------------------------------
    Public Function GetProfileString(ByVal asIniName As String, ByVal Section As String, ByVal Entry As String) As String
        Dim strReturn As String = ""
        Dim lstrN As StringBuilder = New StringBuilder(1024)

        GetPrivateProfileString(Section, Entry, "", lstrN, lstrN.Capacity, asIniName)
        strReturn = lstrN.ToString()

        Return strReturn
    End Function
#End Region
#Region "�ݒ�t�@�C���ǂݍ���(�w��Z�N�V�����̃L�[�̈ꗗ�𓾂�)"
    '-----------------------------------------------------------------------------
    ' @(n)
    '
    ' �֐���    : GetProfileStringKey
    '
    ' �@�\      : �ݒ�t�@�C���ǂݍ���(�w��Z�N�V�����̃L�[�̈ꗗ�𓾂�)
    '
    ' ������    : ARG1 -
    '
    ' �@�\����  : INI�t�@�C�������w�肵�Đݒ�t�@�C����ǂݍ��݂܂�(�w��Z�N�V�����̃L�[�̈ꗗ�𓾂�)
    '
    ' ���l      :","��؂�Ŏ擾����
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
#Region "��X�y�[�X�ҏW"
    '-----------------------------------------------------------------------------
    ' @(n)
    '
    ' �@�\�@�@  : ��X�y�[�X���ߏ���
    '
    ' �Ԃ�l�@  : ������(�X�y�[�X����)
    '
    ' �������@  : ������[strChara],�o�C�g��[iByteSize]
    '
    ' �@�\����  : �n���ꂽ��������A�o�C�g���Ō�X�y�[�X�ҏW���A�l��߂�
    '
    ' ���l�@�@  :
    '
    '-----------------------------------------------------------------------------
    Public Function SetAfterSpace(ByVal strChara As String, ByVal iByteSize As Integer) As String
        Dim strReturn As String = strChara

        strReturn = SetStringByte(strChara, iByteSize, False, " ")

        Return strReturn

    End Function
#End Region
#Region "�O�[���ҏW"
    '-----------------------------------------------------------------------------
    ' @(n)
    '
    ' �@�\�@�@  : �O�[�����ߏ���
    '
    ' �Ԃ�l�@  : ������(�[������)
    '
    ' �������@  : ������[strChara],�o�C�g��[iByteSize]
    '
    ' �@�\����  : �n���ꂽ��������A�o�C�g���őO�[���ҏW���A�l��߂�
    '
    ' ���l�@�@  :
    '
    '-----------------------------------------------------------------------------
    Public Function SetBeforeZero(ByVal strChara As String, ByVal iByteSize As Integer) As String
        Dim strReturn As String = strChara

        strReturn = SetStringByte(strChara, iByteSize, True, "0")


        Return strReturn
    End Function
#End Region
#Region "������ҏW"
    '-----------------------------------------------------------------------------
    ' @(n)
    '
    ' �@�\�@�@  : ������ҏW
    '
    ' �Ԃ�l�@  : ������
    '
    ' �������@  : ������[strChara],�o�C�g��[iByteSize]
    '�@�@�@�@�@�@�@�O�ɉ����邩��ɉ����邩[bBefore],�����镶��[AddString]
    '
    ' �@�\����  : 
    '
    ' ���l�@�@  :
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
#Region "�����񂪋󔒂ł���΁A0��Ԃ�"
    '-----------------------------------------------------------------------------
    ' @(n)
    '
    ' �@�\�@�@  : ���l�f�[�^�擾
    '
    ' �Ԃ�l�@  : ������
    '
    ' �������@  : ������[strChara]
    '
    ' �@�\����  : �^����ꂽ�����񂪋󔒂ł���΁A0��Ԃ�
    '
    ' ���l�@�@  :
    '
    '-----------------------------------------------------------------------------
    Public Function SetNullToZero(ByVal strChara As String) As String
        Dim strReturn As String = strChara

        If String.IsNullOrEmpty(strReturn) Then strReturn = "0"

        Return strReturn
    End Function
#End Region
#Region "String��Long�ɕϊ�"
	'-----------------------------------------------------------------------------
	' @(n)
	'
	' �@�\�@�@  : String��Long�ɕϊ�
	'
	' �Ԃ�l�@  : Long
	'
	' �������@  : ������[strChara]
	'
	' �@�\����  :  String��Long�ɕϊ��ł��Ȃ���΁A0��Ԃ�
	'
	' ���l�@�@  :
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
#Region "String��Integer�ɕϊ�"
    '-----------------------------------------------------------------------------
    ' @(n)
    '
    ' �@�\�@�@  : String��Integer�ɕϊ�
    '
    ' �Ԃ�l�@  : Integer
    '
    ' �������@  : ������[strChara]
    '
    ' �@�\����  :  String��Integer�ɕϊ��ł��Ȃ���΁A0��Ԃ�
    '
    ' ���l�@�@  :
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
#Region "String��Short�ɕϊ�"
    '-----------------------------------------------------------------------------
    ' @(n)
    '
    ' �@�\�@�@  : String��Short�ɕϊ�
    '
    ' �Ԃ�l�@  : Short
    '
    ' �������@  : ������[strChara]
    '
    ' �@�\����  :  String��Integer�ɕϊ��ł��Ȃ���΁A0��Ԃ�
    '
    ' ���l�@�@  :
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
#Region "������̎w�肳�ꂽ�o�C�g�ʒu�ȍ~�̂��ׂ�(�܂��͎w�肵���T�C�Y)�̕������Ԃ��܂��B"
    '-----------------------------------------------------------------------------
    ' @(n)
    '
    ' �@�\�@�@  : ������̎w�肳�ꂽ�o�C�g�ʒu�ȍ~�̂��ׂĂ̕������Ԃ��܂��B
    '
    ' �Ԃ�l�@  : �w�肳�ꂽ�o�C�g�ʒu�ȍ~�̂��ׂĂ̕�����B
    '
    ' �������@  : stTarget    ���o�����ɂȂ镶����B
    '            iStart      ���o�����J�n����ʒu�B
    ' �@�\����  : VS2005�ɕϊ��Ŏg�p
    '
    ' ���l�@�@  :
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
#Region "������̎w�肳�ꂽ������̃o�C�g����Ԃ��܂��B"
    '-----------------------------------------------------------------------------
    ' @(n)
    '
    ' �@�\�@�@  : ������̎w�肳�ꂽ������̃o�C�g����Ԃ��܂��B
    '
    ' �Ԃ�l�@  : �w�肳�ꂽ������̃o�C�g��
    '
    ' �������@  : stTarget    ������B
    ' �@�\����  : VS2005�ɕϊ��Ŏg�p
    '
    ' ���l�@�@  :
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
#Region "������̍��[����w�肵���o�C�g�����̕������Ԃ��܂��B"
    '-----------------------------------------------------------------------------
    ' @(n)
    '
    ' �@�\�@�@  : ������̍��[����w�肵���o�C�g�����̕������Ԃ��܂��B
    '
    ' �Ԃ�l�@  : ���[����w�肳�ꂽ�o�C�g�����̕�����B
    '
    ' �������@  : stTarget    ���o�����ɂȂ镶����B
    '            iByteSize   ���o���o�C�g���B
    ' �@�\����  : VS2005�ɕϊ��Ŏg�p
    '
    ' ���l�@�@  :
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
#Region "������̉E�[����w�肵���o�C�g�����̕������Ԃ��܂��B"
    '-----------------------------------------------------------------------------
    ' @(n)
    '
    ' �@�\�@�@  : ������̉E�[����w�肵���o�C�g�����̕������Ԃ��܂��B
    '
    ' �Ԃ�l�@  : �E�[����w�肳�ꂽ�o�C�g�����̕�����B
    '
    ' �������@  : stTarget    ���o�����ɂȂ镶����B
    '            iByteSize   ���o���o�C�g���B
    ' �@�\����  : VS2005�ɕϊ��Ŏg�p
    '
    ' ���l�@�@  :
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
                iStart = UBound(bBytes) + 1 - iByteSize '�o�C�g��
                bResult = hEncoding.GetString(bBytes, iStart, iByteSize)
                bResult = bResult.Trim
            End If
        End If
        Return bResult
    End Function
#End Region
#End Region
End Module
