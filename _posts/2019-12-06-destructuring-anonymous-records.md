# Destructuring Anonymous Records

Does F# support destructuring anonymous types in the same manner as record types?

```fsharp
// REMINDER! Record destructuring:
type User =
   {   FirstName: string;
       LastName: string
       Email: string option
       }

// Destructure the record within the parameter with only the properties you care about.
let GetUserName { FirstName = fn; LastName = ln } = fn + " " + ln 
    // val GetUserName : User -> string


// Try Anonymous:
let GetUserNameAnon {| FirstName = fn; LastName = ln |}) = fn + " " + ln
    //  error FS0010: Unexpected symbol '{|' in pattern
let bob =
    {|  FirstName = "Bob"
        LastName  = "Smith"
        Email = Some ""
        |}
bob |> GetUserNameAnon

```

Indeed, F# does not support destructuring anonymous types as of:

```
Microsoft (R) F# Interactive version 10.6.0.0 for F# 4.7
Copyright (c) Microsoft Corporation. All Rights Reserved.
```