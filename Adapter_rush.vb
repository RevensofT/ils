Namespace ILS
    Partial Friend Module Generate
        Friend Sub gen_rush()
            Dim Listing = Function(SB As Text.StringBuilder, N As Integer, Body As Func(Of Integer, String))
                              Dim I As Int32
                              For I = 1 To N - 1
                                  SB.Append($"{Body(I)}, ")
                              Next
                              Return SB.Append(Body(I))
                          End Function
            Dim Listing2 = Function(SB As Text.StringBuilder, N As Integer, Body As Func(Of Integer, String))
                               Dim I As Int32
                               For I = 1 To N - 1
                                   SB.Append($"{Body(I)}, ")
                               Next
                               Return SB
                           End Function

            With New Text.StringBuilder

                For i = 1 To 16

                    'T1, T2, ... TN
                    Dim T0 = Listing(.Clear, i, Function(N) $"T{N}").ToString

                    'Arg1 As T1, Arg2 As T2, ... ArgN As TN
                    Dim T1 = Listing(.Clear, i, Function(N) $"Arg{N} As T{N}").ToString

                    Dim T2 = Listing(.Clear, i, Function(N) $"Arg{N}").ToString


                    Dim T3 = Listing2(.Clear, i, Function(N) $"Arg{N} As T{N}").ToString & $"Inintialize_output As T{i}"

                    Dim T4 = Listing2(.Clear, i, Function(N) $"Arg{N}").ToString & $"Inintialize_output"

                    Dim D1 = $"Func(Of {T0}, T{i})"
                    Dim D2 = $"({D1}, T{i})"

                    '[sub] = New ils(Of Action(Of {T0}))(Script, Info.delegate(Of Action(Of {T0})).create_method).type(Of {T0}).fin
                    'func.method = New ils(Of {D1})(Script, Info.delegate(Of {D1}).create_method).type(Of {T0}).fin

                    Diagnostics.Debug.WriteLine(.Clear.Append($"
        <Extension, Method(inline)>
        Public Function [sub](Of {T0})(Script As String, {T1}) As incident(Of {T0})
            [sub] = New ils(Of Action(Of {T0}))(Script).type(Of {T0}).fin
            [sub].method({T2})
        End Function

        <Extension, Method(inline)>
        Public Function func(Of {T0})(Script As String, {T3}) As solution(Of {T0})
            func.method = New ils(Of {D1})(Script).type(Of {T0}).fin
            func.result = func.method({T4})
        End Function").
                        ToString)
                Next
            End With
        End Sub
    End Module

    Partial Public Module Adapter

        <Extension, Method(inline)>
        Public Function [sub](Script As String) As incident
            [sub] = New ils(Of Action)(Script).fin
            [sub].method()
        End Function


        <Extension, Method(inline)>
        Public Function [sub](Of T1)(Script As String, Arg1 As T1) As incident(Of T1)
            [sub] = New ils(Of Action(Of T1))(Script).type(Of T1).fin
            [sub].method(Arg1)
        End Function

        <Extension, Method(inline)>
        Public Function func(Of T1)(Script As String, Inintialize_output As T1) As solution(Of T1)
            func.method = New ils(Of Func(Of T1, T1))(Script).type(Of T1).fin
            func.result = func.method(Inintialize_output)
        End Function

        <Extension, Method(inline)>
        Public Function [sub](Of T1, T2)(Script As String, Arg1 As T1, Arg2 As T2) As incident(Of T1, T2)
            [sub] = New ils(Of Action(Of T1, T2))(Script).type(Of T1, T2).fin
            [sub].method(Arg1, Arg2)
        End Function

        <Extension, Method(inline)>
        Public Function func(Of T1, T2)(Script As String, Arg1 As T1, Inintialize_output As T2) As solution(Of T1, T2)
            func.method = New ils(Of Func(Of T1, T2, T2))(Script).type(Of T1, T2).fin
            func.result = func.method(Arg1, Inintialize_output)
        End Function

        <Extension, Method(inline)>
        Public Function [sub](Of T1, T2, T3)(Script As String, Arg1 As T1, Arg2 As T2, Arg3 As T3) As incident(Of T1, T2, T3)
            [sub] = New ils(Of Action(Of T1, T2, T3))(Script).type(Of T1, T2, T3).fin
            [sub].method(Arg1, Arg2, Arg3)
        End Function

        <Extension, Method(inline)>
        Public Function func(Of T1, T2, T3)(Script As String, Arg1 As T1, Arg2 As T2, Inintialize_output As T3) As solution(Of T1, T2, T3)
            func.method = New ils(Of Func(Of T1, T2, T3, T3))(Script).type(Of T1, T2, T3).fin
            func.result = func.method(Arg1, Arg2, Inintialize_output)
        End Function

        <Extension, Method(inline)>
        Public Function [sub](Of T1, T2, T3, T4)(Script As String, Arg1 As T1, Arg2 As T2, Arg3 As T3, Arg4 As T4) As incident(Of T1, T2, T3, T4)
            [sub] = New ils(Of Action(Of T1, T2, T3, T4))(Script).type(Of T1, T2, T3, T4).fin
            [sub].method(Arg1, Arg2, Arg3, Arg4)
        End Function

        <Extension, Method(inline)>
        Public Function func(Of T1, T2, T3, T4)(Script As String, Arg1 As T1, Arg2 As T2, Arg3 As T3, Inintialize_output As T4) As solution(Of T1, T2, T3, T4)
            func.method = New ils(Of Func(Of T1, T2, T3, T4, T4))(Script).type(Of T1, T2, T3, T4).fin
            func.result = func.method(Arg1, Arg2, Arg3, Inintialize_output)
        End Function

        <Extension, Method(inline)>
        Public Function [sub](Of T1, T2, T3, T4, T5)(Script As String, Arg1 As T1, Arg2 As T2, Arg3 As T3, Arg4 As T4, Arg5 As T5) As incident(Of T1, T2, T3, T4, T5)
            [sub] = New ils(Of Action(Of T1, T2, T3, T4, T5))(Script).type(Of T1, T2, T3, T4, T5).fin
            [sub].method(Arg1, Arg2, Arg3, Arg4, Arg5)
        End Function

        <Extension, Method(inline)>
        Public Function func(Of T1, T2, T3, T4, T5)(Script As String, Arg1 As T1, Arg2 As T2, Arg3 As T3, Arg4 As T4, Inintialize_output As T5) As solution(Of T1, T2, T3, T4, T5)
            func.method = New ils(Of Func(Of T1, T2, T3, T4, T5, T5))(Script).type(Of T1, T2, T3, T4, T5).fin
            func.result = func.method(Arg1, Arg2, Arg3, Arg4, Inintialize_output)
        End Function

        <Extension, Method(inline)>
        Public Function [sub](Of T1, T2, T3, T4, T5, T6)(Script As String, Arg1 As T1, Arg2 As T2, Arg3 As T3, Arg4 As T4, Arg5 As T5, Arg6 As T6) As incident(Of T1, T2, T3, T4, T5, T6)
            [sub] = New ils(Of Action(Of T1, T2, T3, T4, T5, T6))(Script).type(Of T1, T2, T3, T4, T5, T6).fin
            [sub].method(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6)
        End Function

        <Extension, Method(inline)>
        Public Function func(Of T1, T2, T3, T4, T5, T6)(Script As String, Arg1 As T1, Arg2 As T2, Arg3 As T3, Arg4 As T4, Arg5 As T5, Inintialize_output As T6) As solution(Of T1, T2, T3, T4, T5, T6)
            func.method = New ils(Of Func(Of T1, T2, T3, T4, T5, T6, T6))(Script).type(Of T1, T2, T3, T4, T5, T6).fin
            func.result = func.method(Arg1, Arg2, Arg3, Arg4, Arg5, Inintialize_output)
        End Function

        <Extension, Method(inline)>
        Public Function [sub](Of T1, T2, T3, T4, T5, T6, T7)(Script As String, Arg1 As T1, Arg2 As T2, Arg3 As T3, Arg4 As T4, Arg5 As T5, Arg6 As T6, Arg7 As T7) As incident(Of T1, T2, T3, T4, T5, T6, T7)
            [sub] = New ils(Of Action(Of T1, T2, T3, T4, T5, T6, T7))(Script).type(Of T1, T2, T3, T4, T5, T6, T7).fin
            [sub].method(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7)
        End Function

        <Extension, Method(inline)>
        Public Function func(Of T1, T2, T3, T4, T5, T6, T7)(Script As String, Arg1 As T1, Arg2 As T2, Arg3 As T3, Arg4 As T4, Arg5 As T5, Arg6 As T6, Inintialize_output As T7) As solution(Of T1, T2, T3, T4, T5, T6, T7)
            func.method = New ils(Of Func(Of T1, T2, T3, T4, T5, T6, T7, T7))(Script).type(Of T1, T2, T3, T4, T5, T6, T7).fin
            func.result = func.method(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Inintialize_output)
        End Function

        <Extension, Method(inline)>
        Public Function [sub](Of T1, T2, T3, T4, T5, T6, T7, T8)(Script As String, Arg1 As T1, Arg2 As T2, Arg3 As T3, Arg4 As T4, Arg5 As T5, Arg6 As T6, Arg7 As T7, Arg8 As T8) As incident(Of T1, T2, T3, T4, T5, T6, T7, T8)
            [sub] = New ils(Of Action(Of T1, T2, T3, T4, T5, T6, T7, T8))(Script).type(Of T1, T2, T3, T4, T5, T6, T7, T8).fin
            [sub].method(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8)
        End Function

        <Extension, Method(inline)>
        Public Function func(Of T1, T2, T3, T4, T5, T6, T7, T8)(Script As String, Arg1 As T1, Arg2 As T2, Arg3 As T3, Arg4 As T4, Arg5 As T5, Arg6 As T6, Arg7 As T7, Inintialize_output As T8) As solution(Of T1, T2, T3, T4, T5, T6, T7, T8)
            func.method = New ils(Of Func(Of T1, T2, T3, T4, T5, T6, T7, T8, T8))(Script).type(Of T1, T2, T3, T4, T5, T6, T7, T8).fin
            func.result = func.method(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Inintialize_output)
        End Function

        <Extension, Method(inline)>
        Public Function [sub](Of T1, T2, T3, T4, T5, T6, T7, T8, T9)(Script As String, Arg1 As T1, Arg2 As T2, Arg3 As T3, Arg4 As T4, Arg5 As T5, Arg6 As T6, Arg7 As T7, Arg8 As T8, Arg9 As T9) As incident(Of T1, T2, T3, T4, T5, T6, T7, T8, T9)
            [sub] = New ils(Of Action(Of T1, T2, T3, T4, T5, T6, T7, T8, T9))(Script).type(Of T1, T2, T3, T4, T5, T6, T7, T8, T9).fin
            [sub].method(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9)
        End Function

        <Extension, Method(inline)>
        Public Function func(Of T1, T2, T3, T4, T5, T6, T7, T8, T9)(Script As String, Arg1 As T1, Arg2 As T2, Arg3 As T3, Arg4 As T4, Arg5 As T5, Arg6 As T6, Arg7 As T7, Arg8 As T8, Inintialize_output As T9) As solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9)
            func.method = New ils(Of Func(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T9))(Script).type(Of T1, T2, T3, T4, T5, T6, T7, T8, T9).fin
            func.result = func.method(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Inintialize_output)
        End Function

        <Extension, Method(inline)>
        Public Function [sub](Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)(Script As String, Arg1 As T1, Arg2 As T2, Arg3 As T3, Arg4 As T4, Arg5 As T5, Arg6 As T6, Arg7 As T7, Arg8 As T8, Arg9 As T9, Arg10 As T10) As incident(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)
            [sub] = New ils(Of Action(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10))(Script).type(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).fin
            [sub].method(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10)
        End Function

        <Extension, Method(inline)>
        Public Function func(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)(Script As String, Arg1 As T1, Arg2 As T2, Arg3 As T3, Arg4 As T4, Arg5 As T5, Arg6 As T6, Arg7 As T7, Arg8 As T8, Arg9 As T9, Inintialize_output As T10) As solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)
            func.method = New ils(Of Func(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T10))(Script).type(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).fin
            func.result = func.method(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Inintialize_output)
        End Function

        <Extension, Method(inline)>
        Public Function [sub](Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)(Script As String, Arg1 As T1, Arg2 As T2, Arg3 As T3, Arg4 As T4, Arg5 As T5, Arg6 As T6, Arg7 As T7, Arg8 As T8, Arg9 As T9, Arg10 As T10, Arg11 As T11) As incident(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)
            [sub] = New ils(Of Action(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11))(Script).type(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).fin
            [sub].method(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11)
        End Function

        <Extension, Method(inline)>
        Public Function func(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)(Script As String, Arg1 As T1, Arg2 As T2, Arg3 As T3, Arg4 As T4, Arg5 As T5, Arg6 As T6, Arg7 As T7, Arg8 As T8, Arg9 As T9, Arg10 As T10, Inintialize_output As T11) As solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)
            func.method = New ils(Of Func(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T11))(Script).type(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).fin
            func.result = func.method(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Inintialize_output)
        End Function

        <Extension, Method(inline)>
        Public Function [sub](Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)(Script As String, Arg1 As T1, Arg2 As T2, Arg3 As T3, Arg4 As T4, Arg5 As T5, Arg6 As T6, Arg7 As T7, Arg8 As T8, Arg9 As T9, Arg10 As T10, Arg11 As T11, Arg12 As T12) As incident(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)
            [sub] = New ils(Of Action(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12))(Script).type(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).fin
            [sub].method(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12)
        End Function

        <Extension, Method(inline)>
        Public Function func(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)(Script As String, Arg1 As T1, Arg2 As T2, Arg3 As T3, Arg4 As T4, Arg5 As T5, Arg6 As T6, Arg7 As T7, Arg8 As T8, Arg9 As T9, Arg10 As T10, Arg11 As T11, Inintialize_output As T12) As solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)
            func.method = New ils(Of Func(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T12))(Script).type(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).fin
            func.result = func.method(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Inintialize_output)
        End Function

        <Extension, Method(inline)>
        Public Function [sub](Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13)(Script As String, Arg1 As T1, Arg2 As T2, Arg3 As T3, Arg4 As T4, Arg5 As T5, Arg6 As T6, Arg7 As T7, Arg8 As T8, Arg9 As T9, Arg10 As T10, Arg11 As T11, Arg12 As T12, Arg13 As T13) As incident(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13)
            [sub] = New ils(Of Action(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13))(Script).type(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13).fin
            [sub].method(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12, Arg13)
        End Function

        <Extension, Method(inline)>
        Public Function func(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13)(Script As String, Arg1 As T1, Arg2 As T2, Arg3 As T3, Arg4 As T4, Arg5 As T5, Arg6 As T6, Arg7 As T7, Arg8 As T8, Arg9 As T9, Arg10 As T10, Arg11 As T11, Arg12 As T12, Inintialize_output As T13) As solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13)
            func.method = New ils(Of Func(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T13))(Script).type(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13).fin
            func.result = func.method(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12, Inintialize_output)
        End Function

        <Extension, Method(inline)>
        Public Function [sub](Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14)(Script As String, Arg1 As T1, Arg2 As T2, Arg3 As T3, Arg4 As T4, Arg5 As T5, Arg6 As T6, Arg7 As T7, Arg8 As T8, Arg9 As T9, Arg10 As T10, Arg11 As T11, Arg12 As T12, Arg13 As T13, Arg14 As T14) As incident(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14)
            [sub] = New ils(Of Action(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14))(Script).type(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14).fin
            [sub].method(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12, Arg13, Arg14)
        End Function

        <Extension, Method(inline)>
        Public Function func(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14)(Script As String, Arg1 As T1, Arg2 As T2, Arg3 As T3, Arg4 As T4, Arg5 As T5, Arg6 As T6, Arg7 As T7, Arg8 As T8, Arg9 As T9, Arg10 As T10, Arg11 As T11, Arg12 As T12, Arg13 As T13, Inintialize_output As T14) As solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14)
            func.method = New ils(Of Func(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T14))(Script).type(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14).fin
            func.result = func.method(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12, Arg13, Inintialize_output)
        End Function

        <Extension, Method(inline)>
        Public Function [sub](Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15)(Script As String, Arg1 As T1, Arg2 As T2, Arg3 As T3, Arg4 As T4, Arg5 As T5, Arg6 As T6, Arg7 As T7, Arg8 As T8, Arg9 As T9, Arg10 As T10, Arg11 As T11, Arg12 As T12, Arg13 As T13, Arg14 As T14, Arg15 As T15) As incident(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15)
            [sub] = New ils(Of Action(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15))(Script).type(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15).fin
            [sub].method(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12, Arg13, Arg14, Arg15)
        End Function

        <Extension, Method(inline)>
        Public Function func(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15)(Script As String, Arg1 As T1, Arg2 As T2, Arg3 As T3, Arg4 As T4, Arg5 As T5, Arg6 As T6, Arg7 As T7, Arg8 As T8, Arg9 As T9, Arg10 As T10, Arg11 As T11, Arg12 As T12, Arg13 As T13, Arg14 As T14, Inintialize_output As T15) As solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15)
            func.method = New ils(Of Func(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T15))(Script).type(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15).fin
            func.result = func.method(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12, Arg13, Arg14, Inintialize_output)
        End Function

        <Extension, Method(inline)>
        Public Function [sub](Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16)(Script As String, Arg1 As T1, Arg2 As T2, Arg3 As T3, Arg4 As T4, Arg5 As T5, Arg6 As T6, Arg7 As T7, Arg8 As T8, Arg9 As T9, Arg10 As T10, Arg11 As T11, Arg12 As T12, Arg13 As T13, Arg14 As T14, Arg15 As T15, Arg16 As T16) As incident(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16)
            [sub] = New ils(Of Action(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16))(Script).type(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16).fin
            [sub].method(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12, Arg13, Arg14, Arg15, Arg16)
        End Function

        <Extension, Method(inline)>
        Public Function func(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16)(Script As String, Arg1 As T1, Arg2 As T2, Arg3 As T3, Arg4 As T4, Arg5 As T5, Arg6 As T6, Arg7 As T7, Arg8 As T8, Arg9 As T9, Arg10 As T10, Arg11 As T11, Arg12 As T12, Arg13 As T13, Arg14 As T14, Arg15 As T15, Inintialize_output As T16) As solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16)
            func.method = New ils(Of Func(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T16))(Script).type(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16).fin
            func.result = func.method(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12, Arg13, Arg14, Arg15, Inintialize_output)
        End Function


    End Module
End Namespace