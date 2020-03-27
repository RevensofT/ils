Namespace ILS
    Partial Friend Module Generate
        Friend Sub gen_ils_type()
            Dim Listing = Function(SB As Text.StringBuilder, N As Integer, Body As Func(Of Integer, String))
                              Dim I As Int32
                              For I = 1 To N - 1
                                  SB.Append($"{Body(I)}, ")
                              Next
                              Return SB.Append(Body(I))
                          End Function

            With New Text.StringBuilder

                For i = 2 To 16

                    'T1, T2, ... TN
                    Dim T0 = Listing(.Clear, i, Function(N) $"T{N}").ToString

                    Diagnostics.Debug.WriteLine(.Clear.Append($"
        <Method(inline)>
        Public Function type(Of {T0})() As ils(Of T)
            types = Info.type(Of {T0}).types
            Return Me
        End Function").
                        ToString)
                Next
            End With
        End Sub
    End Module

    Partial Public Structure ils(Of T As Class)

#Region ".field"
        <Method(inline)>
        Public Function field(Of T1)(Name1 As String) As ils(Of T)
            fields = {Info.type(Of T1).field(Name1)}
            Return Me
        End Function

        <Method(inline)>
        Public Function field(Of T2, T1)(Name2 As String, Name1 As String) As ils(Of T)
            fields = {Info.type(Of T2).field(Name2), Info.type(Of T1).field(Name1)}
            Return Me
        End Function

        <Method(inline)>
        Public Function field(Of T3, T2, T1)(Name3 As String, Name2 As String, Name1 As String) As ils(Of T)
            fields = {Info.type(Of T3).field(Name3), Info.type(Of T2).field(Name2), Info.type(Of T1).field(Name1)}
            Return Me
        End Function

        <Method(inline)>
        Public Function field(Of T4, T3, T2, T1)(Name4 As String, Name3 As String, Name2 As String, Name1 As String) As ils(Of T)
            fields = {Info.type(Of T4).field(Name4), Info.type(Of T3).field(Name3), Info.type(Of T2).field(Name2), Info.type(Of T1).field(Name1)}
            Return Me
        End Function

        <Method(inline)>
        Public Function field(Of T5, T4, T3, T2, T1)(Name5 As String, Name4 As String, Name3 As String, Name2 As String, Name1 As String) As ils(Of T)
            fields = {Info.type(Of T5).field(Name5), Info.type(Of T4).field(Name4), Info.type(Of T3).field(Name3), Info.type(Of T2).field(Name2), Info.type(Of T1).field(Name1)}
            Return Me
        End Function

        <Method(inline)>
        Public Function field(Of T6, T5, T4, T3, T2, T1)(Name6 As String, Name5 As String, Name4 As String, Name3 As String, Name2 As String, Name1 As String) As ils(Of T)
            fields = {Info.type(Of T6).field(Name6), Info.type(Of T5).field(Name5), Info.type(Of T4).field(Name4), Info.type(Of T3).field(Name3), Info.type(Of T2).field(Name2), Info.type(Of T1).field(Name1)}
            Return Me
        End Function

        <Method(inline)>
        Public Function field(Of T7, T6, T5, T4, T3, T2, T1)(Name7 As String, Name6 As String, Name5 As String, Name4 As String, Name3 As String, Name2 As String, Name1 As String) As ils(Of T)
            fields = {Info.type(Of T7).field(Name7), Info.type(Of T6).field(Name6), Info.type(Of T5).field(Name5), Info.type(Of T4).field(Name4), Info.type(Of T3).field(Name3), Info.type(Of T2).field(Name2), Info.type(Of T1).field(Name1)}
            Return Me
        End Function

        <Method(inline)>
        Public Function field(Of T8, T7, T6, T5, T4, T3, T2, T1)(Name8 As String, Name7 As String, Name6 As String, Name5 As String, Name4 As String, Name3 As String, Name2 As String, Name1 As String) As ils(Of T)
            fields = {Info.type(Of T8).field(Name8), Info.type(Of T7).field(Name7), Info.type(Of T6).field(Name6), Info.type(Of T5).field(Name5), Info.type(Of T4).field(Name4), Info.type(Of T3).field(Name3), Info.type(Of T2).field(Name2), Info.type(Of T1).field(Name1)}
            Return Me
        End Function
#End Region

#Region ".ctors"
        <Method(inline)>
        Public Function [new](Of T1)() As ils(Of T)
            news = {Info.type(Of T1).ctor}
            Return Me
        End Function
        <Method(inline)>
        Public Function [new](Of T2, T1)() As ils(Of T)
            news = Info.type(Of T2, T1).ctors
            Return Me
        End Function

        <Method(inline)>
        Public Function [new](Of T3, T2, T1)() As ils(Of T)
            news = Info.type(Of T3, T2, T1).ctors
            Return Me
        End Function

        <Method(inline)>
        Public Function [new](Of T4, T3, T2, T1)() As ils(Of T)
            news = Info.type(Of T4, T3, T2, T1).ctors
            Return Me
        End Function

        <Method(inline)>
        Public Function [new](Of T5, T4, T3, T2, T1)() As ils(Of T)
            news = Info.type(Of T5, T4, T3, T2, T1).ctors
            Return Me
        End Function

        <Method(inline)>
        Public Function [new](Of T6, T5, T4, T3, T2, T1)() As ils(Of T)
            news = Info.type(Of T6, T5, T4, T3, T2, T1).ctors
            Return Me
        End Function

        <Method(inline)>
        Public Function [new](Of T7, T6, T5, T4, T3, T2, T1)() As ils(Of T)
            news = Info.type(Of T7, T6, T5, T4, T3, T2, T1).ctors
            Return Me
        End Function

        <Method(inline)>
        Public Function [new](Of T8, T7, T6, T5, T4, T3, T2, T1)() As ils(Of T)
            news = Info.type(Of T8, T7, T6, T5, T4, T3, T2, T1).ctors
            Return Me
        End Function
#End Region

#Region "types"
        <Method(inline)>
        Public Function type(Of T1)() As ils(Of T)
            types = {Info.type(Of T1).type}
            Return Me
        End Function

        <Method(inline)>
        Public Function type(Of T1, T2)() As ils(Of T)
            types = Info.type(Of T1, T2).types
            Return Me
        End Function

        <Method(inline)>
        Public Function type(Of T1, T2, T3)() As ils(Of T)
            types = Info.type(Of T1, T2, T3).types
            Return Me
        End Function

        <Method(inline)>
        Public Function type(Of T1, T2, T3, T4)() As ils(Of T)
            types = Info.type(Of T1, T2, T3, T4).types
            Return Me
        End Function

        <Method(inline)>
        Public Function type(Of T1, T2, T3, T4, T5)() As ils(Of T)
            types = Info.type(Of T1, T2, T3, T4, T5).types
            Return Me
        End Function

        <Method(inline)>
        Public Function type(Of T1, T2, T3, T4, T5, T6)() As ils(Of T)
            types = Info.type(Of T1, T2, T3, T4, T5, T6).types
            Return Me
        End Function

        <Method(inline)>
        Public Function type(Of T1, T2, T3, T4, T5, T6, T7)() As ils(Of T)
            types = Info.type(Of T1, T2, T3, T4, T5, T6, T7).types
            Return Me
        End Function

        <Method(inline)>
        Public Function type(Of T1, T2, T3, T4, T5, T6, T7, T8)() As ils(Of T)
            types = Info.type(Of T1, T2, T3, T4, T5, T6, T7, T8).types
            Return Me
        End Function

        <Method(inline)>
        Public Function type(Of T1, T2, T3, T4, T5, T6, T7, T8, T9)() As ils(Of T)
            types = Info.type(Of T1, T2, T3, T4, T5, T6, T7, T8, T9).types
            Return Me
        End Function

        <Method(inline)>
        Public Function type(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)() As ils(Of T)
            types = Info.type(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10).types
            Return Me
        End Function

        <Method(inline)>
        Public Function type(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)() As ils(Of T)
            types = Info.type(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11).types
            Return Me
        End Function

        <Method(inline)>
        Public Function type(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)() As ils(Of T)
            types = Info.type(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12).types
            Return Me
        End Function

        <Method(inline)>
        Public Function type(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13)() As ils(Of T)
            types = Info.type(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13).types
            Return Me
        End Function

        <Method(inline)>
        Public Function type(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14)() As ils(Of T)
            types = Info.type(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14).types
            Return Me
        End Function

        <Method(inline)>
        Public Function type(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15)() As ils(Of T)
            types = Info.type(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15).types
            Return Me
        End Function

        <Method(inline)>
        Public Function type(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16)() As ils(Of T)
            types = Info.type(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16).types
            Return Me
        End Function

#End Region

#Region "delegates"
        <Method(inline)>
        Public Function used(Of T1 As Class)() As ils(Of T)
            'useds = {Info.delegate(Of T1).invoker, Info.delegate(Of T2).invoker}
            With New List(Of sr.MethodInfo)(useds)
                .Add(Info.delegate(Of T1).invoker)
                useds = .ToArray
            End With
            Return Me
        End Function
        <Method(inline)>
        Public Function used(Of T1 As Class, T2 As Class)() As ils(Of T)
            'useds = {Info.delegate(Of T1).invoker, Info.delegate(Of T2).invoker}
            With New List(Of sr.MethodInfo)(useds)
                .AddRange({Info.delegate(Of T1).invoker, Info.delegate(Of T2).invoker})
                useds = .ToArray
            End With
            Return Me
        End Function

        <Method(inline)>
        Public Function used(Of T3 As Class, T2 As Class, T1 As Class)() As ils(Of T)
            'useds = {Info.delegate(Of T3).invoker, Info.delegate(Of T2).invoker, Info.delegate(Of T1).invoker}
            With New List(Of sr.MethodInfo)(useds)
                .AddRange({Info.delegate(Of T3).invoker, Info.delegate(Of T2).invoker, Info.delegate(Of T1).invoker})
                useds = .ToArray
            End With
            Return Me
        End Function

        <Method(inline)>
        Public Function used(Of T4 As Class, T3 As Class, T2 As Class, T1 As Class)() As ils(Of T)
            'useds = {Info.delegate(Of T4).invoker, Info.delegate(Of T3).invoker, Info.delegate(Of T2).invoker, Info.delegate(Of T1).invoker}
            With New List(Of sr.MethodInfo)(useds)
                .AddRange({Info.delegate(Of T4).invoker, Info.delegate(Of T3).invoker, Info.delegate(Of T2).invoker, Info.delegate(Of T1).invoker})
                useds = .ToArray
            End With
            Return Me
        End Function

        <Method(inline)>
        Public Function used(Of T5 As Class, T4 As Class, T3 As Class, T2 As Class, T1 As Class)() As ils(Of T)
            'useds = {Info.delegate(Of T5).invoker, Info.delegate(Of T4).invoker, Info.delegate(Of T3).invoker, Info.delegate(Of T2).invoker, Info.delegate(Of T1).invoker}
            With New List(Of sr.MethodInfo)(useds)
                .AddRange({Info.delegate(Of T5).invoker, Info.delegate(Of T4).invoker, Info.delegate(Of T3).invoker, Info.delegate(Of T2).invoker, Info.delegate(Of T1).invoker})
                useds = .ToArray
            End With
            Return Me
        End Function

        <Method(inline)>
        Public Function used(Of T6 As Class, T5 As Class, T4 As Class, T3 As Class, T2 As Class, T1 As Class)() As ils(Of T)
            'useds = {Info.delegate(Of T6).invoker, Info.delegate(Of T5).invoker, Info.delegate(Of T4).invoker, Info.delegate(Of T3).invoker, Info.delegate(Of T2).invoker, Info.delegate(Of T1).invoker}
            With New List(Of sr.MethodInfo)(useds)
                .AddRange({Info.delegate(Of T6).invoker, Info.delegate(Of T5).invoker, Info.delegate(Of T4).invoker, Info.delegate(Of T3).invoker, Info.delegate(Of T2).invoker, Info.delegate(Of T1).invoker})
                useds = .ToArray
            End With
            Return Me
        End Function

        <Method(inline)>
        Public Function used(Of T7 As Class, T6 As Class, T5 As Class, T4 As Class, T3 As Class, T2 As Class, T1 As Class)() As ils(Of T)
            'useds = {Info.delegate(Of T7).invoker, Info.delegate(Of T6).invoker, Info.delegate(Of T5).invoker, Info.delegate(Of T4).invoker, Info.delegate(Of T3).invoker, Info.delegate(Of T2).invoker, Info.delegate(Of T1).invoker}
            With New List(Of sr.MethodInfo)(useds)
                .AddRange({Info.delegate(Of T7).invoker, Info.delegate(Of T6).invoker, Info.delegate(Of T5).invoker, Info.delegate(Of T4).invoker, Info.delegate(Of T3).invoker, Info.delegate(Of T2).invoker, Info.delegate(Of T1).invoker})
                useds = .ToArray
            End With
            Return Me
        End Function

        <Method(inline)>
        Public Function used(Of T8 As Class, T7 As Class, T6 As Class, T5 As Class, T4 As Class, T3 As Class, T2 As Class, T1 As Class)() As ils(Of T)
            'useds = {Info.delegate(Of T8).invoker, Info.delegate(Of T7).invoker, Info.delegate(Of T6).invoker, Info.delegate(Of T5).invoker, Info.delegate(Of T4).invoker, Info.delegate(Of T3).invoker, Info.delegate(Of T2).invoker, Info.delegate(Of T1).invoker}
            With New List(Of sr.MethodInfo)(useds)
                .AddRange({Info.delegate(Of T8).invoker, Info.delegate(Of T7).invoker, Info.delegate(Of T6).invoker, Info.delegate(Of T5).invoker, Info.delegate(Of T4).invoker, Info.delegate(Of T3).invoker, Info.delegate(Of T2).invoker, Info.delegate(Of T1).invoker})
                useds = .ToArray
            End With
            Return Me
        End Function
#End Region

#Region "define"
        <Method(inline)>
        Public Function def(Of T1)() As ils(Of T)
            il.DeclareLocal(GetType(T1))
            Return Me
        End Function

        <Method(inline)>
        Public Function def(Of T2, T1)() As ils(Of T)
            il.DeclareLocal(GetType(T2))
            il.DeclareLocal(GetType(T1))
            Return Me
        End Function

        <Method(inline)>
        Public Function def(Of T3, T2, T1)() As ils(Of T)
            il.DeclareLocal(GetType(T3))
            il.DeclareLocal(GetType(T2))
            il.DeclareLocal(GetType(T1))
            Return Me
        End Function

        <Method(inline)>
        Public Function def(Of T4, T3, T2, T1)() As ils(Of T)
            il.DeclareLocal(GetType(T4))
            il.DeclareLocal(GetType(T3))
            il.DeclareLocal(GetType(T2))
            il.DeclareLocal(GetType(T1))
            Return Me
        End Function

        <Method(inline)>
        Public Function def(Of T5, T4, T3, T2, T1)() As ils(Of T)
            il.DeclareLocal(GetType(T5))
            il.DeclareLocal(GetType(T4))
            il.DeclareLocal(GetType(T3))
            il.DeclareLocal(GetType(T2))
            il.DeclareLocal(GetType(T1))
            Return Me
        End Function

        <Method(inline)>
        Public Function def(Of T6, T5, T4, T3, T2, T1)() As ils(Of T)
            il.DeclareLocal(GetType(T6))
            il.DeclareLocal(GetType(T5))
            il.DeclareLocal(GetType(T4))
            il.DeclareLocal(GetType(T3))
            il.DeclareLocal(GetType(T2))
            il.DeclareLocal(GetType(T1))
            Return Me
        End Function

        <Method(inline)>
        Public Function def(Of T7, T6, T5, T4, T3, T2, T1)() As ils(Of T)
            il.DeclareLocal(GetType(T7))
            il.DeclareLocal(GetType(T6))
            il.DeclareLocal(GetType(T5))
            il.DeclareLocal(GetType(T4))
            il.DeclareLocal(GetType(T3))
            il.DeclareLocal(GetType(T2))
            il.DeclareLocal(GetType(T1))
            Return Me
        End Function

        <Method(inline)>
        Public Function def(Of T8, T7, T6, T5, T4, T3, T2, T1)() As ils(Of T)
            il.DeclareLocal(GetType(T8))
            il.DeclareLocal(GetType(T7))
            il.DeclareLocal(GetType(T6))
            il.DeclareLocal(GetType(T5))
            il.DeclareLocal(GetType(T4))
            il.DeclareLocal(GetType(T3))
            il.DeclareLocal(GetType(T2))
            il.DeclareLocal(GetType(T1))
            Return Me
        End Function
#End Region

#Region "define unsafe"
        <Method(inline)>
        Public Function defu(Of T1 As Class)() As ils(Of T)
            il.DeclareLocal(GetType(T1), True)
            Return Me
        End Function

        <Method(inline)>
        Public Function defu(Of T2, T1)() As ils(Of T)
            il.DeclareLocal(GetType(T2), True)
            il.DeclareLocal(GetType(T1), True)
            Return Me
        End Function

        <Method(inline)>
        Public Function defu(Of T3, T2, T1)() As ils(Of T)
            il.DeclareLocal(GetType(T3), True)
            il.DeclareLocal(GetType(T2), True)
            il.DeclareLocal(GetType(T1), True)
            Return Me
        End Function

        <Method(inline)>
        Public Function defu(Of T4, T3, T2, T1)() As ils(Of T)
            il.DeclareLocal(GetType(T4), True)
            il.DeclareLocal(GetType(T3), True)
            il.DeclareLocal(GetType(T2), True)
            il.DeclareLocal(GetType(T1), True)
            Return Me
        End Function

        <Method(inline)>
        Public Function defu(Of T5, T4, T3, T2, T1)() As ils(Of T)
            il.DeclareLocal(GetType(T5), True)
            il.DeclareLocal(GetType(T4), True)
            il.DeclareLocal(GetType(T3), True)
            il.DeclareLocal(GetType(T2), True)
            il.DeclareLocal(GetType(T1), True)
            Return Me
        End Function

        <Method(inline)>
        Public Function defu(Of T6, T5, T4, T3, T2, T1)() As ils(Of T)
            il.DeclareLocal(GetType(T6), True)
            il.DeclareLocal(GetType(T5), True)
            il.DeclareLocal(GetType(T4), True)
            il.DeclareLocal(GetType(T3), True)
            il.DeclareLocal(GetType(T2), True)
            il.DeclareLocal(GetType(T1), True)
            Return Me
        End Function

        <Method(inline)>
        Public Function defu(Of T7, T6, T5, T4, T3, T2, T1)() As ils(Of T)
            il.DeclareLocal(GetType(T7), True)
            il.DeclareLocal(GetType(T6), True)
            il.DeclareLocal(GetType(T5), True)
            il.DeclareLocal(GetType(T4), True)
            il.DeclareLocal(GetType(T3), True)
            il.DeclareLocal(GetType(T2), True)
            il.DeclareLocal(GetType(T1), True)
            Return Me
        End Function

        <Method(inline)>
        Public Function defu(Of T8, T7, T6, T5, T4, T3, T2, T1)() As ils(Of T)
            il.DeclareLocal(GetType(T8), True)
            il.DeclareLocal(GetType(T7), True)
            il.DeclareLocal(GetType(T6), True)
            il.DeclareLocal(GetType(T5), True)
            il.DeclareLocal(GetType(T4), True)
            il.DeclareLocal(GetType(T3), True)
            il.DeclareLocal(GetType(T2), True)
            il.DeclareLocal(GetType(T1), True)
            Return Me
        End Function
#End Region
    End Structure
End Namespace