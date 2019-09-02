---
title: 
categories: [Guide-F#]
tags: []
---

```fsharp
type User =
   {   FirstName: string;
       LastName: string
       Email: string option
       }

let GetUserName { FirstName = fn; LastName = ln } = fn + " " + ln
let GetUserName' user = user.FirstName + " " + user.LastName

let bob : User =
   {   FirstName = "Bob"
       LastName  = "Smith"
       Email = None
       }
  
bob |> GetUserName
bob |> GetUserName'


type Employer =
   {   FirstName: string;
       LastName: string
       Email: string option
       }

// let GetEmployerName { FirstName = fn; LastName = ln } = fn + " " + ln
let GetEmployerName ({ FirstName = fn; LastName = ln }: Employer) = fn + " " + ln
      
let sarah : Employer =
   {   FirstName = "Sarah"
       LastName  = "Jones"
       Email = None
       }
  
sarah |> GetEmployerName

```