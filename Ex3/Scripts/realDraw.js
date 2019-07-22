var realDraw = function (c,color) {
    var temp_lon = ((lon + 180) * (window.innerWidth / 360));
    var temp_lat = ((lat + 90) * (window.innerHeight / 180));
    c.beginPath();
    c.lineWidth = 1;
    c.moveTo(old_lon, old_lat);
    c.lineTo(temp_lon, temp_lat);
    c.strokeStyle = color;
    c.stroke();
    c.closePath();
    old_lon = temp_lon;
    old_lat = temp_lat;
}