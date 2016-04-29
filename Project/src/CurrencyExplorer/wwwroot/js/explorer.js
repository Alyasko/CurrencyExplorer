var parsedChartJsonData = null;
var currencyLblsStatus = new Array();
var currencyLblsHandlerAssigned = false;
var CANVAS_ID = "canvas-explorer-chart";
var SCROLLBAR_MARGIN = 40;

var isScrollBarDragging;
var scrollBarPosDifference;
var mousePosition = new Object();
var chartScrollingValue;

$(document).ready(function () {
    updateCanvasWidth(CANVAS_ID);
    loadChartData(0, 0, 0);

    isScrollBarDragging = false;
    scrollBarPosDifference = 0;

    chartScrollingValue = 0;

    $("#bar").mousedown(scrollBarMouseDown);
    $("#bar").mouseup(scrollBarMouseUp);
    $(document).mousemove(scrollBarMouseMove);
});

$(window).resize(function () {
    if (parsedChartJsonData != null) {
        updateCanvasWidth(CANVAS_ID);
        drawChart(null, CANVAS_ID, false, chartScrollingValue);
    }
});

function scrollBarMouseMove(e) {
    if (isScrollBarDragging) {
        var windowWidth = $(document).width();
        var bar = $("#bar");
        var x = e.pageX - scrollBarPosDifference;
        if (x >= (SCROLLBAR_MARGIN + 2) && (x + bar.width() + 18) <= (windowWidth - SCROLLBAR_MARGIN)) {
            bar.css("left", x);
        }
    }
}

function scrollBarMouseDown(e) {
    isScrollBarDragging = true;
    scrollBarPosDifference = e.pageX - $("#bar").position().left;
}

function scrollBarMouseUp() {
    isScrollBarDragging = false;
}



function updateCanvasWidth(id) {
    var windowWidth = $(document).width();
    var canvas = document.getElementById(id);
    canvas.width = windowWidth - 300;
}

function loadChartData(begin, end, currencies) {
    // PROBLEM
    /*
    begin = new Date(2016, 4, 1);
    end = new Date();
    */

    begin = new Date(2016, 1, 22);
    end = new Date(2016, 3, 22);
    currencies = new Array("USD", "AUD", "EUR");

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
        success: function (d) {
            drawChart(d, CANVAS_ID, true, 0);
        },
        failure: function (err) {
            alert(err);
        }
    });
}

function getDrawingObject(jData) {
    return drawData;
}

function drawChart(jData, cnv, parseJson, horTransl) {
    if (parseJson == true) {
        parsedChartJsonData = $.parseJSON(jData);
    }
    var data = parsedChartJsonData;

    var chart = $("#canvas-explorer-chart");

    var NORMAL_MODE_MAX_POINTS_COUNT = 200;
    var MARGIN_LEFT = 20;
    var MARGIN_RIGHT = 20;
    var MARGIN_BOTTOM = 50;
    var MARGIN_TOP = 50;

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

    //ctx.scale(1, -1);
    ctx.setTransform(1, 0, 0, -1, 0, 0);
    ctx.translate(0, -cnvHeight);

    ctx.clearRect(0, 0, cnvWidth, cnvHeight);

    ctx.strokeStyle = "black";
    ctx.strokeWidth = 1;

    ctx.font = "20px Segoe UI";


    for (var currency in data) {
        var chartPointsCount = data[currency].length;

        if (chartPointsCount < NORMAL_MODE_MAX_POINTS_COUNT) {

            var pointHStep = (cnvWidth - MARGIN_LEFT - MARGIN_RIGHT) / chartPointsCount;
            var pointVStep = (cnvHeight - MARGIN_TOP - MARGIN_BOTTOM) / diffValue;

            if (pointHStep < TIGHT_POINTS_THRESHOLD) {
                // If points are too tight.
                pointHStep = TIGHT_POINTS_THRESHOLD;
            }

            var x = 0;
            var y = 0;

            ctx.beginPath();

            for (var j = chartPointsCount - 1; j >= 0; j--) {
                var chartPoint = data[currency][j];

                x = pointHStep / 2 + pointHStep * j + MARGIN_LEFT + horTranslation;
                y = pointVStep * (chartPoint.Value - minValue) + MARGIN_BOTTOM;

                if (x > (horTranslation) && x < cnvWidth + horTranslation) {

                    if (j !== chartPointsCount - 1) {
                        ctx.lineTo(x, y);
                    } else {
                        ctx.moveTo(x, y);
                    }

                    if (j == 0) {
                        if (currencyLblsStatus.indexOf(currency) == -1) {
                            // First point.
                            var lblFor = "lbl-for-" + currency.toLowerCase();
                            chart.before('<div class="chart-currency-label" data-descr="' + chartPoint.Name + '" id="' + lblFor + '">' + currency + '</div>');

                            $("#" + lblFor).css({
                                left: '100px',
                                bottom: (y - 6) + 'px'
                            });

                        }

                        currencyLblsStatus[currencyLblsStatus.length] = currency;
                    }
                }

                //ctx.fillArc(x, y, 5, 0, 2 * Math.PI);
            }

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

function scrollChartLeft(amount) {
    chartScrollingValue -= amount;
    drawChart(null, CANVAS_ID, false, chartScrollingValue);
}

function scrollChartRight(amount) {
    chartScrollingValue += amount;
    drawChart(null, CANVAS_ID, false, chartScrollingValue);
}


