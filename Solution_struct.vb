Namespace ILS
    Partial Friend Module Generate
        Friend Sub gen_solution()
            Dim Listing = Function(SB As Text.StringBuilder, N As Integer, Body As Func(Of Integer, String))
                              Dim I As Int32
                              For I = 1 To N - 1
                                  SB.Append($"{Body(I)}, ")
                              Next
                              Return SB.Append(Body(I))
                          End Function

            With New Text.StringBuilder

                For i = 1 To 16

                    'T1, T2, ... TN
                    Dim T0 = Listing(.Clear, i, Function(N) $"T{N}").ToString

                    'Arg1 As T1, Arg2 As T2, ... ArgN As TN
                    Dim T1 = Listing(.Clear, i, Function(N) $"Arg{N} As T{N}").ToString

                    Dim T2 = Listing(.Clear, i, Function(N) $"Arg{N}").ToString

                    Dim D1 = $"Func(Of {T0}, T{i})"
                    Dim D2 = $"({D1}, T{i})"


                    Diagnostics.Debug.WriteLine(.Clear.Append($"
    Public Structure solution(Of {T0})
        Public method As {D1}, result As T{i}

        Default Public ReadOnly Property recall({T1}) As solution(Of {T0})
            Get
                Return New solution(Of {T0}) With {{.method = method, .result = method({T2})}}
            End Get
        End Property

        <Method(inline)>
        Public Shared Widening Operator CType(Input As {D2}) As solution(Of {T0})
            Return New solution(Of {T0}) With {{.method = Input.Item1, .result = Input.Item2}}
        End Operator

        <Method(inline)>
        Public Shared Widening Operator CType(Input As solution(Of {T0})) As {D2}
            Return (Input.method, Input.result)
        End Operator
        <Method(inline)>
        Public Shared Widening Operator CType(Input As solution(Of {T0})) As {D1}
            Return Input.method
        End Operator
        <Method(inline)>
        Public Shared Widening Operator CType(Input As solution(Of {T0})) As T{i}
            Return Input.result
        End Operator
    End Structure").
                        ToString)
                Next
            End With
        End Sub
    End Module


    Public Structure solution(Of T1)
        Public method As Func(Of T1, T1), result As T1

        Default Public ReadOnly Property recall(Arg1 As T1) As solution(Of T1)
            Get
                Return New solution(Of T1) With {.method = method, .result = method(Arg1)}
            End Get
        End Property

        <Method(inline)>
        Public Shared Widening Operator CType(Input As (Func(Of T1, T1), T1)) As solution(Of T1)
            Return New solution(Of T1) With {.method = Input.Item1, .result = Input.Item2}
        End Operator

        <Method(inline)>
        Public Shared Widening Operator CType(Input As solution(Of T1)) As (Func(Of T1, T1), T1)
            Return (Input.method, Input.result)
        End Operator
        <Method(inline)>
        Public Shared Widening Operator CType(Input As solution(Of T1)) As Func(Of T1, T1)
            Return Input.method
        End Operator
        <Method(inline)>
        Public Shared Widening Operator CType(Input As solution(Of T1)) As T1
            Return Input.result
        End Operator
    End Structure

    Public Structure solution(Of T1, T2)
        Public method As Func(Of T1, T2, T2), result As T2

        Default Public ReadOnly Property recall(Arg1 As T1, Arg2 As T2) As solution(Of T1, T2)
            Get
                Return New solution(Of T1, T2) With {.method = method, .result = method(Arg1, Arg2)}
            End Get
        End Property

        <Method(inline)>
        Public Shared Widening Operator CType(Input As (Func(Of T1, T2, T2), T2)) As solution(Of T1, T2)
            Return New solution(Of T1, T2) With {.method = Input.Item1, .result = Input.Item2}
        End Operator

        <Method(inline)>
        Public Shared Widening Operator CType(Input As solution(Of T1, T2)) As (Func(Of T1, T2, T2), T2)
            Return (Input.method, Input.result)
        End Operator
        <Method(inline)>
        Public Shared Widening Operator CType(Input As solution(Of T1, T2)) As Func(Of T1, T2, T2)
            Return Input.method
        End Operator
        <Method(inline)>
        Public Shared Widening Operator CType(Input As solution(Of T1, T2)) As T2
            Return Input.result
        End Operator
    End Structure

    Public Structure solution(Of T1, T2, T3)
        Public method As Func(Of T1, T2, T3, T3), result As T3

        Default Public ReadOnly Property recall(Arg1 As T1, Arg2 As T2, Arg3 As T3) As solution(Of T1, T2, T3)
            Get
                Return New solution(Of T1, T2, T3) With {.method = method, .result = method(Arg1, Arg2, Arg3)}
            End Get
        End Property

        <Method(inline)>
        Public Shared Widening Operator CType(Input As (Func(Of T1, T2, T3, T3), T3)) As solution(Of T1, T2, T3)
            Return New solution(Of T1, T2, T3) With {.method = Input.Item1, .result = Input.Item2}
        End Operator

        <Method(inline)>
        Public Shared Widening Operator CType(Input As solution(Of T1, T2, T3)) As (Func(Of T1, T2, T3, T3), T3)
            Return (Input.method, Input.result)
        End Operator
        <Method(inline)>
        Public Shared Widening Operator CType(Input As solution(Of T1, T2, T3)) As Func(Of T1, T2, T3, T3)
            Return Input.method
        End Operator
        <Method(inline)>
        Public Shared Widening Operator CType(Input As solution(Of T1, T2, T3)) As T3
            Return Input.result
        End Operator
    End Structure

    Public Structure solution(Of T1, T2, T3, T4)
        Public method As Func(Of T1, T2, T3, T4, T4), result As T4

        Default Public ReadOnly Property recall(Arg1 As T1, Arg2 As T2, Arg3 As T3, Arg4 As T4) As solution(Of T1, T2, T3, T4)
            Get
                Return New solution(Of T1, T2, T3, T4) With {.method = method, .result = method(Arg1, Arg2, Arg3, Arg4)}
            End Get
        End Property

        <Method(inline)>
        Public Shared Widening Operator CType(Input As (Func(Of T1, T2, T3, T4, T4), T4)) As solution(Of T1, T2, T3, T4)
            Return New solution(Of T1, T2, T3, T4) With {.method = Input.Item1, .result = Input.Item2}
        End Operator

        <Method(inline)>
        Public Shared Widening Operator CType(Input As solution(Of T1, T2, T3, T4)) As (Func(Of T1, T2, T3, T4, T4), T4)
            Return (Input.method, Input.result)
        End Operator
        <Method(inline)>
        Public Shared Widening Operator CType(Input As solution(Of T1, T2, T3, T4)) As Func(Of T1, T2, T3, T4, T4)
            Return Input.method
        End Operator
        <Method(inline)>
        Public Shared Widening Operator CType(Input As solution(Of T1, T2, T3, T4)) As T4
            Return Input.result
        End Operator
    End Structure

    Public Structure solution(Of T1, T2, T3, T4, T5)
        Public method As Func(Of T1, T2, T3, T4, T5, T5), result As T5

        Default Public ReadOnly Property recall(Arg1 As T1, Arg2 As T2, Arg3 As T3, Arg4 As T4, Arg5 As T5) As solution(Of T1, T2, T3, T4, T5)
            Get
                Return New solution(Of T1, T2, T3, T4, T5) With {.method = method, .result = method(Arg1, Arg2, Arg3, Arg4, Arg5)}
            End Get
        End Property

        <Method(inline)>
        Public Shared Widening Operator CType(Input As (Func(Of T1, T2, T3, T4, T5, T5), T5)) As solution(Of T1, T2, T3, T4, T5)
            Return New solution(Of T1, T2, T3, T4, T5) With {.method = Input.Item1, .result = Input.Item2}
        End Operator

        <Method(inline)>
        Public Shared Widening Operator CType(Input As solution(Of T1, T2, T3, T4, T5)) As (Func(Of T1, T2, T3, T4, T5, T5), T5)
            Return (Input.method, Input.result)
        End Operator
        <Method(inline)>
        Public Shared Widening Operator CType(Input As solution(Of T1, T2, T3, T4, T5)) As Func(Of T1, T2, T3, T4, T5, T5)
            Return Input.method
        End Operator
        <Method(inline)>
        Public Shared Widening Operator CType(Input As solution(Of T1, T2, T3, T4, T5)) As T5
            Return Input.result
        End Operator
    End Structure

    Public Structure solution(Of T1, T2, T3, T4, T5, T6)
        Public method As Func(Of T1, T2, T3, T4, T5, T6, T6), result As T6

        Default Public ReadOnly Property recall(Arg1 As T1, Arg2 As T2, Arg3 As T3, Arg4 As T4, Arg5 As T5, Arg6 As T6) As solution(Of T1, T2, T3, T4, T5, T6)
            Get
                Return New solution(Of T1, T2, T3, T4, T5, T6) With {.method = method, .result = method(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6)}
            End Get
        End Property

        <Method(inline)>
        Public Shared Widening Operator CType(Input As (Func(Of T1, T2, T3, T4, T5, T6, T6), T6)) As solution(Of T1, T2, T3, T4, T5, T6)
            Return New solution(Of T1, T2, T3, T4, T5, T6) With {.method = Input.Item1, .result = Input.Item2}
        End Operator

        <Method(inline)>
        Public Shared Widening Operator CType(Input As solution(Of T1, T2, T3, T4, T5, T6)) As (Func(Of T1, T2, T3, T4, T5, T6, T6), T6)
            Return (Input.method, Input.result)
        End Operator
        <Method(inline)>
        Public Shared Widening Operator CType(Input As solution(Of T1, T2, T3, T4, T5, T6)) As Func(Of T1, T2, T3, T4, T5, T6, T6)
            Return Input.method
        End Operator
        <Method(inline)>
        Public Shared Widening Operator CType(Input As solution(Of T1, T2, T3, T4, T5, T6)) As T6
            Return Input.result
        End Operator
    End Structure

    Public Structure solution(Of T1, T2, T3, T4, T5, T6, T7)
        Public method As Func(Of T1, T2, T3, T4, T5, T6, T7, T7), result As T7

        Default Public ReadOnly Property recall(Arg1 As T1, Arg2 As T2, Arg3 As T3, Arg4 As T4, Arg5 As T5, Arg6 As T6, Arg7 As T7) As solution(Of T1, T2, T3, T4, T5, T6, T7)
            Get
                Return New solution(Of T1, T2, T3, T4, T5, T6, T7) With {.method = method, .result = method(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7)}
            End Get
        End Property

        <Method(inline)>
        Public Shared Widening Operator CType(Input As (Func(Of T1, T2, T3, T4, T5, T6, T7, T7), T7)) As solution(Of T1, T2, T3, T4, T5, T6, T7)
            Return New solution(Of T1, T2, T3, T4, T5, T6, T7) With {.method = Input.Item1, .result = Input.Item2}
        End Operator

        <Method(inline)>
        Public Shared Widening Operator CType(Input As solution(Of T1, T2, T3, T4, T5, T6, T7)) As (Func(Of T1, T2, T3, T4, T5, T6, T7, T7), T7)
            Return (Input.method, Input.result)
        End Operator
        <Method(inline)>
        Public Shared Widening Operator CType(Input As solution(Of T1, T2, T3, T4, T5, T6, T7)) As Func(Of T1, T2, T3, T4, T5, T6, T7, T7)
            Return Input.method
        End Operator
        <Method(inline)>
        Public Shared Widening Operator CType(Input As solution(Of T1, T2, T3, T4, T5, T6, T7)) As T7
            Return Input.result
        End Operator
    End Structure

    Public Structure solution(Of T1, T2, T3, T4, T5, T6, T7, T8)
        Public method As Func(Of T1, T2, T3, T4, T5, T6, T7, T8, T8), result As T8

        Default Public ReadOnly Property recall(Arg1 As T1, Arg2 As T2, Arg3 As T3, Arg4 As T4, Arg5 As T5, Arg6 As T6, Arg7 As T7, Arg8 As T8) As solution(Of T1, T2, T3, T4, T5, T6, T7, T8)
            Get
                Return New solution(Of T1, T2, T3, T4, T5, T6, T7, T8) With {.method = method, .result = method(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8)}
            End Get
        End Property

        <Method(inline)>
        Public Shared Widening Operator CType(Input As (Func(Of T1, T2, T3, T4, T5, T6, T7, T8, T8), T8)) As solution(Of T1, T2, T3, T4, T5, T6, T7, T8)
            Return New solution(Of T1, T2, T3, T4, T5, T6, T7, T8) With {.method = Input.Item1, .result = Input.Item2}
        End Operator

        <Method(inline)>
        Public Shared Widening Operator CType(Input As solution(Of T1, T2, T3, T4, T5, T6, T7, T8)) As (Func(Of T1, T2, T3, T4, T5, T6, T7, T8, T8), T8)
            Return (Input.method, Input.result)
        End Operator
        <Method(inline)>
        Public Shared Widening Operator CType(Input As solution(Of T1, T2, T3, T4, T5, T6, T7, T8)) As Func(Of T1, T2, T3, T4, T5, T6, T7, T8, T8)
            Return Input.method
        End Operator
        <Method(inline)>
        Public Shared Widening Operator CType(Input As solution(Of T1, T2, T3, T4, T5, T6, T7, T8)) As T8
            Return Input.result
        End Operator
    End Structure

    Public Structure solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9)
        Public method As Func(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T9), result As T9

        Default Public ReadOnly Property recall(Arg1 As T1, Arg2 As T2, Arg3 As T3, Arg4 As T4, Arg5 As T5, Arg6 As T6, Arg7 As T7, Arg8 As T8, Arg9 As T9) As solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9)
            Get
                Return New solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9) With {.method = method, .result = method(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9)}
            End Get
        End Property

        <Method(inline)>
        Public Shared Widening Operator CType(Input As (Func(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T9), T9)) As solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9)
            Return New solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9) With {.method = Input.Item1, .result = Input.Item2}
        End Operator

        <Method(inline)>
        Public Shared Widening Operator CType(Input As solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9)) As (Func(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T9), T9)
            Return (Input.method, Input.result)
        End Operator
        <Method(inline)>
        Public Shared Widening Operator CType(Input As solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9)) As Func(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T9)
            Return Input.method
        End Operator
        <Method(inline)>
        Public Shared Widening Operator CType(Input As solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9)) As T9
            Return Input.result
        End Operator
    End Structure

    Public Structure solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)
        Public method As Func(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T10), result As T10

        Default Public ReadOnly Property recall(Arg1 As T1, Arg2 As T2, Arg3 As T3, Arg4 As T4, Arg5 As T5, Arg6 As T6, Arg7 As T7, Arg8 As T8, Arg9 As T9, Arg10 As T10) As solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)
            Get
                Return New solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10) With {.method = method, .result = method(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10)}
            End Get
        End Property

        <Method(inline)>
        Public Shared Widening Operator CType(Input As (Func(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T10), T10)) As solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)
            Return New solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10) With {.method = Input.Item1, .result = Input.Item2}
        End Operator

        <Method(inline)>
        Public Shared Widening Operator CType(Input As solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)) As (Func(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T10), T10)
            Return (Input.method, Input.result)
        End Operator
        <Method(inline)>
        Public Shared Widening Operator CType(Input As solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)) As Func(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T10)
            Return Input.method
        End Operator
        <Method(inline)>
        Public Shared Widening Operator CType(Input As solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)) As T10
            Return Input.result
        End Operator
    End Structure

    Public Structure solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)
        Public method As Func(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T11), result As T11

        Default Public ReadOnly Property recall(Arg1 As T1, Arg2 As T2, Arg3 As T3, Arg4 As T4, Arg5 As T5, Arg6 As T6, Arg7 As T7, Arg8 As T8, Arg9 As T9, Arg10 As T10, Arg11 As T11) As solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)
            Get
                Return New solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11) With {.method = method, .result = method(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11)}
            End Get
        End Property

        <Method(inline)>
        Public Shared Widening Operator CType(Input As (Func(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T11), T11)) As solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)
            Return New solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11) With {.method = Input.Item1, .result = Input.Item2}
        End Operator

        <Method(inline)>
        Public Shared Widening Operator CType(Input As solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)) As (Func(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T11), T11)
            Return (Input.method, Input.result)
        End Operator
        <Method(inline)>
        Public Shared Widening Operator CType(Input As solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)) As Func(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T11)
            Return Input.method
        End Operator
        <Method(inline)>
        Public Shared Widening Operator CType(Input As solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)) As T11
            Return Input.result
        End Operator
    End Structure

    Public Structure solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)
        Public method As Func(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T12), result As T12

        Default Public ReadOnly Property recall(Arg1 As T1, Arg2 As T2, Arg3 As T3, Arg4 As T4, Arg5 As T5, Arg6 As T6, Arg7 As T7, Arg8 As T8, Arg9 As T9, Arg10 As T10, Arg11 As T11, Arg12 As T12) As solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)
            Get
                Return New solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12) With {.method = method, .result = method(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12)}
            End Get
        End Property

        <Method(inline)>
        Public Shared Widening Operator CType(Input As (Func(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T12), T12)) As solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)
            Return New solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12) With {.method = Input.Item1, .result = Input.Item2}
        End Operator

        <Method(inline)>
        Public Shared Widening Operator CType(Input As solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)) As (Func(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T12), T12)
            Return (Input.method, Input.result)
        End Operator
        <Method(inline)>
        Public Shared Widening Operator CType(Input As solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)) As Func(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T12)
            Return Input.method
        End Operator
        <Method(inline)>
        Public Shared Widening Operator CType(Input As solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)) As T12
            Return Input.result
        End Operator
    End Structure

    Public Structure solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13)
        Public method As Func(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T13), result As T13

        Default Public ReadOnly Property recall(Arg1 As T1, Arg2 As T2, Arg3 As T3, Arg4 As T4, Arg5 As T5, Arg6 As T6, Arg7 As T7, Arg8 As T8, Arg9 As T9, Arg10 As T10, Arg11 As T11, Arg12 As T12, Arg13 As T13) As solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13)
            Get
                Return New solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13) With {.method = method, .result = method(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12, Arg13)}
            End Get
        End Property

        <Method(inline)>
        Public Shared Widening Operator CType(Input As (Func(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T13), T13)) As solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13)
            Return New solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13) With {.method = Input.Item1, .result = Input.Item2}
        End Operator

        <Method(inline)>
        Public Shared Widening Operator CType(Input As solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13)) As (Func(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T13), T13)
            Return (Input.method, Input.result)
        End Operator
        <Method(inline)>
        Public Shared Widening Operator CType(Input As solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13)) As Func(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T13)
            Return Input.method
        End Operator
        <Method(inline)>
        Public Shared Widening Operator CType(Input As solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13)) As T13
            Return Input.result
        End Operator
    End Structure

    Public Structure solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14)
        Public method As Func(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T14), result As T14

        Default Public ReadOnly Property recall(Arg1 As T1, Arg2 As T2, Arg3 As T3, Arg4 As T4, Arg5 As T5, Arg6 As T6, Arg7 As T7, Arg8 As T8, Arg9 As T9, Arg10 As T10, Arg11 As T11, Arg12 As T12, Arg13 As T13, Arg14 As T14) As solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14)
            Get
                Return New solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14) With {.method = method, .result = method(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12, Arg13, Arg14)}
            End Get
        End Property

        <Method(inline)>
        Public Shared Widening Operator CType(Input As (Func(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T14), T14)) As solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14)
            Return New solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14) With {.method = Input.Item1, .result = Input.Item2}
        End Operator

        <Method(inline)>
        Public Shared Widening Operator CType(Input As solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14)) As (Func(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T14), T14)
            Return (Input.method, Input.result)
        End Operator
        <Method(inline)>
        Public Shared Widening Operator CType(Input As solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14)) As Func(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T14)
            Return Input.method
        End Operator
        <Method(inline)>
        Public Shared Widening Operator CType(Input As solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14)) As T14
            Return Input.result
        End Operator
    End Structure

    Public Structure solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15)
        Public method As Func(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T15), result As T15

        Default Public ReadOnly Property recall(Arg1 As T1, Arg2 As T2, Arg3 As T3, Arg4 As T4, Arg5 As T5, Arg6 As T6, Arg7 As T7, Arg8 As T8, Arg9 As T9, Arg10 As T10, Arg11 As T11, Arg12 As T12, Arg13 As T13, Arg14 As T14, Arg15 As T15) As solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15)
            Get
                Return New solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15) With {.method = method, .result = method(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12, Arg13, Arg14, Arg15)}
            End Get
        End Property

        <Method(inline)>
        Public Shared Widening Operator CType(Input As (Func(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T15), T15)) As solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15)
            Return New solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15) With {.method = Input.Item1, .result = Input.Item2}
        End Operator

        <Method(inline)>
        Public Shared Widening Operator CType(Input As solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15)) As (Func(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T15), T15)
            Return (Input.method, Input.result)
        End Operator
        <Method(inline)>
        Public Shared Widening Operator CType(Input As solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15)) As Func(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T15)
            Return Input.method
        End Operator
        <Method(inline)>
        Public Shared Widening Operator CType(Input As solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15)) As T15
            Return Input.result
        End Operator
    End Structure

    Public Structure solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16)
        Public method As Func(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T16), result As T16

        Default Public ReadOnly Property recall(Arg1 As T1, Arg2 As T2, Arg3 As T3, Arg4 As T4, Arg5 As T5, Arg6 As T6, Arg7 As T7, Arg8 As T8, Arg9 As T9, Arg10 As T10, Arg11 As T11, Arg12 As T12, Arg13 As T13, Arg14 As T14, Arg15 As T15, Arg16 As T16) As solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16)
            Get
                Return New solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16) With {.method = method, .result = method(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12, Arg13, Arg14, Arg15, Arg16)}
            End Get
        End Property

        <Method(inline)>
        Public Shared Widening Operator CType(Input As (Func(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T16), T16)) As solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16)
            Return New solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16) With {.method = Input.Item1, .result = Input.Item2}
        End Operator

        <Method(inline)>
        Public Shared Widening Operator CType(Input As solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16)) As (Func(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T16), T16)
            Return (Input.method, Input.result)
        End Operator
        <Method(inline)>
        Public Shared Widening Operator CType(Input As solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16)) As Func(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T16)
            Return Input.method
        End Operator
        <Method(inline)>
        Public Shared Widening Operator CType(Input As solution(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16)) As T16
            Return Input.result
        End Operator
    End Structure

End Namespace