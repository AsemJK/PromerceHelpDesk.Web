

$(function () {
    $('#btnLogin').on('click', Login);
})


function Login() {
    console.log(apiBase);
    var loginObject = {
        username: $('#txtUserName').val(),
        password: $('#txtPassword').val(),
        tenant: $('#txtTenant').val()
    }
    $.ajax({
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        url: `/api/user/login`,
        type: 'POST',
        data: JSON.stringify(loginObject),
        dataType: 'json',
        success: function (response) {
            console.log(response);
            if (response.status) {
                new Noty({
                    timeout: 1000,
                    type: 'success',
                    layout: 'topRight',
                    text: response.result
                }).show();
                setTimeout(() => {
                    window.location.href = '/';
                },1000);
                
            }
            else {
                new Noty({
                    timeout:1000,
                    type: 'error',
                    layout: 'topRight',
                    text: response.result
                }).show();
            }
        }
    });
}