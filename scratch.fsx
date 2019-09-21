open System

let f x = x + 3
let g x = x + 5

let fg = f >> g

[1;2;3;4;5] |> List.map fg

type User = { first: string; last: string }
let getUserName (user: User) = user.first + " " + user.last
let getFirstUser (users: User list) : User = users |> List.head
let getFirstUserName = getFirstUser >> getUserName
