module App

open System
open Fable.Core.JsInterop
open Fable.Import.Browser

let tile = 300.
let fill = "rgba(0,0,0,0.5)"
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
    let (_, ctx) = createCanvas("square")
    ctx.fillStyle <- !^fill
    let x = tile * (1. - scale) / 2.
    let h = tile * scale
    ctx.fillRect (x, x, h, h)

let circle () =
    let (_, ctx) = createCanvas("circle")
    ctx.fillStyle <- !^fill
    let r = scale / 2. * tile
    let x = tile / 2.
    ctx.beginPath()
    ctx.arc(x, x, r, 0., 360.)
    ctx.fill()

let genFractal (gens: (float -> float * float * float * float) list) =
    let (_, ctx) = createCanvas("fractal1")
    ctx.fillStyle <- !^fill
    let data = 
        { 0 .. 20 } 
        |> List.ofSeq
        |> List.map(fun i -> tile * Math.Pow(0.5, float i))

    let draw data = ctx.fillRect data
    data
    |> List.collect (fun d -> gens |> List.map(fun g -> g d)) 
    |> List.iter ctx.fillRect

let f x = x()
let initCanvas() =
    [
        square
        circle
    ]
    |> List.iter f
        
    [
        fun x -> x, x, x, x
    ] |> genFractal
    [
        fun x -> (tile - x), x, x, x
    ] |> genFractal
    [
        fun x -> x, (tile - x), x, x
    ] |> genFractal
    [
        fun x -> x, x, (tile - x), x
    ] |> genFractal
    [
        fun x -> x, (tile - x), (tile - x), x
    ] |> genFractal
    [
        fun x -> (tile - x), (tile - x), (tile - x), x
    ] |> genFractal
    [
        fun x -> (tile - x), (tile - x), x, x
    ] |> genFractal
    [
        fun x -> (tile - x), x, x, (tile - x)
    ] |> genFractal
    [
        fun x -> x * 0.2, x * 0.2, x * 1.5, x * 1.5
    ] |> genFractal
    [
        fun x -> x * 0.2, x * 0.2, x * 1.5, x * 1.5
    ] |> genFractal

initCanvas()