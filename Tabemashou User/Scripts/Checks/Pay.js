oTable = $('#checkoutTable').DataTable({
    "pageLength": 5,
    "lengthChange": false,
});

$('#tableSearch').keyup(function () {
    oTable.search($(this).val()).draw();
});

$("#checkoutTable_filter").hide();