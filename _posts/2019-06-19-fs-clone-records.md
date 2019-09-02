---
title: F# Cloning Records
categories: [Guide-F#]
tags: []
---

# Cloning Records

F# supports a nice syntax for cloning records into new records.


```fsharp
type User = 
    {   id    : int
        name  : string 
        title : string
        }

let bob = { id = 1; name = "Bob"; title = "Sr."}
let bobJr = { bob with id = 2; title = "Jr." }
```


> Output:
```fsharp
type User =
  {id: int;
   name: string;
   title: string;}
val bob : User = {id = 1;
                  name = "Bob";
                  title = "Sr.";}
val bobJr : User = {id = 2;
                    name = "Bob";
                    title = "Jr.";}
```

F# also supports deep clones of nested records.


```fsharp
type Child = 
    {   id   : int
        name : string
        }

type Parent = 
    {   id       : int
        name     : string
        children : Child list
        }

let mom = 
    {   id       = 1
        name     = "Mom"
        children = 
            [
                { id = 1; name = "Bob" }
                { id = 2; name = "Sarah" }
                { id = 3; name = "Tim" }
            ]}

let dad = 
    { mom with id = 2; name = "Dad"}    
```


> Output:
```fsharp
type Child =
  {id: int;
   name: string;}
type Parent =
  {id: int;
   name: string;
   children: Child list;}
val mom : Parent =
  {id = 1;
   name = "Mom";
   children = [{id = 1;
                name = "Bob";}; {id = 2;
                                 name = "Sarah";}; {id = 3;
                                                    name = "Tim";}];}
val dad : Parent =
  {id = 2;
   name = "Dad";
   children = [{id = 1;
                name = "Bob";}; {id = 2;
                                 name = "Sarah";}; {id = 3;
                                                    name = "Tim";}];}
```
