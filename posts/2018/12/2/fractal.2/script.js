(function() {
  var c = document.getElementById("myCanvas");
  var ctx = c.getContext("2d");

  var genList = count => [...Array(count).keys()]

  ctx.translate(0.5, 0.5); // anti-alias trick

  function drawLine(p0, p1, color = "black") {
    ctx.beginPath();
    ctx.moveTo(p0.x, p0.y);
    ctx.lineTo(p1.x, p1.y);
    ctx.strokeStyle = color;
    ctx.lineWidth = 1;
    ctx.stroke();
  }

  function drawLinePoints(points, color = "black") {
    drawLine(points.a, points.b, color);
  }

  function drawTriangle(p0, p1, p2) {
    drawLine(p0, p1);
    drawLine(p1, p2);
    drawLine(p2, p0);
  }

  function drawFract(p0, p1, p2, limit) {
    if (limit > 0) {
      const pA = {
          x: p0.x + (p1.x - p0.x) / 2,
          y: p0.y - (p0.y - p1.y) / 2
        },
        pB = {
          x: p1.x + (p2.x - p1.x) / 2,
          y: p1.y - (p1.y - p2.y) / 2
        },
        pC = {
          x: p0.x + (p2.x - p0.x) / 2,
          y: p0.y
        };
      drawFract(p0, pA, pC, limit - 1);
      drawFract(pA, p1, pB, limit - 1);
      drawFract(pC, pB, p2, limit - 1);
    } else {
      drawTriangle(p0, p1, p2);
    }
  }
  // drawFract({x: 0, y:400},{x:200, y:0},  {x:400, y:400}, 6)

  function drawLines() {
    let list = genList(100)
      .map(i => {
        let y = 400 - 400 / (i / 2);
        return {
          a: { x: 0, y }, 
          b: { x: 400, y }
        };
      });
    list.forEach(drawLinePoints);
  }

  function drawLines2() {
    let list = genList(1000)
      .map(i => {
        let y = 400 - 400 / (i / 2);
        return {
          a: { x: 0, y: y * 0.1 * i }, 
          b: { x: 400, y: y / (10 * i)  }
        };
      });
    list.forEach(drawLinePoints);
  }

  drawLines2();

})();
