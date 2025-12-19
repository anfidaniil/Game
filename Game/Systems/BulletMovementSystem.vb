Public Class BulletMovementSystem
    Implements ISystem

    Public Sub Update(world As World, dt As Single) Implements ISystem.Update
        For Each kv In world.Movements.All
            Dim id = kv.Key
            If world.Bullets.HasComponent(id) Then
                Dim m = kv.Value
                Dim t = world.Transforms.GetComponent(id)

                m.velocity = New PointF(
                    m.velocity.X + m.acceleration.X * dt,
                    m.velocity.Y + m.acceleration.Y * dt
                )

                m.velocity = New PointF(
                    m.velocity.X * (1 - m.damping * dt),
                    m.velocity.Y * (1 - m.damping * dt)
                )

                t.pos = New PointF(
                    t.pos.X + m.velocity.X * dt,
                    t.pos.Y + m.velocity.Y * dt
                )

                If Math.Abs(m.velocity.X) < 0.1F AndAlso Math.Abs(m.velocity.Y) < 0.1F Then
                    m.velocity = New PointF(0, 0)
                End If
            End If
        Next
    End Sub

    Public Sub Draw(world As World) Implements ISystem.Draw
    End Sub
End Class