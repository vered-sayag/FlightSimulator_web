var xmlRead = function (xml) {
    var xmlDoc = $.parseXML(xml);
    $xml = $(xmlDoc);

    lon = parseFloat($xml.find("Lon").text());

    lat = parseFloat($xml.find("Lat").text());
}