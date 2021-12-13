$(document).ready(function () {
    const spinner = $('.choose-estimates .spinner-border');

    $('#energy-and-water-btn').click(function () {
        spinner.addClass('d-inline-block');
        let formData = new FormData();
        AppendEstimateFiles(formData);
        $.ajax({
            url: '/EnergyAndWater/WriteEnergyAndWater',
            data: formData,
            processData: false,
            contentType: false,
            type: 'POST',
            success: function () {
                spinner.removeClass('d-inline-block');
                window.location = '/EnergyAndWater/Download';
            }
        });
    });
});