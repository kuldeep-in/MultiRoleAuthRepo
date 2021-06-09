// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var countdown;
function countdown_trigger(countdown_number) {

    if (countdown_number > 0) {
        countdown_number--;
        document.getElementById('countdown_text').innerHTML = countdown_number;
        if (countdown_number > 0) {
            countdown = setTimeout('countdown_trigger(' + countdown_number + ')', 1000);
        }
        else {
            location.reload();
        }
    }
}

function countdown_clear() {
    clearTimeout(countdown);
}