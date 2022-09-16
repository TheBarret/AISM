Public Class History(Of T)
    Public Property Max As Integer = 10
    Public Property Items As List(Of T)

    Sub New()
        Me.Items = New List(Of T)
    End Sub

    Sub New(max As Integer)
        Me.Items = New List(Of T)
        Me.Max = max
    End Sub

    Public Sub Push(item As T)
        SyncLock Me.Items
            Me.Items.Insert(0, item)
            If (Me.Items.Count > Me.Max) Then
                Do
                    Me.Items.RemoveAt(Me.Items.Count - 1)
                Loop While Me.Items.Count > Me.Max
            End If
        End SyncLock
    End Sub

    Public Sub Pop()
        SyncLock Me.Items
            Me.Items.RemoveAt(Me.Items.Count - 1)
        End SyncLock
    End Sub

    Public ReadOnly Property Count As Integer
        Get
            Return Me.Items.Count
        End Get
    End Property

End Class
