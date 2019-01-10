(function(document) {
  "use strict";

  let ctx = document.getElementById("canvas").getContext("2d");
  let v = [...Array(500).keys()];
  v.forEach(i =>
    v.forEach(j => {
      ctx.fillStyle =
        "rgb(" +
        Math.floor(0.8 * Math.cos(2 * Math.PI * i / 360) * 255) +
        ", " +
        Math.floor(0.8 * Math.sin(2 * Math.PI * j / 360) * 255) +
        ", " +
        Math.floor(
          0.8 *
            Math.cos(2 * Math.PI * Math.pow(i, 4) * Math.pow(j, 5) / 500) *
            255
        ) +
        ")";
      ctx.fillRect(i, j, 2, 2);
    })
  );
})(document);
