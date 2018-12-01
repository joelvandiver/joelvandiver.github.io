#r @"C:\git\joelvandiver.github.io\packages\FSharp.Formatting\lib\net40\FSharp.Markdown.dll"

open System.IO
open FSharp.Core
open FSharp.Markdown
open HandlebarsDotNet

let root = __SOURCE_DIRECTORY__
let postPath = root + @"\posts"
let indexMarkdown = root + @"\index.md"
let indexTemplate = root + @"\index.hbs"
df
type FileText = 
    {  file : string
       path : string }

let readFileText (path: string)  = 
  { file = File.ReadAllText(path)
    path = path }

let postMds = 
  indexMarkdown :: (Directory.GetFiles(postPath, "*.md", SearchOption.AllDirectories) |> List.ofSeq)
  |> List.map readFileText
  
let private marked (markdown: string): string = markdown |> Markdown.Parse |> Markdown.WriteHtml

let parseMarkedFileText (ft: FileText) : FileText =
  { file = ft.file |> marked
    path = ft.path.Replace(".md", ".html")}

let parsed = postMds |> List.map parseMarkedFileText


