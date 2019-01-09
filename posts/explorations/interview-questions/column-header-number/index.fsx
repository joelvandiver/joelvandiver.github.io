(**
# Convert Spreadsheet Column Headers to a Number

## Problem 1:  Convert a whole number to a spreadsheet column header.

### Expected Sample Set:
| x | y |
| -- | -- |
| 1 | A |
| 2 | B |
| 3 | C |
| 4 | D |
| ... | ... |
| 27 | AA |
| 28 | AB |
| 29 | AC |
| 30 | AD |
| 31 | AE |
| ... | ... |

*)

open System

let alpha = 
   "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
   |> List.ofSeq

let divide (q: double) (d: double) : double * double =
   Math.Truncate(q / d), q % d

// let convertBase (n: double) (b: double) : 

let getColumnName (n: double) : string =
   match n with 
   | n when n <= 0 ->  ""
   | _ -> ""
