Imports lbl = System.Collections.Generic.Dictionary(Of Integer, System.Reflection.Emit.Label)
Imports cil = System.Reflection.Emit.ILGenerator

Namespace ILS

    Public Module Builder
        <Extension, Method(inline)>
        Public Function compile(Of T As Class)(Script As String) As ils(Of T)
            Return New ils(Of T)(Script, Info.delegate(Of T).create_method)
        End Function
        <Extension, Method(inline)>
        Public Function compile(Of T As Class)(Script As String, Assembly_module As sr.Module) As ils(Of T)
            Return New ils(Of T)(Script, Info.delegate(Of T).create_method(Assembly_module))
        End Function
        <Extension, Method(inline)>
        Public Function compile(Of T As Class, Base As Class)(Script As String) As ils(Of T)
            Return New ils(Of T)(Script, Info.delegate(Of T).create_method(GetType(Base)))
        End Function
    End Module

    Public Structure ils(Of T As Class)
        Friend meth As sre.DynamicMethod
        Friend il As cil

        Private lbls As lbl
        Private news() As sr.ConstructorInfo
        Private uses() As sr.MethodInfo
        Private useds() As sr.MethodInfo
        Private fields() As sr.FieldInfo
        Private txts() As String
        Private types() As System.Type

        Private i4s() As Int32
        Private i8s() As Int64
        Private r4s() As Single
        Private r8s() As Double

        Private is_input, is_input_gen As Boolean
        Private key As Int64, num As Integer

        Public code As String

        <Method(inline)>
        Public Sub New(Code As String, Method As sre.DynamicMethod)
            meth = Method
            il = Method.GetILGenerator
            Me.code = Code
            lbls = New lbl
        End Sub

        <Method(inline)>
        Public Function fin() As T
            reading(0, code.Length, code.ToCharArray)
            keygen()
            il.Emit(op.Ret)
            Return DirectCast(meth.CreateDelegate(GetType(T)), Object)
        End Function

        <Method(inline)>
        Private Sub reading(I As int, E As int, Code() As Char)
            While I < E
                process(AscW(Code(I)))
                I += 1
            End While
        End Sub

        <Method(inline)>
        Private Sub process(I As Byte)
            Select Case I
                Case AscW("."c)
                    If is_input Then keygen() : is_input_gen = True  'num = 0
                    is_input = True
                Case AscW(":"c)
                    key = (key << 8) + I
                    is_input = True
                Case AscW(" "c)
                    keygen()
                    key = 0
                    num = 0
                    is_input = False
                    is_input_gen = False
                Case Else
                    If is_input Then numbering(I) Else key = (key << 8) + I 'key <<= 8 : key += I
            End Select
        End Sub

        <Method(inline)>
        Private Sub numbering(Input As Byte)
            If Input > 47 And Input < 58 Then
                If is_input_gen Then num = 0 : is_input_gen = False
                num = Input - 48 + num * 10
            ElseIf key = AscW(":"c) And Input = AscW(":"c) Then ' :: == goto target label
                key = (key << 8) + key
            Else 'la.1.2.sa == la.1 la.2 sa.2
                is_input = False
                key = Input
            End If
        End Sub

        '(((AscW("["c) << 8) + AscW("-"c)) << 8) + AscW("]"c) 'Easy for compiler
        ' [TIP]:: start with '(CLng(AscW("u"c)) << 8)' for <= 4 chars or '(CLng(AscW("u"c)) << 8)' for > 4 but < 9 
        '         paste '+ AscW("-"c)) << 8)' then juse add '(' at start until no error found.
        ' (((((((((CLng(AscW("u"c)) << 8) + AscW("s"c)) << 8) + AscW("e"c)) << 8) + AscW("-"c)) << 8) + AscW("m"c)) << 8) + AscW("e"c)

        '(AscW("["c) << 8 * 2) + (AscW("-"c) << 8 * 1) + (AscW("]"c) << 8 * 0) ' Easy for macro
        '(AscW("["c) << 8 * 2) + (AscW("-"c) << 8) + AscW("]"c) 'Easy for Human
        'Public Const SSD = (AscW("["c) << 8 * 2) + (AscW("-"c) << 8) + AscW("]"c)

        'better compare chars value sum before compare each position of chars.
        Private Sub keygen()
            With il
                Select Case key
                    Case 0
                        If is_input Then ldc()
                    Case AscW(";"c)
                        .Emit(op.Ret)
                    Case (AscW("!"c) << 8) + AscW("!"c)
                        .Emit(op.Break)
                    Case (AscW("["c) << 8 * 2) + (AscW("-"c) << 8) + AscW("]"c)
                        .Emit(op.Pop)
                    Case (AscW("["c) << 8 * 2) + (AscW("+"c) << 8) + AscW("]"c)
                        .Emit(op.Dup)

                        'Must not has anything left in stack except uint of new alloc size.
                    Case (AscW("["c) << 8 * 2) + (AscW("n"c) << 8) + AscW("]"c)
                        .Emit(op.Localloc)
#Region "Math"
                    Case AscW("+"c)
                        .Emit(op.Add)
                    Case AscW("-"c)
                        .Emit(op.Sub)
                    Case AscW("*"c)
                        .Emit(op.Mul)
                    Case AscW("/"c)
                        .Emit(op.Div)
                    Case AscW("%"c)
                        .Emit(op.[Rem])'mod

                    Case (AscW("-"c) << 8) + AscW("1"c)
                        .Emit(op.Ldc_I4_M1)

                    Case (AscW("+"c) << 8) + AscW("+"c)
                        .Emit(op.Ldc_I4_1)
                        .Emit(op.Add)
                    Case (AscW("-"c) << 8) + AscW("-"c)
                        .Emit(op.Ldc_I4_1)
                        .Emit(op.Sub)
                    Case (AscW("*"c) << 8) + AscW("*"c)
                        .Emit(op.Dup)
                        .Emit(op.Mul)
                    Case (AscW("-"c) << 8) + AscW("*"c)
                        .Emit(op.Ldc_I4_M1)
                        .Emit(op.Mul)
                    Case (AscW("-"c) << 8) + AscW("/"c)
                        .Emit(op.Ldc_I4_M1)
                        .Emit(op.Div)

                    Case AscW("&"c)
                        .Emit(op.And)
                    Case AscW("|"c)
                        .Emit(op.Or)
                    Case (AscW("&"c) << 8) + AscW("|"c)
                        .Emit(op.Xor)
                    Case AscW("!"c)
                        .Emit(op.Not)

                    Case AscW(">"c)
                        .Emit(op.Cgt)
                    Case AscW("<"c)
                        .Emit(op.Clt)
                    Case AscW("="c)
                        .Emit(op.Ceq)

                    Case (AscW("<"c) << 8) + AscW("<"c)
                        .Emit(op.Shl)
                    Case (AscW(">"c) << 8) + AscW(">"c)
                        .Emit(op.Shl)
#End Region

#Region "Branch"
                    Case AscW(":"c)
                        .MarkLabel(lbl(num))
                    Case (AscW(":"c) << 8) + AscW(":"c)
                        .Emit(op.Br, lbl(num))
                    Case (AscW("t"c) << 8) + AscW(":"c)
                        .Emit(op.Brtrue, lbl(num))
                    Case (AscW("f"c) << 8) + AscW(":"c)
                        .Emit(op.Brfalse, lbl(num))

                    Case (AscW("="c) << 8) + AscW(":"c)
                        .Emit(op.Beq, lbl(num))
                    Case (AscW(">"c) << 8) + AscW(":"c)
                        .Emit(op.Bgt, lbl(num))
                    Case (AscW("<"c) << 8) + AscW(":"c)
                        .Emit(op.Blt, lbl(num))
                    Case (AscW(">"c) << 8 * 2) + (AscW("="c) << 8) + AscW(":"c)
                        .Emit(op.Bge, lbl(num))
                    Case (AscW("<"c) << 8 * 2) + (AscW("="c) << 8) + AscW(":"c)
                        .Emit(op.Ble, lbl(num))

                    Case (AscW("!"c) << 8 * 2) + (AscW("="c) << 8) + AscW(":"c)
                        .Emit(op.Bne_Un, lbl(num))
                    Case (AscW("u"c) << 8 * 2) + (AscW(">"c) << 8) + AscW(":"c)
                        .Emit(op.Bgt_Un, lbl(num))
                    Case (AscW("u"c) << 8 * 2) + (AscW("<"c) << 8) + AscW(":"c)
                        .Emit(op.Blt_Un, lbl(num))
                    Case (AscW("u"c) << 8 * 3) + (AscW(">"c) << 8 * 2) + (AscW("="c) << 8) + AscW(":"c)
                        .Emit(op.Bge_Un, lbl(num))
                    Case (AscW("u"c) << 8 * 3) + (AscW("<"c) << 8 * 2) + (AscW("="c) << 8) + AscW(":"c)
                        .Emit(op.Ble_Un, lbl(num))
#End Region

                        'Invoke method
                        '[re] with [use] for recursion method
                    Case (AscW("r"c) << 8) + AscW("e"c)
                        .Emit(op.Tailcall)
                    Case (AscW("u"c) << 8 * 2) + (AscW("s"c) << 8) + AscW("e"c)
                        .Emit(op.Call, uses(num))
                    Case (AscW("u"c) << 8 * 3) + (AscW("s"c) << 8 * 2) + (AscW("e"c) << 8) + AscW("d"c)
                        .Emit(op.Call, useds(num))
                        '[use-me]
                    Case (CLng(AscW("u"c)) << 8 * 5) + (CLng(AscW("s"c)) << 8 * 4) + (AscW("e"c) << 8 * 3) + (AscW("-"c) << 8 * 2) + (AscW("m"c) << 8) + AscW("e"c)
                        .Emit(op.Call, meth)
                    Case (AscW("j"c) << 8 * 2) + (AscW("m"c) << 8) + AscW("p"c)
                        .Emit(op.Jmp, uses(num))
                        '[jmp-me]
                    Case (CLng(AscW("j"c)) << 8 * 5) + (CLng(AscW("m"c)) << 8 * 4) + (AscW("p"c) << 8 * 3) + (AscW("-"c) << 8 * 2) + (AscW("m"c) << 8) + AscW("e"c)
                        .Emit(op.Jmp, meth)

                        'Load constant data
                    Case (AscW("t"c) << 8 * 2) + (AscW("x"c) << 8) + AscW("t"c)
                        .Emit(op.Ldstr, txts(num))
                    Case (AscW("i"c) << 8) + AscW("4"c)
                        .Emit(op.Ldc_I4, i4s(num))
                    Case (AscW("i"c) << 8) + AscW("8"c)
                        .Emit(op.Ldc_I8, i8s(num))
                    Case (AscW("r"c) << 8) + AscW("4"c)
                        .Emit(op.Ldc_R4, r4s(num))
                    Case (AscW("r"c) << 8) + AscW("8"c)
                        .Emit(op.Ldc_R8, r8s(num))

                        'Get size of data
                    Case (AscW("m"c) << 8) + AscW("t"c)
                        .Emit(op.Sizeof, types(num))

                        'New object and array
                    Case (AscW("n"c) << 8 * 2) + (AscW("a"c) << 8) + AscW("t"c)
                        .Emit(op.Newarr, types(num))
                    Case (AscW("n"c) << 8 * 2) + (AscW("e"c) << 8) + AscW("w"c)
                        .Emit(op.Newobj, news(num))
                    Case (AscW("a"c) << 8 * 3) + (AscW("n"c) << 8 * 2) + (AscW("e"c) << 8) + AscW("w"c)
                        .Emit(op.Call, news(num))

                        'Memory manage
                    Case (AscW("c"c) << 8 * 3) + (AscW("p"c) << 8 * 2) + (AscW("y"c) << 8) + AscW("t"c)
                        .Emit(op.Cpobj, types(num))
                    Case (AscW("i"c) << 8 * 3) + (AscW("n"c) << 8 * 2) + (AscW("i"c) << 8) + AscW("t"c)
                        .Emit(op.Initobj, types(num))
                    Case (AscW("c"c) << 8 * 3) + (AscW("p"c) << 8 * 2) + (AscW("y"c) << 8) + AscW("b"c)
                        .Emit(op.Cpblk)
                    Case (AscW("i"c) << 8 * 3) + (AscW("n"c) << 8 * 2) + (AscW("i"c) << 8) + AscW("b"c)
                        .Emit(op.Initblk)

                        'Argument
                    Case (AscW("l"c) << 8) + AscW("a"c)
                        ldarg()
                    Case (AscW("s"c) << 8) + AscW("a"c)
                        .Emit(op.Starg_S, num)
                    Case (AscW("l"c) << 8 * 2) + (AscW("a"c) << 8) + AscW("a"c)
                        .Emit(op.Ldarga_S, num)

                        'Local variant
                    Case (AscW("l"c) << 8) + AscW("o"c)
                        ldloc()
                    Case (AscW("s"c) << 8) + AscW("o"c)
                        stloc()
                    Case (AscW("l"c) << 8 * 2) + (AscW("a"c) << 8) + AscW("o"c)
                        .Emit(op.Ldloca_S, num)

                        'Field
                    Case (AscW("l"c) << 8) + AscW("f"c)
                        .Emit(op.Ldfld, fields(num))
                    Case (AscW("s"c) << 8) + AscW("f"c)
                        .Emit(op.Stfld, fields(num))
                    Case (AscW("l"c) << 8 * 2) + (AscW("a"c) << 8) + AscW("f"c)
                        .Emit(op.Ldflda, fields(num))

                        'Static field
                    Case (AscW("l"c) << 8 * 2) + (AscW("s"c) << 8) + AscW("f"c)
                        .Emit(op.Ldsfld, fields(num))
                    Case (AscW("s"c) << 8 * 2) + (AscW("s"c) << 8) + AscW("f"c)
                        .Emit(op.Stsfld, fields(num))
                    Case (AscW("l"c) << 8 * 3) + (AscW("a"c) << 8 * 2) + (AscW("s"c) << 8) + AscW("f"c)
                        .Emit(op.Ldsflda, fields(num))

                        'Array element
                    Case (AscW("l"c) << 8 * 2) + (AscW("e"c) << 8) + AscW("n"c)
                        .Emit(op.Ldlen)
                    Case (AscW("l"c) << 8 * 3) + (AscW("a"c) << 8 * 2) + (AscW("e"c) << 8) + AscW("t"c)
                        .Emit(op.Ldelema, types(num))
                    Case (AscW("l"c) << 8 * 2) + (AscW("e"c) << 8) + AscW("t"c)
                        .Emit(op.Ldelem, types(num))
                    Case (AscW("s"c) << 8 * 2) + (AscW("e"c) << 8) + AscW("t"c)
                        .Emit(op.Stelem, types(num))

                        'Object type
                    Case (AscW("l"c) << 8 * 2) + (AscW("o"c) << 8) + AscW("t"c)
                        .Emit(op.Ldobj, types(num))
                    Case (AscW("s"c) << 8 * 2) + (AscW("o"c) << 8) + AscW("t"c)
                        .Emit(op.Stobj, types(num))
                    Case (AscW("c"c) << 8 * 2) + (AscW("o"c) << 8) + AscW("t"c)
                        .Emit(op.Castclass, types(num))

#Region "Indirect"
                    Case (AscW("l"c) << 8) + AscW("i"c)
                        .Emit(op.Ldind_I)
                    Case (AscW("l"c) << 8 * 2) + (AscW("i"c) << 8) + AscW("1"c)
                        .Emit(op.Ldind_I1)
                    Case (AscW("l"c) << 8 * 2) + (AscW("i"c) << 8) + AscW("2"c)
                        .Emit(op.Ldind_I2)
                    Case (AscW("l"c) << 8 * 2) + (AscW("i"c) << 8) + AscW("4"c)
                        .Emit(op.Ldind_I4)
                    Case (AscW("l"c) << 8 * 2) + (AscW("i"c) << 8) + AscW("8"c)
                        .Emit(op.Ldind_I8)
                    Case (AscW("l"c) << 8 * 2) + (AscW("r"c) << 8) + AscW("4"c)
                        .Emit(op.Ldind_R4)
                    Case (AscW("l"c) << 8 * 2) + (AscW("r"c) << 8) + AscW("8"c)
                        .Emit(op.Ldind_R8)
                    Case (AscW("l"c) << 8) + AscW("u"c)
                        .Emit(op.Ldind_Ref)
                    Case (AscW("l"c) << 8 * 2) + (AscW("u"c) << 8) + AscW("1"c)
                        .Emit(op.Ldind_U1)
                    Case (AscW("l"c) << 8 * 2) + (AscW("u"c) << 8) + AscW("2"c)
                        .Emit(op.Ldind_U2)
                    Case (AscW("l"c) << 8 * 2) + (AscW("u"c) << 8) + AscW("4"c)
                        .Emit(op.Ldind_U4)

                    Case (AscW("s"c) << 8) + AscW("i"c)
                        .Emit(op.Stind_I)
                    Case (AscW("s"c) << 8 * 2) + (AscW("i"c) << 8) + AscW("1"c)
                        .Emit(op.Stind_I1)
                    Case (AscW("s"c) << 8 * 2) + (AscW("i"c) << 8) + AscW("2"c)
                        .Emit(op.Stind_I2)
                    Case (AscW("s"c) << 8 * 2) + (AscW("i"c) << 8) + AscW("4"c)
                        .Emit(op.Stind_I4)
                    Case (AscW("s"c) << 8 * 2) + (AscW("i"c) << 8) + AscW("8"c)
                        .Emit(op.Stind_I8)
                    Case (AscW("s"c) << 8 * 2) + (AscW("r"c) << 8) + AscW("4"c)
                        .Emit(op.Stind_R4)
                    Case (AscW("s"c) << 8 * 2) + (AscW("r"c) << 8) + AscW("8"c)
                        .Emit(op.Stind_R8)
                    Case (AscW("s"c) << 8) + AscW("u"c)
                        .Emit(op.Stind_Ref)
#End Region

#Region "Convert"
                    Case (AscW("c"c) << 8) + AscW("i"c)
                        .Emit(op.Conv_I)
                    Case (AscW("c"c) << 8 * 2) + (AscW("i"c) << 8) + AscW("1"c)
                        .Emit(op.Conv_I1)
                    Case (AscW("c"c) << 8 * 2) + (AscW("i"c) << 8) + AscW("2"c)
                        .Emit(op.Conv_I2)
                    Case (AscW("c"c) << 8 * 2) + (AscW("i"c) << 8) + AscW("4"c)
                        .Emit(op.Conv_I4)
                    Case (AscW("c"c) << 8 * 2) + (AscW("i"c) << 8) + AscW("8"c)
                        .Emit(op.Conv_I8)

                    Case (AscW("c"c) << 8) + AscW("u"c)
                        .Emit(op.Conv_U)
                    Case (AscW("c"c) << 8 * 2) + (AscW("u"c) << 8) + AscW("1"c)
                        .Emit(op.Conv_U1)
                    Case (AscW("c"c) << 8 * 2) + (AscW("u"c) << 8) + AscW("2"c)
                        .Emit(op.Conv_U2)
                    Case (AscW("c"c) << 8 * 2) + (AscW("u"c) << 8) + AscW("4"c)
                        .Emit(op.Conv_U4)
                    Case (AscW("c"c) << 8 * 2) + (AscW("u"c) << 8) + AscW("8"c)
                        .Emit(op.Conv_U8)

                    Case (AscW("c"c) << 8 * 2) + (AscW("r"c) << 8) + AscW("4"c)
                        .Emit(op.Conv_R4)
                    Case (AscW("c"c) << 8 * 2) + (AscW("r"c) << 8) + AscW("8"c)
                        .Emit(op.Conv_R8)
#End Region
                    Case Else
                        Throw New Exception("Unknow key : " & key)
                End Select
            End With
        End Sub

        <Method(inline)>
        Private Function lbl(Input As Int32) As sre.Label
            If Not lbls.ContainsKey(Input) Then lbls.Add(Input, il.DefineLabel)
            Return lbls(Input)
        End Function

        <Method(inline)>
        Private Sub stloc()
            Select Case num
                Case 0
                    il.Emit(op.Stloc_0)
                Case 1
                    il.Emit(op.Stloc_1)
                Case 2
                    il.Emit(op.Stloc_2)
                Case 3
                    il.Emit(op.Stloc_3)
                Case Else
                    il.Emit(op.Stloc_S, num)
            End Select
        End Sub

        <Method(inline)>
        Private Sub ldloc()
            Select Case num
                Case 0
                    il.Emit(op.Ldloc_0)
                Case 1
                    il.Emit(op.Ldloc_1)
                Case 2
                    il.Emit(op.Ldloc_2)
                Case 3
                    il.Emit(op.Ldloc_3)
                Case Else
                    il.Emit(op.Ldloc_S, num)
            End Select
        End Sub

        <Method(inline)>
        Private Sub ldarg()
            Select Case num
                Case 0
                    il.Emit(op.Ldarg_0)
                Case 1
                    il.Emit(op.Ldarg_1)
                Case 2
                    il.Emit(op.Ldarg_2)
                Case 3
                    il.Emit(op.Ldarg_3)
                Case Else
                    il.Emit(op.Ldarg_S, num)
            End Select
        End Sub

        <Method(inline)>
        Private Sub ldc()
            Select Case num
                Case 0
                    il.Emit(op.Ldc_I4_0)
                Case 1
                    il.Emit(op.Ldc_I4_1)
                Case 2
                    il.Emit(op.Ldc_I4_2)
                Case 3
                    il.Emit(op.Ldc_I4_3)
                Case 4
                    il.Emit(op.Ldc_I4_4)
                Case 5
                    il.Emit(op.Ldc_I4_5)
                Case 6
                    il.Emit(op.Ldc_I4_6)
                Case 7
                    il.Emit(op.Ldc_I4_7)
                Case 8
                    il.Emit(op.Ldc_I4_8)
                Case Else
                    If num < 256 Then il.Emit(op.Ldc_I4_S, CByte(num)) Else il.Emit(op.Ldc_I4, num)
            End Select
        End Sub
        <Method(inline)>
        Public Function field(ParamArray Input() As sr.FieldInfo) As ils(Of T)
            fields = Input
            Return Me
        End Function

        '''<summary>Can't use with express method.</summary>
        <Method(inline)>
        Public Function use(ParamArray Input() As sr.MethodInfo) As ils(Of T)
            uses = Input
            Return Me
        End Function
        <Method(inline)>
        Public Function [new](ParamArray Input() As sr.ConstructorInfo) As ils(Of T)
            news = Input
            Return Me
        End Function
        <Method(inline)>
        Public Function txt(ParamArray Input() As String) As ils(Of T)
            txts = Input
            Return Me
        End Function
        <Method(inline)>
        Public Function type(ParamArray Input() As System.Type) As ils(Of T)
            types = Input
            Return Me
        End Function
        Public Function i4(ParamArray Input() As Integer) As ils(Of T)
            i4s = Input
            Return Me
        End Function
        <Method(inline)>
        Public Function i8(ParamArray Input() As Long) As ils(Of T)
            i8s = Input
            Return Me
        End Function
        <Method(inline)>
        Public Function r4(ParamArray Input() As Single) As ils(Of T)
            r4s = Input
            Return Me
        End Function
        <Method(inline)>
        Public Function r8(ParamArray Input() As Double) As ils(Of T)
            r8s = Input
            Return Me
        End Function

        <Method(inline)>
        Private Sub defining(I As int, E As int, Input() As Type)
            While I < E
                il.DeclareLocal(Input(I))
                I += 1
            End While
        End Sub
        <Method(inline)>
        Public Function def(ParamArray Input() As Type) As ils(Of T)
            defining(0, Input.Length, Input)
            Return Me
        End Function

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
        Public Function type(Of T2, T1)() As ils(Of T)
            types = Info.type(Of T2, T1).types
            Return Me
        End Function

        <Method(inline)>
        Public Function type(Of T3, T2, T1)() As ils(Of T)
            types = Info.type(Of T3, T2, T1).types
            Return Me
        End Function

        <Method(inline)>
        Public Function type(Of T4, T3, T2, T1)() As ils(Of T)
            types = Info.type(Of T4, T3, T2, T1).types
            Return Me
        End Function

        <Method(inline)>
        Public Function type(Of T5, T4, T3, T2, T1)() As ils(Of T)
            types = Info.type(Of T5, T4, T3, T2, T1).types
            Return Me
        End Function

        <Method(inline)>
        Public Function type(Of T6, T5, T4, T3, T2, T1)() As ils(Of T)
            types = Info.type(Of T6, T5, T4, T3, T2, T1).types
            Return Me
        End Function

        <Method(inline)>
        Public Function type(Of T7, T6, T5, T4, T3, T2, T1)() As ils(Of T)
            types = Info.type(Of T7, T6, T5, T4, T3, T2, T1).types
            Return Me
        End Function

        <Method(inline)>
        Public Function type(Of T8, T7, T6, T5, T4, T3, T2, T1)() As ils(Of T)
            types = Info.type(Of T8, T7, T6, T5, T4, T3, T2, T1).types
            Return Me
        End Function
#End Region

#Region "delegates"
        <Method(inline)>
        Public Function used(Of T1 As Class)() As ils(Of T)
            useds = {Info.delegate(Of T1).invoker}
            Return Me
        End Function
        <Method(inline)>
        Public Function used(Of T1 As Class, T2 As Class)() As ils(Of T)
            useds = {Info.delegate(Of T1).invoker, Info.delegate(Of T2).invoker}
            Return Me
        End Function

        <Method(inline)>
        Public Function used(Of T3 As Class, T2 As Class, T1 As Class)() As ils(Of T)
            useds = {Info.delegate(Of T3).invoker, Info.delegate(Of T2).invoker, Info.delegate(Of T1).invoker}
            Return Me
        End Function

        <Method(inline)>
        Public Function used(Of T4 As Class, T3 As Class, T2 As Class, T1 As Class)() As ils(Of T)
            useds = {Info.delegate(Of T4).invoker, Info.delegate(Of T3).invoker, Info.delegate(Of T2).invoker, Info.delegate(Of T1).invoker}
            Return Me
        End Function

        <Method(inline)>
        Public Function used(Of T5 As Class, T4 As Class, T3 As Class, T2 As Class, T1 As Class)() As ils(Of T)
            useds = {Info.delegate(Of T5).invoker, Info.delegate(Of T4).invoker, Info.delegate(Of T3).invoker, Info.delegate(Of T2).invoker, Info.delegate(Of T1).invoker}
            Return Me
        End Function

        <Method(inline)>
        Public Function used(Of T6 As Class, T5 As Class, T4 As Class, T3 As Class, T2 As Class, T1 As Class)() As ils(Of T)
            useds = {Info.delegate(Of T6).invoker, Info.delegate(Of T5).invoker, Info.delegate(Of T4).invoker, Info.delegate(Of T3).invoker, Info.delegate(Of T2).invoker, Info.delegate(Of T1).invoker}
            Return Me
        End Function

        <Method(inline)>
        Public Function used(Of T7 As Class, T6 As Class, T5 As Class, T4 As Class, T3 As Class, T2 As Class, T1 As Class)() As ils(Of T)
            useds = {Info.delegate(Of T7).invoker, Info.delegate(Of T6).invoker, Info.delegate(Of T5).invoker, Info.delegate(Of T4).invoker, Info.delegate(Of T3).invoker, Info.delegate(Of T2).invoker, Info.delegate(Of T1).invoker}
            Return Me
        End Function

        <Method(inline)>
        Public Function used(Of T8 As Class, T7 As Class, T6 As Class, T5 As Class, T4 As Class, T3 As Class, T2 As Class, T1 As Class)() As ils(Of T)
            useds = {Info.delegate(Of T8).invoker, Info.delegate(Of T7).invoker, Info.delegate(Of T6).invoker, Info.delegate(Of T5).invoker, Info.delegate(Of T4).invoker, Info.delegate(Of T3).invoker, Info.delegate(Of T2).invoker, Info.delegate(Of T1).invoker}
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