---
title: F# Implicit vs Explicit Value Declaration
categories: [Guide-F#]
tags: []
---


# F# Implicit vs Explicit Value Declaration

Values can be defined in F# either implicitly or explicitly, as in the values below.

```fsharp
let implicitInt = 1
let explicitInt : int = 1
```

*Output:*
```console
val implicit : int = 1
val explicit : int = 1
```


Choosing between implicit and explicit value declarations will largely depend on context.  

Simple value declarations are easier to work with as implicit declarations, but values involving complex type systems are generally easier to maintain with explicit declarations.  


