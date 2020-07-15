Imports lbl = System.Collections.Generic.Dictionary(Of Integer, System.Reflection.Emit.Label)
Imports cil = System.Reflection.Emit.ILGenerator
Imports sri = System.Runtime.InteropServices

Namespace ILS
    Public Module Builder
        <Extension, Method(inline)>
        Public Function compile(Of T As Class)(Script As String) As ils(Of T)
            Return New ils(Of T)(Script, DirectCast(Nothing, String)) ', Info.delegate(Of T).create_method)
        End Function
        <Extension, Method(inline)>
        Public Function compile(Of T As Class)(Script As String, Assembly_module As sr.Module) As ils(Of T)
            Return New ils(Of T)(Script, Info.delegate(Of T).create_method(Assembly_module), Nothing)

            '==========================
            ' Implement reuse system
            '==========================
            'Dim Collect_key = Script & GetType(T).FullName & Assembly_module.FullyQualifiedName
            'If ils(Of T).collector.ContainsKey(Collect_key) Then
            '    Return New ils(Of T)(Script, Collect_key)
            'Else
            '    Return New ils(Of T)(Script, Info.delegate(Of T).create_method(Assembly_module), Collect_key)
            'End If
        End Function
        <Extension, Method(inline)>
        Public Function compile(Of T As Class, Base As Class)(Script As String) As ils(Of T)
            Return New ils(Of T)(Script, Info.delegate(Of T).create_method(GetType(Base)), Nothing)

            '==========================
            ' Implement reuse system
            '==========================
            'Dim Collect_key = Script & GetType(T).FullName & GetType(Base).FullName
            'If ils(Of T).collector.ContainsKey(Collect_key) Then
            '    Return New ils(Of T)(Script, Collect_key)
            'Else
            '    Return New ils(Of T)(Script, Info.delegate(Of T).create_method(GetType(Base)), Collect_key)
            'End If
        End Function
    End Module

    <sri.StructLayout(manual)>
    Friend Structure reverser
        <sri.FieldOffset(0)>
        Public n As Long

        <sri.FieldOffset(7)>
        Public b1 As Byte
        <sri.FieldOffset(6)>
        Public b2 As Byte
        <sri.FieldOffset(5)>
        Public b3 As Byte
        <sri.FieldOffset(4)>
        Public b4 As Byte
        <sri.FieldOffset(3)>
        Public b5 As Byte
        <sri.FieldOffset(2)>
        Public b6 As Byte
        <sri.FieldOffset(1)>
        Public b7 As Byte
        <sri.FieldOffset(0)>
        Public b8 As Byte

        Shared Function to_string(Input As Long) As String
            Dim R As New reverser With {.n = Input}
            With New Text.StringBuilder
                If R.b1 <> 0 Then .Append(R.b1.as(Of Char))
                If R.b2 <> 0 Then .Append(R.b2.as(Of Char))
                If R.b3 <> 0 Then .Append(R.b3.as(Of Char))
                If R.b4 <> 0 Then .Append(R.b4.as(Of Char))
                If R.b5 <> 0 Then .Append(R.b5.as(Of Char))
                If R.b6 <> 0 Then .Append(R.b6.as(Of Char))
                If R.b7 <> 0 Then .Append(R.b7.as(Of Char))
                If R.b8 <> 0 Then .Append(R.b8.as(Of Char))
                Return .ToString
            End With
        End Function
    End Structure

    Friend Structure collector(Of T As Class)
        Friend Shared ReadOnly items As New Dictionary(Of String, T)
    End Structure

    Friend Enum Stat
        Key
        Num
        Gen
        Word = 4
    End Enum

    '''<typeparam name="T">T is type of delegate.</typeparam>
    Public Structure ils(Of T As Class)
        Friend meth As sre.DynamicMethod
        Friend il As cil
        Friend ReadOnly lbls As lbl

        Friend news() As sr.ConstructorInfo
        Friend uses() As sr.MethodInfo
        Friend useds() As sr.MethodInfo
        Friend fields() As sr.FieldInfo
        Friend types() As System.Type

        Friend txts() As String
        Friend i4s() As Int32
        Friend i8s() As Int64
        Friend r4s() As Single
        Friend r8s() As Double

        Friend key As Int64, num As Int32
        Friend stat As Stat
        Friend ReadOnly word As Text.StringBuilder

        Public code As String

        '==========================
        ' Implement reuse system
        '==========================
        Friend ReadOnly be_collected As Boolean
        Friend ReadOnly key_collected As String
        '==========================

#If IL_DEBUG Then
        Public stack As Stack(Of Type)
#End If

        <Method(inline)>
        Friend Sub New(Code As String)
            Me.New(Code, Info.delegate(Of T).create_method, Code)
        End Sub
        <Method(inline)>
        Public Sub New(Code As String, Method As sre.DynamicMethod)
            Me.New(Code, Method, Nothing)
        End Sub
        <Method(inline)>
        Friend Sub New(Code As String, Collect_key As String)
            Me.New(Code, Info.delegate(Of T).create_method, Collect_key)
        End Sub

        <Method(inline)>
        Friend Sub New(Code As String, Method As sre.DynamicMethod, Collect_key As String)
#If IL_DEBUG Then
            Debug.WriteLine("===========================")
            Debug.WriteLine(Method.ToString)
            Debug.WriteLine(Code)
            Debug.WriteLine("===========================")
            stack = New Stack(Of Type)
#End If
            If Collect_key IsNot Nothing Then be_collected = collector(Of T).items.ContainsKey(Collect_key)

            If Not be_collected Then
                meth = Method
                il = Method.GetILGenerator
                Me.code = Code
                lbls = New lbl
                word = New Text.StringBuilder
                useds = Info.delegate(Of T).invoke_param
            End If
        End Sub
    End Structure

    Public Module Exten_ils
        <Extension, Method(inline)>
        Private Function _lbl(Il As cil, Lbls As lbl, Input As Int32) As sre.Label
            If Not Lbls.ContainsKey(Input) Then Lbls.Add(Input, Il.DefineLabel)
            Return Lbls(Input)
        End Function

        <Extension, Method(inline)>
        Private Sub _keygen(Of T As Class)(Host As ils(Of T))
            Dim Il = Host.il
            Dim Fields = Host.fields
            Dim Num = Host.num
            Dim Meth = Host.meth
            Dim Uses = Host.uses
            Dim Types = Host.types
            Dim News = Host.news
            Dim Lbls = Host.lbls

#If IL_DEBUG Then
            Dim Stack = Host.stack
#End If
            Select Case Host.key
#Region "Constant"
                Case 0
                    Select Case Host.stat
                        Case Stat.Num
                            Select Case Num
                                Case 0
                                    Il.Emit(op.Ldc_I4_0)
                                Case 1
                                    Il.Emit(op.Ldc_I4_1)
                                Case 2
                                    Il.Emit(op.Ldc_I4_2)
                                Case 3
                                    Il.Emit(op.Ldc_I4_3)
                                Case 4
                                    Il.Emit(op.Ldc_I4_4)
                                Case 5
                                    Il.Emit(op.Ldc_I4_5)
                                Case 6
                                    Il.Emit(op.Ldc_I4_6)
                                Case 7
                                    Il.Emit(op.Ldc_I4_7)
                                Case 8
                                    Il.Emit(op.Ldc_I4_8)
                                Case -128 To 127
                                    Il.Emit(op.Ldc_I4_S, CSByte(Num))
                                Case < 256
                                    Il.Emit(op.Ldc_I4_S, CByte(Num))
                                Case Else
                                    Il.Emit(op.Ldc_I4, Num)
                            End Select
#If IL_DEBUG Then
                            Debug.WriteLine($"ldc.i4 {Num}")
                            Stack.su(Of Int32)()
#End If
                        Case Stat.Word
                            Il.Emit(op.Ldstr, Host.word.ToString)
#If IL_DEBUG Then
                            Debug.WriteLine($"ldstr {Host.word}")
                            Stack.su(Of String)()
#End If
                    End Select

                'Load constant data
                Case (AscW("t"c) << 8 * 2) + (AscW("x"c) << 8) + AscW("t"c)
                    Il.Emit(op.Ldstr, Host.txts(Num))
#If IL_DEBUG Then
                    Debug.WriteLine($"ldstr {Host.txts(Num)}")
                    Stack.su(Of String)()
#End If
                Case (AscW("i"c) << 8) + AscW("4"c)
                    Il.Emit(op.Ldc_I4, Host.i4s(Num))
#If IL_DEBUG Then
                    Debug.WriteLine($"ldc.i4 {Host.i4s(Num)}")
                    Stack.su(Of Int32)()
#End If
                Case (AscW("i"c) << 8) + AscW("8"c)
                    Il.Emit(op.Ldc_I8, Host.i8s(Num))
#If IL_DEBUG Then
                    Debug.WriteLine($"ldc.i8 {Host.i8s(Num)}")
                    Stack.su(Of Int64)()
#End If
                Case (AscW("r"c) << 8) + AscW("4"c)
                    Il.Emit(op.Ldc_R4, Host.r4s(Num))
#If IL_DEBUG Then
                    Debug.WriteLine($"ldc.r4 {Host.r4s(Num)}")
                    Stack.su(Of Single)()
#End If
                Case (AscW("r"c) << 8) + AscW("8"c)
                    Il.Emit(op.Ldc_R8, Host.r8s(Num))
#If IL_DEBUG Then
                    Debug.WriteLine($"ldc.r8 {Host.r8s(Num)}")
                    Stack.su(Of Double)()
#End If
#End Region

#Region "Var"
                Case (AscW("l"c) << 8) + AscW("a"c)
                    Select Case Num
                        Case 0
                            Il.Emit(op.Ldarg_0)
                        Case 1
                            Il.Emit(op.Ldarg_1)
                        Case 2
                            Il.Emit(op.Ldarg_2)
                        Case 3
                            Il.Emit(op.Ldarg_3)
                        Case 0 To 255
                            Il.Emit(op.Ldarg_S, CByte(Num))
                        Case Is > 255
                            Il.Emit(op.Ldarg, Num)
                    End Select
#If IL_DEBUG Then
                    Debug.WriteLine($"ldarg {Num}")
                    Stack.su(Info.delegate(Of T).param_types(Num))
#End If
                Case (AscW("s"c) << 8) + AscW("a"c)
                    If Num < 256 Then Il.Emit(op.Starg_S, CByte(Num)) _
                                 Else Il.Emit(op.Starg, Num)
#If IL_DEBUG Then
                    Debug.WriteLine($"starg {Num}")
                    Stack.so()
#End If
                Case (AscW("l"c) << 8 * 2) + (AscW("a"c) << 8) + AscW("a"c)
                    If Num < 256 Then Il.Emit(op.Ldarga_S, CByte(Num)) _
                                 Else Il.Emit(op.Ldarga, Num)
#If IL_DEBUG Then
                    Debug.WriteLine($"ldarga {Num}")
                    Stack.su(Of UIntPtr)()
#End If
                        'Local variant
                Case (AscW("l"c) << 8) + AscW("o"c)
                    Select Case Num
                        Case 0
                            Il.Emit(op.Ldloc_0)
                        Case 1
                            Il.Emit(op.Ldloc_1)
                        Case 2
                            Il.Emit(op.Ldloc_2)
                        Case 3
                            Il.Emit(op.Ldloc_3)
                        Case 0 To 255
                            Il.Emit(op.Ldloc_S, Num)
                        Case > 255
                            Il.Emit(op.Ldloc, Num)
                    End Select
#If IL_DEBUG Then
                    Debug.WriteLine($"ldloc.s {Num}")
                    Stack.su(Of local)()
#End If
                Case (AscW("s"c) << 8) + AscW("o"c)
                    Select Case Num
                        Case 0
                            Il.Emit(op.Stloc_0)
                        Case 1
                            Il.Emit(op.Stloc_1)
                        Case 2
                            Il.Emit(op.Stloc_2)
                        Case 3
                            Il.Emit(op.Stloc_3)
                        Case 0 To 255
                            Il.Emit(op.Stloc_S, Num)
                        Case Is > 255
                            Il.Emit(op.Stloc, Num)
                    End Select
#If IL_DEBUG Then
                    Debug.WriteLine($"stloc.s {Num}")
                    Stack.so()
#End If
                Case (AscW("l"c) << 8 * 2) + (AscW("a"c) << 8) + AscW("o"c)
                    If Num < 256 Then Il.Emit(op.Ldloca_S, CByte(Num)) _
                                 Else Il.Emit(op.Ldloca, Num)
#If IL_DEBUG Then
                    Debug.WriteLine($"ldloca.s {Num}")
                    Stack.su(Of UIntPtr)()
#End If
#End Region

#Region "Branch"
                Case AscW(":"c)
                    Il.MarkLabel(Il._lbl(Lbls, Num))
#If IL_DEBUG Then
                    Debug.WriteLine($"{Num}:")
#End If
                Case (AscW(":"c) << 8) + AscW(":"c)
                    Il.Emit(op.Br, Il._lbl(Lbls, Num))
#If IL_DEBUG Then
                    Debug.WriteLine($"br {Num}")
#End If
                Case (AscW("t"c) << 8) + AscW(":"c)
                    Il.Emit(op.Brtrue, Il._lbl(Lbls, Num))
#If IL_DEBUG Then
                    Debug.WriteLine($"brtrue {Num}")
                    Stack.so()
#End If
                Case (AscW("f"c) << 8) + AscW(":"c)
                    Il.Emit(op.Brfalse, Il._lbl(Lbls, Num))
#If IL_DEBUG Then
                    Debug.WriteLine($"brfalse {Num}")
                    Stack.so()
#End If
                Case (AscW("="c) << 8) + AscW(":"c)
                    Il.Emit(op.Beq, Il._lbl(Lbls, Num))
#If IL_DEBUG Then
                    Debug.WriteLine($"beq {Num}")
                    Stack.soo()
#End If
                Case (AscW(">"c) << 8) + AscW(":"c)
                    Il.Emit(op.Bgt, Il._lbl(Lbls, Num))
#If IL_DEBUG Then
                    Debug.WriteLine($"bgt {Num}")
                    Stack.soo()
#End If
                Case (AscW("<"c) << 8) + AscW(":"c)
                    Il.Emit(op.Blt, Il._lbl(Lbls, Num))
#If IL_DEBUG Then
                    Debug.WriteLine($"blt {Num}")
                    Stack.soo()
#End If
                Case (AscW(">"c) << 8 * 2) + (AscW("="c) << 8) + AscW(":"c)
                    Il.Emit(op.Bge, Il._lbl(Lbls, Num))
#If IL_DEBUG Then
                    Debug.WriteLine($"bge {Num}")
                    Stack.soo()
#End If
                Case (AscW("<"c) << 8 * 2) + (AscW("="c) << 8) + AscW(":"c)
                    Il.Emit(op.Ble, Il._lbl(Lbls, Num))
#If IL_DEBUG Then
                    Debug.WriteLine($"ble {Num}")
                    Stack.soo()
#End If

                Case (AscW("!"c) << 8 * 2) + (AscW("="c) << 8) + AscW(":"c)
                    Il.Emit(op.Bne_Un, Il._lbl(Lbls, Num))
#If IL_DEBUG Then
                    Debug.WriteLine($"bne.un {Num}")
                    Stack.soo()
#End If
                Case (AscW("u"c) << 8 * 2) + (AscW(">"c) << 8) + AscW(":"c)
                    Il.Emit(op.Bgt_Un, Il._lbl(Lbls, Num))
#If IL_DEBUG Then
                    Debug.WriteLine($"bgt.un {Num}")
                    Stack.soo()
#End If
                Case (AscW("u"c) << 8 * 2) + (AscW("<"c) << 8) + AscW(":"c)
                    Il.Emit(op.Blt_Un, Il._lbl(Lbls, Num))
#If IL_DEBUG Then
                    Debug.WriteLine($"blt.un {Num}")
                    Stack.soo()
#End If
                Case (AscW("u"c) << 8 * 3) + (AscW(">"c) << 8 * 2) + (AscW("="c) << 8) + AscW(":"c)
                    Il.Emit(op.Bge_Un, Il._lbl(Lbls, Num))
#If IL_DEBUG Then
                    Debug.WriteLine($"bge.un {Num}")
                    Stack.soo()
#End If
                Case (AscW("u"c) << 8 * 3) + (AscW("<"c) << 8 * 2) + (AscW("="c) << 8) + AscW(":"c)
                    Il.Emit(op.Ble_Un, Il._lbl(Lbls, Num))
#If IL_DEBUG Then
                    Debug.WriteLine($"ble.un {Num}")
                    Stack.soo()
#End If
#End Region

#Region "Common"
                Case (AscW("!"c) << 8) + AscW("!"c) : Il.Emit(op.Break)
#If IL_DEBUG Then
                    Debug.WriteLine("break")
#End If
                Case (AscW("["c) << 8 * 2) + (AscW("-"c) << 8) + AscW("]"c) : Il.Emit(op.Pop)
#If IL_DEBUG Then
                    Debug.WriteLine("pop")
                    Stack.so()
#End If
                Case (AscW("["c) << 8 * 2) + (AscW("+"c) << 8) + AscW("]"c) : Il.Emit(op.Dup)
#If IL_DEBUG Then
                    Debug.WriteLine("dup")
                    Stack.su()
#End If
                        'Must not has anything left in stack except uint of new alloc size.
                Case (AscW("["c) << 8 * 2) + (AscW("n"c) << 8) + AscW("]"c) : Il.Emit(op.Localloc)
#If IL_DEBUG Then
                    Debug.WriteLine("alloc")
                    If Stack.Count <> 1 Then Throw New Exception("Only alloc size value could be on stack when alloc stack frame memory.")
                    Stack.sou(Of UIntPtr)()
#End If
#End Region

#Region "Math"
                Case AscW("+"c) : Il.Emit(op.Add)
#If IL_DEBUG Then
                    Debug.WriteLine("add")
                    Stack.soou()
#End If
                Case AscW("-"c) : Il.Emit(op.Sub)
#If IL_DEBUG Then
                    Debug.WriteLine("sub")
                    Stack.soou()
#End If
                Case AscW("*"c) : Il.Emit(op.Mul)
#If IL_DEBUG Then
                    Debug.WriteLine("mul")
                    Stack.soou()
#End If
                Case AscW("/"c) : Il.Emit(op.Div)
#If IL_DEBUG Then
                    Debug.WriteLine("div")
                    Stack.soou()
#End If
                Case AscW("%"c) : Il.Emit(op.[Rem]) 'mod
#If IL_DEBUG Then
                    Debug.WriteLine("rem")
                    Stack.soou()
#End If

                Case (AscW("-"c) << 8) + AscW("1"c) : Il.Emit(op.Ldc_I4_M1)
#If IL_DEBUG Then
                    Debug.WriteLine("ldc.i4.m1")
                    Stack.su(Of Int32)()
#End If

                Case (AscW("+"c) << 8) + AscW("+"c)
                    Il.Emit(op.Ldc_I4_1)
                    Il.Emit(op.Add)
#If IL_DEBUG Then
                    Debug.WriteLine($"ldc.i4.1")
                    Debug.WriteLine("add")
                    Stack.sou()
#End If
                Case (AscW("-"c) << 8) + AscW("-"c)
                    Il.Emit(op.Ldc_I4_1)
                    Il.Emit(op.Sub)
#If IL_DEBUG Then
                    Debug.WriteLine("ldc.i4.1")
                    Debug.WriteLine("sub")
                    Stack.sou()
#End If
                Case (AscW("*"c) << 8) + AscW("*"c)
                    Il.Emit(op.Dup)
                    Il.Emit(op.Mul)
#If IL_DEBUG Then
                    Debug.WriteLine("dup")
                    Debug.WriteLine("mul")
                    Stack.sou()
#End If
                Case (AscW("-"c) << 8) + AscW("*"c)
                    Il.Emit(op.Ldc_I4_M1)
                    Il.Emit(op.Mul)
#If IL_DEBUG Then
                    Debug.WriteLine("ldc.i4.m1")
                    Debug.WriteLine("mul")
                    Stack.sou()
#End If
                Case (AscW("-"c) << 8) + AscW("/"c)
                    Il.Emit(op.Ldc_I4_M1)
                    Il.Emit(op.Div)
#If IL_DEBUG Then
                    Debug.WriteLine("ldc.i4.m1")
                    Debug.WriteLine("div")
                    Stack.sou()
#End If

                Case AscW("&"c) : Il.Emit(op.And)
#If IL_DEBUG Then
                    Debug.WriteLine("and")
                    Stack.soou()
#End If
                Case AscW("|"c) : Il.Emit(op.Or)
#If IL_DEBUG Then
                    Debug.WriteLine("or")
                    Stack.soou()
#End If
                Case (AscW("&"c) << 8) + AscW("|"c) : Il.Emit(op.Xor)
#If IL_DEBUG Then
                    Debug.WriteLine("xor")
                    Stack.soou()
#End If
                Case AscW("!"c) : Il.Emit(op.Not)
#If IL_DEBUG Then
                    Debug.WriteLine("not")
                    Stack.rmin(1)
#End If

                Case AscW(">"c) : Il.Emit(op.Cgt)
#If IL_DEBUG Then
                    Debug.WriteLine("cgt")
                    Stack.soou(Of Boolean)()
#End If
                Case AscW("<"c) : Il.Emit(op.Clt)
#If IL_DEBUG Then
                    Debug.WriteLine("clt")
                    Stack.soou(Of Boolean)()
#End If
                Case AscW("="c) : Il.Emit(op.Ceq)
#If IL_DEBUG Then
                    Debug.WriteLine("ceq")
                    Stack.soou(Of Boolean)()
#End If

                Case (AscW("<"c) << 8) + AscW("<"c) : Il.Emit(op.Shl)
#If IL_DEBUG Then
                    Debug.WriteLine("shl")
                    Stack.soou()
#End If
                Case (AscW(">"c) << 8) + AscW(">"c) : Il.Emit(op.Shr)
#If IL_DEBUG Then
                    Debug.WriteLine("shr")
                    Stack.soou()
#End If
#End Region

#Region "Pointer"
                        'Object type
                Case (AscW("l"c) << 8 * 2) + (AscW("o"c) << 8) + AscW("t"c)
                    Il.Emit(op.Ldobj, Types(Num))
#If IL_DEBUG Then
                    Debug.WriteLine($"ldobj {Types(Num).Name}")
                    Stack.sou(Types(Num))
#End If
                Case (AscW("s"c) << 8 * 2) + (AscW("o"c) << 8) + AscW("t"c)
                    Il.Emit(op.Stobj, Types(Num))
#If IL_DEBUG Then
                    Debug.WriteLine($"stobj {Types(Num).Name}")
                    Stack.soo()
#End If
                Case (AscW("c"c) << 8 * 2) + (AscW("o"c) << 8) + AscW("t"c)
                    Il.Emit(op.Castclass, Types(Num))
#If IL_DEBUG Then
                    Debug.WriteLine($"castclass {Types(Num)}")
                    Stack.sou(Types(Num))
#End If

                    'Array element
                Case (AscW("l"c) << 8 * 2) + (AscW("e"c) << 8) + AscW("n"c)
                    Il.Emit(op.Ldlen)
#If IL_DEBUG Then
                    Debug.WriteLine("ldlen")
                    Stack.sou(Of uint)()
#End If
                Case (AscW("l"c) << 8 * 3) + (AscW("a"c) << 8 * 2) + (AscW("e"c) << 8) + AscW("t"c)
                    Il.Emit(op.Ldelema, Types(Num))
#If IL_DEBUG Then
                    Debug.WriteLine($"ldlema {Types(Num)}")
                    Stack.soou(Of UIntPtr)()
#End If
                Case (AscW("l"c) << 8 * 2) + (AscW("e"c) << 8) + AscW("t"c)
                    Il.Emit(op.Ldelem, Types(Num))
#If IL_DEBUG Then
                    Debug.WriteLine($"ldlem {Types(Num)}")
                    Stack.soou(Types(Num))
#End If
                Case (AscW("s"c) << 8 * 2) + (AscW("e"c) << 8) + AscW("t"c)
                    Il.Emit(op.Stelem, Types(Num))
#If IL_DEBUG Then
                    Debug.WriteLine($"stlem {Types(Num)}")
                    Stack.sox(3)
#End If

                    'Memory manage
                Case (AscW("c"c) << 8 * 3) + (AscW("p"c) << 8 * 2) + (AscW("y"c) << 8) + AscW("t"c)
                    Il.Emit(op.Cpobj, Types(Num))
#If IL_DEBUG Then
                    Debug.WriteLine($"cpobj {Types(Num)}")
                    Stack.soo()
#End If
                Case (AscW("i"c) << 8 * 3) + (AscW("n"c) << 8 * 2) + (AscW("i"c) << 8) + AscW("t"c)
                    Il.Emit(op.Initobj, Types(Num))
#If IL_DEBUG Then
                    Debug.WriteLine($"initobj {Types(Num)}")
                    Stack.so()
#End If
                Case (AscW("c"c) << 8 * 3) + (AscW("p"c) << 8 * 2) + (AscW("y"c) << 8) + AscW("b"c)
                    Il.Emit(op.Cpblk)
#If IL_DEBUG Then
                    Debug.WriteLine("cpblk")
                    Stack.sox(3)
#End If
                Case (AscW("i"c) << 8 * 3) + (AscW("n"c) << 8 * 2) + (AscW("i"c) << 8) + AscW("b"c)
                    Il.Emit(op.Initblk)
#If IL_DEBUG Then
                    Debug.WriteLine("initblk")
                    Stack.sox(3)
#End If
#End Region

#Region "Method"
                Case AscW(";"c) : Il.Emit(op.Ret)
#If IL_DEBUG Then
                    Stack.rexit(Meth)
#End If
                'Invoke method
                '[re] with [use] for recursion method
                Case (AscW("r"c) << 8) + AscW("e"c) : Il.Emit(op.Tailcall)
#If IL_DEBUG Then
                    Debug.Write("tail.")
#End If
                Case (AscW("u"c) << 8 * 2) + (AscW("s"c) << 8) + AscW("e"c)
                    Il.Emit(op.Call, Uses(Num))
#If IL_DEBUG Then
                    Debug.WriteLine($"call {Uses(Num)}")
                    Stack.soxu(Uses(Num).GetParameters.Length + If(Uses(Num).IsVirtual, 1, 0),
                               Uses(Num).ReturnType)
#End If
                Case (AscW("u"c) << 8 * 3) + (AscW("s"c) << 8 * 2) + (AscW("e"c) << 8) + AscW("d"c)
                    Il.Emit(op.Call, Host.useds(Num))
#If IL_DEBUG Then
                    Debug.WriteLine($"call {Host.useds(Num)}")
                    Stack.soxu(Host.useds(Num).GetParameters.Length + 1,
                               Host.useds(Num).ReturnType)
#End If
                        '[use-me]
                Case (CLng(AscW("u"c)) << 8 * 5) + (CLng(AscW("s"c)) << 8 * 4) + (AscW("e"c) << 8 * 3) +
                     (AscW("-"c) << 8 * 2) + (AscW("m"c) << 8) + AscW("e"c)
                    Il.Emit(op.Call, Meth)
#If IL_DEBUG Then
                    Debug.WriteLine($"call {Meth}")
                    Stack.soxu(Meth.GetParameters.Length, Meth.ReturnType)
#End If
                Case (AscW("j"c) << 8 * 2) + (AscW("m"c) << 8) + AscW("p"c) : Il.Emit(op.Jmp, Uses(Num))
#If IL_DEBUG Then
                    Debug.WriteLine($"jmp {Uses(Num)}")
                    Stack.rmax(0)
#End If
                        '[jmp-me]
                Case (CLng(AscW("j"c)) << 8 * 5) + (CLng(AscW("m"c)) << 8 * 4) + (AscW("p"c) << 8 * 3) +
                     (AscW("-"c) << 8 * 2) + (AscW("m"c) << 8) + AscW("e"c)
                    Il.Emit(op.Jmp, Meth)
#If IL_DEBUG Then
                    Debug.WriteLine($"jmp {Meth}")
                    Stack.rmax(0)
#End If
#End Region

#Region "Constructor"
                    'Get size of data
                Case (AscW("m"c) << 8) + AscW("t"c)
                    Il.Emit(op.Sizeof, Types(Num))
#If IL_DEBUG Then
                    Debug.WriteLine($"sizeof {Types(Num)}")
                    Stack.su(Of int)()
#End If

                        'New object and array
                Case (AscW("n"c) << 8 * 2) + (AscW("a"c) << 8) + AscW("t"c)
                    Il.Emit(op.Newarr, Types(Num))
#If IL_DEBUG Then
                    Debug.WriteLine($"newarr {Types(Num)}")
                    Stack.sou(Types(Num).MakeArrayType)
#End If
                Case (AscW("n"c) << 8 * 2) + (AscW("e"c) << 8) + AscW("w"c)
                    Dim Ctor = If(News Is Nothing, Types(Num).GetConstructors(16 Or 32 Or 4)(0), News(Num))
                    Il.Emit(op.Newobj, Ctor)
#If IL_DEBUG Then
                    Debug.WriteLine($"newobj {Ctor}")
                    Stack.soxu(Ctor.GetParameters.Length, Ctor.DeclaringType)
#End If
                Case (AscW("a"c) << 8 * 3) + (AscW("n"c) << 8 * 2) + (AscW("e"c) << 8) + AscW("w"c)
                    Il.Emit(op.Call, News(Num))
#If IL_DEBUG Then
                    Debug.WriteLine($"call {News(Num)}")
                    Stack.sox(News(Num).GetParameters.Length + 1)
#End If
#End Region

#Region "Field"
                'Field
                Case (AscW("l"c) << 8) + AscW("f"c)
                    Il.Emit(op.Ldfld, Fields(Num))
#If IL_DEBUG Then
                    Debug.WriteLine($"ldfld {Fields(Num)}")
                    Stack.sou(Fields(Num).FieldType)
#End If
                Case (AscW("s"c) << 8) + AscW("f"c)
                    Il.Emit(op.Stfld, Fields(Num))
#If IL_DEBUG Then
                    Debug.WriteLine($"stfld {Fields(Num)}")
                    Stack.soo()
#End If
                Case (AscW("l"c) << 8 * 2) + (AscW("a"c) << 8) + AscW("f"c)
                    Il.Emit(op.Ldflda, Fields(Num))
#If IL_DEBUG Then
                    Debug.WriteLine($"ldflda {Fields(Num)}")
                    Stack.sou(Of UIntPtr)()
#End If

                        'Static field
                Case (AscW("l"c) << 8 * 2) + (AscW("s"c) << 8) + AscW("f"c)
                    Il.Emit(op.Ldsfld, Fields(Num))
#If IL_DEBUG Then
                    Debug.WriteLine($"ldsfld {Fields(Num)}")
                    Stack.su(Fields(Num).FieldType)
#End If
                Case (AscW("s"c) << 8 * 2) + (AscW("s"c) << 8) + AscW("f"c)
                    Il.Emit(op.Stsfld, Fields(Num))
#If IL_DEBUG Then
                    Debug.WriteLine($"stsfld {Fields(Num)}")
                    Stack.so()
#End If
                Case (AscW("l"c) << 8 * 3) + (AscW("a"c) << 8 * 2) + (AscW("s"c) << 8) + AscW("f"c)
                    Il.Emit(op.Ldsflda, Fields(Num))
#If IL_DEBUG Then
                    Debug.WriteLine($"ldflda {Fields(Num)}")
                    Stack.su(Of UIntPtr)()
#End If
#End Region

#Region "Indirect"
                Case (AscW("l"c) << 8) + AscW("i"c)
                    Il.Emit(op.Ldind_I)
#If IL_DEBUG Then
                    Debug.WriteLine("ldind.i")
                    Stack.sou(Of IntPtr)()
#End If
                Case (AscW("l"c) << 8 * 2) + (AscW("i"c) << 8) + AscW("1"c)
                    Il.Emit(op.Ldind_I1)
#If IL_DEBUG Then
                    Debug.WriteLine("ldind.i1")
                    Stack.sou(Of SByte)()
#End If
                Case (AscW("l"c) << 8 * 2) + (AscW("i"c) << 8) + AscW("2"c)
                    Il.Emit(op.Ldind_I2)
#If IL_DEBUG Then
                    Debug.WriteLine("ldind.i2")
                    Stack.sou(Of Short)()
#End If
                Case (AscW("l"c) << 8 * 2) + (AscW("i"c) << 8) + AscW("4"c)
                    Il.Emit(op.Ldind_I4)
#If IL_DEBUG Then
                    Debug.WriteLine("ldind.i4")
                    Stack.sou(Of Integer)()
#End If
                Case (AscW("l"c) << 8 * 2) + (AscW("i"c) << 8) + AscW("8"c)
                    Il.Emit(op.Ldind_I8)
#If IL_DEBUG Then
                    Debug.WriteLine("ldind.i8")
                    Stack.sou(Of Long)()
#End If
                Case (AscW("l"c) << 8 * 2) + (AscW("r"c) << 8) + AscW("4"c)
                    Il.Emit(op.Ldind_R4)
#If IL_DEBUG Then
                    Debug.WriteLine("ldind.r4")
                    Stack.sou(Of Single)()
#End If
                Case (AscW("l"c) << 8 * 2) + (AscW("r"c) << 8) + AscW("8"c)
                    Il.Emit(op.Ldind_R8)
#If IL_DEBUG Then
                    Debug.WriteLine("ldind.r8")
                    Stack.sou(Of Double)()
#End If
                Case (AscW("l"c) << 8) + AscW("u"c)
                    Il.Emit(op.Ldind_Ref)
#If IL_DEBUG Then
                    Debug.WriteLine("ldind.ref")
                    Stack.sou(Of UIntPtr)()
#End If
                Case (AscW("l"c) << 8 * 2) + (AscW("u"c) << 8) + AscW("1"c)
                    Il.Emit(op.Ldind_U1)
#If IL_DEBUG Then
                    Debug.WriteLine("ldind.u1")
                    Stack.sou(Of Byte)()
#End If
                Case (AscW("l"c) << 8 * 2) + (AscW("u"c) << 8) + AscW("2"c)
                    Il.Emit(op.Ldind_U2)
#If IL_DEBUG Then
                    Debug.WriteLine("ldind.u2")
                    Stack.sou(Of UShort)()
#End If
                Case (AscW("l"c) << 8 * 2) + (AscW("u"c) << 8) + AscW("4"c)
                    Il.Emit(op.Ldind_U4)
#If IL_DEBUG Then
                    Debug.WriteLine("ldind.u4")
                    Stack.sou(Of UInteger)()
#End If

                Case (AscW("s"c) << 8) + AscW("i"c)
                    Il.Emit(op.Stind_I)
#If IL_DEBUG Then
                    Debug.WriteLine("stind.i")
                    Stack.soo()
#End If
                Case (AscW("s"c) << 8 * 2) + (AscW("i"c) << 8) + AscW("1"c)
                    Il.Emit(op.Stind_I1)
#If IL_DEBUG Then
                    Debug.WriteLine("stind.i1")
                    Stack.soo()
#End If
                Case (AscW("s"c) << 8 * 2) + (AscW("i"c) << 8) + AscW("2"c)
                    Il.Emit(op.Stind_I2)
#If IL_DEBUG Then
                    Debug.WriteLine("stind.i2")
                    Stack.soo()
#End If
                Case (AscW("s"c) << 8 * 2) + (AscW("i"c) << 8) + AscW("4"c)
                    Il.Emit(op.Stind_I4)
#If IL_DEBUG Then
                    Debug.WriteLine("stind.4")
                    Stack.soo()
#End If
                Case (AscW("s"c) << 8 * 2) + (AscW("i"c) << 8) + AscW("8"c)
                    Il.Emit(op.Stind_I8)
#If IL_DEBUG Then
                    Debug.WriteLine("stind.i8")
                    Stack.soo()
#End If
                Case (AscW("s"c) << 8 * 2) + (AscW("r"c) << 8) + AscW("4"c)
                    Il.Emit(op.Stind_R4)
#If IL_DEBUG Then
                    Debug.WriteLine("stind.r4")
                    Stack.soo()
#End If
                Case (AscW("s"c) << 8 * 2) + (AscW("r"c) << 8) + AscW("8"c)
                    Il.Emit(op.Stind_R8)
#If IL_DEBUG Then
                    Debug.WriteLine("stind.r8")
                    Stack.soo()
#End If
                Case (AscW("s"c) << 8) + AscW("u"c)
                    Il.Emit(op.Stind_Ref)
#If IL_DEBUG Then
                    Debug.WriteLine("stind.ref")
                    Stack.soo()
#End If
#End Region

#Region "Convert"
                Case (AscW("c"c) << 8) + AscW("i"c)
                    Il.Emit(op.Conv_I)
#If IL_DEBUG Then
                    Debug.WriteLine("conv.i")
                    Stack.sou(Of IntPtr)()
#End If
                Case (AscW("c"c) << 8 * 2) + (AscW("i"c) << 8) + AscW("1"c)
                    Il.Emit(op.Conv_I1)
#If IL_DEBUG Then
                    Debug.WriteLine("conv.i1")
                    Stack.sou(Of SByte)()
#End If
                Case (AscW("c"c) << 8 * 2) + (AscW("i"c) << 8) + AscW("2"c)
                    Il.Emit(op.Conv_I2)
#If IL_DEBUG Then
                    Debug.WriteLine("conv.i2")
                    Stack.sou(Of Short)()
#End If
                Case (AscW("c"c) << 8 * 2) + (AscW("i"c) << 8) + AscW("4"c)
                    Il.Emit(op.Conv_I4)
#If IL_DEBUG Then
                    Debug.WriteLine("conv.i4")
                    Stack.sou(Of Integer)()
#End If
                Case (AscW("c"c) << 8 * 2) + (AscW("i"c) << 8) + AscW("8"c)
                    Il.Emit(op.Conv_I8)
#If IL_DEBUG Then
                    Debug.WriteLine("conv.i8")
                    Stack.sou(Of Long)()
#End If

                Case (AscW("c"c) << 8) + AscW("u"c)
                    Il.Emit(op.Conv_U)
#If IL_DEBUG Then
                    Debug.WriteLine("conv.u")
                    Stack.sou(Of UIntPtr)()
#End If
                Case (AscW("c"c) << 8 * 2) + (AscW("u"c) << 8) + AscW("1"c)
                    Il.Emit(op.Conv_U1)
#If IL_DEBUG Then
                    Debug.WriteLine("conv.u1")
                    Stack.sou(Of Byte)()
#End If
                Case (AscW("c"c) << 8 * 2) + (AscW("u"c) << 8) + AscW("2"c)
                    Il.Emit(op.Conv_U2)
#If IL_DEBUG Then
                    Debug.WriteLine("conv.u2")
                    Stack.sou(Of UShort)()
#End If
                Case (AscW("c"c) << 8 * 2) + (AscW("u"c) << 8) + AscW("4"c)
                    Il.Emit(op.Conv_U4)
#If IL_DEBUG Then
                    Debug.WriteLine("conv.u4")
                    Stack.sou(Of UInteger)()
#End If
                Case (AscW("c"c) << 8 * 2) + (AscW("u"c) << 8) + AscW("8"c)
                    Il.Emit(op.Conv_U8)
#If IL_DEBUG Then
                    Debug.WriteLine("conv.u8")
                    Stack.sou(Of ULong)()
#End If

                Case (AscW("c"c) << 8 * 2) + (AscW("r"c) << 8) + AscW("4"c)
                    Il.Emit(op.Conv_R4)
#If IL_DEBUG Then
                    Debug.WriteLine("conv.r4")
                    Stack.sou(Of Single)()
#End If
                Case (AscW("c"c) << 8 * 2) + (AscW("r"c) << 8) + AscW("8"c)
                    Il.Emit(op.Conv_R8)
#If IL_DEBUG Then
                    Debug.WriteLine("conv.r8")
                    Stack.sou(Of Double)()
#End If
#End Region

#Region "Special"
                'Define Field Type - register field member of type
                Case (AscW("d"c) << 8 * 2) + (AscW("f"c) << 8) + AscW("t"c)
                    If Fields Is Nothing Then
                        Fields = {Types(Num).GetField(Host.word.ToString)}
                    Else
                        ReDim Preserve Fields(Fields.Length)
                        Fields(Fields.Length - 1) = Types(Num).GetField(Host.word.ToString)
                    End If

                 'Case (AscW("d"c) << 8 * 2) + (AscW("m"c) << 8) + AscW("t"c)
                 '    types(num).GetMethod(word.ToString)

                'Define Local Type - declare local variant
                Case (AscW("d"c) << 8 * 2) + (AscW("o"c) << 8) + AscW("t"c)
                    Il.DeclareLocal(Types(Num))
#End Region

#Region "Extra"
                    'Argument
                Case (AscW("c"c) << 8) + AscW("a"c)
                    Il.Emit(op.Dup)
                    If Num < 256 Then Il.Emit(op.Starg_S, CByte(Num)) _
                                 Else Il.Emit(op.Starg, Num)
#If IL_DEBUG Then
                    Debug.WriteLine("dup")
                    Debug.WriteLine($"starg.s {Num}")
#End If
                        'Local variant
                Case (AscW("c"c) << 8) + AscW("o"c)
                    Il.Emit(op.Dup)
                    Select Case Num
                        Case 0
                            Il.Emit(op.Stloc_0)
                        Case 1
                            Il.Emit(op.Stloc_1)
                        Case 2
                            Il.Emit(op.Stloc_2)
                        Case 3
                            Il.Emit(op.Stloc_3)
                        Case 0 To 255
                            Il.Emit(op.Stloc_S, Num)
                        Case Is > 255
                            Il.Emit(op.Stloc, Num)
                    End Select
#If IL_DEBUG Then
                    Debug.WriteLine("dup")
                    Debug.WriteLine($"stloc {num}")
#End If
                        'Static field
                Case (AscW("c"c) << 8 * 2) + (AscW("s"c) << 8) + AscW("f"c)
                    Il.Emit(op.Dup)
                    Il.Emit(op.Stsfld, Fields(Num))
#If IL_DEBUG Then
                    Debug.WriteLine("dup")
                    Debug.WriteLine($"stsfld {Fields(Num)}")
#End If
#End Region
                Case Else
                    Throw New Exception("Unknow key : " & reverser.to_string(Host.key))
            End Select
        End Sub

        <Extension, Method(inline)>
        Public Function fin(Of T As Class)(Host As ils(Of T)) As T
            If Host.be_collected Then Return collector(Of T).items(Host.key_collected)

            For Each Item In Host.code
                Host._process(AscW(Item), Host.key, Host.num, Host.stat, Host.word)
            Next

            Host._keygen
            Host.il.Emit(op.Ret)

#If IL_DEBUG Then
            Host.stack.rexit(Host.meth)
#End If

            fin = DirectCast(Host.meth.CreateDelegate(GetType(T)), Object)

            If Host.key_collected IsNot Nothing Then collector(Of T).items.Add(Host.key_collected, fin)
        End Function

        <Extension, Method(inline)>
        Private Sub _process(Of T As Class)(ByRef Host As ils(Of T), I As Byte,
                                            ByRef Key As int, ByRef Num As Int32,
                                            ByRef Stat As Stat,
                                            Word As Text.StringBuilder)
            Dim State = Stat
            Select Case I
                Case AscW(" "c)
                    If Key = 0 And Num = 0 And State <> Stat.Num Then Exit Sub
                    Host._keygen
                    Key = 0
                    Num = 0
                    Stat = Stat.Key
                    Word.Clear()
                Case AscW(vbCr)
                Case AscW(vbLf)
                Case AscW("$"c)
                    Select Case State
                        Case Stat.Word
                            Host._keygen
                            Word.Clear()
                        Case Else
                            Stat = Stat.Word
                    End Select
                Case AscW("."c)
                    Select Case State
                        Case Stat.Num
                            Host._keygen
                            Stat = Stat.Gen
                        Case Stat.Gen
                            Host._keygen
                        Case Else
                            Stat = Stat.Num
                    End Select
                Case AscW(":"c)
                    Key = (Key << 8) + I
                    Stat = Stat.Num
                Case Else
                    Select Case State
                        Case Stat.Key
                            Key = (Key << 8) + I
                        Case Stat.Word
                            Word.Append(I.as(Of Char))
                        Case Else
                            Select Case I
                                Case 48 To 57
                                    If State = Stat.Gen Then Num = 0 : Stat = Stat.Num
                                    Num = I - 48 + Num * 10
                                Case AscW(":"c)
                                    If Key = AscW(":"c) Then Key = (Key << 8) + Key
                                Case AscW("-"c)
                                    Num *= -1
                                Case Else 'la.1.2.sa == la.1 la.2 sa.2
                                    Stat = Stat.Key
                                    Key = I
                            End Select
                    End Select
            End Select
        End Sub
    End Module

#If IL_DEBUG Then
    Public Structure local
    End Structure

    Friend Module Exten_ils_debug
        <Extension, Method(inline)>
        Public Function jsk(Stack As Stack(Of Type)) As String
            With New Text.StringBuilder
                If Stack.Count > 0 Then
                    Do Until Stack.Count = 1
                        .Append($"{Stack.Pop}, ")
                    Loop
                    .Append($"{Stack.Pop}")
                End If
                Return .ToString
            End With
        End Function

        <Extension, Method(inline)>
        Public Sub rmin(Stack As Stack(Of Type), Min As Int32)
            If Stack.Count < Min Then
                Dim Message = $"Last operator request {Min} values on stack : {jsk(Stack)}"
                Debug.WriteLine(Message)
                Throw New Exception(Message)
            End If
        End Sub
        <Extension, Method(inline)>
        Public Sub rmax(Stack As Stack(Of Type), Max As Int32)
            If Stack.Count > Max Then
                Dim Message = $"Value exceed limit {Max} values on stack to operate last operator : {{ {jsk(Stack)} }}"
                Debug.WriteLine(Message)
                Throw New Exception(Message)
            End If
        End Sub

        <Extension, Method(inline)>
        Public Sub rexit(Stack As Stack(Of Type), Meth As sre.DynamicMethod)
            Debug.WriteLine("ret")
            If Meth.ReturnType Is GetType(Void) Then
                rmax(Stack, 0)
            Else
                rmax(Stack, 1)
                so(Stack)
            End If
        End Sub

        'Stack pUsh, pOp, pEek

        <Extension, Method(inline)>
        Public Sub so(Stack As Stack(Of Type))
            rmin(Stack, 1)
            Stack.Pop()
        End Sub
        <Extension, Method(inline)>
        Public Sub soo(Stack As Stack(Of Type))
            rmin(Stack, 2)
            Stack.Pop()
            Stack.Pop()
        End Sub
        <Extension, Method(inline)>
        Public Sub soxu(Stack As Stack(Of Type), Pop_count As Int32, Push_type As Type)
            rmin(Stack, Pop_count)
            For i = 1 To Pop_count
                Stack.Pop()
            Next
            If Push_type IsNot GetType(Void) Then Stack.Push(Push_type)
        End Sub
        <Extension, Method(inline)>
        Public Sub sox(Stack As Stack(Of Type), Pop_count As Int32)
            rmin(Stack, Pop_count)
            For i = 1 To Pop_count
                Stack.Pop()
            Next
        End Sub

        <Extension, Method(inline)>
        Public Sub su(Stack As Stack(Of Type), Push_type As Type)
            If Push_type IsNot GetType(Void) Then Stack.Push(Push_type)
        End Sub
        <Extension, Method(inline)>
        Public Sub su(Of ST)(Stack As Stack(Of Type))
            Stack.Push(GetType(ST))
        End Sub
        <Extension, Method(inline)>
        Public Sub su(Stack As Stack(Of Type))
            rmin(Stack, 1)
            Stack.Push(Stack.Peek)
        End Sub

        <Extension, Method(inline)>
        Public Sub sou(Stack As Stack(Of Type), Push_type As Type)
            rmin(Stack, 1)
            Stack.Pop()
            If Push_type IsNot GetType(Void) Then Stack.Push(Push_type)
        End Sub
        <Extension, Method(inline)>
        Public Sub sou(Of ST)(Stack As Stack(Of Type))
            rmin(Stack, 1)
            Stack.Pop()
            Stack.Push(GetType(ST))
        End Sub
        <Extension, Method(inline)>
        Public Sub sou(Stack As Stack(Of Type))
            rmin(Stack, 1)
            Stack.Push(Stack.Pop())
        End Sub
        <Extension, Method(inline)>
        Public Sub soou(Of ST)(Stack As Stack(Of Type))
            rmin(Stack, 2)
            Stack.Pop()
            Stack.Pop()
            Stack.Push(GetType(ST))
        End Sub
        <Extension, Method(inline)>
        Public Sub soou(Stack As Stack(Of Type), Push_type As Type)
            rmin(Stack, 2)
            Stack.Pop()
            Stack.Pop()
            If Push_type IsNot GetType(Void) Then Stack.Push(Push_type)
        End Sub
        <Extension, Method(inline)>
        Public Sub soou(Stack As Stack(Of Type))
            rmin(Stack, 2)
            Stack.Pop()
            Stack.Push(Stack.Pop)
        End Sub
    End Module
#End If

    Partial Public Module Exten_ils
        <Extension, Method(inline)>
        Public Function field(Of T As Class)(Host As ils(Of T), ParamArray Input() As sr.FieldInfo) As ils(Of T)
            Host.fields = Input
            Return Host
        End Function

        '''<summary>Can't use with express method.</summary>
        <Extension, Method(inline)>
        Public Function use(Of T As Class)(Host As ils(Of T), ParamArray Input() As sr.MethodInfo) As ils(Of T)
            Host.uses = Input
            Return Host
        End Function
        <Extension, Method(inline)>
        Public Function [new](Of T As Class)(Host As ils(Of T), ParamArray Input() As sr.ConstructorInfo) As ils(Of T)
            Host.news = Input
            Return Host
        End Function
        <Extension, Method(inline)>
        Public Function txt(Of T As Class)(Host As ils(Of T), ParamArray Input() As String) As ils(Of T)
            Host.txts = Input
            Return Host
        End Function
        <Extension, Method(inline)>
        Public Function type(Of T As Class)(Host As ils(Of T), ParamArray Input() As System.Type) As ils(Of T)
            Host.types = Input
            Return Host
        End Function
        Public Function i4(Of T As Class)(Host As ils(Of T), ParamArray Input() As Integer) As ils(Of T)
            Host.i4s = Input
            Return Host
        End Function
        <Extension, Method(inline)>
        Public Function i8(Of T As Class)(Host As ils(Of T), ParamArray Input() As Long) As ils(Of T)
            Host.i8s = Input
            Return Host
        End Function
        <Extension, Method(inline)>
        Public Function r4(Of T As Class)(Host As ils(Of T), ParamArray Input() As Single) As ils(Of T)
            Host.r4s = Input
            Return Host
        End Function
        <Extension, Method(inline)>
        Public Function r8(Of T As Class)(Host As ils(Of T), ParamArray Input() As Double) As ils(Of T)
            Host.r8s = Input
            Return Host
        End Function
        <Extension, Method(inline)>
        Public Function def(Of T As Class)(Host As ils(Of T), ParamArray Input() As Type) As ils(Of T)
            For Each Item In Input
                Host.il.DeclareLocal(Item)
            Next
            Return Host
        End Function
    End Module
End Namespace