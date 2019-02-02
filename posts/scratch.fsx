let test n = 
   let nS = n * n
   nS * nS

test 3

let inTest n = 
   let nS = n * n in nS * nS

inTest 3

test 3 = inTest 3