Namespace ILS
    Partial Friend Module Generate
        Friend Sub gen_incident()
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

    Public Structure incident(Of {T0})
        <Method(inline)>
        Public Shared Function [of](Input As Action(Of {T0})) As incident(Of {T0})
            Return Input
        End Function

        Public method As Action(Of {T0})

        Default Public ReadOnly Property recall({T1}) As incident(Of {T0})
            <Method(inline)>
            Get
                method({T2})
                Return Me
            End Get
        End Property

        <Method(inline)>
        Public Shared Widening Operator CType(Input As Action(Of {T0})) As incident(Of {T0})
            Return New incident(Of {T0}) With {{ .method = Input}}
        End Operator

        <Method(inline)>
        Public Shared Widening Operator CType(Input As incident(Of {T0})) As Action(Of {T0})
            Return Input.method
        End Operator
    End Structure").
                        ToString)
                Next
            End With
        End Sub
    End Module

    Public Structure incident
        Public Shared Function [of](Input As Action) As incident
            Return Input
        End Function
        Public method As Action

        'Public ReadOnly Property recall() As incident
        '    Get
        '        method()
        '        Return Me
        '    End Get
        'End Property

        <Method(inline)>
        Public Shared Widening Operator CType(Input As Action) As incident
            Return New incident With {.method = Input}
        End Operator

        <Method(inline)>
        Public Shared Widening Operator CType(Input As incident) As Action
            Return Input.method
        End Operator
    End Structure

    Public Structure incident(Of T1)
        <Method(inline)>
        Public Shared Function [of](Input As Action(Of T1)) As incident(Of T1)
            Return Input
        End Function

        Public method As Action(Of T1)

        Default Public ReadOnly Property recall(Arg1 As T1) As incident(Of T1)
            <Method(inline)>
            Get
                method(Arg1)
                Return Me
            End Get
        End Property

        <Method(inline)>
        Public Shared Widening Operator CType(Input As Action(Of T1)) As incident(Of T1)
            Return New incident(Of T1) With {.method = Input}
        End Operator

        <Method(inline)>
        Public Shared Widening Operator CType(Input As incident(Of T1)) As Action(Of T1)
            Return Input.method
        End Operator
    End Structure


    Public Structure incident(Of T1, T2)
        <Method(inline)>
        Public Shared Function [of](Input As Action(Of T1, T2)) As incident(Of T1, T2)
            Return Input
        End Function

        Public method As Action(Of T1, T2)

        Default Public ReadOnly Property recall(Arg1 As T1, Arg2 As T2) As incident(Of T1, T2)
            <Method(inline)>
            Get
                method(Arg1, Arg2)
                Return Me
            End Get
        End Property

        <Method(inline)>
        Public Shared Widening Operator CType(Input As Action(Of T1, T2)) As incident(Of T1, T2)
            Return New incident(Of T1, T2) With {.method = Input}
        End Operator

        <Method(inline)>
        Public Shared Widening Operator CType(Input As incident(Of T1, T2)) As Action(Of T1, T2)
            Return Input.method
        End Operator
    End Structure


    Public Structure incident(Of T1, T2, T3)
        <Method(inline)>
        Public Shared Function [of](Input As Action(Of T1, T2, T3)) As incident(Of T1, T2, T3)
            Return Input
        End Function

        Public method As Action(Of T1, T2, T3)

        Default Public ReadOnly Property recall(Arg1 As T1, Arg2 As T2, Arg3 As T3) As incident(Of T1, T2, T3)
            <Method(inline)>
            Get
                method(Arg1, Arg2, Arg3)
                Return Me
            End Get
        End Property

        <Method(inline)>
        Public Shared Widening Operator CType(Input As Action(Of T1, T2, T3)) As incident(Of T1, T2, T3)
            Return New incident(Of T1, T2, T3) With {.method = Input}
        End Operator

        <Method(inline)>
        Public Shared Widening Operator CType(Input As incident(Of T1, T2, T3)) As Action(Of T1, T2, T3)
            Return Input.method
        End Operator
    End Structure


    Public Structure incident(Of T1, T2, T3, T4)
        <Method(inline)>
        Public Shared Function [of](Input As Action(Of T1, T2, T3, T4)) As incident(Of T1, T2, T3, T4)
            Return Input
        End Function

        Public method As Action(Of T1, T2, T3, T4)

        Default Public ReadOnly Property recall(Arg1 As T1, Arg2 As T2, Arg3 As T3, Arg4 As T4) As incident(Of T1, T2, T3, T4)
            <Method(inline)>
            Get
                method(Arg1, Arg2, Arg3, Arg4)
                Return Me
            End Get
        End Property

        <Method(inline)>
        Public Shared Widening Operator CType(Input As Action(Of T1, T2, T3, T4)) As incident(Of T1, T2, T3, T4)
            Return New incident(Of T1, T2, T3, T4) With {.method = Input}
        End Operator

        <Method(inline)>
        Public Shared Widening Operator CType(Input As incident(Of T1, T2, T3, T4)) As Action(Of T1, T2, T3, T4)
            Return Input.method
        End Operator
    End Structure


    Public Structure incident(Of T1, T2, T3, T4, T5)
        <Method(inline)>
        Public Shared Function [of](Input As Action(Of T1, T2, T3, T4, T5)) As incident(Of T1, T2, T3, T4, T5)
            Return Input
        End Function

        Public method As Action(Of T1, T2, T3, T4, T5)

        Default Public ReadOnly Property recall(Arg1 As T1, Arg2 As T2, Arg3 As T3, Arg4 As T4, Arg5 As T5) As incident(Of T1, T2, T3, T4, T5)
            <Method(inline)>
            Get
                method(Arg1, Arg2, Arg3, Arg4, Arg5)
                Return Me
            End Get
        End Property

        <Method(inline)>
        Public Shared Widening Operator CType(Input As Action(Of T1, T2, T3, T4, T5)) As incident(Of T1, T2, T3, T4, T5)
            Return New incident(Of T1, T2, T3, T4, T5) With {.method = Input}
        End Operator

        <Method(inline)>
        Public Shared Widening Operator CType(Input As incident(Of T1, T2, T3, T4, T5)) As Action(Of T1, T2, T3, T4, T5)
            Return Input.method
        End Operator
    End Structure


    Public Structure incident(Of T1, T2, T3, T4, T5, T6)
        <Method(inline)>
        Public Shared Function [of](Input As Action(Of T1, T2, T3, T4, T5, T6)) As incident(Of T1, T2, T3, T4, T5, T6)
            Return Input
        End Function

        Public method As Action(Of T1, T2, T3, T4, T5, T6)

        Default Public ReadOnly Property recall(Arg1 As T1, Arg2 As T2, Arg3 As T3, Arg4 As T4, Arg5 As T5, Arg6 As T6) As incident(Of T1, T2, T3, T4, T5, T6)
            <Method(inline)>
            Get
                method(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6)
                Return Me
            End Get
        End Property

        <Method(inline)>
        Public Shared Widening Operator CType(Input As Action(Of T1, T2, T3, T4, T5, T6)) As incident(Of T1, T2, T3, T4, T5, T6)
            Return New incident(Of T1, T2, T3, T4, T5, T6) With {.method = Input}
        End Operator

        <Method(inline)>
        Public Shared Widening Operator CType(Input As incident(Of T1, T2, T3, T4, T5, T6)) As Action(Of T1, T2, T3, T4, T5, T6)
            Return Input.method
        End Operator
    End Structure


    Public Structure incident(Of T1, T2, T3, T4, T5, T6, T7)
        <Method(inline)>
        Public Shared Function [of](Input As Action(Of T1, T2, T3, T4, T5, T6, T7)) As incident(Of T1, T2, T3, T4, T5, T6, T7)
            Return Input
        End Function

        Public method As Action(Of T1, T2, T3, T4, T5, T6, T7)

        Default Public ReadOnly Property recall(Arg1 As T1, Arg2 As T2, Arg3 As T3, Arg4 As T4, Arg5 As T5, Arg6 As T6, Arg7 As T7) As incident(Of T1, T2, T3, T4, T5, T6, T7)
            <Method(inline)>
            Get
                method(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7)
                Return Me
            End Get
        End Property

        <Method(inline)>
        Public Shared Widening Operator CType(Input As Action(Of T1, T2, T3, T4, T5, T6, T7)) As incident(Of T1, T2, T3, T4, T5, T6, T7)
            Return New incident(Of T1, T2, T3, T4, T5, T6, T7) With {.method = Input}
        End Operator

        <Method(inline)>
        Public Shared Widening Operator CType(Input As incident(Of T1, T2, T3, T4, T5, T6, T7)) As Action(Of T1, T2, T3, T4, T5, T6, T7)
            Return Input.method
        End Operator
    End Structure


    Public Structure incident(Of T1, T2, T3, T4, T5, T6, T7, T8)
        <Method(inline)>
        Public Shared Function [of](Input As Action(Of T1, T2, T3, T4, T5, T6, T7, T8)) As incident(Of T1, T2, T3, T4, T5, T6, T7, T8)
            Return Input
        End Function

        Public method As Action(Of T1, T2, T3, T4, T5, T6, T7, T8)

        Default Public ReadOnly Property recall(Arg1 As T1, Arg2 As T2, Arg3 As T3, Arg4 As T4, Arg5 As T5, Arg6 As T6, Arg7 As T7, Arg8 As T8) As incident(Of T1, T2, T3, T4, T5, T6, T7, T8)
            <Method(inline)>
            Get
                method(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8)
                Return Me
            End Get
        End Property

        <Method(inline)>
        Public Shared Widening Operator CType(Input As Action(Of T1, T2, T3, T4, T5, T6, T7, T8)) As incident(Of T1, T2, T3, T4, T5, T6, T7, T8)
            Return New incident(Of T1, T2, T3, T4, T5, T6, T7, T8) With {.method = Input}
        End Operator

        <Method(inline)>
        Public Shared Widening Operator CType(Input As incident(Of T1, T2, T3, T4, T5, T6, T7, T8)) As Action(Of T1, T2, T3, T4, T5, T6, T7, T8)
            Return Input.method
        End Operator
    End Structure


    Public Structure incident(Of T1, T2, T3, T4, T5, T6, T7, T8, T9)
        <Method(inline)>
        Public Shared Function [of](Input As Action(Of T1, T2, T3, T4, T5, T6, T7, T8, T9)) As incident(Of T1, T2, T3, T4, T5, T6, T7, T8, T9)
            Return Input
        End Function

        Public method As Action(Of T1, T2, T3, T4, T5, T6, T7, T8, T9)

        Default Public ReadOnly Property recall(Arg1 As T1, Arg2 As T2, Arg3 As T3, Arg4 As T4, Arg5 As T5, Arg6 As T6, Arg7 As T7, Arg8 As T8, Arg9 As T9) As incident(Of T1, T2, T3, T4, T5, T6, T7, T8, T9)
            <Method(inline)>
            Get
                method(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9)
                Return Me
            End Get
        End Property

        <Method(inline)>
        Public Shared Widening Operator CType(Input As Action(Of T1, T2, T3, T4, T5, T6, T7, T8, T9)) As incident(Of T1, T2, T3, T4, T5, T6, T7, T8, T9)
            Return New incident(Of T1, T2, T3, T4, T5, T6, T7, T8, T9) With {.method = Input}
        End Operator

        <Method(inline)>
        Public Shared Widening Operator CType(Input As incident(Of T1, T2, T3, T4, T5, T6, T7, T8, T9)) As Action(Of T1, T2, T3, T4, T5, T6, T7, T8, T9)
            Return Input.method
        End Operator
    End Structure


    Public Structure incident(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)
        <Method(inline)>
        Public Shared Function [of](Input As Action(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)) As incident(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)
            Return Input
        End Function

        Public method As Action(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)

        Default Public ReadOnly Property recall(Arg1 As T1, Arg2 As T2, Arg3 As T3, Arg4 As T4, Arg5 As T5, Arg6 As T6, Arg7 As T7, Arg8 As T8, Arg9 As T9, Arg10 As T10) As incident(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)
            <Method(inline)>
            Get
                method(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10)
                Return Me
            End Get
        End Property

        <Method(inline)>
        Public Shared Widening Operator CType(Input As Action(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)) As incident(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)
            Return New incident(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10) With {.method = Input}
        End Operator

        <Method(inline)>
        Public Shared Widening Operator CType(Input As incident(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)) As Action(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)
            Return Input.method
        End Operator
    End Structure


    Public Structure incident(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)
        <Method(inline)>
        Public Shared Function [of](Input As Action(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)) As incident(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)
            Return Input
        End Function

        Public method As Action(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)

        Default Public ReadOnly Property recall(Arg1 As T1, Arg2 As T2, Arg3 As T3, Arg4 As T4, Arg5 As T5, Arg6 As T6, Arg7 As T7, Arg8 As T8, Arg9 As T9, Arg10 As T10, Arg11 As T11) As incident(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)
            <Method(inline)>
            Get
                method(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11)
                Return Me
            End Get
        End Property

        <Method(inline)>
        Public Shared Widening Operator CType(Input As Action(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)) As incident(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)
            Return New incident(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11) With {.method = Input}
        End Operator

        <Method(inline)>
        Public Shared Widening Operator CType(Input As incident(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)) As Action(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)
            Return Input.method
        End Operator
    End Structure


    Public Structure incident(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)
        <Method(inline)>
        Public Shared Function [of](Input As Action(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)) As incident(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)
            Return Input
        End Function

        Public method As Action(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)

        Default Public ReadOnly Property recall(Arg1 As T1, Arg2 As T2, Arg3 As T3, Arg4 As T4, Arg5 As T5, Arg6 As T6, Arg7 As T7, Arg8 As T8, Arg9 As T9, Arg10 As T10, Arg11 As T11, Arg12 As T12) As incident(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)
            <Method(inline)>
            Get
                method(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12)
                Return Me
            End Get
        End Property

        <Method(inline)>
        Public Shared Widening Operator CType(Input As Action(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)) As incident(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)
            Return New incident(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12) With {.method = Input}
        End Operator

        <Method(inline)>
        Public Shared Widening Operator CType(Input As incident(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)) As Action(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)
            Return Input.method
        End Operator
    End Structure


    Public Structure incident(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13)
        <Method(inline)>
        Public Shared Function [of](Input As Action(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13)) As incident(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13)
            Return Input
        End Function

        Public method As Action(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13)

        Default Public ReadOnly Property recall(Arg1 As T1, Arg2 As T2, Arg3 As T3, Arg4 As T4, Arg5 As T5, Arg6 As T6, Arg7 As T7, Arg8 As T8, Arg9 As T9, Arg10 As T10, Arg11 As T11, Arg12 As T12, Arg13 As T13) As incident(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13)
            <Method(inline)>
            Get
                method(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12, Arg13)
                Return Me
            End Get
        End Property

        <Method(inline)>
        Public Shared Widening Operator CType(Input As Action(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13)) As incident(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13)
            Return New incident(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13) With {.method = Input}
        End Operator

        <Method(inline)>
        Public Shared Widening Operator CType(Input As incident(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13)) As Action(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13)
            Return Input.method
        End Operator
    End Structure


    Public Structure incident(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14)
        <Method(inline)>
        Public Shared Function [of](Input As Action(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14)) As incident(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14)
            Return Input
        End Function

        Public method As Action(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14)

        Default Public ReadOnly Property recall(Arg1 As T1, Arg2 As T2, Arg3 As T3, Arg4 As T4, Arg5 As T5, Arg6 As T6, Arg7 As T7, Arg8 As T8, Arg9 As T9, Arg10 As T10, Arg11 As T11, Arg12 As T12, Arg13 As T13, Arg14 As T14) As incident(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14)
            <Method(inline)>
            Get
                method(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12, Arg13, Arg14)
                Return Me
            End Get
        End Property

        <Method(inline)>
        Public Shared Widening Operator CType(Input As Action(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14)) As incident(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14)
            Return New incident(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14) With {.method = Input}
        End Operator

        <Method(inline)>
        Public Shared Widening Operator CType(Input As incident(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14)) As Action(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14)
            Return Input.method
        End Operator
    End Structure


    Public Structure incident(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15)
        <Method(inline)>
        Public Shared Function [of](Input As Action(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15)) As incident(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15)
            Return Input
        End Function

        Public method As Action(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15)

        Default Public ReadOnly Property recall(Arg1 As T1, Arg2 As T2, Arg3 As T3, Arg4 As T4, Arg5 As T5, Arg6 As T6, Arg7 As T7, Arg8 As T8, Arg9 As T9, Arg10 As T10, Arg11 As T11, Arg12 As T12, Arg13 As T13, Arg14 As T14, Arg15 As T15) As incident(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15)
            <Method(inline)>
            Get
                method(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12, Arg13, Arg14, Arg15)
                Return Me
            End Get
        End Property

        <Method(inline)>
        Public Shared Widening Operator CType(Input As Action(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15)) As incident(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15)
            Return New incident(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15) With {.method = Input}
        End Operator

        <Method(inline)>
        Public Shared Widening Operator CType(Input As incident(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15)) As Action(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15)
            Return Input.method
        End Operator
    End Structure


    Public Structure incident(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16)
        <Method(inline)>
        Public Shared Function [of](Input As Action(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16)) As incident(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16)
            Return Input
        End Function

        Public method As Action(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16)

        Default Public ReadOnly Property recall(Arg1 As T1, Arg2 As T2, Arg3 As T3, Arg4 As T4, Arg5 As T5, Arg6 As T6, Arg7 As T7, Arg8 As T8, Arg9 As T9, Arg10 As T10, Arg11 As T11, Arg12 As T12, Arg13 As T13, Arg14 As T14, Arg15 As T15, Arg16 As T16) As incident(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16)
            <Method(inline)>
            Get
                method(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12, Arg13, Arg14, Arg15, Arg16)
                Return Me
            End Get
        End Property

        <Method(inline)>
        Public Shared Widening Operator CType(Input As Action(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16)) As incident(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16)
            Return New incident(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16) With {.method = Input}
        End Operator

        <Method(inline)>
        Public Shared Widening Operator CType(Input As incident(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16)) As Action(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16)
            Return Input.method
        End Operator
    End Structure
End Namespace