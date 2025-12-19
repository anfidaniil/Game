Public Class Game
    Private world As World
    Private g As Graphics
    Private gameState As GameState

    Public Sub New(g As Graphics, input As InputState)
        Me.world = New World(g, input, Me)
        Me.g = g
        Me.gameState = GameState.Playing
        CreateTestWorld()
    End Sub

    Public Sub CreateTestWorld()
        world.CreatePlayer()
        CreateEnemiesAroundPoint(0, 0, 2)
        CreateEnemiesAroundPoint(400, 0, 2)
    End Sub

    Public Sub GameOver()
        Debug.WriteLine("GameOver")
        gameState = GameState.GameOver
    End Sub

    Public Sub CreateEnemiesAroundPoint(posX As Integer, posY As Integer, numEnemies As Integer)
        For i = 1 To numEnemies
            world.CreateEnemy(New PointF(
                posX + Random.Shared.Next(-10, 10),
                posY + Random.Shared.Next(-10, 10)
            ))
        Next
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
                g.Clear(Color.Beige)
        End Select
    End Sub
End Class
