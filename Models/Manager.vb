Imports System.Text
Imports AisParser
Imports AisParser.Messages
Imports GMap.NET
Imports GMap.NET.WindowsForms

Namespace Models
    Public Class Manager
        Implements IDisposable
        Public Property Client As Client
        Public Property Base As PointLatLng
        Public Property Overlay As GMapOverlay
        Public Property Logger As Action(Of String)
        Public Property Countries As Dictionary(Of Int32, String)
        Public Property Tracking As Dictionary(Of UInt32, Broadcast)
        Private Property Disposed As Boolean

        Public Event Update(type As MessageType, msg As Broadcast, seen As Boolean)

        Sub New(host As String, port As Integer, base As PointLatLng)
            Me.Base = base
            Me.Initialize()
            Me.Prepare(host, port)
        End Sub

        Private Sub Initialize()
            Me.Overlay = New GMapOverlay("AIS")
            Me.Tracking = New Dictionary(Of UInteger, Broadcast)
            Me.Countries = New Dictionary(Of Int32, String)
            Dim data As String = My.Resources.codes
            For Each line As String In data.Split(ControlChars.NewLine)
                Dim index As Integer = line.Str2int(0, 3)
                Dim origin As String = line.Substring(3).Trim
                Me.Countries.Add(index, origin)
            Next
        End Sub

        Private Sub Prepare(host As String, port As Integer)
            Me.Client = New Client(Me, host, port)
            AddHandler Me.Client.OnDataReceived, AddressOf Me.OnDataReceived
        End Sub

        Private Sub OnDataReceived(buffer() As Byte)
            SyncLock Me.Tracking
                Me.Process(buffer)
                Me.Client.Close()
            End SyncLock
        End Sub

        Private Sub Process(buffer() As Byte)
            Dim value As String = String.Empty
            Static AIS As New AisParser.Parser
            value = UTF8Encoding.ASCII.GetString(buffer)
            If (value.Contains(ControlChars.NewLine)) Then
                For Each line As String In value.Split(ControlChars.NewLine)
                    If (line.Length > 0) Then
                        Me.Assign(AIS.Parse(line))
                    End If
                Next
            Else
                Me.Assign(AIS.Parse(value))
            End If
        End Sub

        Private Sub Assign(message As AisMessage)
            If (message IsNot Nothing) Then
                Select Case message.MessageType
                    Case AisMessageType.PositionReportClassA
                        If (Me.Tracking.ContainsKey(message.Mmsi)) Then
                            Dim m As PositionReportClassAMessage = CType(message, PositionReportClassAMessage)
                            If (Me.Tracking(message.Mmsi).Coordinates.Distance(m.Coordinates) > 1) Then
                                Me.Tracking(message.Mmsi).History.Push(Me.Tracking(message.Mmsi).Coordinates)
                            End If
                            Me.Tracking(message.Mmsi).Lat = m.Latitude
                            Me.Tracking(message.Mmsi).Lon = m.Longitude
                            Me.Tracking(message.Mmsi).Last = DateTime.Now
                            RaiseEvent Update(MessageType.Vessel, Me.Tracking(message.Mmsi), True)
                        Else
                            Me.Tracking.Add(message.Mmsi, New Broadcast(Me, message))
                            Me.Overlay.Markers.Add(Me.Tracking(message.Mmsi).Marker)
                            Me.Tracking(message.Mmsi).History.Push(Me.Tracking(message.Mmsi).Coordinates)
                            RaiseEvent Update(MessageType.Vessel, Me.Tracking(message.Mmsi), False)
                        End If
                    Case AisMessageType.BaseStationReport
                        If (Me.Tracking.ContainsKey(message.Mmsi)) Then
                            Dim m As BaseStationReportMessage = CType(message, BaseStationReportMessage)
                            If (Me.Tracking(message.Mmsi).Coordinates.Distance(m.Coordinates) > 1) Then
                                Me.Tracking(message.Mmsi).History.Push(Me.Tracking(message.Mmsi).Coordinates)
                            End If
                            Me.Tracking(message.Mmsi).Lat = m.Latitude
                            Me.Tracking(message.Mmsi).Lon = m.Longitude
                            Me.Tracking(message.Mmsi).Last = DateTime.Now
                            RaiseEvent Update(MessageType.Station, Me.Tracking(message.Mmsi), True)
                        Else
                            Me.Tracking.Add(message.Mmsi, New Broadcast(Me, message))
                            Me.Overlay.Markers.Add(Me.Tracking(message.Mmsi).Marker)
                            Me.Tracking(message.Mmsi).History.Push(Me.Tracking(message.Mmsi).Coordinates)
                            RaiseEvent Update(MessageType.Station, Me.Tracking(message.Mmsi), False)
                        End If
                End Select
            End If
        End Sub

        Public Sub UpdateNodes()
            SyncLock Me.Tracking
                For Each entry As KeyValuePair(Of UInt32, Broadcast) In Me.Tracking
                    entry.Value.Update()
                Next
            End SyncLock
        End Sub

        Public Sub Log(message As String)
            If (Me.Logger IsNot Nothing) Then
                Me.Logger.Invoke(message)
            End If
        End Sub

        Public Sub Log(message As String, ParamArray args() As Object)
            If (Me.Logger IsNot Nothing) Then
                Me.Logger.Invoke(String.Format(message, args))
            End If
        End Sub

        Public Sub FetchUpdate()
            If (Not Me.Running) Then
                Me.Client.Start()
            End If
        End Sub

        Public ReadOnly Property Running As Boolean
            Get
                Return Me.Client.Running
            End Get
        End Property

        Public ReadOnly Property GetCountry(mmsi As UInt32) As String
            Get
                If (mmsi > 99) Then
                    Dim index As Integer = mmsi.Uint2int(0, 3)
                    If (Me.Countries.ContainsKey(index)) Then
                        Return Me.Countries(index)
                    End If
                End If
                Return "Unknown"
            End Get
        End Property

        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Disposed Then
                If disposing Then
                    RemoveHandler Me.Client.OnDataReceived, AddressOf Me.OnDataReceived
                    Me.Client.Dispose()
                    Me.Tracking.Clear()
                    Me.Countries.Clear()
                End If
                Disposed = True
            End If
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            Dispose(disposing:=True)
            GC.SuppressFinalize(Me)
        End Sub
    End Class
End Namespace