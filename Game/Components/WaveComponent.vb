Public Class WaveComponent
    Public Enum WaveState
        FadingIn
        Holding
        FadingOut
        Spawning
        Playing
    End Enum

    Public State As WaveState = WaveState.FadingIn
    Public RoundNumber As Integer = 1
    Public Opacity As Single = 0
    Public Timer As Single = 0
    Public EnemiesSpawned As Integer = 0
End Class
