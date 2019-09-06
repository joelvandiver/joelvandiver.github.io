(function () {
    debugger;
    const date = new Date();
    const today = date.toISOString().split("T")[0]
    const key = "APOD_RESULT";
    const last = localStorage.getItem(key);
    const lastDate = last ? JSON.parse(last) : undefined;

    if (!lastDate || lastDate.date !== today) {
        fetch("https://api.nasa.gov/planetary/apod?api_key=5VhLWB6JaWs6m11mLfrNKply8naxchfbWb0Nu2Q9")
            .then(function (response) {
                return response.json();
            })
            .then(function (json) {
                debugger;
                if (!json.media_type === "image") { return; }
                var src = json.url;
                var container = document.getElementById("nasa-img-container");
                var img = document.createElement('img');
                img.src = src;
                container.title = json.explanation;
                container.appendChild(img);
                localStorage.setItem(key, JSON.stringify(json));
            });
    }
})();