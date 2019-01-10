(function(document) {
  "use strict";

  let ctx = document.getElementById("canvas").getContext("2d");
  let v = [...Array(500).keys()];
  v.forEach(i =>
    v.forEach(j => {
      ctx.fillStyle =
        "rgb(" +
        Math.floor(Math.cos(2 * Math.PI * i / 360) * 255) +
        ", " +
        Math.floor(Math.sin(2 * Math.PI * j / 360) * 255) +
        ", " +
        Math.floor(
          Math.cos(2 * Math.PI * Math.pow(i, 3) * Math.pow(j, 3) / 50) * 255
        ) +
        ")";
      ctx.fillRect(i, j, 2, 2);
    })
  );
})(document);
