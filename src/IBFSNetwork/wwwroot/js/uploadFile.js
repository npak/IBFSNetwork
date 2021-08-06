function UploadFilesOLD() {
    var fileUpload = $("#files").get(0);
    var files = fileUpload.files;
    var data = new FormData();

    for (var i = 0; i < files.length ; i++) {
        data.append(files[i].name, files[i]);
    }
    var ui = document.getElementById("myUploadID");
    var items = ui.getElementsByTagName("li");

    if (typeof (items) != "undefined" || items != null) {
        data.append("filecnt", items.length);

        for (var i = 0; i < items.length; ++i) {
            data.append("file" + i.toString(), items[i].innerText);
        }

        for (var i = 0; i < items.length; ++i) {

            var iid = "#con_" + i.toString()
            var tt = $(iid).val();
            data.append("con" + i.toString(), tt);
            var docid = "#docid_" + i.toString()
            var dt = $(docid).val();
            data.append("docid" + i.toString(), dt);
        }
    }

    $.ajax({
        type: "POST",
        url: "/Documents/UploadFilesAjax",
        contentType: false,
        processData: false,
        data: data,
        success: function (message) {
            message = message.replace(/class="Doc_/g, 'asp-for="');
            $('#filelist').html(message);
        },
        error: function () {
            alert("There was error uploading files!");
        }
    });
};

function UploadFiles() {
    var fileUpload = $("#files").get(0);
    var files = fileUpload.files;
    var data = new FormData();

    for (var i = 0; i < files.length ; i++) {
        data.append(files[i].name, files[i]);
    }
    var ui = document.getElementById("myUploadID");
    var items = ui.getElementsByTagName("li");
    var cnt = 0;

    if (typeof (items) != "undefined" || items != null) {
        //data.append("filecnt", items.length);
        cnt = items.length;
        //for (var i = 0; i < items.length; ++i) {
        //    data.append("file" + i.toString(), items[i].innerText);
        //}

        for (var i = 0; i < items.length; ++i) {
            var ind = items[i].id.split("_").pop();
            var iid = "#con_" + ind.toString()
            var tt = $(iid).val();
            data.append("con" + i.toString(), tt);
            var docid = "#docid_" + ind.toString()
            var dt = $(docid).val();
            data.append("docid" + i.toString(), dt);

            var docname = "#docname_" + ind.toString()
            var dn = $(docname).val();
            data.append("file" + i.toString(), dn);
            //alert(ind);
            //alert(dn);

        }
    }
    var uidoc = document.getElementById("myUploadDoc");
    var itemsdoc = uidoc.getElementsByTagName("li");
    
    if (typeof (itemsdoc) != "undefined" || itemsdoc != null) {
        //data.append("filecnt", items.length);
        var j = cnt;
        cnt = cnt + itemsdoc.length;
        //for (var i = 0; i < itemsdoc.length; ++i) {
        //    data.append("file" + (j + i).toString(), itemsdoc[i].innerText);
        //}

        for (var i = 0; i < itemsdoc.length; ++i) {
            var ind = itemsdoc[i].id.split("_").pop();
            var iid = "#con_" + ind.toString()
            var tt = $(iid).val();
            data.append("con" + (j+i).toString(), tt);
            var docid = "#docid_" + ind.toString()
            var dt = $(docid).val();
            data.append("docid" + (j + i).toString(), dt);

            var docname = "#docname_" + ind.toString()
            var dn = $(docname).val();
            data.append("file" + (j + i).toString(), dn);
        }
    }

    data.append("filecnt",cnt);
    $.ajax({
        type: "POST",
        url: "/Documents/UploadFilesAjax",
        contentType: false,
        processData: false,
        data: data,
        success: function (message) {
            message = message.replace(/class="Doc_/g, 'asp-for="');

            $('#filelist').html(message);
        },
        error: function () {
            alert("There was error uploading files!");
        }
    });
};

function UploadFilesNewCopy() {
    var fileUpload = $("#files").get(0);
    var files = fileUpload.files;
    var data = new FormData();

    for (var i = 0; i < files.length ; i++) {
        data.append(files[i].name, files[i]);
    }
    var ui = document.getElementById("myUploadID");
    var items = ui.getElementsByTagName("li");
    var cnt = 0;

    if (typeof (items) != "undefined" || items != null) {
        //data.append("filecnt", items.length);
        cnt = items.length;
        for (var i = 0; i < items.length; ++i) {
            //data.append("file" + i.toString(), items[i].innerText);
            //alert(items[i].innerText);
            //alert(items[i].id);
            //var ind = items[i].id.substr(items[i].indexOf("_") + 1);
            //alert(ind);
            data.append("file" + i.toString(), items[i].innerText);

        }

        for (var i = 0; i < items.length; ++i) {
            var ind = items[i].id.split("_").pop();
            var iid = "#con_" + ind.toString()
            var tt = $(iid).val();
            data.append("con" + i.toString(), tt);
            var docid = "#docid_" + ind.toString()
            var dt = $(docid).val();
            data.append("docid" + i.toString(), dt);
        }
    }
    var uidoc = document.getElementById("myUploadDoc");
    var itemsdoc = uidoc.getElementsByTagName("li");
    //alert(uidoc);
    //alert(itemsdoc);


    if (typeof (itemsdoc) != "undefined" || itemsdoc != null) {
        //data.append("filecnt", items.length);
        var j = cnt;
        cnt = cnt + itemsdoc.length;
        for (var i = 0; i < itemsdoc.length; ++i) {
            //data.append("file" + i.toString(), items[i].innerText);
            //alert(items[i].innerText);
            //alert(items[i].id);
            //var ind = items[i].id.substr(items[i].indexOf("_") + 1);
            //alert(ind);
            data.append("file" + (j + i).toString(), itemsdoc[i].innerText);
            alert(itemsdoc[i].innerText);
            alert((j + i).toString());

        }

        for (var i = 0; i < itemsdoc.length; ++i) {
            var ind = itemsdoc[i].id.split("_").pop();
            var iid = "#con_" + ind.toString()
            var tt = $(iid).val();
            data.append("con" + (j + i).toString(), tt);
            var docid = "#docid_" + ind.toString()
            var dt = $(docid).val();
            data.append("docid" + (j + i).toString(), dt);
        }
    }

    data.append("filecnt", cnt);
    $.ajax({
        type: "POST",
        url: "/Documents/UploadFilesAjax",
        contentType: false,
        processData: false,
        data: data,
        success: function (message) {
            message = message.replace(/class="Doc_/g, 'asp-for="');
            $('#filelist').html(message);
        },
        error: function () {
            alert("There was error uploading files!");
        }
    });
};


function SaveAlert() {

    var data = new FormData();

    var ui = document.getElementById("myUploadID");
    var items = ui.getElementsByTagName("li");

    if (typeof (items) != "undefined" || items != null) {
        data.append("filecnt", items.length);

        for (var i = 0; i < items.length; ++i) {
            data.append("file" + i.toString(), items[i].innerText);
        }
    }

    for (var i = 0; i < items.length; ++i) {
        var iid = "#con_" + i.toString()
        var tt = $(iid).val();
        data.append("con" + i.toString(), tt);
    }

    var alertid = document.getElementById("AlertId");
    data.append("AlertId", alertid.value);

    $.ajax({
        type: "POST",
        url: "/Documents/Save",
        contentType: false,
        processData: false,
        data: data,
        success: function () {
            $('#filelist').html("<span>File(s) succesfully saved.</span>");
            $("#files").replaceWith($("#files").val('').clone(true));
        },
        error: function () {
            alert("There was error uploading files!");
        }
    });
};

$(document).ready(function () {
    $("#Save099").click(function (evt) {

        var data = new FormData();

        var ui = document.getElementById("myUploadID");
        var items = ui.getElementsByTagName("li");

        if (typeof (items) != "undefined" || items != null) {
            data.append("filecnt", items.length);

            for (var i = 0; i < items.length; ++i) {
                data.append("file" + i.toString(), items[i].innerText);
            }

        }


        for (var i = 0; i < items.length; ++i) {

            var iid = "#con_" + i.toString()
            var tt = $(iid).val();

            data.append("con" + i.toString(), tt);
            //alert("con" + i.toString());
        }

        var alertid = document.getElementById("AlertId");
        data.append("AlertId", alertid.value);

        $.ajax({
            type: "POST",
            url: "/Documents/Save",
            contentType: false,
            processData: false,
            data: data,
            success: function () {
                //alert(message);
                $('#filelist').html("<span>File(s) succesfully saved.</span>");
                $("#files").replaceWith($("#files").val('').clone(true));
            },
            error: function () {
                alert("There was error uploading files!");
            }
        });
    });
});

