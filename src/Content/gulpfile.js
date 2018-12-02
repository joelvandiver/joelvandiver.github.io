"use strict";
 
var gulp = require("gulp");
var sass = require("gulp-sass");
 
sass.compiler = require("node-sass");
 
gulp.task("sass", function () {
  return gulp.src("site.scss")
    .pipe(sass().on("error", sass.logError))
    .pipe(gulp.dest("../../assets"));
});
 
gulp.task("sass:watch", function () {
  gulp.watch("site.scss", ["sass"]);
});
