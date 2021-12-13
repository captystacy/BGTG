$(document).ready(function () {
    const spinner = $('.choose-estimates .spinner-border');

    $('#labor-costs-duration-btn').click(function () {
        spinner.addClass('d-inline-block');
        let formData = new FormData();
        AppendEstimateFiles(formData);
        AppendLaborCostsDurationVM(formData);
        $.ajax({
            url: '/LaborCostsDuration/WriteLaborCostsDuration',
            data: formData,
            processData: false,
            contentType: false,
            type: 'POST',
            success: function () {
                spinner.removeClass('d-inline-block');
                window.location = '/LaborCostsDuration/Download';
            }
        });
    });

    function AppendLaborCostsDurationVM(formData) {
        formData.append('NumberOfWorkingDays', ReplaceDotWithComma($('#number-of-working-days').val()));
        formData.append('WorkingDayDuration', ReplaceDotWithComma($('#working-day-duration').val()));
        formData.append('Shift', ReplaceDotWithComma($('#shift').val()));
        formData.append('NumberOfEmployees', $('#number-of-employees').val());
        formData.append('AcceptanceTimeIncluded', $('#acceptance-time-included').is(":checked"));
        formData.append('TechnologicalLaborCosts', ReplaceDotWithComma($('#technological-labor-costs').val()));
    }
});