---
title: F# Single Case Discriminated Unions
categories: [Guide-F#]
tags: []
---

# Single Case Discriminated Unions

First, let's start with a simple tuple of two integers:


```fsharp
let xy = 1, 3
```


> Output:
```fsharp
val xy : int * int = (1, 3)
```

The `xy` value can be used to model many things, but it does not contain any labels other than the value itself to convey what it means.

If you would like to describe the type more thoroughly, than you could use a record type definition:

Let's compare a simple point record and a single case discriminated union.

```fsharp
type Point = { x: int; y: int }

let point = { x = 1; y = 3 }
```


> Output:
```fsharp
type Point =      
  {x: int;        
   y: int;}       
val point : Point = {x = 1; 
                     y = 3;}
```

We have a definition that labels the type, `Point`, and labels the fields, `x` and `y` respectively.  But, a case could be made that the `x` and `y` have a natural order and mathematics and could be inferred by their location in the definition instead.

Now, let's compare the record type to a discriminated union of single case:

```fsharp
type Point' = Cartesian of int * int

let point' = Cartesian (1, 3)
```


> Output:
```fsharp
type Point' = | Cartesian of int * int
val point' : Point' = Cartesian (1,3)
```

Here, we are only describing the type and the case and not the fields themselves.  

We can use the single case with a pattern match.


```fsharp
let translate a b = function Cartesian (x, y) -> (x + a, y + b)
```


> Output:
```fsharp
val translate : a:int -> b:int -> _arg1:Point' -> int * int
```

Note that a discriminated union of a single case does not require the `|` operator to separate cases:
```fsharp
let translate a b = function
    | Cartesian (x, y) -> (x + a, y + b)
```


```fsharp
let point'' = translate 1 2 point'
```


> Output:
```fsharp
val point'' : int * int = (2, 5)
```


