function SearchUser() {
    $.ajax({
        type: "POST",
        url: "/Social/FindCustomerByUsername/",
        data: { 'name': $('#socialSearch').val()},
        success: function (id) {
            window.location.href = '/Social/Profile/' + id;
        },
        error: function (xhr) {
            alert('No user found.');
        }
    });
}