open System

type User = 
    { firstname : string
      lastname  : string
      username  : string }

let isEmpty = String.IsNullOrWhiteSpace

let validateUserExceptions (user: User) : unit = 
    if isEmpty user.firstname then failwith "The first name is required."
    if isEmpty user.lastname then failwith "The last name is required."
    if isEmpty user.username then failwith "The username is required."

let validateUserResult (user: User) : Result<User, string list> =
    let errors = 
        [
            if isEmpty user.firstname then Some "The first name is required." else None
            if isEmpty user.lastname then Some "The last name is required." else None
            if isEmpty user.username then Some "The username is required." else None
        ]
        |> List.filter Option.isSome
        |> List.map Option.get

    match errors with 
    | [] -> Ok user
    | _ -> Error errors

// TODO:  Illustrate binding results.
// TODO:  Illustrate branching vs aggregating error state.
