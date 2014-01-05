(function () {
    $('#use_curr_location').click(function () {
        if (navigator.geolocation)
            navigator.geolocation.getCurrentPosition(handleGetCurrentPosition, onError);
    });

    $('form').submit(function (e) {
        e.preventDefault();
        codeAddress();
    });

})();

var latitude, longitude;
var city, street, streetnumber, region, address;
function handleGetCurrentPosition(location){

    latitude = location.coords.latitude;
    longitude = location.coords.longitude;

    var geocoder = new google.maps.Geocoder();
    var latLng = new google.maps.LatLng(latitude, longitude);

    if (geocoder) {
        geocoder.geocode({ 'latLng': latLng }, function (results, status) {
            if (status == google.maps.GeocoderStatus.OK) {
                city = results[0].address_components[2].short_name;
                street = results[0].address_components[1].short_name;
                streetnumber = results[0].address_components[0].short_name;

                region = results[0].address_components[3].long_name;

                $('.city').val(city);
                $('.street').val(street);
                $('.streetnumber').val(streetnumber);

                $(".region option").filter(function () {
                    return $(this).text() == region;
                }).prop('selected', true);
            }
            else {
                console.log("Geocoding failed: " + status);
            }
        });
    }
}

function onError()
{
    alert("Something went wrong!");
}

function codeAddress() {
    city = $('.city').val();
    street = $('.street').val();
    streetnumber = $('.streetnumber').val();

    address = street + " " + streetnumber + ", " + city + ", Belgie";

    geocoder = new google.maps.Geocoder();

    geocoder.geocode({ 'address': address }, function (results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            latitude = results[0].geometry.location.nb;
            longitude = results[0].geometry.location.ob;

            longitude = new String(longitude);
            longitude = longitude.replace(/\./g, ',');
            
            latitude = new String(latitude);
            latitude = latitude.replace(/\./g, ',');

            $('.latitude').val(latitude);
            $('.longitude').val(longitude);

            $("form")[0].submit();

        } else {
            $('.latitude').val(0);
            $('.longitude').val(0);
            $("form")[0].submit();
            //alert("Geocode was not successful for the following reason: " + status);
        }
    });
}