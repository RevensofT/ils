Namespace ILS
   
    Public Module Adapter

#Region "func"
        <Extension, Method(inline)>
        Public Function func(Of TR)(Script As String) As ils(Of Func(Of TR))
            Return New ils(Of Func(Of TR))(Script)
        End Function
        <Extension, Method(inline)>
        Public Function func(Of TR, T1)(Script As String) As ils(Of Func(Of T1, TR))
            Return New ils(Of Func(Of T1, TR))(Script)
        End Function
        <Extension, Method(inline)>
        Public Function func(Of TR, T1, T2)(Script As String) As ils(Of Func(Of T1, T2, TR))
            Return New ils(Of Func(Of T1, T2, TR))(Script)
        End Function
        <Extension, Method(inline)>
        Public Function func(Of TR, T1, T2, T3)(Script As String) As ils(Of Func(Of T1, T2, T3, TR))
            Return New ils(Of Func(Of T1, T2, T3, TR))(Script)
        End Function
        <Extension, Method(inline)>
        Public Function func(Of TR, T1, T2, T3, T4, T5)(Script As String) As ils(Of Func(Of T1, T2, T3, T4, T5, TR))
            Return New ils(Of Func(Of T1, T2, T3, T4, T5, TR))(Script)
        End Function

        <Extension, Method(inline)>
        Public Function func(Of TR, T1, T2, T3, T4, T5, T6)(Script As String) As ils(Of Func(Of T1, T2, T3, T4, T5, T6, TR))
            Return New ils(Of Func(Of T1, T2, T3, T4, T5, T6, TR))(Script)
        End Function

        <Extension, Method(inline)>
        Public Function func(Of TR, T1, T2, T3, T4, T5, T6, T7)(Script As String) As ils(Of Func(Of T1, T2, T3, T4, T5, T6, T7, TR))
            Return New ils(Of Func(Of T1, T2, T3, T4, T5, T6, T7, TR))(Script)
        End Function

        <Extension, Method(inline)>
        Public Function func(Of TR, T1, T2, T3, T4, T5, T6, T7, T8)(Script As String) As ils(Of Func(Of T1, T2, T3, T4, T5, T6, T7, T8, TR))
            Return New ils(Of Func(Of T1, T2, T3, T4, T5, T6, T7, T8, TR))(Script)
        End Function
#End Region

#Region "sub"
        <Extension, Method(inline)>
        Public Function [sub_0](Script As String) As ils(Of Action)
            Return New ils(Of Action)(Script)
        End Function
        Public Function [sub](Of T1)(Script As String) As ils(Of Action(Of T1))
            Return New ils(Of Action(Of T1))(Script)
        End Function

        <Extension, Method(inline)>
        Public Function [sub](Of T1, T2)(Script As String) As ils(Of Action(Of T1, T2))
            Return New ils(Of Action(Of T1, T2))(Script)
        End Function

        <Extension, Method(inline)>
        Public Function [sub](Of T1, T2, T3)(Script As String) As ils(Of Action(Of T1, T2, T3))
            Return New ils(Of Action(Of T1, T2, T3))(Script)
        End Function

        <Extension, Method(inline)>
        Public Function [sub](Of T1, T2, T3, T4)(Script As String) As ils(Of Action(Of T1, T2, T3, T4))
            Return New ils(Of Action(Of T1, T2, T3, T4))(Script)
        End Function

        <Extension, Method(inline)>
        Public Function [sub](Of T1, T2, T3, T4, T5)(Script As String) As ils(Of Action(Of T1, T2, T3, T4, T5))
            Return New ils(Of Action(Of T1, T2, T3, T4, T5))(Script)
        End Function

        <Extension, Method(inline)>
        Public Function [sub](Of T1, T2, T3, T4, T5, T6)(Script As String) As ils(Of Action(Of T1, T2, T3, T4, T5, T6))
            Return New ils(Of Action(Of T1, T2, T3, T4, T5, T6))(Script)
        End Function

        <Extension, Method(inline)>
        Public Function [sub](Of T1, T2, T3, T4, T5, T6, T7)(Script As String) As ils(Of Action(Of T1, T2, T3, T4, T5, T6, T7))
            Return New ils(Of Action(Of T1, T2, T3, T4, T5, T6, T7))(Script)
        End Function

        <Extension, Method(inline)>
        Public Function [sub](Of T1, T2, T3, T4, T5, T6, T7, T8)(Script As String) As ils(Of Action(Of T1, T2, T3, T4, T5, T6, T7, T8))
            Return New ils(Of Action(Of T1, T2, T3, T4, T5, T6, T7, T8))(Script)
        End Function
#End Region
    End Module

End Namespace
