$(function () {
    var selectType = "#local_IdDistrict";
    $(selectType).select2({
        placeholder: 'Select a District',
        width: '100%'
    });

    $(selectType).select2("val", null);

    $('#searchMap').geocomplete({
        map: "#Mapau",
        mapOptions: {
            scrollwheel: true
        },
        markerOptions: {
            draggable: true
        },
        location: "CR SJ"
    });


    $("#searchMap").bind("geocode:result", function (event, result) {
        $("#local_Latitude").val(result.geometry.location.lat());
        $("#local_Longitude").val(result.geometry.location.lng());
    });
    $("#searchMap").bind("geocode:dragged", function (event, latLng) {
        $("#local_Latitude").val(latLng.lat());
        $("#local_Longitude").val(latLng.lng());
    });

    /* The todo list plugin */
    $(".todo-list").todolist({
        onCheck: function (ele) {
            window.console.log("The element has been checked");
            return ele;
        },
        onUncheck: function (ele) {
            window.console.log("The element has been unchecked");
            return ele;
        }
    });
});