Imports System.Drawing

Public Class UICard
    Public Sprite As Bitmap

    Public Sub New(resource As Bitmap)
        Me.Sprite = resource
    End Sub

    Public ReadOnly Property Width As Integer
        Get
            Return If(Sprite IsNot Nothing, Sprite.Width, 0)
        End Get
    End Property

    Public ReadOnly Property Height As Integer
        Get
            Return If(Sprite IsNot Nothing, Sprite.Height, 0)
        End Get
    End Property

    Public Sub Draw(g As Graphics, rect As Rectangle, isFocused As Boolean)
        If Sprite Is Nothing Then Return

        g.DrawImage(Sprite, rect)

        If isFocused Then
            Using pen As New Pen(Color.Gold, 4)
                g.DrawRectangle(pen, rect)
            End Using
        Else
            Using brush As New SolidBrush(Color.FromArgb(150, 0, 0, 0))
                g.FillRectangle(brush, rect)
            End Using
        End If
    End Sub
End Class
