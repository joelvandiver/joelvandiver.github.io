---
title: 01 F# Values
categories: [Guide-F#]
tags: []
---


# F# Values

### Value Definition
Let's start our F# journey with a simple `let` value binding:

```fsharp
let x = ()
```


#### Description
`let` instructs the F# compiler to set aside a memory location with a name of `x`.  The value in this case is `()` which is F#'s unit.  The F#'s unit value is the simplest possible value.  It's meaning is synonymous to *nothing* as in "**Nothing** meaningful happened."  This concept may seem strange at first, but it leads to greater flexibility later on.

> The use of the term *value* is preferrable over *variable* since *variable* implies that the value may change over time.

#### Note on Immutability
F# enforces immutability on values by default.  Attempting to set a value to a new value after definition will not compile.  The `isImmuatable` value below will fail on the second line.

```fsharp
let isImmuatable = true
```



*Output*:
```console
val isImmuatable : bool = true
```

Note the F# mutation operator `<-` below.

```fsharp
isImmuatable <- false
```

*Output*:
```console
index.fsx(25,1): error FS0027: This value is not mutable. Consider using the mutable keyword, e.g. 'let mutable isImmuatable = expression'.
``` 



#### Mutable Values

```fsharp
let mutable mutableValue = true
mutableValue <- false
mutableValue;;
```

*Output:*
```console
val mutable mutableValue : bool = false
val it : bool = false
```



#### Note on Different Types
A mutable value cannot be set to another value of another type.  The following will not compile:

```fsharp
mutableValue <- 1
```

Also, values of different types cannot be compared.
```fsharp
let willNotCompile = true = 1
```




