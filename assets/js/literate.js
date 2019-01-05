var currentElement = null;

document.onkeydown = hideTip2;
document.onclick = hideTip2;

function hideTip(evt,name) {
  // Stub for F# Literate onmouseout
}

function hideTip2() {
  if (currentElement) {
    currentElement.style.display = "none";
  }

}

function showTip(evt,name) {
  hideTip2();

  var parent = evt.srcElement ? evt.srcElement : evt.target;

  var rect = parent.getBoundingClientRect();
  var posx = rect.x;
  var posy = rect.y;

  var el = document.getElementById(name);
  currentElement = el;
  el.style.position = "fixed";
  el.style.left = posx + "px";
  el.style.top = posy + 30 + "px";
  el.style.display = "block";
}