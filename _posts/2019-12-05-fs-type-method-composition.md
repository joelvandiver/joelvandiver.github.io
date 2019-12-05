---
title:  F# Type Method Composition
categories: [F#-Topics]
tags: []
---

# F# Type Method Composition

I regularly implement F# function composition with function values.  This feature of the language has helped me to focus on the building blocks of an application.  Usually, I will compose those simpler functions into more complex functions at the boundary to the system.

A question came to my mind yesterday about applying function composition with type members.  This would allow me to encapsulate state behind the function in the composition.  I would then be free to implement interfaces of external connections to decouple it from the function itself.  I can then inject the required dependency of that interface later in the stack.

Let's take a look.

```fsharp
open System

type IDependency =
    abstract log: string -> unit
    abstract connection: unit -> IDisposable

type RequiresDependency(dependency: IDependency) =
    member __.GetNumbers() =
        use connection = dependency.connection()
        dependency.log ("Getting data.")
        // ... go get data
        [ 1; 2; 3; 4 ]

let multiplyBy2 (data: int list) = data |> List.map ((*) 2)

let logger =
    { new IDependency with
        member __.log text = printfn "%s" text
        member __.connection() =
            { new IDisposable with
                member __.Dispose() = printfn "Disposing connection." } }

let requiresDependency = RequiresDependency logger
let getAndMultiplyBy2 = requiresDependency.GetNumbers >> multiplyBy2
let expected = [ 2; 4; 6; 8 ]
let actual = getAndMultiplyBy2()

expected = actual
```

> Output

```fsharp
type IDependency =
  interface
    abstract member connection : unit -> System.IDisposable
    abstract member log : string -> unit
  end
type RequiresDependency =
  class
    new : dependency:IDependency -> RequiresDependency
    member GetNumbers : unit -> int list
  end

val multiplyBy2 : data:int list -> int list
val logger : IDependency
val requiresDependency : RequiresDependency
val getAndMultiplyBy2 : (unit -> int list)
val expected : int list = [2; 4; 6; 8]

Getting data.
Disposing connection.
val actual : int list = [2; 4; 6; 8]
val it : bool = true
```