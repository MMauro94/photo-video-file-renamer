Public Class TimeSpanSelector


    Private span As TimeSpan
    Public Sub New(Optional ByVal span As TimeSpan = Nothing)
        InitializeComponent()
        If span <> Nothing Then
            Me.span = span
        Else
            Me.span = New TimeSpan(0, 0, 0)
        End If
    End Sub

    Public Sub ApplyTimeSpan()
        Days.Value = span.Days
        Hours.Value = span.Hours
        Minutes.Value = span.Minutes
        Seconds.Value = span.Seconds
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        span = New TimeSpan(Days.Value, Hours.Value, Minutes.Value, Seconds.Value)
        RaiseEvent OnEnd(span)
        Me.Close()
    End Sub

    Public Event OnEnd(ByVal newSpan As TimeSpan)

    Private Sub TimeSpanSelector_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ApplyTimeSpan()
    End Sub
End Class