---
title: Use F# Lazy Evaluation for Integration Testing
categories: [F#-Guide]
tags: []
---

F#'s `lazy` keyword is incredibly powerful at encapsulating expensive operations in integration testing.

First, let's explore the keyword.

```fsharp
let x = lazy (5 + 4)
```

> Output:

```fsharp
val x : Lazy<int> = Value is not created.
```

Note that we do not have a value yet. To get the value out of `x`, you have to `Force` it to do the work.

```fsharp
let y = x.Force()
```

> Output:

```fsharp
val y : int = 9
```

Just as with a `lazy` persion, you have to `Force` them to do some work.

The `lazy` isn't very helpful for simple values such as `9` though.

What's actually going on behind the scenes with the `Force`? Let's set up an example that includes a print in the worker so that we can see the what's happening in the work.

```fsharp
let mutable workNumCount = 0
let workNum () =
    printfn "Starting Num %i" workNumCount
    workNumCount <- workNumCount + 1
    workNumCount

let numVal = lazy (workNum())
```

> Output:

```fsharp
val mutable workNumCount : int = 1
val workNum : unit -> int
val numVal : Lazy<int> = 1
```

- What will happen if we were to `Force` `numVal` to evaluate multiple times?
- How many print statements would there be?
- What would the value of `workNumCount` be on each successive call?

```fsharp
let numVals =
    [
        numVal
        numVal
        numVal
        numVal
        numVal
        numVal
        numVal
        numVal
    ]
    |> List.map(fun numVal -> numVal.Force())
```

> Output:

```fsharp
Starting Num 0
val numVals : int list = [1; 1; 1; 1; 1; 1; 1; 1]
```

open System
open System.IO
open System.Net

let uri = @"http://dash/"

let mutable count = 1

let fetch (uri: string) =
printfn "Request #%i @ %A" count DateTime.UtcNow
count <- count + 1

let response =
let mutable req = WebRequest.CreateHttp(uri);
req.Method <- "GET"
req.ContentLength <- 0L
req.ContentType <- "application/json"
req.UseDefaultCredentials <- true;
req.UserAgent <- "GCS API";
req.AuthenticationLevel <- System.Net.Security.AuthenticationLevel.MutualAuthRequested;
req.Credentials <- System.Net.CredentialCache.DefaultNetworkCredentials;
req.GetResponse()
let reader = new StreamReader(response.GetResponseStream())
reader.ReadToEnd()

let response = lazy (fetch uri)

printfn "%A" response

let calls =
[
response
response
response
response
response
]
|> List.map(
fun response ->
response.Force()
)

printfn "%A" calls

```

```
