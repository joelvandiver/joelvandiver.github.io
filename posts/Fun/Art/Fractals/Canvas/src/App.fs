module App

open System
open Fable.Core.JsInterop
open Fable.Import.Browser

let tile = 300.
let fill = "rgb(50,50,50)"
let scale = 0.6

let createCanvas (name) =
    let canvas = document.createElement("canvas") :?> HTMLCanvasElement
    canvas.id <- name
    canvas.width  <- tile
    canvas.height <- tile
    document.body.appendChild canvas |> ignore
    canvas.style.border <- "solid 1px #ccc"
    canvas.style.margin <- "10px"
    let ctx = canvas.getContext_2d()
    (canvas, ctx)

let square () = 
    let (canvas, ctx) = createCanvas("square")
    ctx.fillStyle <- !^fill
    let x = tile * (1. - scale) / 2.
    let h = tile * scale
    ctx.fillRect (x, x, h, h)

let circle () =
    let (canvas, ctx) = createCanvas("circle")
    ctx.fillStyle <- !^fill
    let r = scale / 2. * tile
    let x = tile / 2.
    ctx.beginPath()
    ctx.arc(x, x, r, 0., 360.)
    ctx.fill()

let fractal1 () =
    let (canvas, ctx) = createCanvas("fractal1")
    ctx.fillStyle <- !^fill
    let data = 
        { 0 .. 10 } 
        |> List.ofSeq
        |> List.map(fun i -> Math.Pow(0.5, float i))
    let draw data = ctx.fillRect data
    (List.map ((fun x -> tile * x) >> (fun x -> x, x, x, x)) data) |> List.iter draw
    data |> List.iter (fun x -> ctx.fillRect (tile * x, tile * x, tile * x, tile * x))
    // data |> List.iter (fun x -> ctx.fillRect (tile * x * 2., tile * x * 2., tile * x, tile * x))

let f x = x()
let initCanvas() =
    [
        square
        circle
        fractal1
    ]
    |> List.iter f

initCanvas()