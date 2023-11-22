$(function () {
    FillTenants();
    $('#btnSave').on('click', Add);
})


function FillTenants() {
    $.ajax({
        url: `/api/tenant/list`,
        dataType: 'json',
        success: function (data) {
            console.log(data);
            var cardHtml = '';
            if (data.length > 0) {
                $('#tenantTable').dataTable({
                    data: data,
                    columns: [
                        { data: 'tenantName' },
                        { data: 'tenantCode' },
                        {
                            data: 'updateTime',
                            render: function (data) {
                                const dateObj = new Date(data);
                                const formatter = new Intl.DateTimeFormat('en-US', {
                                    year: 'numeric',
                                    month: '2-digit',
                                    day: '2-digit'
                                });
                                return formatter.format(dateObj);
                            }
                        },
                        {
                            data: 'id',
                            render: function (data) { return `<a class='btn' href='#' onclick='DeleteTenant(${data})' ><i class='fa fa-trash text-danger fs-4'></i></a>`; }
                        }
                    ]
                });
            }
            else {
                new Noty({
                    timeout: 2000,
                    type: 'error',
                    layout: 'topRight',
                    text: localizedStrings.noTenants
                }).show();
            }
        }
    });
}

function Add() {
    var obj = {
        Id: 0,
        TenantName: $('#TenantName').val(),
        TenantCode: $('#TenantCode').val(),
    }
    if (obj.TenantName === '' || obj.TenantCode === '') {
        new Noty({
            timeout: 2000,
            type: 'error',
            layout: 'topRight',
            text: localizedStrings.required
        }).show();
    }
    else {
        $.ajax({
            url: `/api/tenant`,
            type: 'POST',
            data: JSON.stringify(obj),
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            dataType: 'json',
            success: function (succ) {
                if (succ) {
                    new Noty({
                        timeout: 2000,
                        type: 'success',
                        layout: 'topRight',
                        text: localizedStrings.ok
                    }).show();
                    window.location.href = '/Admin/Tenant';
                }
                else {
                    new Noty({
                        timeout: 2000,
                        type: 'error',
                        layout: 'topRight',
                        text: localizedStrings.error + '(' + response + ')'
                    }).show();
                }
            }
        });
    }
}

function DeleteTenant(id) {
    if (ConfirmAction('Delete')) {
        $.ajax({
            url: `/api/tenant/delete/${id}`,
            type: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            dataType: 'json',
            success: function (succ) {
                if (succ) {
                    new Noty({
                        timeout: 2000,
                        type: 'success',
                        layout: 'topRight',
                        text: localizedStrings.ok
                    }).show();
                    window.location.href = '/Admin/Tenant';
                }
                else {
                    new Noty({
                        timeout: 2000,
                        type: 'error',
                        layout: 'topRight',
                        text: localizedStrings.error + '(' + response + ')'
                    }).show();
                }
            }
        });
    }    
}