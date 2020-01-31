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

        Public Shared Function create_method() As sre.DynamicMethod
            Return New sre.DynamicMethod("", return_type, param_types, app)
        End Function
        Public Shared Function create_method(Name As String) As sre.DynamicMethod
            Return New sre.DynamicMethod(Name, return_type, param_types, app)
        End Function
        Public Shared Function create_method(Assembly_module As sr.Module) As sre.DynamicMethod
            Return New sre.DynamicMethod("", return_type, param_types, Assembly_module)
        End Function
        Public Shared Function create_method(Name As String, Assembly_module As sr.Module) As sre.DynamicMethod
            Return New sre.DynamicMethod(Name, return_type, param_types, Assembly_module)
        End Function
        Public Shared Function create_method(Base_class As System.Type) As sre.DynamicMethod
            Return New sre.DynamicMethod("", return_type, param_types, Base_class)
        End Function
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
    Public Class type(Of T2, T1)
        Public Shared ReadOnly types() As System.Type = {GetType(T2), GetType(T1)}
        Public Shared ReadOnly ctors() As System.Reflection.ConstructorInfo = {type(Of T2).ctor, type(Of T1).ctor}
    End Class

    Public Class type(Of T3, T2, T1)
        Public Shared ReadOnly types() As System.Type = {GetType(T3), GetType(T2), GetType(T1)}
        Public Shared ReadOnly ctors() As System.Reflection.ConstructorInfo = {type(Of T3).ctor, type(Of T2).ctor, type(Of T1).ctor}
    End Class

    Public Class type(Of T4, T3, T2, T1)
        Public Shared ReadOnly types() As System.Type = {GetType(T4), GetType(T3), GetType(T2), GetType(T1)}
        Public Shared ReadOnly ctors() As System.Reflection.ConstructorInfo = {type(Of T4).ctor, type(Of T3).ctor, type(Of T2).ctor, type(Of T1).ctor}
    End Class

    Public Class type(Of T5, T4, T3, T2, T1)
        Public Shared ReadOnly types() As System.Type = {GetType(T5), GetType(T4), GetType(T3), GetType(T2), GetType(T1)}
        Public Shared ReadOnly ctors() As System.Reflection.ConstructorInfo = {type(Of T5).ctor, type(Of T4).ctor, type(Of T3).ctor, type(Of T2).ctor, type(Of T1).ctor}
    End Class

    Public Class type(Of T6, T5, T4, T3, T2, T1)
        Public Shared ReadOnly types() As System.Type = {GetType(T6), GetType(T5), GetType(T4), GetType(T3), GetType(T2), GetType(T1)}
        Public Shared ReadOnly ctors() As System.Reflection.ConstructorInfo = {type(Of T6).ctor, type(Of T5).ctor, type(Of T4).ctor, type(Of T3).ctor, type(Of T2).ctor, type(Of T1).ctor}
    End Class

    Public Class type(Of T7, T6, T5, T4, T3, T2, T1)
        Public Shared ReadOnly types() As System.Type = {GetType(T7), GetType(T6), GetType(T5), GetType(T4), GetType(T3), GetType(T2), GetType(T1)}
        Public Shared ReadOnly ctors() As System.Reflection.ConstructorInfo = {type(Of T7).ctor, type(Of T6).ctor, type(Of T5).ctor, type(Of T4).ctor, type(Of T3).ctor, type(Of T2).ctor, type(Of T1).ctor}
    End Class

    Public Class type(Of T8, T7, T6, T5, T4, T3, T2, T1)
        Public Shared ReadOnly types() As System.Type = {GetType(T8), GetType(T7), GetType(T6), GetType(T5), GetType(T4), GetType(T3), GetType(T2), GetType(T1)}
        Public Shared ReadOnly ctors() As System.Reflection.ConstructorInfo = {type(Of T8).ctor, type(Of T7).ctor, type(Of T6).ctor, type(Of T5).ctor, type(Of T4).ctor, type(Of T3).ctor, type(Of T2).ctor, type(Of T1).ctor}
    End Class

    Public Class type(Of T9, T8, T7, T6, T5, T4, T3, T2, T1)
        Public Shared ReadOnly types() As System.Type = {GetType(T9), GetType(T8), GetType(T7), GetType(T6), GetType(T5), GetType(T4), GetType(T3), GetType(T2), GetType(T1)}
        Public Shared ReadOnly ctors() As System.Reflection.ConstructorInfo = {type(Of T9).ctor, type(Of T8).ctor, type(Of T7).ctor, type(Of T6).ctor, type(Of T5).ctor, type(Of T4).ctor, type(Of T3).ctor, type(Of T2).ctor, type(Of T1).ctor}
    End Class

    Public Class type(Of T10, T9, T8, T7, T6, T5, T4, T3, T2, T1)
        Public Shared ReadOnly types() As System.Type = {GetType(T10), GetType(T9), GetType(T8), GetType(T7), GetType(T6), GetType(T5), GetType(T4), GetType(T3), GetType(T2), GetType(T1)}
        Public Shared ReadOnly ctors() As System.Reflection.ConstructorInfo = {type(Of T10).ctor, type(Of T9).ctor, type(Of T8).ctor, type(Of T7).ctor, type(Of T6).ctor, type(Of T5).ctor, type(Of T4).ctor, type(Of T3).ctor, type(Of T2).ctor, type(Of T1).ctor}
    End Class
#End Region
End Namespace