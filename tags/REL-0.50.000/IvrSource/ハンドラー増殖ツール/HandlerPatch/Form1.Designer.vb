<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows フォーム デザイナで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
    'Windows フォーム デザイナを使用して変更できます。  
    'コード エディタを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
Me.Button1 = New System.Windows.Forms.Button
Me.CheckBox1 = New System.Windows.Forms.CheckBox
Me.CheckBox2 = New System.Windows.Forms.CheckBox
Me.CheckBox3 = New System.Windows.Forms.CheckBox
Me.Label1 = New System.Windows.Forms.Label
Me.Label2 = New System.Windows.Forms.Label
Me.Label3 = New System.Windows.Forms.Label
Me.Label4 = New System.Windows.Forms.Label
Me.SuspendLayout()
'
'Button1
'
Me.Button1.Location = New System.Drawing.Point(166, 77)
Me.Button1.Name = "Button1"
Me.Button1.Size = New System.Drawing.Size(98, 23)
Me.Button1.TabIndex = 0
Me.Button1.Text = "モジュール生成"
Me.Button1.UseVisualStyleBackColor = True
'
'CheckBox1
'
Me.CheckBox1.AutoSize = True
Me.CheckBox1.Location = New System.Drawing.Point(39, 33)
Me.CheckBox1.Name = "CheckBox1"
Me.CheckBox1.Size = New System.Drawing.Size(39, 16)
Me.CheckBox1.TabIndex = 5
Me.CheckBox1.Text = "UT"
Me.CheckBox1.UseVisualStyleBackColor = True
'
'CheckBox2
'
Me.CheckBox2.AutoSize = True
Me.CheckBox2.Location = New System.Drawing.Point(39, 55)
Me.CheckBox2.Name = "CheckBox2"
Me.CheckBox2.Size = New System.Drawing.Size(39, 16)
Me.CheckBox2.TabIndex = 2
Me.CheckBox2.Text = "TR"
Me.CheckBox2.UseVisualStyleBackColor = True
'
'CheckBox3
'
Me.CheckBox3.AutoSize = True
Me.CheckBox3.Location = New System.Drawing.Point(39, 77)
Me.CheckBox3.Name = "CheckBox3"
Me.CheckBox3.Size = New System.Drawing.Size(52, 16)
Me.CheckBox3.TabIndex = 3
Me.CheckBox3.Text = "TEST"
Me.CheckBox3.UseVisualStyleBackColor = True
'
'Label1
'
Me.Label1.AutoSize = True
Me.Label1.Location = New System.Drawing.Point(22, 9)
Me.Label1.Name = "Label1"
Me.Label1.Size = New System.Drawing.Size(100, 12)
Me.Label1.TabIndex = 6
Me.Label1.Text = "作成モジュール環境"
'
'Label2
'
Me.Label2.AutoSize = True
Me.Label2.Location = New System.Drawing.Point(97, 36)
Me.Label2.Name = "Label2"
Me.Label2.Size = New System.Drawing.Size(29, 12)
Me.Label2.TabIndex = 7
Me.Label2.Text = "環境"
'
'Label3
'
Me.Label3.AutoSize = True
Me.Label3.Location = New System.Drawing.Point(97, 57)
Me.Label3.Name = "Label3"
Me.Label3.Size = New System.Drawing.Size(29, 12)
Me.Label3.TabIndex = 8
Me.Label3.Text = "環境"
'
'Label4
'
Me.Label4.AutoSize = True
Me.Label4.Location = New System.Drawing.Point(97, 78)
Me.Label4.Name = "Label4"
Me.Label4.Size = New System.Drawing.Size(29, 12)
Me.Label4.TabIndex = 9
Me.Label4.Text = "環境"
'
'Form1
'
Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
Me.ClientSize = New System.Drawing.Size(339, 112)
Me.Controls.Add(Me.Label4)
Me.Controls.Add(Me.Label3)
Me.Controls.Add(Me.Label2)
Me.Controls.Add(Me.Label1)
Me.Controls.Add(Me.CheckBox1)
Me.Controls.Add(Me.CheckBox2)
Me.Controls.Add(Me.CheckBox3)
Me.Controls.Add(Me.Button1)
Me.Name = "Form1"
Me.Text = "あおぞら銀行Handler作成ツール"
Me.ResumeLayout(False)
Me.PerformLayout()

End Sub
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents CheckBox2 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox3 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label

End Class
