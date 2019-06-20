(**
# Discriminated Unions
06-20-2019

F# Discriminated Unions have labelled cases that can handle payloads of data.

Take the following example:
*)

type Person = string

type Family =
| Dad of Person
| Mom of Person
| Siblings of Person list
| Self

(**
Each of the discriminated cases behave like function in instantiating values: 

*)

let dad = Dad "Tim"
let mom = Mom "Joanna"
let siblings = Siblings ["Tim Jr."; "Sarah"; "Bobby"]
let me = Self

(**
> Output:
```fsharp
val dad : Family = Dad "Tim"
val mom : Family = Mom "Joanna"
val siblings : Family = Siblings ["Tim Jr."; "Sarah"; "Bobby"]
val me : Family = Self
```

The `Family` cases are all of type `Family` so they can be packaged up in other values.
*)

let myFamily = 
    [
        dad
        mom
        siblings
        me
    ]

(**
> Output:
```fsharp
val myFamily : Family list =
  [Dad "Tim"; Mom "Joanna"; Siblings ["Tim Jr."; "Sarah"; "Bobby"]; Self]
```

Discriminated Unions also support pattern matching in handling each of the cases.
*)

let stateName person = 
    match person with 
    | Dad d -> printfn "My dad's name is %s." d
    | Mom m -> printfn "My mom's name is %s." m
    | Siblings sibs -> 
        sibs
        |> List.iter(fun s -> printfn "My sibling's name is %s." s)
    | Self -> printfn "My name is John"

