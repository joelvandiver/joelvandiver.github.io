(**
# F# Function Values
04-08-2019

F# Function values are synonymous with mathematical functions in that the function is not directly coupled to the data structures it operates on.  

The simplest function value is:
*)

let f () = ()

(**
> Output:
```fsharp
val f : unit -> unit
```
*)

(**
But, this function takes unit (nothing) in and returns unit.  This function would only be useful if it had a side effect such as writing to a database, writing to a file, etc.

Let's take another function value examples.
*)

let g x = x * 2

(**
> Output:
```fsharp
val g : x:int -> int
```
*)

(**
The function `g` takes one parameter `x` and returns `x * 2`.  Since `g` is a first-class value, it can be passed around just like any other value.
*)

let h multiplier x = multiplier(x)
h g 3

(**
> Output:
```fsharp
val h : multiplier:('a -> 'b) -> x:'a -> 'b
val it : int = 6
```
*)

(**
A function value can take two partially applied parameters.
*)

let j x y = x + y

(**
> Output:
```fsharp
val j : x:int -> y:int -> int
```
*)

(**
The function `g` takes two parameters: `x` and `y`.  The parameters can be partially applied from the left.  This allows for creating new functions with the partially applied parameters.
*)

let add2 = j 2

(**
> Output:
```fsharp
val add2 : (int -> int)
```
*)

(**
Function values can also be passed to collection functions as well.
*)

let numbers = 
    [
        1
        2
        3
        4
        5
        6
    ]

let numbersPlus2 = numbers |> List.map add2

(**
> Output:
```fsharp
val numbers : int list = [1; 2; 3; 4; 5; 6]
val numbersPlus2 : int list = [3; 4; 5; 6; 7; 8]
```
*)


(**
Another useful technique is to put values behind a unit parameter.  This will delay the computation of the value until invocation of the funciton.
*)

let x = 2
let fx () = 2
fx()

(**
> Output:
```fsharp
val x : int = 2
val fx : unit -> int

> fx();;
val it : int = 2
```
*)

(**
The simple value `x` becomes a function value `fx` with a unit parameter.  
*)