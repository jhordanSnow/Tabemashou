$(function () {

    $("#table_tags").DataTable({
        "lengthChange": false,
        "showEntries": false,
        "bInfo": false,
        "pageLength": 5,
        "columnDefs": [{
            "targets": 1,
            "orderable": false
        }]
    });

    var selectType = "#IdDistrict";
    $(selectType).select2({
        placeholder: 'Select a District',
        multiple: true,
        width: '100%'
    });

    var locations = [];

    $(".info_local").each(function () {
        var input = $(this);
        var contentLocation = [input.val(), input.attr("data-lat"), input.attr("data-lng")];
        locations.push(contentLocation);
    });
    
    var map = new google.maps.Map(document.getElementById('Mapau'), {
        zoom: 10,
        center: new google.maps.LatLng(9.9333300, -84.0833300),
        mapTypeId: google.maps.MapTypeId.ROADMAP
    });

    var infowindow = new google.maps.InfoWindow();

    var marker, i;

    for (i = 0; i < locations.length; i++) {
        marker = new google.maps.Marker({
            position: new google.maps.LatLng(locations[i][1], locations[i][2]),
            map: map
        });

        google.maps.event.addListener(marker, 'click', (function (marker, i) {
            return function () {
                infowindow.setContent($("#content_" + locations[i][0]).html());
                infowindow.open(map, marker);
            }
        })(marker, i));
    }
});
