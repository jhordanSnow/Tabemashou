
$(function () {
	var menuItem = $('#left-sidebar-menu-account');
    menuItem.addClass('active');
    var subMenuItem = menuItem.find('#left-sidebar-menu-account-userprofile');
    subMenuItem.addClass('active');

    $("#BirthDate").datepicker({ format: "yyyy-mm-dd" });
    $(".rating").rating({ disabled: true, showCaption: false, showClear: false, });
});
function uploadImage() {
    $("#image").click();
    $("#image").change(function () {
        changePhoto(this, $("#preImage"));
    });
    
}

function changePhoto(input, target) {

    var $file = input,
        $formData = new FormData();

    if ($file.files.length > 0) {
        for (var i = 0; i < $file.files.length; i++) {
            $formData.append('file-' + i, $file.files[i]);
        }
    }

    $.ajax({
        url: '/Account/ChangePhoto',
        type: 'POST',
        data: $formData,
        dataType: 'json',
        contentType: false,
        processData: false,
        success: function ($data) {
            location.reload();
        }
    });
}