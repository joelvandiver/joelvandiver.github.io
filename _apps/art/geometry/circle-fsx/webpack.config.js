
var path = require("path");

module.exports = {
    mode: "development",
    entry: "./fable.fsx",
    output: {
        path: path.join(__dirname, "./"),
        filename: "fable-fsx.js",
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