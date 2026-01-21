Imports System.Drawing
Imports System.Linq
Imports System.Drawing.Drawing2D
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
            Dim targetEnemies = 5 + (roundNumber - 1)

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

        Dim rX As Single = CSng((rng.NextDouble() * 2.0) - 1.0)
        Dim rY As Single = CSng((rng.NextDouble() * 2.0) - 1.0)

        Dim finalX As Single = (rX * 400) + playerPos.X + (Math.Sign(rX) * 400)
        Dim finalY As Single = (rY * 400) + playerPos.Y + (Math.Sign(rY) * 400)

        If finalX < SAFE_MIN Then
            finalX = SAFE_MIN + 200
        ElseIf finalX > SAFE_MAX Then
            finalX = SAFE_MAX - 200
        End If

        If finalY < SAFE_MIN Then
            finalY = SAFE_MIN + 200
        ElseIf finalY > SAFE_MAX Then
            finalY = SAFE_MAX - 200
        End If

        Return New PointF(finalX, finalY)
    End Function

    Public Sub Draw(world As World, g As Graphics) Implements ISystem.Draw
        If isShowingText Then
            Dim text As String = "RONDA " & roundNumber
            Using font As New Font("Arial", 40, FontStyle.Bold)
                Dim textSize = g.MeasureString(text, font)

                Dim state = g.Save()
                g.ResetTransform()

                Dim xPos As Single = CSng((world.SCREEN_WIDTH - textSize.Width) / 2)
                Dim yPos As Single = CSng(world.SCREEN_HEIGHT / 4)

                Dim alpha As Integer = 255
                Dim fadeDuration As Single = 0.5F
                If textTimer < fadeDuration Then
                    alpha = CInt((textTimer / fadeDuration) * 255)
                End If
                alpha = Math.Max(0, Math.Min(255, alpha))
                Using shadowBrush As New SolidBrush(Color.FromArgb(alpha, Color.Black))
                    Using textBrush As New SolidBrush(Color.FromArgb(alpha, Color.White))

                        g.DrawString(text, font, shadowBrush, xPos + 4, yPos + 4)
                        g.DrawString(text, font, textBrush, xPos, yPos)

                    End Using
                End Using
                g.Restore(state)
            End Using
        End If
    End Sub
End Class