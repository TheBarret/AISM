
Imports System.Text
Imports AISM.Models
Imports AisParser
Imports AisParser.Messages
Imports GMap.NET
Imports GMap.NET.WindowsForms
Imports GMap.NET.WindowsForms.Markers

Public Class Main

    Public Property Manager As Manager

    Private Sub Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Initialize()
    End Sub

    Private Sub Initialize()
        Me.Manager = New Manager("192.168.2.3", 10100)
        Me.Manager.Logger = AddressOf Me.Log
        Me.MapControl.MapProvider = GMap.NET.MapProviders.BingMapProvider.Instance
        Me.MapControl.Manager.Mode = GMap.NET.AccessMode.ServerAndCache
        Me.MapControl.Position = New GMap.NET.PointLatLng(52.084328, 4.266643)
        Me.MapControl.Manager.BoostCacheEngine = True
        Me.MapControl.Zoom = 5
        Me.mapZoom.Value = 5
        Me.MapControl.Overlays.Add(Me.Manager.Overlay)
    End Sub

    Private Sub Main_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Me.Interval.Stop()
        Me.Interval.Dispose()
        Me.Manager.Dispose()
        Me.MapControl.Manager.CancelTileCaching()
    End Sub

    Private Sub mapZoom_ValueChanged(sender As Object, e As EventArgs) Handles mapZoom.ValueChanged
        Me.MapControl.Zoom = Me.mapZoom.Value
    End Sub

    Private Sub MapControl_OnMapZoomChanged() Handles MapControl.OnMapZoomChanged
        Me.mapZoom.Value = CInt(Me.MapControl.Zoom)
    End Sub

    Private Sub MapControl_OnTileLoadStart() Handles MapControl.OnTileLoadStart
        Me.SetMapStatus("Loading tiles...")
    End Sub

    Private Sub MapControl_OnTileLoadComplete(elapsedMilliseconds As Long) Handles MapControl.OnTileLoadComplete
        Me.SetMapStatus(String.Format("Finished in {0}ms", elapsedMilliseconds))
    End Sub

    Private Sub MapControl_OnPositionChanged(point As PointLatLng) Handles MapControl.OnPositionChanged
        Me.SetLatLng(point)
    End Sub

    Private Sub SetLatLng(point As PointLatLng)
        If (Me.InvokeRequired) Then
            Me.Invoke(Sub() Me.SetLatLng(point))
        Else
            Me.txtCoordLat.Text = String.Format("{0}°", point.Lat)
            Me.txtCoordLon.Text = String.Format("{0}°", point.Lng)
        End If
    End Sub

    Private Sub SetMapStatus(message As String)
        If (Me.InvokeRequired) Then
            Me.Invoke(Sub() Me.SetMapStatus(message))
        Else
            Me.MapStatus.Text = message
        End If
    End Sub

    Private Sub Log(message As String)
        If (Me.InvokeRequired) Then
            Me.Invoke(Sub() Me.Log(message))
        Else
            Me.TextLog.AppendText(String.Format("{0}{1}", message, ControlChars.NewLine))
        End If
    End Sub

    Private Sub SocketReader_Tick(sender As Object, e As EventArgs) Handles Interval.Tick
        If (Not Me.Manager.Client.Running) Then
            Me.Manager.Client.Start()
        End If
    End Sub

    Private Sub cmdTimer_Click(sender As Object, e As EventArgs) Handles cmdTimer.Click
        If (Not Me.Interval.Enabled) Then
            Me.Interval.Start()
            Me.cmdTimer.Text = "Stop"
        Else
            Me.Interval.Stop()
            Me.cmdTimer.Text = "Update"
        End If
    End Sub
End Class