---
title:  Reflection of Types
categories: [Exploration-PERF]
tags: []
---

# Reflection of Types

```fsharp
open System

type SomeTypeToReflect = 
   {  id      : float
      title   : string
      created : int64 }

let instances = 
   { 0. .. 100000000000000000. }
   |> Seq.map(fun i -> { id = i; title = sprintf "#%f" i; created = DateTime.UtcNow.Ticks })
```

*Output:*
```console
type SomeTypeToReflect =
  {id: float;
   title: string;
   created: int64;}
val instances : seq<SomeTypeToReflect>
```






## Reflecting Each Instance

```fsharp
#time
let reflectEach =
   instances 
   |> Seq.map(
      fun x -> 
         let props = x.GetType().GetProperties()
         let values = props |> Seq.map(fun prop -> (prop.Name, prop.GetValue(x)))
         values
   )
#time
printfn "%A" reflectEach
```

*Output:*
```console
--> Timing now on

Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val reflectEach : seq<seq<string * obj>>


--> Timing now off

seq
  [seq [("id", 0.0); ("title", "#0.000000"); ("created", 636847300273895338L)];
   seq [("id", 1.0); ("title", "#1.000000"); ("created", 636847300273905300L)];
   seq [("id", 2.0); ("title", "#2.000000"); ("created", 636847300273915271L)];
   seq [("id", 3.0); ("title", "#3.000000"); ("created", 636847300273915271L)];
   ...]
val it : unit = ()
```






## Reflecting the Type

```fsharp
#time
let reflectOnce  =
   let props = typeof<SomeTypeToReflect>.GetProperties()
   instances 
   |> Seq.map(
      fun x -> 
         let values = props |> Seq.map(fun prop -> (prop.Name, prop.GetValue(x)))
         values
   )
#time
printfn "%A" reflectOnce
```


*Output:*
```console
--> Timing now on

Real: 00:00:00.000, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0
val reflectOnce : seq<seq<string * obj>>


--> Timing now off

seq
  [seq [("id", 0.0); ("title", "#0.000000"); ("created", 636847300392606574L)];
   seq [("id", 1.0); ("title", "#1.000000"); ("created", 636847300392616546L)];
   seq [("id", 2.0); ("title", "#2.000000"); ("created", 636847300392626512L)];
   seq [("id", 3.0); ("title", "#3.000000"); ("created", 636847300392626512L)];
   ...]
val it : unit = ()
```



