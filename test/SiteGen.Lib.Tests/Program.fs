open System
open Expecto
open SiteGen.Lib

let tests =
  test "A simple test" {
    let subject = "Hello World"
    Expect.equal subject "Hello World" "The strings should equal"
  }


// should create an <html> tag.
// should create an html document.
// should create an <body>.
// should create a <p> tag.
// should create a <strong> tag.
// should create a <div> tag.
// should create a <span> tag.
// should create a <li> tag.
// should create a <ul> tag.
// should create a <ol> tag.

[<EntryPoint>]
let main args = runTestsWithArgs defaultConfig args tests
