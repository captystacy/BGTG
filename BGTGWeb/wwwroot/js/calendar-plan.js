$(document).ready(function () {
    const formatter = new Intl.DateTimeFormat('ru', { month: 'long', year: 'numeric' });
    const dInlineBlockClass = 'd-inline-block';
    const dFlexClass = 'd-flex';
    const columnPercentCell = `
        <td>
            <div class="input-group mb-1 mt-1" style="min-width: 6rem;">
                <input class="form-control" id="column-percent-input" value="0" type="number" min="0" max="100" step="1" />
                <div class="input-group-append">
                    <span class="input-group-text">%</span>
                </div>
            </div>
        </td>`;

    const spinner = $('#estimate-calculations #choose-estimates #spinner');
    const estimateFiles = $('#estimate-calculations #estimate-files');

    const percentagesTable = $('#estimate-calculations #percentages-table');
    const calendarPlanBtns = $('#estimate-calculations #calendar-plan-btns');
    const percentagesTableBody = $('#estimate-calculations #percentages-table tbody');
    const monthRow = $('#estimate-calculations #month-row');
    const columnPercentsRow = $('#estimate-calculations #column-percents-row');

    const calendarPlanFailures = $('#estimate-calculations #calendar-plan-failures');
    const constructionStartDateFailure = $('#estimate-calculations #construction-start-date-failure');
    const constructionDurationFailure = $('#estimate-calculations #construction-duration-failure');

    const calendarPlanTotalWorkChapters = $('#estimate-calculations #calendar-plan-total-work-chapters');
    const posBtns =
        $('#estimate-calculations #duration-by-labor-costs-btn, #estimate-calculations #calendar-plan-btn, #estimate-calculations #energy-and-water-btn');

    let calendarPlanVM;

    estimateFiles.change(function () {
        if ($(this).val()) {
            posBtns.prop('disabled', false);
        } else {
            posBtns.prop('disabled', true);
        }

        percentagesTable.hide();
        calendarPlanBtns.removeClass(dFlexClass);
        calendarPlanFailures.removeClass(dFlexClass);
    });

    $('#estimate-calculations #calendar-plan-btn').click(function () {
        spinner.addClass(dInlineBlockClass);
        let formData = new FormData();
        appendEstimateFiles(formData);
        appendTotalWorkChapter(formData);
        $.ajax(
            {
                url: '/Pos/GetCalendarPlanVM',
                data: formData,
                processData: false,
                contentType: false,
                type: 'POST',
                success: function (viewModel) {
                    calendarPlanVM = viewModel;
                    if (calendarPlanFailures.hasClass(dFlexClass)) {
                        let constructionStartDate = constructionStartDateFailure.find('input').val();
                        if (constructionStartDateFailure.hasClass(dFlexClass)) {
                            if (constructionStartDate) {
                                calendarPlanVM.constructionStartDate = constructionStartDate;
                            } else {
                                spinner.removeClass(dInlineBlockClass);
                                return;
                            }
                        }

                        let constructionDuration = constructionDurationFailure.find('input').val();
                        if (constructionDurationFailure.hasClass(dFlexClass)) {
                            if (constructionDuration && constructionDuration >= 1 && constructionDuration <= 21) {
                                calendarPlanVM.constructionDurationCeiling = Math.ceil(constructionDuration);
                            } else {
                                spinner.removeClass(dInlineBlockClass);
                                return;
                            }
                        }
                    } else {
                        let constructionStartDateIsCorrupted = new Date(Date.parse(calendarPlanVM.constructionStartDate)).getFullYear() <= 1900;
                        let constructionDurationIsCorrupted = calendarPlanVM.constructionDurationCeiling == 0;
                        if (constructionDurationIsCorrupted || constructionStartDateIsCorrupted) {
                            calendarPlanFailures.addClass(dFlexClass);

                            if (constructionStartDateIsCorrupted) {
                                constructionStartDateFailure.addClass(dFlexClass);
                            }

                            if (constructionDurationIsCorrupted) {
                                constructionDurationFailure.addClass(dFlexClass);
                            }

                            spinner.removeClass(dInlineBlockClass);
                            return;
                        }
                    }

                    if (calendarPlanVM.constructionDurationCeiling == 1) {
                        downloadOneMonthCalendarPlanAjax(formData);
                    } else {
                        monthRow.empty();
                        columnPercentsRow.empty();
                        appendDateRow(calendarPlanVM.constructionStartDate, calendarPlanVM.constructionDurationCeiling);

                        percentagesTableBody.empty();
                        appendRows(calendarPlanVM);

                        appendAcceptanceTimeCell(calendarPlanVM.userWorks.length);

                        spinner.removeClass(dInlineBlockClass);
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
            formData.append('estimateFiles', files[i]);
        }
    }

    function appendConstructionStartDate(formData) {
        formData.append('ConstructionStartDate', calendarPlanVM.constructionStartDate);
    }

    function appendConstructionDurationCeiling(formData) {
        formData.append('ConstructionDurationCeiling', calendarPlanVM.constructionDurationCeiling);
    }

    function appendTotalWorkChapter(formData) {
        formData.append('TotalWorkChapter', calendarPlanTotalWorkChapters.find('input[type=radio]:checked').val());
    }

    function downloadOneMonthCalendarPlanAjax(formData) {
        appendConstructionStartDate(formData);
        appendConstructionDurationCeiling(formData);
        appendUserWorksForOneMonth(formData);
        appendTotalWorkChapter(formData);

        ajaxWriteAndDownloadCalendarPlan(formData);

        spinner.removeClass(dInlineBlockClass);
    }

    function appendUserWorksForOneMonth(formData) {
        for (let i = 0; i < calendarPlanVM.userWorks.length; i++) {
            formData.append(`UserWorks[${i}].WorkName`, calendarPlanVM.userWorks[i].workName);
            formData.append(`UserWorks[${i}].Percentages[0]`, 1);
        }
    }

    function appendDateRow(constructionStartDate, constructionDurationCeiling) {
        let milliseconds = Date.parse(constructionStartDate);
        let currentDate = new Date(milliseconds);
        for (let i = 0; i < constructionDurationCeiling + 1; i++) {
            var monthYearStr = formatter.format(currentDate);
            let monthCell = `<th class="align-middle text-center">
                                ${monthYearStr[0].charAt(0).toUpperCase() + monthYearStr.slice(1)}
                            </th>`;
            monthRow.append(monthCell);

            currentDate.setMonth(currentDate.getMonth() + 1);

            if (i < constructionDurationCeiling) {
                columnPercentsRow.append(columnPercentCell);
            }

            if (i == constructionDurationCeiling) {
                columnPercentsRow.append('<th class="align-middle text-center">Проценты для колонок</th>');
            }
        }
    }

    function appendRows(calendarPlanVM) {
        for (let i = 0; i < calendarPlanVM.userWorks.length; i++) {
            let inputRow = '';
            for (let j = 0; j < calendarPlanVM.constructionDurationCeiling; j++) {
                inputRow += `
                        <td>
                            <div class="input-group mb-1 mt-1" style="min-width: 6rem;">
                                <input name="UserWorks[${i}].Percentages[${j}]" class="form-control" id="percent-input" value="0" type="number" min="0" max="100" step="1" />
                                <div class="input-group-append">
                                    <span class="input-group-text">%</span>
                                </div>
                            </div>
                        </td>`;
            }

            let userRow = `
                    <tr>
                        <th scope="row">
                            ${calendarPlanVM.userWorks[i].workName}
                            <input name="UserWorks[${i}].WorkName" type="hidden" value="${calendarPlanVM.userWorks[i].workName}"/>
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

    $('#estimate-calculations #calculate-percentages').click(function () {
        spinner.addClass(dInlineBlockClass);
        let formData = new FormData();
        appendEstimateFiles(formData);
        appendUserWorksForSeveralMonths(formData);
        appendConstructionStartDate(formData);
        appendConstructionDurationCeiling(formData);
        appendTotalWorkChapter(formData);
        $.ajax({
            url: '/Pos/GetTotalPercentages',
            data: formData,
            processData: false,
            contentType: false,
            type: 'POST',
            success: function (totalPercentages) {
                let mainTotalWorkRowAlreadyOnPage = $('#percentages-table #main-total-work');
                if (mainTotalWorkRowAlreadyOnPage.length) {
                    mainTotalWorkRowAlreadyOnPage.remove();
                }
                let mainTotalWorkRow = generateMainTotalWorkRow(totalPercentages);


                spinner.removeClass(dInlineBlockClass);
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

    $('#estimate-calculations #download-calendar-plan-btn').click(function () {
        let formData = new FormData();

        appendEstimateFiles(formData);
        appendUserWorksForSeveralMonths(formData);
        appendConstructionStartDate(formData);
        appendConstructionDurationCeiling(formData);
        appendTotalWorkChapter(formData);

        ajaxWriteAndDownloadCalendarPlan(formData);
    });

    function ajaxWriteAndDownloadCalendarPlan(formData) {
        $.ajax({
            url: '/Pos/WriteCalendarPlan',
            data: formData,
            processData: false,
            contentType: false,
            type: 'POST',
            success: function () {
                window.location = `/Pos/DownloadCalendarPlan?objectCipher=${calendarPlanVM.objectCipher}`;
            }
        });
    }

    function appendUserWorksForSeveralMonths(formData) {
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

        let a = 0;
    });

    $(document).on('blur', '#percent-input, #column-percent-input', function () {
        let value = $(this).val();

        if (!value.trim() || value < 0) {
            $(this).val(0);
        }

        $(this).keyup();
    });
});
