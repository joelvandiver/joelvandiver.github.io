"use strict";
 
var gulp = require("gulp");
var sass = require("gulp-sass");
 
sass.compiler = require("node-sass");
 
gulp.task("sass", function () {
  return gulp.src("./content/site.scss")
    .pipe(sass().on("error", sass.logError))
    .pipe(gulp.dest("./assets/css"));
});
 
gulp.task("sass:watch", function () {
  gulp.watch("./content/*.scss", ["sass"]);
});

gulp.task("default", ["sass:watch"]);
