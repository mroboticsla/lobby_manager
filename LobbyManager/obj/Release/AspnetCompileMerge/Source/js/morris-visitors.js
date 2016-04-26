$(function () {
    Morris.Donut({
        element: 'morris-donut-chart',
        data: [{
            label: "Con DUI",
            value: 92
        }, {
            label: "Otro documento",
            value: 26
        }, {
            label: "Sin documento",
            value: 9
        }],
        resize: true
    });
});