Public Class MovementSystem
    Implements ISystem

    Public Sub Update(world As World, dt As Single) Implements ISystem.Update
        For Each kv In world.Transforms.All
            Dim id = kv.Key
            If world.Movements.HasComponent(id) Then
                Dim t = kv.Value
                Dim m = world.Movements.GetComponent(id)

                Dim vx = m.velocity.X + m.acceleration.X * dt
                Dim vy = m.velocity.Y + m.acceleration.Y * dt

                vx = Math.Max(-World.MAX_VELOCITY, Math.Min(World.MAX_VELOCITY, vx))
                vy = Math.Max(-World.MAX_VELOCITY, Math.Min(World.MAX_VELOCITY, vy))

                If m.acceleration.X = 0 Then
                    vx *= Math.Max(0, 1 - m.damping * dt)
                End If

                If m.acceleration.Y = 0 Then
                    vy *= Math.Max(0, 1 - m.damping * dt)
                End If

                If Math.Abs(vx) < 0.01F Then vx = 0
                If Math.Abs(vy) < 0.01F Then vy = 0

                m.velocity = New PointF(vx, vy)

                t.pos = New PointF(
                    t.pos.X + m.velocity.X * dt,
                    t.pos.Y + m.velocity.Y * dt)
            End If
        Next
    End Sub

    Public Sub Draw(world As World) Implements ISystem.Draw

    End Sub
End Class
