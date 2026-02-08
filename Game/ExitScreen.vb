Imports System.Drawing
Imports Windows.Win32.UI.Input

Public Class ExitScreen
    Private game As Game
    Public buttons As New List(Of UIButton)
    Private imgBG As Bitmap

    Public Sub New(gameInstance As Game, yesAction As Action, noAction As Action)
        Me.game = gameInstance
        LoadResources()
        InitializeButtons(yesAction, noAction)
    End Sub

    Private Sub LoadResources()
        imgBG = My.Resources.GameResources.MAINmenu
    End Sub

    Private Sub InitializeButtons(yesAction As Action, noAction As Action)
        Dim screenW As Integer = Form1.Width
        Dim screenH As Integer = Form1.Height

        Dim scale As Single = game.GetUIElementScale()

        Dim btnW As Integer = CInt(200 * scale)
        Dim btnH As Integer = CInt(50 * scale)
        Dim spacing As Integer = CInt(40 * scale)

        Dim bottomMargin As Integer = CInt(150 * scale)
        Dim btnY As Integer = screenH - bottomMargin

        Dim totalWidth As Integer = (btnW * 2) + spacing
        Dim startX As Integer = (screenW - totalWidth) \ 2

        buttons.Add(New UIButtonYes With {
            .bounds = New Rectangle(startX, btnY, btnW, btnH),
            .text = "",
            .onClick = yesAction
        })

        buttons.Add(New UIButtonNo With {
            .bounds = New Rectangle(startX + btnW + spacing, btnY, btnW, btnH),
            .text = "",
            .onClick = noAction
        })
    End Sub

    Public Sub Draw(g As Graphics, world As World)
        g.Clear(Color.Black)

        g.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor
        g.PixelOffsetMode = Drawing2D.PixelOffsetMode.Half

        If imgBG IsNot Nothing Then
            Dim imgRatio As Single = CSng(imgBG.Width) / imgBG.Height
            Dim formRatio As Single = CSng(Form1.Width) / Form1.Height
            Dim drawW, drawH As Integer

            If formRatio > imgRatio Then
                drawH = Form1.Height
                drawW = CInt(Form1.Height * imgRatio)
            Else
                drawW = Form1.Width
                drawH = CInt(Form1.Width / imgRatio)
            End If

            Dim x As Integer = (Form1.Width - drawW) \ 2
            Dim y As Integer = (Form1.Height - drawH) \ 2
            g.DrawImage(imgBG, x, y, drawW, drawH)
        End If

        Using font As New Font("Arial", 24, FontStyle.Bold)
            Dim text = "Tem a certeza de que quer sair?"
            Dim size = g.MeasureString(text, font)

            Dim textX As Single = (Form1.Width - size.Width) / 2
            Dim textY As Single = CSng(Form1.Height * 0.4)

            g.DrawString(text, font, Brushes.Black, textX + 2, textY + 2)
            g.DrawString(text, font, Brushes.White, textX, textY)
        End Using

        For Each btn In buttons
            If (btn.sprite IsNot Nothing) Then
                g.DrawImage(btn.sprite, btn.bounds)
            Else
                Dim bgBrush As Brush = Brushes.DarkGray
                If TypeOf btn Is UIButtonYes Then bgBrush = Brushes.DarkRed

                g.FillRectangle(bgBrush, btn.bounds)
                g.DrawRectangle(Pens.White, btn.bounds)

                Using font As New Font("Arial", 16, FontStyle.Bold)
                    Dim size = g.MeasureString(btn.text, font)
                    g.DrawString(btn.text, font, Brushes.White,
                        btn.bounds.X + (btn.bounds.Width - size.Width) \ 2,
                        btn.bounds.Y + (btn.bounds.Height - size.Height) \ 2)
                End Using
            End If
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
