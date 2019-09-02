(**
# F# Simple Values
*)

open System

(**
### Numbers
*)
let numInteger = 1
let numDecimal = 1M
let numLong    = 1L
let numDouble  = 1.
(**
*Output:*
```console
val numInteger : int = 1
val numDecimal : decimal = 1M
val numLong : int64 = 1L
val numDouble : float = 1.0
```
*)

(**
### Boolean
*)
let flag = false
(**
*Output:*
```console
val flag : bool = false
```
*)

(**
### String
*)
let text = "Here's some text."
(**
*Output:*
```console
val text : string = "Here's some text."
```
*)

(**
### DateTime
*)
let date = DateTime.UtcNow
(**
*Output:*
```console
val date : DateTime = 1/15/19 11:57:13 PM
```
*)