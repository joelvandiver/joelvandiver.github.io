(function () {
    const date = new Date();
    const today = date.toISOString().split("T")[0]
    const key = "APOD_RESULT";
    const last = localStorage.getItem(key);
    const lastDate = last ? JSON.parse(last) : undefined;

    function renderImg(json) {
        // Fallback on Jupiter if the media_type is not an image.
        var src = json.media_type === "image" ?
            json.url :
            "https://www.nasa.gov/sites/default/files/styles/full_width_feature/public/thumbnails/image/pia22949.jpg";
        var container = document.getElementById("nasa-img-container");
        var img = document.createElement('img');
        img.src = src;
        container.title = json.explanation;
        container.appendChild(img);
        localStorage.setItem(key, JSON.stringify(json));
    }

    if (!lastDate || lastDate.date !== today) {
        fetch("https://api.nasa.gov/planetary/apod?api_key=5VhLWB6JaWs6m11mLfrNKply8naxchfbWb0Nu2Q9")
            .then(function (response) {
                return response.json();
            })
            .then(renderImg);
    } else {
        renderImg(last);
    }
})();