# The Art of Meta Programming in F#

## Extract Text from a String
[GIST](https://gist.github.com/joelvandiver/dc3693409136f4bf0bc48b6352d1ac5d)

```fsharp
open System.Text.RegularExpressions

let contents = 
   """(** 
# Some Blog Post Title

*)
   """

let regex = Regex("\n# (?<title>.*?)\n")
let m = regex.Match(contents)
m.Groups.["title"].Value
```