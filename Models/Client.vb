Imports System.Net
Imports System.Net.Sockets
Imports System.Threading

Namespace Models
    Public Class Client
        Implements IDisposable
        Const BUFFERSIZE As Integer = 1024
        Public Property Parent As Manager
        Public Property Host As String
        Public Property Port As Integer
        Public Property Running As Boolean
        Public Property Socket As TcpClient
        Private Disposed As Boolean
        Private Reset As ManualResetEvent
        Public Event OnDataReceived(buffer() As Byte)

        Sub New(parent As Manager, host As String, port As Integer)
            Me.Parent = parent
            Me.Port = port
            Me.Host = host
            Me.Running = False
            Me.Reset = New ManualResetEvent(False)
        End Sub

        Public Sub Start()
            Me.Socket = New TcpClient With {.LingerState = New LingerOption(False, 0)}
            Me.Socket.BeginConnect(IPAddress.Parse(Me.Host), Me.Port, New AsyncCallback(AddressOf Me.OnConnect), Me.Socket)
        End Sub

        Public Sub Close()
            Me.Running = False
        End Sub

        Private Sub OnConnect(ar As IAsyncResult)
            Try
                Me.Socket.EndConnect(ar)
                If (Me.Socket.Connected) Then
                    Call New Thread(AddressOf Me.Worker) With {.IsBackground = True}.Start()
                    Return
                End If
                Me.Parent.Log("! Connection attempt failed")
            Catch ex As Exception
                Me.Parent.Log("! Fatal error, {0}", ex.Message)
            Finally
                If (Not Me.Socket.Connected) Then
                    Me.Socket.Dispose()
                End If
            End Try
        End Sub

        Private Sub Worker()
            Try

                If (Me.Running) Then
                    Me.Running = False
                    Me.Reset.WaitOne()
                End If

                Me.Running = True
                Me.Reset.Reset()

                Dim total As Integer = Me.Socket.Available
                If (total > 0) Then
                    Me.Parent.Log("<- Fetching {0} bytes", total)
                    If (Me.Socket.Available <= Client.BUFFERSIZE) Then      '// read all at once
                        Dim buffer As Byte() = New Byte(total - 1) {}
                        Me.Socket.GetStream.Read(buffer, 0, buffer.Length)
                        RaiseEvent OnDataReceived(buffer)
                    Else                                                    '// read by chunk
                        Dim buffer As New List(Of Byte)
                        Dim length As Integer, chunk As Byte()
                        Do
                            length = If(total <= Client.BUFFERSIZE, total, Client.BUFFERSIZE)
                            chunk = New Byte(length - 1) {}
                            Me.Socket.GetStream.Read(chunk, 0, chunk.Length)
                            buffer.AddRange(chunk)
                            total -= length
                        Loop While Me.Socket.Available > 0
                        RaiseEvent OnDataReceived(buffer.ToArray)
                    End If
                End If
            Catch ex As Exception
                Me.Parent.Log("! Fatal error, {0}", ex.Message)
            Finally
                Me.Socket.Close()
                Me.Running = False
                Me.Reset.Set()
            End Try
        End Sub

        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Disposed Then
                If disposing Then
                    Me.Close()
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