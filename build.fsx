#r "paket:
source https://api.nuget.org/v3/index.json
source https://ci.appveyor.com/nuget/fsharp-formatting

nuget Fake.Core.Target
nuget Fake.IO.FileSystem
nuget Fake.Core.Trace
nuget FSharp.Data
nuget Fable.React
nuget FSharp.Literate //" 

#if !FAKE
  #r "netstandard"
#endif

#load ".fake/build.fsx/intellisense.fsx"

open System.IO
open System.Text.RegularExpressions
open Fable.Helpers.React
open Fable.Helpers.React.Props
open FSharp.Markdown
open FSharp.Literate
open Fake.Core
open Fake.Core.TargetOperators

// let root = __SOURCE_DIRECTORY__ + @"\..\posts"
let root = @"C:\git\joelvandiver.github.io\posts\"
let sections = 
    [
        "Daily"
        "Explorations"
        "Snippets"
    ]

type Post = {
  title: string
  content: string
}

let template post = 
  html [Lang "en"] [
      head [] [
          title [] [ str ("Joel Vandiver / " + post.title) ]
          meta [CharSet "utf-8"]
          meta [
            HttpEquiv "X-UA-Compatible"
          ]
          link [
            Rel "shortcut icon"
            Href "/assets/img/favicon.ico"
            Type "image/x-icon"
          ]
          link [
            Rel "stylesheet"
            Href "/assets/vendor/fontawesome-free/css/all.min.css"
          ]
          link [
            Rel "stylesheet"
            Type "text/css"
            Href "/assets/css/site.css"
          ]
      ]
      body [
        Class "container"
      ] [
          div [
            Class "sidebar"
          ] [
            h1 [] [
              a [
                Href "/"
              ] [
                Text "Joel Vandiver"
              ]
            ]
            img [
              Src "/assets/9.29.17.svg"
            ]
          ]
          div [
            Class "main"
          ] [
            RawText post.content
            script [Src "/assets/vendor/jquery/jquery.min.js"] []
            script [Src "/assets/vendor/bootstrap/js/bootstrap.bundle.min.js"] []
            script [Src "/assets/vendor/jquery-easing/jquery.easing.min.js"] []
            script [Src "/assets/js/literate.js"] []
          ]
      ]
  ]

let render html =
  fragment [] [ 
    RawText "<!DOCTYPE html>"
    RawText "\n" 
    html ]
  |> Fable.Helpers.ReactServer.renderToString 

let fsharpCoreDir = @"-I:C:\git\joelvandiver.github.io\packages\FSharp.Core\lib\netstandard1.6\"
let systemRuntime = "-r:System.Runtime"
let parseFSX source =
    let doc = 
      Literate.ParseScriptString(
                  source,
                  compilerOptions = systemRuntime + " " + fsharpCoreDir,
                  fsiEvaluator = FSharp.Literate.FsiEvaluator([|fsharpCoreDir|])
                )
    FSharp.Literate.Literate.FormatLiterateNodes(doc, OutputKind.Html, "", true, true)

let parseMD source = 
    let doc = 
        Literate.ParseMarkdownString(
            source,
            compilerOptions = systemRuntime + " " + fsharpCoreDir,
            fsiEvaluator = FSharp.Literate.FsiEvaluator([|fsharpCoreDir|])
        )
    FSharp.Literate.Literate.FormatLiterateNodes(doc, OutputKind.Html, "", true, true)

let format (doc: LiterateDocument) = Formatting.format doc.MarkdownDocument true OutputKind.Html

let convertFSXPost content =
  { title = "Blog"
    content = content |> parseFSX |> format }
  |> template
  |> render 

let convertMDPost content =
  { title = "Blog"
    content = content |> parseMD |> format }
  |> template
  |> render 

let convertToLink (path: string) : string = 
   let source = __SOURCE_DIRECTORY__
   let rel = 
        path.Replace(source, "")
            .Replace("\\index.fsx", "")
            .Replace("\\index.html", "")
            .Replace("\\index.md", "")
   let dirs = 
        rel.Replace("\\posts\\", "")
           .Replace("\\", " - ")
           .Replace(" Fs", " F#")
   let clean = rel.Replace("\\", "/")
   sprintf "- [%s](%s)" dirs clean

Target.create "Build" (fun _ -> 
  (List.map ((fun f -> (f, File.ReadAllText f)) >> (fun (f, content) -> 
      printfn "%s" f
      let path = f.Replace(".fsx", ".html")
      let post = convertFSXPost content
      (path, post))) (Directory.GetFiles(root, "*.fsx", SearchOption.AllDirectories)
  |> List.ofSeq))
  |> List.iter File.WriteAllText
)

Target.create "Section" (fun _ -> 
    let addSectionLinks folder =
        let sectionMD = folder + @"\index.md"
        let sectionHtml = folder + @"\index.html"
        let html = Directory.GetFiles(folder, "*.html", SearchOption.AllDirectories) |> List.ofSeq
        let links = 
            html
            |> List.filter((<>) sectionHtml)
            |> List.map convertToLink
            |> List.fold(fun a b -> a + "\r\n" + b) ""
        let contents = File.ReadAllText(sectionMD)
        let pattern = "## (.*?)\r\n(.|\n)*"
        let replaced = 
            let text = sprintf "## $1\r\n%s\r\n" links
            Regex.Replace(contents, pattern, text)
        File.WriteAllText(sectionMD, replaced)
        let content = File.ReadAllText sectionMD |> convertMDPost
        File.WriteAllText(sectionHtml, content)
    sections 
    |> List.map ((+) root)
    |> List.iter addSectionLinks
    ()
)

"Build"
  ==> "Section"

Target.runOrDefault "Section"