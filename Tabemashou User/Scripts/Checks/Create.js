var dishesCheck = [];

function addDish(object) {
    var dishId = $(object).attr("dish-id");
    var dishPrice = $(object).attr("dish-price");
    var dishName = $("#name_" + dishId).html();
    var dishDesc = $("#desc_" + dishId).html();
    var cant = parseInt($("#cant_" + dishId).val());

    if (cant > 0) {
        $("#cant_" + dishId).val(cant + 1);
        $("#desc_cant_" + dishId).html(cant + 1);
    } else {
        var newDish = "<tr id='dish_" + dishId + "'>";
        newDish += "<input type='hidden' name='cant[]' id='cant_" + dishId + "' value='1' />";
        newDish += "<td id='desc_cant_" + dishId + "'>1</td>";
        newDish += "<td>" + dishName + "</td>";
        newDish += "<td>" + dishDesc + "</td>";
        newDish += "<td>" + dishPrice + "</td>";
        newDish += "<td><a class='btn btn-danger fa fa-minus' OnClick='removeDish(this)' dish-id='" + dishId + "'></a></td>";
        newDish += "</tr>";
        $("#destiny-products").append(newDish);
    }
    dishesCheck.push(dishId);
    $("#UserDishes").val(dishesCheck);
}

function removeDish(object) {
    var dishId = $(object).attr("dish-id");
    var cant = parseInt($("#cant_" + dishId).val());
    if (cant > 1) {
        $("#cant_" + dishId).val(cant - 1);
        $("#desc_cant_" + dishId).html(cant - 1);
    } else {
        $("#dish_" + dishId).remove();
    }
    delete dishesCheck[dishesCheck.indexOf(dishId)];
    $("#UserDishes").val(dishesCheck);
}

