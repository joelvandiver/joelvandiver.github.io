#load @"packages/FSharp.Formatting/FSharp.Formatting.fsx"

open System.IO
open System.Text.RegularExpressions
open FSharp.Literate

let source = __SOURCE_DIRECTORY__
let template = Path.Combine(__SOURCE_DIRECTORY__, @"content/template.html")
let file = @"C:\git\joelvandiver.github.io\posts\explorations\ideas\json-list-deserialization\index.fsx"

Literate.ProcessScriptFile(file, template)
