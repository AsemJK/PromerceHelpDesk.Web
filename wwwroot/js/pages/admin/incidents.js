$(function () {
    FillIncidents();
    $('#loadAll').on('change', FillIncidents);
})
function FillIncidents() {
    $.ajax({
        url: `/api/incident/list`,
        dataType: 'json',
        success: function (data) {
            if (data.length > 0) {
                var loadAllIncidents = true;
                loadAllIncidents = $('#loadAll').is(':checked');
                if (!loadAllIncidents) {
                    data = data.filter(v => v.lastStatus !== 'Done');
                };
                if ($.fn.DataTable.isDataTable("#incidents")) {
                    $('#incidents').DataTable().clear().destroy();
                };
                $('#incidents').DataTable({
                    data: data,
                    columns: [
                        { data: 'id' },
                        { data: 'tenantName' },
                        { data: 'subject' },
                        { data: 'lastStatus' },
                        {
                            data: 'id',
                            render: function (data, full) { return `<a href="#" onclick='Details(${data})'><i class='fa fa-eye fs-5 mx-3'></i></a><a href="#" onclick='Reply(${data})'><i class='fa fa-reply fs-5 mx-3'></i></a><a href="#" onclick='CloseIncident(${data})'><i class='fa fa-times text-danger mx-3 fs-5'></i></a><a href="#" onclick='Replies(${data})'><i class='fa fa-inbox text-warning mx-3 fs-5'></i></a>`; }
                        }
                    ]
                });
            }
            else {
                new Noty({
                    timeout: 2000,
                    type: 'error',
                    layout: 'topRight',
                    text: localizedStrings.noIncidents
                }).show();
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            new Noty({
                timeout: 2000,
                type: 'error',
                layout: 'topRight',
                text: 'Failed to load incidents: ' + errorThrown
            }).show();
        }
    });
}

function Reply(incident_id) {
    $('#hfIncidentId').val(incident_id);
    $('#incidentReplyModal').modal('show');

}

function CloseIncident(id) {
    if (ConfirmAction('Close')) {
        $.ajax({
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            url: `/api/incident/close/${id}`,
            type: "POST",
            data: null,
            dataType: "json",
            success: function (response) {
                new Noty({
                    timeout: 2000,
                    type: 'success',
                    layout: 'topRight',
                    text: localizedStrings.ok
                }).show();
                window.location.href = '/Admin/Incidents';
            },
            failure: function (response) {
                new Noty({
                    timeout: 2000,
                    type: 'error',
                    layout: 'topRight',
                    text: localizedStrings.error + '(' + response + ')'
                }).show();
            },
            error: function (response) {
                new Noty({
                    timeout: 2000,
                    type: 'error',
                    layout: 'topRight',
                    text: localizedStrings.error + '(' + response + ')'
                }).show();
            }
        });
    }

}

function CloseModal() {
    $('#incidentReplyModal').modal('hide');
    $('#incidentRepliesModal').modal('hide');
}

function Update(incident_id) {
    var incidentid = $('#hfIncidentId').val();

}

function PostResolution() {
    var id = $('#hfIncidentId').val();
    var obj = {
        IncidentId: id,
        ReplyBody: $('#txtResolution').val(),
        UserId: '-'
    }
    $.ajax({
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        url: `/api/incident/Resolve`,
        type: "POST",
        data: JSON.stringify(obj),
        dataType: "json",
        success: function (response) {
            new Noty({
                timeout: 2000,
                type: 'success',
                layout: 'topRight',
                text: localizedStrings.ok
            }).show();
            window.location.href = '/Admin/Incidents';
        },
        failure: function (response) {
            new Noty({
                timeout: 2000,
                type: 'error',
                layout: 'topRight',
                text: localizedStrings.error + '(' + response + ')'
            }).show();
        },
        error: function (response) {
            new Noty({
                timeout: 2000,
                type: 'error',
                layout: 'topRight',
                text: localizedStrings.error + '(' + response + ')'
            }).show();
        }
    });

}

function Replies(incidentId) {
    $('#incidentRepliesModal').modal('show');
    $(`.incidentRepliesModalBody`).html('')
    $.ajax({
        url: `/api/incident/resolutions?id=${incidentId}`,
        dataType: 'json',
        async: false,
        success: function (data) {
            var cardRepHtml = '';
            if (data) {
                $.each(data, (index, key) => {
                    cardRepHtml += `<div class="row">`;
                    cardRepHtml += `    <div class='col-md-6'>${formatedTimestamp(key.creationTime)}</div>`;
                    cardRepHtml += `    <div class='col-md-6'>${key.replyBody}</div>`;
                    cardRepHtml += `</div>`;
                });
            }
            $(`.incidentRepliesModalBody`).html(cardRepHtml);
        }
    });
}

function Details(id) {
    window.location.href = `/Admin/Incidents/Details?id=${id}`;
}