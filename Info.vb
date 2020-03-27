Namespace ILS
    Partial Friend Module Generate
        Friend Sub gen_get_type()
            Dim Listing = Function(SB As Text.StringBuilder, N As Integer, Body As Func(Of Integer, String))
                              Dim I As Int32
                              For I = 1 To N - 1
                                  SB.Append($"{Body(I)}, ")
                              Next
                              Return SB.Append(Body(I))
                          End Function

            With New Text.StringBuilder

                For i = 2 To 32

                    'T1, T2, ... TN
                    Dim T0 = Listing(.Clear, i, Function(N) $"T{N}").ToString

                    Dim T1 = Listing(.Clear, i, Function(N) $"GetType(T{N})").ToString

                    Dim T2 = Listing(.Clear, i, Function(N) $"type(Of T{N}).ctor").ToString

                    Diagnostics.Debug.WriteLine(.Clear.Append($"
    Public Class type(Of {T0})
        Public Shared ReadOnly types() As System.Type = {{{T1}}}
        Public Shared ReadOnly ctors() As System.Reflection.ConstructorInfo = {{{T2}}}
    End Class").
                        ToString)
                Next
            End With
        End Sub
    End Module
End Namespace

Namespace ILS.Info
    Public Structure [delegate](Of T As Class)
        Public Shared ReadOnly type As System.Type = GetType(T)
        Public Shared ReadOnly invoker As sr.MethodInfo = type.GetMethod("Invoke")
        Public Shared ReadOnly return_type As System.Type = invoker.ReturnType
        Public Shared ReadOnly parameters() As sr.ParameterInfo = invoker.GetParameters
        Public Shared ReadOnly param_types() As System.Type = Function()
                                                                  With New List(Of Type)
                                                                      For Each item In parameters
                                                                          .Add(item.ParameterType)
                                                                      Next
                                                                      Return .ToArray
                                                                  End With
                                                              End Function()
        Public Shared ReadOnly invoke_param() As sr.MethodInfo = Function()
                                                                     With New List(Of sr.MethodInfo)
                                                                         For Each item In param_types
                                                                             If item.IsSubclassOf(Info.type(Of [Delegate]).type) Then
                                                                                 .Add(item.GetMethod("Invoke"))
                                                                             End If
                                                                         Next
                                                                         Return .ToArray
                                                                     End With
                                                                 End Function()

        <Method(inline)>
        Public Shared Function create_method() As sre.DynamicMethod
            Return New sre.DynamicMethod("", return_type, param_types, app)
        End Function
        <Method(inline)>
        Public Shared Function create_method(Name As String) As sre.DynamicMethod
            Return New sre.DynamicMethod(Name, return_type, param_types, app)
        End Function
        <Method(inline)>
        Public Shared Function create_method(Assembly_module As sr.Module) As sre.DynamicMethod
            Return New sre.DynamicMethod("", return_type, param_types, Assembly_module)
        End Function
        <Method(inline)>
        Public Shared Function create_method(Name As String, Assembly_module As sr.Module) As sre.DynamicMethod
            Return New sre.DynamicMethod(Name, return_type, param_types, Assembly_module)
        End Function
        <Method(inline)>
        Public Shared Function create_method(Base_class As System.Type) As sre.DynamicMethod
            Return New sre.DynamicMethod("", return_type, param_types, Base_class)
        End Function
        <Method(inline)>
        Public Shared Function create_method(Name As String, Base_class As System.Type) As sre.DynamicMethod
            Return New sre.DynamicMethod(Name, return_type, param_types, Base_class)
        End Function
    End Structure

    Public Class type(Of Base)
        Public Shared ReadOnly type As System.Type = GetType(Base)
        Public Shared ReadOnly pointer As System.Type = type.MakePointerType
        Public Shared ReadOnly by_ref As System.Type = type.MakeByRefType
        Public Shared ReadOnly ctors() As System.Reflection.ConstructorInfo = type.GetConstructors(sr.BindingFlags.Public Or
                                                                                                  sr.BindingFlags.NonPublic Or
                                                                                                  sr.BindingFlags.Instance)
        Public Shared ReadOnly ctor As System.Reflection.ConstructorInfo = If(ctors.Length > 0, ctors(0), Nothing)

        <Method(inline)>
        Public Shared Function field(Name As String) As sr.FieldInfo
            Return type.GetField(Name, sr.BindingFlags.Public Or
                                       sr.BindingFlags.NonPublic Or
                                       sr.BindingFlags.Static Or
                                       sr.BindingFlags.Instance)
        End Function

        '''<summary>Get Invoke() method of the type if it exited.</summary>
        <Method(inline)>
        Public Shared Function invoke() As System.Reflection.MethodInfo
            Return type.GetMethod("Invoke")
        End Function

#Region "Get Method"
        <Method(inline)>
        Public Shared Function method(Name As String) As System.Reflection.MethodInfo
            Return type.GetMethod(Name)
        End Function
        <Method(inline)>
        Public Shared Function method(Of T1)(Name As String) As System.Reflection.MethodInfo
            Return type.GetMethod(Name, {Info.type(Of T1).type})
        End Function
        <Method(inline)>
        Public Shared Function method(Of T1, T2)(Name As String) As System.Reflection.MethodInfo
            Return type.GetMethod(Name, Info.type(Of T1, T2).types)
        End Function
        <Method(inline)>
        Public Shared Function method(Of T1, T2, T3)(Name As String) As System.Reflection.MethodInfo
            Return type.GetMethod(Name, Info.type(Of T1, T2, T3).types)
        End Function
        <Method(inline)>
        Public Shared Function method(Of T1, T2, T3, T4)(Name As String) As System.Reflection.MethodInfo
            Return type.GetMethod(Name, Info.type(Of T1, T2, T3, T4).types)
        End Function
        <Method(inline)>
        Public Shared Function method(Of T1, T2, T3, T4, T5)(Name As String) As System.Reflection.MethodInfo
            Return type.GetMethod(Name, Info.type(Of T1, T2, T3, T4, T5).types)
        End Function
        <Method(inline)>
        Public Shared Function method(Of T1, T2, T3, T4, T5, T6)(Name As String) As System.Reflection.MethodInfo
            Return type.GetMethod(Name, Info.type(Of T1, T2, T3, T4, T5, T6).types)
        End Function
        <Method(inline)>
        Public Shared Function method(Of T1, T2, T3, T4, T5, T6, T7)(Name As String) As System.Reflection.MethodInfo
            Return type.GetMethod(Name, Info.type(Of T1, T2, T3, T4, T5, T6, T7).types)
        End Function
        <Method(inline)>
        Public Shared Function method(Of T1, T2, T3, T4, T5, T6, T7, T8)(Name As String) As System.Reflection.MethodInfo
            Return type.GetMethod(Name, Info.type(Of T1, T2, T3, T4, T5, T6, T7, T8).types)
        End Function
#End Region

#Region "Get Constructor"
        <Method(inline)>
        Public Shared Function [new](ParamArray Types() As System.Type) As System.Reflection.ConstructorInfo
            Return type.GetConstructor(Types)
        End Function
        <Method(inline)>
        Public Shared Function [new]() As System.Reflection.ConstructorInfo
            [new] = type.GetConstructor({})
            If [new] Is Nothing Then [new] = type.TypeInitializer
        End Function
        <Method(inline)>
        Public Shared Function [new](Of T1)() As System.Reflection.ConstructorInfo
            Return type.GetConstructor({Info.type(Of T1).type})
        End Function
        <Method(inline)>
        Public Shared Function [new](Of T1, T2)() As System.Reflection.ConstructorInfo
            Return type.GetConstructor(Info.type(Of T1, T2).types)
        End Function
        <Method(inline)>
        Public Shared Function [new](Of T1, T2, T3)() As System.Reflection.ConstructorInfo
            Return type.GetConstructor(Info.type(Of T1, T2, T3).types)
        End Function
        <Method(inline)>
        Public Shared Function [new](Of T1, T2, T3, T4)() As System.Reflection.ConstructorInfo
            Return type.GetConstructor(Info.type(Of T1, T2, T3, T4).types)
        End Function
        <Method(inline)>
        Public Shared Function [new](Of T1, T2, T3, T4, T5)() As System.Reflection.ConstructorInfo
            Return type.GetConstructor(Info.type(Of T1, T2, T3, T4, T5).types)
        End Function
        <Method(inline)>
        Public Shared Function [new](Of T1, T2, T3, T4, T5, T6)() As System.Reflection.ConstructorInfo
            Return type.GetConstructor(Info.type(Of T1, T2, T3, T4, T5, T6).types)
        End Function
        <Method(inline)>
        Public Shared Function [new](Of T1, T2, T3, T4, T5, T6, T7)() As System.Reflection.ConstructorInfo
            Return type.GetConstructor(Info.type(Of T1, T2, T3, T4, T5, T6, T7).types)
        End Function
        <Method(inline)>
        Public Shared Function [new](Of T1, T2, T3, T4, T5, T6, T7, T8)() As System.Reflection.ConstructorInfo
            Return type.GetConstructor(Info.type(Of T1, T2, T3, T4, T5, T6, T7, T8).types)
        End Function
#End Region
    End Class

#Region "Get types"

    Public Class type(Of T1, T2)
        Public Shared ReadOnly types() As System.Type = {GetType(T1), GetType(T2)}
        Public Shared ReadOnly ctors() As System.Reflection.ConstructorInfo = {type(Of T1).ctor, type(Of T2).ctor}
    End Class

    Public Class type(Of T1, T2, T3)
        Public Shared ReadOnly types() As System.Type = {GetType(T1), GetType(T2), GetType(T3)}
        Public Shared ReadOnly ctors() As System.Reflection.ConstructorInfo = {type(Of T1).ctor, type(Of T2).ctor, type(Of T3).ctor}
    End Class

    Public Class type(Of T1, T2, T3, T4)
        Public Shared ReadOnly types() As System.Type = {GetType(T1), GetType(T2), GetType(T3), GetType(T4)}
        Public Shared ReadOnly ctors() As System.Reflection.ConstructorInfo = {type(Of T1).ctor, type(Of T2).ctor, type(Of T3).ctor, type(Of T4).ctor}
    End Class

    Public Class type(Of T1, T2, T3, T4, T5)
        Public Shared ReadOnly types() As System.Type = {GetType(T1), GetType(T2), GetType(T3), GetType(T4), GetType(T5)}
        Public Shared ReadOnly ctors() As System.Reflection.ConstructorInfo = {type(Of T1).ctor, type(Of T2).ctor, type(Of T3).ctor, type(Of T4).ctor, type(Of T5).ctor}
    End Class

    Public Class type(Of T1, T2, T3, T4, T5, T6)
        Public Shared ReadOnly types() As System.Type = {GetType(T1), GetType(T2), GetType(T3), GetType(T4), GetType(T5), GetType(T6)}
        Public Shared ReadOnly ctors() As System.Reflection.ConstructorInfo = {type(Of T1).ctor, type(Of T2).ctor, type(Of T3).ctor, type(Of T4).ctor, type(Of T5).ctor, type(Of T6).ctor}
    End Class

    Public Class type(Of T1, T2, T3, T4, T5, T6, T7)
        Public Shared ReadOnly types() As System.Type = {GetType(T1), GetType(T2), GetType(T3), GetType(T4), GetType(T5), GetType(T6), GetType(T7)}
        Public Shared ReadOnly ctors() As System.Reflection.ConstructorInfo = {type(Of T1).ctor, type(Of T2).ctor, type(Of T3).ctor, type(Of T4).ctor, type(Of T5).ctor, type(Of T6).ctor, type(Of T7).ctor}
    End Class

    Public Class type(Of T1, T2, T3, T4, T5, T6, T7, T8)
        Public Shared ReadOnly types() As System.Type = {GetType(T1), GetType(T2), GetType(T3), GetType(T4), GetType(T5), GetType(T6), GetType(T7), GetType(T8)}
        Public Shared ReadOnly ctors() As System.Reflection.ConstructorInfo = {type(Of T1).ctor, type(Of T2).ctor, type(Of T3).ctor, type(Of T4).ctor, type(Of T5).ctor, type(Of T6).ctor, type(Of T7).ctor, type(Of T8).ctor}
    End Class

    Public Class type(Of T1, T2, T3, T4, T5, T6, T7, T8, T9)
        Public Shared ReadOnly types() As System.Type = {GetType(T1), GetType(T2), GetType(T3), GetType(T4), GetType(T5), GetType(T6), GetType(T7), GetType(T8), GetType(T9)}
        Public Shared ReadOnly ctors() As System.Reflection.ConstructorInfo = {type(Of T1).ctor, type(Of T2).ctor, type(Of T3).ctor, type(Of T4).ctor, type(Of T5).ctor, type(Of T6).ctor, type(Of T7).ctor, type(Of T8).ctor, type(Of T9).ctor}
    End Class

    Public Class type(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)
        Public Shared ReadOnly types() As System.Type = {GetType(T1), GetType(T2), GetType(T3), GetType(T4), GetType(T5), GetType(T6), GetType(T7), GetType(T8), GetType(T9), GetType(T10)}
        Public Shared ReadOnly ctors() As System.Reflection.ConstructorInfo = {type(Of T1).ctor, type(Of T2).ctor, type(Of T3).ctor, type(Of T4).ctor, type(Of T5).ctor, type(Of T6).ctor, type(Of T7).ctor, type(Of T8).ctor, type(Of T9).ctor, type(Of T10).ctor}
    End Class

    Public Class type(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)
        Public Shared ReadOnly types() As System.Type = {GetType(T1), GetType(T2), GetType(T3), GetType(T4), GetType(T5), GetType(T6), GetType(T7), GetType(T8), GetType(T9), GetType(T10), GetType(T11)}
        Public Shared ReadOnly ctors() As System.Reflection.ConstructorInfo = {type(Of T1).ctor, type(Of T2).ctor, type(Of T3).ctor, type(Of T4).ctor, type(Of T5).ctor, type(Of T6).ctor, type(Of T7).ctor, type(Of T8).ctor, type(Of T9).ctor, type(Of T10).ctor, type(Of T11).ctor}
    End Class

    Public Class type(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)
        Public Shared ReadOnly types() As System.Type = {GetType(T1), GetType(T2), GetType(T3), GetType(T4), GetType(T5), GetType(T6), GetType(T7), GetType(T8), GetType(T9), GetType(T10), GetType(T11), GetType(T12)}
        Public Shared ReadOnly ctors() As System.Reflection.ConstructorInfo = {type(Of T1).ctor, type(Of T2).ctor, type(Of T3).ctor, type(Of T4).ctor, type(Of T5).ctor, type(Of T6).ctor, type(Of T7).ctor, type(Of T8).ctor, type(Of T9).ctor, type(Of T10).ctor, type(Of T11).ctor, type(Of T12).ctor}
    End Class

    Public Class type(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13)
        Public Shared ReadOnly types() As System.Type = {GetType(T1), GetType(T2), GetType(T3), GetType(T4), GetType(T5), GetType(T6), GetType(T7), GetType(T8), GetType(T9), GetType(T10), GetType(T11), GetType(T12), GetType(T13)}
        Public Shared ReadOnly ctors() As System.Reflection.ConstructorInfo = {type(Of T1).ctor, type(Of T2).ctor, type(Of T3).ctor, type(Of T4).ctor, type(Of T5).ctor, type(Of T6).ctor, type(Of T7).ctor, type(Of T8).ctor, type(Of T9).ctor, type(Of T10).ctor, type(Of T11).ctor, type(Of T12).ctor, type(Of T13).ctor}
    End Class

    Public Class type(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14)
        Public Shared ReadOnly types() As System.Type = {GetType(T1), GetType(T2), GetType(T3), GetType(T4), GetType(T5), GetType(T6), GetType(T7), GetType(T8), GetType(T9), GetType(T10), GetType(T11), GetType(T12), GetType(T13), GetType(T14)}
        Public Shared ReadOnly ctors() As System.Reflection.ConstructorInfo = {type(Of T1).ctor, type(Of T2).ctor, type(Of T3).ctor, type(Of T4).ctor, type(Of T5).ctor, type(Of T6).ctor, type(Of T7).ctor, type(Of T8).ctor, type(Of T9).ctor, type(Of T10).ctor, type(Of T11).ctor, type(Of T12).ctor, type(Of T13).ctor, type(Of T14).ctor}
    End Class

    Public Class type(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15)
        Public Shared ReadOnly types() As System.Type = {GetType(T1), GetType(T2), GetType(T3), GetType(T4), GetType(T5), GetType(T6), GetType(T7), GetType(T8), GetType(T9), GetType(T10), GetType(T11), GetType(T12), GetType(T13), GetType(T14), GetType(T15)}
        Public Shared ReadOnly ctors() As System.Reflection.ConstructorInfo = {type(Of T1).ctor, type(Of T2).ctor, type(Of T3).ctor, type(Of T4).ctor, type(Of T5).ctor, type(Of T6).ctor, type(Of T7).ctor, type(Of T8).ctor, type(Of T9).ctor, type(Of T10).ctor, type(Of T11).ctor, type(Of T12).ctor, type(Of T13).ctor, type(Of T14).ctor, type(Of T15).ctor}
    End Class

    Public Class type(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16)
        Public Shared ReadOnly types() As System.Type = {GetType(T1), GetType(T2), GetType(T3), GetType(T4), GetType(T5), GetType(T6), GetType(T7), GetType(T8), GetType(T9), GetType(T10), GetType(T11), GetType(T12), GetType(T13), GetType(T14), GetType(T15), GetType(T16)}
        Public Shared ReadOnly ctors() As System.Reflection.ConstructorInfo = {type(Of T1).ctor, type(Of T2).ctor, type(Of T3).ctor, type(Of T4).ctor, type(Of T5).ctor, type(Of T6).ctor, type(Of T7).ctor, type(Of T8).ctor, type(Of T9).ctor, type(Of T10).ctor, type(Of T11).ctor, type(Of T12).ctor, type(Of T13).ctor, type(Of T14).ctor, type(Of T15).ctor, type(Of T16).ctor}
    End Class

    Public Class type(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17)
        Public Shared ReadOnly types() As System.Type = {GetType(T1), GetType(T2), GetType(T3), GetType(T4), GetType(T5), GetType(T6), GetType(T7), GetType(T8), GetType(T9), GetType(T10), GetType(T11), GetType(T12), GetType(T13), GetType(T14), GetType(T15), GetType(T16), GetType(T17)}
        Public Shared ReadOnly ctors() As System.Reflection.ConstructorInfo = {type(Of T1).ctor, type(Of T2).ctor, type(Of T3).ctor, type(Of T4).ctor, type(Of T5).ctor, type(Of T6).ctor, type(Of T7).ctor, type(Of T8).ctor, type(Of T9).ctor, type(Of T10).ctor, type(Of T11).ctor, type(Of T12).ctor, type(Of T13).ctor, type(Of T14).ctor, type(Of T15).ctor, type(Of T16).ctor, type(Of T17).ctor}
    End Class

    Public Class type(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18)
        Public Shared ReadOnly types() As System.Type = {GetType(T1), GetType(T2), GetType(T3), GetType(T4), GetType(T5), GetType(T6), GetType(T7), GetType(T8), GetType(T9), GetType(T10), GetType(T11), GetType(T12), GetType(T13), GetType(T14), GetType(T15), GetType(T16), GetType(T17), GetType(T18)}
        Public Shared ReadOnly ctors() As System.Reflection.ConstructorInfo = {type(Of T1).ctor, type(Of T2).ctor, type(Of T3).ctor, type(Of T4).ctor, type(Of T5).ctor, type(Of T6).ctor, type(Of T7).ctor, type(Of T8).ctor, type(Of T9).ctor, type(Of T10).ctor, type(Of T11).ctor, type(Of T12).ctor, type(Of T13).ctor, type(Of T14).ctor, type(Of T15).ctor, type(Of T16).ctor, type(Of T17).ctor, type(Of T18).ctor}
    End Class

    Public Class type(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19)
        Public Shared ReadOnly types() As System.Type = {GetType(T1), GetType(T2), GetType(T3), GetType(T4), GetType(T5), GetType(T6), GetType(T7), GetType(T8), GetType(T9), GetType(T10), GetType(T11), GetType(T12), GetType(T13), GetType(T14), GetType(T15), GetType(T16), GetType(T17), GetType(T18), GetType(T19)}
        Public Shared ReadOnly ctors() As System.Reflection.ConstructorInfo = {type(Of T1).ctor, type(Of T2).ctor, type(Of T3).ctor, type(Of T4).ctor, type(Of T5).ctor, type(Of T6).ctor, type(Of T7).ctor, type(Of T8).ctor, type(Of T9).ctor, type(Of T10).ctor, type(Of T11).ctor, type(Of T12).ctor, type(Of T13).ctor, type(Of T14).ctor, type(Of T15).ctor, type(Of T16).ctor, type(Of T17).ctor, type(Of T18).ctor, type(Of T19).ctor}
    End Class

    Public Class type(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20)
        Public Shared ReadOnly types() As System.Type = {GetType(T1), GetType(T2), GetType(T3), GetType(T4), GetType(T5), GetType(T6), GetType(T7), GetType(T8), GetType(T9), GetType(T10), GetType(T11), GetType(T12), GetType(T13), GetType(T14), GetType(T15), GetType(T16), GetType(T17), GetType(T18), GetType(T19), GetType(T20)}
        Public Shared ReadOnly ctors() As System.Reflection.ConstructorInfo = {type(Of T1).ctor, type(Of T2).ctor, type(Of T3).ctor, type(Of T4).ctor, type(Of T5).ctor, type(Of T6).ctor, type(Of T7).ctor, type(Of T8).ctor, type(Of T9).ctor, type(Of T10).ctor, type(Of T11).ctor, type(Of T12).ctor, type(Of T13).ctor, type(Of T14).ctor, type(Of T15).ctor, type(Of T16).ctor, type(Of T17).ctor, type(Of T18).ctor, type(Of T19).ctor, type(Of T20).ctor}
    End Class

    Public Class type(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21)
        Public Shared ReadOnly types() As System.Type = {GetType(T1), GetType(T2), GetType(T3), GetType(T4), GetType(T5), GetType(T6), GetType(T7), GetType(T8), GetType(T9), GetType(T10), GetType(T11), GetType(T12), GetType(T13), GetType(T14), GetType(T15), GetType(T16), GetType(T17), GetType(T18), GetType(T19), GetType(T20), GetType(T21)}
        Public Shared ReadOnly ctors() As System.Reflection.ConstructorInfo = {type(Of T1).ctor, type(Of T2).ctor, type(Of T3).ctor, type(Of T4).ctor, type(Of T5).ctor, type(Of T6).ctor, type(Of T7).ctor, type(Of T8).ctor, type(Of T9).ctor, type(Of T10).ctor, type(Of T11).ctor, type(Of T12).ctor, type(Of T13).ctor, type(Of T14).ctor, type(Of T15).ctor, type(Of T16).ctor, type(Of T17).ctor, type(Of T18).ctor, type(Of T19).ctor, type(Of T20).ctor, type(Of T21).ctor}
    End Class

    Public Class type(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22)
        Public Shared ReadOnly types() As System.Type = {GetType(T1), GetType(T2), GetType(T3), GetType(T4), GetType(T5), GetType(T6), GetType(T7), GetType(T8), GetType(T9), GetType(T10), GetType(T11), GetType(T12), GetType(T13), GetType(T14), GetType(T15), GetType(T16), GetType(T17), GetType(T18), GetType(T19), GetType(T20), GetType(T21), GetType(T22)}
        Public Shared ReadOnly ctors() As System.Reflection.ConstructorInfo = {type(Of T1).ctor, type(Of T2).ctor, type(Of T3).ctor, type(Of T4).ctor, type(Of T5).ctor, type(Of T6).ctor, type(Of T7).ctor, type(Of T8).ctor, type(Of T9).ctor, type(Of T10).ctor, type(Of T11).ctor, type(Of T12).ctor, type(Of T13).ctor, type(Of T14).ctor, type(Of T15).ctor, type(Of T16).ctor, type(Of T17).ctor, type(Of T18).ctor, type(Of T19).ctor, type(Of T20).ctor, type(Of T21).ctor, type(Of T22).ctor}
    End Class

    Public Class type(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23)
        Public Shared ReadOnly types() As System.Type = {GetType(T1), GetType(T2), GetType(T3), GetType(T4), GetType(T5), GetType(T6), GetType(T7), GetType(T8), GetType(T9), GetType(T10), GetType(T11), GetType(T12), GetType(T13), GetType(T14), GetType(T15), GetType(T16), GetType(T17), GetType(T18), GetType(T19), GetType(T20), GetType(T21), GetType(T22), GetType(T23)}
        Public Shared ReadOnly ctors() As System.Reflection.ConstructorInfo = {type(Of T1).ctor, type(Of T2).ctor, type(Of T3).ctor, type(Of T4).ctor, type(Of T5).ctor, type(Of T6).ctor, type(Of T7).ctor, type(Of T8).ctor, type(Of T9).ctor, type(Of T10).ctor, type(Of T11).ctor, type(Of T12).ctor, type(Of T13).ctor, type(Of T14).ctor, type(Of T15).ctor, type(Of T16).ctor, type(Of T17).ctor, type(Of T18).ctor, type(Of T19).ctor, type(Of T20).ctor, type(Of T21).ctor, type(Of T22).ctor, type(Of T23).ctor}
    End Class

    Public Class type(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24)
        Public Shared ReadOnly types() As System.Type = {GetType(T1), GetType(T2), GetType(T3), GetType(T4), GetType(T5), GetType(T6), GetType(T7), GetType(T8), GetType(T9), GetType(T10), GetType(T11), GetType(T12), GetType(T13), GetType(T14), GetType(T15), GetType(T16), GetType(T17), GetType(T18), GetType(T19), GetType(T20), GetType(T21), GetType(T22), GetType(T23), GetType(T24)}
        Public Shared ReadOnly ctors() As System.Reflection.ConstructorInfo = {type(Of T1).ctor, type(Of T2).ctor, type(Of T3).ctor, type(Of T4).ctor, type(Of T5).ctor, type(Of T6).ctor, type(Of T7).ctor, type(Of T8).ctor, type(Of T9).ctor, type(Of T10).ctor, type(Of T11).ctor, type(Of T12).ctor, type(Of T13).ctor, type(Of T14).ctor, type(Of T15).ctor, type(Of T16).ctor, type(Of T17).ctor, type(Of T18).ctor, type(Of T19).ctor, type(Of T20).ctor, type(Of T21).ctor, type(Of T22).ctor, type(Of T23).ctor, type(Of T24).ctor}
    End Class

    Public Class type(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25)
        Public Shared ReadOnly types() As System.Type = {GetType(T1), GetType(T2), GetType(T3), GetType(T4), GetType(T5), GetType(T6), GetType(T7), GetType(T8), GetType(T9), GetType(T10), GetType(T11), GetType(T12), GetType(T13), GetType(T14), GetType(T15), GetType(T16), GetType(T17), GetType(T18), GetType(T19), GetType(T20), GetType(T21), GetType(T22), GetType(T23), GetType(T24), GetType(T25)}
        Public Shared ReadOnly ctors() As System.Reflection.ConstructorInfo = {type(Of T1).ctor, type(Of T2).ctor, type(Of T3).ctor, type(Of T4).ctor, type(Of T5).ctor, type(Of T6).ctor, type(Of T7).ctor, type(Of T8).ctor, type(Of T9).ctor, type(Of T10).ctor, type(Of T11).ctor, type(Of T12).ctor, type(Of T13).ctor, type(Of T14).ctor, type(Of T15).ctor, type(Of T16).ctor, type(Of T17).ctor, type(Of T18).ctor, type(Of T19).ctor, type(Of T20).ctor, type(Of T21).ctor, type(Of T22).ctor, type(Of T23).ctor, type(Of T24).ctor, type(Of T25).ctor}
    End Class

    Public Class type(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26)
        Public Shared ReadOnly types() As System.Type = {GetType(T1), GetType(T2), GetType(T3), GetType(T4), GetType(T5), GetType(T6), GetType(T7), GetType(T8), GetType(T9), GetType(T10), GetType(T11), GetType(T12), GetType(T13), GetType(T14), GetType(T15), GetType(T16), GetType(T17), GetType(T18), GetType(T19), GetType(T20), GetType(T21), GetType(T22), GetType(T23), GetType(T24), GetType(T25), GetType(T26)}
        Public Shared ReadOnly ctors() As System.Reflection.ConstructorInfo = {type(Of T1).ctor, type(Of T2).ctor, type(Of T3).ctor, type(Of T4).ctor, type(Of T5).ctor, type(Of T6).ctor, type(Of T7).ctor, type(Of T8).ctor, type(Of T9).ctor, type(Of T10).ctor, type(Of T11).ctor, type(Of T12).ctor, type(Of T13).ctor, type(Of T14).ctor, type(Of T15).ctor, type(Of T16).ctor, type(Of T17).ctor, type(Of T18).ctor, type(Of T19).ctor, type(Of T20).ctor, type(Of T21).ctor, type(Of T22).ctor, type(Of T23).ctor, type(Of T24).ctor, type(Of T25).ctor, type(Of T26).ctor}
    End Class

    Public Class type(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27)
        Public Shared ReadOnly types() As System.Type = {GetType(T1), GetType(T2), GetType(T3), GetType(T4), GetType(T5), GetType(T6), GetType(T7), GetType(T8), GetType(T9), GetType(T10), GetType(T11), GetType(T12), GetType(T13), GetType(T14), GetType(T15), GetType(T16), GetType(T17), GetType(T18), GetType(T19), GetType(T20), GetType(T21), GetType(T22), GetType(T23), GetType(T24), GetType(T25), GetType(T26), GetType(T27)}
        Public Shared ReadOnly ctors() As System.Reflection.ConstructorInfo = {type(Of T1).ctor, type(Of T2).ctor, type(Of T3).ctor, type(Of T4).ctor, type(Of T5).ctor, type(Of T6).ctor, type(Of T7).ctor, type(Of T8).ctor, type(Of T9).ctor, type(Of T10).ctor, type(Of T11).ctor, type(Of T12).ctor, type(Of T13).ctor, type(Of T14).ctor, type(Of T15).ctor, type(Of T16).ctor, type(Of T17).ctor, type(Of T18).ctor, type(Of T19).ctor, type(Of T20).ctor, type(Of T21).ctor, type(Of T22).ctor, type(Of T23).ctor, type(Of T24).ctor, type(Of T25).ctor, type(Of T26).ctor, type(Of T27).ctor}
    End Class

    Public Class type(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28)
        Public Shared ReadOnly types() As System.Type = {GetType(T1), GetType(T2), GetType(T3), GetType(T4), GetType(T5), GetType(T6), GetType(T7), GetType(T8), GetType(T9), GetType(T10), GetType(T11), GetType(T12), GetType(T13), GetType(T14), GetType(T15), GetType(T16), GetType(T17), GetType(T18), GetType(T19), GetType(T20), GetType(T21), GetType(T22), GetType(T23), GetType(T24), GetType(T25), GetType(T26), GetType(T27), GetType(T28)}
        Public Shared ReadOnly ctors() As System.Reflection.ConstructorInfo = {type(Of T1).ctor, type(Of T2).ctor, type(Of T3).ctor, type(Of T4).ctor, type(Of T5).ctor, type(Of T6).ctor, type(Of T7).ctor, type(Of T8).ctor, type(Of T9).ctor, type(Of T10).ctor, type(Of T11).ctor, type(Of T12).ctor, type(Of T13).ctor, type(Of T14).ctor, type(Of T15).ctor, type(Of T16).ctor, type(Of T17).ctor, type(Of T18).ctor, type(Of T19).ctor, type(Of T20).ctor, type(Of T21).ctor, type(Of T22).ctor, type(Of T23).ctor, type(Of T24).ctor, type(Of T25).ctor, type(Of T26).ctor, type(Of T27).ctor, type(Of T28).ctor}
    End Class

    Public Class type(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29)
        Public Shared ReadOnly types() As System.Type = {GetType(T1), GetType(T2), GetType(T3), GetType(T4), GetType(T5), GetType(T6), GetType(T7), GetType(T8), GetType(T9), GetType(T10), GetType(T11), GetType(T12), GetType(T13), GetType(T14), GetType(T15), GetType(T16), GetType(T17), GetType(T18), GetType(T19), GetType(T20), GetType(T21), GetType(T22), GetType(T23), GetType(T24), GetType(T25), GetType(T26), GetType(T27), GetType(T28), GetType(T29)}
        Public Shared ReadOnly ctors() As System.Reflection.ConstructorInfo = {type(Of T1).ctor, type(Of T2).ctor, type(Of T3).ctor, type(Of T4).ctor, type(Of T5).ctor, type(Of T6).ctor, type(Of T7).ctor, type(Of T8).ctor, type(Of T9).ctor, type(Of T10).ctor, type(Of T11).ctor, type(Of T12).ctor, type(Of T13).ctor, type(Of T14).ctor, type(Of T15).ctor, type(Of T16).ctor, type(Of T17).ctor, type(Of T18).ctor, type(Of T19).ctor, type(Of T20).ctor, type(Of T21).ctor, type(Of T22).ctor, type(Of T23).ctor, type(Of T24).ctor, type(Of T25).ctor, type(Of T26).ctor, type(Of T27).ctor, type(Of T28).ctor, type(Of T29).ctor}
    End Class

    Public Class type(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30)
        Public Shared ReadOnly types() As System.Type = {GetType(T1), GetType(T2), GetType(T3), GetType(T4), GetType(T5), GetType(T6), GetType(T7), GetType(T8), GetType(T9), GetType(T10), GetType(T11), GetType(T12), GetType(T13), GetType(T14), GetType(T15), GetType(T16), GetType(T17), GetType(T18), GetType(T19), GetType(T20), GetType(T21), GetType(T22), GetType(T23), GetType(T24), GetType(T25), GetType(T26), GetType(T27), GetType(T28), GetType(T29), GetType(T30)}
        Public Shared ReadOnly ctors() As System.Reflection.ConstructorInfo = {type(Of T1).ctor, type(Of T2).ctor, type(Of T3).ctor, type(Of T4).ctor, type(Of T5).ctor, type(Of T6).ctor, type(Of T7).ctor, type(Of T8).ctor, type(Of T9).ctor, type(Of T10).ctor, type(Of T11).ctor, type(Of T12).ctor, type(Of T13).ctor, type(Of T14).ctor, type(Of T15).ctor, type(Of T16).ctor, type(Of T17).ctor, type(Of T18).ctor, type(Of T19).ctor, type(Of T20).ctor, type(Of T21).ctor, type(Of T22).ctor, type(Of T23).ctor, type(Of T24).ctor, type(Of T25).ctor, type(Of T26).ctor, type(Of T27).ctor, type(Of T28).ctor, type(Of T29).ctor, type(Of T30).ctor}
    End Class

    Public Class type(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31)
        Public Shared ReadOnly types() As System.Type = {GetType(T1), GetType(T2), GetType(T3), GetType(T4), GetType(T5), GetType(T6), GetType(T7), GetType(T8), GetType(T9), GetType(T10), GetType(T11), GetType(T12), GetType(T13), GetType(T14), GetType(T15), GetType(T16), GetType(T17), GetType(T18), GetType(T19), GetType(T20), GetType(T21), GetType(T22), GetType(T23), GetType(T24), GetType(T25), GetType(T26), GetType(T27), GetType(T28), GetType(T29), GetType(T30), GetType(T31)}
        Public Shared ReadOnly ctors() As System.Reflection.ConstructorInfo = {type(Of T1).ctor, type(Of T2).ctor, type(Of T3).ctor, type(Of T4).ctor, type(Of T5).ctor, type(Of T6).ctor, type(Of T7).ctor, type(Of T8).ctor, type(Of T9).ctor, type(Of T10).ctor, type(Of T11).ctor, type(Of T12).ctor, type(Of T13).ctor, type(Of T14).ctor, type(Of T15).ctor, type(Of T16).ctor, type(Of T17).ctor, type(Of T18).ctor, type(Of T19).ctor, type(Of T20).ctor, type(Of T21).ctor, type(Of T22).ctor, type(Of T23).ctor, type(Of T24).ctor, type(Of T25).ctor, type(Of T26).ctor, type(Of T27).ctor, type(Of T28).ctor, type(Of T29).ctor, type(Of T30).ctor, type(Of T31).ctor}
    End Class

    Public Class type(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32)
        Public Shared ReadOnly types() As System.Type = {GetType(T1), GetType(T2), GetType(T3), GetType(T4), GetType(T5), GetType(T6), GetType(T7), GetType(T8), GetType(T9), GetType(T10), GetType(T11), GetType(T12), GetType(T13), GetType(T14), GetType(T15), GetType(T16), GetType(T17), GetType(T18), GetType(T19), GetType(T20), GetType(T21), GetType(T22), GetType(T23), GetType(T24), GetType(T25), GetType(T26), GetType(T27), GetType(T28), GetType(T29), GetType(T30), GetType(T31), GetType(T32)}
        Public Shared ReadOnly ctors() As System.Reflection.ConstructorInfo = {type(Of T1).ctor, type(Of T2).ctor, type(Of T3).ctor, type(Of T4).ctor, type(Of T5).ctor, type(Of T6).ctor, type(Of T7).ctor, type(Of T8).ctor, type(Of T9).ctor, type(Of T10).ctor, type(Of T11).ctor, type(Of T12).ctor, type(Of T13).ctor, type(Of T14).ctor, type(Of T15).ctor, type(Of T16).ctor, type(Of T17).ctor, type(Of T18).ctor, type(Of T19).ctor, type(Of T20).ctor, type(Of T21).ctor, type(Of T22).ctor, type(Of T23).ctor, type(Of T24).ctor, type(Of T25).ctor, type(Of T26).ctor, type(Of T27).ctor, type(Of T28).ctor, type(Of T29).ctor, type(Of T30).ctor, type(Of T31).ctor, type(Of T32).ctor}
    End Class

#End Region
End Namespace