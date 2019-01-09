(**
# Accessing Command Line Arguments in an FSX Script

To access the command line arguments in an FSX script, simply use the provided fsi.CommandLineArgs.

Let's use the following test lines in this script:
*)

let args = fsi.CommandLineArgs

printfn "%A" args

(**
## Examples:

Current Script:  C:\git\joelvandiver.github.io\posts\tutorials\fs\fsx\cli-args\index.fsx

> *Note* - Ensure access to FSI in your PATH. My FSI is located here:  C:\Program Files (x86)\Microsoft SDKs\F#\4.1\Framework\v4.0\fsi.exe

### I. Basic

```command
fsi C:\git\joelvandiver.github.io\posts\tutorials\fs\fsx\cli-args\index.fsx
```

*Ouput*:
```command
[|"C:\git\joelvandiver.github.io\posts\tutorials\fs\fsx\cli-args\index.fsx"|]
```

### II.  Pass Parameters

```command
fsi C:\git\joelvandiver.github.io\posts\tutorials\fs\fsx\cli-args\index.fsx a b c d 1 2 3 4 
```

*Ouput*:
```command
[|"C:\git\joelvandiver.github.io\posts\tutorials\fs\fsx\cli-args\index.fsx"; "a";
  "b"; "c"; "d"; "1"; "2"; "3"; "4"|]
```

*)


