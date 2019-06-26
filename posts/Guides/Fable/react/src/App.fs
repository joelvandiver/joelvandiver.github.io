module App

open Fable.Core
open Fable.Core.JsInterop
open Fable.Import
open Browser.Types
open Browser.Dom
open Fable.React
open Fable.React.Props
open Fable.Core.Util
open Fable.Core.Extensions

// Ref:  https://blog.vbfox.net/2018/02/06/fable-react-1-react-in-fable-land.html

module ReactHookExample =
    let view =
        FunctionComponent.Of(fun (props: {| initCount: int |}) ->
        let state = Hooks.useState(props.initCount) // This is where the magic happens
        button
            [ OnClick (fun _ -> state.update(fun s -> s + 1)) ]
            [ str "Times clicked: "; ofInt state.current ]
        )

module StatelessExample = 
    let welcome =    
        FunctionComponent.Of(fun (props: {| message: string |}) ->
            span [] [str props.message])

module StateExample = 
    // A pure, stateless component that will simply display the counter
    type CounterDisplayProps = { counter: int }

    type CounterDisplay(initialProps) =
        inherit PureStatelessComponent<CounterDisplayProps>(initialProps)
        override this.render() =
            span [] [ str "Counter = "; ofInt this.props.counter ]

    let inline counterDisplay p = ofType<CounterDisplay,_,_> p []

    // Another pure component displaying the buttons
    type AddRemoveProps = { add: MouseEvent -> unit; remove: MouseEvent -> unit }

    type AddRemove(initialProps) =
        inherit PureStatelessComponent<AddRemoveProps>(initialProps)
        override this.render() =
            span [] [
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
            this.setState(fun counterState -> fun t -> { counter = counterState.counter + 1 })

        member this.Remove(_:MouseEvent) =
            this.setState(fun counterState -> fun t -> { counter = counterState.counter - 1 })

        override this.render() =
            span [] [
                counterDisplay { CounterDisplayProps.counter = this.state.counter }
                addRemove { add = add; remove = remove }
            ]

    let inline counter props = ofType<Counter,_,_> props []

let init() =
    let element =
        ul
            [ClassName "my-ul"; Id "unique-ul"]
            [
                li [] [ str "React Hook Example -> "; ReactHookExample.view {| initCount = 0 |} ]                
                li [] [ str "Stateless Example -> "; StatelessExample.welcome {| message = "🌍" |} ]
                li [] [ str "State Example -> "; StateExample.counter createEmpty ]
                li [] [ str "String Example -> "; str "Hello 🌍" ]
                li [] [ str "ofInt Example -> "; str "The answer is: "; ofInt 42 ]
                li [] [ str "ofFloat Example -> "; str "π="; ofFloat 3.14 ]
                li [] [ str "ofOption (null or something) Example -> "; str "🤐"; ofOption (Some (str "🔫")) ]
                li [] [ str "ofOption (render nothing) Example -> "; str "😃"; ofOption None ]
                li [] [ str "ofList Example -> "
                        ul []
                            [ [1;2;3]
                                |> List.map(fun i ->
                                    let si = i.ToString()
                                    li [Key si] [str "🎯 "; str si])
                                |> ofList ] ]

                li [] 
                    [ str "React Fragment Example -> "
                      ul []
                        [ [1;2;3]
                            |> List.map(fun i ->
                                let si = i.ToString()
                                li [] [str "🎲 "; str si])
                            |> fragment [] ] ] ]
    ReactDom.render(element, document.getElementById("root"))

init()