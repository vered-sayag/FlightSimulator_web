var drawPoint = function (lon, lat) {
    lon = ((lon + 180) * (window.innerWidth / 360));
     lat = ((lat + 90) * (window.innerHeight / 180));
    c.beginPath();
    c.arc(lon, lat, 6, 0, 2 * Math.PI);
    c.stroke();
    c.fillStyle = "red";
    c.fill();
    old_lon = lon;
    old_lat = lat;
}