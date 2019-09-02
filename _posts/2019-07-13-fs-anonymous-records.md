---
title: F# Anonymous Records
categories: [F#-Topics]
tags: [F#-Topics]
---

# F# Anonymous Records

I recently discovered F#'s new Anonymous Records as of version 4.6.

Normally, we would explicitly define a record such as:

```fsharp
type Player =
    {   id    : int
        name  : string
        }
```


But, with the new Anonymous Records, we can define these types on the fly as we need them.


let play (player: {| id: string; name: string |}) = 
    sprintf "%s runs and jumps." player.name


Anonymous Records may also be used in the place of tuples to provide names to the values.


```fsharp
let ambiguous = 1, "Tim"
let clear = {| id = 1; name = "Time" |}
```
