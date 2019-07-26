$(document).ready(function () {
    //Date Picker
    $("#txtCalender").datepicker({
    });
    $("#txtFromDate").datepicker({
    });
    $("#txtToDate").datepicker({
    });
    $("#btnSearchByDate").click(function () {
        $('#tableReport').DataTable().destroy();
        var obj = {};
        obj.fromDate = $("#txtFromDate").val();
        obj.toDate = $("#txtToDate").val();
        //Ajax Call to pass fromDate and toDate and return the list
        //$("#tableReport").DataTable().fnDestroy();
        $.ajax({
            type: "POST",
            url: '/Report/GetReportByDate',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(obj),
            dataType: "json",
            success: function (data) {
                $('#tableReport').DataTable({
                    columns: [
                        { 'data': 'reportID' },
                        { 'data': 'reportType' },
                        { 'data': 'hospital' },
                        { 'data': 'doctor' },
                        { 'data': 'date' },
                        {
                            "data": "upload", "name": "Uploaded Files",
                            fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                                $(nTd).html("<button class=\"btn btn-primary\"> <a style=\"color:white\" href='Proxy.aspx//?Imagepath=" + oData.upload + "' target='_blank'>View</a></button>");
                            }
                        },
                        { "render": function () { return '<input type="button" class="btn btn-primary downloadButtonClicked" name="downloadButton" value="Download">'; } },
                        { "render": function () { return '<input type="button" class="btn btn-danger deleteButtonClicked" name="deleteButton" value="Delete">'; } },
                    ]
                    //"bDestroy": true
                });
            },
            error: function (data) {
                alert("Error");
            }
        });
    });


    $.ajax({
        type: "POST",
        url: '/Report/GetReportDetails',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            $('#tableReport').DataTable({
                data: data.data,
                columns: [
                    { 'data': 'reportID' },
                    { 'data': 'reportType' },
                    { 'data': 'hospital' },
                    { 'data': 'doctor' },
                    { 'data': 'date' },
                    {
                        "data": "upload", "name": "Uploaded Files",
                        fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                            $(nTd).html("<button class=\"btn btn-primary\"> <a style=\"color:white\" href='http://myportal.com" + "//ProfilePicture//ChangeProfile//" + "?Imagepath=" + oData.upload +""+ "' target='_blank'>View</a></button>");
                        }
                    },
                    {
                        "data": "upload", "name": "Download",
                        fnCreatedCell: function (nTd, sData, oData, iRow, iCol) {
                            $(nTd).html("<button class=\"btn btn-primary\"> <a style=\"color:white\" href='http://myportal.com" + "//ProfilePicture//ChangeProfile//" + "?Imagepath=" + oData.upload + "" + "' download >Download</a></button>");
                        }
                    }
                    ,
                    { "render": function () { return '<input type="button" class="btn btn-danger deleteButtonClicked" name="deleteButton" value="Delete">'; } },
                ]
            });
        },
        error: function (data) {
            alert("Error");
        }
    });

});

