#r "paket:
source https://api.nuget.org/v3/index.json
source https://ci.appveyor.com/nuget/fsharp-formatting

nuget Fake.IO.FileSystem
nuget Fake.Core.Trace
nuget FSharp.Data
nuget Fable.React
nuget FSharp.Literate //" 

#load ".fake/build.fsx/intellisense.fsx"

open FSharp.Literate

let md = """# Markdown is cool
especially with *FSharp.Formatting* ! """
            |> FSharp.Markdown.Markdown.TransformHtml

let snipet  =
    """
    (** # *F# literate* in action *)
    printfn "Hello"
    """
let parse source =
    let doc = 
      let fsharpCoreDir = "-I:" + __SOURCE_DIRECTORY__ + @"\..\lib"
      let systemRuntime = "-r:System.Runtime"
      Literate.ParseScriptString(
                  source, 
                  compilerOptions = systemRuntime + " " + fsharpCoreDir,
                  fsiEvaluator = FSharp.Literate.FsiEvaluator([|fsharpCoreDir|]))
    FSharp.Literate.Literate.FormatLiterateNodes(doc, OutputKind.Html, "", true, true)
let format (doc: LiterateDocument) =
    Formatting.format doc.MarkdownDocument true OutputKind.Html
let fs =
    snipet 
    |> parse
    |> format