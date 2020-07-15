Imports ILS

Public Module STD
    Public Const inline As System.Runtime.CompilerServices.MethodImplOptions = System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining
    Public Const external As System.Runtime.CompilerServices.MethodImplOptions = System.Runtime.CompilerServices.MethodImplOptions.ForwardRef
    Public Const manual As System.Runtime.InteropServices.LayoutKind = System.Runtime.InteropServices.LayoutKind.Explicit

    Public ReadOnly app As System.Reflection.Module = sr.Assembly.GetEntryAssembly.ManifestModule
End Module

Module Exten
#Disable Warning IDE0060 ' Remove unused parameter
    <Extension, Method(inline)>
    Sub [void](Of N)(Input As N)
    End Sub
#Enable Warning IDE0060 ' Remove unused parameter

    <Extension, Method(inline)>
    Function [as](Of T, V)(Input As T) As V
        Return Pre.as(Of T, V).cast(Input)
    End Function
End Module

Namespace Pre
    Structure [as](Of T, V)
        Shared cast As Func(Of T, V)
        <Method(inline)>
        Shared Sub New()
            With Info.delegate(Of Func(Of T, V)).create_method 'New sre.DynamicMethod("", GetType(V), {GetType(T)})
                incident(Of sre.OpCode).of(AddressOf .GetILGenerator.Emit)(op.Ldarg_0)(op.Ret).void
                cast = DirectCast(.CreateDelegate(GetType(Func(Of T, V))), Object)
            End With
        End Sub
    End Structure
End Namespace