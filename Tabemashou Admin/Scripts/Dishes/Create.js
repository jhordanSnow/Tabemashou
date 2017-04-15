
$(function () {
    
    var selectType = "#restTypesId";
    $(selectType).select2({
        placeholder: 'Select a Type',
        multiple: true,
        width: '100%'
    });

    $(selectType).select2("val", null);

    $("#createType").click(function () {
        var nameType = $("#tipos_Name").val();
        $.ajax({
            type: "POST",
            url: "/Types/Create/",
            data: { 'name': nameType },
            success: function (typeId) {
                $('#modalAddType').modal('hide');
                $(selectType).append($('<option>', {
                    value: typeId,
                    text: nameType
                }));
                $(selectType).select2({ width: '100%' });
                $("#tipos_Name").val("");
            },
            error: function (xhr) {
                alert('There is another type with that name or name can\'t be null.');
            }
        });
    });
});

var finalFiles = [];
$(function () {
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
