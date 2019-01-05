(function(document) {
   "use strict";
 
   let ctx = document.getElementById("fractal-logo").getContext("2d");
   function draw(startX, startY, len, angle) {
     ctx.beginPath();
     ctx.save();
 
     ctx.translate(startX, startY);
     ctx.rotate((angle * Math.PI) / 180);
     ctx.moveTo(0, 0);
     ctx.lineTo(0, -len);
     ctx.strokeStyle = "white";
     ctx.stroke();
 
     if (len < 35) {
       ctx.restore();
       return;
     }
 
     draw(0, -len, len * 0.95, -15);
     draw(0, -len, len * 0.95, 15);
 
     ctx.restore();
   }
 
   draw(350, 600, 120, 0);
 })(document);