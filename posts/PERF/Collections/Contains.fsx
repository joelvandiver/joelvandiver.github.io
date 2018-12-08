open System

// Define the expected power operator:
let (^) x n = pown x n
let million = 10 ^ 6

type PERF = 
   {  Title  : string 
      Time   : TimeSpan
      Memory : Double }

let stats = []

let timer = TimeSpan

let aSeq    = { (-1*million) .. million }
let aList   = aSeq |> List.ofSeq
let aSet    = aSeq |> Set.ofSeq
let aArray  = aSeq |> Array.ofSeq

