// Create printers for values above.
module addPrinters = 
    let print_bool x = if x then "T" else "F"
    fsi.AddPrinter(fun (data) -> 
        data
        |> List.map(fun (p, q, r) -> 
            let p' = print_bool p
            let q' = print_bool q
            let r' = print_bool r
            sprintf "  | %s | %s | %s |" p' q' r')
        |> List.fold(fun a b -> a + "\r\n" + b) """| p | q | r |
  | - | - | - |"""
        )


// Let p be a proposition.

let ps = [true; false]

// Table of Negation -> (p, ¬p)
let ps_negation = ps |> List.map(fun p -> p, not p)

let qs = [true; false]

let cartesian xs ys =
    xs
    |> List.map(fun x -> 
        ys
        |> List.map(fun y -> 
            (x, y)))
    |> List.concat        

let pqs = cartesian ps qs

// Table of Conjunction (And) -> (p, q) -> (p ∧ q)
let ps_conjunction = pqs |> List.map(fun (p, q) -> p, q, p && q)

// Table of Disjunction (Or) -> (p, q) -> (p ∨ q)
let ps_disjunction = pqs |> List.map(fun (p, q) -> p, q, p || q)

