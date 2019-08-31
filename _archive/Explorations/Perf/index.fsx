(**
# Measure Time in FSI
*)

let square (x : float) = x * x
#time
{ 0. .. 10000000000. } |> Seq.map square
#time

(**
*Output:*
```console
--> Timing now on

Real: 00:00:00.001, CPU: 00:00:00.015, GC gen0: 0, gen1: 0, gen2: 0
val it : seq<float> = seq [0.0; 1.0; 4.0; 9.0; ...]


--> Timing now off
```
*)