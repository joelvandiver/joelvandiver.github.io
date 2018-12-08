#load ".fake/build.fsx/intellisense.fsx"
open Fake.Core
open Fake.DotNet
open Fake.IO
open Fake.IO.FileSystemOperators
open Fake.IO.Globbing.Operators
open Fake.Core.TargetOperators
open Fake.DotNet.Testing

Target.create "Clean" (fun _ ->
    !! "src/**/bin"
    ++ "src/**/obj"
    ++ "test/**/obj"
    ++ "test/**/obj"
    |> Shell.cleanDirs 
)

Target.create "Build" (fun _ ->
    !! "src/**/*.fsproj"
    ++ "test/**/*.fsproj"
    |> Seq.iter (DotNet.build id)
)

Target.create "Test" (fun _ -> 
    let setParams ps = ps
    let assemblies = 
        !! "test/**/Debug/**/*.Tests.dll"
    Expecto.run setParams assemblies
)

Target.create "All" ignore

"Clean"
  ==> "Build"
  ==> "Test"
  ==> "All"

Target.runOrDefault "All"
