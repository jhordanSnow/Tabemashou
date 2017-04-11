$(function () {
    var selectType = "#restTypesId";
    $(selectType).select2({
        placeholder: 'Select a Type',
        multiple: true,
        width: '100%'
    });

    $("#createType").click(function () {
        var nameType = $("#tipos_Name").val();
        $.ajax({
            type: "POST",
            url: "/Types/CreateType/",
            data: { 'name': nameType },
            success: function (typeId) {
                $('#modalAddType').modal('hide');
                $(selectType).append($('<option>', {
                    value: typeId,
                    text: nameType
                }));
                $(selectType).select2();
                $("#tipos_Name").val("");
            },
            error: function (xhr) {
                alert('There is another type with that name or name can\'t be null.');
            }
        });
    });
});

function uploadImage() {
    $("#image").click();
    $("#image").change(function() {
        preview(this, $("#preImage"));
    });
}

function preview(input, target) {

    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            target.attr("src", e.target.result);
        }

        reader.readAsDataURL(input.files[0]);
    }
}