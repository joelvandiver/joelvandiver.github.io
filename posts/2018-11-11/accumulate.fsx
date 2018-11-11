#r @"C:\git\joelvandiver.github.io\packages\Expecto\lib\netstandard2.0\Expecto.dll"

open Expecto

let accumulate<'T> (accumulator: 'T -> 'T -> 'T) (l: 'T list) : 'T list =
  match l with 
  | [] -> []
  | [h] -> [h]
  | _ -> 

    let rec accumulate (g: 'T list) (accumulated: 'T list) : 'T list =
        match g with 
        | [] -> accumulated
        | h :: t -> 
            match accumulated with 
            | [] -> []
            | _ -> 
                let last = accumulated |> List.rev |> List.head
                let a = (last, h) ||> accumulator
                accumulate t (accumulated @ [a])

    accumulate l.Tail [l.Head]

let multiply a b = a * b
let multiplier = accumulate multiply

let dotConcat a b = a + "." + b
let dotMapper = accumulate dotConcat

let numberTests = 
  testList "numberTests" [
    testCase "should return an empty list from an empty list." <| fun _ -> 
      let d = []
      let r = multiplier d
      Expect.equal [] r "The list is not empty."
    testCase "should return a list of one from a list of one." <| fun _ -> 
      let d = [5]
      let r = multiplier d
      Expect.equal [5] r "The list has more than one value."
    testCase "should return a list of the accumulated values." <| fun _ -> 
      let d = [1;2;3;4;5]
      let r = multiplier d
      Expect.equal [1;2;6;24;120] r "The multiplier of [1;2;3;4;5] did not yield [1;2;6;24;120]."
  ]

let stringTests =
  testList "stringTests" [
    testCase "should return a list of the accumulated values." <| fun _ -> 
      let alphabet = "abcdef"
      // let alphabet = "abcdefghijklmnopqrstuvwxyz"
      let alphaList = alphabet |> List.ofSeq |> List.map string
      let r = dotMapper alphaList
      let e = ["a"; "a.b"; "a.b.c"; "a.b.c.d"; "a.b.c.d.e"; "a.b.c.d.e.f"]
      Expect.equal e r "The lists are not equal."
  ]

let tests = 
  testList "tests" [
    numberTests
    stringTests
  ]

runTests defaultConfig tests
