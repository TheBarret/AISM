Imports AisParser
Imports AisParser.Messages
Imports GMap.NET
Imports GMap.NET.WindowsForms.Markers

Namespace Models
    Public Class Broadcast
        Public Property Parent As Manager
        Public Property Mmsi As UInt32
        Public Property Lat As Double
        Public Property Lon As Double
        Public Property Origin As String
        Public Property Node As TreeNode
        Public Property Type As MessageType
        Public Property Updates As Integer
        Public Property Last As DateTime
        Public Property Marker As GMarkerGoogle
        Public Property History As History(Of PointLatLng)

        Sub New(parent As Manager, msg As AisMessage)
            Me.Parent = parent
            Me.Last = DateTime.Now
            Me.History = New History(Of PointLatLng)
            Select Case msg.MessageType
                Case AisMessageType.PositionReportClassA
                    Dim m As PositionReportClassAMessage = CType(msg, PositionReportClassAMessage)
                    Me.Mmsi = m.Mmsi
                    Me.Lat = m.Latitude
                    Me.Lon = m.Longitude
                    Me.Origin = Me.Parent.GetCountry(m.Mmsi)
                    Me.Type = MessageType.Vessel
                    Me.Marker = New GMarkerGoogle(New PointLatLng(m.Latitude, m.Longitude), Me.Crosshair)
                    Me.Marker.ToolTipText = String.Format("MMSI: {0}", m.Mmsi)
                    Me.Populate(m.Mmsi)
                Case AisMessageType.BaseStationReport
                    Dim m As BaseStationReportMessage = CType(msg, BaseStationReportMessage)
                    Me.Mmsi = m.Mmsi
                    Me.Lat = m.Latitude
                    Me.Lon = m.Longitude
                    Me.Origin = Me.Parent.GetCountry(m.Mmsi)
                    Me.Type = MessageType.Station
                    Me.Marker = New GMarkerGoogle(New PointLatLng(m.Latitude, m.Longitude), Me.Crosshair)
                    Me.Marker.ToolTipText = String.Format("MMSI: {0}", m.Mmsi)
                    Me.Populate(m.Mmsi)
            End Select
        End Sub

        Public Sub Update()
            Me.Marker.Bitmap = Me.Crosshair
            Me.Node.Nodes(0).Text = String.Format("Lat: {0}", Me.Lat)
            Me.Node.Nodes(1).Text = String.Format("Lon: {0}", Me.Lon)
            Me.Node.Nodes(3).Text = String.Format("History: {0}", Me.History.Count)
            Me.Node.Nodes(4).Text = String.Format("Age: {0}", (DateTime.Now - Me.Last).Duration)
            Me.Node.Nodes(6).Text = String.Format("Distance: {0}km", Me.Parent.Base.Distance(Me.Marker.Position, True))
        End Sub

        Public Sub Populate(mmsi As UInt32)
            Me.Node = New TreeNode(mmsi.ToString)
            Me.Node.Nodes.Add(String.Format("Lat: {0}", Me.Lat))
            Me.Node.Nodes.Add(String.Format("Lon: {0}", Me.Lon))
            Me.Node.Nodes.Add(String.Format("Country: {0}", Me.Origin))
            Me.Node.Nodes.Add(String.Format("History: {0}", Me.History.Count))
            Me.Node.Nodes.Add(String.Format("Age: {0}", (DateTime.Now - Me.Last).Duration))
            Me.Node.Nodes.Add(String.Format("Type: {0}", Me.Type))
            Me.Node.Nodes.Add(String.Format("Distance: {0}Km", Me.Parent.Base.Distance(Me.Marker.Position, True)))
        End Sub

        Public ReadOnly Property Crosshair() As Bitmap
            Get
                Dim color As Color = Color.Red
                Select Case (DateTime.Now - Me.Last).Duration.TotalSeconds
                    Case >= 60 : color = Color.Gray
                    Case >= 30 : color = Color.Red
                    Case < 30 : color = Color.Green
                End Select
                Dim bm As New Bitmap(32, 32)
                Using g As Graphics = Graphics.FromImage(bm)
                    g.InterpolationMode = Drawing2D.InterpolationMode.High
                    g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
                    g.Clear(Color.Transparent)
                    Broadcast.Draw(g, New Rectangle(0, 0, 32, 32), color, 5)
                    Broadcast.Draw(g, New Rectangle(0, 0, 32, 32), color, 10)
                End Using
                Return bm
            End Get
        End Property

        Public ReadOnly Property Coordinates() As PointLatLng
            Get
                Return New PointLatLng(Me.Lat, Me.Lon)
            End Get
        End Property

        Public Shared Sub Draw(g As Graphics, base As Rectangle, color As Color, Optional radius As Single = 10)
            Dim center As New PointF(base.Width / 2.0F, base.Height / 2.0F)
            Dim origin As New PointF(center.X - radius, center.Y - radius)
            Dim rectr As New RectangleF(origin, New SizeF(radius * 2, radius * 2))
            Using p As New Pen(color, 2)
                p.DashStyle = Drawing2D.DashStyle.Dot
                g.DrawEllipse(p, rectr)
            End Using
        End Sub
    End Class
End Namespace
