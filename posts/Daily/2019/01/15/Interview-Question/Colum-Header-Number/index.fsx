(**
# Convert Spreadsheet Column Headers to a Number

### Expected Sample Set:
| x | y |
| -- | -- |
| A | 1 | 
| B | 2 | 
| C | 3 | 
| D | 4 | 
| ... | ... |
| AA | 27 | 
| AB | 28 | 
| AC | 29 | 
| AD | 30 | 
| AE | 31 | 
| ... | ... |

*)

open System

(**
> Step 1.  Define the Alpha String
*)

let alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"

(**
> Step 2.  Define the convertText function that accepts the text as input and returns an int. 
*)

let convertText (text: string) : int =
   text
    |> List.ofSeq   // Convert the string to a list of chars.
    |> List.rev     // Revs the list so that the index starts in the lowest order.
    |> List.mapi(   // Use map to include the index with the alpha char.
         fun i a -> 
            // Found the power of 26 to get the multiplier at each index place.
            let power26 = Math.Pow(26., (float i))
            // Conver the index of the alpha char from the original list (not the reversed list).
            let alphaIndex = alpha.IndexOf(a) + 1 |> float
            // Multiply the final power to the alpha index and conver to and int from a double.
            let result = power26 * alphaIndex |> int
            result)
   // Sum the final list.
   |> List.reduce(+)

(**
*Output:*
```console
val alpha : string = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
val convertText : text:string -> int
```
*)

(**
### Test
*)

let data = ["AA"; "AB"; "AC"; "AD"; "AE"; "AF"; "AG"; "AH"; "AI"; "AJ";]
let expected = [27; 28; 29; 30; 31; 32; 33; 34; 35; 36;] 

let result = data |> List.map convertText
let test = expected = result

(**
*Output:*
```console
val data : string list =
  ["AA"; "AB"; "AC"; "AD"; "AE"; "AF"; "AG"; "AH"; "AI"; "AJ"]
val expected : int list = [27; 28; 29; 30; 31; 32; 33; 34; 35; 36]
val result : int list = [27; 28; 29; 30; 31; 32; 33; 34; 35; 36]
val test : bool = true
```
*)
