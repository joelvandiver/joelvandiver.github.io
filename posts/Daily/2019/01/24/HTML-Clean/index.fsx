(**
# Clean Html with XElement.Parse
*)

#r @"C:\git\joelvandiver.github.io\packages\System.Xml.Linq\lib\net20\System.Xml.Linq.dll"
open System.IO
open System.Xml.Linq

let path = @"C:\git\joelvandiver.github.io\index.html"

let cleanHtml (file) : unit =
   let clean = file |> File.ReadAllText |> XElement.Parse |> string
   File.WriteAllText(file, clean)         

path |> cleanHtml