Imports Windows.Win32.UI.Input

Public Class StartScreen
    Public buttons As New List(Of UIButton)
    Dim buttonWidth = 200
    Dim buttonHeight = 50

    Public Sub New(screenWidth As Integer, screenHeight As Integer, restart As Action, quit As Action)
        Dim centerX = (screenWidth - buttonWidth) \ 2
        Dim centerY = screenHeight \ 2

        buttons.Add(New UIButton With {
            .bounds = New Rectangle(centerX - buttonWidth / 2 - 20, centerY, buttonWidth, buttonHeight),
            .text = "Start New Game",
            .onClick = restart
        })

        buttons.Add(New UIButton With {
            .bounds = New Rectangle(centerX + buttonWidth / 2 + 20, centerY, buttonWidth, buttonHeight),
            .text = "Quit",
            .onClick = quit
        })
    End Sub

    Public Sub Draw(g As Graphics, world As World)
        g.Clear(Color.Black)

        Dim imgRatio As Single = 1920 / 1080
        Dim formRatio As Single = Form1.Width / Form1.Height

        Dim drawW As Integer
        Dim drawH As Integer

        If formRatio > imgRatio Then
            drawH = Form1.Height
            drawW = CInt(Form1.Height * imgRatio)
        Else
            drawW = Form1.Width
            drawH = CInt(Form1.Width / imgRatio)
        End If

        ' Center the image
        Dim x As Integer = (Form1.Width - drawW) \ 2
        Dim y As Integer = (Form1.Height - drawH) \ 2

        g.DrawImage(world.game.bgc, x, y, drawW, drawH)

        Using font As New Font("Arial", 24, FontStyle.Bold)
            Dim text = World.GAME_NAME
            Dim size = g.MeasureString(text, font)
            g.DrawString(text, font, Brushes.White,
                (Form1.Width - size.Width) / 2, 100)
        End Using

        For Each btn In buttons
            g.FillRectangle(Brushes.DarkGray, btn.bounds)
            g.DrawRectangle(Pens.White, btn.bounds)

            Using font As New Font("Arial", 16, FontStyle.Bold)
                Dim size = g.MeasureString(btn.text, font)
                g.DrawString(btn.text, font, Brushes.White,
                    btn.bounds.X + (btn.bounds.Width - size.Width) \ 2,
                    btn.bounds.Y + (btn.bounds.Height - size.Height) \ 2)
            End Using
        Next
    End Sub

    Public Sub HandleMouseClick(mousePos As Point)
        For Each btn In buttons
            If btn.bounds.Contains(mousePos) Then
                btn.onClick?.Invoke()
            End If
        Next
    End Sub
End Class
