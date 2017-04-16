function SearchUser() {
    $.ajax({
        type: "POST",
        url: "/Social/FindCustomerByUsername/",
        data: { 'name': $('#socialSearch').val() },
        dataType: "json",
        success: function (id) {
            location.href = '/Social/Profile/' + id;
        },
        error: function (xhr) {
            alert('No user found.');
        }
    });
}