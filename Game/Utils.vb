Module Utils
    Function NormaliseVector(x As Double, y As Double) As Double()
        Dim res(2) As Double
        Dim magnitude As Double = (Math.Sqrt(x * x + y * y))

        If (magnitude <> 0) Then
            x = x / magnitude
            y = y / magnitude
        End If

        res(0) = x
        res(1) = y

        Return res
    End Function
End Module
