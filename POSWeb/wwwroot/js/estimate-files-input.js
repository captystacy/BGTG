function AppendEstimateFiles(formData) {
    let estimateFiles = $('#estimate-files').get(0).files;
    for (let i = 0; i != estimateFiles.length; i++) {
        formData.append('estimateFiles', estimateFiles[i]);
    }
}

const dot = '.';
const comma = ',';
function ReplaceDotWithComma(value) {
    return value.replace(dot, comma);
}

$(document).ready(function () {
    $('#estimate-files').change(function () {
        $('.pos-buttons').show();
        $('.percentages-table').hide();
    });
});