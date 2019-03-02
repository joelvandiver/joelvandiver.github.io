open System

let circleArea = fun r -> Math.PI * Math.Pow(r, 2.)
let xs = { 0. .. 10. }
let table = xs |> Seq.map circleArea

(**
*Output:*
```console
val circleArea : r:float -> float
val xs : seq<float>
val table : seq<float>
```
*)

(**
*Output:*
```console
val it : seq<float> = seq [0.0; 3.141592654; 12.56637061; 28.27433388; ...]
```
*)