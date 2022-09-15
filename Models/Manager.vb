Imports AisParser
Imports System.Text
Imports GMap.NET.WindowsForms
Imports AisParser.Messages

Namespace Models
    Public Class Manager
        Implements IDisposable

        Public Property Client As Client
        Public Property Overlay As GMapOverlay
        Public Property Logger As Action(Of String)
        Public Property Countries As Dictionary(Of String, String)
        Public Property Tracking As Dictionary(Of UInt32, Message)
        Private Property Disposed As Boolean

        Sub New(host As String, port As Integer)
            Me.Initialize()
            Me.Prepare(host, port)
        End Sub

        Private Sub Initialize()
            Me.Overlay = New GMapOverlay("AIS")
            Me.Tracking = New Dictionary(Of UInteger, Message)
            Me.Countries = New Dictionary(Of String, String)
            Dim data As String = My.Resources.codes
            For Each line As String In data.Split(ControlChars.NewLine)
                Dim index As String = line.Trim.Substring(0, 3)
                Dim origin As String = line.Substring(3).Trim
                Me.Countries.Add(index, origin)
            Next
        End Sub

        Private Sub Prepare(host As String, port As Integer)
            Me.Client = New Client(Me, host, port)
            AddHandler Me.Client.OnDataReceived, AddressOf Me.OnDataReceived
        End Sub

        Private Sub OnDataReceived(buffer() As Byte)
            Me.Process(buffer)
            Me.Client.Close()
        End Sub

        Private Sub Process(buffer() As Byte)
            Dim value As String = String.Empty, message As AisMessage
            Static AIS As New AisParser.Parser
            value = UTF8Encoding.ASCII.GetString(buffer)
            If (value.Contains(ControlChars.NewLine)) Then
                For Each line As String In value.Split(ControlChars.NewLine)
                    If (line.Length > 0) Then
                        message = AIS.Parse(line)
                        Me.Assign(message)
                    End If
                Next
            Else
                message = AIS.Parse(value)
                Me.Assign(message)
            End If
        End Sub

        Private Sub Assign(message As AisMessage)
            If (message IsNot Nothing) Then
                Select Case message.MessageType
                    Case AisMessageType.PositionReportClassA
                        If (Me.Tracking.ContainsKey(message.Mmsi)) Then
                            Me.Tracking(message.Mmsi).Lat = CType(message, PositionReportClassAMessage).Latitude
                            Me.Tracking(message.Mmsi).Lon = CType(message, PositionReportClassAMessage).Longitude
                            Me.Log(String.Format("Vessel at {0}° {1}° [known]", Me.Tracking(message.Mmsi).Lat, Me.Tracking(message.Mmsi).Lon))
                        Else
                            Me.Tracking.Add(message.Mmsi, New Message(Me, message))
                            Me.Overlay.Markers.Add(Me.Tracking(message.Mmsi).Marker)
                            Me.Log(String.Format("Vessel at {0}° {1}°", Me.Tracking(message.Mmsi).Lat, Me.Tracking(message.Mmsi).Lon))
                        End If
                    Case AisMessageType.BaseStationReport
                        If (Me.Tracking.ContainsKey(message.Mmsi)) Then
                            Me.Tracking(message.Mmsi).Lat = CType(message, BaseStationReportMessage).Latitude
                            Me.Tracking(message.Mmsi).Lon = CType(message, BaseStationReportMessage).Longitude
                            Me.Log(String.Format("Station at {0}° {1}° [known]", Me.Tracking(message.Mmsi).Lat, Me.Tracking(message.Mmsi).Lon))
                        Else
                            Me.Tracking.Add(message.Mmsi, New Message(Me, message))
                            Me.Overlay.Markers.Add(Me.Tracking(message.Mmsi).Marker)
                            Me.Log(String.Format("Station at {0}° {1}°", Me.Tracking(message.Mmsi).Lat, Me.Tracking(message.Mmsi).Lon))
                        End If
                End Select
            End If
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

        Public ReadOnly Property Mmsi2Country(mmsi As UInt32) As String
            Get
                If (mmsi > 99) Then
                    Dim index As String = mmsi.ToString.Substring(0, 3)
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