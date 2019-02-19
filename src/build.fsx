#r "paket:
source https://api.nuget.org/v3/index.json
source https://ci.appveyor.com/nuget/fsharp-formatting

nuget FSharpVSPowerTools.Core
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
open FSharp.Markdown
open FSharp.Literate

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
      ]
      body [] [
          RawText post.content
      ]
  ]

let render html =
  fragment [] [ 
    RawText "<!doctype html>"
    RawText "\n" 
    html ]
  |> Fable.Helpers.ReactServer.renderToString 

let parse source =
    let doc = 
      // let fsharpCoreDir = "-I:" + __SOURCE_DIRECTORY__ + @"\..\lib"
      // let systemRuntime = "-r:System.Runtime"
      Literate.ParseScriptString(
                  source
                  // compilerOptions = systemRuntime + " " + fsharpCoreDir,
                  // fsiEvaluator = FSharp.Literate.FsiEvaluator([|fsharpCoreDir|])
                )
    FSharp.Literate.Literate.FormatLiterateNodes(doc, OutputKind.Html, "", true, true)
let format (doc: LiterateDocument) =
    Formatting.format doc.MarkdownDocument true OutputKind.Html

let convertFSXPost content =
  { title = "Joel Vandiver's Blog"
    content = content |> parse |> format
      }
  |> template
  |> render 

let posts = 
  Directory.GetFiles(root, "*.fsx", SearchOption.AllDirectories)
  |> List.ofSeq
  |> List.filter(fun f -> f.Contains("_archive") |> not)
  |> List.map(
    fun f -> 
      let path = f.Replace(".fsx", ".html")
      let content = File.ReadAllText f
      let post = convertFSXPost content
      (path, post)
  )


let work () = posts |> List.iter File.WriteAllText
