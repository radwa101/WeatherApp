var SelfService = SelfService || {};
SelfService.TrackClaim = SelfService.TrackClaim || {};

$.extend(SelfService.TrackClaim, (function () {
    var filesToUpload = [];
    var sectionIdentifier = "file-container";

    var init = function () {
        bindFileUploadChange();
        bindFileRemove();
        //bindFileSubmit();
        bindSubmitFiles();
    }

    var bindSubmitFiles = function () {
        $('#btnSubmitFiles1').on("click", function (evt) {
            $.ajax({
                type: "GET",
                url: "/Home/GetData",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    //alert("Hello: " + response.Success);
                },
                failure: function (response) {
                    //alert(response.responseText);
                },
                error: function (response) {
                    //alert(response.responseText);
                }
            });
        });
    }

    var bindFileUploadChange = function () {
        var fileIdCounter = 0;
        $("form").change(function (evt) {
            var output = [];

            for (var i = 0; i < evt.target.files.length; i++) {
                fileIdCounter++;
                var file = evt.target.files[i];
                var fileId = sectionIdentifier + fileIdCounter;

                filesToUpload.push({
                    id: fileId,
                    file: file
                });

                var removeLink = "<span class=\"fa fa-remove removeFile\" data-fileid=\"" + fileId + "\">";
                output.push("<li style='list-style:none;'><strong>", escape(file.name), "</strong> - ", Axa.Common.FormatBytes(file.size), " bytes.", removeLink, "</li> ");
            };

            $(".fileList").append(output.join(""));
            displayFileCount();
           // evt.target.value = null;
        });
    }

    var bindFileRemove = function () {
        $("form").on("click", ".removeFile", function (evt) {
            evt.preventDefault();
            var fileId = $(this).parent().children("span").data("fileid");

            for (var i = 0; i < filesToUpload.length; ++i) {
                if (filesToUpload[i].id === fileId) {
                    filesToUpload.splice(i, 1);
                    displayFileCount();
                }
            }
            //var files = [
            //    new File([], 'sample1.txt'),
            //    new File([], 'sample2.txt')
            //];
            $('#btnSelectedFiles')[0].files = new FileListItem(filesToUpload);
            $(this).parent().remove();
        });
    }

    var bindFileSubmit = function () {
        $("form").submit(function (e) {
            //e.preventDefault();

            //var formData = new FormData();
            //for (var i = 0, len = filesToUpload.length; i < len; i++) {
            //    formData.append("files", filesToUpload[i].file);
            //}
            //formData.append("Name", $('#Name').val());

            $('#Name').val('Mary Dowling');

            var files = [
                new File([], 'sample1.txt'),
                new File([], 'sample2.txt')
            ];
            $('#btnSelectedFiles')[0].files = new FileListItem(files);

            //$.ajax({
            //    url: $('form').attr('action'),
            //    data: formData,
            //    processData: false,
            //    contentType: false,
            //    type: "POST",
            //    success: function (data) {
            //        alert("DONE");
            //        clear();
            //        displayFileCount();
            //    },
            //    error: function (data) {
            //        alert("ERROR - " + data.responseText);
            //    }
            //});
        });
    }

    function FileListItem(files) {
        //a = [].slice.call(Array.isArray(a) ? a : arguments)
        //for (var c, b = c = a.length, d = !0; b-- && d;) d = a[b] instanceof File
        //if (!d) throw new TypeError("expected argument to FileList is File or array of File objects")
        //for (b = (new ClipboardEvent("")).clipboardData || new DataTransfer; c--;) b.items.add(a[c])

        //var b = new DataTransfer();
        var b = (new ClipboardEvent("").clipboardData);
        for (var i = 0; i < files; i++) {
            b.items.add(files[i]);
        }

        //window.clipboardData.setData("Text", files); 

        return b.files
    }

    var clear = function () {
        $(".fileList").empty();
        filesToUpload = [];
        displayFileCount();
    }

    var displayFileCount = function () {
        if (filesToUpload.length > 1) {
            $('form label').text(filesToUpload.length + " files selected");
        }
        else if (filesToUpload.length > 0) {
            $('form label').text(filesToUpload.length + " file selected");
        }
        else {
            $('form label').text("No files selected");
        }
    }
    3.

    return {
        Init: init
    }
}()));

$(document).ready(function () {
    SelfService.TrackClaim.Init();
});
