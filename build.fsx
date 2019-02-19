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
open Fable.Helpers.React
open Fable.Helpers.React.Props
// open FSharp.Markdown
open FSharp.Literate
open Fake.Core

// let root = __SOURCE_DIRECTORY__ + @"\..\posts"
let root = @"C:\git\joelvandiver.github.io\posts"

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
              Src "/posts/Fun/Art/Fractals/9.29.17.svg"
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

let parse source =
    let doc = 
      let fsharpCoreDir = @"-I:C:\git\joelvandiver.github.io\packages\FSharp.Core\lib\netstandard1.6\FSharp.Core.dll"
      let systemRuntime = "-r:System.Runtime"
      Literate.ParseScriptString(
                  source,
                  compilerOptions = systemRuntime + " " + fsharpCoreDir,
                  fsiEvaluator = FSharp.Literate.FsiEvaluator([|fsharpCoreDir|])
                )
    FSharp.Literate.Literate.FormatLiterateNodes(doc, OutputKind.Html, "", true, true)
let format (doc: LiterateDocument) =
    Formatting.format doc.MarkdownDocument true OutputKind.Html

let convertFSXPost content =
  { title = "Blog"
    content = content |> parse |> format }
  |> template
  |> render 

Target.create "Build" (fun _ -> 
  Directory.GetFiles(root, "*.fsx", SearchOption.AllDirectories)
  |> List.ofSeq
  |> List.map(fun f -> (f, File.ReadAllText f))
  |> List.map(
    fun (f, content) -> 
      printfn "%s" f
      let path = f.Replace(".fsx", ".html")
      let post = convertFSXPost content
      (path, post)
  )
  |> List.iter File.WriteAllText
)

Target.runOrDefault "Build"