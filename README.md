# ILS - Intermediate Language Script

Release : V.1 : .Net core 3.1


## What's this using for ?
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
2. '.' is key to tell compiler after this char going to be int32 number and ready to excute command key next time, number will be reset when next char is number.
3. ' ' is excution command key, reset keyword and number back to 0, ready to input next command code.

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

## Number
Anyone already forget we still has the last part ? don't worry me too...

I think most of reader already guess what's number in this part using for, that right it use to tell compiler to pick info from data we get in section 'Managed info input from managed code'.

### let write some dynamic method

```vb
Dim Find = "la.1 .1 - sa.1 la.1 .0 <:0 la.3 la.0 la.1 let.0 la.2 used.0 t:0 jmp-me :0 la.1".
            compile(Of Func(Of T(), Int32, Int32, Func(Of Int32, Int32, Boolean))).
            type(Of Int32).
            used(Of Func(Of Int32, Int32, Boolean)).
            fin

Dim Where_is_7 = Find({9, 8, 7, 6, 5}, 5, 7, Function(R, L) R = L) 
```
From the name of example function, of course this is simple function for find target element in array but how much you undestand it ?

#### Let's break it down!

`la.1` is `load argument index 1`, when we look at `Where_is_7` line, arg1 is `5`.

`.1` is put number `1` into stack and then we `-` them, so now `5` and `1` are gone but! we get `4` back instead.

`sa.1` now, we sent our `4` to store back at arg1 ... well, I just explain `arg1 -= 1` for 3 lines(maybe I should write some book XD).

Then what's about this `la.1 .0 <:0` ? it's load arg1(our `4`) back into stack and put `0` too and most of the time we put any int with 0 is when we compare and branching code!

`<:0` is if previous less then last on stack just go to label `0`, of course our `4` is more then `0` so we move to next keyword.

Next is `la.3 la.0 la.1 let.0 la.2 used.0 t:0` !

Reader : Wait! you can get your OT, just slow down!

Don't worry, I need to bring entire part; let skip `la.3` and look at `la.0`, arg0 is `{9, 8, 7, 6, 5}` and we reload arg1 our `4` back then we `let.0` it, L.E.T is `load element type` and `.type(0)` is `Int32`, so we can rearrange it as `arg0 index 4 load element type Int32` and now we just get `arg0(arg1)` value, it's `5`!

Now `la.0 la.1 let.0` is gone, let replace it with 5 then we got this `la.3 .5 la.2 used.0 t:0`.

`la.3` is delegate so when we use keyword `used.0` we invoke delegate index 0 from `.used(Of Func(Of Int32, Int32, Boolean))` and `arg2` is 7 so it become `arg3(5, 7)` then we move to `Function(R, L) R = L)` and we got compare result `5 = 7` back to our method.

After we get our spoil back, `t:0` knock our door and ask did we `true` back ? of course not, so we move to `jmp-me` be dejavu again but this time our `arg1` is `4` from the last time we store it and then the story continue, again and again until we got `true` for `t:0` or our `arg1` worthless then `0`.

But even if we finally reach `:0` label number 0 we try to reach for so long, `la.1` mostly be `-1`, a value less then `0`.

#### Reader : Wait! where is `return`!  I never find any `;` in the code.

ILS always add `return` or `;` at last key also `jmp` and `jmp-me` are special, those 2 never need to return because they jump foward, not return back.

#### Reader : What's about `tail call` or that `re use` ?

The need to `return` as other `call` but just in formal, they never return back too, let's me replace `jmp` to `re use` for example.

`la.1 .1 - sa.1 la.1 .0 <:0 la.3 la.0 la.1 let.0 la.2 used.0 t:0 la.0 la.1 la.2 la.3 re use-me ; :0 la.1`

Reader : The code is so long for no reason.

Yep but `jmp` sometime has issue with generic method and `tail call` is much friendly to VS stack tracer then `jmp`.

Reader : But the code is...

No need to worry because it's time for....

## Quality of life(syntax)

ILS has a feature call `Continuum of keyword` so I can rewrite `tail call` Find method like this.

`la.1 .1 - sa.1 la.1 .0 <:0 la.3.0.1 let.0 la.2 used.0 t:0 la.0.1.2.3 re use-me ; :0 la.1`

Reader : Much shoter but how ?

From `Process key`#2 when `.` 2nd time without ` ` before it, compiler will excute the same keyword with new number.

Reader : Can I do it with keyword ?

Of course !
`la.1 .1 - sa.1.la .0 <:0 la.3.0.1 let.0 la.2 used.0 t:0 la.0.1.2.3 re use-me ; :0 la.1`

Reader : Wait, `Process key`#3 said key and number will reset to 0 so ... can I get rid of 0 !?

As you wish !
`la.1 .1 - sa.1.la . <: la.3.0.1 let la.2 used t: la..1.2.3 re use-me ; : la.1`

#### Reader : Huh, why `la.3.0.1` still has 0 on it ? should it be `la3..1` ?

#### No, it shouldn't, don't forget only after key ` ` reset number back to 0; if you write like that it will be `la3.3.1` instead!

Never be too greedy, cut too much corner is dangerous.

Somebody passby : Hey you, did you just teach people to abuse your own code bug !?

Ok everyone, hope ILS make your quality of life on coding better and hope my code get into `GitHub Arctic Code Vault` XD.
