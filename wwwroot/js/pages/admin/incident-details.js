$(function () {
    tinyMCE.init({
        selector: "#txt_subject",
        menubar: false,
        fontsize_formats: "14pt 18pt 24pt 36pt",
    });
})

function AddReply(incident_id) {
    $('#hfIncidentId').val(incident_id);
    $('#incidentReplyModal').modal('show');
}
function CloseModal() {
    $('#incidentReplyModal').modal('hide');
    $('#incidentRepliesModal').modal('hide');
}
function PostResolution() {
    var inc_id = $('#hfIncidentId').val();
    $('#btnSaveResolution').prop('disabled', true);
    //first add attachments :
    var formData = new FormData();
    formData.append('source', 'Resolution');
    formData.append('incident_id', inc_id);
    var fileInput = $(`#fleAttachments`)[0];
    for (var x = 0; x < fileInput.files.length; x++) {
        formData.append('files', fileInput.files.item(x));
    }
    $.ajax({
        url: '/api/incident/upload',
        data: formData,
        type: 'POST',
        cache: false,
        contentType: false,
        processData: false,
        success: function (res) {
            var signature = 'no-attachments';
            if (res !== 'no-attachments') {
                var filesJsonList = JSON.parse(res);
                signature = filesJsonList[0].uploadSignature;
            }
            
            var obj = {
                IncidentId: inc_id,
                ReplyBody: $('#txtResolution').val(),
                UserId: '-',
                UserName: 'NA'
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
        }, complete: function () {
            $('#btnSave').prop('disabled', false);
        }
    });
}

function EnlargFile(path) {
    $('#pdfIframe').attr('src', path);
    $('#pdfPreviewModal').css('z-index', 999999999);
    $('#pdfPreviewModal').modal('show');
}
function CloseModal() {
    $('#pdfPreviewModal').modal('hide');
}