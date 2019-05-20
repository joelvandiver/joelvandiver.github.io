(**
# Option
05-20-2019

Option values in F# provide the ability to decribe a value as either `Some` or `None`.  Typically, in imperative languages such as C#, `null` is used to convey the same meaning.  However, the use of `null` may also indicate other procedural meanings such as "incomplete" or "not yet".  This overloading of meaning creates ambiguity.  

*)

(**
> Returning option values from functions requires the caller to handle the None case.

*)

let workOnOptionalValue data = 
    if data = "Do Work" then Some "Work was done."
    else None

let workIsDone = workOnOptionalValue "Do Work"
let workIsNone = workOnOptionalValue "No work"


(**
> Returning null values does not require the caller to handle the null case.

*)
let workOnNullValue data = 
    if data = "Do Work" then "Work was done."
    else null

let workIsDoneAmbiguous = workOnNullValue "Do Work"
let workIsNullAmbiguous = workOnNullValue "No work"

(**
> Output:
```fsharp
val workOnNullValue : data:string -> string
val workIsDoneAmbiguous : string = "Work was done."
val workIsNullAmbiguous : string = null
```
*)

(**
> A null reference exception will occur when the caller operates on a null value.

*)

let willRaiseException = workIsNullAmbiguous.Length = 7

(**
> Output:
```fsharp
System.NullReferenceException: Object reference not set to an instance of an object.
   at <StartupCode$FSI_0005>.$FSI_0005.main@() in c:\git\joelvandiver.github.io\posts\Guides\Fs\01.Introduction\05.Option\index.fsx:line 25
Stopped due to error
```
*)


(**
> Output:
```fsharp
val workOnOptionalValue : data:string -> string option
val workIsDone : string option = Some "Work was done."
val workIsNone : string option = None
```
*)


(**
Option values can be pattern matched to extract values.
...
*)

