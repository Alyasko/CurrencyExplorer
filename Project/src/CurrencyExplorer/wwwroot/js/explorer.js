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

    var drawData = new Object();

    $.each(data, function (currency, dataObj) {
        
        drawData[currency] = new Array();

        $.each(dataObj, function(point, currObj) {
            var stopData = new Object();
            stopData.date = currObj[0];
            stopData.val = currObj[1];
            stopData.alias = currObj[2];
            stopData.name = currObj[3];

            drawData[currency][drawData.length] = stopData;
        });
    });

    // HERE
    var max = -1;
    for (var i in drawData) {
        alert(drawData);
        $.each(drawData[i], function(j, v) {
            alert(j);
        });

        //for (var j = 0; j < drawData[i].length; j++) {
        //    //alert(drawData[i][j].val);
        //    if (drawData[i][j].val > max)
        //        max = drawData[i][j].val;
        //}
    }
    //

    var NORMAL_MODE_MAX_POINTS_COUNT = 100;
    var MARGIN_LEFT = 10;
    var MARGIN_RIGHT = 10;
    var MARGIN_BOTTOM = 10;
    var MARGIN_TOP = 10;

    var canvas = document.getElementById(cnv);
    var ctx = canvas.getContext("2d");

    var cnvHeight = canvas.clientHeight;
    var cnvWidth = canvas.clientWidth;

    //var maxValue = findChartMax(drawData);
    //var minValue = findChartMin(drawData);

    ctx.scale(1, -1);
    ctx.translate(0, -cnvHeight);

    ctx.strokeStyle = "black";
    ctx.strokeWidth = 1;

    ctx.beginPath();

    for (var currency in drawData) {
        var chartPointsCount = 0;

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
    ctx.closePath();
}

function findChartMax(drawData) {
    var max = -1;
    
    Object.keys(drawData).forEach(key => {
        //alert(drawData[key]);
    });


    $.each(drawData, function (ind1, val1) {
        //alert(val1);

        //????

        $.each(val1, function (ind2, val2) {
            //alert(val2);
            $.each(val2, function(ind3, val3) {
                //alert(val3);
            });
        });
    });

    for (var currency in drawData) {
        //alert(drawData[currency]);
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