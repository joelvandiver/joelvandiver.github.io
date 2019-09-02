---
title: Test with Clone and Generator
categories: [F#-Topics]
tags: []
---

# Test with Clone and Generator
04-06-2019

One of the primary goals of each unit test is to ensure that the test code is as decoupled from the source code as possible.  Usually, the test should only have code specific to its concerns.  

> **Key Problem:** Generating test data is usually where the largest amount of coupling to production code exits.

Let's setup some code to test.

```fsharp
open System

type Role =
| Senator
| Jedi
| Sith

type Character =
    {   id          : Guid
        firstname   : string
        lastname    : string
        role        : Role
        }

let isSithLord character = character.role = Sith
```


> **Requirement**:  isSithLord should return true if the role is Sith.

## Creating Test Data in the Test


```fsharp
let testA = 
    let palpatine =
        {   id          = Guid.NewGuid()
            firstname   = "Darth"
            lastname    = "Sidious"
            role        = Sith
            }
    printfn "%A" palpatine
    let actual = isSithLord palpatine
    printfn "Palpatine is a Sith Lord:  %b" actual
    actual
```


> Output:
```fsharp
{id = 489c7a10-f872-4cfa-a79f-cf0e48fbb2d1;
 firstname = "Darth";
 lastname = "Sidious";
 role = Sith;}
Palpatine is a Sith Lord:  true
val testA : bool = true
```

Key Points:

1. The data for the test (palpatine) was created within the test itself.  
2. The only necessary data to test is that the role of Palpatine is Sith.




## Abstracting Test Data Creation into a Generator



```fsharp
let generateCharacter () =
    {   id          = Guid.NewGuid()
        firstname   = "Some First Name"
        lastname    = "Some Last Name"
        role        = Senator
        }

let testB = 
    let character =
        { generateCharacter() with  
            role = Sith
            }
    printfn "%A" character
    let actual = isSithLord character
    printfn "A Sith Character is a Sith Lord:  %b" actual
    actual
```


> Output:
```fsharp
{id = 1aa8abac-57e4-4fcd-ac15-f0f05509f8cf;
 firstname = "Some First Name";
 lastname = "Some Last Name";
 role = Sith;}
A Sith Character is a Sith Lord:  true
val generateCharacter : unit -> Character
val testB : bool = true
```

Key Points:

1. Generating the data has been abstracted in a utility function.
2. testB is only coupled to the role field of the Character type.
3. The test data is more generic in that it doesn't reference a specific character such as Palpatine.

Abstracting generation of test data can go further by generating all strings with a Random string generator.  This will aide in testing uniqueness of a given string.


