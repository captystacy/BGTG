$(document).ready(function () {
    const dFlexClass = 'd-flex';

    const calendarPlanSpinner = $('#estimate-calculations #calendar-plan-btn #spinner');
    const calculatePercentagesSpinner = $('#estimate-calculations #calculate-percentages-btn #spinner');
    const calendarPlanDownloadSpinner = $('#estimate-calculations #calendar-plan-download-btn #spinner');
    const durationByLCSpinner = $('#estimate-calculations #duration-by-lc-btn #spinner');
    const energyAndWaterSpinner = $('#estimate-calculations #energy-and-water-btn #spinner');
    const estimateFiles = $('#estimate-calculations #estimate-files');

    const objectCipher = $('#pos #construction-object-cipher');
    const percentagesTable = $('#estimate-calculations #percentages-table');
    const calendarPlanBtns = $('#estimate-calculations #calendar-plan-btns');
    const percentagesTableBody = $('#estimate-calculations #percentages-table tbody');
    const monthRow = $('#estimate-calculations #month-row');
    const columnPercentsRow = $('#estimate-calculations #column-percents-row');

    const estimateFailures = $('#estimate-calculations #estimate-failures');
    const constructionStartDateFailure = $('#estimate-calculations #construction-start-date-failure');
    const constructionDurationFailure = $('#estimate-calculations #construction-duration-failure');

    const posBtns =
        $('#estimate-calculations #duration-by-lc-btn, #estimate-calculations #calendar-plan-btn, #estimate-calculations #energy-and-water-btn');

    let calendarPlanCreateViewModel;

    if (estimateFiles.val()) {
        posBtns.prop('disabled', false);
    }

    estimateFiles.change(function () {
        if ($(this).val()) {
            posBtns.prop('disabled', false);
        } else {
            posBtns.prop('disabled', true);
        }

        percentagesTable.hide();
        calendarPlanBtns.removeClass(dFlexClass);
        estimateFailures.removeClass(dFlexClass);
    });

    $('#estimate-calculations #calendar-plan-btn').click(function () {
        calendarPlanSpinner.addClass(dInlineBlockClass);
        let formData = new FormData();
        appendEstimateFiles(formData);
        appendTotalWorkChapter(formData);
        $.ajax(
            {
                url: 'CalendarPlans/GetCalendarPlanCreateViewModel',
                data: formData,
                processData: false,
                contentType: false,
                type: 'POST',
                success: function (operation) {
                    if (!operation.ok) {
                        appendValidationAlert(operation);
                        calendarPlanSpinner.removeClass(dInlineBlockClass);
                        return;
                    }

                    calendarPlanCreateViewModel = operation.result;
                    if (estimateFailures.hasClass(dFlexClass)) {
                        let constructionStartDate = constructionStartDateFailure.find('input').val();
                        if (constructionStartDateFailure.hasClass(dFlexClass)) {
                            if (constructionStartDate) {
                                calendarPlanCreateViewModel.constructionStartDate = constructionStartDate;
                            } else {
                                calendarPlanSpinner.removeClass(dInlineBlockClass);
                                return;
                            }
                        }

                        let constructionDuration = constructionDurationFailure.find('input').val();
                        if (constructionDurationFailure.hasClass(dFlexClass)) {
                            if (constructionDuration && constructionDuration >= 1 && constructionDuration <= 21) {
                                calendarPlanCreateViewModel.constructionDuration = constructionDuration;
                                calendarPlanCreateViewModel.constructionDurationCeiling = Math.ceil(constructionDuration);
                            } else {
                                calendarPlanSpinner.removeClass(dInlineBlockClass);
                                return;
                            }
                        }
                    } else {
                        let constructionStartDateIsCorrupted = new Date(Date.parse(calendarPlanCreateViewModel.constructionStartDate)).getFullYear() <= 1900;
                        let constructionDurationIsCorrupted = calendarPlanCreateViewModel.constructionDurationCeiling == 0;
                        if (constructionDurationIsCorrupted || constructionStartDateIsCorrupted) {
                            estimateFailures.addClass(dFlexClass);

                            if (constructionStartDateIsCorrupted) {
                                constructionStartDateFailure.addClass(dFlexClass);
                            }

                            if (constructionDurationIsCorrupted) {
                                constructionDurationFailure.addClass(dFlexClass);
                            }

                            calendarPlanSpinner.removeClass(dInlineBlockClass);
                            return;
                        }
                    }

                    if (calendarPlanCreateViewModel.constructionDurationCeiling == 1) {
                        downloadOneMonthCalendarPlanAjax(formData);
                    } else {
                        monthRow.empty();
                        columnPercentsRow.empty();
                        appendDateRow(calendarPlanCreateViewModel.constructionStartDate, calendarPlanCreateViewModel.constructionDurationCeiling);

                        percentagesTableBody.empty();
                        appendRows(calendarPlanCreateViewModel);

                        appendAcceptanceTimeCell(calendarPlanCreateViewModel.calendarWorkViewModels.length);

                        calendarPlanSpinner.removeClass(dInlineBlockClass);
                        percentagesTable.show();
                        calendarPlanBtns.addClass(dFlexClass);
                    }
                }
            }
        );
    });

    function appendEstimateFiles(formData) {
        let files = estimateFiles.get(0).files;
        for (let i = 0; i < files.length; i++) {
            formData.append('EstimateFiles', files[i]);
        }
    }

    function appendConstructionStartDateAndDurationAndDurationCeiling(formData) {
        formData.append('ConstructionStartDate', calendarPlanCreateViewModel.constructionStartDate);
        formData.append('ConstructionDurationCeiling', calendarPlanCreateViewModel.constructionDurationCeiling);
        formData.append('ConstructionDuration', calendarPlanCreateViewModel.constructionDuration);
    }

    function appendTotalWorkChapter(formData) {
        formData.append('TotalWorkChapter', $('#estimate-calculations #calendar-plan-total-work-chapters').find('input[type=radio]:checked').val());
    }

    function appendObjectCipher(formData) {
        formData.append('ObjectCipher', objectCipher.val());
    }

    function downloadOneMonthCalendarPlanAjax(formData) {
        appendConstructionStartDateAndDurationAndDurationCeiling(formData);
        appendCalendarWorkViewModelsForOneMonth(formData);
        appendTotalWorkChapter(formData);
        appendObjectCipher(formData);

        ajaxWriteCalendarPlan(formData);

        calendarPlanSpinner.removeClass(dInlineBlockClass);
    }

    function appendCalendarWorkViewModelsForOneMonth(formData) {
        for (let i = 0; i < calendarPlanCreateViewModel.calendarWorkViewModels.length; i++) {
            formData.append(`CalendarWorkViewModels[${i}].WorkName`, calendarPlanCreateViewModel.calendarWorkViewModels[i].workName);
            formData.append(`CalendarWorkViewModels[${i}].Percentages[0]`, 1);
        }
    }

    function appendDateRow(constructionStartDate, constructionDurationCeiling) {
        let milliseconds = Date.parse(constructionStartDate);
        let currentDate = new Date(milliseconds);
        let formatter = new Intl.DateTimeFormat('ru', { month: 'long', year: 'numeric' });
        for (let i = 0; i < constructionDurationCeiling + 1; i++) {
            var monthYearStr = formatter.format(currentDate);
            let monthCell = `<th class="align-middle text-center">
                                ${monthYearStr[0].charAt(0).toUpperCase() + monthYearStr.slice(1)}
                            </th>`;
            monthRow.append(monthCell);

            currentDate.setMonth(currentDate.getMonth() + 1);

            if (i < constructionDurationCeiling) {
                columnPercentsRow.append(`
                    <td>
                        <div class="input-group mb-1 mt-1" style="min-width: 6rem;">
                            <input class="form-control" id="column-percent-input" value="0" type="number" min="0" max="100" step="1" />
                            <div class="input-group-append">
                                <span class="input-group-text">%</span>
                            </div>
                        </div>
                    </td>`);
            }

            if (i == constructionDurationCeiling) {
                columnPercentsRow.append('<th class="align-middle text-center">Проценты для колонок</th>');
            }
        }
    }

    function appendRows(calendarPlanCreateViewModel) {
        for (let i = 0; i < calendarPlanCreateViewModel.calendarWorkViewModels.length; i++) {
            let inputRow = '';
            for (let j = 0; j < calendarPlanCreateViewModel.constructionDurationCeiling; j++) {
                inputRow += `
                        <td>
                            <div class="input-group mb-1 mt-1" style="min-width: 6rem;">
                                <input name="CalendarWorkViewModels[${i}].Percentages[${j}]" class="form-control" id="percent-input" value="0" type="number" min="0" max="100" step="1" />
                                <div class="input-group-append">
                                    <span class="input-group-text">%</span>
                                </div>
                            </div>
                        </td>`;
            }

            let userRow = `
                    <tr>
                        <th scope="row">
                            ${calendarPlanCreateViewModel.calendarWorkViewModels[i].workName}
                            <input name="CalendarWorkViewModels[${i}].WorkName" type="hidden" value="${calendarPlanCreateViewModel.calendarWorkViewModels[i].workName}"/>
                        </th>
                        ${inputRow}
                    </tr>`;

            percentagesTableBody.append(userRow);
        }
    }

    function appendAcceptanceTimeCell(rowspan) {
        let acceptanceTimeCell = `
                <td class="align-middle text-center" rowspan="${rowspan + 1}" style="writing-mode: vertical-rl; transform: rotate( -180deg);">
                    Приемка объекта в эксплуатацию
                </td>
            `;

        percentagesTableBody.find('tr:first-child').append(acceptanceTimeCell);
    }

    $('#estimate-calculations #calculate-percentages-btn').click(function () {
        calculatePercentagesSpinner.addClass(dInlineBlockClass);
        let formData = new FormData();
        appendEstimateFiles(formData);
        appendConstructionStartDateAndDurationAndDurationCeiling(formData);
        appendCalendarWorkViewModelsForSeveralMonths(formData);
        appendTotalWorkChapter(formData);
        appendObjectCipher(formData);
        $.ajax({
            url: 'CalendarPlans/GetTotalPercentages',
            data: formData,
            processData: false,
            contentType: false,
            type: 'POST',
            success: function (operation) {
                if (!operation.ok) {
                    appendValidationAlert(operation);
                    calculatePercentagesSpinner.removeClass(dInlineBlockClass);
                    return;
                }

                let mainTotalWorkRowAlreadyOnPage = $('#percentages-table #main-total-work');
                if (mainTotalWorkRowAlreadyOnPage.length) {
                    mainTotalWorkRowAlreadyOnPage.remove();
                }
                let mainTotalWorkRow = generateMainTotalWorkRow(operation.result);

                calculatePercentagesSpinner.removeClass(dInlineBlockClass);
                percentagesTableBody.append(mainTotalWorkRow);
            }
        });
    });

    function generateMainTotalWorkRow(totalPercentages) {
        let mainTotalPercentagesCells = '';
        for (var i = 0; i < totalPercentages.length; i++) {
            mainTotalPercentagesCells += `
                            <td>
                                ${(totalPercentages[i] * 100).toFixed(2)} %
                            </td>`;
        }

        let mainTotalWorkRow = `
                        <tr id="main-total-work">
                            <th scope="row">
                                Итого:
                            </th>
                            ${mainTotalPercentagesCells}
                        </tr>`;
        return mainTotalWorkRow;
    }

    $('#estimate-calculations #calendar-plan-download-btn').click(function () {
        calendarPlanDownloadSpinner.addClass(dInlineBlockClass);

        let formData = new FormData();
        appendEstimateFiles(formData);
        appendCalendarWorkViewModelsForSeveralMonths(formData);
        appendConstructionStartDateAndDurationAndDurationCeiling(formData);
        appendTotalWorkChapter(formData);
        appendObjectCipher(formData);

        ajaxWriteCalendarPlan(formData);
    });

    function ajaxWriteCalendarPlan(formData) {
        $.ajax({
            url: '/CalendarPlans/Write',
            data: formData,
            processData: false,
            contentType: false,
            type: 'POST',
            success: function (operation) {
                if (!operation.ok) {
                    appendValidationAlert(operation);
                    calendarPlanSpinner.removeClass(dInlineBlockClass);
                    calendarPlanDownloadSpinner.removeClass(dInlineBlockClass);
                    return;
                }

                $('#estimate-calculations #calendar-plan-download-submit-btn').click();
                calendarPlanSpinner.removeClass(dInlineBlockClass);
                calendarPlanDownloadSpinner.removeClass(dInlineBlockClass);
            }
        });
    }

    function appendCalendarWorkViewModelsForSeveralMonths(formData) {
        percentagesTableBody.find('input').each(function () {
            if ($(this).attr('type') == 'number') {
                formData.append($(this).attr('name'), $(this).val() / 100);
            } else {
                formData.append($(this).attr('name'), $(this).val());
            }
        });
    }

    $(document).on('keyup', '#percent-input, #column-percent-input', function () {
        let thisPercent = parseInt($(this).val());
        let percentSum = thisPercent;
        $(this).parents('td').siblings('td').each(function () {
            let percent = $(this).find('input').val();
            if (percent) {
                percentSum += parseInt(percent);
            }
        });

        if (percentSum > 100) {
            let sumOfOtherPercents = percentSum - thisPercent;
            $(this).val(100 - sumOfOtherPercents);
        }
    });

    $(document).on('keyup', '#column-percent-input', function () {
        let tdPosition = $(this).parents('td').index() + 1;

        percentagesTableBody.find(`tr td:nth-child(${tdPosition + 1}) #percent-input`).val($(this).val());
    });

    $(document).on('blur', '#percent-input, #column-percent-input', function () {
        let value = $(this).val();

        if (!value.trim() || value < 0) {
            $(this).val(0);
        }

        $(this).keyup();
    });

    $('#estimate-calculations #duration-by-lc-btn').click(function () {
        durationByLCSpinner.addClass(dInlineBlockClass);

        let formData = new FormData();
        formData.append('NumberOfWorkingDays', $('#estimate-calculations #number-of-working-days').val());
        formData.append('WorkingDayDuration', $('#estimate-calculations #working-day-duration').val());
        formData.append('Shift', $('#estimate-calculations #shift').val());
        formData.append('NumberOfEmployees', $('#estimate-calculations #number-of-employees').val());
        formData.append('TechnologicalLaborCosts', $('#estimate-calculations #technological-labor-costs').val());
        formData.append('AcceptanceTimeIncluded', $('#estimate-calculations #acceptance-time-included').prop('checked'));
        appendEstimateFiles(formData);
        appendObjectCipher(formData);

        $.ajax({
            url: 'DurationByLCs/Write',
            data: formData,
            processData: false,
            contentType: false,
            type: 'POST',
            success: function (operation) {
                if (!operation.ok) {
                    appendValidationAlert(operation);
                    durationByLCSpinner.removeClass(dInlineBlockClass);
                    return;
                }

                $('#estimate-calculations #duration-by-lc-download-submit-btn').click();
                durationByLCSpinner.removeClass(dInlineBlockClass);
            }
        });
    });

    $('#estimate-calculations #energy-and-water-btn').click(function () {
        energyAndWaterSpinner.addClass(dInlineBlockClass);

        let formData = new FormData();
        appendEstimateFiles(formData);
        appendObjectCipher(formData);

        $.ajax({
            url: 'EnergyAndWaters/Write',
            data: formData,
            processData: false,
            contentType: false,
            type: 'POST',
            success: function (operation) {
                if (!operation.ok) {
                    appendValidationAlert(operation);
                    energyAndWaterSpinner.removeClass(dInlineBlockClass);
                    return;
                }

                $('#estimate-calculations #energy-and-water-download-submit-btn').click();
                energyAndWaterSpinner.removeClass(dInlineBlockClass);
            }
        });
    });
});
