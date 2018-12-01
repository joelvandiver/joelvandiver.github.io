#r @"..\packages\FSharp.Data\lib\net45\FSharp.Data.dll"
open FSharp.Data
open HtmlDocument

// type Index = HtmlProvider<"../index.html">
// let index = Index()
// Index.GetSample()

let index = HtmlDocument.Load(__SOURCE_DIRECTORY__ + "../../index.html")

let buttons = index.Descendants ["button"] |> List.ofSeq
let buttons2 = index.Descendants ["button"] |> List.ofSeq

let recurseOnMatch = true
let predicate html = true
let doc = index
let children = descendants recurseOnMatch predicate doc |> List.ofSeq

body(index)
elements(index)