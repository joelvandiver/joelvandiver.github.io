#load @"packages/FSharp.Formatting/FSharp.Formatting.fsx"

open System.IO
open FSharp.Literate

let source = __SOURCE_DIRECTORY__
let template = Path.Combine(__SOURCE_DIRECTORY__, @"/content/template.html")
let script = Path.Combine(source, "posts/2018/12/08/literate.fsx")

Literate.ProcessScriptFile(script, template)
