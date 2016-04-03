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

function drawChart(jData, cnv) {
    var data = $.parseJSON(jData);

    var canvas = document.getElementById(cnv);

    var ctx = canvas.getContext("2d");

    var drawData = new Array();

    for (var currency in data) {
        drawData[currency] = new Array();
        for (var point in data[currency]) {
            var stopData = new Array();
            stopData.date = data[currency][point][0];
            stopData.val = data[currency][point][1];
            stopData.alias = data[currency][point][2];
            stopData.name = data[currency][point][3];

            drawData[currency][drawData.length] = stopData;
            //ctx.strokeStyle = "black";
            //ctx.lineWidth = 1;

            //ctx.beginPath();

            //ctx.moveTo(0, 0);
            //ctx.lineTo(10, 10);
            //ctx.lineTo(60, 20);

            //ctx.stroke();

            //ctx.closePath();

        }
    }



    alert(findChartMax(drawData));

}

function findChartMax(drawData) {
    var max = -1;
    for (var currency in drawData) {
        for (var i = 0; i < drawData[currency].length; i++) {
            alert(drawData[currency][i].val);
            if (drawData[currency][i].val > max)
                max = drawData[currency][i].val;
        }
    }
    return max;
}