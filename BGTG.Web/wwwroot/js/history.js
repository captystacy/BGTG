$(document).ready(function () {
    $('#history #calendar-plan-download-btn').click(function () {
        let spinner = $(this).find('#spinner');
        spinner.addClass(dInlineBlockClass);

        let historyItem = $(this).parents('.history-item');
        let calendarPlanId = historyItem.find('#calendar-plan-id').val();

        $.post(`api/calendar-plans/write-by-id/${calendarPlanId}`, function (operationResult) {
            if (!operationResult.ok) {
                appendAlerts(operationResult);
                spinner.removeClass(dInlineBlockClass);
                return;
            }

            historyItem.find('#calendar-plan-download-submit-btn').click();
            spinner.removeClass(dInlineBlockClass);
        });
    });

    $('#history #calendar-plan-delete-btn').click(function () {
        let spinner = $(this).find('#spinner');
        spinner.addClass(dInlineBlockClass);

        let historyItem = $(this).parents('.history-item');
        let calendarPlanId = historyItem.find('#calendar-plan-id').val();
        $.ajax({
            url: `api/calendar-plans/delete-item/${calendarPlanId}`,
            type: 'DELETE',
            success: function (operationResult) {
                if (!operationResult.ok) {
                    appendAlerts(operationResult);
                    spinner.removeClass(dInlineBlockClass);
                    return;
                }

                spinner.removeClass(dInlineBlockClass);
                historyItem.remove();
            }
        });
    });

    $('#history #duration-by-lc-download-btn').click(function () {
        let spinner = $(this).find('#spinner');
        spinner.addClass(dInlineBlockClass);

        let historyItem = $(this).parents('.history-item');
        let durationByLCId = historyItem.find('#duration-by-lc-id').val();

        $.post(`api/duration-by-lcs/write-by-id/${durationByLCId}`, function (operationResult) {
            if (!operationResult.ok) {
                appendAlerts(operationResult);
                spinner.removeClass(dInlineBlockClass);
                return;
            }

            historyItem.find('#duration-by-lc-download-submit-btn').click();
            spinner.removeClass(dInlineBlockClass);
        });
    });

    $('#history #duration-by-lc-delete-btn').click(function () {
        let spinner = $(this).find('#spinner');
        spinner.addClass(dInlineBlockClass);

        let historyItem = $(this).parents('.history-item');
        let durationByLCId = historyItem.find('#duration-by-lc-id').val();
        $.ajax({
            url: `api/duration-by-lcs/delete-item/${durationByLCId}`,
            type: 'DELETE',
            success: function (operationResult) {
                if (!operationResult.ok) {
                    appendAlerts(operationResult);
                    spinner.removeClass(dInlineBlockClass);
                    return;
                }

                spinner.removeClass(dInlineBlockClass);
                historyItem.remove();
            }
        });
    });

    $('#history #duration-by-tcp-download-btn').click(function () {
        let spinner = $(this).find('#spinner');
        spinner.addClass(dInlineBlockClass);

        let historyItem = $(this).parents('.history-item');
        let durationByTCPId = historyItem.find('#duration-by-tcp-id').val();

        $.post(`api/duration-by-tcps/write-by-id/${durationByTCPId}`, function (operationResult) {
            if (!operationResult.ok) {
                appendAlerts(operationResult);
                spinner.removeClass(dInlineBlockClass);
                return;
            }

            historyItem.find('#duration-by-tcp-download-submit-btn').click();
            spinner.removeClass(dInlineBlockClass);
        });
    });

    $('#history #duration-by-tcp-delete-btn').click(function () {
        let spinner = $(this).find('#spinner');
        spinner.addClass(dInlineBlockClass);

        let historyItem = $(this).parents('.history-item');
        let durationByTCPId = historyItem.find('#duration-by-tcp-id').val();
        $.ajax({
            url: `api/duration-by-tcps/delete-item/${durationByTCPId}`,
            type: 'DELETE',
            success: function (operationResult) {
                if (!operationResult.ok) {
                    appendAlerts(operationResult);
                    spinner.removeClass(dInlineBlockClass);
                    return;
                }

                spinner.removeClass(dInlineBlockClass);
                historyItem.remove();
            }
        });
    });

    $('#history #energy-and-water-download-btn').click(function () {
        let spinner = $(this).find('#spinner');
        spinner.addClass(dInlineBlockClass);

        let historyItem = $(this).parents('.history-item');
        let energyAndWaterId = historyItem.find('#energy-and-water-id').val();

        $.post(`api/energy-and-waters/write-by-id/${energyAndWaterId}`, function (operationResult) {
            if (!operationResult.ok) {
                appendAlerts(operationResult);
                spinner.removeClass(dInlineBlockClass);
                return;
            }

            historyItem.find('#energy-and-water-download-submit-btn').click();
            spinner.removeClass(dInlineBlockClass);
        });
    });

    $('#history #energy-and-water-delete-btn').click(function () {
        let spinner = $(this).find('#spinner');
        spinner.addClass(dInlineBlockClass);

        let historyItem = $(this).parents('.history-item');
        let energyAndWaterId = historyItem.find('#energy-and-water-id').val();
        $.ajax({
            url: `api/energy-and-waters/delete-item/${energyAndWaterId}`,
            type: 'DELETE',
            success: function (operationResult) {
                if (!operationResult.ok) {
                    appendAlerts(operationResult);
                    spinner.removeClass(dInlineBlockClass);
                    return;
                }

                spinner.removeClass(dInlineBlockClass);
                historyItem.remove();
            }
        });
    });
});