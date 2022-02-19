$(document).ready(function () {
    $('#history #calendar-plan-download-btn').click(function () {
        let spinner = $(this).find('#spinner');
        spinner.addClass(dInlineBlockClass);

        let historyItem = $(this).parents('.history-item');
        let calendarPlanId = historyItem.find('#calendar-plan-id').val();

        $.post(`calendarplans/writebyid?id=${calendarPlanId}`, function (operation) {
            if (!operation.ok) {
                appendValidationAlert(operation);
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
            url: `calendarplans/delete/?id=${calendarPlanId}`,
            type: 'DELETE',
            success: function (operation) {
                if (!operation.ok) {
                    appendValidationAlert(operation);
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

        $.post(`durationbylcs/writebyid?id=${durationByLCId}`, function (operation) {
            if (!operation.ok) {
                appendValidationAlert(operation);
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
            url: `durationbylcs/delete?id=${durationByLCId}`,
            type: 'DELETE',
            success: function (operation) {
                if (!operation.ok) {
                    appendValidationAlert(operation);
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

        $.post(`durationbytcps/writebyid?id=${durationByTCPId}`, function (operation) {
            if (!operation.ok) {
                appendValidationAlert(operation);
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
            url: `durationbytcps/delete?id=${durationByTCPId}`,
            type: 'DELETE',
            success: function (operation) {
                if (!operation.ok) {
                    appendValidationAlert(operation);
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

        $.post(`energyandwaters/writebyid?id=${energyAndWaterId}`, function (operation) {
            if (!operation.ok) {
                appendValidationAlert(operation);
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
            url: `energyandwaters/delete?id=${energyAndWaterId}`,
            type: 'DELETE',
            success: function (operation) {
                if (!operation.ok) {
                    appendValidationAlert(operation);
                    spinner.removeClass(dInlineBlockClass);
                    return;
                }

                spinner.removeClass(dInlineBlockClass);
                historyItem.remove();
            }
        });
    });
});