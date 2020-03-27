Imports lbl = System.Collections.Generic.Dictionary(Of Integer, System.Reflection.Emit.Label)
Imports cil = System.Reflection.Emit.ILGenerator

Namespace ILS

    Public Module Builder
        '<Extension, Method(inline)>
        'Public Function [do](Of V, S)(Variant_data As V, Static_data As S,
        '                              Until As Func(Of V, S, Boolean),
        '                              [Loop] As Func(Of V, S, V)) As V
        '    Return pre(Of V, S)._do(Variant_data, Static_data, Until, [Loop])
        'End Function
        '<Extension, Method(inline)>
        'Public Function [do](Of T)(Data As T, Until As Func(Of T, Boolean), [Loop] As Func(Of T, T)) As T
        '    Return pre(Of T)._do(Data, Until, [Loop])
        'End Function


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

    Friend Structure collector(Of T As Class)
        Friend Shared ReadOnly items As New System.Collections.Generic.Dictionary(Of String, T)
    End Structure

    '''<typeparam name="T">T is type of delegate.</typeparam>
    Public Structure ils(Of T As Class)
        Friend meth As sre.DynamicMethod
        Friend il As cil

        Private ReadOnly lbls As lbl
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

        Private is_input, is_input_gen, is_input_word As Boolean
        Private key As Int64, num As Integer
        Private ReadOnly word As Text.StringBuilder

        Public code As String

        '==========================
        ' Implement reuse system
        '==========================
        'Friend Shared ReadOnly collector As New System.Collections.Generic.Dictionary(Of String, Object)
        Private ReadOnly be_collected As Boolean
        Private ReadOnly key_collected As String
        '==========================

        <Method(inline)>
        Friend Sub New(Code As String)
            Me.New(Code, Info.delegate(Of T).create_method, Code)

            'Old version with type name in key, replace with generic type selection.
            'Me.New(Code, Info.delegate(Of T).create_method, Code & GetType(T).FullName)
        End Sub
        <Method(inline)>
        Public Sub New(Code As String, Method As sre.DynamicMethod)
            Me.New(Code, Method, Nothing)
        End Sub
        <Method(inline)>
        Friend Sub New(Code As String, Collect_key As String)
            Me.New(Code, Info.delegate(Of T).create_method, Collect_key)
        End Sub


#Const IL_DEBUG = False
        <Method(inline)>
        Friend Sub New(Code As String, Method As sre.DynamicMethod, Collect_key As String)
#If IL_DEBUG Then
            Debug.WriteLine("===========================")
            Debug.WriteLine(Method.ToString)
            Debug.WriteLine(Code)
            Debug.WriteLine("===========================")
#End If

            key_collected = Collect_key
            If key_collected IsNot Nothing Then be_collected = collector(Of T).items.ContainsKey(key_collected)

            If Not be_collected Then
                meth = Method
                il = Method.GetILGenerator
                Me.code = Code
                lbls = New lbl
                word = New Text.StringBuilder
                useds = Info.delegate(Of T).invoke_param
            End If
        End Sub

        <Method(inline)>
        Public Function fin() As T
            If be_collected Then Return collector(Of T).items(key_collected)

            reading(0, code.Length, code.ToCharArray)
            keygen()
            il.Emit(op.Ret)
            fin = DirectCast(meth.CreateDelegate(GetType(T)), Object)

            If key_collected IsNot Nothing Then collector(Of T).items.Add(key_collected, fin)
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
                Case AscW("$"c)
                    If is_input_word Then
                        keygen()
                        word.Clear()
                    Else
                        is_input = False
                        is_input_gen = False
                        is_input_word = True
                    End If
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
                    is_input_word = False
                    word.Clear()
                Case Else
                    Select Case True
                        Case is_input
                            numbering(I)
                        Case is_input_word
                            wording(I)
                        Case Else
                            key = (key << 8) + I
                    End Select
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

        <Method(inline)>
        Private Sub wording(Input As Byte)
            word.Append(ChrW(Input))
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
                        Select Case True
                            Case is_input
                                ldc()
#If IL_DEBUG Then
                                Debug.WriteLine($"ldc.i4 {num}")
#End If
                            Case is_input_word
                                .Emit(op.Ldstr, word.ToString)
#If IL_DEBUG Then
                                Debug.WriteLine($"ldstr {word}")
#End If
                        End Select
                    Case AscW(";"c)
                        .Emit(op.Ret)
#If IL_DEBUG Then
                        Debug.WriteLine("ret")
#End If
                    Case (AscW("!"c) << 8) + AscW("!"c)
                        .Emit(op.Break)
#If IL_DEBUG Then
                        Debug.WriteLine("break")
#End If
                    Case (AscW("["c) << 8 * 2) + (AscW("-"c) << 8) + AscW("]"c)
                        .Emit(op.Pop)
#If IL_DEBUG Then
                        Debug.WriteLine("pop")
#End If
                    Case (AscW("["c) << 8 * 2) + (AscW("+"c) << 8) + AscW("]"c)
                        .Emit(op.Dup)
#If IL_DEBUG Then
                        Debug.WriteLine("dup")
#End If
                        'Must not has anything left in stack except uint of new alloc size.
                    Case (AscW("["c) << 8 * 2) + (AscW("n"c) << 8) + AscW("]"c)
                        .Emit(op.Localloc)
#If IL_DEBUG Then
                        Debug.WriteLine("alloc")
#End If

#Region "Math"
                    Case AscW("+"c)
                        .Emit(op.Add)
#If IL_DEBUG Then
                        Debug.WriteLine("add")
#End If
                    Case AscW("-"c)
                        .Emit(op.Sub)
#If IL_DEBUG Then
                        Debug.WriteLine("sub")
#End If
                    Case AscW("*"c)
                        .Emit(op.Mul)
#If IL_DEBUG Then
                        Debug.WriteLine("mul")
#End If
                    Case AscW("/"c)
                        .Emit(op.Div)
#If IL_DEBUG Then
                        Debug.WriteLine("div")
#End If
                    Case AscW("%"c)
                        .Emit(op.[Rem]) 'mod
#If IL_DEBUG Then
                        Debug.WriteLine("rem")
#End If

                    Case (AscW("-"c) << 8) + AscW("1"c)
                        .Emit(op.Ldc_I4_M1)
#If IL_DEBUG Then
                        Debug.WriteLine("ldc.i4.m1")
#End If

                    Case (AscW("+"c) << 8) + AscW("+"c)
                        .Emit(op.Ldc_I4_1)
                        .Emit(op.Add)
#If IL_DEBUG Then
                        Debug.WriteLine($"ldc.i4.1")
                        Debug.WriteLine("add")
#End If
                    Case (AscW("-"c) << 8) + AscW("-"c)
                        .Emit(op.Ldc_I4_1)
                        .Emit(op.Sub)
#If IL_DEBUG Then
                        Debug.WriteLine("ldc.i4.1")
                        Debug.WriteLine("sub")
#End If
                    Case (AscW("*"c) << 8) + AscW("*"c)
                        .Emit(op.Dup)
                        .Emit(op.Mul)
#If IL_DEBUG Then
                        Debug.WriteLine("dup")
                        Debug.WriteLine("mul")
#End If
                    Case (AscW("-"c) << 8) + AscW("*"c)
                        .Emit(op.Ldc_I4_M1)
                        .Emit(op.Mul)
#If IL_DEBUG Then
                        Debug.WriteLine("ldc.i4.m1")
                        Debug.WriteLine("mul")
#End If
                    Case (AscW("-"c) << 8) + AscW("/"c)
                        .Emit(op.Ldc_I4_M1)
                        .Emit(op.Div)
#If IL_DEBUG Then
                        Debug.WriteLine("ldc.i4.m1")
                        Debug.WriteLine("div")
#End If

                    Case AscW("&"c)
                        .Emit(op.And)
#If IL_DEBUG Then
                        Debug.WriteLine("and")
#End If
                    Case AscW("|"c)
                        .Emit(op.Or)
#If IL_DEBUG Then
                        Debug.WriteLine("or")
#End If
                    Case (AscW("&"c) << 8) + AscW("|"c)
                        .Emit(op.Xor)
#If IL_DEBUG Then
                        Debug.WriteLine("xor")
#End If
                    Case AscW("!"c)
                        .Emit(op.Not)
#If IL_DEBUG Then
                        Debug.WriteLine("not")
#End If

                    Case AscW(">"c)
                        .Emit(op.Cgt)
#If IL_DEBUG Then
                        Debug.WriteLine("cgt")
#End If
                    Case AscW("<"c)
                        .Emit(op.Clt)
#If IL_DEBUG Then
                        Debug.WriteLine("clt")
#End If
                    Case AscW("="c)
                        .Emit(op.Ceq)
#If IL_DEBUG Then
                        Debug.WriteLine("ceq")
#End If

                    Case (AscW("<"c) << 8) + AscW("<"c)
                        .Emit(op.Shl)
#If IL_DEBUG Then
                        Debug.WriteLine("shl")
#End If
                    Case (AscW(">"c) << 8) + AscW(">"c)
                        .Emit(op.Shr)
#If IL_DEBUG Then
                        Debug.WriteLine("shr")
#End If
#End Region

#Region "Branch"
                    Case AscW(":"c)
                        .MarkLabel(lbl(num))
#If IL_DEBUG Then
                        Debug.WriteLine($":{num}")
#End If
                    Case (AscW(":"c) << 8) + AscW(":"c)
                        .Emit(op.Br, lbl(num))
#If IL_DEBUG Then
                        Debug.WriteLine($"br {num}")
#End If
                    Case (AscW("t"c) << 8) + AscW(":"c)
                        .Emit(op.Brtrue, lbl(num))
#If IL_DEBUG Then
                        Debug.WriteLine($"brtrue {num}")
#End If
                    Case (AscW("f"c) << 8) + AscW(":"c)
                        .Emit(op.Brfalse, lbl(num))
#If IL_DEBUG Then
                        Debug.WriteLine($"brfalse {num}")
#End If

                    Case (AscW("="c) << 8) + AscW(":"c)
                        .Emit(op.Beq, lbl(num))
#If IL_DEBUG Then
                        Debug.WriteLine($"beq {num}")
#End If
                    Case (AscW(">"c) << 8) + AscW(":"c)
                        .Emit(op.Bgt, lbl(num))
#If IL_DEBUG Then
                        Debug.WriteLine($"bgt {num}")
#End If
                    Case (AscW("<"c) << 8) + AscW(":"c)
                        .Emit(op.Blt, lbl(num))
#If IL_DEBUG Then
                        Debug.WriteLine($"blt {num}")
#End If
                    Case (AscW(">"c) << 8 * 2) + (AscW("="c) << 8) + AscW(":"c)
                        .Emit(op.Bge, lbl(num))
#If IL_DEBUG Then
                        Debug.WriteLine($"bge {num}")
#End If
                    Case (AscW("<"c) << 8 * 2) + (AscW("="c) << 8) + AscW(":"c)
                        .Emit(op.Ble, lbl(num))
#If IL_DEBUG Then
                        Debug.WriteLine($"ble {num}")
#End If

                    Case (AscW("!"c) << 8 * 2) + (AscW("="c) << 8) + AscW(":"c)
                        .Emit(op.Bne_Un, lbl(num))
#If IL_DEBUG Then
                        Debug.WriteLine($"bne.un {num}")
#End If
                    Case (AscW("u"c) << 8 * 2) + (AscW(">"c) << 8) + AscW(":"c)
                        .Emit(op.Bgt_Un, lbl(num))
#If IL_DEBUG Then
                        Debug.WriteLine($"bgt.un {num}")
#End If
                    Case (AscW("u"c) << 8 * 2) + (AscW("<"c) << 8) + AscW(":"c)
                        .Emit(op.Blt_Un, lbl(num))
#If IL_DEBUG Then
                        Debug.WriteLine($"blt.un {num}")
#End If
                    Case (AscW("u"c) << 8 * 3) + (AscW(">"c) << 8 * 2) + (AscW("="c) << 8) + AscW(":"c)
                        .Emit(op.Bge_Un, lbl(num))
#If IL_DEBUG Then
                        Debug.WriteLine($"bge.un {num}")
#End If
                    Case (AscW("u"c) << 8 * 3) + (AscW("<"c) << 8 * 2) + (AscW("="c) << 8) + AscW(":"c)
                        .Emit(op.Ble_Un, lbl(num))
#If IL_DEBUG Then
                        Debug.WriteLine($"ble.un {num}")
#End If
#End Region

                        'Invoke method
                        '[re] with [use] for recursion method
                    Case (AscW("r"c) << 8) + AscW("e"c)
                        .Emit(op.Tailcall)
#If IL_DEBUG Then
                        Debug.Write("tail.")
#End If
                    Case (AscW("u"c) << 8 * 2) + (AscW("s"c) << 8) + AscW("e"c)
                        .Emit(op.Call, uses(num))
#If IL_DEBUG Then
                        Debug.WriteLine($"call {uses(num)}")
#End If
                    Case (AscW("u"c) << 8 * 3) + (AscW("s"c) << 8 * 2) + (AscW("e"c) << 8) + AscW("d"c)
                        .Emit(op.Call, useds(num))
#If IL_DEBUG Then
                        Debug.WriteLine($"call {useds(num)}")
#End If
                        '[use-me]
                    Case (CLng(AscW("u"c)) << 8 * 5) + (CLng(AscW("s"c)) << 8 * 4) + (AscW("e"c) << 8 * 3) + (AscW("-"c) << 8 * 2) + (AscW("m"c) << 8) + AscW("e"c)
                        .Emit(op.Call, meth)
#If IL_DEBUG Then
                        Debug.WriteLine($"call {meth}")
#End If
                    Case (AscW("j"c) << 8 * 2) + (AscW("m"c) << 8) + AscW("p"c)
                        .Emit(op.Jmp, uses(num))
#If IL_DEBUG Then
                        Debug.WriteLine($"jmp {uses(num)}")
#End If
                        '[jmp-me]
                    Case (CLng(AscW("j"c)) << 8 * 5) + (CLng(AscW("m"c)) << 8 * 4) + (AscW("p"c) << 8 * 3) + (AscW("-"c) << 8 * 2) + (AscW("m"c) << 8) + AscW("e"c)
                        .Emit(op.Jmp, meth)
#If IL_DEBUG Then
                        Debug.WriteLine($"jmp {meth}")
#End If

                        'Load constant data
                    Case (AscW("t"c) << 8 * 2) + (AscW("x"c) << 8) + AscW("t"c)
                        .Emit(op.Ldstr, txts(num))
#If IL_DEBUG Then
                        Debug.WriteLine($"ldstr {txts(num)}")
#End If
                    Case (AscW("i"c) << 8) + AscW("4"c)
                        .Emit(op.Ldc_I4, i4s(num))
#If IL_DEBUG Then
                        Debug.WriteLine($"ldc.i4 {i4s(num)}")
#End If
                    Case (AscW("i"c) << 8) + AscW("8"c)
                        .Emit(op.Ldc_I8, i8s(num))
#If IL_DEBUG Then
                        Debug.WriteLine($"ldc.i8 {i8s(num)}")
#End If
                    Case (AscW("r"c) << 8) + AscW("4"c)
                        .Emit(op.Ldc_R4, r4s(num))
#If IL_DEBUG Then
                        Debug.WriteLine($"ldc.r4 {r4s(num)}")
#End If
                    Case (AscW("r"c) << 8) + AscW("8"c)
                        .Emit(op.Ldc_R8, r8s(num))
#If IL_DEBUG Then
                        Debug.WriteLine($"ldc.r8 {r8s(num)}")
#End If

                        'Get size of data
                    Case (AscW("m"c) << 8) + AscW("t"c)
                        .Emit(op.Sizeof, types(num))
#If IL_DEBUG Then
                        Debug.WriteLine($"sizeof {types(num)}")
#End If

                        'New object and array
                    Case (AscW("n"c) << 8 * 2) + (AscW("a"c) << 8) + AscW("t"c)
                        .Emit(op.Newarr, types(num))
#If IL_DEBUG Then
                        Debug.WriteLine($"newarr {types(num)}")
#End If
                    Case (AscW("n"c) << 8 * 2) + (AscW("e"c) << 8) + AscW("w"c)
                        .Emit(op.Newobj, If(news Is Nothing,
                                            types(num).GetConstructors(sr.BindingFlags.Public Or
                                                                       sr.BindingFlags.NonPublic Or
                                                                       sr.BindingFlags.Instance)(0),
                                            news(num)))
#If IL_DEBUG Then
                        Debug.WriteLine($"newobj {If(news Is Nothing, types(num).GetConstructors(16 Or 32 Or 4)(0), news(num))}")
#End If
                    Case (AscW("a"c) << 8 * 3) + (AscW("n"c) << 8 * 2) + (AscW("e"c) << 8) + AscW("w"c)
                        .Emit(op.Call, news(num))
#If IL_DEBUG Then
                        Debug.WriteLine($"call {news(num)}")
#End If

                        'Memory manage
                    Case (AscW("c"c) << 8 * 3) + (AscW("p"c) << 8 * 2) + (AscW("y"c) << 8) + AscW("t"c)
                        .Emit(op.Cpobj, types(num))
#If IL_DEBUG Then
                        Debug.WriteLine($"cpobj {types(num)}")
#End If
                    Case (AscW("i"c) << 8 * 3) + (AscW("n"c) << 8 * 2) + (AscW("i"c) << 8) + AscW("t"c)
                        .Emit(op.Initobj, types(num))
#If IL_DEBUG Then
                        Debug.WriteLine($"initobj {types(num)}")
#End If
                    Case (AscW("c"c) << 8 * 3) + (AscW("p"c) << 8 * 2) + (AscW("y"c) << 8) + AscW("b"c)
                        .Emit(op.Cpblk)
#If IL_DEBUG Then
                        Debug.WriteLine("cpblk")
#End If
                    Case (AscW("i"c) << 8 * 3) + (AscW("n"c) << 8 * 2) + (AscW("i"c) << 8) + AscW("b"c)
                        .Emit(op.Initblk)
#If IL_DEBUG Then
                        Debug.WriteLine("initblk")
#End If

                        'Argument
                    Case (AscW("l"c) << 8) + AscW("a"c)
                        ldarg()
#If IL_DEBUG Then
                        Debug.WriteLine($"ldarg {num}")
#End If
                    Case (AscW("s"c) << 8) + AscW("a"c)
                        .Emit(op.Starg_S, num)
#If IL_DEBUG Then
                        Debug.WriteLine($"starg {num}")
#End If
                    Case (AscW("l"c) << 8 * 2) + (AscW("a"c) << 8) + AscW("a"c)
                        .Emit(op.Ldarga_S, num)
#If IL_DEBUG Then
                        Debug.WriteLine($"ldarga {num}")
#End If

                        'Local variant
                    Case (AscW("l"c) << 8) + AscW("o"c)
                        ldloc()
#If IL_DEBUG Then
                        Debug.WriteLine($"ldloc.s {num}")
#End If
                    Case (AscW("s"c) << 8) + AscW("o"c)
                        stloc()
#If IL_DEBUG Then
                        Debug.WriteLine($"stloc.s {num}")
#End If
                    Case (AscW("l"c) << 8 * 2) + (AscW("a"c) << 8) + AscW("o"c)
                        .Emit(op.Ldloca_S, num)
#If IL_DEBUG Then
                        Debug.WriteLine($"ldloca.s {num}")
#End If

                        'Field
                    Case (AscW("l"c) << 8) + AscW("f"c)
                        .Emit(op.Ldfld, fields(num))
#If IL_DEBUG Then
                        Debug.WriteLine($"ldfld {fields(num)}")
#End If
                    Case (AscW("s"c) << 8) + AscW("f"c)
                        .Emit(op.Stfld, fields(num))
#If IL_DEBUG Then
                        Debug.WriteLine($"stfld {fields(num)}")
#End If
                    Case (AscW("l"c) << 8 * 2) + (AscW("a"c) << 8) + AscW("f"c)
                        .Emit(op.Ldflda, fields(num))
#If IL_DEBUG Then
                        Debug.WriteLine($"ldflda {fields(num)}")
#End If

                        'Static field
                    Case (AscW("l"c) << 8 * 2) + (AscW("s"c) << 8) + AscW("f"c)
                        .Emit(op.Ldsfld, fields(num))
#If IL_DEBUG Then
                        Debug.WriteLine($"ldsfld {fields(num)}")
#End If
                    Case (AscW("s"c) << 8 * 2) + (AscW("s"c) << 8) + AscW("f"c)
                        .Emit(op.Stsfld, fields(num))
#If IL_DEBUG Then
                        Debug.WriteLine($"stsfld {fields(num)}")
#End If
                    Case (AscW("l"c) << 8 * 3) + (AscW("a"c) << 8 * 2) + (AscW("s"c) << 8) + AscW("f"c)
                        .Emit(op.Ldsflda, fields(num))
#If IL_DEBUG Then
                        Debug.WriteLine($"ldflda {fields(num)}")
#End If

                        'Array element
                    Case (AscW("l"c) << 8 * 2) + (AscW("e"c) << 8) + AscW("n"c)
                        .Emit(op.Ldlen)
#If IL_DEBUG Then
                        Debug.WriteLine("ldlen")
#End If
                    Case (AscW("l"c) << 8 * 3) + (AscW("a"c) << 8 * 2) + (AscW("e"c) << 8) + AscW("t"c)
                        .Emit(op.Ldelema, types(num))
#If IL_DEBUG Then
                        Debug.WriteLine($"ldlema {types(num)}")
#End If
                    Case (AscW("l"c) << 8 * 2) + (AscW("e"c) << 8) + AscW("t"c)
                        .Emit(op.Ldelem, types(num))
#If IL_DEBUG Then
                        Debug.WriteLine($"ldlem {types(num)}")
#End If
                    Case (AscW("s"c) << 8 * 2) + (AscW("e"c) << 8) + AscW("t"c)
                        .Emit(op.Stelem, types(num))
#If IL_DEBUG Then
                        Debug.WriteLine($"stlem {types(num)}")
#End If

                        'Object type
                    Case (AscW("l"c) << 8 * 2) + (AscW("o"c) << 8) + AscW("t"c)
                        .Emit(op.Ldobj, types(num))
#If IL_DEBUG Then
                        Debug.WriteLine($"ldobj {types(num).Name}")
#End If
                    Case (AscW("s"c) << 8 * 2) + (AscW("o"c) << 8) + AscW("t"c)
                        .Emit(op.Stobj, types(num))
#If IL_DEBUG Then
                        Debug.WriteLine($"stobj {types(num).Name}")
#End If
                    Case (AscW("c"c) << 8 * 2) + (AscW("o"c) << 8) + AscW("t"c)
                        .Emit(op.Castclass, types(num))
#If IL_DEBUG Then
                        Debug.WriteLine($"castclass {types(num)}")
#End If

#Region "Indirect"
                    Case (AscW("l"c) << 8) + AscW("i"c)
                        .Emit(op.Ldind_I)
#If IL_DEBUG Then
                        Debug.WriteLine("ldind.i")
#End If
                    Case (AscW("l"c) << 8 * 2) + (AscW("i"c) << 8) + AscW("1"c)
                        .Emit(op.Ldind_I1)
#If IL_DEBUG Then
                        Debug.WriteLine("ldind.i1")
#End If
                    Case (AscW("l"c) << 8 * 2) + (AscW("i"c) << 8) + AscW("2"c)
                        .Emit(op.Ldind_I2)
#If IL_DEBUG Then
                        Debug.WriteLine("ldind.i2")
#End If
                    Case (AscW("l"c) << 8 * 2) + (AscW("i"c) << 8) + AscW("4"c)
                        .Emit(op.Ldind_I4)
#If IL_DEBUG Then
                        Debug.WriteLine("ldind.i4")
#End If
                    Case (AscW("l"c) << 8 * 2) + (AscW("i"c) << 8) + AscW("8"c)
                        .Emit(op.Ldind_I8)
#If IL_DEBUG Then
                        Debug.WriteLine("ldind.i8")
#End If
                    Case (AscW("l"c) << 8 * 2) + (AscW("r"c) << 8) + AscW("4"c)
                        .Emit(op.Ldind_R4)
#If IL_DEBUG Then
                        Debug.WriteLine("ldind.r4")
#End If
                    Case (AscW("l"c) << 8 * 2) + (AscW("r"c) << 8) + AscW("8"c)
                        .Emit(op.Ldind_R8)
#If IL_DEBUG Then
                        Debug.WriteLine("ldind.r8")
#End If
                    Case (AscW("l"c) << 8) + AscW("u"c)
                        .Emit(op.Ldind_Ref)
#If IL_DEBUG Then
                        Debug.WriteLine("ldind.ref")
#End If
                    Case (AscW("l"c) << 8 * 2) + (AscW("u"c) << 8) + AscW("1"c)
                        .Emit(op.Ldind_U1)
#If IL_DEBUG Then
                        Debug.WriteLine("ldind.u1")
#End If
                    Case (AscW("l"c) << 8 * 2) + (AscW("u"c) << 8) + AscW("2"c)
                        .Emit(op.Ldind_U2)
#If IL_DEBUG Then
                        Debug.WriteLine("ldind.u2")
#End If
                    Case (AscW("l"c) << 8 * 2) + (AscW("u"c) << 8) + AscW("4"c)
                        .Emit(op.Ldind_U4)
#If IL_DEBUG Then
                        Debug.WriteLine("ldind.u4")
#End If

                    Case (AscW("s"c) << 8) + AscW("i"c)
                        .Emit(op.Stind_I)
#If IL_DEBUG Then
                        Debug.WriteLine("stind.i")
#End If
                    Case (AscW("s"c) << 8 * 2) + (AscW("i"c) << 8) + AscW("1"c)
                        .Emit(op.Stind_I1)
#If IL_DEBUG Then
                        Debug.WriteLine("stind.i1")
#End If
                    Case (AscW("s"c) << 8 * 2) + (AscW("i"c) << 8) + AscW("2"c)
                        .Emit(op.Stind_I2)
#If IL_DEBUG Then
                        Debug.WriteLine("stind.i2")
#End If
                    Case (AscW("s"c) << 8 * 2) + (AscW("i"c) << 8) + AscW("4"c)
                        .Emit(op.Stind_I4)
#If IL_DEBUG Then
                        Debug.WriteLine("stind.4")
#End If
                    Case (AscW("s"c) << 8 * 2) + (AscW("i"c) << 8) + AscW("8"c)
                        .Emit(op.Stind_I8)
#If IL_DEBUG Then
                        Debug.WriteLine("stind.i8")
#End If
                    Case (AscW("s"c) << 8 * 2) + (AscW("r"c) << 8) + AscW("4"c)
                        .Emit(op.Stind_R4)
#If IL_DEBUG Then
                        Debug.WriteLine("stind.r4")
#End If
                    Case (AscW("s"c) << 8 * 2) + (AscW("r"c) << 8) + AscW("8"c)
                        .Emit(op.Stind_R8)
#If IL_DEBUG Then
                        Debug.WriteLine("stind.r8")
#End If
                    Case (AscW("s"c) << 8) + AscW("u"c)
                        .Emit(op.Stind_Ref)
#If IL_DEBUG Then
                        Debug.WriteLine("stind.ref")
#End If
#End Region

#Region "Convert"
                    Case (AscW("c"c) << 8) + AscW("i"c)
                        .Emit(op.Conv_I)
#If IL_DEBUG Then
                        Debug.WriteLine("conv.i")
#End If
                    Case (AscW("c"c) << 8 * 2) + (AscW("i"c) << 8) + AscW("1"c)
                        .Emit(op.Conv_I1)
#If IL_DEBUG Then
                        Debug.WriteLine("conv.i1")
#End If
                    Case (AscW("c"c) << 8 * 2) + (AscW("i"c) << 8) + AscW("2"c)
                        .Emit(op.Conv_I2)
#If IL_DEBUG Then
                        Debug.WriteLine("conv.i2")
#End If
                    Case (AscW("c"c) << 8 * 2) + (AscW("i"c) << 8) + AscW("4"c)
                        .Emit(op.Conv_I4)
#If IL_DEBUG Then
                        Debug.WriteLine("conv.i4")
#End If
                    Case (AscW("c"c) << 8 * 2) + (AscW("i"c) << 8) + AscW("8"c)
                        .Emit(op.Conv_I8)
#If IL_DEBUG Then
                        Debug.WriteLine("conv.i8")
#End If

                    Case (AscW("c"c) << 8) + AscW("u"c)
                        .Emit(op.Conv_U)
#If IL_DEBUG Then
                        Debug.WriteLine("conv.u")
#End If
                    Case (AscW("c"c) << 8 * 2) + (AscW("u"c) << 8) + AscW("1"c)
                        .Emit(op.Conv_U1)
#If IL_DEBUG Then
                        Debug.WriteLine("conv.u1")
#End If
                    Case (AscW("c"c) << 8 * 2) + (AscW("u"c) << 8) + AscW("2"c)
                        .Emit(op.Conv_U2)
#If IL_DEBUG Then
                        Debug.WriteLine("conv.u2")
#End If
                    Case (AscW("c"c) << 8 * 2) + (AscW("u"c) << 8) + AscW("4"c)
                        .Emit(op.Conv_U4)
#If IL_DEBUG Then
                        Debug.WriteLine("conv.u4")
#End If
                    Case (AscW("c"c) << 8 * 2) + (AscW("u"c) << 8) + AscW("8"c)
                        .Emit(op.Conv_U8)
#If IL_DEBUG Then
                        Debug.WriteLine("conv.u8")
#End If

                    Case (AscW("c"c) << 8 * 2) + (AscW("r"c) << 8) + AscW("4"c)
                        .Emit(op.Conv_R4)
#If IL_DEBUG Then
                        Debug.WriteLine("conv.r4")
#End If
                    Case (AscW("c"c) << 8 * 2) + (AscW("r"c) << 8) + AscW("8"c)
                        .Emit(op.Conv_R8)
#If IL_DEBUG Then
                        Debug.WriteLine("conv.r8")
#End If
#End Region

                        '=======================
                        ' Extra command
                        '=======================

                        'Argument
                    Case (AscW("c"c) << 8) + AscW("a"c)
                        .Emit(op.Dup)
                        .Emit(op.Starg_S, num)
#If IL_DEBUG Then
                        Debug.WriteLine("dup")
                        Debug.WriteLine($"starg.s {num}")
#End If
                        'Local variant
                    Case (AscW("c"c) << 8) + AscW("o"c)
                        .Emit(op.Dup)
                        stloc()
#If IL_DEBUG Then
                        Debug.WriteLine("dup")
                        Debug.WriteLine($"stloc {num}")
#End If
                        'Static field
                    Case (AscW("c"c) << 8 * 2) + (AscW("s"c) << 8) + AscW("f"c)
                        .Emit(op.Dup)
                        .Emit(op.Stsfld, fields(num))
#If IL_DEBUG Then
                        Debug.WriteLine("dup")
                        Debug.WriteLine($"stsfld {fields(num)}")
#End If

                        'Define Field Type - register field member of type
                    Case (AscW("d"c) << 8 * 2) + (AscW("f"c) << 8) + AscW("t"c)
                        If fields Is Nothing Then
                            fields = {types(num).GetField(word.ToString)}
                        Else
                            ReDim Preserve fields(fields.Length)
                            fields(fields.Length - 1) = types(num).GetField(word.ToString)
                        End If

                    'Case (AscW("d"c) << 8 * 2) + (AscW("m"c) << 8) + AscW("t"c)
                    '    types(num).GetMethod(word.ToString)


                        'Define Local Type - declare local variant
                    Case (AscW("d"c) << 8 * 2) + (AscW("o"c) << 8) + AscW("t"c)
                        .DeclareLocal(types(num))

                        'cat.0
                        'pointer 

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
                    If num < 128 Then il.Emit(op.Ldc_I4_S, CByte(num)) Else il.Emit(op.Ldc_I4, num)
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

    End Structure

End Namespace
