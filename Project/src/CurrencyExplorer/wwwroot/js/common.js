var $menu;
var $settings;
var $wind;

var defSettingsBaseTop;
var defSettingsMenuTopDiff;
var defSettingsWidth;

// is added to defSettingsWidth to hide the shadow
var defSettingsHideValue = 20;

var isSettingsShown = true;
var isScrolledUnderMenu = false;

var drawChartHelpers;

// Chart constants
var NORMAL_MODE_MAX_POINTS_COUNT;
var MARGIN_LEFT;
var MARGIN_RIGHT;
var MARGIN_BOTTOM;
var MARGIN_TOP;

var CURRENCY_LABELS_RIGHT_MARGIN;

var TIGHT_POINTS_THRESHOLD;

$(document).ready(function () {

    resetChartConstants();

    $menu = $("#main-menu");
    $settings = $("#settings");
    $wind = $(window);

    // set handlers for explorer nav buttons

    $("#main-menu li").click(function (e) {
        var name = $(this).children("a").first().attr("data-name");
        MenuItemsClick(name);
        if (name === "settings") {
            e.preventDefault();
        }
    });

    $(".lang").click(function () {
        $(".lang").removeClass("lang-selected");
        $(this).toggleClass("lang-selected");
    });

    $("#main-menu ul li").hover(function () {
        $(this).toggleClass("menu-item-hover");
    });

    $("#settings-close").click(function (e) {
        $settings.css({
            left: -defSettingsWidth - defSettingsHideValue + "px"
        });
        e.preventDefault();
    });

    // menu and settings movements

    var menuOffset = $menu.offset().top;
    var settingsOffset = $settings.offset().top;
    defSettingsBaseTop = settingsOffset;
    var blocksTopDiff = settingsOffset - menuOffset;
    defSettingsMenuTopDiff = blocksTopDiff;

    defSettingsWidth = $settings.width();

    $settings.css({
        left: -defSettingsWidth - defSettingsHideValue + "px"
    });

    isSettingsShown = false;

    //$settings.css({
    //    height: $wind.height() - settingsOffset + "px"
    //});

    var isFixed = true;

    $wind.scroll(function () {
        if ($wind.scrollTop() > menuOffset && isFixed) {
            $menu.addClass("main-menu-floating");

            if (isSettingsShown) {
                $settings.css({
                    position: "fixed"
                });
            }
            isScrolledUnderMenu = true;
            isFixed = false;
        }
        else if ($wind.scrollTop() < menuOffset && !isFixed) {
            $menu.removeClass("main-menu-floating");

            if (isSettingsShown) {
                $settings.css({
                    position: "absolute"
                });
            }
            isScrolledUnderMenu = false;
            isFixed = true;
        }
    });


    // REMOVE
    //MenuItemsClick("settings");
});

function resetChartConstants() {
    NORMAL_MODE_MAX_POINTS_COUNT = 200;
    MARGIN_LEFT = 30;
    MARGIN_RIGHT = 0;
    MARGIN_BOTTOM = 50;
    MARGIN_TOP = 50;

    CURRENCY_LABELS_RIGHT_MARGIN = 80;

    TIGHT_POINTS_THRESHOLD = 20;
}

function validateInputDates(beginDate, endDate) {
    var result = true;

    if (isNaN(beginDate.getTime()) || isNaN(endDate.getTime())) {
        result = false;
    }

    return result;
}

function exportAsImage() {
    drawChartHelpers = true;
    loadChartData();

    //sendExportRequest("Export/ExportAsImage", "image/png");
}

function exportAsTable() {
    sendExportRequest("Export/ExportAsTable", "json");
}

