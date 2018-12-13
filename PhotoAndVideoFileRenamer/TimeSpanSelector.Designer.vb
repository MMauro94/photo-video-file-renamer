<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TimeSpanSelector
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Hours = New System.Windows.Forms.NumericUpDown()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Days = New System.Windows.Forms.NumericUpDown()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Minutes = New System.Windows.Forms.NumericUpDown()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Seconds = New System.Windows.Forms.NumericUpDown()
        Me.Button1 = New System.Windows.Forms.Button()
        CType(Me.Hours, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Days, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Minutes, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Seconds, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(87, 40)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(35, 13)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "Hours"
        '
        'Hours
        '
        Me.Hours.Location = New System.Drawing.Point(12, 38)
        Me.Hours.Maximum = New Decimal(New Integer() {23, 0, 0, 0})
        Me.Hours.Name = "Hours"
        Me.Hours.Size = New System.Drawing.Size(69, 20)
        Me.Hours.TabIndex = 6
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(87, 14)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(31, 13)
        Me.Label4.TabIndex = 5
        Me.Label4.Text = "Days"
        '
        'Days
        '
        Me.Days.Location = New System.Drawing.Point(12, 12)
        Me.Days.Maximum = New Decimal(New Integer() {999999, 0, 0, 0})
        Me.Days.Name = "Days"
        Me.Days.Size = New System.Drawing.Size(69, 20)
        Me.Days.TabIndex = 4
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(87, 66)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(44, 13)
        Me.Label5.TabIndex = 9
        Me.Label5.Text = "Minutes"
        '
        'Minutes
        '
        Me.Minutes.Location = New System.Drawing.Point(12, 64)
        Me.Minutes.Maximum = New Decimal(New Integer() {59, 0, 0, 0})
        Me.Minutes.Name = "Minutes"
        Me.Minutes.Size = New System.Drawing.Size(69, 20)
        Me.Minutes.TabIndex = 8
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(87, 92)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(49, 13)
        Me.Label6.TabIndex = 11
        Me.Label6.Text = "Seconds"
        '
        'Seconds
        '
        Me.Seconds.Location = New System.Drawing.Point(12, 90)
        Me.Seconds.Maximum = New Decimal(New Integer() {59, 0, 0, 0})
        Me.Seconds.Name = "Seconds"
        Me.Seconds.Size = New System.Drawing.Size(69, 20)
        Me.Seconds.TabIndex = 10
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(12, 116)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(138, 23)
        Me.Button1.TabIndex = 12
        Me.Button1.Text = "OK"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'TimeSpanSelector
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(162, 150)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Seconds)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Minutes)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Hours)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Days)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "TimeSpanSelector"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "TimeSpanSelector"
        CType(Me.Hours, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Days, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Minutes, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Seconds, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Hours As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Days As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Minutes As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Seconds As System.Windows.Forms.NumericUpDown
    Friend WithEvents Button1 As System.Windows.Forms.Button
End Class
