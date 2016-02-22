function updateItemCount(data) {
    alert(data.count);
}

function disableAddToCart(element) {
    var $this = $("#" + element.id);
    $this.addClass("disabled");
}

function isAvailableClassCheck($doc) {
    if ($doc.IsAvailable == true) {
        return "";
    }
    return "disabled";
}

function BuildInfoWindow($doc) {
    var contentString = '<div id="infoWindows" style="width: 400px; height: 300px"><div id="siteNotice"></div><h4>' + $doc.Name +
        '</h4><h5>Type : ' + $doc.Type +
        '</h5><div id="infoContent"><p> Publisher : ' + $doc.Publisher + '</p><p>Price : ' + $doc.Price + ' EUR</p><p>' + $doc.Description + '</p></div>' +
        '' +
        '' +
        //'<a class="btn btn-primary ' + isAvailableClassCheck($doc) + '" data-ajax="true" data-ajax-complete="disableAddToCart(this);" id="btn-' + $doc.Id + '" data-ajax-method="GET" data-ajax-mode="replace" data-ajax-update="update_ajax_element" href="/Market/AddToCampaign?itemId=' + $doc.Id + '">Add To Campaign</a></div>' +
        '<a class="btn btn-primary addButton ' + isAvailableClassCheck($doc) + '" id="' + $doc.Id + '">add</a>';
    return contentString;
}

function initialize() {

    var myLatlng = new google.maps.LatLng(49.192050599999990000, 16.61319090000006300);
    var mapOptions = {
        zoom: 9,
        center: myLatlng
    }
    var map = new google.maps.Map(document.getElementById("map-canvas"), mapOptions);

    var infowindow = new google.maps.InfoWindow({
        content: ""
    });

    $.get('/search/getmediawithtext/', { search: "" }, function (data) {

        for (var i = 0; i < data.Documents.length; i++) {
            var $doc = data.Documents[i];
            var latLon = new google.maps.LatLng($doc.Location.Latitude, $doc.Location.Longitude);
            var marker = new google.maps.Marker({
                position: latLon,
                title: data.Documents[i].Name
            });
            var infoContent = BuildInfoWindow($doc);
            bindInfoWindow(marker, map, infowindow, infoContent);
            marker.setMap(map);
        }
    });

    function bindInfoWindow(marker, map, infowindow, strDescription) {
        google.maps.event.addListener(marker, 'click', function () {
            infowindow.setContent(strDescription);
            infowindow.open(map, marker);
        });
    }
}

google.maps.event.addDomListener(window, 'load', initialize);

