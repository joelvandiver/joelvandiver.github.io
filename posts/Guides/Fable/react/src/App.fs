module App

open Fable.Core
open Fable.Core.JsInterop
open Fable.Import
open Fable.Import.Browser
open Fable.Import.React
open Fable.Helpers.React
open Fable.Helpers.React.Props

type WelcomeProps = { name: string }

let Welcome { name = name } =
    h1 [] [ str "Hello, "; str name ]

let inline welcome name = ofFunction Welcome { name = name } []

let init() =
    let element =
        // Each HTML element has an helper with the same name
        ul
            // The first parameter is the properties of the elements.
            // For html elements they are specified as a list and for custom
            // elements it's more typical to find a record creation
            [ClassName "my-ul"; Id "unique-ul"]

            // The second parameter is the list of children
            [
                li [] [ welcome "🌍" ]
                // str is the helper for exposing a string to React as an element
                li [] [ str "Hello 🌍" ]

                // Helpers exists also for other primitive types
                li [] [ str "The answer is: "; ofInt 42 ]
                li [] [ str "π="; ofFloat 3.14 ]

                // ofOption can be used to return either null or something
                li [] [ str "🤐"; ofOption (Some (str "🔫")) ]
                // And it can also be used to unconditionally return null, rendering nothing
                li [] [ str "😃"; ofOption None ]

                // ofList allows to expose a list to react, as with any list of elements
                // in React each need an unique and stable key
                [1;2;3]
                    |> List.map(fun i ->
                        let si = i.ToString()
                        li [Key si] [str "🎯 "; str si])
                    |> ofList

                // fragment is the <Fragment/> element introduced in React 16 to return
                // multiple elements
                [1;2;3]
                    |> List.map(fun i ->
                        let si = i.ToString()
                        li [] [str "🎲 "; str si])
                    |> fragment []
            ]
    ReactDom.render(element, document.getElementById("root"))

init()