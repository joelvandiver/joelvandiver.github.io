(**
# First Fable FSX Canvas
04-07-2019

I'll start my Fable Canvas scripting experience with a simple circle.
*)

(**
<canvas id="drawing"></canvas>

I'll add the following element to this post.
```html
<canvas id="drawing"></canvas>
```
*)

(**
Let's import the Fable.Core and Fable.Import.Browser assemblies that were installed with Paket.
*)

#r @"C:\git\joelvandiver.github.io\packages\Fable.Core\lib\netstandard2.0\Fable.Core.dll"
#r @"C:\git\joelvandiver.github.io\packages\Fable.Import.Browser\lib\netstandard1.6\Fable.Import.Browser.dll"

open Fable.Core.JsInterop
open Fable.Import.Browser

let tile = 200.
let scale = 0.5

let canvas = document.getElementById("drawing") :?> HTMLCanvasElement
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

(**
To build, add the following web.config.js 
```javascript
var path = require("path");

module.exports = {
    mode: "development",
    entry: "./posts/Daily/2019/04/07/Fable-FSX/index.fsx",
    output: {
        path: path.join(__dirname, "./"),
        filename: "bundle.js",
    },
    devServer: {
        contentBase: "./",
        port: 8080,
    },
    module: {
        rules: [{
            test: /\.fs(x|proj)?$/,
            use: "fable-loader"
        }]
    }
}
```

And, then run the following command to build.
```bat
webpack --config c:/git/joelvandiver.github.io/posts/Daily/2019/04/07/Fable-FSX/webpack.config.js --mode production
```

Next, I'll add a referenced to the bundle.js that was built from the code above.
```html
<script src="./bundle.js" />
```

<script src="./bundle.js" />
*)