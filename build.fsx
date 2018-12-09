#load @"packages/FSharp.Formatting/FSharp.Formatting.fsx"

open System.IO
open System.Text.RegularExpressions
open FSharp.Literate

let source = __SOURCE_DIRECTORY__
let template = Path.Combine(__SOURCE_DIRECTORY__, @"content/template.html")

// TODO:  Build home with links to all posts
// TODO:  Put navigation back home in all sub pages.
// TODO:  Build Navigate to all other pages.

let mdLink (path: string) = 
   let clean = path.Replace("\\", "/")
   sprintf "[%s](%s)" clean clean

let postFiles = 
   Directory.GetFiles(Path.Combine(source, "posts"), "index.fsx", SearchOption.AllDirectories)
   |> List.ofSeq

let writeHome () =
   let posts = 
      postFiles
      |> List.map(fun p -> p.Replace(source, "").Replace("index.fsx", ""))
      |> List.map mdLink
      |> List.map(fun l -> "- " + l)
      |> List.fold(fun a b -> a + "\r\n" + b) ""
   let home = Path.Combine(source, "index.md")
   let contents = File.ReadAllText(home)
   let pattern = "## Posts(.|\n)*"
   let m = Regex.Matches(contents, pattern)
   let replaced = 
      let text = sprintf "## Posts%s\r\n" posts
      Regex.Replace(contents, pattern, text)
   File.WriteAllText(home, replaced)

   Literate.ProcessMarkdown(home, template)

let processScript script = Literate.ProcessScriptFile(script, template)


writeHome()

postFiles
|> List.iter processScript


