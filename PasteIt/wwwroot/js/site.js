function createFolder() {
    var form = $('#createFolderForm');
    $.ajax({
        type: 'POST',
        url: 'https://localhost:7096/Folder/CreateFolder',
        data: form.serialize(),
        success: function (data) {
            if (data === "200") {
                alert("OK");
                getFolders();
            }
            else {
                alert("Data Error!");
            }
        },
        error: function (data) {
            alert("Error! " + data);
        }
    });
}

function getFolders() {
    $.ajax({
        type: 'GET',
        url: 'https://localhost:7096/Folder/GetFolders',
        success: function (res) {
            const folderSelect = $('#folderSelect');
            folderSelect.empty();
            var list = JSON.parse(res.responseMessage);
            $.each(list, function (index, value) {
                folderSelect.append("<option value=\"" + value.Id +"\">" + value.Name + "</option>");
            });
        },
        error: function(err) {
            alert("Error! " + err);
        }
    });
}

$(document).ready(function () {
    getFolders();
});