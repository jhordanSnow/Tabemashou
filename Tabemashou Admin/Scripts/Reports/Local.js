$(function() {

    $("#RestaurantId").select2({
        placeholder: 'Select a Restaurant',
        width: '100%'
    });

    var start = moment($("#DateStart").val());
    var end = moment($("#DateEnd").val());

    $("#DateStart").val(start.format('YYYY-MM-DD'));
    $("#DateEnd").val(end.format('YYYY-MM-DD'));

    function cb(start, end) {
        $('#reportrange span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
        $("#DateStart").val(start.format('YYYY-MM-DD'));
        $("#DateEnd").val(end.format('YYYY-MM-DD'));
    }

    $('#reportrange').daterangepicker({
        startDate: start,
        endDate: end,
        ranges: {
            'Today': [moment(), moment()],
            'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
            'Last 7 Days': [moment().subtract(6, 'days'), moment()],
            'Last 30 Days': [moment().subtract(29, 'days'), moment()],
            'This Month': [moment().startOf('month'), moment().endOf('month')],
            'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
        }
    }, cb);

    cb(start, end);
    

});


function filterMenu(object) {
    var typeId = $(object).attr("id-type");
    var checked = $(object).prop('checked');
    $(".filter-menu").each(function () {
        var filter_items = $(this).attr("data-filter").split(",");
        var filter_default = $(this).attr("data-filter-default").split(",");

        if (checked && filter_default.indexOf(typeId) >= 0) {
            if (filter_items.length == 0) {
                filter_items = typeId;
            } else {
                filter_items.push(typeId);
            }
        } else {
            filter_items = jQuery.grep(filter_items, function (value) {
                return value != typeId;
            });
        }

        if (filter_items == "") {
            $(this).hide();
        } else {
            $(this).show();
        }
        $(this).attr("data-filter", filter_items);
    });

}