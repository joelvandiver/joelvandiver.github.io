#r "paket:
source https://api.nuget.org/v3/index.json
source https://ci.appveyor.com/nuget/fsharp-formatting

nuget Fake.IO.FileSystem
nuget Fake.Core.Trace
nuget FSharp.Data
nuget Fable.React
nuget FSharp.Literate //" 

#load ".fake/build.fsx/intellisense.fsx"

#if !FAKE
  #r "netstandard"
#endif

open System.IO
open Fable.Helpers.React
open Fable.Helpers.React.Props
open FSharp.Markdown
open FSharp.Literate

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

let index =
    { title = "Joel Vandiver's Blog"
      content = Markdown.TransformHtml "# **interesting** things" }
    |> template
    |> render 

File.WriteAllText(__SOURCE_DIRECTORY__ + @"\index.html", index)

