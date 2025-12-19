Public Class Game
    Private world As World
    Private g As Graphics
    Private gameState As GameState

    Public Sub New(g As Graphics, input As InputState)
        Me.world = New World(g, input, Me)
        Me.g = g
        Me.gameState = GameState.Playing
        world.CreatePlayer()
        world.CreateEnemy()
        world.CreateStain(New PointF(0, 100))
    End Sub

    Public Sub GameOver()
        Debug.WriteLine("GameOver")
        gameState = GameState.GameOver
    End Sub

    Public Sub Update(dt As Single)
        Select Case gameState
            Case GameState.Playing
                world.Update(dt)
                world.CollisionEvents.Clear()
            Case GameState.GameOver

        End Select
    End Sub

    Public Sub Draw()
        Select Case gameState
            Case GameState.Playing
                world.Draw()
            Case GameState.GameOver
                g.Clear(Color.Red)
        End Select
    End Sub
End Class
