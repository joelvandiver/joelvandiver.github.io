(**
# Factorials
*)

let rec fact = function
   | n when n < 0 -> 0
   | 0 -> 1
   | n -> n * fact (n - 1)

(**
*Output:*
```console
val fact : _arg1:int -> int
```
*)

let xs = { 0 .. 20 }
let ys = xs |> Seq.map fact

(**
*Output:*
```console
val xs : seq<int>
val ys : seq<int>
val it : seq<int> = seq [1; 1; 2; 6; ...]
```
*)

let badValue = fact -1
(**
*Output:*
```console
val badValue : int = 0
```
*)

let rec factPrinter = function
   | 0 -> "1", 1
   | n -> 
      let stmt', value = factPrinter (n - 1)
      let final = sprintf "%d * fact (%s)" n stmt'
      final, n * value

factPrinter 4

(**
*Output:*
```console
val factPrinter : _arg1:int -> string * int
val it : string * int = ("4 * fact (3 * fact (2 * fact (1 * fact (1))))", 24)
```
*)