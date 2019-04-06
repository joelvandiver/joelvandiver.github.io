(** 
# List Accumulator Function 

[GIST](https://gist.github.com/joelvandiver/d709141d3d18eeffcb2936f67ad23e9d)

Create a new list by accumulating the values in the list in ascending order.
*)

let accumulate<'T> (accumulator: 'T -> 'T -> 'T) (l: 'T list) : 'T list =
  match l with 
  | [] -> []
  | [h] -> [h]
  | _ -> 

    let rec accumulate (g: 'T list) (accumulated: 'T list) : 'T list =
        match g with 
        | [] -> accumulated
        | h :: t -> 
            match accumulated with 
            | [] -> []
            | _ -> 
                let last = accumulated |> List.rev |> List.head
                let a = (last, h) ||> accumulator
                accumulate t (accumulated @ [a])

    accumulate l.Tail [l.Head]

(**
> Output:
```fsharp
val accumulate : accumulator:('T -> 'T -> 'T) -> l:'T list -> 'T list
```
*)


(** 
## Example:  Mathematical Adder
*)
let adder = accumulate (+)
let addList = [0; 1; 2; 3; 4; 5]
let addResult = adder addList

(**
> Output:
```fsharp
val adder : (int list -> int list)
val addList : int list = [0; 1; 2; 3; 4; 5]
val addResult : int list = [0; 1; 3; 6; 10; 15]
```
*)


(** 
## Example:  Mathematical Multiplier
*)
let multiplier = accumulate (*)
let multiList = [1; 2; 3; 4; 5]
let multiResult = multiplier multiList

(**
> Output:
```fsharp
val multiplier : (int list -> int list)
val multiList : int list = [1; 2; 3; 4; 5]
val multiResult : int list = [1; 2; 6; 24; 120]
```
*)


(** 
## Example:  String Concatenator
*)
let dotConcat a b = a + "." + b
let docAccumulator = accumulate dotConcat
let alphabet = "abcdef"
// let alphabet = "abcdefghijklmnopqrstuvwxyz"
let alphaList = alphabet |> List.ofSeq |> List.map string
let alphaResult = docAccumulator alphaList

(**
> Output:
```fsharp
val dotConcat : a:string -> b:string -> string
val docAccumulator : (string list -> string list)
val alphabet : string = "abcdef"
val alphaList : string list = ["a"; "b"; "c"; "d"; "e"; "f"]
val alphaResult : string list =
  ["a"; "a.b"; "a.b.c"; "a.b.c.d"; "a.b.c.d.e"; "a.b.c.d.e.f"]
```
*)