# Basic F# Expecto Example

First, let's gather our dependencies.

```fsharp
#r @"C:\Users\joelv\.nuget\packages\NETStandard.Library\2.0.3\build\netstandard2.0\ref\netstandard.dll"
#r @"C:\git\joelvandiver.github.io\packages\Expecto\lib\netstandard2.0\Expecto.dll"

open Expecto


## Define a simple test case.

let toBeTest =
  testCase "To be or not to be" <| fun () ->
    let toBe = true
    Expect.isTrue toBe "You do not exist."

runTests defaultConfig toBeTest
```


*Output:*
```console
[15:36:13 INF] EXPECTO? Running tests... <Expecto>
[15:36:13 INF] EXPECTO! 1 tests run in 00:00:00.0109563 for To be or not to be – 1 passed, 0 ignored, 0 failed, 0 errored. Success! <Expecto>
val toBeTest : Test =
  TestLabel
    ("To be or not to be",TestCase (Sync <fun:toBeTest@5-1>,Normal),Normal)
val it : int = 0
```




## Combine multiple tests into a list.


```fsharp
let toThinkTest =
  test "I think therefore I am." {
    let iThink = true
    Expect.isTrue iThink "You do not think so you do not exist."
  } 

let existenceTests = testList "Test for Existence" [toBeTest; toThinkTest]

runTests defaultConfig existenceTests
```


*Output:*
```console
[15:36:46 INF] EXPECTO? Running tests... <Expecto>
[15:36:47 INF] EXPECTO! 2 tests run in 00:00:00.0219511 for Test for Existence – 2 passed, 0 ignored, 0 failed, 0 errored. Success! <Expecto>
val toThinkTest : Test =
  TestLabel
    ("I think therefore I am.",TestCase (Sync <fun:toThinkTest@11-1>,Normal),
     Normal)
val existenceTests : Test =
  TestLabel
    ("Test for Existence",
     TestList
       ([TestLabel
           ("To be or not to be",TestCase (Sync <fun:toBeTest@5-1>,Normal),
            Normal);
         TestLabel
           ("I think therefore I am.",
            TestCase (Sync <fun:toThinkTest@11-1>,Normal),Normal)],Normal),
     Normal)
val it : int = 0
```


