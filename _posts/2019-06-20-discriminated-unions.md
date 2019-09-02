---
title: F# Discriminated Unions
categories: [F#-Topics]
tags: []
---

# Discriminated Unions

F# Discriminated Unions have labelled cases that can handle payloads of data.

Take the following example:

```fsharp
type Person = string

type Family =
| Dad of Person
| Mom of Person
| Siblings of Person list
| Twins of Person * Person
| Self
```


Each of the discriminated cases behave like function in instantiating values: 



```fsharp
let dad = Dad "Tim"
let mom = Mom "Joanna"
let siblings = Siblings ["Tim Jr."; "Sarah"; "Bobby"]
let twins = Twins ("Jane", "John")
let me = Self
```


> Output:
```fsharp
val dad : Family = Dad "Tim"
val mom : Family = Mom "Joanna"
val siblings : Family = Siblings ["Tim Jr."; "Sarah"; "Bobby"]
val twins : Family = Twins ("Jane","John")
val me : Family = Self
```

The `Family` cases are all of type `Family` so they can be packaged up in other values.


```fsharp
let myFamily = 
    [
        dad
        mom
        siblings
        twins
        me
    ]
```


> Output:
```fsharp
val myFamily : Family list =
  [Dad "Tim"; Mom "Joanna"; Siblings ["Tim Jr."; "Sarah"; "Bobby"];
   Twins ("Jane","John"); Self]
```

Discriminated Unions also support pattern matching in handling each of the cases.


```fsharp
let stateName person = 
    match person with 
    | Dad d -> printfn "My dad's name is %s." d
    | Mom m -> printfn "My mom's name is %s." m
    | Siblings sibs -> 
        sibs
        |> List.iter(fun s -> printfn "My sibling's name is %s." s)
    | Twins (a, b) -> 
        printfn "My first twin's name is %s" a
        printfn "My second twin's name is %s" b
    | Self -> printfn "My name is John"
```


> Output:
```fsharp
val stateName : person:Family -> unit
```


myFamily |> List.iter stateName


> Output:
```fsharp
My dad's name is Tim.
My mom's name is Joanna.
My sibling's name is Tim Jr..
My sibling's name is Sarah.
My sibling's name is Bobby.
My first twin's name is Jane
My second twin's name is John
My name is John
```


```fsharp
let isMyMom p = p = Mom "Joanna"
isMyMom mom
isMyMom dad
```


> Output:
```fsharp
val isMyMom : p:Family -> bool
val it : bool = false
```
