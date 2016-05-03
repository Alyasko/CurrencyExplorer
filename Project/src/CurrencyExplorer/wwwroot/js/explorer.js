var parsedChartJsonData = null;
var currencyLblsStatus = new Array();
var currencyPopupPoints = new Array();
var currencyLblsHandlerAssigned = false;

var CANVAS_ID = "canvas-explorer-chart";
var CHART_POPUP_ID = "chart-popup";

var SCROLLBAR_MARGIN = 40;
var CHART_LEFT_RIGHT_MARGIN = 40;
var CANVAS_PAGE_MARGINS = 130;


var isScrollBarDragging;
var scrollBarPosDifference;
var mousePosition = new Object();
var chartScrollingValue;
var totalChartWidth;

$(document).ready(function () {
    updateCanvasWidth(CANVAS_ID);
    loadChartData();

    isScrollBarDragging = false;
    scrollBarPosDifference = 0;

    chartScrollingValue = 0;

    $("#" + CANVAS_ID).mousedown(scrollBarMouseDown);
    $("#" + CANVAS_ID).mouseup(scrollBarMouseUp);
    $("#" + CANVAS_ID).mousemove(scrollBarMouseMove);
});

$(window).resize(function () {
    if (parsedChartJsonData != null) {
        updateCanvasWidth(CANVAS_ID);
        drawChart(null, CANVAS_ID, false, chartScrollingValue);
    }
});

function scrollBarMouseMove(e) {
    if (isScrollBarDragging) {
        var x = e.pageX - scrollBarPosDifference;

        scrollChartAbsolute(x);
    } else {
        checkDataPoints(e);
    }
}

function checkDataPoints(e) {
    var HIT_TEST_RADIUS = 10;

    var canvas = $("#" + CANVAS_ID);
    var canvasLeft = canvas.offset().left;
    var canvasTop = canvas.offset().top;
    var canvasHeight = canvas.height();

    var popup = $("#" + CHART_POPUP_ID);

    var isHit = false;
    var lastTop = 0;
     
    for (var i = 0; i < currencyPopupPoints.length; i++) {
        var obj = currencyPopupPoints[i];
        var currencyData = obj.Object;

        var pX = obj.X;
        var pY = obj.Y;

        var mX = e.pageX - canvasLeft;
        var mY = canvasTop + canvasHeight - e.pageY;

        lastTop = canvasHeight - pY - popup.height() - 20;

        if ((pX - mX) * (pX - mX) + (pY - mY) * (pY - mY) <= (HIT_TEST_RADIUS * HIT_TEST_RADIUS)) {
            $("#currency-name").text(currencyData.Name);
            $("#currency-value").text(currencyData.Value);
            $("#currency-date").text(currencyData.ActualDate);

            popup.removeClass("popup-hidden");
            popup.addClass("popup-shown");
            popup.css({
                left: canvasLeft + pX - popup.width() / 2 + "px",
                top: lastTop + "px"
            });
            isHit = true;
        }
    }

    if (isHit === false) {
        popup.removeClass("popup-shown");
        popup.addClass("popup-hidden");
    }
}

function scrollBarMouseDown(e) {
    isScrollBarDragging = true;
    //alert($("#" + CANVAS_ID).offset().left);
    scrollBarPosDifference = e.pageX - chartScrollingValue;
}

function scrollBarMouseUp() {
    isScrollBarDragging = false;
}



function updateCanvasWidth(id) {
    var windowWidth = $(document).width();
    var canvas = document.getElementById(id);
    canvas.width = windowWidth - CANVAS_PAGE_MARGINS * 2;
}

function checkPeriodDates(beginValue, endValue) {
    var result = true;

    var beginDate = new Date(beginValue);
    var endDate = new Date(endValue);

    if (isNaN(beginDate.getTime()) || isNaN(endDate.getTime())) {
        result = false;
    }

    return result;
}

