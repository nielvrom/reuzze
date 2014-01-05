(function () {
    google.maps.event.addDomListener(window, 'load', initialize);
})();

var map;
function initialize() {
    var mapOptions = {
        zoom: 8,
        center: new google.maps.LatLng(50.85, 4.35) // BELGIUM
    };
    map = new google.maps.Map(document.getElementById('map-canvas'),
        mapOptions);

    // Add markers to the map
    addMarkers(model, map);
}

var latitude, longitude, myLatlng;
function addMarkers(model, map) {
    $.each(model, function (index, value) {
        latitude = value.Latitude;
        longitude = value.Longitude;

        if (latitude != 0 && longitude != 0) {
            myLatlng = new google.maps.LatLng(latitude, longitude);
            var marker = new google.maps.Marker({
                position: myLatlng,
                map: map
            });
        }
    });
}
