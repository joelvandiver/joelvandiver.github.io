module Fable.StaticPageGenerator.Main

open Fable.Core.JsInterop
open Fable.Import
open Fable.Helpers.React
open Helpers
open Fulma

type IPerson =
    abstract firstName: string
    abstract familyName: string
    abstract birthday: string

// Make sure to always resolve paths to avoid conflicts in generated JS files
// Check fable-splitter README for info about ${entryDir} macro

let templatePath = resolve "${entryDir}/../templates/template.hbs"
let markdownPath = resolve "${entryDir}/../markdown/index.md"
let dataPath = resolve "${entryDir}/../data/people.json"
let indexPath = resolve "${entryDir}/../index.html"

let createTable() =
    let createHead (headers: string list) =
        thead [] [
            tr [] [for header in headers do
                    yield th [] [str header]]
        ]
    let people =
        readFile dataPath
        |> JS.JSON.parse
        |> unbox<IPerson array>
    div [] [
        hr []
        p [] [str ("The text above has been parsed from markdown, " +
                    "the table below is generated from a React component.")]
        Table.table [ Table.IsStriped ] [
            createHead ["First Name"; "Family Name"; "Birthday"]
            tbody [] [
                for person in people do
                    yield tr [] [
                        td [] [str person.firstName]
                        td [] [str person.familyName]
                        td [] [str person.birthday]
                    ]
            ]
        ]
    ]

let render() =
    [ "title" ==> "Joel Vandiver"
      "body" ==> parseMarkdown markdownPath
      "data" ==> (createTable() |> parseReactStatic) ]
    |> parseTemplate templatePath
    |> writeFile indexPath

render()
