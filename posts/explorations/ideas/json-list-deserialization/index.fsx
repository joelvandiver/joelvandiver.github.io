(**
F# JSON List Deserialization
============================

> Lists should be default to an empty list.
*)

#r @"C:\git\joelvandiver.github.io\packages\Newtonsoft.Json\lib\netstandard2.0\Newtonsoft.Json.dll"

open System
open Newtonsoft.Json

type A = 
   {  Title: string
      Items: string list }

let json = """{ "Title": "JSON A" } """

let itemsIsNull = JsonConvert.DeserializeObject<A>(json)

(**
> Output:
```fsharp
val itemsIsNull : A = {Title = "JSON A";
                        Items = null;}
```

## Problem 1 - Null Lists Cannot Be Checked

```fsharp
let willNotCompile = itemsIsNull.Items = null
```

> Output:
```fsharp
index.fsx(28,42): error FS0043: The type 'string list' does not have 'null' as a proper value
```

### Solution - Use Object.ReferenceEquals to check for null.
*)

let willBeNull = Object.ReferenceEquals(itemsIsNull.Items, null)

(**
> Output:
```fsharp
val willBeNull : bool = true
```
*)
