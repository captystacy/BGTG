$(document).ready(function () {
    let energyAndWaterVM;
    let spinner = $('.choose-estimates .spinner-border');

    $('#energy-and-water-btn').click(function () {
        spinner.addClass('d-inline-block');

        let formData = new FormData();
        estimateFiles = $('#estimate-files').get(0).files;

        AppendEstimateFiles(formData);
        $.ajax({
            url: '/EnergyAndWater/GetEnergyAndWaterVM',
            data: formData,
            processData: false,
            contentType: false,
            type: 'POST',
            success: function (viewModel) {
                energyAndWaterVM = viewModel;

                AppendDateAndObjectCipher(formData);

                DownloadEnergyAndWaterAjax(formData);

                spinner.removeClass('d-inline-block');
            }
        });
    });

    function AppendDateAndObjectCipher(formData) {
        formData.append('ConstructionStartDate', energyAndWaterVM.constructionStartDate);
        formData.append('ObjectCipher', energyAndWaterVM.objectCipher);
    }

    function DownloadEnergyAndWaterAjax(formData) {
        $.ajax({
            url: '/EnergyAndWater/WriteAndGetObjectCipher',
            data: formData,
            processData: false,
            contentType: false,
            type: 'POST',
            success: function (objectCipher) {
                window.location = `/EnergyAndWater/Download?objectCipher=${objectCipher}`;
            }
        });
    }

    function AppendEstimateFiles(formData) {
        for (let i = 0; i != estimateFiles.length; i++) {
            formData.append('estimateFiles', estimateFiles[i]);
        }
    }
});