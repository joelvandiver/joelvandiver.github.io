module App

open Fable.Core
open Fable.Core.JsInterop
open Fable.Import
open Fable.Import.Browser
open Fable.Import.React
open Fable.Helpers.React
open Fable.Helpers.React.Props

// Ref:  https://blog.vbfox.net/2018/02/06/fable-react-1-react-in-fable-land.html


module StatelessExample = 
    type WelcomeProps = { name: string }

    let Welcome { name = name } =
        h1 [] [ str "Hello, "; str name ]

    let inline welcome name = ofFunction Welcome { name = name } []


module StateExample = 
    // A pure, stateless component that will simply display the counter
    type CounterDisplayProps = { counter: int }

    type CounterDisplay(initialProps) =
        inherit PureStatelessComponent<CounterDisplayProps>(initialProps)
        override this.render() =
            div [] [ str "Counter = "; ofInt this.props.counter ]

    let inline counterDisplay p = ofType<CounterDisplay,_,_> p []

    // Another pure component displaying the buttons
    type AddRemoveProps = { add: MouseEvent -> unit; remove: MouseEvent -> unit }

    type AddRemove(initialProps) =
        inherit PureStatelessComponent<AddRemoveProps>(initialProps)
        override this.render() =
            div [] [
                button [OnClick this.props.add] [str "👍"]
                button [OnClick this.props.remove] [str "👎"]
            ]

    let inline addRemove props = ofType<AddRemove,_,_> props []

    // The counter itself using state to keep the count
    type CounterState = { counter: int }

    type Counter(initialProps) as this =
        inherit Component<obj, CounterState>(initialProps)
        do
            this.setInitState({ counter = 0})

        // This is the equivalent of doing `this.add = this.add.bind(this)`
        // in javascript (Except for the fact that we can't reuse the name)
        let add = this.Add
        let remove = this.Remove

        member this.Add(_:MouseEvent) =
            this.setState({ counter = this.state.counter + 1 })

        member this.Remove(_:MouseEvent) =
            this.setState({ counter = this.state.counter - 1 })

        override this.render() =
            div [] [
                counterDisplay { CounterDisplayProps.counter = this.state.counter }
                addRemove { add = add; remove = remove }
            ]

    let inline counter props = ofType<Counter,_,_> props []




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
                li [] [ StatelessExample.welcome "🌍" ]
                li [] [ StateExample.counter createEmpty ]
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