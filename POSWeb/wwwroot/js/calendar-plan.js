$(document).ready(function () {
    const formatter = new Intl.DateTimeFormat('ru', { month: 'long', year: 'numeric' });
    let calendarPlanVM;
    let estimateFiles;

    $('#estimate-files').change(function () {
        var spinner = $('.choose-estimates .spinner-border');
        spinner.addClass('d-inline-block');
        let formData = new FormData();
        estimateFiles = $(this).get(0).files;

        AppendEstimateFiles(formData);
        $.ajax(
            {
                url: '/CalendarPlan/GetCalendarPlanVMJson',
                data: formData,
                processData: false,
                contentType: false,
                type: 'POST',
                success: function (viewModel) {
                    calendarPlanVM = viewModel;
                    calendarPlanVM.constructionDuration = Math.ceil(calendarPlanVM.constructionDuration);
                    if (calendarPlanVM.constructionDuration == 1) {
                        DownloadOneMonthCalendarPlanAjax(formData);
                    } else {
                        $('.calendar-plan #ConstructionDuration').val(calendarPlanVM.constructionDuration);
                        $('.calendar-plan #ConstructionStartDate').val(calendarPlanVM.constructionStartDate);
                        $('.calendar-plan #ObjectCipher').val(calendarPlanVM.objectCipher);

                        $('.percentages-table thead tr:last-child').empty();
                        AppendDateRow(calendarPlanVM.constructionStartDate, calendarPlanVM.constructionDuration);

                        $('.percentages-table tbody').empty();
                        AppendRows(calendarPlanVM);

                        AppendAcceptanceTimeCell(calendarPlanVM.userWorks.length);

                        spinner.removeClass('d-inline-block');
                        $('.percentages-table').show();
                    }
                }
            }
        );
    });

    function DownloadOneMonthCalendarPlanAjax(formData) {
        AppendDateAndDuration(formData);
        AppendPercentagesForOneMonthCalendarPlan(formData);

        $.ajax({
            url: '/CalendarPlan/Index',
            data: formData,
            processData: false,
            contentType: false,
            type: 'POST',
            success: function () {
                window.location = `/CalendarPlan/Download?objectCipher=${calendarPlanVM.objectCipher}`;
            }
        });
        $('.choose-estimates .spinner-border').removeClass('d-inline-block');
    }

    function AppendDateRow(constructionStartDate, constructionDuration) {
        let milliseconds = Date.parse(constructionStartDate);
        let startDate = new Date(milliseconds);
        let monthRow = $('.percentages-table thead #month-row')
        let currentDate = startDate;
        for (let i = 0; i < constructionDuration + 1; i++) {
            var monthYearStr = formatter.format(currentDate);
            let monthCell =
                `<th class="align-middle text-center">
                    ${monthYearStr[0].charAt(0).toUpperCase() + monthYearStr.slice(1)}
                </th>`;
            monthRow.append(monthCell);

            if (startDate.getMonth() == 11) {
                currentDate = new Date(currentDate.getFullYear() + 1, 0, 1);
            } else {
                currentDate = new Date(currentDate.getFullYear(), currentDate.getMonth() + 1, 1);
            }
        }
    }

    function AppendRows(calendarPlanVM) {
        for (let i = 0; i < calendarPlanVM.userWorks.length; i++) {
            let inputRow = '';
            for (let j = 0; j < calendarPlanVM.constructionDuration; j++) {
                inputRow += `
                    <td>
                        <div class="input-group mb-3 mt-3 min-w-rem-4d8">
                            <input name="UserWorks[${i}].Percentages[${j}]" id="percent-input" value="0" type="number" min="0" max="100" step="1" id="percent-part" class="form-control" />
                            <div class="input-group-append">
                                <span class="input-group-text">%</span>
                            </div>
                        </div>
                    </td>`;
            }

            let userRow = `
                <tr>
                    <th scope="row" class="text-break">
                        ${calendarPlanVM.userWorks[i].workName}
                        <input name="UserWorks[${i}].WorkName" type="hidden" value="${calendarPlanVM.userWorks[i].workName}"/>
                    </th>
                    ${inputRow}
                </tr>`;

            $('.percentages-table tbody').append(userRow);
        }
    }

    function AppendAcceptanceTimeCell(rowspan) {
        let acceptanceTimeCell = `
            <td class="acceptance-time" rowspan="${rowspan + 1}">
                Приемка объекта в эксплуатацию
            </td>
        `;

        $('.percentages-table tbody tr:first-child').append(acceptanceTimeCell);
    }

    $('#calculate-percentages').click(function () {
        let calculateBtn = $('.calculate-btn');
        calculateBtn.hide();
        let calculateBtnLoading = $('.calculate-btn-loading');
        calculateBtnLoading.show();
        let formData = new FormData();
        AppendEstimateFiles(formData);
        AppendDateAndDuration(formData);
        AppendPercentagesForSeveralMonthsCalendarPlan(formData);

        $.ajax({
            url: '/CalendarPlan/GetMainTotalWorkJson',
            data: formData,
            processData: false,
            contentType: false,
            type: 'POST',
            success: function (mainTotalWork) {
                let mainTotalWorkRowAlreadyOnPage = $('.percentages-table #main-total-work');
                if (mainTotalWorkRowAlreadyOnPage.length) {
                    mainTotalWorkRowAlreadyOnPage.remove();
                }
                let mainTotalWorkRow = GenerateMainTotalWorkRow(mainTotalWork);

                calculateBtnLoading.hide();
                calculateBtn.show();

                $('.percentages-table tbody').append(mainTotalWorkRow);
            }
        });
    });

    function GenerateMainTotalWorkRow(mainTotalWork) {
        let mainTotalPercentagesCells = '';
        for (var i = 0; i < mainTotalWork.percentages.length; i++) {
            mainTotalPercentagesCells += `
                        <td>
                            ${(mainTotalWork.percentages[i] * 100).toFixed(2)} %
                        </td>`;
        }

        let mainTotalWorkRow = `
                    <tr id="main-total-work">
                        <th scope="row">
                            ${mainTotalWork.workName}
                        </th>
                        ${mainTotalPercentagesCells}
                    </tr>`;
        return mainTotalWorkRow;
    }

    function AppendEstimateFiles(formData) {
        for (let i = 0; i != estimateFiles.length; i++) {
            formData.append('estimateFiles', estimateFiles[i]);
        }
    }

    function AppendDateAndDuration(formData) {
        formData.append('ConstructionStartDate', calendarPlanVM.constructionStartDate);
        formData.append('ConstructionDuration', calendarPlanVM.constructionDuration);
    }

    function AppendPercentagesForOneMonthCalendarPlan(formData) {
        for (let i = 0; i < calendarPlanVM.userWorks.length; i++) {
            formData.append(`UserWorks[${i}].WorkName`, calendarPlanVM.userWorks[i].workName);
            formData.append(`UserWorks[${i}].Percentages[0]`, 100);
        }
    }

    function AppendPercentagesForSeveralMonthsCalendarPlan(formData) {
        for (let i = 0; i < calendarPlanVM.userWorks.length; i++) {
            formData.append(`UserWorks[${i}].WorkName`, calendarPlanVM.userWorks[i].workName);
            for (let j = 0; j < calendarPlanVM.constructionDuration; j++) {
                let percent = $(`.percentages-table tbody tr:nth-child(${i + 1}) td:nth-child(${j + 2}) #percent-input`).val();
                formData.append(`UserWorks[${i}].Percentages[${j}]`, percent);
            }
        }
    }

    $(document).on('keyup', '#percent-input', function () {
        let thisPercent = parseInt($(this).val());
        let percentSum = thisPercent;
        $(this).parents('td').siblings('td').each(function () {
            let percent = $(this).find('#percent-input').val();
            if (percent) {
                percentSum += parseInt(percent);
            }
        });

        if (percentSum > 100) {
            let sumOfOtherPercents = percentSum - thisPercent;
            $(this).val(100 - sumOfOtherPercents);
        }
    });

    $(document).on('blur', '#percent-input', function () {
        let value = $(this).val();

        if (!value.trim() || value < 0) {
            $(this).val(0);
        }
    });
});