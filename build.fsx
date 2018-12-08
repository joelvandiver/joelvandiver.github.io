#load ".fake/build.fsx/intellisense.fsx"
open Fake.Core
open Fake.DotNet
open Fake.IO
open Fake.IO.FileSystemOperators
open Fake.IO.Globbing.Operators
open Fake.Core.TargetOperators
open Fake.DotNet.Testing

Target.create "Build" (fun _ ->
    printfn "Building..."
)

Target.create "All" ignore

"Build"
  ==> "All"

Target.runOrDefault "All"
