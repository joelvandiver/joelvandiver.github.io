(**
# Extract a Text from a String
*)

open System.Text.RegularExpressions


let contents = 
   """(** 
# Some Blog Post Title

*)
   """

let regex = new Regex("\n# (?<title>.*?)\n")
let m = regex.Match(contents)
m.Groups.["title"].Value
