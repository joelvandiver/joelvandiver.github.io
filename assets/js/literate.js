var currentElement = null;

document.addEventListener("click", hideTip2)

function hideTip(evt, name) {
    // Stub for F# Literate onmouseout
}

function hideTip2(evt) {
    if (currentElement) {
        currentElement.style.display = "none";
    }

}

function showTip(evt, name) {
    // Do not change the tooltip if there is a selection on the window.
    if (window.getSelection().toString().length > 0) {
        return;
    }

    // Hide the previous tip if there is one.
    hideTip2();

    var parent = evt.srcElement ? evt.srcElement : evt.target;

    var rect = parent.getBoundingClientRect();
    var posx = rect.x;
    var posy = rect.y;

    var el = document.getElementById(name);
    if (!el) { return; }
    currentElement = el;
    el.addEventListener("click", function (clickEvt) {
        clickEvt.stopPropagation();
    });
    el.style.position = "fixed";
    el.style.left = posx + "px";
    el.style.top = posy + 30 + "px";
    el.style.display = "block";
}