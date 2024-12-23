$(function () {
    $('#btnLogin').on('click', Login);
})

function getReturnUrlFromHref(href) {
    // Create a regular expression to match the returnUrl= parameter.
    var regex = /ReturnUrl=(.*?)(&|$)/;
    // Match the returnUrl= parameter in the href.
    var match = regex.exec(href);
    // If a match is found, return the returnUrl= parameter value.
    if (match !== null) {
        return match[1];
    }
    // If no match is found, return null.
    return '/';
}
function Login() {
    $('#btnLogin').prop('disabled', true);
    var href = window.location.href;
    // Get the returnUrl= parameter value from the href.
    var returnUrl = decodeURIComponent(getReturnUrlFromHref(href));

    var loginObject = {
        UserName: $('#txtUserName').val(),
        Password: $('#txtPassword').val(),
        TenantCode: $('#txtTenant').val(),
        ReturnUrl: returnUrl
    }
    $.ajax({
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        url: `/authentication/login`,
        type: 'POST',
        data: JSON.stringify(loginObject),
        dataType: 'json',
        success: function (response) {
            console.log(response);
            if (response.loginResult == 'Ok') {
                new Noty({
                    timeout: 1000,
                    type: 'success',
                    layout: 'topRight',
                    text: localizedStrings.ok
                }).show();
                setTimeout(() => {
                    window.location.href = returnUrl;
                }, 100);

            }
            else {
                new Noty({
                    timeout: 2000,
                    type: 'error',
                    layout: 'topRight',
                    text: localizedStrings.error + '  ( ' + response.loginResult + ' )',
                    theme: 'metroui'
                }).show();
            }
        },
        complete: function () {
            $('#btnLogin').prop('disabled', false);
        }
    });
}

function EnterPressHandler(event,target) {
    if (event.keyCode === 13) {
        event.preventDefault();
        if ($('#txtUserName').val() != '' && $('#txtPassword').val() != '')
            Login();
        else {
            if (target.name === 'UserName')
                $('#txtPassword').focus();
            else if (target.name === 'Password')
                $('#txtTenant').focus();
            else if (target.name === 'Tenant')
                Login();
        }
    }
}