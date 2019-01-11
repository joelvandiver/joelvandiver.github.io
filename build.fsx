#load @"packages/FSharp.Formatting/FSharp.Formatting.fsx"

open System.IO
open System.Text.RegularExpressions
open FSharp.Literate

let source = __SOURCE_DIRECTORY__
let template = Path.Combine(__SOURCE_DIRECTORY__, @"content/template.html")

// TODO:  Build markdown pages.
// TODO:  Separate links into sections by folders.
// TODO:  Only build files that have changed after the template changed.

let fsxLink (path: string) = 
   let pattern = "\n# (.*?)\r"
   let rel = 
      path
         .Replace(source, "")
         .Replace("\\index.fsx", "")
   let dirs = 
      rel
         .Replace("\\posts\\", "")
         .Replace("\\", " - ")
         .Replace(" Fs", " F#")
   let clean = rel.Replace("\\", "/")
   sprintf "[%s](%s)" dirs clean

let postFiles = 
   Directory.GetFiles(Path.Combine(source, "posts"), "index.fsx", SearchOption.AllDirectories)
   |> List.ofSeq

let writeHome () =
   let links = 
      postFiles
      |> List.filter(fun f -> f.Contains("_archive") |> not)
      |> List.map fsxLink
      |> List.map(fun l -> "- " + l)
      |> List.sort
      |> List.fold(fun a b -> a + "\r\n" + b) ""
   let home = Path.Combine(source, "index.md")
   let contents = File.ReadAllText(home)
   let pattern = "## Posts(.|\n)*"
   let replaced = 
      let text = sprintf "## Posts%s\r\n" links
      Regex.Replace(contents, pattern, text)
   File.WriteAllText(home, replaced)

   Literate.ProcessMarkdown(home, template)

let processScript script = Literate.ProcessScriptFile(script, template)


writeHome()

postFiles
|> List.iter processScript


