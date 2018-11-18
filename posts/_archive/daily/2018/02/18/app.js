(function(document) {
  "use strict";

  (function drawLine() {
    let canvas = document.getElementById("line");
    let ctx = canvas.getContext("2d");
    ctx.moveTo(0, 0);
    ctx.lineTo(100, 100);
    ctx.stroke();
  })();

  (function drawCircle() {
    let canvas = document.getElementById("circle");
    let ctx = canvas.getContext("2d");
    ctx.beginPath();
    ctx.arc(50, 50, 40, 0, 2 * Math.PI);
    ctx.stroke();
  })();

  (function drawRectangle() {
    let canvas = document.getElementById("rectangle");
    let ctx = canvas.getContext("2d");
    ctx.fillRect(10, 30, 80, 40);
  })();

  (function drawSquare() {
    let canvas = document.getElementById("square");
    let ctx = canvas.getContext("2d");
    ctx.fillRect(30, 30, 40, 40);
  })();

  (function drawCoordinateSystem() {
    let canvas = document.getElementById("coordinate-system");
    let ctx = canvas.getContext("2d");
    getCoordinateSystem(ctx);
  })();

  function getCoordinateSystem(ctx) {
    // Find w & h
    let w = ctx.canvas.clientWidth;
    let h = ctx.canvas.clientHeight;

    // x-axis
    ctx.moveTo(0, h / 2);
    ctx.lineTo(w, h / 2);
    ctx.stroke();

    // y-axis
    ctx.moveTo(w / 2, 0);
    ctx.lineTo(w / 2, h);
    ctx.stroke();
  }

  (function drawSlopeIntercept() {
    let canvas = document.getElementById("slope-intercept");
    let ctx = canvas.getContext("2d");
    getCoordinateSystem(ctx);

    let m = 2;
    let b = 10;

    let f = x => m * x + b;

    // Find extrema within canvas
    let w = canvas.clientWidth;
    let h = canvas.clientHeight;

    // From cartesian origin
    let xMax = w / 2;
    let xMin = -w / 2;

    let beg = [xMin, f(xMin)];
    let end = [xMax, f(xMax)];

    let actual = {
      beg: transformToCanvas(beg[0], beg[1], w, h),
      end: transformToCanvas(end[0], end[1], w, h)
    };

    getLineFromPoints(ctx, actual.beg, actual.end);
  })();

  function getLineFromPoints(ctx, beg, end) {
    ctx.moveTo(beg[0], beg[1]);
    ctx.lineTo(end[0], end[1]);
    ctx.stroke();
  }

  function transformToCanvas(x, y, w, h) {
    // Flip over x-axis
    // and go to the center of w & h
    return [x + w / 2, -1 * y + h / 2];
  }

  (function displayCode() {
    let pre = document.createElement("pre");
    pre.textContent = `
    "use strict";

    (function drawLine() {
      let canvas = document.getElementById("line");
      let ctx = canvas.getContext("2d");
      ctx.moveTo(0, 0);
      ctx.lineTo(100, 100);
      ctx.stroke();
    })();
  
    (function drawCircle() {
      let canvas = document.getElementById("circle");
      let ctx = canvas.getContext("2d");
      ctx.beginPath();
      ctx.arc(50, 50, 40, 0, 2 * Math.PI);
      ctx.stroke();
    })();
  
    (function drawRectangle() {
      let canvas = document.getElementById("rectangle");
      let ctx = canvas.getContext("2d");
      ctx.fillRect(10, 30, 80, 40);
    })();
  
    (function drawSquare() {
      let canvas = document.getElementById("square");
      let ctx = canvas.getContext("2d");
      ctx.fillRect(30, 30, 40, 40);
    })();
  
    (function drawCoordinateSystem() {
      let canvas = document.getElementById("coordinate-system");
      let ctx = canvas.getContext("2d");
      getCoordinateSystem(ctx);
    })();
  
    function getCoordinateSystem(ctx) {
      // Find w & h
      let w = ctx.canvas.clientWidth;
      let h = ctx.canvas.clientHeight;
  
      // x-axis
      ctx.moveTo(0, h / 2);
      ctx.lineTo(w, h / 2);
      ctx.stroke();
  
      // y-axis
      ctx.moveTo(w / 2, 0);
      ctx.lineTo(w / 2, h);
      ctx.stroke();
    }
  
    (function drawSlopeIntercept() {
      let canvas = document.getElementById("slope-intercept");
      let ctx = canvas.getContext("2d");
      getCoordinateSystem(ctx);
  
      let m = 2;
      let b = 10;
  
      let f = x => m * x + b;
  
      // Find extrema within canvas
      let w = canvas.clientWidth;
      let h = canvas.clientHeight;
  
      // From cartesian origin
      let xMax = w / 2;
      let xMin = -w / 2;
  
      let beg = [xMin, f(xMin)];
      let end = [xMax, f(xMax)];
  
      let actual = {
        beg: transformToCanvas(beg[0], beg[1], w, h),
        end: transformToCanvas(end[0], end[1], w, h)
      };
  
      getLineFromPoints(ctx, actual.beg, actual.end);
    })();
  
    function getLineFromPoints(ctx, beg, end) {
      ctx.moveTo(beg[0], beg[1]);
      ctx.lineTo(end[0], end[1]);
      ctx.stroke();
    }
  
    function transformToCanvas(x, y, w, h) {
      // Flip over x-axis
      // and go to the center of w & h
      return [x + w / 2, -1 * y + h / 2];
    }
      `;
    pre.className = "prettyprint";
    document.body.appendChild(pre);
  })();
})(document);
