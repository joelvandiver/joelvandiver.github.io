# Joel Vandiver's Blog

The url for this Blog is here:
[https://joelvandiver.github.io/](https://joelvandiver.github.io/)

## TODO

### F# Guide

- [ ] F# Guide
  - [ ] Option
  - [ ] `function` keyword
  - [ ] Lazy Values
  - [ ] [Function-Values](https://joelvandiver.github.io/posts/Daily/2019/04/08/Function-Values/)
  - [ ] Function-Values
    - [ ] Function Pipelining
    - [ ] Function Composition
    - [ ] Compare with Type Members

### Fable Canvas Guide

- [ ] Start

### Dev

- [ ] BUILD - Subsections
  - [ ] Create a landing page per sub section and do not dynamically build the navigation
- [ ] Handle Navbar for Phone Sizes
- [ ] Build by section.md instead of index.md with static list of sections
- [ ] Nav to section from postsPosts
- [ ] Export F# Library Documentation https://fsprojects.github.io/FSharp.Formatting/metadata.html
- [ ] Blog-Build Process
  - [ ] Blog about the Blog-Build Process
- [ ] Parsing Arithmetic Expressions into Polish Notation (p.52 VSI)
- [ ] Implement an F# presentation with FsReveal http://fsprojects.github.io/FsReveal/
- [ ] Questions
  - [ ] How can I begin quantifying the goodness of the algorithms I create based on abstract criteria?
- [ ] Backlog
  - [ ] Implement a General XML to Json Solution

- [ ] Convert Blog to 
```F#
#load @"packages\FSharp.Formatting\FSharp.Formatting.fsx"
open FSharp.Literate
open System.IO

let src = __SOURCE_DIRECTORY__ + "/test/src"
let output = __SOURCE_DIRECTORY__ + "/test/output"
let template = __SOURCE_DIRECTORY__ + "/template-project.html"

// Load the template & specify project information
let projInfo =
 [ "page-description", "GCS Pro Documentation"
   "page-author", "Joel Vandiver"
   "project-name", "GCS Pro" ]

// Process all files and save results to 'output' directory
let x =
   Literate.ProcessDirectory
     (src, template, output, replacements = projInfo)
```


### Posts

- [ ] Explore Tail Recursive (Ex. Factorial, Fibonacci, ...)
- [ ] Restrict the Domain of a Function at Compile Time with Types
- [ ] Explore Modular Arithmetic
- [ ] Setup an API Gateway with Ocelot
- [ ] Setup Giraffe with Swagger
- [ ] Setup Windows Authentication with HTTP.sys
- [ ] Setup [ML.NET](https://dotnet.microsoft.com/apps/machinelearning-ai)
  - [ ] [Install](https://dotnet.microsoft.com/learn/machinelearning-ai/ml-dotnet-get-started-tutorial/install)