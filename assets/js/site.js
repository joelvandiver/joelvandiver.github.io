(function () {
    fetch("https://api.nasa.gov/planetary/apod?api_key=5VhLWB6JaWs6m11mLfrNKply8naxchfbWb0Nu2Q9")
        .then(function (response) {
            return response.json();
        })
        .then(function (json) {
            if (!json.media_type === "image") { return; }
            var src = json.url;
            var container = document.getElementById("nasa-img-container");
            var img = document.createElement('img');
            img.src = src;
            container.title = json.explanation;
            container.appendChild(img);
        });
})();