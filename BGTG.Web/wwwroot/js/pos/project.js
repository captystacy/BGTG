$(document).ready(function () {
    const objectCipher = $('#pos #construction-object-cipher');
    const objectName = $('#project #object-name');
    const projectTemplate = $('#project #project-template');
    const chiefProjectEngineer = $('#project #chief-project-engineer');
    const designerEngineer = $('#project #designer-engineer');
    const householdTownIncluded = $('#project #household-town-included');

    const titlePageSpinner = $('#project #title-page-download-btn #spinner');
    const tableOfContentsSpinner = $('#project #table-of-contents-download-btn #spinner');
    const projectSpinner = $('#project #project-download-btn #spinner');

    $('#project #title-page-download-btn').click(function () {
        titlePageSpinner.addClass(dInlineBlockClass);
        let titlePageViewModel = {
            ObjectCipher: objectCipher.val(),
            ObjectName: objectName.val(),
            ChiefProjectEngineer: chiefProjectEngineer.val()
        };

        $.post('api/title-pages/write', titlePageViewModel, function (operation) {
            if (!operation.ok) {
                appendAlerts(operation);
                titlePageSpinner.removeClass(dInlineBlockClass);
                return;
            }

            $('#project #title-page-download-submit-btn').click();
            titlePageSpinner.removeClass(dInlineBlockClass);
        });
    });

    $('#project #table-of-contents-download-btn').click(function () {
        tableOfContentsSpinner.addClass(dInlineBlockClass);
        let tableOfContentsViewModel = {
            ObjectCipher: objectCipher.val(),
            ChiefProjectEngineer: chiefProjectEngineer.val()
        };

        $.post('api/table-of-contents/write', tableOfContentsViewModel, function (operation) {
            if (!operation.ok) {
                appendAlerts(operation);
                tableOfContentsSpinner.removeClass(dInlineBlockClass);
                return;
            }

            $('#project #table-of-contents-download-submit-btn').click();
            tableOfContentsSpinner.removeClass(dInlineBlockClass);
        });
    });

    $('#project #project-download-btn').click(function () {
        projectSpinner.addClass(dInlineBlockClass);
        let projectViewModel = {
            ObjectCipher: objectCipher.val(),
            ProjectTemplate: projectTemplate.val(),
            ChiefProjectEngineer: chiefProjectEngineer.val(),
            DesignerEngineer: designerEngineer.val(),
            HouseholdTownIncluded: householdTownIncluded.prop('checked')
        };

        $.post('api/projects/write', projectViewModel, function (operation) {
            if (!operation.ok) {
                appendAlerts(operation);
                projectSpinner.removeClass(dInlineBlockClass);
                return;
            }

            $('#project #project-download-submit-btn').click();
            projectSpinner.removeClass(dInlineBlockClass);
        });
    });
});