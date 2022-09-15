Imports AisParser
Imports AisParser.Messages
Imports GMap.NET
Imports GMap.NET.WindowsForms.Markers
Namespace Models
    Public Class Message
        Public Property Parent As Manager
        Public Property Mmsi As UInt32
        Public Property Lat As Double
        Public Property Lon As Double
        Public Property Origin As String
        Public Property Type As MessageType
        Public Property Marker As GMarkerGoogle
        Public Property History As List(Of PointLatLng)

        Sub New(parent As Manager, message As AisMessage)
            Me.Parent = parent
            Me.History = New List(Of PointLatLng)
            Select Case message.MessageType
                Case AisMessageType.PositionReportClassA
                    Dim m As PositionReportClassAMessage = CType(message, PositionReportClassAMessage)
                    Me.Mmsi = m.Mmsi
                    Me.Lat = m.Latitude
                    Me.Lon = m.Longitude
                    Me.Origin = Me.Parent.Mmsi2Country(m.Mmsi)
                    Me.Type = MessageType.Vessel
                    Me.Marker = New GMarkerGoogle(New PointLatLng(m.Latitude, m.Longitude), GMarkerGoogleType.arrow)
                    Me.Marker.ToolTipText = String.Format("MMSI: {1}{0}Country: {2}{0}LAT: {3}{0}LON: {4}", ControlChars.NewLine, m.Mmsi, Me.Origin, Me.Lat, Me.Lon)
                Case AisMessageType.BaseStationReport
                    Dim m As BaseStationReportMessage = CType(message, BaseStationReportMessage)
                    Me.Mmsi = m.Mmsi
                    Me.Lat = m.Latitude
                    Me.Lon = m.Longitude
                    Me.Origin = Me.Parent.Mmsi2Country(m.Mmsi)
                    Me.Type = MessageType.Station
                    Me.Marker = New GMarkerGoogle(New PointLatLng(m.Latitude, m.Longitude), GMarkerGoogleType.red)
                    Me.Marker.ToolTipText = String.Format("MMSI: {1}{0}Country: {2}{0}LAT: {3}{0}LON: {4}", ControlChars.NewLine, m.Mmsi, Me.Origin, Me.Lat, Me.Lon)
            End Select
        End Sub
    End Class
End Namespace
