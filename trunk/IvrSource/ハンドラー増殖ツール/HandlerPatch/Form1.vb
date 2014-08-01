Imports System.IO
Imports System.Text

Public Class Form1

    Private _assemblyPath As String = ""
    Private _encode As Encoding

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        _assemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location
        'shift_jis=932
        _encode = Encoding.GetEncoding(Convert.ToInt32("932"))
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        If CheckBox1.Checked = False _
            And CheckBox2.Checked = False _
            And CheckBox3.Checked = False Then
            MessageBox.Show("�쐬���W���[�������`�F�b�N���Ă�������", "�G���[", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return

        End If

        Dim checkEnv() As String = New String(2) {}
        Dim icnt As Integer
        Dim icnt2 As Integer = 0

        For icnt = 1 To 3

            If icnt = 1 Then
                If CheckBox1.Checked = True Then
                    checkEnv(icnt2) = CheckBox1.Text
                    icnt2 += 1
                End If
            ElseIf icnt = 2 Then
                If CheckBox2.Checked = True Then
                    checkEnv(icnt2) = CheckBox2.Text
                    icnt2 += 1
                End If
            ElseIf icnt = 3 Then
                If CheckBox3.Checked = True Then
                    checkEnv(icnt2) = CheckBox3.Text
                    icnt2 += 1
                End If
            End If

        Next

        '------------------------------
        '�t�@�C���I���_�C�A���O�̕\��
        '------------------------------
        'Dim ofd As New OpenFileDialog()
        'ofd.InitialDirectory = System.IO.Path.GetDirectoryName(_assemblyPath)
        'ofd.FileName = "*.ihd"
        'ofd.Filter = "handler�t�@�C��(*.ihd)|*.ihd|���ׂẴt�@�C��(*.*)|*.*"
        'ofd.FilterIndex = 1
        'ofd.Title = "handler�t�@�C����I�����Ă�������"
        'ofd.RestoreDirectory = True

        '�_�C�A���O��\������
        'If Not ofd.ShowDialog() = DialogResult.OK Then
        '    Return
        'End If
        'Dim handlerFile As String = ofd.FileName
        'Dim backupFile As String = handlerFile & "_bak"

        '���s�m�F
        'Dim dRslt As DialogResult = MessageBox.Show("�p�b�`�ύX���܂��B" & ControlChars.NewLine & _
        '                                            "��낵���ł����H" & ControlChars.NewLine & _
        '                                            "(��ꂽ�炲�߂�Ȃ������)", "�m�F", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)
        'If dRslt = Windows.Forms.DialogResult.Cancel Then
        'Return
        'End If

        Dim obd As New FolderBrowserDialog()
        obd.Description = "Handler�̃R�s�[���t�H���_��I�����Ă�������"
        obd.RootFolder = Environment.SpecialFolder.Desktop
        'obd.RootFolder = My.Application.Info.DirectoryPath.ToString()
        'obd.SelectedPath = "C:\"
        obd.SelectedPath = My.Application.Info.DirectoryPath.ToString()
        'obd.ShowNewFolderButton = True
        obd.ShowNewFolderButton = False

        If Not obd.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Return
        End If

        Dim Path As String = obd.SelectedPath
        Dim Files As String() = Directory.GetFiles(Path, "*.ihd")

        '���s�m�F
        Dim dRslt As DialogResult = MessageBox.Show("�p�b�`�ύX���܂��B" & ControlChars.NewLine & _
                                                    "��낵���ł����H" & ControlChars.NewLine & _
                                                    "(��ꂽ�炲�߂�Ȃ������)", "�m�F", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)

        Dim dtNow As DateTime = DateTime.Now
        If dRslt = Windows.Forms.DialogResult.Cancel Then
            Return
        End If

        For j As Integer = 0 To 2
            If Not checkEnv(j) = "" Then

                For i As Integer = 0 To Files.Length - 1
                    Dim handlerFile As String = Files(i)
                    Dim File As String = Dir(handlerFile)
                    'Dim backupFile As String = handlerFile & "_bak"
                    Dim envStr As String = checkEnv(j) & "_"
                    Dim nFile As String = Strings.Left(File, 3) & envStr & Strings.Mid(File, 4, File.Length)
                    Dim newFile As String = Path & "\" & checkEnv(j) & "\" & nFile

                    ' �t�H���_ (�f�B���N�g��) �����݂��Ă��邩�ǂ����m�F����
                    If Not System.IO.Directory.Exists(Path & "\" & checkEnv(j)) Then
                        ' �t�H���_ (�f�B���N�g��) ���쐬����
                        System.IO.Directory.CreateDirectory(Path & "\" & checkEnv(j))
                    Else
                        ' �t�@�C�������݂��Ă��邩�ǂ����m�F����
                        If System.IO.File.Exists(newFile) Then
                            Dim sYear As String = dtNow.Year.ToString
                            Dim sMonth1 As String = dtNow.Month.ToString
                            Dim sMonth As String
                            Dim sDay1 As String = dtNow.Day.ToString
                            Dim sDay As String

                            Dim sHour1 As String = dtNow.Hour.ToString
                            Dim sHour As String
                            Dim sMin1 As String = dtNow.Minute.ToString
                            Dim sMin As String
                            Dim sSec1 As String = dtNow.Second.ToString
                            Dim sSec As String
                            Dim sMill1 As String = dtNow.Millisecond.ToString
                            Dim sMill As String

                            If sMonth1.Length = 1 Then
                                sMonth = "0" & sMonth1
                            Else
                                sMonth = sMonth1
                            End If
                            If sDay1.Length = 1 Then
                                sDay = "0" & sDay1
                            Else
                                sDay = sDay1
                            End If

                            If sHour1.Length = 1 Then
                                sHour = "0" & sHour1
                            Else
                                sHour = sHour1
                            End If
                            If sMin1.Length = 1 Then
                                sMin = "0" & sMin1
                            Else
                                sMin = sMin1
                            End If
                            If sSec1.Length = 1 Then
                                sSec = "0" & sSec1
                            Else
                                sSec = sSec1
                            End If
                            If sMill1.Length = 1 Then
                                sMill = "00" & sMill1
                            ElseIf sMill1.Length = 2 Then
                                sMill = "0" & sMill1
                            Else
                                sMill = sMill1
                            End If
                            Dim sDateTime As String = sYear & sMonth & sDay & sHour & sMin & sSec & sMill

                            Dim nPath As String = Path & "\" & checkEnv(j) & "\" & sDateTime

                            ' �t�H���_ (�f�B���N�g��) ���쐬����
                            System.IO.Directory.CreateDirectory(nPath)
                            ' �t�@�C�����ړ�����
                            System.IO.File.Move(newFile, nPath & "\" & nFile)
                        End If

                    End If

                    Dim fs As FileStream = Nothing
                    Dim fs_write As FileStream = Nothing
                    'Dim stReader As StreamReader = Nothing

                    Try
                        '�t�@�C���o�b�N�A�b�v
                        'FileSystem.Rename(handlerFile, backupFile)

                        '�t�@�C���I�[�v��
                        fs = New FileStream(handlerFile, FileMode.Open, FileAccess.Read)

                        '�ꊇ�Ǎ���
                        Dim bs(fs.Length - 1) As Byte
                        fs.Read(bs, 0, bs.Length)

                        Dim pos As Integer = 0
                        Dim writeBuff(bs.Length + 1000) As Byte
                        Dim writeSize As Integer = 0
                        Do
                            Dim wrkByte(14) As Byte
                            '�Ǎ��݃f�[�^����14�o�C�g��(A.Z._._.H._._.)���擾
                            System.Array.ConstrainedCopy(bs, pos, wrkByte, 0, wrkByte.Length)

                            '�Ǎ��݃f�[�^���uA.Z._.H._.�v�ƈ�v���邩�ǂ������`�F�b�N
                            '(��v�����ꍇ)
                            If MyBinaryCmp2(wrkByte, _encode.GetBytes("AZ_H_")) = True Then
                                '�uA.Z._.H._.�v��1�o�C�g�O�̃n���h���[���T�C�Y���擾
                                Dim size As Byte = bs(pos - 1)
                                '�����݃o�b�t�@�Ƀn���h���[���T�C�Y���Z�b�g
                                writeBuff(writeSize - 1) = size + _encode.GetBytes(envStr).Length
                                '�����݃o�b�t�@�ɁuA.Z._.�v�܂ł��Z�b�g
                                System.Array.ConstrainedCopy(bs, pos, writeBuff, writeSize, (3 * 2))
                                writeSize += (3 * 2)

                                '�t�^������R�[�h��Byte�z��𐶐�
                                Dim workEnvByte((envStr.Length * 2) - 1) As Byte
                                Dim cnt As Integer = 0
                                For n As Integer = 0 To envStr.Length - 1
                                    workEnvByte(cnt) = _encode.GetBytes(envStr)(n)
                                    workEnvByte(cnt + 1) = 0
                                    cnt += 2
                                Next

                                '�����݃o�b�t�@�Ɋ��R�[�h���Z�b�g
                                'System.Array.ConstrainedCopy(_encode.GetBytes(envStr), 0, writeBuff, writeSize, _encode.GetBytes(envStr).Length)
                                System.Array.ConstrainedCopy(workEnvByte, 0, writeBuff, writeSize, workEnvByte.Length)
                                writeSize += workEnvByte.Length

                                '�����݃o�b�t�@�ɁuH_�`�v�ȍ~���Z�b�g
                                System.Array.ConstrainedCopy(bs, pos + (3 * 2), writeBuff, writeSize, (size * 2) - (3 * 2))
                                writeSize = writeSize + ((size * 2) - (3 * 2))

                                '�Ǎ��݃f�[�^�ʒu��i�߂�
                                pos += (size * 2)

                            '�Ǎ��݃f�[�^���uA.Z._._.H._._.�v�ƈ�v���邩�ǂ������`�F�b�N
                            '(��v�����ꍇ)
                            ElseIf MyBinaryCmp2(wrkByte, _encode.GetBytes("AZ__H__")) = True Then
                                Dim envStr2 As String = envStr & "_"

                                '�uA.Z._._.H._._.�v��1�o�C�g�O�̃n���h���[���T�C�Y���擾
                                Dim size As Byte = bs(pos - 1)
                                '�����݃o�b�t�@�Ƀn���h���[���T�C�Y���Z�b�g
                                writeBuff(writeSize - 1) = size + _encode.GetBytes(envStr2).Length
                                '�����݃o�b�t�@�ɁuA.Z._._.�v�܂ł��Z�b�g
                                System.Array.ConstrainedCopy(bs, pos, writeBuff, writeSize, (4 * 2))
                                writeSize += (4 * 2)

                                '�t�^������R�[�h��Byte�z��𐶐�
                                Dim workEnvByte((envStr2.Length * 2) - 1) As Byte
                                Dim cnt As Integer = 0
                                For n As Integer = 0 To envStr2.Length - 1
                                    workEnvByte(cnt) = _encode.GetBytes(envStr2)(n)
                                    workEnvByte(cnt + 1) = 0
                                    cnt += 2
                                Next

                                '�����݃o�b�t�@�Ɋ��R�[�h���Z�b�g
                                'System.Array.ConstrainedCopy(_encode.GetBytes(envStr2), 0, writeBuff, writeSize, _encode.GetBytes(envStr2).Length)
                                System.Array.ConstrainedCopy(workEnvByte, 0, writeBuff, writeSize, workEnvByte.Length)
                                writeSize += workEnvByte.Length

                                '�����݃o�b�t�@�ɁuH__�`�v�ȍ~���Z�b�g
                                System.Array.ConstrainedCopy(bs, pos + (4 * 2), writeBuff, writeSize, (size * 2) - (4 * 2))
                                writeSize = writeSize + ((size * 2) - (4 * 2))

                                '�Ǎ��݃f�[�^�ʒu��i�߂�
                                pos += (size * 2)

                            Else
                                writeBuff(writeSize) = bs(pos)
                                writeSize += 1
                                pos += 1
                            End If

                            '�I������
                            If pos >= bs.Length - wrkByte.Length Then
                                System.Array.ConstrainedCopy(bs, pos, writeBuff, writeSize, wrkByte.Length)
                                writeSize += wrkByte.Length
                                Exit Do
                            End If
                        Loop

                        '�t�@�C���I�[�v��

                        fs_write = New FileStream(newFile, FileMode.CreateNew, FileAccess.Write)
                        fs_write.Write(writeBuff, 0, writeSize)
                        fs_write.Flush()


                    Catch ex As Exception
                        MessageBox.Show(ex.Message, "�G���[", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        'FileSystem.Rename(backupFile, handlerFile)
                    Finally
                        If Not IsNothing(fs) Then
                            fs.Close()
                        End If

                        If Not IsNothing(fs_write) Then
                            fs_write.Close()
                        End If

                    End Try

                Next i

            End If

        Next j

        MessageBox.Show("����ɏI�����܂����B", "�n���h���[�p�b�`", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Return
    End Sub

    Private Function MyBinaryCmp1(ByVal searchByte() As Byte, ByVal trgByte() As Byte) As Boolean
        Dim rslt As Boolean = True
        If searchByte.Length < trgByte.Length Then
            Return False
        End If
        For i As Integer = 0 To trgByte.Length - 1
            If Not trgByte(i) = searchByte(i) Then
                rslt = False
                Exit For
            End If
        Next i
        Return rslt
    End Function

    Private Function MyBinaryCmp2(ByVal searchByte() As Byte, ByVal trgByte() As Byte) As Boolean
        Dim rslt As Boolean = True

        'trgByte()�����H����
        Dim trgByteEdit((trgByte.Length * 2) - 1) As Byte
        Dim cnt As Integer = 0
        For i As Integer = 0 To trgByte.Length - 1
            trgByteEdit(cnt) = trgByte(i)
            trgByteEdit(cnt + 1) = 0
            cnt += 2
        Next

        If searchByte.Length < trgByteEdit.Length Then
            Return False
        End If
        For i As Integer = 0 To trgByteEdit.Length - 1
            If Not trgByteEdit(i) = searchByte(i) Then
                rslt = False
                Exit For
            End If
        Next i
        Return rslt
    End Function

End Class
