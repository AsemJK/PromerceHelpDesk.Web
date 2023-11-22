let mainSysMod;
let subSysMod;
let lang = $('#hfculture').val();
$(function () {
    Reset();
    FillProjects();
})

function Reset() {
    tinyMCE.init({
        selector: "#txt_subject",
        menubar: false,
        content_style: `body { text-align: ${lang == 'ar' ? 'right' : 'left'};direction: ${lang == 'ar' ? 'rtl' : 'ltr'};background-color: #fff9ee;font-size: 2rem; }`,
        fontsize_formats: "14pt 18pt 24pt 36pt",
    });

    setTimeout(() => {
        var editor = tinymce.get('txt_subject'); // Replace 'editor' with your textarea's ID
        editor.setContent('');
    }, 400);
}
function AddIncident() {
    $('#btnSave').prop('disabled', true);
    //first add attachments :
    var formData = new FormData();
    formData.append('source', 'Issue');
    var fileInput = $(`input[type='file']`)[0];
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
            var subjectEditor = tinymce.get("txt_subject");
            var subjectContent = subjectEditor.getContent();
            var obj = {
                subject: subjectContent,
                tenantCode: '',
                userId: '',
                lastStatus: 'New',
                attachmentsGuid: signature,
                priority: $('input[name="opPriority"]:checked').val(),
                resolution: '',
                technicianId: 0,
                moduleId: $("#subSystemModules").val()
            };
            $.ajax({
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                url: '/api/incident/add',
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
                    window.location.href = '/Incidents';
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
function FillProjects() {
    $.ajax({
        url: `/api/project/list`,
        dataType: 'json',
        async: false,
        success: function (data) {
            if (data) {
                $('#ddlProject').html('');
                $.each(data, (index, key) => {
                    $('#ddlProject').append(`<option value='${key.id}'>${key.name}</>`);
                });
                FillMainSystemModules(data[0].id);
            }
        }
    });
}
function FillMainSystemModules(project_id) {
    $.ajax({
        url: `/api/systemModule/main?projectId=${project_id}`,
        dataType: 'json',
        async: false,
        success: function (data) {
            if (data) {
                mainSysMod = data.filter(v => v.parentId == 0);
                subSysMod = data.filter(v => v.parentId > 0);
                $('#mainSystemModules').html('');
                $.each(mainSysMod, (index, key) => {
                    $('#mainSystemModules').append(`<option value='${key.id}'>${key.name}</>`);
                });
                FillSubModules(mainSysMod[0].id);
            }
        }
    });
}
function FillSubModules(parent) {
    $('#subSystemModules').html('');
    var childs = subSysMod.filter(v => v.parentId == parent);
    $.each(childs, (index, key) => {
        $('#subSystemModules').append(`<option value='${key.id}'>${key.name}</>`);
    });
    SetPriority(childs[0].id);
}
function SetPriority(submoduleid) {
    var ch = subSysMod.filter(v => v.id == submoduleid);
    $('input[name="opPriority"]').prop('disabled', true);
    $(`#opPriority_${ch[0].priority}`).prop('checked',true);
}