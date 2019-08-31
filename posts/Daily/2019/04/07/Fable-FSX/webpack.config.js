// Note this only includes basic configuration for development mode.
// For a more comprehensive configuration check:
// https://github.com/fable-compiler/webpack-config-template

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