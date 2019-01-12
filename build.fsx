#load @"packages/FSharp.Formatting/FSharp.Formatting.fsx"

open System.IO
open System.Text.RegularExpressions
open FSharp.Literate
open FSharp.Markdown

// TODO: Inject JS Post Files into Corresponding Posts

let source = __SOURCE_DIRECTORY__
let template = Path.Combine(__SOURCE_DIRECTORY__, @"content/template.html")

let convertToPrevAndNextOption<'T> (items: 'T list) : ('T option * 'T * 'T option ) list = 
   match items with 
   | [] -> []
   | h :: [] -> [None, h, None]
   | _ -> 
      let max = items.Length - 1
      items
      |> List.mapi(
         fun i x -> 
            match i with 
            | 0 -> None, x, Some items.[1]
            | i when i < max -> Some items.[i - 1], x, Some items.[i + 1]
            | _ -> Some items.[i - 1], x, None)

let getPosts (fileName: string) : string list =
   Directory.GetFiles(Path.Combine(source, "posts"), fileName, SearchOption.AllDirectories)
   |> List.ofSeq
   |> List.filter(fun f -> f.Contains("_archive") |> not)

let convertToLink (path: string) : string * string = 
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
   path, sprintf "- [%s](%s)" dirs clean

let writeHome (indexes: string list) =
   let links = 
      indexes
      |> List.fold(fun a b -> a + "\r\n" + b) ""
   let home = Path.Combine(source, "index.md")
   let contents = File.ReadAllText(home)
   let pattern = "## Posts(.|\n)*"
   let replaced = 
      let text = sprintf "## Posts\r\n%s\r\n" links
      Regex.Replace(contents, pattern, text)
   File.WriteAllText(home, replaced)

   let replacements = ["navigation", ""]

   Literate.ProcessMarkdown(home, template, replacements = replacements)

let getIndexPathLinks() = 
   let all = 
      "index.*" 
      |> getPosts 
      |> List.map convertToLink
      |> List.sort
   
   all
   // Remove Built Html Files
   |> List.filter(
      fun (path, link) -> 
         if path.EndsWith(".html") |> not then true
         else 
            all
            |> List.exists(fun (_, link') -> link' = link)
            |> not
   )

let indexPathLinks = getIndexPathLinks()

let navigationLinks (prev: (string * string) option) (next: (string * string) option) = 
   let prev' = 
      match prev with 
      | None -> ""
      | Some (_, link) -> sprintf """<h4>Previous</h4>%s""" link
   let next' = 
      match next with 
      | None -> ""
      | Some (_, link) -> sprintf """<h4>Next</h4>%s""" link
   sprintf """
     <div class="row post-nav">
       <div class="col-md-6 post-nav-prev">
         %s
       </div>
       <div class="col-md-6 post-nav-next">
         %s
       </div>
     </div>""" prev' next'

indexPathLinks
|> List.map(fun (path, link) -> path, Markdown.TransformHtml link)
|> convertToPrevAndNextOption
|> List.iter(
   fun (prev, curr, next) -> 
      let path, _ = curr
      let replacements = ["navigation", (navigationLinks prev next)]
      match path with 
      | path when path.EndsWith(".fsx") -> 
         Literate.ProcessScriptFile(path, template, replacements = replacements)
      | path when path.EndsWith(".md") -> 
         Literate.ProcessMarkdown(path, template, replacements = replacements)
      | _ -> ()
)

let indexes = indexPathLinks |> List.map(fun (_, index) -> index)
writeHome indexes
