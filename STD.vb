Public Module STD
    Public Const inline As System.Runtime.CompilerServices.MethodImplOptions = System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining
    Public Const external As System.Runtime.CompilerServices.MethodImplOptions = System.Runtime.CompilerServices.MethodImplOptions.ForwardRef
    Public Const manual As System.Runtime.InteropServices.LayoutKind = System.Runtime.InteropServices.LayoutKind.Explicit

    Public ReadOnly app As System.Reflection.Module = sr.Assembly.GetEntryAssembly.ManifestModule
End Module
