$(document).ready(function () {
    loadChartData(0, 0, 0);
});


function loadChartData(begin, end, currencies) {
    // PROBLEM
    /*
    begin = new Date(2016, 4, 1);
    end = new Date();
    */

    begin = new Date(2016, 3, 8);
    end = new Date(2016, 3, 22);
    currencies = new Array("USD", "EUR");
    
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
            drawChart(d, "canvas-explorer-chart");
        },
        failure: function (err) {
            alert(err);
        }
    });
}

function getDrawingObject(jData) {
    return drawData;
}

function drawChart(jData, cnv) {

    var data = $.parseJSON(jData);

    var NORMAL_MODE_MAX_POINTS_COUNT = 100;
    var MARGIN_LEFT = 50;
    var MARGIN_RIGHT = 50;
    var MARGIN_BOTTOM = 50;
    var MARGIN_TOP = 50;

    var canvas = document.getElementById(cnv);
    var ctx = canvas.getContext("2d");

    var cnvHeight = canvas.clientHeight;
    var cnvWidth = canvas.clientWidth;

    var maxValue = findChartMax(data);
    var minValue = findChartMin(data);

    var diffValue = maxValue - minValue;

    ctx.scale(1, -1);
    ctx.translate(0, -cnvHeight);

    ctx.strokeStyle = "black";
    ctx.strokeWidth = 1;

    ctx.beginPath();

    for (var currency in data) {
        var chartPointsCount = data[currency].length;

        for (var point in data[currency]) {

            if (chartPointsCount < NORMAL_MODE_MAX_POINTS_COUNT) {

                var pointHStep = (cnvWidth - MARGIN_LEFT - MARGIN_RIGHT) / chartPointsCount;
                var pointVStep = (cnvHeight - MARGIN_TOP - MARGIN_BOTTOM) / diffValue;

                var x = 0;
                var y = 0;

                for (var j = chartPointsCount - 1; j >= 0; j--) {
                    var chartPoint = data[currency][j];

                    x = pointHStep / 2 + pointHStep * j + MARGIN_LEFT;
                    y = pointVStep * (chartPoint.Value - minValue) + MARGIN_BOTTOM;

                    if (j !== chartPointsCount - 1) {
                        ctx.lineTo(x, y);
                    } else {
                        ctx.moveTo(x, y);
                    }

                    //ctx.fillArc(x, y, 5, 0, 2 * Math.PI);
                }
            }
        }
    }

    ctx.stroke();
    ctx.closePath();


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