Imports System.Drawing
Imports System.Linq

Public Class WaveSystem
    Implements ISystem

    Private Const SAFE_MIN As Integer = 1300
    Private Const SAFE_MAX As Integer = 2800

    Private roundNumber As Integer = 0
    Private enemiesSpawnedInThisRound As Integer = 0
    Private isWaveActive As Boolean = False
    Private rng As New Random()

    Private textTimer As Single = 0
    Private showTextDuration As Single = 3.0F
    Private isShowingText As Boolean = False

    Public Sub Update(world As World, dt As Single) Implements ISystem.Update

        Dim enemiesAlive = world.Enemies.All.Count

        If enemiesAlive = 0 AndAlso Not isShowingText AndAlso Not isWaveActive Then
            StartNextRound()
        End If

        If isShowingText Then
            textTimer -= dt
            If textTimer <= 0 Then
                isShowingText = False
                isWaveActive = True
                enemiesSpawnedInThisRound = 0
            End If
            Return
        End If

        If isWaveActive Then
            Dim calculatedEnemies = 5 + (roundNumber - 1)
            Dim targetEnemies = Math.Min(calculatedEnemies, 25)

            If enemiesSpawnedInThisRound < targetEnemies Then

                Dim playerPos As PointF = New PointF(2048, 2048)
                If world.Players.HasComponent(world.PlayerID) Then
                    playerPos = world.Transforms.GetComponent(world.PlayerID).pos
                End If

                Dim spawnPos = GetRandomSpawnPos(playerPos)
                world.CreateEnemy(spawnPos)

                enemiesSpawnedInThisRound += 1
            Else
                isWaveActive = False
            End If
        End If
    End Sub

    Private Sub StartNextRound()
        roundNumber += 1
        isShowingText = True
        textTimer = showTextDuration
    End Sub

    Private Function GetRandomSpawnPos(playerPos As PointF) As PointF
        Dim x, y As Single
        For i As Integer = 1 To 15
            x = rng.Next(SAFE_MIN, SAFE_MAX)
            y = rng.Next(SAFE_MIN, SAFE_MAX)

            Dim dx = x - playerPos.X
            Dim dy = y - playerPos.Y
            Dim dist = Math.Sqrt(dx * dx + dy * dy)

            If dist > 400 Then Exit For
        Next

        Return New PointF(x, y)
    End Function

    Public Sub Draw(world As World, g As Graphics) Implements ISystem.Draw
        If isShowingText Then
            Dim text As String = "RONDA " & roundNumber
            Using font As New Font("Arial", 40, FontStyle.Bold)
                Dim textSize = g.MeasureString(text, font)
                Dim oldMatrix = g.Transform
                g.ResetTransform()

                Dim xPos As Single = CSng((world.SCREEN_WIDTH - textSize.Width) / 2)
                Dim yPos As Single = CSng(world.SCREEN_HEIGHT / 4)

                g.DrawString(text, font, Brushes.Black, xPos + 4, yPos + 4)
                g.DrawString(text, font, Brushes.White, xPos, yPos)

                g.Transform = oldMatrix
            End Using
        End If
    End Sub
End Class