function loadChartData() {
    loadingShow();

    var beginValue = $("#date-begin").val();
    var endValue = $("#date-end").val();

    var beginDate = new Date(beginValue);
    var endDate = new Date(endValue);

    var isCorrectPeriod = true;

    if (isNaN(beginDate.getTime()) || isNaN(endDate.getTime())) {
        isCorrectPeriod = false;
    }

    if (isCorrectPeriod === true) {
        if (beginDate < endDate) {
            var begin = beginDate;
            var end = endDate;

            var currencies = new Array("USD");

            var dataObj = {
                Begin: begin.toUTCString(),
                End: end.toUTCString(),
                Currencies: currencies
            };

            $.ajax({
                type: "POST",
                dataType: "json",
                url: "Chart/LoadChartData",
                data: "json=" + JSON.stringify(dataObj),
                success: function(d) {
                    var parsedData = $.parseJSON(d);

                    var state = parsedData["State"];
                    var data = parsedData["Data"];

                    if (state === "Success") {
                        drawChart(data, CANVAS_ID, true, 0);
                    } else if (state === "Failed") {
                        alert(data);
                    }

                    loadingHide();
                },
                failure: function(err) {
                    alert(err);
                }
            });
        } else {
            alert("Begin date cannot be later than end date.");
        }
    } else {
        alert("Incorrect date. I will not load data.");
    }
}

function loadingShow() {
    var cont = $("#loading-container");
    if (cont.hasClass("loading-hidden")) {
        cont.removeClass("loading-hidden");
        cont.addClass("loading-shown");
    }
}

function loadingHide() {
    var cont = $("#loading-container");
    if (cont.hasClass("loading-shown")) {
        cont.removeClass("loading-shown");
        cont.addClass("loading-hidden");
    }
}

function getDrawingObject(jData) {
    return drawData;
}

function drawChart(jData, cnv, parseJson, horTransl) {
    if (parseJson == true) {
        parsedChartJsonData = jData;
    }
    var data = parsedChartJsonData;

    var chart = $("#canvas-explorer-chart");
    currencyPopupPoints = new Array();

    var NORMAL_MODE_MAX_POINTS_COUNT = 200;
    var MARGIN_LEFT = 0;
    var MARGIN_RIGHT = 0;
    var MARGIN_BOTTOM = 50;
    var MARGIN_TOP = 50;

    var CURRENCY_LABELS_RIGHT_MARGIN = 80;

    var TIGHT_POINTS_THRESHOLD = 20;

    var canvas = document.getElementById(cnv);
    var ctx = canvas.getContext("2d");

    var cnvHeight = canvas.clientHeight;
    var cnvWidth = canvas.clientWidth;


    var maxValue = findChartMax(data);
    var minValue = findChartMin(data);

    var diffValue = maxValue - minValue;

    // For scrolling.
    var horTranslation = horTransl;

    $("#bar").text(horTranslation);

    //ctx.scale(1, -1);
    ctx.setTransform(1, 0, 0, -1, 0, 0);
    ctx.translate(0, -cnvHeight);

    ctx.clearRect(0, 0, cnvWidth, cnvHeight);

    ctx.strokeStyle = "black";
    ctx.strokeWidth = 1;

    ctx.font = "20px Segoe UI";


    for (var currency in data) {
        var chartPointsCount = data[currency].length;

        if (chartPointsCount > 0) {

            totalChartWidth = 0;

            var pointHStep = (cnvWidth - MARGIN_LEFT - MARGIN_RIGHT) / chartPointsCount;
            var pointVStep = (cnvHeight - MARGIN_TOP - MARGIN_BOTTOM) / diffValue;

            if (pointHStep < TIGHT_POINTS_THRESHOLD) {
                // If points are too tight.
                pointHStep = TIGHT_POINTS_THRESHOLD;
            }

            var x = 0;
            var y = 0;

            ctx.beginPath();

            var visiblePointNumber = 0;

            for (var j = chartPointsCount - 1; j >= 0; j--) {
                var chartPoint = data[currency][j];

                x = pointHStep / 2 + pointHStep * j + MARGIN_LEFT + horTranslation;
                y = pointVStep * (chartPoint.Value - minValue) + MARGIN_BOTTOM;

                totalChartWidth += pointHStep;



                if (x > 0 && x < cnvWidth) {

                    if (j !== chartPointsCount - 1) {
                        ctx.lineTo(x, y);
                    } else {
                        ctx.moveTo(x, y);
                    }

                    if (visiblePointNumber == 0) {
                        //alert(visiblePointNumber);

                        var lblFor = "lbl-for-" + currency.toLowerCase();

                        if (currencyLblsStatus.indexOf(currency) == -1) {
                            // First point.
                            chart.before('<div class="chart-currency-label" data-descr="' + chartPoint.Name + '" id="' + lblFor + '">' + currency + '</div>');
                        }

                        $("#" + lblFor).css({
                            right: CURRENCY_LABELS_RIGHT_MARGIN + 'px',
                            bottom: (y - 6) + 'px'
                        });

                        visiblePointNumber++;

                        currencyLblsStatus[currencyLblsStatus.length] = currency;
                    }

                    var popupObject = new Object();

                    popupObject.X = x;
                    popupObject.Y = y;
                    popupObject.Object = chartPoint;

                    currencyPopupPoints[currencyPopupPoints.length] = popupObject;
                }
            }

            totalChartWidth += pointHStep;

            ctx.stroke();
            ctx.closePath();
        }
    }



    if (currencyLblsHandlerAssigned == false) {
        currencyLblsHandlerAssigned = true;

        $(".chart-currency-label").hover(function () {
            var descr = $(this).attr("data-descr");
            var alias = $(this).text();

            $(this).text(descr);
            $(this).attr("data-descr", alias);

        }, function () {
            var descr = $(this).attr("data-descr");
            var alias = $(this).text();

            $(this).text(descr);
            $(this).attr("data-descr", alias);
        });
    }
}

