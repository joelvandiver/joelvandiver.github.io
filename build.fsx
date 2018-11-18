#r @"C:\git\joelvandiver.github.io\packages\FSharp.Formatting\lib\net40\FSharp.Markdown.dll"
#r @"C:\git\joelvandiver.github.io\packages\Handlebars.Net\lib\net452\Handlebars.dll"

open System.IO
open FSharp.Core
open FSharp.Markdown
open HandlebarsDotNet

let root = __SOURCE_DIRECTORY__
let postPath = root + @"\posts"
let indexMarkdown = root + @"\index.md"
let indexTemplate = root + @"\index.hbs"

type FileText = 
    {  file : string
       path : string }

let readFileText (path: string)  = 
  { file = File.ReadAllText(path)
    path = path }

let postMds = 
  indexMarkdown :: (Directory.GetFiles(postPath, "*.md", SearchOption.AllDirectories) |> List.ofSeq)
  |> List.map readFileText
  
let private marked (markdown: string): string = markdown |> Markdown.Parse |> Markdown.WriteHtml

let parseMarkedFileText (ft: FileText) : FileText =
  { file = ft.file |> marked
    path = ft.path.Replace(".md", ".html")}

let parsed = postMds |> List.map parseMarkedFileText



type Anchor = 
  { title : string
    url   : string }

let navs : Anchor list =
  parsed
  |> List.map(
      fun ft -> 
        let url = 
          ft.path
            .Replace(root, "")
            .Replace(@"\", "/")
            .Replace("/index.html", "")
        { title = url
                    .Replace("/posts/", "")
                    .Replace("/index", "")
                    .Replace(".html", "")
          url   = url  })
  |> List.filter(fun url -> url.url <> "/index.html")      

type Components = 
  { postnavs : Anchor list 
    post     : string }

let genComponents post = 
  { postnavs = navs
    post = post }

Handlebars.RegisterTemplate("postnav", """
  <li><a href="{{url}}">{{title}}</a></li>
""");

let source = File.ReadAllText(indexTemplate)


let genHtml post = 
  let helper (writer: TextWriter) (_: obj) (_: obj) = writer.WriteSafeString(post)
  Handlebars.RegisterHelper("post", HandlebarsHelper(helper))
  let template = Handlebars.Compile(source)
  post
  |> genComponents
  |> template.Invoke

let save () =
  parsed
  |> List.iter(
      fun ft -> 
        printfn "%A" ft
        let html = ft.file |> genHtml
        File.WriteAllText(ft.path, html))

save()

