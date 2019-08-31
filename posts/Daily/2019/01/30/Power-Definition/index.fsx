(**
# Power Definition
*)

let powerReduce b = function
| n when n < 0 -> 0
| 0 -> 1
| n -> 
   { 0 .. (n - 1) }
   |> Seq.map (fun _ -> b)
   |> Seq.reduce (*)

powerReduce 3 3
powerReduce 4 2

(**
*Output:*
```console
val power1 : b:int -> _arg1:int -> int
val it : int = 27
val it : int = 16
```
*)

let rec powerPair = function  
   | (_, n) when n < 0 -> 0.
   | (_, 0) -> 1.
   | (b, n) -> b * powerPair(b, n - 1)

let xs = { 0 .. 100 }
let ys = 
   xs
   |> Seq.map (
      fun x -> 
         let y = (float x, x) |> powerPair
         (x, y))

(**
*Output:*
```console
val powerPair : int * int -> int
val xs : seq<int>
val ys : seq<int * int>
val it : seq<int * int> = seq [(0, 1); (1, 1); (2, 4); (3, 27); ...]
```
*)
