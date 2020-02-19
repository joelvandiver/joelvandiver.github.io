let someError = true

if someError then failwith "Some error occurred."


let isLessThan5 x : Result<int, string> = 
    if x < 5
    then Error "The value must be less than 5."
    else Ok x