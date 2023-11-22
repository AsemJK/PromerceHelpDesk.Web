
function RestPassword() {
    if (ConfirmAction('Reset Pssword')) {
        var restobject = {
            userId: '-',
            currentPassword: $('#current_password').val(),
            newPassword: $('#new_password').val(),
            confirmPassword: $('#confirm_password').val()
        }
        console.log(restobject);
        $.ajax({
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            url: `/api/user/ResetPassword`,
            type: 'POST',
            dataType: 'json',
            data: JSON.stringify(restobject),
            success: function (res) {
                console.log(res);
                if (res === '"Done"') {
                    new Noty({
                        timeout: 4000,
                        type: 'success',
                        layout: 'topRight',
                        text: localizedStrings.ok
                    }).show();
                }
                else {
                    new Noty({
                        timeout: 4000,
                        type: 'error',
                        layout: 'topRight',
                        text: localizedStrings.error + '(' + res + ')'
                    }).show();
                }
            }
        });
    }


}