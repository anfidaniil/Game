Imports System.Drawing

Public Class WaveSystem
    Implements ISystem

    ' Estados do Sistema
    Private Enum WaveState
        FadingIn    ' Texto a aparecer
        Holding     ' Texto parado no ecrã (opcional, para leitura rápida)
        FadingOut   ' Texto a desaparecer
        Spawning    ' Criar inimigos
        Playing     ' Jogador a lutar (esperando zero inimigos)
    End Enum

    Private currentState As WaveState = WaveState.FadingIn

    ' Configurações da Ronda
    Private roundNumber As Integer = 1
    Private enemiesPerRound As Integer = 8
    Private mapSize As Integer = 2500 ' Tamanho do teu mapa level1
    Private spawnMargin As Integer = 150 ' Margem para não tocar nas bordas
    Private rng As New Random()

    ' Variáveis de Animação
    Private opacity As Single = 0
    Private fadeSpeed As Single = 1.5F ' Velocidade do efeito
    Private holdTimer As Single = 0
    Private holdDuration As Single = 0.5F ' Tempo que o texto fica totalmente visível

    Public Sub Update(world As World, dt As Single) Implements ISystem.Update
        Select Case currentState

            Case WaveState.FadingIn
                opacity += fadeSpeed * dt
                If opacity >= 1.0F Then
                    opacity = 1.0F
                    currentState = WaveState.Holding
                    holdTimer = holdDuration
                End If

            Case WaveState.Holding
                holdTimer -= dt
                If holdTimer <= 0 Then
                    currentState = WaveState.FadingOut
                End If

            Case WaveState.FadingOut
                opacity -= fadeSpeed * dt
                If opacity <= 0.0F Then
                    opacity = 0.0F
                    currentState = WaveState.Spawning
                End If

            Case WaveState.Spawning
                SpawnWave(world)
                currentState = WaveState.Playing

            Case WaveState.Playing
                ' Verifica se ainda há inimigos vivos
                If world.Enemies.All.Count = 0 Then
                    ' Se zero, prepara a próxima ronda
                    roundNumber += 1
                    currentState = WaveState.FadingIn
                    opacity = 0
                End If

        End Select
    End Sub

    Private Sub SpawnWave(world As World)
        Dim spawnedCount As Integer = 0

        ' Obtém a posição do jogador para evitar spawn em cima dele
        Dim playerPos As PointF = New PointF(1000, 1000) ' Default
        If world.Players.HasComponent(world.PlayerID) Then
            playerPos = world.Transforms.GetComponent(world.PlayerID).pos
        End If

        While spawnedCount < enemiesPerRound
            Dim pos = GetSafeSpawnPosition(playerPos)
            If pos.HasValue Then
                world.CreateEnemy(pos.Value)
                spawnedCount += 1
            End If
        End While
    End Sub

    Private Function GetSafeSpawnPosition(playerPos As PointF) As Nullable(Of PointF)
        ' Tenta encontrar uma posição válida (máximo 20 tentativas para não travar o jogo)
        For i As Integer = 1 To 20
            Dim x = rng.Next(spawnMargin, mapSize - spawnMargin)
            Dim y = rng.Next(spawnMargin, mapSize - spawnMargin)

            ' Verifica distância do jogador (ex: 500 pixeis)
            Dim dx = x - playerPos.X
            Dim dy = y - playerPos.Y
            Dim distSq = dx * dx + dy * dy

            ' 500 ao quadrado = 250000 (evita raiz quadrada para performance)
            If distSq > 250000 Then
                Return New PointF(x, y)
            End If
        Next
        Return Nothing ' Se falhar 20 vezes, retorna nada (tenta no próximo loop)
    End Function

    Public Sub Draw(world As World, g As Graphics) Implements ISystem.Draw
        ' Só desenha se estivermos numa fase de animação
        If currentState = WaveState.FadingIn OrElse currentState = WaveState.Holding OrElse currentState = WaveState.FadingOut Then

            ' Salva o estado da câmara
            Dim originalState = g.Save()

            ' Reseta a transformação para desenhar FIXO no ecrã (HUD)
            g.ResetTransform()

            Dim text As String = "ROUND " & roundNumber
            Using font As New Font("Arial", 48, FontStyle.Bold)
                Dim textSize = g.MeasureString(text, font)
                Dim x = (Form1.Width - textSize.Width) / 2
                Dim y = (Form1.Height - textSize.Height) / 3

                ' Define a cor com a opacidade atual
                Dim alpha As Integer = Math.Max(0, Math.Min(255, CInt(opacity * 255)))
                Using brush As New SolidBrush(Color.FromArgb(alpha, Color.White))
                    g.DrawString(text, font, brush, x, y)
                End Using
            End Using

            ' Restaura a câmara para o resto do jogo
            g.Restore(originalState)
        End If
    End Sub
End Class