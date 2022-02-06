const dInlineBlockClass = 'd-inline-block';

function appendAlerts(operationResult) {
    for (var i = 0; i < operationResult.logs.length; i++) {
        let alert = $('#alert-error-template').clone();
        alert.find('#alert-error-template-text').text(operationResult.logs[i]);
        $('#alerts').append(alert);
    }
}