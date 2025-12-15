Public Class EnemyMovementSystem
    Implements ISystem

    Public Sub Update(world As World, dt As Single) Implements ISystem.Update
        For Each kv In world.Transforms.All
            Dim id = kv.Key
            If world.Movements.HasComponent(id) Then
                Dim t = kv.Value
                Dim m = world.Movements.GetComponent(id)

                Dim dx As Double = 0
                Dim dy As Double = 0

                If Not world.Players.HasComponent(id) Then
                    Dim playerPos = world.Transforms.GetComponent(world.PlayerID)
                    Dim x = playerPos.pos.X
                    Dim y = playerPos.pos.Y

                    dx = (x - t.pos.X)
                    dy = (y - t.pos.Y)

                    Dim magnitude As Double = (Math.Sqrt(dx * dx + dy * dy))

                    dx = dx / magnitude
                    dy = dy / magnitude
                End If
                    t.pos = New PointF(
                        t.pos.X + dx * m.speed * dt,
                        t.pos.Y + dy * m.speed * dt)
            End If
        Next
    End Sub

    Public Sub Draw(world As World) Implements ISystem.Draw

    End Sub
End Class
