---
title: F# Precomputation Exploration
categories: [Exploration-PERF]
tags: []
---

# Precomputation Exploration

Create Test Data with Search for the Worst (Last) Value

```fsharp
open System

let data = [0 .. 1000000]
let search = 1000000
```


## List Search

```fsharp
#time
data |> List.contains (search)
#time
```


> Output:
```fsharp
Real: 00:00:00.101, CPU: 00:00:00.109, GC gen0: 29, gen1: 1, gen2: 1
val it : bool = true
```



## Set Search

```fsharp
let set = data |> Set.ofList
#time
set.Contains(search)
#time
```


> Output:
```fsharp
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val it : bool = true
```



## Precompute Set with Partial Application


```fsharp
let partiallyApplied (listToSearch: int list) (value: int) : bool = 
    let setToSearch = listToSearch |> Set.ofList
    setToSearch.Contains(value)

let searcher = partiallyApplied data
#time
searcher search
#time
```


> Output:
```fsharp
Real: 00:00:01.568, CPU: 00:00:01.562, GC gen0: 410, gen1: 13, gen2: 0
val it : bool = true
```



## Precompute Set with Function Computation


```fsharp
let returnFunction (listToSearch: int list) : int -> bool =
    let setToSearch = listToSearch |> Set.ofList
    fun v -> setToSearch.Contains(v)

let searcher' = returnFunction data
#time
searcher' search
#time
```


> Output:
```fsharp
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val it : bool = true
```



## Test Partial Application with Printer


```fsharp
let printer (items: int list) (item: int) : bool =
    printfn "Starting Partially Applied Printer"
    let set = items |> Set.ofList
    printfn "After Set Creation"
    let r = set.Contains item
    printfn "After Search"
    r
    
let printerTest = printer data
```

> Output:
```fsharp
val printerTest : (int -> bool)
```


```fsharp
#time
printerTest search
#time
```


> Output:
```fsharp
Starting Partially Applied Printer
After Set Creation
After Search
Real: 00:00:01.488, CPU: 00:00:01.468, GC gen0: 411, gen1: 13, gen2: 0
val it : bool = true
```



## Test Precomputation with Printer


```fsharp
let printer' (items: int list) : int -> bool =
    printfn "Starting Precomputation Printer"
    let set = items |> Set.ofList
    printfn "After Set Creation"
    let compute = fun v -> 
        printfn "Before Search"
        let r = set.Contains v
        printfn "After Search"
        r
    compute

let printerTest' = printer' data
```


> Output:
```fsharp
Starting Precomputation Printer
After Set Creation
val printerTest' : (int -> bool)
```


```fsharp
#time
printerTest' search
#time
```


> Output:
```fsharp
Before Search
After Search
Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val it : bool = true
```



## Conclusion

Precomputation is only applied when a new function is returned from a function.  Partial application does not precompute any encapsulated state.

