#load @"packages/FSharp.Formatting/FSharp.Formatting.fsx"

open System.IO
open System.Text.RegularExpressions
open FSharp.Literate

// TODO:  Build Single with Current File through VSCode Task

let args = 
   fsi.CommandLineArgs 
   |> List.ofArray

let file = 
   let found = 
      args
      |> List.tryFind(fun arg -> arg.StartsWith("file:"))
   match found with 
   | None -> raise (System.Exception("The `file:` command line argument is required."))
   | Some file -> file.Replace("file:", "")

printfn "Running Build for File:  %s" file

let source = __SOURCE_DIRECTORY__
let template = Path.Combine(__SOURCE_DIRECTORY__, @"content/template.html")

Literate.ProcessScriptFile(file, template)
