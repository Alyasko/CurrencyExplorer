var currenciesList = new Array();

$(document).ready(function() {
    $(".currency-item").each(function(i) {
        var curItem = new Object();
        var curObject = $(this);

        curItem.Name = curObject.find(".currency-name").text();
        curItem.Value = curObject.attr("data-value");

        currenciesList[currenciesList.length] = curItem;
    });
});

function addCurrency() {
    var selectedOption = $("#new-currency option:selected");

    var val = selectedOption.attr("name");
    var name = selectedOption.text();

    var exists = false;

    for (var i = 0; i < currenciesList.length; i++) {
        var curCurrency = currenciesList[i];
        if (curCurrency.Value === val) {
            exists = true;
            break;
        }
    }

    if (exists === false) {
        var curItem = new Object();
        curItem.Value = val;
        curItem.Name = name;

        currenciesList[currenciesList.length] = curItem;

        drawNewCurrencyItem(curItem);
    } else {
        alert("Item already exist.");
    }
}

function drawNewCurrencyItem(item) {
    var itemIndex = currenciesList.length - 1;
    var html = '<div id="ci-' + itemIndex + '" class="currency-item" data-value="' + item.Value + '">' +
        '<div onclick="deleteCurrency(' + itemIndex + ')" class="delete currency-list-button">' +
        '<svg fill="#FFFFFF" class="currency-list-button-svg" width="26" height="26" viewBox="0 0 24 24">' +
        '<path d="M12,20C7.59,20 4,16.41 4,12C4,7.59 7.59,4 12,4C16.41,4 20,7.59 20,12C20,16.41 16.41,20 12,20M12,2C6.47,2 2,6.47 2,12C2,17.53 6.47,22 12,22C17.53,22 22,17.53 22,12C22,6.47 17.53,2 12,2M14.59,8L12,10.59L9.41,8L8,9.41L10.59,12L8,14.59L9.41,16L12,13.41L14.59,16L16,14.59L13.41,12L16,9.41L14.59,8Z"/>' +
        '</svg>' +
        '</div>' +
        '<div class="currency-name">' + item.Name + '</div>' +
        '</div>';

    $("#currencies-list").append(html);
}

function deleteCurrency(id) {
    var itemToDelete = $("#ci-" + id);
    itemToDelete.remove();
    currenciesList.splice(id);
}

