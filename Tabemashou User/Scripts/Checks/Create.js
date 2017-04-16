var dishesCheck = [];

function addDish(object) {
    var dishId = $(object).attr("dish-id");
    var dishPrice = $(object).attr("dish-price");
    var dishName = $("#name_" + dishId).html();
    var dishDesc = $("#desc_" + dishId).html();
    var cant = parseInt($("#cant_" + dishId).val());

    if (cant > 0) {
        $("#cant_" + dishId).val(cant + 1);
        var cell = $("#table").DataTable().cell($("#desc_cant_" + dishId));
        cell.data(cant + 1).draw();
    } else {
        var newDish = "<tr id='dish_" + dishId + "'>";
        newDish += "<input type='hidden' name='cant[]' id='cant_" + dishId + "' value='1' />";
        newDish += "<td id='desc_cant_" + dishId + "'>1</td>";
        newDish += "<td>" + dishName + "</td>";
        newDish += "<td>" + dishDesc + "</td>";
        newDish += "<td>" + dishPrice + "</td>";
        newDish += "<td><a class='btn btn-danger fa fa-minus' OnClick='removeDish(this)' dish-id='" + dishId + "'></a></td>";
        newDish += "</tr>";
        $("#table").DataTable().row.add($(newDish)[0]).draw();
        //$("#destiny-products").append(newDish);
    }
    dishesCheck.push(dishId);
    $("#UserDishes").val(dishesCheck);
}

function removeDish(object) {
    var dishId = $(object).attr("dish-id");
    var cant = parseInt($("#cant_" + dishId).val());
    if (cant > 1) {
        $("#cant_" + dishId).val(cant - 1);
        var cell = $("#table").DataTable().cell($("#desc_cant_" + dishId));
        cell.data(cant - 1).draw();
    } else {
        $("#table").DataTable().row($("#dish_" + dishId)).remove().draw();
    }
    delete dishesCheck[dishesCheck.indexOf(dishId)];
    $("#UserDishes").val(dishesCheck);
}

function initTable()
{
    $("#table").DataTable({
        "lengthChange": false,
        "pageLength": 5,
        "columnDefs": [{
            "targets": 4,
            "orderable": false
        }]
    });
}

function filterMenu(object) {
    var typeId = $(object).attr("id-type");
    var checked = $(object).prop('checked');
    $(".filter-menu").each(function() {
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

$(function() {
    initTable();
});

