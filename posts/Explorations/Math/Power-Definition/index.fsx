(**
# Power Definition
*)

let (^) b = function
| n when n < 0 -> 0
| 0 -> 1
| n -> 
   { 0 .. (n - 1) }
   |> Seq.map (fun _ -> b)
   |> Seq.reduce (*)

3 ^ 3
4 ^ 2

(**
*Output:*
```console
val ( ^ ) : b:int -> _arg1:int -> int
val it : int = 27
val it : int = 16
```
*)
