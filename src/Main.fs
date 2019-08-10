module Main

open Fable.Import
open Fable.Helpers.React
open Fable.Helpers.React.Props
open StaticWebGenerator
open Fulma

type IPerson =
    abstract firstName: string
    abstract familyName: string
    abstract birthday: string

// Make sure to always resolve paths to avoid conflicts in generated JS files
// Check fable-splitter README for info about ${entryDir} macro

let markdownPath = IO.resolve "${entryDir}/../posts/index.md"
let indexPath = IO.resolve "${entryDir}/../deploy/index.html"
let filePaths = IO.resolve "${entryDir}/../data/files.json"

let readFiles() =
    let files =
        IO.readFile filePaths
        |> JS.JSON.parse
        |> unbox<string array>
    div [] [
        for file in files do
            yield IO.readFile file |> parseMarkdownAsReactEl "content"
    ]

let frame titleText content data =
    let cssLink path =
        link [ Rel "stylesheet"; Type "text/css"; Href path ]
    html [] [
        head [] [
            yield title [] [str titleText]
            yield meta [ HTMLAttr.Custom ("httpEquiv", "Content-Type")
                         HTMLAttr.Content "text/html; charset=utf-8" ]
            yield meta [ Name "viewport"
                         HTMLAttr.Content "width=device-width, initial-scale=1" ]
            yield cssLink "https://maxcdn.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css"
            yield cssLink "https://cdnjs.cloudflare.com/ajax/libs/bulma/0.5.1/css/bulma.min.css"
        ]
        body [] [
            Fulma.Container.container [] [
                content
                data
            ]
        ]
    ]

let render() =
    let content =
        IO.readFile markdownPath
        |> parseMarkdownAsReactEl "content"
    let data =
        readFiles()
    frame "My page" content data
    |> parseReactStatic
    |> IO.writeFile indexPath
    printfn "Render complete!"

render()
