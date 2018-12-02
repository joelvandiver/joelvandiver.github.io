(function() {
  var myCanvas = document.createElement("canvas");
  myCanvas.width = 800;
  myCanvas.height = 600;
  document.body.appendChild(myCanvas);
  var ctx = myCanvas.getContext("2d");
  function draw(startX, startY, len, angle) {
    ctx.beginPath();
    ctx.save();

    ctx.translate(startX, startY);
    ctx.rotate((angle * Math.PI) / 180);
    ctx.moveTo(0, 0);
    ctx.lineTo(0, -len);
    ctx.stroke();

    if (len < 10) {
      ctx.restore();
      return;
    }

    draw(0, -len, len * 0.8, -15);
    draw(0, -len, len * 0.8, 15);

    ctx.restore();
  }

  draw(350, 600, 120, 0);
})();
