---
title: F# Anonymous Records
categories: [Fs-Topics]
tags: [Fs-Topics]
---

# F# Anonymous Records

I recently discovered F#'s new Anonymous Records as of version 4.6.

Let's take a look.

```fsharp
// Normally, we would explicitly define a record such as:
type Player =
    {   id    : int
        name  : string
        }

// But, with the new Anonymous Records, we can define these types on the fly as we need them.
let play (player: {| id: string; name: string |}) = 
    sprintf "%s runs and jumps." player.name

let tim_ambiguous = 1, "Tim"
// Anonymous Records may also be used in the place of tuples to provide names to the values.
let tim_clear = {| id = 1; name = "Tim" |}
```
