#load @"packages/FSharp.Formatting/FSharp.Formatting.fsx"

open System.IO
open System.Text.RegularExpressions
open FSharp.Literate

// TODO:  Fix VSCode F# Intellisense

let source = __SOURCE_DIRECTORY__
let template = Path.Combine(__SOURCE_DIRECTORY__, @"content/template.html")
let home = Path.Combine(source, "index.md")

let getPosts (fileName: string) : string list =
   Directory.GetFiles(Path.Combine(source, "posts"), fileName, SearchOption.AllDirectories)
   |> List.ofSeq
   |> List.filter(fun f -> f.Contains("_archive") |> not)

let writeHome () =
   let convertToLink (path: string) : string = 
      let rel = 
         path
            .Replace(source, "")
            .Replace("\\index.fsx", "")
            .Replace("\\index.html", "")
            .Replace("\\index.md", "")
      let dirs = 
         rel
            .Replace("\\posts\\", "")
            .Replace("\\", " - ")
            .Replace(" Fs", " F#")
      let clean = rel.Replace("\\", "/")
      sprintf "[%s](%s)" dirs clean

   let indexes = "index.*" |> getPosts |> List.map convertToLink

   let links = 
      indexes
      |> List.map(fun l -> "- " + l)
      |> List.sort
      |> List.distinct
      |> List.fold(fun a b -> a + "\r\n" + b) ""
   let contents = File.ReadAllText(home)
   let pattern = "## Posts(.|\n)*"
   let replaced = 
      let text = sprintf "## Posts\r\n%s\r\n" links
      Regex.Replace(contents, pattern, text)
   File.WriteAllText(home, replaced)

   Literate.ProcessMarkdown(home, template)



"index.fsx" |> getPosts |> List.iter (fun script -> Literate.ProcessScriptFile(script, template))
"index.md" |> getPosts |> List.iter (fun script -> Literate.ProcessMarkdown(script, template))
writeHome()
