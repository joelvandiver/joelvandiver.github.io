// # Relations and Functions

(*
   A relation of inputs and outputs as ordered pairs can be generated for every algorithm.  
   
   **Conjecture** -> Algorithms with side effects cannot be precisely defined as a functions since the output may vary for a given input.
   
   **Key Idea** -> Strict functions lead to precision and predictability.
*)

open System

module ``Exploration of Functions as Data`` =
   let x = 3

module ``Exploration of Relations and Functions`` = 
   // ## Exploration
   let f x = 3 * x + 4

   // (?) -> How can I limit the domain of a function in F#?
   let fD = { -2 .. 2 } |> Set.ofSeq
   let fRelation = fD |> Set.map (fun x -> x, x |> f)

   // (?) -> How can I test empirically & prove deductively that for every input there is only one output for a given function?

