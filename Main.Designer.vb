<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Main
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
        Me.components = New System.ComponentModel.Container()
        Me.MapControl = New GMap.NET.WindowsForms.GMapControl()
        Me.mapZoom = New System.Windows.Forms.TrackBar()
        Me.TextLog = New System.Windows.Forms.TextBox()
        Me.txtCoordLat = New System.Windows.Forms.TextBox()
        Me.txtCoordLon = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Interval = New System.Windows.Forms.Timer(Me.components)
        Me.MapStatus = New System.Windows.Forms.TextBox()
        Me.cmdTimer = New System.Windows.Forms.Button()
        Me.Lview = New System.Windows.Forms.ListView()
        Me.tbHost = New System.Windows.Forms.TextBox()
        Me.tbPort = New System.Windows.Forms.TextBox()
        CType(Me.mapZoom, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'MapControl
        '
        Me.MapControl.Bearing = 0!
        Me.MapControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.MapControl.CanDragMap = True
        Me.MapControl.EmptyTileColor = System.Drawing.Color.White
        Me.MapControl.GrayScaleMode = True
        Me.MapControl.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow
        Me.MapControl.LevelsKeepInMemory = 5
        Me.MapControl.Location = New System.Drawing.Point(12, 12)
        Me.MapControl.MarkersEnabled = True
        Me.MapControl.MaxZoom = 18
        Me.MapControl.MinZoom = 2
        Me.MapControl.MouseWheelZoomEnabled = True
        Me.MapControl.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionWithoutCenter
        Me.MapControl.Name = "MapControl"
        Me.MapControl.NegativeMode = False
        Me.MapControl.PolygonsEnabled = True
        Me.MapControl.RetryLoadTile = 0
        Me.MapControl.RoutesEnabled = False
        Me.MapControl.ScaleMode = GMap.NET.WindowsForms.ScaleModes.Fractional
        Me.MapControl.SelectedAreaFillColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(65, Byte), Integer), CType(CType(105, Byte), Integer), CType(CType(225, Byte), Integer))
        Me.MapControl.ShowTileGridLines = False
        Me.MapControl.Size = New System.Drawing.Size(700, 488)
        Me.MapControl.TabIndex = 0
        Me.MapControl.Zoom = 5.0R
        '
        'mapZoom
        '
        Me.mapZoom.Location = New System.Drawing.Point(718, 97)
        Me.mapZoom.Maximum = 20
        Me.mapZoom.Name = "mapZoom"
        Me.mapZoom.Size = New System.Drawing.Size(279, 45)
        Me.mapZoom.TabIndex = 1
        Me.mapZoom.TickStyle = System.Windows.Forms.TickStyle.Both
        Me.mapZoom.Value = 5
        '
        'TextLog
        '
        Me.TextLog.Font = New System.Drawing.Font("Consolas", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.TextLog.Location = New System.Drawing.Point(12, 532)
        Me.TextLog.Multiline = True
        Me.TextLog.Name = "TextLog"
        Me.TextLog.ReadOnly = True
        Me.TextLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TextLog.Size = New System.Drawing.Size(700, 67)
        Me.TextLog.TabIndex = 2
        '
        'txtCoordLat
        '
        Me.txtCoordLat.Font = New System.Drawing.Font("Consolas", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.txtCoordLat.Location = New System.Drawing.Point(718, 30)
        Me.txtCoordLat.Name = "txtCoordLat"
        Me.txtCoordLat.ReadOnly = True
        Me.txtCoordLat.Size = New System.Drawing.Size(279, 20)
        Me.txtCoordLat.TabIndex = 3
        '
        'txtCoordLon
        '
        Me.txtCoordLon.Font = New System.Drawing.Font("Consolas", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.txtCoordLon.Location = New System.Drawing.Point(718, 71)
        Me.txtCoordLon.Name = "txtCoordLon"
        Me.txtCoordLon.ReadOnly = True
        Me.txtCoordLon.Size = New System.Drawing.Size(279, 20)
        Me.txtCoordLon.TabIndex = 4
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(715, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(50, 15)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Latitude"
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(716, 53)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(61, 15)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "Longitude"
        '
        'Interval
        '
        Me.Interval.Interval = 5000
        '
        'MapStatus
        '
        Me.MapStatus.Font = New System.Drawing.Font("Consolas", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.MapStatus.Location = New System.Drawing.Point(12, 506)
        Me.MapStatus.Name = "MapStatus"
        Me.MapStatus.ReadOnly = True
        Me.MapStatus.Size = New System.Drawing.Size(700, 20)
        Me.MapStatus.TabIndex = 7
        '
        'cmdTimer
        '
        Me.cmdTimer.Location = New System.Drawing.Point(718, 188)
        Me.cmdTimer.Name = "cmdTimer"
        Me.cmdTimer.Size = New System.Drawing.Size(279, 29)
        Me.cmdTimer.TabIndex = 8
        Me.cmdTimer.Text = "Update"
        Me.cmdTimer.UseVisualStyleBackColor = True
        '
        'Lview
        '
        Me.Lview.HideSelection = False
        Me.Lview.Location = New System.Drawing.Point(718, 223)
        Me.Lview.Name = "Lview"
        Me.Lview.Size = New System.Drawing.Size(279, 376)
        Me.Lview.TabIndex = 9
        Me.Lview.UseCompatibleStateImageBehavior = False
        '
        'tbHost
        '
        Me.tbHost.Enabled = False
        Me.tbHost.Location = New System.Drawing.Point(718, 159)
        Me.tbHost.Name = "tbHost"
        Me.tbHost.Size = New System.Drawing.Size(187, 23)
        Me.tbHost.TabIndex = 10
        Me.tbHost.Text = "192.168.2.3"
        '
        'tbPort
        '
        Me.tbPort.Enabled = False
        Me.tbPort.Location = New System.Drawing.Point(909, 159)
        Me.tbPort.Name = "tbPort"
        Me.tbPort.Size = New System.Drawing.Size(88, 23)
        Me.tbPort.TabIndex = 11
        Me.tbPort.Text = "10100"
        '
        'Main
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1009, 611)
        Me.Controls.Add(Me.tbPort)
        Me.Controls.Add(Me.tbHost)
        Me.Controls.Add(Me.Lview)
        Me.Controls.Add(Me.cmdTimer)
        Me.Controls.Add(Me.MapStatus)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtCoordLon)
        Me.Controls.Add(Me.txtCoordLat)
        Me.Controls.Add(Me.TextLog)
        Me.Controls.Add(Me.mapZoom)
        Me.Controls.Add(Me.MapControl)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Main"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "AISM"
        CType(Me.mapZoom, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents MapControl As GMap.NET.WindowsForms.GMapControl
    Friend WithEvents mapZoom As TrackBar
    Friend WithEvents TextLog As TextBox
    Friend WithEvents txtCoordLat As TextBox
    Friend WithEvents txtCoordLon As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Interval As Timer
    Friend WithEvents MapStatus As TextBox
    Friend WithEvents cmdTimer As Button
    Friend WithEvents Lview As ListView
    Friend WithEvents tbHost As TextBox
    Friend WithEvents tbPort As TextBox
End Class
