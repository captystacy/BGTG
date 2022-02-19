$(document).ready(function () {
    $('#construction-objects #calendar-plan-download-btn').click(function () {
        let spinner = $(this).find('#spinner');
        spinner.addClass(dInlineBlockClass);

        let posItem = $(this).parents('.pos-item');
        let calendarPlanId = posItem.find('#calendar-plan-id').val();

        $.post(`calendarplans/WriteById?id=${calendarPlanId}`, function (operation) {
            if (!operation.ok) {
                appendValidationAlert(operation);
                spinner.removeClass(dInlineBlockClass);
                return;
            }

            posItem.find('#calendar-plan-download-submit-btn').click();
            spinner.removeClass(dInlineBlockClass);
        });
    });

    $('#construction-objects #calendar-plan-delete-btn').click(function () {
        let spinner = $(this).find('#spinner');
        spinner.addClass(dInlineBlockClass);

        let posItem = $(this).parents('.pos-item');
        let calendarPlanId = posItem.find('#calendar-plan-id').val();
        $.ajax({
            url: `calendarplans/Delete/?id=${calendarPlanId}`,
            type: 'DELETE',
            success: function (operation) {
                if (!operation.ok) {
                    appendValidationAlert(operation);
                    spinner.removeClass(dInlineBlockClass);
                    return;
                }

                spinner.removeClass(dInlineBlockClass);
                posItem.remove();
            }
        });
    });

    $('#construction-objects #duration-by-lc-download-btn').click(function () {
        let spinner = $(this).find('#spinner');
        spinner.addClass(dInlineBlockClass);

        let posItem = $(this).parents('.pos-item');
        let durationByLCId = posItem.find('#duration-by-lc-id').val();

        $.post(`DurationByLCs/WriteById?id=${durationByLCId}`, function (operation) {
            if (!operation.ok) {
                appendValidationAlert(operation);
                spinner.removeClass(dInlineBlockClass);
                return;
            }

            posItem.find('#duration-by-lc-download-submit-btn').click();
            spinner.removeClass(dInlineBlockClass);
        });
    });

    $('#construction-objects #duration-by-lc-delete-btn').click(function () {
        let spinner = $(this).find('#spinner');
        spinner.addClass(dInlineBlockClass);

        let posItem = $(this).parents('.pos-item');
        let durationByLCId = posItem.find('#duration-by-lc-id').val();
        $.ajax({
            url: `DurationByLCs/Delete?id=${durationByLCId}`,
            type: 'DELETE',
            success: function (operation) {
                if (!operation.ok) {
                    appendValidationAlert(operation);
                    spinner.removeClass(dInlineBlockClass);
                    return;
                }

                spinner.removeClass(dInlineBlockClass);
                posItem.remove();
            }
        });
    });

    $('#construction-objects #duration-by-tcp-download-btn').click(function () {
        let spinner = $(this).find('#spinner');
        spinner.addClass(dInlineBlockClass);

        let posItem = $(this).parents('.pos-item');
        let durationByTCPId = posItem.find('#duration-by-tcp-id').val();

        $.post(`DurationByTCPs/WriteById?id=${durationByTCPId}`, function (operation) {
            if (!operation.ok) {
                appendValidationAlert(operation);
                spinner.removeClass(dInlineBlockClass);
                return;
            }

            posItem.find('#duration-by-tcp-download-submit-btn').click();
            spinner.removeClass(dInlineBlockClass);
        });
    });

    $('#construction-objects #duration-by-tcp-delete-btn').click(function () {
        let spinner = $(this).find('#spinner');
        spinner.addClass(dInlineBlockClass);

        let posItem = $(this).parents('.pos-item');
        let durationByTCPId = posItem.find('#duration-by-tcp-id').val();
        let calculationType = posItem.find('#duration-by-tcp-calculation-type').val();

        let action;
        if (calculationType.includes("Interpolation")) {
            action = "DeleteInterpolation";
        } else if (calculationType.includes("StepwiseExtrapolation")) {
            action = "DeleteStepwiseExtrapolation";

        } else if (calculationType.includes("Extrapolation")) {
            action = "DeleteExtrapolation";
        } else if (calculationType.includes("StepwiseExtrapolation")) {
            action = "DeleteStepwiseExtrapolation";
        } else {
            return;
        }

        $.ajax({
            url: `DurationByTCPs/${action}?id=${durationByTCPId}`,
            type: 'DELETE',
            success: function (operation) {
                if (!operation.ok) {
                    appendValidationAlert(operation);
                    spinner.removeClass(dInlineBlockClass);
                    return;
                }

                spinner.removeClass(dInlineBlockClass);
                posItem.remove();
            }
        });
    });

    $('#construction-objects #energy-and-water-download-btn').click(function () {
        let spinner = $(this).find('#spinner');
        spinner.addClass(dInlineBlockClass);

        let posItem = $(this).parents('.pos-item');
        let energyAndWaterId = posItem.find('#energy-and-water-id').val();

        $.post(`energyandwaters/WriteById?id=${energyAndWaterId}`, function (operation) {
            if (!operation.ok) {
                appendValidationAlert(operation);
                spinner.removeClass(dInlineBlockClass);
                return;
            }

            posItem.find('#energy-and-water-download-submit-btn').click();
            spinner.removeClass(dInlineBlockClass);
        });
    });

    $('#construction-objects #energy-and-water-delete-btn').click(function () {
        let spinner = $(this).find('#spinner');
        spinner.addClass(dInlineBlockClass);

        let posItem = $(this).parents('.pos-item');
        let energyAndWaterId = posItem.find('#energy-and-water-id').val();
        $.ajax({
            url: `energyandwaters/Delete?id=${energyAndWaterId}`,
            type: 'DELETE',
            success: function (operation) {
                if (!operation.ok) {
                    appendValidationAlert(operation);
                    spinner.removeClass(dInlineBlockClass);
                    return;
                }

                spinner.removeClass(dInlineBlockClass);
                posItem.remove();
            }
        });
    });
});