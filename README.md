# ILS - Intermediate Language Script

Release : V.1 : .Net core 3.1


## What's it do ?
Dynamic compile a new method on runtime from string.

## Why not use `System.Reflection.Emit.ILGenerator` directly ?
Because it's very long code to write and read.

## Why do I feel like I write cil/msil direcly ?
I like the simplicity of cil syntax so I try to keep it as it is.

### Enough of talk, let move to ils.

# Initialize a new method

```vb
Dim New_method As Delegate = "ils code".compile(Of Delegate).{Managed info input}.fin()
```
#### If you can't use extension method with generic type.
```C#
Delegate New_method = compile(Of Delegate)("ils code).{Managed info input}.fin()"
```

## What's 'Managed info input' ?
Any data not keyword or int32 input like int64, float, method signal or type signal.

### Let's write a very generic method, the Hello world.
```vb
Call "txt.0 use.0 txt.1 use.0".
      compile(Of Action).
      txt("Hello", " world").
      use(New Action(Of String)(AddressOf Console.Write).Method).fin()()
```

It's compile into cil like this.

```cil
ldstr "Hello"
call  void [System.Console]System.Console::Write(string)
ldstr " world"
call  void [System.Console]System.Console::Write(string)
ret
```

# Syntax
ils syntax code is very simple, it has only 3 key.
1. Process key
2. Keyword
3. Number

## Process key
Process key is a trigger key char, when compiler read to this key it will do some compile, only 3 keys use in ils 1.0.
1. ':' is key to tell compiler code about to be branch like if, do, for or goto.
2. '.' is key to tell compiler after this char going to be int32 number.
3. ' ' is finish command key and ready to input next command code.

## Keyword
This going to be a bit long section but if you used to write dynamic method, I'm sure those key will look very familar.

### Math
```
+
-
*
/
% Mod, reminder of 2 divide number.

-1 special input, doen't need to '.' to tell compiler.
++
--
** number power by 2
-* multiply by -1
-/ divide by -1

& and
| or
&| xor
! not

<< shift left
>> shift right

> greater
< lesser
= equal
```

### Local and Argument.
```
lo.{number} == Load local variant {number}.
so.{number} == Store local variant {number}.
lao.{number} == Load address of local variant {number}.

la.{number} == Load method argument {number}.
sa.{number} == Store method argument {number}.
laa.{number} == Load address of method argument {number}.
```
### Branch
```
:{number} label{number}
::{number} goto label{number}
t:{number} on true goto label{number}
f:{number} on false goto label{number}

=:{number}
>:{number}
<:{number}
>=:{number}
<=:{number}

//compare unsign//
u>:{number}
u<:{number}
u>=:{number}
u<=:{number}
```

### Method invoke
```
use.{number} == invoke method{number}
use-me == invoke this method aka recursion this method.

used.{number} == invoke delegate; 
Example,
Function D(A As String, B As Func(Of String, Object)) B(A);
D = "la.1 la.0 used.0".
     compile(Of Func(Of String, Func(Of String, Object), Object).
     used(Of Func(Of String, Object)).
     fin()

re == It's key use before 'use', 'use-me' or 'used' to do tail call, must not left anything on stack except method argument.
Example,
D = "la.1 la.0 re used.0".

jmp.{number} == jump to method of 'use.{number}';
Like 're', it do tall call too but doesn't need to place any argument on stack, it move current argument for target method.

jmp-me == just jump loop back to current method to do tail call recursion.
```

### Array
```
nat.{number} == New array type{number}.
len == Load array length.
let.{number} == Load array element as type{number}.
set.{number} == Store array element as type{number}.
laet.{number} = Load address of type{number} from array element.
```

### Class & Field
```
new.{number} == new object type{number}, like 'use' put all argument for it befor this keyword.

lf.{number} == load field {number}.
sf.{number} == store field {number}.
laf.{number} == load address of field {number}.

//for static field
ldsf.{number}
stsf.{number}
lasf.{number}
```

### Pointer
```
lot.{number} == Load object type{number} from address.
sot.{number} == Store object type{number} to address.

[shortcut for primitive type]
li == load native int.
li1
li2
li4 == load int32.
li8

lu == load unsign native int.
lu1
lu2
lu4

lr4
lr8 == load float64(double).

si == store native int.
si1
si2
si4
si8

su

sr4
sr8
```

### Convert type
```
cot.{number} == Convert to type{number}
ci == convert to native int.
ci1
ci2
ci4 == convert to int32.
ci8

cu == convert to unsign native int.
cu1
cu2
cu4

cr4
cr8 == convert to float64(double).
```

### Memory managed
```
mt.{number} == mass(size) of type{number}

cpyt.{number} == copy type{number} to target from source.
init.{number} == initialize value type{number}

cpyb == copy a block of byte.
inib == initialize value to a block of byte.
```

### General
```
; == ret aka return.
!! == break, for debuging.
[+] == dup aka duplicate last value on stack.
[-] == pop, delete last value on stack.
[n] == alloc dynamic memory pool, must not left anything on stack except byte size to alloc, auto free when end method.
```
### Managed info input
```
i4.{number} == Input int32 number from {number} index of '.i4(int32())'.
i8.{number}
r4.{number}
r8.{number}
txt.{number}
```
### Managed info input from managed code
They are instance method for input info from managed code to dynamic method.
```vb
Dim New_method As Delegate = "ils code".compile(Of Delegate).{Managed info input}.fin()
```

```vb
.i4(ParamArray Input() As Integer)
.i8(ParamArray Input() As Long)
.r4(ParamArray Input() As Single)
.r8(ParamArray Input() As Double)
.txt(ParamArray Input() As String)

.field(ParamArray Input() As FieldInfo)
.use(ParamArray Input() As MethodInfo)
.new(ParamArray Input() As ConstructorInfo)
.type(ParamArray Input() As System.Type)

'// define local variant.
.def(ParamArray Input() As Type)
```

#### Quality of life
```vb
.field(Of T1, ..., T8)(Name1 As String, ..., Name8 As String)
.used(Of T1 As Class, ..., T8 As Class)()
.new(Of T1, ..., T8)()
.type(Of T1, ..., T8)()

.def(Of T1, ..., T8)()

'// define pined local variant, be careful, it could lead to memory leak problem.
.defu(Of T1, ..., T8)()
```
