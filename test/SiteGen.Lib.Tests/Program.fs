open System
open Expecto
open SiteGen.Lib

let say = Say.hello "Joel"

let tests =
  test "A simple test" {
    let subject = "Hello World"
    Expect.equal subject "Hello World" "The strings should equal"
  }

[<EntryPoint>]
let main args = runTestsWithArgs defaultConfig args tests
