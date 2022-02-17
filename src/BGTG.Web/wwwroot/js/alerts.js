const dInlineBlockClass = 'd-inline-block';

function appendValidationAlert(operation) {
    let alert = $('#alert-error-template').clone();
    alert.find('#alert-error-template-text').text(operation.metadata.message);
    $('#alerts').append(alert);
}