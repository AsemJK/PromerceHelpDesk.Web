// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


let priorityArr = [localizedStrings.low, localizedStrings.normal, localizedStrings.emergency];
let priorityColor = ['#D2D8D5', '#ff99a5', '#990012'];
function ConfirmAction(action) {
    var result = confirm(`Are you sure you want to ${action}?`);
    return result;
}

function formatedTimestamp(input) {
    const date = input.toString().split('T')[0];
    const time = input.toString().split('T')[1].split('.')[0];
    return `${date} @ ${time}`
}
