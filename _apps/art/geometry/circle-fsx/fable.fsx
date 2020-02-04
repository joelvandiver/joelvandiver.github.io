
#r @"C:\Users\joelv\.nuget\packages\fable.core\3.1.4\lib\netstandard2.0\Fable.Core.dll"
#r @"C:\Users\joelv\.nuget\packages\fable.import.browser\1.3.0\lib\netstandard1.6\Fable.Import.Browser.dll"

open Fable.Core.JsInterop
open Fable.Import.Browser

let tile = 200.
let scale = 0.5

let canvas = document.getElementById("fable-drawing") :?> HTMLCanvasElement
canvas.width  <- tile
canvas.height <- tile
let ctx = canvas.getContext_2d()
(canvas, ctx)

let r = scale / 2. * tile
let x = tile / 2.
ctx.strokeStyle <- !^"#aaa"
ctx.beginPath()
ctx.arc(x, x, r, 0., 360.)
ctx.stroke()