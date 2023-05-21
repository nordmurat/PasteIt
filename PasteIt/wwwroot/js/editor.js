$(document).ready(function () {
    var selectedSyntax = $('form #Syntax').val();
    EditorUpdate(selectedSyntax, 'xcode');

    $('form #Syntax').on("input", function() {
        var selectedSyntax = $('form #Syntax').val();
        EditorUpdate(selectedSyntax, 'xcode');
    });

    $("#submitFormBtn").click(function() {
        SubmitForm();
    });

    ReadFile();
});

function EditorUpdate(syntax, theme)
{
    var editor = ace.edit("editor");
    editor.setTheme("ace/theme/"+theme);
    editor.session.setMode("ace/mode/"+syntax);
}

function SubmitForm()
{
    $('form #editorContent').val(ace.edit('editor').getValue());
    $('#pasteForm').submit();
}

function ReadFile()
{
    let fileInput = document.getElementById('fileInput');
    fileInput.addEventListener('change', function() {
        let reader = new FileReader();
        reader.readAsText(fileInput.files[0]);
        reader.onload = function() {
            ace.edit('editor').setValue(reader.result)
            var extension = fileInput.files[0].name.split('.').pop();;
            var datalistOptions = document.getElementById('SyntaxOptions').options;
            for(var i = 0; i < datalistOptions.length; i++) {
                if(datalistOptions[i].dataset.ext === extension) {
                    document.getElementById('Syntax').value = datalistOptions[i].value;
                    EditorUpdate(datalistOptions[i].dataset.ext, "xcode");
                    break;
                }
            }
        };
    });
}
