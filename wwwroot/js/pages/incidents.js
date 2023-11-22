let lang = $('#hfculture').val();
$(function () {
    FillIncidents('');
})

function FillIncidents(keysearch) {
    $('#incidents').html('');
    var statusImageName = 'in-progress';

    $.ajax({
        url: `/api/incident/list?q=${keysearch}`,
        dataType: 'json',
        success: function (data) {
            var loadAllIncidents = true;
            loadAllIncidents = $('#loadAll').is(':checked');
            if (!loadAllIncidents) {
                data = data.filter(v => v.lastStatus !== 'Done');
            };

            var cardHtml = '';
            if (data.length > 0) {
                cardHtml = '';
                $.each(data, (index, key) => {
                    switch (key.lastStatus) {
                        case 'New':
                            statusImageName = 'in-progress';
                            break;
                        case 'Done':
                            statusImageName = 'completed';
                            break;
                        case 'Pending':
                            statusImageName = 'in-hold';
                            break;
                        case 'Canceled':
                            statusImageName = 'rejected';
                            break;
                        default:
                            statusImageName = 'in-progress';
                            break;
                    }
                    var formattedDate = moment(key.creationTime).format('LLL');
                    var incidentStatus = key.lastStatus;
                    var localizedStatus = incidentStatus == 'Done' ? localizedStrings.done : localizedStrings.onProgress;
                    cardHtml += `<div class='card mb-2 border border-secondary'>`;
                    cardHtml += `   <div class="card-header" style='border-top: solid 10px;border-top-color: ${priorityColor[key.priority]};'>`;
                    cardHtml += `       <b class='fs-2'># ${key.id} </b> <span class='mx-4'>${formattedDate}</span>`;
                    cardHtml += `       <img width='25' src='/images/${statusImageName}.png' alt='${key.lastStatus}' title='${key.lastStatus}'/>`;
                    cardHtml += `       <a href='#' class="btn btn-danger btn-sm mt-2 ${lang == 'ar' ? 'pull-left' : 'pull-right'}" onclick="ForceClose(${key.id})" title='${localizedStrings.close}'><i class='fa fa-times'></i></a>`;
                    cardHtml += `   </div>`;
                    cardHtml += `   <div class="card-body">`;
                    cardHtml += `       ${key.subject}`;
                    cardHtml += `   </div>`;
                    cardHtml += `   <div class='my-3'>`;
                    if (key.attachments.length > 0) {
                        cardHtml += `    <h5>${localizedStrings.attachments}</h5>`;
                        cardHtml += `       <div class='card'>`;
                        cardHtml += `           <div class="card-body">`;
                        cardHtml += `               <div class='row'>`;
                        $.each(key.attachments, function (indx, k) {
                            var fileExt = k.fileName.split('.')[1];
                            cardHtml += `<div class='col-md-1'>`;
                            cardHtml += `<a href='#' onclick='EnlargFile("files/${k.fileName}")'>`;
                            if (fileExt == 'pdf') {
                                cardHtml += `<object data="files/${k.fileName}" type="application/pdf" width="auto" height="auto">`;
                                cardHtml += `     <embed src="files/${k.fileName}" type="application/pdf">`;
                                cardHtml += ` </object>`;
                            }
                            else if (fileExt === 'png' || fileExt === 'jpeg' || fileExt === 'jpg') {
                                cardHtml += `<img id='thumb_att_img' src="files/${k.fileName}" height="100%" style='max-width: max-content;' width="100%" title="description" />`;
                            }
                            else {
                                cardHtml += `<span class='mx-2'></span>
                                            <iframe id='thumb_iframes' src="files/${k.fileName}?autoplay=0" sandbox height="150" width="250" title="description">
                                            </iframe>`;
                            }
                            cardHtml += `</div></a>`;
                        });
                        cardHtml += `               </div>`;
                        cardHtml += `           </div>`;
                        cardHtml += `       </div>`;
                    }
                    if (key.resolutions.length > 0) {
                        cardHtml += `       <div class="row mx-1">                        
                                                    <h5>${localizedStrings.replies}</h5>`;
                        $.each(key.resolutions, (index, ikey) => {
                            cardHtml += `           <div id="incidentReplies_${ikey.incidentId}" class="row">`;
                            cardHtml += `               <div class ='col-md-6'> ${formatedTimestamp(ikey.creationTime)} </div>`;
                            cardHtml += `               <div class ='col-md-6'> ${ikey.replyBody} </div>`;
                            if (ikey.attachments.length > 0) {
                                ikey.attachments = ikey.attachments.filter(v => v.source === 'Resolution');
                                cardHtml += `<h5>${localizedStrings.promerce_attachments}</h5>`;
                                cardHtml += `   <div class='row'>`;
                                $.each(ikey.attachments, function (indx, jkey) {
                                    cardHtml += `       <div class='col-12 mx-1'><a href='#' onclick='EnlargFile("files/${jkey.fileName}")'><img id='thumb_att_img' src="files/${jkey.fileName}" height="75px" style='max-width: max-content;' width="auto" title="${ikey.replyBody}" /></a></div>`;
                                });
                                cardHtml += `   </div>`;
                            }
                            cardHtml += `           </div>`;
                        })
                        cardHtml += `       </div>`;
                    }
                    cardHtml += `   </div>`;
                    cardHtml += `</div>`;


                });

                $('#incidents').append(cardHtml);
            }
            else {
                new Noty({
                    timeout: 2000,
                    type: 'error',
                    layout: 'topRight',
                    text: localizedStrings.noIncidents
                }).show();
            }
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
function ForceClose(id) {
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
    }
}