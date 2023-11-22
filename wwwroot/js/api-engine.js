function api_post_ajax(url, jsonobj) {
    $.ajax({
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        url: url,
        type: "POST",
        data: jsonobj,
        dataType: "json",
        success: function (response) {
            abp.notify.success('Done');
        },
        failure: function (response) {
            abp.notify.error(response.responseText);
        },
        error: function (response) {
            if (response.status != 200)
                abp.notify.error(response.responseText);
        }
    });
}
function api_post_file_ajax(url, formData) {
    $.ajax({
        url: url,
        type: 'POST',
        async: false,
        data: formData,
        processData: false,  // tell jQuery not to process the data
        contentType: false,  // tell jQuery not to set contentType
        done: function (result) {
            return result;
        },
        error: function (jqXHR) {
            abp.notify.error(jqXHR.responseText);
        },
        complete: function (jqXHR, status) {
        }
    });
}
function api_delete_ajax(url, jsonobj) {
    $.ajax({
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        url: url,
        type: "DELETE",
        data: jsonobj,
        dataType: "json",
        success: function (response) {
            abp.notify.success('Done');
        },
        failure: function (response) {
            abp.notify.error(response.responseText);
        },
        error: function (response) {
            if (response.status != 200)
                abp.notify.error(response.responseText);
        }
    });
}

async function getapi(url) {
    var data;
    try {
        const response = await fetch(url);
        data = await response.json();
    } catch (e) {
        new Noty({
            timeout: 2000,
            type: 'error',
            layout: 'topRight',
            text: e
        }).show();
    }
    return data;
}

async function postapi(url, jsonobj) {
    fetch(url, {
        method: 'POST',
        body: jsonobj,
        headers: {
            'Content-type': 'application/json; charset=UTF-8',
        }
    })
        .then(function (response) {
            return response.json()
        })
        .then(function (data) {
        }).catch(error => console.error('Error:', error));
}
async function deleteapi(url, jsonobj) {
    fetch(url, {
        method: 'DELETE',
        body: jsonobj,
        headers: {
            'Content-type': 'application/json; charset=UTF-8',
        }
    })
        .then(function (response) {
            return response.json()
        })
        .then(function (data) {
        }).catch(error => console.error('Error:', error));
}

function api_put_ajax(url, jsonobj) {
    $.ajax({
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        url: url,
        type: "PUT",
        data: jsonobj,
        dataType: "json",
        success: function (response) {
            abp.notify.success('Done');
        },
        failure: function (response) {
            abp.notify.error(response.responseText);
        },
        error: function (response) {
            if (response.status != 200)
                abp.notify.error(response.responseText);
        }
    });
}