const dInlineBlockClass = 'd-inline-block';

function appendAlerts(operation) {
    for (var i = 0; i < operation.logs.length; i++) {
        let alert = $('#alert-error-template').clone();
        alert.find('#alert-error-template-text').text(operation.logs[i]);
        $('#alerts').append(alert);
    }
}