$(document).ready(function () {
    loadChartData(0, 0, 0);
});


function loadChartData(begin, end, currencies) {

    begin = new Date(2016, 2, 29);
    end = new Date();
    currencies = new Array("USD", "EUR");

    var dataObj = {
        Begin: begin,
        End: end,
        Currencies: currencies
    };

    $.ajax({
        type: "POST",
        dataType: "json",
        url: "Chart/LoadChartData",
        data: "json=" + JSON.stringify(dataObj),
        success: function (d) {
            //alert(d);
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

    var drawData = new Array();

    $.each(data, function (currency, data) {
        alert();
        drawData[currency] = new Array();
        for (var point in data[currency]) {
            var stopData = new Array();
            stopData.date = data[currency][point][0];
            stopData.val = data[currency][point][1];
            stopData.alias = data[currency][point][2];
            stopData.name = data[currency][point][3];

            drawData[currency][drawData.length] = stopData;
        }
    });

    alert(drawData['USD']);

    var NORMAL_MODE_MAX_POINTS_COUNT = 100;
    var MARGIN_LEFT = 10;
    var MARGIN_RIGHT = 10;
    var MARGIN_BOTTOM = 10;
    var MARGIN_TOP = 10;

    var canvas = document.getElementById(cnv);
    var ctx = canvas.getContext("2d");

    var cnvHeight = canvas.clientHeight;
    var cnvWidth = canvas.clientWidth;

    var maxValue = findChartMax(drawData);
    var minValue = findChartMin(drawData);

    ctx.scale(1, -1);
    ctx.translate(0, -cnvHeight);

    ctx.strokeStyle = "black";
    ctx.strokeWidth = 1;

    ctx.beginPath();

    for (var currency in drawData) {
        var chartPointsCount = 0;


        alert(JSON.stringify(drawData));
        //alert(dObj[currency]);

        for (var item in drawData[currency]) {
            
        }

        //alert(chartPointsCount);

        if (chartPointsCount < NORMAL_MODE_MAX_POINTS_COUNT) {

            var pointHStep = (cnvWidth - MARGIN_LEFT - MARGIN_RIGHT) / chartPointsCount;
            var pointVStep = (cnvHeight - MARGIN_TOP - MARGIN_BOTTOM) / maxValue;

            

            for (var i = chartPointsCount - 1; i >= 0; i--) {
                var chartPoint = drawData[currency][i];

                ctx.moveTo(10, 10);
                ctx.lineTo(pointHStep * i, pointVStep * chartPoint.val);

                
            }
        }
    }

    ctx.stroke();
    ctx.endPath();
}

function findChartMax(drawData) {
    var max = -1;
    for (var currency in drawData) {
        for (var point in drawData[currency]) {
            if (drawData[currency][point].val > max)
                max = drawData[currency][point].val;
        }
    }
    return max;
}

function findChartMin(drawData) {
    var min = 1000000;
    for (var currency in drawData) {
        for (var point in drawData[currency]) {
            if (drawData[currency][point].val < min)
                min = drawData[currency][point].val;
        }
    }
    return min;
}