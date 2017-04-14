$(function () {
    $('input').iCheck({
        checkboxClass: 'icheckbox_square-blue',
        radioClass: 'iradio_square-blue',
        increaseArea: '20%' // optional
    });
    $("#BirthDate").datepicker({ format: "yyyy-mm-dd" }).val('');
});


function uploadImage() {
    $("#image").click();
    $("#image").change(function () {
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