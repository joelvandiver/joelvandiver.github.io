---
title:  F# ASP.NET Core Dependency Injection
categories: [F#-Topics]
tags: [ASP.NETCore]
---

# F# ASP.NET Core Dependency Injection

ASP.NET Core has a nice dependency injection system that works great for object-oriented languages like C#.  F# can be written in an object-oriented way, but is there a way to inject dependencies in a functional style?

First off, let's review the F# function composition that comes out-of-the-box.  For example, 

```fsharp
let f x = x + 3
let g x = x + 5

let fg = f >> g

// Output
val f : x:int -> int
val g : x:int -> int
val fg : (int -> int)
```

`f` and `g` are functions taking one integer and returning another integer.  The output of `f` is an integer which is the same type of input for `g`.  This allows us to compose `f >> g`.

Note the sample data below.

```fsharp
[1;2;3;4;5] |> List.map fg
val it : int list = [9; 10; 11; 12; 13]
```

Let's expand on the idea to a larger domain than Mathematics.

```fsharp
type User = { first: string; last: string }
let getUserName (user: User) = user.first + " " + user.last
let getFirstUser (users: User list) : User = users |> List.head
let getFirstUserName = getFirstUser >> getUserName

// Output
val getUserName : user:User -> string
val getFirstUser : users:User list -> User
val getFirstUserName : (User list -> string)
```

Here, we have a `getUserName` function that takes a `User` and another function `getFirstUserName` that returns a `User`.  This allows us to compose a new function `getFirstUserName` from `getFirstUser >> getUserName`.  

The lego-brick like functionality is very useful for defining more complex functionalities from little building blocks.

In the course of developing ASP.NET, I have found that the API usually sits on top of a fairly large codebase.  It is useful to compose more complex functions just before creating API constrollers.  This can be solved by injecting dependencies into controllers using the `IControllerActivator` interface.  This interface is effectively an override of the default dependency injection system.

Here's a sample to get you going with an activator.  I usually put the activator and controllers in the same module, since the controller declares the dependencies and the activator composes those dependencies.

```fsharp
[<ApiController>]
[<Route("/api/[controller]")>]
type UserController (getLastCreatedUserName: unit -> string) =
    inherit ControllerBase()

    [<HttpGet>]
    member __.Get() = getLastCreatedUserName()

type MyActivator (config: IConfiguration) = 
    interface IControllerActivator with
        member __.Create(context: ControllerContext) =
            let controllerType = context.ActionDescriptor.ControllerTypeInfo.AsType()
            match controllerType with 
            | c when c = typeof<UserController> -> 
                // Function Composition
                let getLastCreatedUserName = 
                    getUserList
                    >> getFirstUser
                    >> getUserName
                // Dependency Injection
                UserController(getLastCreatedUserName) 
                    |> box
            | _ -> failwith (sprintf "The controller type, %s, could not be found." controllerType)
        
```

Lastly, you will need to register this `MyActivator` in the `ConfigureServices` method of `Startup`.
```fsharp
services.AddSingleton<IControllerActivator>(new Activator(configuration))
```

That's it!  This helps to keep your core library functional while using object oriented techniques for the controllers and startup only.  Very nice!