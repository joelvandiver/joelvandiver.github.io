let x = lazy (5 + 4)

printfn "%A" x

let y = x.Force()



let mutable workNumCount = 0

let workNum() =
    printfn "Starting Num %i" workNumCount
    workNumCount <- workNumCount + 1
    workNumCount

let numVal = lazy (workNum())
let numVals =
    [ numVal; numVal; numVal; numVal; numVal; numVal; numVal; numVal ] |> List.map (fun numVal -> numVal.Force())

numVal.Force()
)