// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function initDateTimePicker(id) {
    var picker = $(id);
            
    picker.datetimepicker();
        
    // programmatically clearing of default model value for aestetics
    if (picker.val() == "0001.01.01 00:00") picker.val("");
};
