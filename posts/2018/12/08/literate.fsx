(**
# F# Literate Site Gen
Some more documentation using `Markdown`.
*)

(*** include: final-sample ***)

(** 
## Second-level heading
With some more documentation
*)

(*** define: final-sample ***)
let helloWorld() = printfn "Hello world!"



#load @"../../../../packages/FSharp.Formatting/FSharp.Formatting.fsx"
open FSharp.Literate
open System.IO

let source = __SOURCE_DIRECTORY__
let template = Path.Combine(source, "template.html")

let script = Path.Combine(source, "literate.fsx")
Literate.ProcessScriptFile(script, template)