function findChartMax(drawData) {
    var max = -1;

    for (var currency in drawData) {
        for (var point in drawData[currency]) {
            if (drawData[currency][point].Value > max)
                max = drawData[currency][point].Value;
        }
    }

    return max;
}

function findChartMin(drawData) {
    var min = 1000000;

    for (var currency in drawData) {
        for (var point in drawData[currency]) {
            if (drawData[currency][point].Value < min)
                min = drawData[currency][point].Value;
        }
    }

    return min;
}

function scrollChartAbsolute(amount) {
    var cnvWidth = $("#" + CANVAS_ID).width();
    if (amount < (0 + CHART_LEFT_RIGHT_MARGIN) && amount > -(totalChartWidth + CHART_LEFT_RIGHT_MARGIN - cnvWidth)) {
        chartScrollingValue = amount;
    }

    drawChart(null, CANVAS_ID, false, chartScrollingValue);
}

function scrollChartLeft(amount) {
    var tempScrollingValue = chartScrollingValue + amount;
    if (tempScrollingValue < (0 + CHART_LEFT_RIGHT_MARGIN)) {
        chartScrollingValue += amount;
    } else {
        chartScrollingValue = (0 + CHART_LEFT_RIGHT_MARGIN);
    }

    drawChart(null, CANVAS_ID, false, chartScrollingValue);
}

function scrollChartRight(amount) {
    var cnvWidth = $("#" + CANVAS_ID).width();
    var tempScrollingValue = chartScrollingValue - amount;
    if (tempScrollingValue > -(totalChartWidth + CHART_LEFT_RIGHT_MARGIN - cnvWidth)) {
        chartScrollingValue = tempScrollingValue;
    } else {
        chartScrollingValue = -(totalChartWidth + CHART_LEFT_RIGHT_MARGIN - cnvWidth);
    }

    drawChart(null, CANVAS_ID, false, chartScrollingValue);
}


