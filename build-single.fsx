#load @"packages/FSharp.Formatting/FSharp.Formatting.fsx"

open System.IO
open System.Text.RegularExpressions
open FSharp.Literate

// TODO:  Build Single with Current File through VSCode Task

let source = __SOURCE_DIRECTORY__
let template = Path.Combine(__SOURCE_DIRECTORY__, @"content/template.html")
let file = @"C:\git\joelvandiver.github.io\posts\tutorials\fs\fsx\cli-args\index.fsx"
// let file = @"C:\git\joelvandiver.github.io\posts\explorations\interview-questions\column-header-number\index.fsx"

Literate.ProcessScriptFile(file, template)
