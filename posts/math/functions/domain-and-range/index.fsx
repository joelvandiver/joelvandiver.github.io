(**
Domain and Range Exploration
============================

*)
let S = [(1, 4); (2, 3); (2, 2); (4, 3); (5, 4)] |> Set.ofList
let Domain S = S |> Set.map(fun (x, _) -> x)
let Range S = S |> Set.map(fun (_, y) -> y)
let DomainS = S |> Domain
let RangeS = S |> Range