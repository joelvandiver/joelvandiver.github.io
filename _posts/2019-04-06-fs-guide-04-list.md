---
title: 04 F# List Values
categories: [Guide-F#]
tags: []
---

# F# List Values

List are arguably one of the more important collection types in F#.  
They represent an immutable collection of items of the same type.

Lists can be initialized with `[]`.

```fsharp
let firstList = [1; 2; 3; 4; 5;]
```

*Output:*
```console
val firstList : int list = [1; 2; 3; 4; 5]
```

The `int list` syntax in the output is the type of an integer list that the compiler inferred from the declaration.


Elements are accessed by the `.[i]` syntax where `i` is the index of the item.

```fsharp
let one = firstList.[0]
let two = firstList.[1]
```


*Output:*
```console
> let one = firstList.[0];;
val one : int = 1

> let two = firstList.[1];;
val two : int = 2
```


Trying to access an element out of bounds of the list will result in an exception.

```fsharp
let outOfBounds = firstList.[100]
```

*Output:*
```console
> let outOfBounds = firstList.[100];;
System.ArgumentException: The index was outside the range of elements in the list.
Parameter name: n
   at Microsoft.FSharp.Collections.PrivateListHelpers.nth[a](FSharpList`1 l, Int32 n)
   at <StartupCode$FSI_0006>.$FSI_0006.main@() in c:\git\joelvandiver.github.io\posts\Guides\Fs\01.Introduction\04.List\index.fsx:line 6
Stopped due to error
```


Elements cannot be mutated to a new value.

```fsharp
firstList.[1] <- 5
```

*Output:*
```console
index.fsx(8,1): error FS0810: Property 'Item' cannot be set
```


