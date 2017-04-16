oTable = $('#checkoutTable').DataTable({
    "pageLength": 5,
    "lengthChange": false,
});

$('#tableSearch').keyup(function () {
    oTable.search($(this).val()).draw();
});

$("#checkoutTable_filter").hide();


$(".slide-text").change(function() {
    var id = $(this).attr("data-id");
    var val = $(this).val();
    $("slide_" + id).slider('value', 50);
    $("slide_" + id).change();
})

$(function () {
    var sliders = $(".slider");
    var maxValue = parseFloat($("#max_slide").val()) + 0.1;
    sliders.each(function () {
        var value = parseFloat($(this).text(), 10),
            availableTotal = maxValue;
        var id = $(this).attr("user-id");

        $(this).empty().slider({
            value: 0,
            min: 0,
            max: maxValue,
            range: "max",
            step: 0.1,
            animate: 100,
            slide: function (event, ui) {
                // Update display to current value
                $(this).siblings().text(ui.value);

                // Get current total
                var total = 0;

                sliders.not(this).each(function () {
                    total += $(this).slider("option", "value");
                });

                // Need to do this because apparently jQ UI
                // does not update value until this event completes
                total += ui.value;

                var max = availableTotal - total;
                $("#balance_check").html("$" + (-1 * (max - 0.1)).toFixed(2));
                $("#balance").val((-1 * (max - 0.1)).toFixed(2));
                // Update each slider
                sliders.not(this).each(function () {
                    var t = $(this),
                        value = t.slider("option", "value");

                    t.slider("option", "max", max + value)
                        .siblings().text(value);
                    t.slider('value', value);
                });
            },
            change: function (event, ui) {
                $("#lbl_" + id).attr('value', ui.value);
            }
        });
    });
});

function pay() {
    var balance = parseFloat($("#balance").val());
    if (balance === 0) {
        $("#real_submit_button").click();
    } else {
        alert("The Balance must be 0");
    }
}