<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.ProjectToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenProjectToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SaveProjectToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CloseProjectToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.extensionCheck = New System.Windows.Forms.ToolStripMenuItem()
        Me.code = New System.Windows.Forms.TextBox()
        Me.OpenFile = New System.Windows.Forms.OpenFileDialog()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ProjectToolStripMenuItem, Me.ToolStripMenuItem1, Me.extensionCheck})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(921, 24)
        Me.MenuStrip1.TabIndex = 0
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'ProjectToolStripMenuItem
        '
        Me.ProjectToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OpenProjectToolStripMenuItem, Me.SaveProjectToolStripMenuItem, Me.CloseProjectToolStripMenuItem})
        Me.ProjectToolStripMenuItem.Name = "ProjectToolStripMenuItem"
        Me.ProjectToolStripMenuItem.Size = New System.Drawing.Size(56, 20)
        Me.ProjectToolStripMenuItem.Text = "Project"
        '
        'OpenProjectToolStripMenuItem
        '
        Me.OpenProjectToolStripMenuItem.Name = "OpenProjectToolStripMenuItem"
        Me.OpenProjectToolStripMenuItem.Size = New System.Drawing.Size(143, 22)
        Me.OpenProjectToolStripMenuItem.Text = "Open Project"
        '
        'SaveProjectToolStripMenuItem
        '
        Me.SaveProjectToolStripMenuItem.Name = "SaveProjectToolStripMenuItem"
        Me.SaveProjectToolStripMenuItem.Size = New System.Drawing.Size(143, 22)
        Me.SaveProjectToolStripMenuItem.Text = "Save Project"
        '
        'CloseProjectToolStripMenuItem
        '
        Me.CloseProjectToolStripMenuItem.Name = "CloseProjectToolStripMenuItem"
        Me.CloseProjectToolStripMenuItem.Size = New System.Drawing.Size(143, 22)
        Me.CloseProjectToolStripMenuItem.Text = "Close Project"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Image = CType(resources.GetObject("ToolStripMenuItem1.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(59, 20)
        Me.ToolStripMenuItem1.Text = "Start"
        '
        'extensionCheck
        '
        Me.extensionCheck.Name = "extensionCheck"
        Me.extensionCheck.Size = New System.Drawing.Size(28, 20)
        Me.extensionCheck.Text = "   "
        '
        'code
        '
        Me.code.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.code.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.code.Location = New System.Drawing.Point(12, 28)
        Me.code.Multiline = True
        Me.code.Name = "code"
        Me.code.Size = New System.Drawing.Size(897, 438)
        Me.code.TabIndex = 1
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(921, 478)
        Me.Controls.Add(Me.code)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "Form1"
        Me.Text = "Gooder Studio"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents ProjectToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OpenProjectToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CloseProjectToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SaveProjectToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents code As TextBox
    Friend WithEvents OpenFile As OpenFileDialog
    Friend WithEvents ToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents extensionCheck As ToolStripMenuItem
End Class
