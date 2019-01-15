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

let alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"

let convertText (text: string) : Double =
   text
   |> List.ofSeq
   |> List.rev
   |> List.mapi(
         fun i a -> 
            let power27 = Math.Pow(27., (float i))
            let alphaIndex = alpha.IndexOf(a) + 1 |> float
            let result = power27 * alphaIndex
            result)
   |> List.reduce(+)

(**
*Output:*
```console
val alpha : string = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
val convertText : text:string -> Double
```
*)

(**
### Test
*)

let data = ["AA"; "AB"; "AC"; "AD"; "AE"; "AF"; "AG"; "AH"; "AI"; "AJ";]
let expected = [28; 29; 30; 31; 32; 33; 34; 35; 36; 37;] |> List.map float

let result = data |> List.map convertText
let test = expected = result

(**
*Output:*
```console
val data : string list =
  ["AA"; "AB"; "AC"; "AD"; "AE"; "AF"; "AG"; "AH"; "AI"; "AJ"]
val expected : float list =
  [28.0; 29.0; 30.0; 31.0; 32.0; 33.0; 34.0; 35.0; 36.0; 37.0]
val result : Double list =
  [28.0; 29.0; 30.0; 31.0; 32.0; 33.0; 34.0; 35.0; 36.0; 37.0]
val test : bool = true
```
*)
