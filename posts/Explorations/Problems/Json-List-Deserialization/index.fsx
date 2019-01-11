(**
# F# JSON List Deserialization

F# promotes immutability and deemphasizes null checks that plague object oriented languages.
However, though this is a guiding principle in coding in F#, F# does have some limitations.  Typically, these limitations arise at the boundaries of the system.  

Transferring messages via JSON over HTTP is one such example of a boundary that 
presents a limitation in F#.  

**Limitation**:  F# Lists are sometimes deserialized as null, and F# states that a list 
does not have 'null' as a proper value.

This limitation arose from an assumption I had when using the Newtonsoft deserializer:

> **Lists should default to an empty list during JSON Deserialization.**


*)

#r @"C:\git\joelvandiver.github.io\packages\Newtonsoft.Json\lib\netstandard2.0\Newtonsoft.Json.dll"

open System
open Newtonsoft.Json

type A = 
   {  Title: string
      Items: string list }

let json = """{ "Title": "JSON A" }"""

let itemsIsNull = JsonConvert.DeserializeObject<A>(json)

(**
*Output:*
```console
val itemsIsNull : A = {Title = "JSON A";
                        Items = null;}
```


### Problem 1 - Use of the List will lead to an Object NullReferenceException.
```fsharp
let willThrow = itemsIsNull.Items.Length
```
*Output:*
```console
System.NullReferenceException: Object reference not set to an instance of an object.
   at <StartupCode$FSI_0007>.$FSI_0007.main@() in 
   c:\git\joelvandiver.github.io\posts\explorations\ideas\json-list-deserialization\index.fsx:line 170
Stopped due to error
```

### Problem 2 - Null lists cannot be checked for null directly.

```fsharp
let willNotCompile = itemsIsNull.Items = null
```

*Output:*
```console
index.fsx(28,42): error FS0043: The type 'string list' does not have 'null' as a proper value
```
*)

(**
___

### Solution 1 - Use Object.ReferenceEquals to Check for Null

*Drawback* -> Implementation code is littered with noisy code.
*)

let willBeNull = Object.ReferenceEquals(itemsIsNull.Items, null)

(**
*Output:*
```console
val willBeNull : bool = true
```
___
*)


(**
### Solution 2 - Change the Type Definition to Allow Option Types

*Drawback* -> Ambiguity is added to the system about the purpose of the optional list.
*)

type B =
   {  Title: string
      Items: string list option }

let itemsIsNone = JsonConvert.DeserializeObject<B>(json)

(**
*Output:*
```console
val itemsIsNone : B = {Title = "JSON A";
                       Items = None;}
```
___
*)


(**
### Solution 3 - Configure the Deserializer to Provide the Empty List

*Drawback* -> Difficult
*)



(**
// TODO:  Add support for Empty Lists in the Deserializer

___
*)