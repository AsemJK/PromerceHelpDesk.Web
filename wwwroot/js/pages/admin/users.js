
$(function () {
    FillTenants();
    FillUsers();
    $('#btnSave').on('click', Register);
})
function FillTenants() {
    $.ajax({
        url: `/api/tenant/list`,
        dataType: 'json',
        success: function (data) {
            var optionsHtml = '';
            if (data.length > 0) {
                $.each(data, function (index,key) {
                    $('#TenantDdl').append(`<option value='${key.tenantCode}'>${key.tenantName}</option>`);
                });
            }
        }
    });
}
function Register() {

    var registerObject = {
        "userName": $('#UserName').val(),
        "email": $('#UserEmail').val(),
        "password": $('#UserPassword').val(),
        "tenantCode": $('#TenantDdl').val(),
        "role": $('#UserRole').val(),
    };
    $.ajax({
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        url: `/api/user/register`,
        type: 'POST',
        data: JSON.stringify(registerObject),
        dataType: 'json',
        success: function (response) {
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
                    text: localizedStrings.error + '(' + response.loginResult + ')',
                    theme: 'metroui'
                }).show();
            }
        }
    });
}

function FillUsers() {
    $.ajax({
        url: `/api/user/list`,
        dataType: 'json',
        success: function (data) {
            if (data.length > 0) {
                $('#usersTable').DataTable({
                    data: data,
                    columns: [
                        {
                            data: 'id',
                            className: 'hidden-column'
                        },
                        { data: 'userName' },
                        { data: 'tenantName' },
                        {
                            data: 'roles',
                            render: function (data) {
                                return `<table><tr>
                                ${$.each(data, function (index,key) {
                                    return `<td>${key}</td>`;
                                })}</tr></table>
                                `;
                            }
                        },
                        {
                            data: 'id',
                            render: function (data, full) { return `<a href="#" onclick='DeactivateUser(${data})' title='Deactivate'><i class='fa fa-eye-slash fs-5 mx-3'></i>`; }
                        }
                    ]
                });
            }
        }
    });
} function DeactivateUser(id) {

}