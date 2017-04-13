var finalFiles = [];

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

    var inputLocalFont = document.getElementById("uploadFiles");
    inputLocalFont.addEventListener("change", previewImages, false);

    function previewImages() {
        var fileList = this.files;

        var anyWindow = window.URL || window.webkitURL;

        for (var i = 0; i < fileList.length; i++) {
            var objectUrl = anyWindow.createObjectURL(fileList[i]);
            finalFiles.push(fileList[i].name);
            var index = finalFiles.indexOf(fileList[i].name);

            var content = "<div class='col-md-3' id='image_" + index +"'>";
            content += "<a class='fa fa-trash' OnClick='deleteImage(this)' index-element='" + index+"'></a>";
            content += "<img class='img-responsive' src='" + objectUrl + "' />";
            content += "</div>";

            $('#contentImages').append(content);
            window.URL.revokeObjectURL(fileList[i]);
        }

        $(inputLocalFont).hide();
        inputLocalFont = $('<input type="file" name="files[]" accept=".jpg,.jpeg,.png" multiple />').appendTo("#filecontainer").get(0);
        inputLocalFont.addEventListener("change", previewImages, false);
        $("#uploadFilesNames").val(finalFiles);
    }
});


function deleteImage(object) {
    var index = $(object).attr("index-element");
    $("#image_" + index + "").remove();
    delete finalFiles[index];
    $("#uploadFilesNames").val(finalFiles);
}