function sendExportRequest(url, dataType) {
    hideAllInputErrors();

    var beginValue = $("#date-begin").val();
    var endValue = $("#date-end").val();

    var beginDate = new Date(beginValue);
    var endDate = new Date(endValue);

    var datesCorrect = validateInputDates(beginDate, endDate);

    if (datesCorrect) {
        if (beginDate < endDate) {

            var dataObj = packChartRequestObject(beginDate, endDate);

            //location.href = "Export/Test";

            $.ajax({
                type: "POST",
                url: url,
                dataType: dataType,
                data: "json=" + JSON.stringify(dataObj),
                success: function (data) {
                    if (data.Status === "OK") {
                        location.href = data.Path;
                    } else {
                        alert("Error");
                    }
                }
            });

            //document.location = "Export/Test/json=" + JSON.stringify(dataObj);


        } else {
            displayIncorrectPeriodError(false);
        }
    } else {
        displayInputDatesError(beginDate, endDate, false);
    }

}

function packChartRequestObject(begin, end) {

    var currencies = new Array();

    for (var i = 0; i < currenciesList.length; i++) {
        currencies.push(currenciesList[i].Value);
    }

    var dataObj = {
        Begin: begin.toUTCString(),
        End: end.toUTCString(),
        CurrencyValues: currencies
    };

    return dataObj;
}

function hideAllInputErrors() {
    hideInputError("date-begin");
    hideInputError("date-end");
}

function displayIncorrectPeriodError(showChartError) {
    showInputError("date-begin", "Begin date should be earlier that end date.");

    hideLoading();

    if (showChartError === true) {
        showLoadingError();
        showMessageContainer();
    }
}

function displayInputDatesError(beginDate, endDate, showChartError) {
    hideLoading();

    if (showChartError === true) {
        showLoadingError();
        showMessageContainer();
    }

    if (isNaN(beginDate.getTime())) {
        showInputError("date-begin", "Incorrect date fromat.");
    }
    if (isNaN(endDate.getTime())) {
        showInputError("date-end", "Incorrect date fromat.");
    }
}

function showInputError(id, text) {
    var inp = $("#" + id);

    inp.parent().parent().find(".error-description").removeClass(".hidden").addClass(".shown").text(text);
    if (inp.hasClass("input-error") === false) {
        inp.addClass("input-error");
    }
}

function hideInputError(id) {
    var inp = $("#" + id);
    inp.parent().parent().find(".error-description").removeClass(".shown").addClass(".hidden").text("");
    if (inp.hasClass("input-error")) {
        inp.removeClass("input-error");
    }
}

function showLoadingError() {
    var err = $("#loading-error");

    if (err.hasClass("hidden")) {
        err.removeClass("hidden");
        err.addClass("shown");

    }
}

function hideLoadingError() {
    var err = $("#loading-error");

    if (err.hasClass("shown")) {
        err.removeClass("shown");
        err.addClass("hidden");
    }
}

function showMessageContainer() {
    var cont = $("#message-container");

    if (cont.hasClass("hidden")) {
        cont.removeClass("hidden");
        cont.addClass("shown");
    }
}

function hideMessageContainer() {
    var cont = $("#message-container");

    if (cont.hasClass("shown")) {
        cont.removeClass("shown");
        cont.addClass("hidden");
    }
}


function showLoading() {
    var loading = $("#loading-animation");

    if (loading.hasClass("hidden")) {
        loading.removeClass("hidden");
        loading.addClass("shown");
    }

}

function hideLoading() {
    var loading = $("#loading-animation");

    if (loading.hasClass("shown")) {
        loading.removeClass("shown");
        loading.addClass("hidden");
    }

}


function MenuItemsClick(name) {
    switch (name) {
        case "settings":
            {
                var pos = "absolute";
                var offsetTop = defSettingsMenuTopDiff;

                if (isScrolledUnderMenu) {
                    pos = "fixed";
                    offsetTop = defSettingsMenuTopDiff;
                }

                $("#settings").css({
                    left: "0px",
                    position: pos,
                    top: offsetTop + "px"
                });

                isSettingsShown = true;
                break;
            }
        case "explore":
            {
                $wind.scrollTop(0, 200);
                break;
            }
    }
}