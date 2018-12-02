var context;
// Existing code unchanged.
window.onload = function() {
  context = new (window.AudioContext || window.webkitAudioContext)();
  // var context = new AudioContext();
  playSquare(context);
};

// One-liner to resume playback when user interacted with the page.
document.querySelector("button").addEventListener("click", function() {
  context.resume().then(() => {
    console.log("Playback resumed successfully");
  });
});

function playSquare(context) {
  // create web audio api context

  // create Oscillator node
  var oscillator = context.createOscillator();

  oscillator.type = "square";
  oscillator.frequency.setValueAtTime(440, context.currentTime); // value in hertz
  oscillator.connect(context.destination);
  oscillator.start();
}
