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
This going to be a bit long section but if you use to write dynamic method, I'm sure those key will look very familar.

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

Local and Argument.
```
lo.{number} == Load local variant {number}.
so.{number} == Store local variant {number}.
lao.{number} == Load address of local variant {number}

la.{number} == Load method argument {number}.
sa.{number} == Store method argument {number}.
laa.{number} == Load address of method argument {number}
```
Branch
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
