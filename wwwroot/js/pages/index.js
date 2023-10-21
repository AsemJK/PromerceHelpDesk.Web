$(function () {

})


function Logout() {
    localStorage.removeItem('user');
    $.ajax({
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        url: `/Logout`,
        type: 'GET',
        data: null,
        dataType: 'json',
        success: function (response) {
        }
    });
}