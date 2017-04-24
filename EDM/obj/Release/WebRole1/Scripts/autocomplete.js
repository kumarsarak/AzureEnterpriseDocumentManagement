/// <reference path="jquery-1.6.2-vsdoc.js" />
/// <reference path="jquery-ui.js" />
$(document).ready(function () {
    $('*[data-autocomplete-url]')
        .each(function () {
            $(this).autocomplete({
                source: $(this).data("autocomplete-url")
            });
        });
});