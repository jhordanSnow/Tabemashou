﻿var finalFiles = [];
var deletedFiles = [];

$(function () {
    $('#TablesDataTable').DataTable({
        columnDefs: [
            { width: 150, targets: [0,1] }
        ],
        fixedColumns: true
    });
    var selectType = "#local_IdDistrict";
    $(selectType).select2({
        placeholder: 'Select a District',
        width: '100%'
    });

    var myLatLng = { lat: parseFloat($("#local_Latitude").val()), lng: parseFloat($("#local_Longitude").val())}

    $('#searchMap').geocomplete({
        map: "#Mapau",
        mapOptions: {
            scrollwheel: true,
            center: myLatLng
        },
        markerOptions: {
            draggable: true,
            position: myLatLng
        }
    });


    $("#searchMap").bind("geocode:result", function (event, result) {
        $("#local_Latitude").val(result.geometry.location.lat());
        $("#local_Longitude").val(result.geometry.location.lng());
    });
    $("#searchMap").bind("geocode:dragged", function (event, latLng) {
        $("#local_Latitude").val(latLng.lat());
        $("#local_Longitude").val(latLng.lng());
    });

    $(".todo-list input:checkbox").each(function () {
        var $this = $(this);
        if (!this.checked) {
            $this.parent().addClass("done");
        }
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

            var content = "<div class='col-md-3' style='overflow: hidden; height: 250px;margin-bottom:10px;' id='image_" + index +"'>";
            content += "<a class='fa fa-remove btn btn-danger' style='float:right;position: absolute;' OnClick='deleteImage(this)' index-element='" + index+"'></a>";
            content += "<img class='img-responsive img-rounded' src='" + objectUrl + "' />";
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


function deleteImageId(object) {
    var index = $(object).attr("index-element");
    $("#image_" + index + "").remove();
    deletedFiles.push(index);
    $("#deletedFilesIds").val(deletedFiles);
}
