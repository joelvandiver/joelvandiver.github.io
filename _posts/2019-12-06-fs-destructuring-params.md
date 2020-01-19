# F# Destructuring Params

F# has another useful technique to help add clarity to code.  

Records are simple definitions of types.  Records are *symmetric* in that the data published by the record is the same as the data used to construct the record.  You can trust that what you give it is what you get from it.  

This symmetry also comes with structural equality.  Two records of the same type with the same properties and values are considered structurally equal even if they are in different locations in memory.

And, with structural equality comes the ability to *destructure* the record within parameters.  

```fsharp
// Define a simple record type.
type User =
   {   FirstName: string;
       LastName: string
       Email: string option
       }

// Destructure the record within the parameter with only the properties you care about.
// ***Note:  Email is ignored!
let GetUserName { FirstName = fn; LastName = ln } = fn + " " + ln 
    // val GetUserName : User -> string

// Provide an instance of the type.
let bob : User =
   {   FirstName = "Bob"
       LastName  = "Smith"
       Email = None
       }
  
bob |> GetUserName 
    // val it : string = "Bob Smith"
```