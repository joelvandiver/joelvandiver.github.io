(**
# Get the Previous and Next Value Per Item
*)

(**
## With Default Value or With Option
*)
let convertToPrevAndNext<'T> (defaultValue: 'T) (items: 'T list) : ('T * 'T * 'T) list = 
   match items with 
   | [] -> []
   | h :: [] -> [defaultValue, h, defaultValue]
   | _ -> 
      let max = items.Length - 1
      items
      |> List.mapi(
         fun i x -> 
            match i with 
            | 0 -> defaultValue, x, items.[1]
            | i when i < max -> items.[i - 1], x, items.[i + 1]
            | i when i = max -> items.[i - 1], x, defaultValue
            | _ -> defaultValue, defaultValue, defaultValue)

let convertToPrevAndNextOption<'T> (items: 'T list) : ('T option * 'T * 'T option ) list = 
   match items with 
   | [] -> []
   | h :: [] -> [None, h, None]
   | _ -> 
      let max = items.Length - 1
      items
      |> List.mapi(
         fun i x -> 
            match i with 
            | 0 -> None, x, Some items.[1]
            | i when i < max -> Some items.[i - 1], x, Some items.[i + 1]
            | _ -> Some items.[i - 1], x, None)



(**
### Test Default Value
*)
let items = ["a"; "b"; "c"; "d"]
let defaultValue = ""

let expected = [
   "", "a", "b"
   "a", "b", "c"
   "b", "c", "d"
   "c", "d", ""
]

let actual = convertToPrevAndNext defaultValue items
let test = actual = expected


(**
*Output:*
```console
val convertToPrevAndNext :
  defaultValue:'T -> items:'T list -> ('T * 'T * 'T) list
val items : string list = ["a"; "b"; "c"; "d"]
val defaultValue : string = ""
val expected : (string * string * string) list =
  [("", "a", "b"); ("a", "b", "c"); ("b", "c", "d"); ("c", "d", "")]
val actual : (string * string * string) list =
  [("", "a", "b"); ("a", "b", "c"); ("b", "c", "d"); ("c", "d", "")]
val test : bool = true
```
*)

(**
### Test With Option
*)
let expectedOption = [
   None, "a", Some "b"
   Some "a", "b", Some "c"
   Some "b", "c", Some "d"
   Some "c", "d", None
]

let actualOption = convertToPrevAndNextOption items
let testOption = actualOption = expectedOption

(**
*Output:*
```console
val convertToPrevAndNextOption :
  items:'T list -> ('T option * 'T * 'T option) list
val expectedOption : (string option * string * string option) list =
  [(None, "a", Some "b"); (Some "a", "b", Some "c"); (Some "b", "c", Some "d");
   (Some "c", "d", None)]
val actualOption : (string option * string * string option) list =
  [(None, "a", Some "b"); (Some "a", "b", Some "c"); (Some "b", "c", Some "d");
   (Some "c", "d", None)]
val testOption : bool = true
```
*)