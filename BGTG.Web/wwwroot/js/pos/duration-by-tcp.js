$(document).ready(function () {
    const appendixACategories =
        [
            'Уличные трубопроводы водо-, газоснабжения и канализации, сооружаемые в траншеях с откосами',
            'Уличные трубопроводы водо-, газоснабжения и канализации, сооружаемые в траншеях с креплением стенок',
            'Уличные тепловые сети, сооружаемые в траншеях с откосами',
            'Уличные тепловые сети, сооружаемые в траншеях с креплением стенок'
        ];

    const appendixBCategories =
        [
            'Наружные трубопроводы',
            'Распределительная газовая сеть'
        ];

    const componentMaterialsAppendixA2And3 = ['сборных железобетонных лотковых элементов'];

    const componentMaterailsAppendixB1 = [
        'стальных труб в две нитки',
        'стальных труб в одну нитку',
        'полиэтиленовых труб в одну нитку'
    ];

    const defaultComponentMaterials =
        [
            'стальных труб',
            'полиэтиленовых труб',
            'чугунных труб',
            'асбестоцементных труб',
            'керамических труб',
            'бестонных труб',
            'железобетонных труб',
            'стеклопластиковых труб'
        ];

    const durationByTCPSpinner = $('#duration-by-tcp-download-btn #spinner');
    const objectCipher = $('#pos #construction-object-cipher');
    const appendixCategory = $('#duration-by-tcp #appendix-category');
    const appendixCategoryDefaultOption = '<option selected disabled>Выберите объект</option>';
    const pipelineMaterial = $('#duration-by-tcp #pipeline-material');
    const pipelineMaterialDefaultOption = '<option selected disabled>Выберите материал</option>';
    const downloadBtn = $('#duration-by-tcp #duration-by-tcp-download-btn');
    const pipelineDiameterAndLengthAndDownloadBtn = $('#duration-by-tcp #pipeline-diameter, #duration-by-tcp #pipeline-length, #duration-by-tcp #duration-by-tcp-download-btn');

    $('#duration-by-tcp #appendix-key').change(function () {
        appendixCategory.empty();
        appendixCategory.append(appendixCategoryDefaultOption);

        pipelineMaterial.empty();
        pipelineMaterial.append(pipelineMaterialDefaultOption);
        pipelineMaterial.prop('disabled', true);

        pipelineDiameterAndLengthAndDownloadBtn.prop('disabled', true);

        let appendixKey = $(this).val();

        let options = '';
        switch (appendixKey) {
            case 'A':
                options = getOptions(appendixACategories);
                break;
            case 'B':
                options = getOptions(appendixBCategories);
        }

        appendixCategory.append(options);
        appendixCategory.prop('disabled', false);
    });

    appendixCategory.change(function () {
        pipelineMaterial.empty();
        pipelineMaterial.append(pipelineMaterialDefaultOption);

        pipelineDiameterAndLengthAndDownloadBtn.prop('disabled', true);
        let value = $(this).val();

        let options = '';
        if (appendixACategories[2] == value || appendixACategories[3] == value) {
            options = getOptions(componentMaterialsAppendixA2And3, 'из');
        } else if (appendixBCategories[1] == value) {
            options = getOptions(componentMaterailsAppendixB1, 'из');
        } else {
            options = getOptions(defaultComponentMaterials, 'из');
        }

        pipelineMaterial.append(options);
        pipelineMaterial.prop('disabled', false);
    });

    pipelineMaterial.change(function () {
        pipelineDiameterAndLengthAndDownloadBtn.prop('disabled', false);
    });

    function getOptions(array, prefix) {
        let options = '';

        array.forEach(x => {
            let optionText = prefix
                ? prefix + ' ' + x
                : x;

            options += `<option value="${x}">${optionText}</opiton>`;
        });

        return options;
    }

    downloadBtn.click(function () {
        durationByTCPSpinner.addClass(dInlineBlockClass);

        let durationByTCP = {
            ObjectCipher: objectCipher.val(),
            AppendixKey: $('#duration-by-tcp #appendix-key').val(),
            PipelineCategoryName: $('#pos #appendix-category').val(),
            PipelineMaterial: $('#duration-by-tcp #pipeline-material').val(),
            PipelineDiameter: $('#duration-by-tcp #pipeline-diameter').val(),
            PipelineLength: $('#duration-by-tcp #pipeline-length').val(),
        };

        $.post('api/duration-by-tcps/write',
            durationByTCP,
            function (operationResult) {
                if (!operationResult.ok) {
                    appendAlerts(operationResult);
                    durationByTCPSpinner.removeClass(dInlineBlockClass);
                    return;
                }

                $('#duration-by-tcp #duration-by-tcp-download-submit-btn').click();
                durationByTCPSpinner.removeClass(dInlineBlockClass);
            });
    });
});