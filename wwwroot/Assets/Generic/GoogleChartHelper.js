// google.charts.load('current', { packages: ['corechart', 'line'] });
// google.load("visualization", "1", { packages: ["corechart", "line"] });
// google.setOnLoadCallBack(async () => { data = new google.visualization.DataTable(); });

let init = async () => {
    await google.charts.load('current', { packages: ['corechart', 'line'] });
    data = new google.visualization.DataTable();
};
init();

var data = undefined;
var chart = undefined;
var options = {};

/**
 * Create it
 * @param {string} elementId
 * @returns {void}
 */
function CreateChart(elementId) {

    if (elementId == null || elementId == undefined) { return; }
    console.log(elementId);
    console.log(document.getElementById(elementId));
    chart = new google.visualization.LineChart(document.getElementById(elementId));
}

/**
 * Initalize chart
 * @param {string} xType
 * @param {string} xTitle
 * @param {string} yType
 * @param {string} yTitle
 * @returns {void}
 */
function InitChart(xType, xTitle, yType, yTitle) {
    data.addColumn(xType, xTitle);
    data.addColumn(yType, yTitle);
}
/**
 * add row
 * @param {any} first
 * @param {any} second
 * @returns {void}
 */
function AddRows(first, second) {
    data.addRow([first, second]);
}
/**
 * Draw chart using option, pass in undefined to use last setting
 * @param {any} option
 * @returns {void}
 */
function DrawChart(option) {
    options = (option != undefined && option != null) ? option : options;
    chart.draw(data, options);
}
/**
 * Reset
 * @returns {void}
 */
function Reset() {
    options = {};
    chart = undefined;
    data = new google.visualization.DataTable();
}
window.onresize = () => {
    if (chart == undefined) return;
    DrawChart(options);
}