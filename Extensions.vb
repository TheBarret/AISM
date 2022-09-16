Imports System.Runtime.CompilerServices
Imports AisParser.Messages
Imports GMap.NET

Public Module Extensions

    ''' <summary>
    ''' Calculate distance between two geo-coordinates.
    ''' </summary>
    <Extension> Public Function Distance(p1 As PointLatLng, p2 As PointLatLng, Optional round As Boolean = False) As Double
        Dim p1r As Double = Math.PI * p1.Lat / 180
        Dim p2r As Double = Math.PI * p2.Lat / 180
        Dim dist As Double = Math.Sin(p1r) * Math.Sin(p2r) +
                             Math.Cos(p1r) * Math.Cos(p2r) *
                             Math.Cos(Math.PI * (p1.Lng - p2.Lng) / 180)
        dist = (((Math.Acos(dist)) * 180 / Math.PI) * 60 * 1.1515) * 1.609344
        If (round) Then Return Math.Round(dist, 2) Else Return dist
    End Function

    ''' <summary>
    ''' Returns the geo-coordinates from a 'PositionReportClassAMessage'
    ''' </summary>
    <Extension> Public Function Coordinates(value As PositionReportClassAMessage) As PointLatLng
        Return New PointLatLng(value.Latitude, value.Longitude)
    End Function

    ''' <summary>
    ''' Returns the geo-coordinates from a 'BaseStationReportMessage'
    ''' </summary>
    <Extension> Public Function Coordinates(value As BaseStationReportMessage) As PointLatLng
        Return New PointLatLng(value.Latitude, value.Longitude)
    End Function

    ''' <summary>
    ''' A supplemental String.Substring for Uint32 to Int
    ''' </summary>
    <Extension> Public Function Uint2int(value As UInt32, index As Integer, length As Integer) As Integer
        Dim str As String = value.ToString
        If (index + length <= str.Length) Then
            If (str.IsNumber) Then
                Dim result As Integer = 0
                If (Integer.TryParse(str.Substring(index, length), result)) Then
                    Return result
                End If
            End If
        End If
        Return 0
    End Function

    ''' <summary>
    ''' A supplemental String.Substring for String to Int
    ''' </summary>
    <Extension> Public Function Str2int(value As String, index As Integer, length As Integer) As Integer
        If (index + length <= value.Length) Then
            Dim v As String = value.Substring(index, length)
            If (v.IsNumber) Then
                Dim result As Integer = 0
                If (Integer.TryParse(v, result)) Then
                    Return result
                End If
            End If
        End If
        Return 0
    End Function

    ''' <summary>
    ''' Returns true if this string is a number.
    ''' </summary>
    <Extension> Public Function IsNumber(value As String) As Boolean
        For Each v As Char In value.ToCharArray
            If (Not Char.IsNumber(v)) Then Return False
        Next
        Return True
    End Function
End Module