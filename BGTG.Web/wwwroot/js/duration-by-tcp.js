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
            'Наружные трубопроводы'
        ];

    const componentMaterialsAppendixA2And3 = ['сборных железобетонных лотковых элементов'];

    const defaultComponentMaterials =
        [
            'стальных',
            'полиэтиленовых',
            'чугунных',
            'асбестоцементных',
            'керамических',
            'бестонных',
            'железобетонных',
            'стеклопластиковых'
        ];

    const appendixCategory = $('#duration-by-tcp #appendix-category');
    const appendixCategoryDefaultOption = '<option selected disabled>Выберите объект</option>';
    const componentMaterial = $('#duration-by-tcp #component-material');
    const componentMaterialDefaultOption = '<option selected disabled>Выберите материал</option>';
    const pipelineDiameterAndLengthAndDownloadBtn = $('#duration-by-tcp #pipeline-diameter, #duration-by-tcp #pipeline-length, #duration-by-tcp #download-duration-by-tcp-btn');

    $('#duration-by-tcp #appendix').change(function () {
        appendixCategory.empty();
        appendixCategory.append(appendixCategoryDefaultOption);

        componentMaterial.empty();
        componentMaterial.append(componentMaterialDefaultOption);
        componentMaterial.prop('disabled', true);

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
        componentMaterial.empty();
        componentMaterial.append(componentMaterialDefaultOption);

        pipelineDiameterAndLengthAndDownloadBtn.prop('disabled', true);
        let value = $(this).val();

        let options = '';
        if (appendixACategories[2] == value || appendixACategories[3] == value) {
            options = getOptions(componentMaterialsAppendixA2And3, 'из');
        } else {
            options = getOptions(defaultComponentMaterials, 'из', 'труб');
        }

        componentMaterial.append(options);
        componentMaterial.prop('disabled', false);
    });

    componentMaterial.change(function() {
        pipelineDiameterAndLengthAndDownloadBtn.prop('disabled', false);
    });

    function getOptions(array, prefix, postfix) {
        let options = '';

        array.forEach(x => {
            let optionText = prefix
                ? prefix + ' ' + x
                : x;

            optionText = postfix
                ? optionText + ' ' + postfix
                : optionText;

            options += `<option value="${x}">${optionText}</opiton>`;
        });

        return options;
    }
});